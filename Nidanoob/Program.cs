#region LICENSE

// Copyright 2014 - 2014 Nidanoob
// Nidanoob.cs is part of Nidanoob.
// Nidanoob is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// Nidanoob is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
// GNU General Public License for more details.
// You should have received a copy of the GNU General Public License
// along with Nidanoob. If not, see <http://www.gnu.org/licenses/>.

#endregion

#region

using System;
using System.Drawing;
using LeagueSharp;
using LeagueSharp.Common;

#endregion

namespace Nidanoob
{
    internal class Program
    {
        public const string ChampionName = "Nidalee";
        private static Spell Q { get; set; }

        private static Obj_AI_Hero Player
        {
            get { return ObjectManager.Player; }
        }

        private static Menu Menu { get; set; }

        private static Obj_AI_Hero Target
        {
            get
            {
                return (SimpleTs.GetSelectedTarget().IsValidTarget(Q.Range) ? SimpleTs.GetSelectedTarget() : null) ??
                       SimpleTs.GetTarget(Q.Range, SimpleTs.DamageType.Magical);
            }
        }

        private static void Main(string[] args)
        {
            CustomEvents.Game.OnGameLoad += Game_OnGameLoad;
        }

        private static void Game_OnGameLoad(EventArgs args)
        {
            if (Player.ChampionName != ChampionName)
                return;

            Q = new Spell(SpellSlot.Q, 1500);
            Q.SetSkillshot(.125f, 40f, 1300f, true, SkillshotType.SkillshotLine);

            Menu = new Menu(ChampionName, ChampionName, true);
            Menu.AddItem(new MenuItem("combo", "UseQ").SetValue(new KeyBind("X".ToCharArray()[0], KeyBindType.Press)));
            Menu.AddToMainMenu();

            Game.OnGameUpdate += Game_OnGameUpdate;
            Drawing.OnDraw += Draw_Shit;

            Game.PrintChat("Nidanoob by zezzy loaded!");
        }

        private static void Game_OnGameUpdate(EventArgs args)
        {
            if (Player.IsDead)
                return;

            if (Q.IsReady() && Target.IsValidTarget(Q.Range) && Menu.Item("combo").GetValue<KeyBind>().Active)
            {
                Q.Cast(Target);
            }
        }

        private static void Draw_Shit(EventArgs args)
        {
            if (Player.IsDead)
                return;

            if (Q.IsReady())
            {
                Utility.DrawCircle(Player.Position, Q.Range, Color.Green);

                if (Target.IsValidTarget())
                    Utility.DrawCircle(Target.Position, 300, Color.Red);
            }
            else
            {
                Utility.DrawCircle(Player.Position, Q.Range, Color.Red);
            }
        }
    }
}