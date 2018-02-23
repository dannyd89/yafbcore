using FlattiBase.Brushes;
using FlattiBase.Helper;
using FlattiBase.Simples;
using Flattiverse;
using JARVIS;
using SharpDX;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace FlattiBase.Mapping.MapUnits
{

    public class MapUnitPlayerShip : MapUnit
    {
        public PlayerShip playerShip;
        private readonly ControllableInfo controllableInfo;
        private readonly Controllable controllable;
        private readonly JUnit junit;

        public readonly bool IsOtherShip;

        private static float penThicknessBars = 8f;
        private static float penThicknessUnitBorder = 2f;

        private SharpDX.DirectWrite.TextLayout textLayout;

        public MapUnitPlayerShip(FlattiBase.Screens.Screen screen, PlayerShip playerShip, Vector movementOffset)
            : base(screen, playerShip, movementOffset)
        {
            this.playerShip = playerShip;
            this.controllableInfo = playerShip.ControllableInfo;
            IsOtherShip = true;

            textLayout = new SharpDX.DirectWrite.TextLayout(screen.Parent.DirectWriteFactory,
                                                            playerShip.Name,
                                                            FlattiBase.Fonts.FormFonts.SmallTextFont,
                                                            100f, 20f);
        }

        public MapUnitPlayerShip(FlattiBase.Screens.Screen screen, Controllable controllable, Vector movementOffset)
            : base(screen, new Vector(), movementOffset, controllable.Radius, controllable.Name)
        {
            this.controllable = controllable;
            IsOtherShip = false;
            Gravity = controllable.Gravity;

            textLayout = new SharpDX.DirectWrite.TextLayout(screen.Parent.DirectWriteFactory,
                                                            controllable.Name,
                                                            FlattiBase.Fonts.FormFonts.SmallTextFont,
                                                            100f, 20f);
        }

        public MapUnitPlayerShip(FlattiBase.Screens.Screen screen, JUnit junit, Vector position, Vector movementOffset)
            : base(screen, position, movementOffset, junit.Radius.Value, junit.UnitName)
        {
            this.junit = junit;
            IsOtherShip = true;
            Gravity = 0.3f;

            textLayout = new SharpDX.DirectWrite.TextLayout(screen.Parent.DirectWriteFactory,
                                                            junit.UnitName,
                                                            FlattiBase.Fonts.FormFonts.SmallTextFont,
                                                            100f, 20f);
        }

        public override MapUnitKind Kind
        {
            get
            {
                return junit != null ? MapUnitKind.TempPlayerShip : MapUnitKind.PlayerShip;
            }
        }

        public override MapUnitMobility Mobility
        {
            get
            {
                return MapUnitMobility.Mobile;
            }
        }

        public override int AgeMax
        {
            get
            {
                return 5;
            }
        }

        public override bool HasAging
        {
            get
            {
                return IsOtherShip;
            }
        }

        public override void Draw(SharpDX.Direct2D1.WindowRenderTarget renderTarget, Transformator X, Transformator Y)
        {
            Vector2 position = new Vector2(X[Position.X], Y[Position.Y]);

            // Draw Playership
            #region Enemy ships
            if (IsOtherShip)
            {
                if (controllableInfo != null)
                {
                    Circle.Draw(renderTarget,
                        SolidColorBrushes.IndianRed,
                        position,
                        X.Prop(Radius), 
                        X.Prop(penThicknessUnitBorder));

                    float healthPercentage = controllableInfo.Hull / controllableInfo.HullMax;

                    if (healthPercentage > 0f)
                        Arc.Draw(renderTarget,
                                 SolidColorBrushes.OrangeRed,
                                 position,
                                 X.Prop(Radius - penThicknessBars),
                                 270f,
                                 360f * healthPercentage,
                                 X.Prop(penThicknessBars));

                    if (controllableInfo.ShieldMax > 0f)
                    {
                        float shieldPercentage = controllableInfo.Shield / controllableInfo.ShieldMax;

                        shieldPercentage = shieldPercentage < 0.01f ? 0.0f : shieldPercentage;

                        if (shieldPercentage > 0.0f)
                            Arc.Draw(renderTarget,
                                     SolidColorBrushes.CadetBlue,
                                     position,
                                     X.Prop(Radius - (penThicknessBars * 2f)),
                                     270f,
                                     360f * shieldPercentage,
                                     X.Prop(penThicknessBars));
                    }

                    if (!textLayout.IsDisposed)
                    {
                        float halfWidth = textLayout.Metrics.Width / 2f;
                        renderTarget.DrawTextLayout(new Vector2(position.X - halfWidth, position.Y + Y.Prop(Radius + penThicknessBars + 2f)),
                                                    textLayout,
                                                    SolidColorBrushes.TeamColors[playerShip.Team.Name], SharpDX.Direct2D1.DrawTextOptions.NoSnap);
                    }
                }
                else if (junit != null)
                {
                    Circle.Draw(renderTarget,
                                SolidColorBrushes.IndianRed,
                                position,
                                X.Prop(Radius), 
                                X.Prop(penThicknessUnitBorder));


                    if (!textLayout.IsDisposed)
                    {
                        float halfWidth = textLayout.Metrics.Width / 2f;
                        renderTarget.DrawTextLayout(new Vector2(position.X - halfWidth, position.Y + Y.Prop(Radius + penThicknessBars + 2f)),
                                                    textLayout, SolidColorBrushes.TeamColors[junit.TeamName], SharpDX.Direct2D1.DrawTextOptions.NoSnap);
                    }
                }
            }
            #endregion
            #region Own ships
            else
            {
                Circle.Draw(renderTarget,
                            SolidColorBrushes.LightBlue,
                            position,
                            X.Prop(Radius), 
                            X.Prop(penThicknessUnitBorder));

                #region Health, Energy, Shield
                float healthPercentage = controllable.Hull / controllable.HullMax;
                float energyPercentage = controllable.Energy / controllable.EnergyMax;

                healthPercentage = healthPercentage < 0.01f ? 0.0f : healthPercentage;

                if (healthPercentage > 0f)
                    Arc.Draw(renderTarget,
                             SolidColorBrushes.OrangeRed,
                             position,
                             X.Prop(Radius - penThicknessBars),
                             270f,
                             360f * healthPercentage,
                             X.Prop(penThicknessBars));

                energyPercentage = energyPercentage < 0.01f ? 0.0f : energyPercentage;

                if (energyPercentage > 0f)
                    Arc.Draw(renderTarget,
                             SolidColorBrushes.BlueViolet,
                             position,
                             X.Prop(Radius - (penThicknessBars * 2f)),
                             270f,
                             360f * energyPercentage,
                             X.Prop(penThicknessBars));

                if (controllable.ShieldMax > 0f)
                {
                    float shieldPercentage = controllable.Shield / controllable.ShieldMax;

                    shieldPercentage = shieldPercentage < 0.01f ? 0.0f : shieldPercentage;

                    if (shieldPercentage > 0f)
                        Arc.Draw(renderTarget,
                                 SolidColorBrushes.CadetBlue,
                                 position,
                                 X.Prop(Radius - (penThicknessBars * 3f)),
                                 270f,
                                 360f * shieldPercentage,
                                 X.Prop(penThicknessBars));
                }

                float weaponLoadPercentage = controllable.WeaponProductionStatus / controllable.WeaponProductionLoad;

                weaponLoadPercentage = weaponLoadPercentage < 0.01f ? 0.0f : weaponLoadPercentage;

                if (weaponLoadPercentage > 0f)
                    Arc.Draw(renderTarget,
                             SolidColorBrushes.LightGoldenrodYellow,
                             position,
                             X.Prop(Radius + (penThicknessBars * 1f)),
                             270f,
                             360f * weaponLoadPercentage,
                             X.Prop(penThicknessBars));
                #endregion

                if (!textLayout.IsDisposed)
                {
                    float halfWidth = textLayout.Metrics.Width / 2f;
                    renderTarget.DrawTextLayout(new Vector2(position.X - halfWidth, position.Y + Y.Prop(Radius + penThicknessBars + 2f)),
                                                textLayout,
                                                SolidColorBrushes.TeamColors[Screen.Parent.Connector.Player.Team.Name], SharpDX.Direct2D1.DrawTextOptions.NoSnap);
                }
            }
            #endregion
        }

        public override bool Calculate(int tickCount = 0)
        {
            throw new NotImplementedException();
        }

        public override bool AgeUnit(Map map)
        {
            if (IsOtherShip)
                Position += Movement;

            if (Age++ < AgeMax)
                return false;

            return true;
        }

        public override void Dispose()
        {
            textLayout.Dispose();
        }
    }
}
