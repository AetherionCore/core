using GameServerCore.Enums;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.GameObjects;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using LeagueSandbox.GameServer.GameObjects.SpellNS;
using LeagueSandbox.GameServer.GameObjects.StatsNS;
using LeagueSandbox.GameServer.Scripting.CSharp;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using LeagueSandbox.GameServer.API;
using GameServerLib.GameObjects.AttackableUnits;

namespace Buffs
{
    internal class ExplosiveShotDebuff : IBuffGameScript
    {
        float Damage;
        Buff ShotDebuff;
        ObjAIBase Tristana;
        AttackableUnit Unit;
        float TimeSinceLastTick = 500f;
        public BuffScriptMetaData BuffMetaData { get; set; } = new BuffScriptMetaData
        {
            BuffType = BuffType.DAMAGE,
            BuffAddType = BuffAddType.REPLACE_EXISTING
        };
        public StatsModifier StatsModifier { get; private set; } = new StatsModifier();
        public void OnActivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {
            Unit = unit;
            ShotDebuff = buff;
            Tristana = ownerSpell.CastInfo.Owner as Champion;
            ApiEventManager.OnDeath.AddListener(this, Unit, OnDeath, true);
            Damage = (35 + (45 * ownerSpell.CastInfo.SpellLevel) + Tristana.Stats.AbilityPower.Total) / 5f;
            AddParticleTarget(Tristana, unit, "tristana_explosiveShot_unit_tar.troy", Tristana, buff.Duration, 1f);
            AddParticleTarget(Tristana, unit, "tristana_explosiveShot_tar.troy", Tristana, buff.Duration, 1f);
        }
        public void OnDeath(DeathData deathData)
        {
            ShotDebuff.DeactivateBuff();
        }
        public void OnUpdate(float diff)
        {
            TimeSinceLastTick += diff;
            if (TimeSinceLastTick >= 1000f && !Unit.IsDead && Unit != null)
            {
                Unit.TakeDamage(Tristana, Damage, DamageType.DAMAGE_TYPE_MAGICAL, DamageSource.DAMAGE_SOURCE_PERIODIC, false);
                TimeSinceLastTick = 0;
            }
        }
    }
}