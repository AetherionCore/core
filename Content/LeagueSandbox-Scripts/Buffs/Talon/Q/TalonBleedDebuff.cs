using GameServerCore.Enums;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using LeagueSandbox.GameServer.GameObjects;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.GameObjects.SpellNS;
using LeagueSandbox.GameServer.GameObjects.StatsNS;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.Scripting.CSharp;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using GameServerLib.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.API;

namespace Buffs
{
    internal class TalonBleedDebuff : IBuffGameScript
    {
        float Amp;
        Particle P;
        Buff Bleed;
        float Damage;
        float ADratio;
        ObjAIBase Talon;
        AttackableUnit Unit;
        float Time = 900f;
        public BuffScriptMetaData BuffMetaData { get; set; } = new BuffScriptMetaData
        {
            BuffType = BuffType.DAMAGE,
            BuffAddType = BuffAddType.REPLACE_EXISTING
        };


        public StatsModifier StatsModifier { get; private set; } = new StatsModifier();

        public void OnActivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {
            Unit = unit;
            Bleed = buff;
            Talon = ownerSpell.CastInfo.Owner as Champion;
            ADratio = Talon.Stats.AttackDamage.Total * 1.2f;
            Damage = ((10 * Talon.Spells[0].CastInfo.SpellLevel) + ADratio) / 6f;
            Amp = 0.03f * Talon.Spells[2].CastInfo.SpellLevel;
            if (Unit.HasBuff("TalonDamageAmp")) { Damage = Damage * (1 + Amp); }
            ApiEventManager.OnDeath.AddListener(this, Unit, OnDeath, true);
            P = AddParticleTarget(Talon, Unit, "talon_base_Q_bleed_indicator", Unit, buff.Duration, 1f);
        }
        public void OnDeath(DeathData deathData)
        {
            Bleed.DeactivateBuff();
        }
        public void OnDeactivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {
            RemoveParticle(P);
        }
        public void OnUpdate(float diff)
        {
            Time += diff;

            if (Time >= 1000.0f && Unit != null)
            {
                Unit.TakeDamage(Talon, Damage, DamageType.DAMAGE_TYPE_PHYSICAL, DamageSource.DAMAGE_SOURCE_PERIODIC, false);
                Time = 0f;
            }
        }
    }
}