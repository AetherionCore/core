using GameServerCore.Enums;
using LeagueSandbox.GameServer.GameObjects.StatsNS;
using LeagueSandbox.GameServer.Scripting.CSharp;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.GameObjects;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.GameObjects.SpellNS;

namespace Buffs
{
    class Masochism : IBuffGameScript
    {
        public BuffScriptMetaData BuffMetaData { get; set; } = new BuffScriptMetaData
        {
            BuffType = BuffType.COMBAT_ENCHANCER
        };

        public BuffAddType BuffAddType => BuffAddType.RENEW_EXISTING;
        public int MaxStacks => 1;
        public bool IsHidden => false;

        public StatsModifier StatsModifier { get; private set; } = new StatsModifier();

        public void OnActivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {
            var owner = ownerSpell.CastInfo.Owner;

            float AD = ((100 / owner.Stats.HealthPoints.Total) * (owner.Stats.HealthPoints.Total - owner.Stats.CurrentHealth) * 0.25f + (0.15f * ownerSpell.CastInfo.SpellLevel));
            float BaseDamage = 25f + (15f * ownerSpell.CastInfo.SpellLevel);
            float damage = BaseDamage + AD;

            StatsModifier.AttackDamage.FlatBonus = damage;
            unit.AddStatModifier(StatsModifier);
            //TODO: Make the AD Buff update based on Lost HP
        }

        public void OnDeactivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {
        }

        public void OnUpdate(float diff)
        {
            //This works to update correctly, but causes the StatModifier to not be removed when the buff ends.
            //Because on line 49 Statsmodifier = damage - buffer, and that will mostly result in 0s when there's no alteration to the health.

            /*timeSinceLastTick += diff;
            if (owner != null && spell != null && timeSinceLastTick >= 1000f)
            {
                AD = ((100 / owner.Stats.HealthPoints.Total) * (owner.Stats.HealthPoints.Total - owner.Stats.CurrentHealth) * 0.25f + (0.15f * spell.CastInfo.SpellLevel));
                BaseDamage = 25f + (15f * spell.CastInfo.SpellLevel);
                damage = BaseDamage + AD;
                StatsModifier.AttackDamage.FlatBonus = damage - buffer;
                buffer = damage;

                Unit.AddStatModifier(StatsModifier);
            }*/
        }
    }
}