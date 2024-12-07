using System.Numerics;
using GameServerCore.Enums;
using GameServerCore.Scripting.CSharp;
using LeaguePackets.Game.Events;
using LeagueSandbox.GameServer.API;
using LeagueSandbox.GameServer.GameObjects;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.Buildings;
using LeagueSandbox.GameServer.GameObjects.SpellNS;
using LeagueSandbox.GameServer.GameObjects.StatsNS;
using LeagueSandbox.GameServer.Scripting.CSharp;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;

namespace Buffs
{
    internal class EkkoR : IBuffGameScript
    {
        public BuffScriptMetaData BuffMetaData { get; set; } = new BuffScriptMetaData
        {
            BuffAddType = BuffAddType.REPLACE_EXISTING
        };

        public StatsModifier StatsModifier { get; private set; } = new StatsModifier();

        Minion Ekko;
        Spell Spell;
        ObjAIBase Owner;
        private Buff buff;
        AttackableUnit Unit;
        float timeSinceLastTick = 1000f;

        public void OnActivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {
            Unit = unit;
            Owner = ownerSpell.CastInfo.Owner;
            Spell = ownerSpell;
            if (ownerSpell.CastInfo.Owner is Champion owner)
            {
                SetStatus(owner, StatusFlags.Ghosted, true);
                SetStatus(owner, StatusFlags.Targetable, false);
            }

        }
        public void OnDeactivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {
            if (Spell.CastInfo.Owner is Champion c)
            {
                SetStatus(c, StatusFlags.Ghosted, false);
                SetStatus(c, StatusFlags.Targetable, true);
                var damage = (150 * Spell.CastInfo.SpellLevel) + (c.Stats.AbilityPower.Total * 1.5f);
                AddParticle(c, null, "Ekko_Base_R_Tar", c.Position);
                var units = GetUnitsInRange(c.Position, 550f, true);
                for (int i = 0; i < units.Count; i++)
                {
                    if (units[i].Team != c.Team && !(units[i] is ObjBuilding || units[i] is BaseTurret))
                    {
                        units[i].TakeDamage(c, damage, DamageType.DAMAGE_TYPE_MAGICAL, DamageSource.DAMAGE_SOURCE_SPELL, false);
                        AddBuff("EkkoPassive", 6f, 1, Spell, units[i], c);
                        //AddParticleTarget(c, units[i], "Ekko_Base_R_Tar_Impact", units[i]);
                    }
                }
            }
        }
    }
}