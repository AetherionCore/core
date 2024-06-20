using GameServerCore.Enums;
using GameServerCore.Scripting.CSharp;
using GameServerLib.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.API;
using LeagueSandbox.GameServer.GameObjects;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using LeagueSandbox.GameServer.GameObjects.SpellNS;
using LeagueSandbox.GameServer.GameObjects.StatsNS;
using LeagueSandbox.GameServer.Scripting.CSharp;
using System.Numerics;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;

namespace Buffs
{
    public class ZedUltExecute : IBuffGameScript
    {
        public BuffScriptMetaData BuffMetaData { get; set; } = new BuffScriptMetaData
        {
            BuffType = BuffType.COMBAT_ENCHANCER,
            BuffAddType = BuffAddType.REPLACE_EXISTING
        };

        public StatsModifier StatsModifier { get; private set; } = new StatsModifier();

        ObjAIBase Owner;
        AttackableUnit Target;
        Spell rspell;
        bool didcast = false;
        float findamage;
        private readonly AttackableUnit target;

        public void OnActivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {
            var owner = ownerSpell.CastInfo.Owner;
            Owner = owner;
            Target = unit;
            rspell = ownerSpell;
            AddParticleTarget(owner, unit, "Zed_Ult_Impact.troy", unit, 10f);
            AddParticleTarget(owner, unit, "Zed_Ult_DelayedDamage_tar.troy", unit, lifetime: 3f);
            AddParticleTarget(owner, unit, "Zed_Ult_DashEnd.troy", unit);
            ApiEventManager.OnTakeDamage.AddListener(this, unit, TakeDamage, false);
        }

        public void TakeDamage(DamageData damageData)
        {
            findamage += damageData.Damage;
        }

        public void OnDeactivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {
            var owner = ownerSpell.CastInfo.Owner;
            float damage = Owner.Stats.AttackDamage.Total;
            float percdamage = 5f + rspell.CastInfo.SpellLevel * 15f;
            float finaldamage = (findamage / 100f * percdamage);
            findamage = damage + finaldamage;
            AddParticleTarget(owner, unit, "zed_ult_pop_kill.troy", unit);
            unit.TakeDamage(Owner, findamage, DamageType.DAMAGE_TYPE_PHYSICAL, DamageSource.DAMAGE_SOURCE_SPELL, false);
        }

        public void OnUpdate(float diff)
        {
        }
    }
}
