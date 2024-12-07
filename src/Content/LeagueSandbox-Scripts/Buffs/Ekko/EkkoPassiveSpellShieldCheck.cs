using System.Numerics;
using GameServerCore.Enums;
using GameServerCore.Scripting.CSharp;
using LeaguePackets.Game.Events;
using LeagueSandbox.GameServer.API;
using LeagueSandbox.GameServer.GameObjects;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using LeagueSandbox.GameServer.GameObjects.SpellNS;
using LeagueSandbox.GameServer.GameObjects.StatsNS;
using LeagueSandbox.GameServer.Scripting.CSharp;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;

namespace Buffs
{
    class EkkoPassiveSlow : IBuffGameScript
    {
        public BuffScriptMetaData BuffMetaData { get; set; } = new BuffScriptMetaData
        {
            BuffType = BuffType.COMBAT_ENCHANCER,
            BuffAddType = BuffAddType.REPLACE_EXISTING
        };

        public StatsModifier StatsModifier { get; private set; } = new StatsModifier();

        float damage;
        AttackableUnit Unit;
        ObjAIBase owner;
        Particle p;
        Particle p1;
        Buff thisBuff;
        bool isVisible = true;
        Particle p2;

        public void OnActivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {
            thisBuff = buff;
            owner = ownerSpell.CastInfo.Owner as Champion;
            Unit = unit;
            var APratio = owner.Stats.AbilityPower.Total * 0.8f;
            damage = 20 + (10 * owner.Stats.Level) + APratio;
            Unit.TakeDamage(owner, damage, DamageType.DAMAGE_TYPE_MAGICAL, DamageSource.DAMAGE_SOURCE_PERIODIC, false);
            AddParticleTarget(owner, unit, "Ekko_Base_P_Proc", unit, 10f, 1f);
            //p = AddParticleTarget(owner, unit, "Ekko_Base_P_Disappear", unit, 0.1f, 1f);
            //p1 = AddParticleTarget(owner, unit, "Ekko_Base_P_Slow_Avatar", unit, 0.1f, 1f);
            p2 = AddParticleTarget(owner, unit, "Ekko_Base_P_Proc_Cool", unit, buff.Duration, 1f);
        }

        public void OnDeactivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {
            RemoveParticle(p);
            RemoveParticle(p1);
            RemoveBuff(thisBuff);
            RemoveParticle(p2);
        }
    }
}