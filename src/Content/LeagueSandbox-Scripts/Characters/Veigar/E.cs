using System.Numerics;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using LeagueSandbox.GameServer.Scripting.CSharp;
using GameServerCore.Scripting.CSharp;
using GameServerCore.Enums;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.GameObjects.SpellNS;
using LeagueSandbox.GameServer.GameObjects.SpellNS.Missile;
using LeagueSandbox.GameServer.GameObjects.SpellNS.Sector;
using LeagueSandbox.GameServer.API;
using System.Collections.Generic;
using LeagueSandbox.GameServer.GameObjects;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.Buildings;
using GameServerLib.GameObjects.AttackableUnits;
using LeaguePackets.Game.Common;
using LeagueSandbox.GameServer.GameObjects.StatsNS;

namespace Spells
{
    public class VeigarEventHorizon : ISpellScript
    {
        Vector2 truecoords;
        public SpellScriptMetadata ScriptMetadata => new SpellScriptMetadata()
        {
            TriggersSpellCasts = true,
            IsDamagingSpell = true,

        };

        public void OnSpellCast(Spell spell)
        {
            var owner = spell.CastInfo.Owner;
            var ownerSkinID = owner.SkinID;
            truecoords = new Vector2(spell.CastInfo.TargetPosition.X, spell.CastInfo.TargetPosition.Z);
            var distance = Vector2.Distance(spell.CastInfo.Owner.Position, truecoords);
            if (distance > 650f)
            {
                truecoords = GetPointFromUnit(spell.CastInfo.Owner, 650f);
            }

            string cage = "";
            switch (ownerSkinID)
            {
                case 8:
                    cage = "Veigar_Skin08_E_cage_green.troy";
                    break;
                case 6:
                    cage = "Veigar_Skin06_E_cage_green.troy";
                    break;
                case 4:
                    cage = "Veigar_Skin04_E_cage_green.troy";
                    break;
                default:
                    cage = "Veigar_Base_E_cage_green.troy";
                    break;
            }
            AddParticle(owner, null, cage, truecoords, lifetime: 3f);

            //TODO: Stun Hitbox & Buff
        }

        public void OnUpdate(float diff)
        {
            //ticks++;

            //if (ticks <= 180)
            //{
            //    var units = GetUnitsInRange(truecoords, 350f, true);
            //    for (int i = 0; i < units.Count; i++)
            //    {
            //        if (Vector2.Distance(units[i].Position, truecoords) >= 350f && Vector2.Distance(units[i].Position, truecoords) <= 370f)
            //        {
            //            units[i].ApplyCrowdControl(stun, Owner);
            //            AddBuff("VeigarEventHorizon", duration, 1, SPELL, units[i], Owner);

            //        }
            //    }

        }
    }
}