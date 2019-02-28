using Flattiverse;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YAFBCore.Mapping.Units
{
    public class MapUnit : IEquatable<MapUnit>
    {
        internal readonly Map Map;

        public readonly string Name;
        public readonly UnitKind Kind;

        // Pretty dirty to just define variables as internal 
        // But it's faster cause of calling Properties often is slowwww

        internal int AgeInternal;

        internal Vector PositionInternal;
        internal Vector MovementInternal;

        internal float RadiusInternal;

        protected bool isOrbiting;
        protected bool isSolid;
        protected bool isMasking;
        protected Mobility mobility;

        protected float gravity;

        internal Vector OrbitingCenter;
        protected List<UnitOrbitingState> orbitingList;

        #region Properties

        public virtual int AgeMax => mobility == Mobility.Steady ? -1 : 5;

        /// <summary>
        /// Tells if the unit is aging over time
        /// </summary>
        public virtual bool IsAging => mobility != Mobility.Still;

        /// <summary>
        /// Returns the current age of the unit
        /// </summary>
        public int CurrentAge => AgeInternal;

        /// <summary>
        /// 
        /// </summary>
        public bool IsOrbiting => isOrbiting;

        /// <summary>
        /// 
        /// </summary>
        public bool IsSolid => isSolid;

        /// <summary>
        /// 
        /// </summary>
        public bool IsMasking => isMasking;

        /// <summary>
        /// 
        /// </summary>
        public virtual bool HasListeners => false;

        /// <summary>
        /// 
        /// </summary>
        public float Radius => RadiusInternal;

        /// <summary>
        /// 
        /// </summary>
        public Mobility Mobility => mobility;

        // TODO: Überlegen ob es hier sinnvoll ist immer einen neuen Vector zu erzeugen, wegen dem GC

        /// <summary>
        /// Returns a copy of the position vector
        /// </summary>
        public Vector Position => new Vector(PositionInternal);

        /// <summary>
        /// Returns a copy of the movement vector
        /// </summary>
        public Vector Movement => new Vector(MovementInternal);
        #endregion

        /// <summary>
        /// Creates a map unit
        /// </summary>
        /// <param name="map"></param>
        /// <param name="unit"></param>
        /// <param name="movementOffset"></param>
        internal MapUnit(Map map, Unit unit, Vector movementOffset)
        {
            Map = map;

            Kind = unit.Kind;
            RadiusInternal = unit.Radius;
            Name = unit.Name;

            mobility = unit.Mobility;
            isOrbiting = unit.IsOrbiting;
            isMasking = unit.IsMasking;
            isSolid = unit.IsSolid;
            
            gravity = unit.Gravity;

            PositionInternal = unit.Position;
            MovementInternal = unit.Movement + movementOffset;
            
            if (isOrbiting)
            {
                OrbitingCenter = unit.OrbitingCenter;

                orbitingList = new List<UnitOrbitingState>();

                foreach (var orbitingState in unit.OrbitingList)
                    orbitingList.Add(new UnitOrbitingState(orbitingState));
            }

            AgeInternal = 0;
        }

        /// <summary>
        /// Optimized ctor for own map units
        /// </summary>
        /// <param name="map"></param>
        /// <param name="kind"></param>
        /// <param name="radius"></param>
        /// <param name="name"></param>
        /// <param name="mobility"></param>
        /// <param name="isOrbiting"></param>
        /// <param name="isMasking"></param>
        /// <param name="isSolid"></param>
        /// <param name="gravity"></param>
        /// <param name="position"></param>
        /// <param name="movement"></param>
        /// <param name="movementOffset"></param>
        /// <param name="orbitingCenter"></param>
        /// <param name="orbitingList"></param>
        internal MapUnit(Map map, 
                         UnitKind kind,
                         float radius,
                         string name, 
                         Mobility mobility,
                         bool isOrbiting,
                         bool isMasking,
                         bool isSolid,
                         float gravity,
                         Vector position, 
                         Vector movement,
                         Vector orbitingCenter = null,
                         IEnumerable<OrbitingState> orbitingList = null)
        {
            Map = map;

            Kind = kind;
            RadiusInternal = radius;
            Name = name;

            this.mobility = mobility;
            this.isOrbiting = isOrbiting;
            this.isMasking = isMasking;
            this.isSolid = isSolid;

            this.gravity = gravity;

            PositionInternal = position;
            MovementInternal = movement;

            if (isOrbiting)
            {
                OrbitingCenter = orbitingCenter;

                this.orbitingList = new List<UnitOrbitingState>();

                foreach (var orbitingState in orbitingList)
                    this.orbitingList.Add(new UnitOrbitingState(orbitingState));
            }

            AgeInternal = 0;
        }

        /// <summary>
        /// Constructor to clone a map unit
        /// </summary>
        /// <param name="mapUnit"></param>
        protected MapUnit(MapUnit mapUnit)
        {
            Map = mapUnit.Map;

            Kind = mapUnit.Kind;
            RadiusInternal = mapUnit.Radius;
            Name = mapUnit.Name;

            mobility = mapUnit.Mobility;
            isOrbiting = mapUnit.IsOrbiting;
            isMasking = mapUnit.IsMasking;
            isSolid = mapUnit.IsSolid;

            gravity = mapUnit.gravity;

            PositionInternal = new Vector(mapUnit.PositionInternal);
            MovementInternal = new Vector(mapUnit.MovementInternal);

            if (isOrbiting)
            {
                OrbitingCenter = mapUnit.OrbitingCenter;

                orbitingList = new List<UnitOrbitingState>();

                foreach (var orbitingState in mapUnit.orbitingList)
                    orbitingList.Add(new UnitOrbitingState(orbitingState));
            }

            AgeInternal = 0;
        }

        /// <summary>
        /// Ages the unit.
        /// Performs orbiting calculations if the unit is orbiting.
        /// </summary>
        /// <returns>Return false if unit age reached AgeMax</returns>
        internal virtual bool Age()
        {
            if (AgeMax > -1 && AgeInternal++ > AgeMax)
                return false;

            if (isOrbiting)
            {
                Vector currentCenter = new Vector(OrbitingCenter);

                foreach (UnitOrbitingState orbitingState in orbitingList)
                {
                    orbitingState.Rotation = orbitingState.RotationInterval > 0 ? (orbitingState.Rotation + 1) % orbitingState.RotationInterval : orbitingState.Rotation - 1 <= orbitingState.RotationInterval ? 0 : orbitingState.Rotation - 1;

                    orbitingState.Angle = orbitingState.StartAngle + (360f * ((float)orbitingState.Rotation / orbitingState.RotationIntervalAbsolute));

                    currentCenter += Vector.FromAngleLength(orbitingState.Angle, orbitingState.Distance);
                }

                PositionInternal = currentCenter;
            }

            return true;
        }

        /// <summary>
        /// Updates current unit with passed unit
        /// No checks 
        /// </summary>
        /// <param name="mapUnit"></param>
        internal virtual void Update(MapUnit mapUnit)
        {
            Debug.Assert(Name == mapUnit.Name && Kind == mapUnit.Kind);

            AgeInternal = 0;

            mobility = mapUnit.Mobility;
            isOrbiting = mapUnit.IsOrbiting;
            isMasking = mapUnit.IsMasking;
            isSolid = mapUnit.IsSolid;

            gravity = mapUnit.gravity;

            PositionInternal = mapUnit.PositionInternal;
            MovementInternal = mapUnit.MovementInternal;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append(Name);
            sb.Append(" ");
            sb.Append("(");
            sb.Append(Kind.ToString());
            sb.Append(") Position: (");
            sb.Append(PositionInternal.X.ToString("F"));
            sb.Append(" / ");
            sb.Append(PositionInternal.Y.ToString("F"));
            sb.Append(")");

            return sb.ToString();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool Equals(MapUnit other)
        {
            return other != null && Name == other.Name && Kind == other.Kind;
        }

        /// <summary>
        /// Checks for equality of the object
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            return Equals(obj as MapUnit);
        }

        /// <summary>
        /// Returns the hash code of this object
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return (Name.GetHashCode() * 397) ^ (int)Kind;
        }
    }
}
