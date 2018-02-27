﻿using Flattiverse;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YAFBCore.Flattiverse.Mapping.Units
{
    public class MapUnit
    {
        public readonly Map Map;

        public readonly string Name;
        public readonly UnitKind Kind;

        protected int age;

        protected bool isOrbiting;
        protected bool isSolid;
        protected bool isMasking;
        protected float radius;
        protected Mobility mobility;

        protected float gravity;

        internal Vector PositionInternal;
        internal Vector MovementInternal;

        internal Vector OrbitingCenter;
        protected List<UnitOrbitingState> orbitingList;

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
            radius = unit.Radius;
            Name = unit.Name;

            mobility = unit.Mobility;
            isOrbiting = unit.IsOrbiting;
            isMasking = unit.IsMasking;
            isSolid = unit.IsSolid;
            
            gravity = unit.Gravity;

            PositionInternal = unit.Position;
            MovementInternal = unit.Movement - movementOffset;
            
            if (isOrbiting)
            {
                OrbitingCenter = unit.OrbitingCenter;

                orbitingList = new List<UnitOrbitingState>();

                foreach (var orbitingState in unit.OrbitingList)
                    orbitingList.Add(new UnitOrbitingState(orbitingState));
            }

            age = 0;
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
                         ReadOnlyCollection<OrbitingState> orbitingList = null)
        {
            Map = map;

            Kind = kind;
            this.radius = radius;
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

            age = 0;
        }

        /// <summary>
        /// Constructor to clone a map unit
        /// </summary>
        /// <param name="mapUnit"></param>
        protected MapUnit(MapUnit mapUnit)
        {
            Map = mapUnit.Map;

            Kind = mapUnit.Kind;
            radius = mapUnit.Radius;
            Name = mapUnit.Name;

            mobility = mapUnit.Mobility;
            isOrbiting = mapUnit.IsOrbiting;
            isMasking = mapUnit.IsMasking;
            isSolid = mapUnit.IsSolid;

            gravity = mapUnit.gravity;

            PositionInternal = mapUnit.PositionInternal;
            MovementInternal = mapUnit.MovementInternal;

            if (isOrbiting)
            {
                OrbitingCenter = mapUnit.OrbitingCenter;

                orbitingList = new List<UnitOrbitingState>();

                foreach (var orbitingState in mapUnit.orbitingList)
                    orbitingList.Add(new UnitOrbitingState(orbitingState));
            }

            age = 0;
        }

        #region Properties
        /// <summary>
        /// Tells if the unit is aging over time
        /// </summary>
        public virtual bool IsAging
        {
            get { return false; }
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual int AgeMax
        {
            get { return -1; }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool IsOrbiting
        {
            get { return isOrbiting; }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool IsSolid
        {
            get { return isSolid; }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool IsMasking
        {
            get { return isMasking; }
        }

        /// <summary>
        /// 
        /// </summary>
        public float Radius
        {
            get { return radius; }
        }

        /// <summary>
        /// 
        /// </summary>
        public Mobility Mobility
        {
            get { return mobility; }
        }

        /// <summary>
        /// Returns a copy of the position vector
        /// </summary>
        public Vector Position
        {
            get { return new Vector(PositionInternal); }
        }

        /// <summary>
        /// Returns a copy of the movement vector
        /// </summary>
        public Vector Movement
        {
            get { return new Vector(MovementInternal); }
        }
        #endregion

        /// <summary>
        /// Ages the unit
        /// Performs orbiting calculations if the unit is orbiting
        /// </summary>
        /// <returns></returns>
        internal virtual bool Age()
        {
            if (AgeMax > -1 && age++ < AgeMax)
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

            age = 0;

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
            sb.Append(PositionInternal.X.ToString("F"));
            sb.Append(")");

            return sb.ToString();
        }
    }
}
