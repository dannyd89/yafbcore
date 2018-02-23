using FlattiBase.Managers;
using FlattiBase.Mapping.MapUnits;
using FlattiBase.Screens;
using Flattiverse;
using JARVIS;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
//using System.Threading.Tasks;

namespace FlattiBase.Mapping
{
    public class Map
    {
        /// <summary>
        /// Known map units
        /// </summary>
        private Dictionary<string, MapUnit> mapUnits;

        private Screen screen;
        private object syncMap = new object();

        //private MapRasterizer mapRasterizer;

        #region Bounds
        private float left;

        /// <summary>
        /// Most left value
        /// </summary>
        public float Left
        {
            get { return left; }
            private set { left = value; }
        }

        private float top;

        /// <summary>
        /// Most top value
        /// </summary>
        public float Top
        {
            get { return top; }
            private set { top = value; }
        }


        private float bottom;

        /// <summary>
        /// Most bottom value
        /// </summary>
        public float Bottom
        {
            get { return bottom; }
            private set { bottom = value; }
        }

        private float right;

        /// <summary>
        /// Most right value
        /// </summary>
        public float Right
        {
            get { return right; }
            private set { right = value; }
        }
        #endregion

        public List<MapUnit> MapUnits
        {
            get
            {
                lock (syncMap)
                    return new List<MapUnit>(mapUnits.Values);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="unitList"></param>
        public Map(Screen screen, Controllable owner, List<Unit> unitList)
        {
            lock (syncMap)
            {
                this.screen = screen;

                mapUnits = new Dictionary<string, MapUnit>();

                Vector movementOffset = new Vector();

                MapUnit mapUnit;

                foreach (Unit unit in unitList)
                    if (unit.Kind != UnitKind.Explosion && unit.Mobility == Mobility.Still)
                    {
                        movementOffset = unit.Movement;
                        break;
                    }

                foreach (Unit unit in unitList)
                {
                    if (unit.Kind == UnitKind.PlayerShip && ((PlayerShip)unit).Player.Name == screen.Parent.Connector.Player.Name)
                        continue;

                    mapUnit = MapUnitFactory.GetMapUnit(screen, unit, movementOffset);

                    if (mapUnit != null)
                    {
                        mapUnits.Add(mapUnit.Name, mapUnit);
                        checkBounds(mapUnit);
                    }
                }

                mapUnit = MapUnitFactory.GetMapUnit(screen, owner, movementOffset);
                checkBounds(mapUnit);

                mapUnits.Add(owner.Name, mapUnit);

                //mapRasterizer = new MapRasterizer(this);
            }
        }

        /// <summary>
        /// Tries to merge a map with another
        /// </summary>
        /// <param name="map"></param>
        /// <returns></returns>
        public bool Merge(Map map)
        {
            lock (syncMap)
            {
                Vector positionOffset = null;

                foreach (MapUnit mapUnit in map.mapUnits.Values)
                    if (mapUnit.Kind != MapUnitKind.Explosion 
                        && (mapUnit.Mobility == MapUnitMobility.Still || mapUnit.Mobility == MapUnitMobility.Steady) 
                        && mapUnits.ContainsKey(mapUnit.Name))
                    {
                        positionOffset = mapUnits[mapUnit.Name].Position - mapUnit.Position;
                        break;
                    }

                if (positionOffset == null)
                    return false;

                foreach (MapUnit mapUnit in map.mapUnits.Values)
                {
                    MapUnit tempMapUnit;

                    if (mapUnits.TryGetValue(mapUnit.Name, out tempMapUnit))
                    {
                        if (mapUnit.Kind == tempMapUnit.Kind)
                        {
                            if (tempMapUnit.Mobility != MapUnitMobility.Still)
                            {
                                tempMapUnit.Position = positionOffset + mapUnit.Position;

                                checkBounds(tempMapUnit);

                                if (tempMapUnit.IsOrbiting && mapUnit.IsOrbiting)
                                    tempMapUnit.OrbitingCenter = positionOffset + mapUnit.OrbitingCenter;

                                tempMapUnit.Movement = mapUnit.Movement;
                                tempMapUnit.Age = 0;

                                switch (mapUnit.Kind)
                                {
                                    case MapUnitKind.Shot:
                                        ((MapUnitShot)tempMapUnit).Time = ((MapUnitShot)mapUnit).Shot.Time;
                                        break;
                                }
                            }

                            mapUnit.Dispose();
                        }
                        else
                        {
                            // Add explosions
                            tempMapUnit.Dispose();
                            mapUnits.Remove(tempMapUnit.Name);

                            tempMapUnit = mapUnit;
                            tempMapUnit.Position = positionOffset + mapUnit.Position;

                            tempMapUnit.Movement = mapUnit.Movement;
                            tempMapUnit.Age = 0;

                            checkBounds(mapUnit);

                            mapUnits.Add(mapUnit.Name, mapUnit);
                        }
                    }
                    else
                    {
                        mapUnit.Position = positionOffset + mapUnit.Position;

                        if (mapUnit.IsOrbiting)
                            mapUnit.OrbitingCenter = positionOffset + mapUnit.OrbitingCenter;

                        checkBounds(mapUnit);

                        mapUnits.Add(mapUnit.Name, mapUnit);
                    }
                }

                return true;
            }            
        }

        /// <summary>
        /// 
        /// </summary>
        public void Age()
        {
            lock (syncMap)
            {
                foreach (KeyValuePair<string, MapUnit> kvp in mapUnits.Reverse())
                    if (kvp.Value.HasAging && kvp.Value.AgeUnit(this))
                    {
                        mapUnits.Remove(kvp.Key);
                        kvp.Value.Dispose();
                    }
                        
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="playerShip"></param>
        /// <param name="stillUnits"></param>
        public void TryAddUnit(JUnit playerShip, List<JUnit> stillUnits)
        {
            lock (syncMap)
            {
                if (!mapUnits.ContainsKey(playerShip.UnitName))
                {
                    foreach (JUnit junit in stillUnits)
                        if (mapUnits.ContainsKey(junit.UnitName) && junit.Kind == JUnitKind.StillUnit)
                        {
                            Vector position = new Vector(mapUnits[junit.UnitName].Position.X + junit.X.Value, mapUnits[junit.UnitName].Position.Y + junit.Y.Value);

                            mapUnits.Add(playerShip.UnitName, MapUnitFactory.GetMapUnit(screen, playerShip, position, Vector.FromNull()));
                            break;
                        }
                }
                else if (mapUnits[playerShip.UnitName].Kind == MapUnitKind.TempPlayerShip)
                {
                    foreach (JUnit junit in stillUnits)
                        if (mapUnits.ContainsKey(junit.UnitName) && junit.Kind == JUnitKind.StillUnit)
                        {
                            Vector position = new Vector(mapUnits[junit.UnitName].Position.X + junit.X.Value, mapUnits[junit.UnitName].Position.Y + junit.Y.Value);

                            mapUnits[playerShip.UnitName].Position = position;
                            break;
                        }
                }
                        
            }
        }

        /// <summary>
        /// Tries to get the unit with the passed name
        /// </summary>
        /// <param name="name">Name of unit</param>
        /// <param name="mapUnit">Found unit</param>
        /// <returns>Returns true if unit was found</returns>
        public bool TryGetUnit(string name, out MapUnit mapUnit)
        {
            lock (syncMap)
            {
                if (!mapUnits.TryGetValue(name, out mapUnit))
                    return false;

                return true;
            }
        }

        /// <summary>
        /// Checks if unit defines a bigger map size
        /// </summary>
        /// <param name="mapUnit"></param>
        private void checkBounds(MapUnit mapUnit)
        {
            if (mapUnit.Position.X - mapUnit.Radius < left)
                left = mapUnit.Position.X - mapUnit.Radius;
            if (mapUnit.Position.X + mapUnit.Radius > right)
                right = mapUnit.Position.X + mapUnit.Radius;

            if (mapUnit.Position.Y - mapUnit.Radius < top)
                top = mapUnit.Position.Y - mapUnit.Radius;
            if (mapUnit.Position.Y + mapUnit.Radius > bottom)
                bottom = mapUnit.Position.Y + mapUnit.Radius;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tileSize"></param>
        /// <returns></returns>
        //public MapRaster GetMapRaster(int tileSize)
        //{
        //    return mapRasterizer.RasterizeMap(tileSize);
        //}
    }
}
