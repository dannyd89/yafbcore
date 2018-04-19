using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using YAFBCore.Networking;

namespace YAFBCore.Controllables
{
    public class Ship : Controllable
    {
        #region Events
        /// <summary>
        /// Is called when ship isActive state is set to false
        /// We want to reregister this ship then in the manager
        /// </summary>
        public event EventHandler ActiveStateChanged;
        #endregion

        /// <summary>
        /// Flattiverse ship
        /// </summary>
        private Flattiverse.Ship ship;

        #region Scanning Fields
        /// <summary>
        /// Current degree the scanner is at
        /// </summary>
        private float currentScanDegree;

        /// <summary>
        /// Scan reference is a position of a still unit which is used as a reference point for the next scan
        /// This is used to always have at least one
        /// </summary>
        private Flattiverse.Vector scanReference;
        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ship"></param>
        internal Ship(UniverseSession universeSession, Flattiverse.Ship ship) 
            : base(universeSession, ship)
        {
            this.ship = ship;
        }

        /// <summary>
        /// Continues the ship if possible
        /// <para>Can trigger ActiveStateChanged event if IsActive is false</para>
        /// </summary>
        /// <returns>True if continue was successful</returns>
        public bool TryContinue()
        {
            try
            {
                if (!ship.IsActive)
                {
                    ActiveStateChanged?.Invoke(this, EventArgs.Empty);
                    return false;
                }

                if (!ship.IsAlive)
                    ship.Continue();

                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);

                return false;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        protected override void worker()
        {
            while (!isDisposed)
            {
                try
                {
                    flowControl.FlowControl.PreWait();

                    scan();

                    move();

                    shoot();

                    flowControl.FlowControl.Commit();
                }
                catch (Exception ex)
                {
                    if (isDisposed)
                        return;

                    Debug.WriteLine($"{ship.Name}: Worker Exception");
                    Debug.WriteLine(ex.Message);
                }
            }
        }

        /// <summary>
        /// Performs a scan with the ship
        /// </summary>
        protected override void scan()
        {
            try
            {
                List<Flattiverse.ScanInfo> scanInfos = new List<Flattiverse.ScanInfo>();

                int scannerCount = ship.ScannerCount;

                if (scanReference != null)
                {
                    // We got a reference point so we scan in that direction with a minimal degree because we know the direction
                    float oneFourthDegree = Math.Min(10f, ship.ScannerDegreePerScan / 4f);

                    scanInfos.Add(new Flattiverse.ScanInfo(scanReference.Angle - oneFourthDegree, 
                                                           scanReference.Angle + oneFourthDegree, 
                                                           ship.ScannerArea.Limit * 0.99f));

                    --scannerCount;
                }
                
                for (int i = 0; i < scannerCount; i++)
                {
                    Flattiverse.ScanInfo scanInfo = new Flattiverse.ScanInfo(currentScanDegree, currentScanDegree + ship.ScannerDegreePerScan, ship.ScannerArea.Limit * 0.99f);

                    currentScanDegree += ship.ScannerDegreePerScan;

                    if (currentScanDegree >= 360f)
                        currentScanDegree -= 360f;

                    scanInfos.Add(scanInfo);
                }

                List<Flattiverse.Unit> scannedUnits = ship.Scan(scanInfos);

                scanReference = null;

                // Get the nearest still unit as reference point
                for (int i = 0; i < scannedUnits.Count; i++)
                    if (scannedUnits[i].Mobility == Flattiverse.Mobility.Still
                        && scannedUnits[i].Kind != Flattiverse.UnitKind.Explosion
                        && (scanReference == null || scannedUnits[i].Position.Length < scanReference.Length))
                        scanReference = scannedUnits[i].Position;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"{ship.Name}: Scan Exception");
                Debug.WriteLine(ex.Message);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        protected override void move()
        {
            base.move();
        }

        /// <summary>
        /// Shoots with the ship
        /// </summary>
        protected override void shoot()
        {
            base.shoot();
        }

        /// <summary>
        /// Disposes the ship
        /// </summary>
        public override void Dispose()
        {
            base.Dispose();

            try
            {
                // We try to close the ship if it's still active
                ship.Close();
            }
            catch { }
        }
    }
}
