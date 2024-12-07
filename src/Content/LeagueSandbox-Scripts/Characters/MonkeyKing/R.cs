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

namespace Spells
{
    public class MonkeyKingSpinToWin : ISpellScript
    {
        public SpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
        {
            TriggersSpellCasts = true,
        };
        public void OnSpellCast(Spell spell)
        {
            var owner = spell.CastInfo.Owner;
            AddBuff("MonkeyKingSpinToWin", 4.0f, 1, spell, owner, owner);
            if (spell.CastInfo.Owner is Champion c)
            {
                var damage = 300 + (100 * (spell.CastInfo.SpellLevel - 1)) + (c.Stats.AbilityPower.Total * 0.6f);

                var units = GetUnitsInRange(c.Position, 450f, true);
                for (int i = 0; i < units.Count; i++)
                {
                    if (units[i].Team != c.Team && !(units[i] is ObjBuilding || units[i] is BaseTurret))
                    {
                        units[i].TakeDamage(c, damage, DamageType.DAMAGE_TYPE_MAGICAL, DamageSource.DAMAGE_SOURCE_SPELL, false);
                        AddParticleTarget(c, units[i], "MonkeyKing_Base_R_Tar.troy", units[i]);
                        AddParticleTarget(c, units[i], "MonkeyKing_Base_R_Tar_Audio.troy", units[i]);
                    }
                }

            }
        }
    }

    public class MonkeyKingSpinToWinLeave : ISpellScript
    {
        public SpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
        {
            TriggersSpellCasts = true,
        };
    }
}