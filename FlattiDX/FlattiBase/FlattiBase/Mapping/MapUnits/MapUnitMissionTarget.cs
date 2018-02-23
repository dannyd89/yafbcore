using FlattiBase.Brushes;
using FlattiBase.Helper;
using FlattiBase.Simples;
using Flattiverse;
using SharpDX;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace FlattiBase.Mapping.MapUnits
{

    public class MapUnitMissionTarget : MapUnit
    {
        public MissionTarget missionTarget;

        private Team team;
        private Team dominatingTeam;

        private bool isHit;

        private SharpDX.DirectWrite.TextLayout textLayout;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="screen"></param>
        /// <param name="missionTarget"></param>
        /// <param name="movementOffset"></param>
        public MapUnitMissionTarget(FlattiBase.Screens.Screen screen, MissionTarget missionTarget, Vector movementOffset)
            : base(screen, missionTarget, movementOffset)
        {
            this.missionTarget = missionTarget;

            team = missionTarget.Team;

            textLayout = new SharpDX.DirectWrite.TextLayout(screen.Parent.DirectWriteFactory,
                                                            "#" + missionTarget.SequenceNumber.ToString().PadLeft(2,'0'),
                                                            FlattiBase.Fonts.FormFonts.SmallTextFont,
                                                            100f, 15f);

            this.radius = 7f;
        }

        /// <summary>
        /// 
        /// </summary>
        public override MapUnitKind Kind
        {
            get
            {
                return MapUnitKind.MissionTarget;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public override MapUnitMobility Mobility
        {
            get
            {
                return missionTarget.IsOrbiting ? MapUnitMobility.Steady : MapUnitMobility.Still;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public override int AgeMax
        {
            get
            {
                return int.MaxValue;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public override bool HasAging
        {
            get
            {
                return missionTarget.IsOrbiting;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="renderTarget"></param>
        /// <param name="X"></param>
        /// <param name="Y"></param>
        public override void Draw(SharpDX.Direct2D1.WindowRenderTarget renderTarget, Transformator X, Transformator Y)
        {
            if (team == null || team != null && team.Name == "None")
                Circle.Draw(renderTarget,
                            isHit ? SolidColorBrushes.LimeGreen : SolidColorBrushes.DarkRed,
                            new Vector2(X[Position.X], Y[Position.Y]),
                            X.Prop(Radius), X.Prop(3f));
            else
                Circle.Draw(renderTarget,
                            SolidColorBrushes.TeamColors[team.Name],
                            new Vector2(X[Position.X], Y[Position.Y]),
                            X.Prop(Radius), X.Prop(7f));

            if (missionTarget.DominationRadius > 0f)
            {
                if (team == null)
                    Circle.Draw(renderTarget,
                                SolidColorBrushes.GreenYellow,
                                new Vector2(X[Position.X], Y[Position.Y]),
                                X.Prop(missionTarget.DominationRadius));
                else
                    Circle.Draw(renderTarget,
                                SolidColorBrushes.TeamColors[team.Name],
                                new Vector2(X[Position.X], Y[Position.Y]),
                                X.Prop(missionTarget.DominationRadius), X.Prop(4f));

                if (dominatingTeam != null)
                    Circle.Draw(renderTarget,
                                SolidColorBrushes.TeamColors[dominatingTeam.Name],
                                new Vector2(X[Position.X], Y[Position.Y]),
                                X.Prop(missionTarget.DominationRadius - 10f), 
                                X.Prop(8f), 
                                new SharpDX.Direct2D1.StrokeStyle(Screen.Parent.Factory, new SharpDX.Direct2D1.StrokeStyleProperties() { DashStyle = SharpDX.Direct2D1.DashStyle.Dash }));
            }



            foreach (Vector hint in missionTarget.Hints)
                renderTarget.DrawLine(new Vector2(X[Position.X], Y[Position.Y]), 
                                      new Vector2(X[Position.X] + hint.X * 2000000f, Y[Position.Y] + hint.Y * 2000000f), 
                                      SolidColorBrushes.White);

            if (textLayout != null && !textLayout.IsDisposed)
            {
                float halfWidth = textLayout.Metrics.Width / 2f;
                float halfHeight = textLayout.Metrics.Height;
                renderTarget.DrawTextLayout(new Vector2(X[Position.X] - halfWidth, Y[Position.Y] - Y.Prop(Radius) - halfHeight),
                                            textLayout,
                                            SolidColorBrushes.White, SharpDX.Direct2D1.DrawTextOptions.NoSnap);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="msg"></param>
        public override void ParseMessage(FlattiverseMessage msg)
        {
            if (msg is TargetDominationStartedMessage)
            {
                TargetDominationStartedMessage targetDominationStartedMessage = (TargetDominationStartedMessage)msg;

                dominatingTeam = targetDominationStartedMessage.DominatingTeam;
            }
            else if (msg is TargetDominationFinishedMessage)
            {
                TargetDominationFinishedMessage targetDominationFinishedMessage = (TargetDominationFinishedMessage)msg;

                dominatingTeam = null;
                team = targetDominationFinishedMessage.DominatingTeam;
            }
            else if (msg is TargetDedominationStartedMessage)
            {
                TargetDedominationStartedMessage targetDedominationStartedMessage = (TargetDedominationStartedMessage)msg;

                dominatingTeam = targetDedominationStartedMessage.DominatingTeam;
            }
            else if (msg is PlayerUnitHitMissionTargetMessage)
            {
                PlayerUnitHitMissionTargetMessage playerUnitHitMissionTargetMessage = (PlayerUnitHitMissionTargetMessage)msg;

                if (playerUnitHitMissionTargetMessage.MissionTargetName == missionTarget.Name
                    && !isHit)
                    isHit = true;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tickCount"></param>
        /// <returns></returns>
        public override bool Calculate(int tickCount = 0)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Dispose()
        {
            textLayout.Dispose();
        }
    }
}
