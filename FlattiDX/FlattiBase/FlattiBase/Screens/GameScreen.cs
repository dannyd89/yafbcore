using FlattiBase.Brushes;
using FlattiBase.Forms;
using FlattiBase.Forms.TableComponents;
using FlattiBase.Helper;
using FlattiBase.Managers;
using FlattiBase.Mapping;
using FlattiBase.Mapping.MapUnits;
using FlattiBase.Screens.MenuScreens;
using FlattiBase.Ships;
using FlattiBase.Simples;
using Flattiverse;
using JARVIS;
using SharpDX;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FlattiBase.Screens
{
    public class GameScreen : Screen
    {
        public Map GlobalMap;

        private Thread mainThread;

        private UniverseGroup currentUniverse;
        private Team currentTeam;

        public static List<ShipBase> ShipList = new List<ShipBase>();
        private ShipBase currentShip;
        private int currentShipIndex = 0;

        private Transformator X;
        private Transformator Y;

        private UniverseTable scoreBoard;
        private bool showScoreBoard = false;

        private bool showGameMenu = false;

        private float viewCenterX = 0f;
        private float viewCenterY = 0f;

        private float destinationViewCenterX = 0f;
        private float destinationViewCenterY = 0f;

        private float currentMouseX = 0f;
        private float currentMouseY = 0f;

        private BinaryMessenger binaryMessenger;
        private volatile bool isRunning = true;
        private bool multiShoot = false;
        private float scale = 3f;
        private Vector clickVector;

        private const int PADDING = 30;

        #region Properties
        public bool ShowGameMenu
        {
            get { return showGameMenu; }
            set { showGameMenu = value; }
        }
        #endregion
        
#if STEPHANOS
        private static string ShipClass = "Danyliebtmichnicht";
        private static string ShipNameStringFormat = "Zaziki Nr.{0}";
#elif THOMAS
        private static string ShipClass = "Test";
        private static string ShipNameStringFormat = "THE GAME Nr.{0}";
#else
        private static string ShipClass = "D3RP";
        private static string ShipNameStringFormat = "D{0}RP";
#endif

        public GameScreen(ScreenManager parent, Team team)
            : base(parent, "GameScreen")
        {
            currentUniverse = parent.UniverseGroup;
            currentTeam = team;

            binaryMessenger = new BinaryMessenger(Parent.Connector);

            binaryMessenger.SendNeedHandshake();

            //if (TryAddShip("M" + (ShipList.Count + 1).ToString() + "RP", "D1RP", out currentShip))
            //    ShipList.Add(currentShip);

            //if (TryAddShip("S" + (ShipList.Count + 1).ToString() + "RP", "Danyliebtmichnicht", out currentShip))
            //    ShipList.Add(currentShip);

            if (TryAddShip(getShipName(ShipList.Count + 1), ShipClass, out currentShip))
                ShipList.Add(currentShip);

            scoreBoard = new UniverseTable(this, currentUniverse, PADDING, PADDING, parent.Width - PADDING * 2f, parent.Height - PADDING * 2f, SolidColorBrushes.BlackHalfTransparent);

            scoreBoard.AddColumn(" ", "SmallAvatar", UniverseTeamTable.MAX_COLUMN_WIDTH, UniverseTeamTable.MAX_COLUMN_HEIGHT);
            scoreBoard.AddColumn("Name", "Name", UniverseTeamTable.MAX_COLUMN_WIDTH, UniverseTeamTable.MAX_COLUMN_HEIGHT);
            scoreBoard.AddColumn("Score", "Score", UniverseTeamTable.MAX_COLUMN_WIDTH, UniverseTeamTable.MAX_COLUMN_HEIGHT);
            scoreBoard.AddColumn("Kills", "Kills", UniverseTeamTable.MAX_COLUMN_WIDTH, UniverseTeamTable.MAX_COLUMN_HEIGHT);
            scoreBoard.AddColumn("Deaths", "Deaths", UniverseTeamTable.MAX_COLUMN_WIDTH, UniverseTeamTable.MAX_COLUMN_HEIGHT);
            scoreBoard.AddColumn("Avg. Commit Time", "AverageCommitTime", 250f, UniverseTeamTable.MAX_COLUMN_HEIGHT);

            mainThread = new Thread(mainWorker);
            mainThread.Start();
        }

        private void mainWorker()
        {
            using (UniverseGroupFlowControl flowControl = currentUniverse.GetNewFlowControl())
                while (isRunning)
                {
                    flowControl.PreWait();

                    if (GlobalMap != null)
                        GlobalMap.Age();

                    FlattiverseMessage msg;
                    while (parent.Connector.NextMessage(out msg))
                    {
                        if (msg is TargetDominationStartedMessage)
                        {
                            TargetDominationStartedMessage targetDominationStartedMessage = (TargetDominationStartedMessage)msg;

                            //Console.WriteLine("--- Domination started ---");
                            //Console.WriteLine("Team: " + targetDominationStartedMessage.DominatingTeam.Name);
                            //Console.WriteLine("MissionTarget: " + targetDominationStartedMessage.MissionTargetName);

                            MapUnit mapUnit = GlobalMap.MapUnits.Find(m => m.Name == targetDominationStartedMessage.MissionTargetName);

                            if (mapUnit != null)
                                mapUnit.ParseMessage(targetDominationStartedMessage);
                        }
                        else if (msg is TargetDominationFinishedMessage)
                        {
                            TargetDominationFinishedMessage targetDominationFinishedMessage = (TargetDominationFinishedMessage)msg;

                            //Console.WriteLine("--- Domination ended ---");
                            //Console.WriteLine("Team: " + targetDominationFinishedMessage.DominatingTeam.Name);
                            //Console.WriteLine("MissionTarget: " + targetDominationFinishedMessage.MissionTargetName);

                            MapUnit mapUnit = GlobalMap.MapUnits.Find(m => m.Name == targetDominationFinishedMessage.MissionTargetName);

                            if (mapUnit != null)
                                mapUnit.ParseMessage(targetDominationFinishedMessage);
                        }
                        else if (msg is TargetDedominationStartedMessage)
                        {
                            TargetDedominationStartedMessage targetDedominationStartedMessage = (TargetDedominationStartedMessage)msg;
                            
                            //Console.WriteLine("--- Dedomination started ---");
                            //Console.WriteLine("Team stealing it: " + targetDedominationStartedMessage.DominatingTeam.Name);
                            //Console.WriteLine("MissionTarget: " + targetDedominationStartedMessage.MissionTargetName);

                            MapUnit mapUnit = GlobalMap.MapUnits.Find(m => m.Name == targetDedominationStartedMessage.MissionTargetName);

                            if (mapUnit != null)
                                mapUnit.ParseMessage(targetDedominationStartedMessage);
                        }
                        else if (msg is BinaryChatMessage)
                        {
                            BinaryMessageInfo msgInfo = binaryMessenger.ParseBinaryMessage(msg as BinaryChatMessage);

                            switch (msgInfo.BinaryMessageType)
                            {
                                case BinaryMessageType.NeedAck:
                                    binaryMessenger.SendHandshake(msgInfo.Source);
                                    break;
                                case BinaryMessageType.PositionFriendly:
                                    if (GlobalMap != null)
                                    {
                                        BinaryMessagePlayerInfo binaryMessagePlayerInfo = (BinaryMessagePlayerInfo)msgInfo;

                                        JUnit playerShip = new JUnit(binaryMessagePlayerInfo.ShipName, binaryMessagePlayerInfo.Team, binaryMessagePlayerInfo.Radius);

                                        GlobalMap.TryAddUnit(playerShip, binaryMessagePlayerInfo.Units);
                                    }
                                    break;
                                case BinaryMessageType.PositionEnemy:
                                    if (GlobalMap != null)
                                    {
                                        BinaryMessageEnemyInfo binaryMessageEnemyInfo = (BinaryMessageEnemyInfo)msgInfo;

                                        foreach (KeyValuePair<JUnit, List<JUnit>> kvp in binaryMessageEnemyInfo.Enemies)
                                            GlobalMap.TryAddUnit(kvp.Key, kvp.Value);
                                    }
                                    break;
                            }
                        }
                        else if (msg is PlayerUnitHitMissionTargetMessage)
                        {
                            PlayerUnitHitMissionTargetMessage playerUnitHitMissionTargetMessage = (PlayerUnitHitMissionTargetMessage)msg;

                            Console.WriteLine("MissionTarget hit: " + playerUnitHitMissionTargetMessage.MissionTargetName);

                            MapUnit mapUnit = GlobalMap.MapUnits.Find(m => m.Name == playerUnitHitMissionTargetMessage.MissionTargetName);

                            if (mapUnit != null)
                                mapUnit.ParseMessage(playerUnitHitMissionTargetMessage);
                        }
                    }
                    //if (binaryMessenger != null)
                    //{
                    //    List<BinaryMessageInfo> msgList = binaryMessenger.GetPendingMessages();

                    //    foreach (BinaryMessageInfo msgInfo in msgList)
                    //    {
                    //        switch (msgInfo.BinaryMessageType)
                    //        {
                    //            case BinaryMessageType.NeedAck:
                    //                binaryMessenger.SendHandshake(msgInfo.Source);
                    //                break;
                    //            case BinaryMessageType.PositionFriendly:
                    //                if (GlobalMap != null)
                    //                {
                    //                    BinaryMessagePlayerInfo binaryMessagePlayerInfo = (BinaryMessagePlayerInfo)msgInfo;

                    //                    JUnit playerShip = new JUnit(binaryMessagePlayerInfo.ShipName, binaryMessagePlayerInfo.Team, binaryMessagePlayerInfo.Radius);

                    //                    GlobalMap.TryAddUnit(playerShip, binaryMessagePlayerInfo.Units);
                    //                }
                    //                break;
                    //            case BinaryMessageType.PositionEnemy:
                    //                if (GlobalMap != null)
                    //                {
                    //                    BinaryMessageEnemyInfo binaryMessageEnemyInfo = (BinaryMessageEnemyInfo)msgInfo;

                    //                    foreach (KeyValuePair<JUnit, List<JUnit>> kvp in binaryMessageEnemyInfo.Enemies)
                    //                        GlobalMap.TryAddUnit(kvp.Key, kvp.Value);
                    //                }
                    //                break;
                    //        }
                    //    }
                    //}

                    flowControl.Wait();

                    flowControl.Commit();
                }
        }

        public bool TryAddShip(string name, string @class, out ShipBase baseShip)
        {
            try
            {
                ShipBase ship = new ShipBase(this, currentUniverse, name, binaryMessenger);
                ship.CreateShip(@class);
                ship.Continue();

                baseShip = ship;
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("--- GameScreen.TryAddShip");
                Console.WriteLine(ex.Message);

                baseShip = null;
                return false;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public override bool IsUpdatable
        {
            get
            {
                return true;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public override bool IsDrawable
        {
            get
            {
                return true;
            }
        }

        public override void Update(TimeSpan lastUpdate)
        {
            if (showScoreBoard)
                scoreBoard.Update(lastUpdate);
        }

        public override void Draw()
        {
            viewCenterX += (destinationViewCenterX - viewCenterX) / 5f;
            viewCenterY += (destinationViewCenterY - viewCenterY) / 5f;

            List<MapUnit> mapUnits = null;

            RectangleF sourceRect;

            MapUnit shipUnit = null;

            if (currentShip != null && currentShip.IsAlive)
            {
                mapUnits = currentShip.MapUnits;

                shipUnit = mapUnits != null ? mapUnits.Find(unit => unit.Name == currentShip.Ship.Name) : null;

                if (shipUnit != null)
                    sourceRect = getSourceRectangleF(shipUnit.Position.X, shipUnit.Position.Y, scale, Parent.Width, Parent.Height);
                else
                    sourceRect = getSourceRectangleF(viewCenterX, viewCenterY, scale, Parent.Width, Parent.Height);
            }
            else
            {
                if (GlobalMap != null)
                    mapUnits = GlobalMap.MapUnits;

                sourceRect = getSourceRectangleF(viewCenterX, viewCenterY, scale, Parent.Width, Parent.Height);
            }

            X = new Transformator(sourceRect.Left, sourceRect.Right, 0, Parent.Width);
            Y = new Transformator(sourceRect.Top, sourceRect.Bottom, 0, Parent.Height);

            if (mapUnits != null)
                foreach (MapUnit mapUnit in mapUnits)
                    mapUnit.Draw(Parent.RenderTarget, X, Y);

            if (clickVector != null)
                Circle.Draw(Parent.RenderTarget,
                            SolidColorBrushes.White,
                            new Vector2(X[clickVector.X], Y[clickVector.Y]),
                            X.Prop(10f));

            Stopwatch sw = Stopwatch.StartNew();

            //if (GlobalMap != null)
            //{
            //    MapRaster mapRaster = GlobalMap.GetMapRaster(90);

            //    Log.AddLogEntry("Raster and Draw Time, Tilecount: " + mapRaster.Raster.Length, sw.Elapsed);

            //    mapRaster.Draw(Parent.RenderTarget, X, Y);
            //}

            if (showScoreBoard)
                scoreBoard.Draw(Parent.RenderTarget);
        }

        public override void MouseWheel(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (e.Delta == 0)
                return;

            if (e.Delta > 0)
            {
                scale *= 1.2f;
            }
            else
            {
                scale /= 1.2f;
            }

            if (scale < 0.10f)
                scale = 0.10f;

            if (scale > 15f)
                scale = 15f;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public override void MouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Middle)
            {
                destinationViewCenterX += (e.X - currentMouseX) / scale;
                destinationViewCenterY += (e.Y - currentMouseY) / scale;

                currentMouseX = e.X;
                currentMouseY = e.Y;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public override void MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (X != null && Y != null)
            {
                if (e.Button == System.Windows.Forms.MouseButtons.Left && showGameMenu == false && currentShip != null)
                {
                    clickVector = new Vector(X.Rev(e.X), Y.Rev(e.Y));
                    currentShip.MoveShip(clickVector);
                }
                else if (e.Button == System.Windows.Forms.MouseButtons.Right && showGameMenu == false && currentShip != null)
                {
                    if (multiShoot)
                    {
                        clickVector = new Vector(X.Rev(e.X), Y.Rev(e.Y));

                        for (int i = 0; i < ShipList.Count; i++)
                            ShipList[i].Shoot(clickVector);
                    }
                    else
                        currentShip.Shoot(new Vector(X.Rev(e.X), Y.Rev(e.Y)));
                }
                else if (e.Button == System.Windows.Forms.MouseButtons.Middle)
                {
                    currentMouseX = e.X;
                    currentMouseY = e.Y;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public override void MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Middle)
            {
                currentMouseX = e.X;
                currentMouseY = e.Y;
            }
        }

        public override void KeyPressed(System.Windows.Forms.Keys keyData)
        {
            if (keyData == System.Windows.Forms.Keys.C && currentShip != null)
                currentShip.Continue();
            else if (keyData == System.Windows.Forms.Keys.Q && currentShip != null)
            {
                currentShip.LoadShield = !currentShip.LoadShield;
                currentShip.RepairSelf = false;
            }
            else if (keyData == System.Windows.Forms.Keys.W && currentShip != null)
            {
                currentShip.RepairSelf = !currentShip.RepairSelf;
                currentShip.LoadShield = false;
            }
            else if (keyData == System.Windows.Forms.Keys.Tab)
            {
                if (showGameMenu == false)
                    showScoreBoard = !showScoreBoard;
            }
            else if (keyData == System.Windows.Forms.Keys.D1)
            {
                if (ShipList.Count >= 1)
                    currentShip = ShipList[0];

                currentShipIndex = 0;
            }
            else if (keyData == System.Windows.Forms.Keys.D2)
            {
                if (ShipList.Count >= 2)
                    currentShip = ShipList[1];

                currentShipIndex = 1;
            }
            else if (keyData == System.Windows.Forms.Keys.D3)
            {
                if (ShipList.Count >= 3)
                    currentShip = ShipList[2];

                currentShipIndex = 2;
            }
            else if (keyData == System.Windows.Forms.Keys.D4)
            {
                if (ShipList.Count >= 4)
                    currentShip = ShipList[3];

                currentShipIndex = 3;
            }
            else if (keyData == System.Windows.Forms.Keys.D5)
            {
                if (ShipList.Count >= 5)
                    currentShip = ShipList[4];

                currentShipIndex = 4;
            }
            else if (keyData == System.Windows.Forms.Keys.D6)
            {
                if (ShipList.Count >= 6)
                    currentShip = ShipList[5];

                currentShipIndex = 5;
            }
            else if (keyData == System.Windows.Forms.Keys.D7)
            {
                if (ShipList.Count >= 7)
                    currentShip = ShipList[6];

                currentShipIndex = 6;
            }
            else if (keyData == System.Windows.Forms.Keys.D8)
            {
                if (ShipList.Count >= 8)
                    currentShip = ShipList[7];

                currentShipIndex = 7;
            }
            else if (keyData == System.Windows.Forms.Keys.D9)
            {
                if (ShipList.Count >= 9)
                    currentShip = ShipList[8];

                currentShipIndex = 8;
            }
            else if (keyData == System.Windows.Forms.Keys.D0)
            {
                currentShip = null;
            }
            else if (keyData == System.Windows.Forms.Keys.Escape)
            {
                if (showGameMenu == false)
                {
                    if (showScoreBoard)
                        showScoreBoard = false;

                    showGameMenu = true;
                    Parent.AddScreen(new GameMenuScreen(this));
                }
            }
            else if (keyData == System.Windows.Forms.Keys.Insert)
            {
                if (TryAddShip(getShipName(ShipList.Count + 1), ShipClass, out currentShip))
                    ShipList.Add(currentShip);

                currentShipIndex = ShipList.Count - 1;
            }
            else if (keyData == System.Windows.Forms.Keys.Delete)
            {
                if (TryAddShip(getShipName(ShipList.Count + 1), ShipClass, out currentShip))
                    ShipList.Add(currentShip);

                currentShipIndex = ShipList.Count - 1;
            }
            else if (keyData == System.Windows.Forms.Keys.PageUp)
            {
                if (TryAddShip(getShipName(ShipList.Count + 1), ShipClass, out currentShip))
                    ShipList.Add(currentShip);

                currentShipIndex = ShipList.Count - 1;
            }
            else if (keyData == System.Windows.Forms.Keys.M)
            {
                multiShoot = !multiShoot;
            }
            else if (keyData == System.Windows.Forms.Keys.F1)
            {
                Parent.AddScreen(new GameMenuScreen(this));
            }
            else if (keyData == System.Windows.Forms.Keys.D)
            {
                if (currentShipIndex < ShipList.Count - 1)
                    currentShip = ShipList[++currentShipIndex];
                else
                {
                    currentShipIndex = 0;
                    currentShip = ShipList[currentShipIndex];
                }
            }
            else if (keyData == System.Windows.Forms.Keys.A)
            {
                if (currentShipIndex > 0)
                    currentShip = ShipList[--currentShipIndex];
                else
                {
                    currentShipIndex = ShipList.Count;
                    currentShip = ShipList[--currentShipIndex];
                }
            }

        }

        public void LeaveCurrentUniverse()
        {
            foreach (ShipBase ship in ShipList)
                ship.RemoveShip();

            while (isRunning)
            {
                foreach (ShipBase ship in ShipList)
                {
                    if (ship.IsActive == true)
                    {
                        isRunning = true;
                        break;
                    }
                    isRunning = false;
                }
            }

            ShipList = new List<ShipBase>();

            binaryMessenger.Close();

            while (mainThread.IsAlive)
                Thread.Sleep(1);

            if (currentUniverse != null)
            {
                currentUniverse.Part();

                currentUniverse = null;
            }

            Parent.RemoveScreen(this);

            Parent.AddScreen(new UniverseLobbyScreen(Parent));
        }

        public override void Dispose()
        {
            foreach (ShipBase ship in ShipList)
                ship.RemoveShip();

            while (isRunning)
            {
                foreach (ShipBase ship in ShipList)
                {
                    if (ship.IsActive == true)
                    {
                        isRunning = true;
                        break;
                    }
                    isRunning = false;
                }
            }

            ShipList = new List<ShipBase>();

            binaryMessenger.Close();

            while (mainThread.IsAlive)
                Thread.Sleep(1);

            if (currentUniverse != null)
            {
                currentUniverse.Part();

                currentUniverse = null;
            }
        }

        private RectangleF getSourceRectangleF(float dx, float dy, float targetScale, float targetWidth, float targetHeight)
        {
            return new RectangleF(dx - targetWidth / targetScale,
                                  dy - targetHeight / targetScale,
                                  targetWidth / targetScale * 2f,
                                  targetHeight / targetScale * 2f);
        }

        private static string getShipName(int count)
        {
            return string.Format(ShipNameStringFormat, count);
        }
    }
}
