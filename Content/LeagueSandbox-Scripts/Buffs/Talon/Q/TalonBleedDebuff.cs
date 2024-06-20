using GameServerCore.Enums;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using LeagueSandbox.GameServer.GameObjects;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.GameObjects.SpellNS;
using LeagueSandbox.GameServer.GameObjects.StatsNS;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.Scripting.CSharp;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;

namespace Buffs
{
    internal class TalonBleedDebuff : IBuffGameScript
    {
        public BuffScriptMetaData BuffMetaData { get; set; } = new BuffScriptMetaData
        {
            BuffType = BuffType.COMBAT_ENCHANCER,
            BuffAddType = BuffAddType.REPLACE_EXISTING
        };

        public StatsModifier StatsModifier { get; private set; } = new StatsModifier();

        float damage;
        float timeSinceLastTick = 900f;
        AttackableUnit Unit;
        ObjAIBase owner;
        Particle p;
        Buff thisBuff;
        bool isVisible = true;
        Particle p2;

        public void OnActivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {
            thisBuff = buff;
            owner = ownerSpell.CastInfo.Owner as Champion;
            Unit = unit;
            var ADratio = owner.Stats.AttackDamage.Total * 1.2f;
            damage = (10 * ownerSpell.CastInfo.SpellLevel + ADratio) / 6f;
            var ELevel = owner.GetSpell("TalonCutthroat").CastInfo.SpellLevel;
            var damageamp = 0.03f * (ELevel - 1);
            if (unit.HasBuff("TalonDamageAmp"))
            {
                damage = damage + damage * damageamp;
            }
            p = AddParticleTarget(owner, unit, "talon_Q_bleed", unit, buff.Duration, 1f);
            p2 = AddParticleTarget(owner, unit, "talon_Q_bleed_indicator", unit, buff.Duration, 1f);
            if (unit.IsDead)
            {
                RemoveParticle(p);
                RemoveBuff(thisBuff);
                RemoveParticle(p2);
            }
        }

        public void OnDeactivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {
            RemoveParticle(p);
            RemoveBuff(thisBuff);
            RemoveParticle(p2);
        }
        public void OnUpdate(float diff)
        {
            timeSinceLastTick += diff;

            if (timeSinceLastTick >= 1000.0f && Unit != null)
            {
                Unit.TakeDamage(owner, damage, DamageType.DAMAGE_TYPE_PHYSICAL, DamageSource.DAMAGE_SOURCE_PERIODIC, false);
                timeSinceLastTick = 0f;
            }
        }
    }
}
