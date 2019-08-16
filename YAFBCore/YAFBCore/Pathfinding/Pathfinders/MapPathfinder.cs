using Flattiverse;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using YAFBCore.Controllables.Commands;
using YAFBCore.Mapping;
using YAFBCore.Utils;

namespace YAFBCore.Pathfinding.Pathfinders
{
    public sealed class MapPathfinder
    {
        /// <summary>
        /// Map this pathfinder is running on
        /// </summary>
        internal readonly Map Map;

        /// <summary>
        /// Tile size this path finder is based on
        /// </summary>
        internal readonly int TileSize;

        /// <summary>
        /// 
        /// </summary>
        private MapSectionRaster[] rasters;

        /// <summary>
        /// 
        /// </summary>
        public MapSectionRaster[] Rasters => rasters;

        /// <summary>
        /// Current count of sections in 1 dimension
        /// </summary>
        private int currentSectionCount;

        /// <summary>
        /// 
        /// </summary>
        private Task<MapSectionRaster>[] tasks;

        /// <summary>
        /// 
        /// </summary>
        //private bool isDisposed;

        /// <summary>
        /// 
        /// </summary>
        //public bool IsDisposed => isDisposed;

        /// <summary>
        /// Creates a 
        /// </summary>
        /// <param name="rasters"></param>
        internal MapPathfinder(int tileSize, Map map, MapSection[] mapSections, int sectionCount)
        {
            TileSize = tileSize;
            Map = map;

            init(mapSections, sectionCount);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="arg"></param>
        private void init(MapSection[] mapSections, int sectionCount)
        {
            try
            {
                //Stopwatch sw = Stopwatch.StartNew();
                
                // TODO: Try to restructure this so the arrays can be reused if possible

                rasters = new MapSectionRaster[mapSections.Length];
                tasks = new Task<MapSectionRaster>[mapSections.Length];

                for (int i = 0; i < mapSections.Length; i++)
                    tasks[i] = mapSections[i].GetRaster(TileSize);

                Task.WaitAll(tasks);
                
                for (int i = 0; i < rasters.Length; i++)
                    rasters[i] = tasks[i].Result;

                // Connect all connecting tiles with each other
                //for (int i = 0; i < rasters.Length; i++)
                //{
                //    int x = i % sectionCount, y = i / sectionCount;

                //    if (rasters[i].TopConnectingTiles[0].To == null && tryGetMapSectionRasterIndex(sectionCount, x, y - 1, out int topIndex))
                //    {
                //        MapSectionRasterConnectingTile[] bottomTiles = rasters[topIndex].BottomConnectingTiles;

                //        for (int tempIndex = 0; tempIndex < bottomTiles.Length; tempIndex++)
                //        {
                //            rasters[i].TopConnectingTiles[tempIndex].To = bottomTiles[tempIndex].From;
                //            bottomTiles[tempIndex].To = rasters[i].TopConnectingTiles[tempIndex].From;
                //        }
                //    }

                //    if (rasters[i].RightConnectingTiles[0].To == null && tryGetMapSectionRasterIndex(sectionCount, x + 1, y, out int rightIndex))
                //    {
                //        MapSectionRasterConnectingTile[] leftTiles = rasters[rightIndex].LeftConnectingTiles;

                //        for (int tempIndex = 0; tempIndex < leftTiles.Length; tempIndex++)
                //        {
                //            rasters[i].RightConnectingTiles[tempIndex].To = leftTiles[tempIndex].From;
                //            leftTiles[tempIndex].To = rasters[i].RightConnectingTiles[tempIndex].From;
                //        }
                //    }

                //    if (rasters[i].BottomConnectingTiles[0].To == null && tryGetMapSectionRasterIndex(sectionCount, x, y + 1, out int bottomIndex))
                //    {
                //        MapSectionRasterConnectingTile[] topTiles = rasters[bottomIndex].TopConnectingTiles;

                //        for (int tempIndex = 0; tempIndex < topTiles.Length; tempIndex++)
                //        {
                //            rasters[i].BottomConnectingTiles[tempIndex].To = topTiles[tempIndex].From;
                //            topTiles[tempIndex].To = rasters[i].BottomConnectingTiles[tempIndex].From;
                //        }
                //    }

                //    if (rasters[i].LeftConnectingTiles[0].To == null && tryGetMapSectionRasterIndex(sectionCount, x - 1, y, out int leftIndex))
                //    {
                //        MapSectionRasterConnectingTile[] rightTiles = rasters[leftIndex].RightConnectingTiles;

                //        for (int tempIndex = 0; tempIndex < rightTiles.Length; tempIndex++)
                //        {
                //            rasters[i].LeftConnectingTiles[tempIndex].To = rightTiles[tempIndex].From;
                //            rightTiles[tempIndex].To = rasters[i].LeftConnectingTiles[tempIndex].From;
                //        }
                //    }
                //}

                currentSectionCount = sectionCount;

                //Console.WriteLine("Pathfinder update time: " + sw.Elapsed);
            }
            catch (Exception ex)
            {
                Console.WriteLine("MapPathfinder.init: ");
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public LinkedList<MoveCommand> Pathfind(Vector from, Vector to)
        {
#if DEBUG
            try
            {
#endif
                //if (isDisposed)
                //    return null;

                float size = (currentSectionCount / 2f) * Map.SectionSize;
                Transformator transformator = new Transformator(-size, size, 0, currentSectionCount);

                int fromIndex = getMapSectionIndex(transformator, from.X, from.Y);
                int toIndex = getMapSectionIndex(transformator, to.X, to.Y);

                if (fromIndex == toIndex)
                {
                    MapSectionRaster raster = rasters[fromIndex];

                    Transformator rasterX = new Transformator(raster.MapSection.Left, raster.MapSection.Right, 0, raster.Size);
                    Transformator rasterY = new Transformator(raster.MapSection.Top, raster.MapSection.Bottom, 0, raster.Size);

                    int x = (int)rasterX[from.X];
                    int y = (int)rasterY[from.Y];

                    MapSectionRasterTile fromTile = raster.Tiles[x + y * raster.Size];
                    fromTile.Status |= MapSectionRasterTileStatus.Start;

                    x = (int)rasterX[to.X];
                    y = (int)rasterY[to.Y];

                    MapSectionRasterTile toTile = raster.Tiles[x + y * raster.Size];
                    toTile.Status |= MapSectionRasterTileStatus.Finish;

                    PriorityQueue priorityQueue = new PriorityQueue(raster.Tiles.Length);

                    priorityQueue.Enqueue(fromTile, 0f);

                    MapSectionRasterTile currentTile = null;

                    float step = 0f;
                    while (priorityQueue.Count > 0)
                    {
                        currentTile = priorityQueue.Dequeue();

                        if ((currentTile.Status & MapSectionRasterTileStatus.Finish) == MapSectionRasterTileStatus.Finish)
                        {
                            Console.WriteLine("Found path to the desired tile");
                            break;
                        }

                        // TODO: Check if this part is optimizable since we could maybe do some checks beforehand

                        // Check top tile
                        x = (int)rasterX[currentTile.X];
                        y = (int)rasterY[currentTile.Y - raster.TileSize];

                        if (x >= 0 && x < raster.Size && y >= 0 && y < raster.Size)
                        {
                            MapSectionRasterTile topTile = raster.Tiles[x + y * raster.Size];

                            if ((topTile.Status & MapSectionRasterTileStatus.Blocked) != MapSectionRasterTileStatus.Blocked
                                && (topTile.Status & MapSectionRasterTileStatus.Checked) != MapSectionRasterTileStatus.Checked)
                            {
                                topTile.ParentX = currentTile.X;
                                topTile.ParentY = currentTile.Y;

                                priorityQueue.Enqueue(topTile, step /*+ 2f * (Math.Abs(topTile.X - toTile.X) + Math.Abs(topTile.Y - toTile.Y))*/);
                            }
                        }

                        // Check right tile
                        x = (int)rasterX[currentTile.X + raster.TileSize];
                        y = (int)rasterY[currentTile.Y];

                        if (x >= 0 && x < raster.Size && y >= 0 && y < raster.Size)
                        {
                            MapSectionRasterTile rightTile = raster.Tiles[x + y * raster.Size];

                            if ((rightTile.Status & MapSectionRasterTileStatus.Blocked) != MapSectionRasterTileStatus.Blocked
                                && (rightTile.Status & MapSectionRasterTileStatus.Checked) != MapSectionRasterTileStatus.Checked)
                            {
                                rightTile.ParentX = currentTile.X;
                                rightTile.ParentY = currentTile.Y;

                                priorityQueue.Enqueue(rightTile, step /*+ 2f * (Math.Abs(rightTile.X - toTile.X) + Math.Abs(rightTile.Y - toTile.Y))*/);
                            }
                        }

                        // Check bottom tile
                        x = (int)rasterX[currentTile.X];
                        y = (int)rasterY[currentTile.Y + raster.TileSize];

                        if (x >= 0 && x < raster.Size && y >= 0 && y < raster.Size)
                        {
                            MapSectionRasterTile bottomTile = raster.Tiles[x + y * raster.Size];

                            if ((bottomTile.Status & MapSectionRasterTileStatus.Blocked) != MapSectionRasterTileStatus.Blocked
                                && (bottomTile.Status & MapSectionRasterTileStatus.Checked) != MapSectionRasterTileStatus.Checked)
                            {
                                bottomTile.ParentX = currentTile.X;
                                bottomTile.ParentY = currentTile.Y;

                                priorityQueue.Enqueue(bottomTile, step /*+ 2f * (Math.Abs(bottomTile.X - toTile.X) + Math.Abs(bottomTile.Y - toTile.Y))*/);
                            }
                        }

                        // Check left tile
                        x = (int)rasterX[currentTile.X - raster.TileSize];
                        y = (int)rasterY[currentTile.Y];

                        if (x >= 0 && x < raster.Size && y >= 0 && y < raster.Size)
                        {
                            MapSectionRasterTile leftTile = raster.Tiles[x + y * raster.Size];

                            if ((leftTile.Status & MapSectionRasterTileStatus.Blocked) != MapSectionRasterTileStatus.Blocked
                                && (leftTile.Status & MapSectionRasterTileStatus.Checked) != MapSectionRasterTileStatus.Checked)
                            {
                                leftTile.ParentX = currentTile.X;
                                leftTile.ParentY = currentTile.Y;

                                priorityQueue.Enqueue(leftTile, step /*+ 2f * (Math.Abs(leftTile.X - toTile.X) + Math.Abs(leftTile.Y - toTile.Y))*/);
                            }
                        }

                        step++;
                    }

                    LinkedList<MoveCommand> linkedList = new LinkedList<MoveCommand>();
                    linkedList.AddFirst(new MoveCommand(to.X, to.Y));

                    while ((currentTile.Status & MapSectionRasterTileStatus.Start) != MapSectionRasterTileStatus.Start)
                    {
                        //Console.WriteLine("Current tile: (" + currentTile.X + " / " + currentTile.Y + ")");

                        x = (int)rasterX[currentTile.ParentX];
                        y = (int)rasterY[currentTile.ParentY];

                        currentTile = raster.Tiles[x + y * raster.Size];

                        linkedList.AddFirst(new MoveCommand(currentTile.X, currentTile.Y));
                    }

                    return linkedList;
                }
                else
                    return new LinkedList<MoveCommand>(new MoveCommand[] { new MoveCommand(to.X, to.Y) }); // Wut wat is pathfinding Lul
#if DEBUG
            }
            catch (Exception ex)
            {
                Console.WriteLine("MapPathfinder.Pathfind: ");
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);

                return null;
            }
#endif
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private bool tryGetMapSectionRasterIndex(int sectionCount, int x, int y, out int index)
        {
            index = -1;

            if (x < 0 || x >= sectionCount || y < 0 || y >= sectionCount)
                return false;

            index = x + y * sectionCount;
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        private int getMapSectionIndex(Transformator transformator, float x, float y)
        {
            int posX = (int)(transformator[x] + 0.1f);
            int posY = (int)(transformator[y] + 0.1f);

            posX = Utils.Mathematics.MathUtil.Clamp(posX, 0, currentSectionCount);
            posY = Utils.Mathematics.MathUtil.Clamp(posY, 0, currentSectionCount);

            return posX + posY * currentSectionCount;
        }
    }
}
