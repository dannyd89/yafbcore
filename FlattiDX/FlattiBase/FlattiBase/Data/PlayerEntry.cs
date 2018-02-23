using FlattiBase.Helper;
using FlattiBase.Interfaces;
using Flattiverse;
using SharpDX.Direct2D1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace FlattiBase.Data
{
    public class PlayerEntry
    {
        public SharpDX.Direct2D1.Bitmap SmallAvatar
        {
            get;
            set;
        }

        public string Name
        {
            get;
            set;
        }

        public string Score
        {
            get;
            set;
        }

        public uint Kills
        {
            get;
            set;
        }

        public uint Deaths
        {
            get;
            set;
        }

        public string Ping
        {
            get;
            set;
        }

        public string AverageCommitTime
        {
            get;
            set;
        }

        private readonly Player player;

        public static Dictionary<string, PropertyInfo> PropertyInfos;

        public PlayerEntry(WindowRenderTarget renderTarget, Player player)
        {
            this.player = player;

            if (player.IsActive)
            {
                Bitmap bitmap;
                if (ImageCollections.PlayersSmallAvatars.TryGetValue(player.Name, out bitmap))
                {
                    if (bitmap == null)
                        SmallAvatar = ImageCollections.PlayerDefaultSmallAvatar;
                    else
                        SmallAvatar = bitmap;
                }
                else
                    SmallAvatar = ImageCollections.PlayerDefaultSmallAvatar;

                Name = player.Name;

                if (player.ControllableInfos != null)
                    foreach (ControllableInfo controllableInfo in player.ControllableInfos)
                        Name = Name + Environment.NewLine + controllableInfo.Name;

                Score = player.PlayerScores.PVPScore.ToString("F");
                Kills = player.PlayerScores.KillEnemyPlayerShip;
                Deaths = player.PlayerScores.DeathEnemyPlayerShip;
                AverageCommitTime = player.AverageCommitTime.Milliseconds.ToString() + " ms";
                Ping = player.Ping.Milliseconds.ToString() + " ms";
            }
        }

    }
}
