using GameServerCore.Enums;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.API;
using LeagueSandbox.GameServer.GameObjects;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using LeagueSandbox.GameServer.GameObjects.SpellNS;
using LeagueSandbox.GameServer.GameObjects.StatsNS;
using LeagueSandbox.GameServer.Scripting.CSharp;
using System.Numerics;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using GameServerLib.GameObjects.AttackableUnits;


namespace Buffs
{
    internal class ZedUltExecute : IBuffGameScript
    {
        private Spell Ult;
        float findamage;
        float t;
        Particle a;
        string Int;
        float damage;
        float Maxdamage;
        float AI;
        float Aidamage;
        float percdamage;
        private ObjAIBase Zed;
        AttackableUnit Target;
        public BuffScriptMetaData BuffMetaData { get; set; } = new BuffScriptMetaData
        {
            BuffType = BuffType.DAMAGE,
            BuffAddType = BuffAddType.REPLACE_EXISTING
        };
        public StatsModifier StatsModifier { get; private set; } = new StatsModifier();
        public void OnActivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {
            Target = unit;
            Ult = ownerSpell;
            Zed = ownerSpell.CastInfo.Owner as Champion;
            AddParticleTarget(Zed, Target, "Zed_Base_R_tar_Impact.troy", Target, 10f);
            AddParticleTarget(Zed, Target, "Zed_Base_R_tar_DelayedDamage.troy", Target, lifetime: 3f);
            AddParticleTarget(Zed, Target, "Zed_Ult_DashEnd.troy", Target);
            ApiEventManager.OnTakeDamage.AddListener(this, Target, TakeDamage, false);
            if (Zed.Stats.AttackDamage.Total >= Target.Stats.CurrentHealth)
            {
                if (a != null)
                {
                }
                else
                {
                    Int = "Zed_Base_R_Tar_pop_Kill.troy";
                    a = AddParticleTarget(Zed, Target, "Zed_Base_R_buf_tell.troy", Target, 10);
                }
            }
        }
        public void TakeDamage(DamageData damageData)
        {
            if (damageData.Attacker == Zed)
            {
                findamage += damageData.Damage;
                Aidamage += damageData.Damage;
            }
        }
        public void OnDeactivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {
            RemoveParticle(a);
            damage = Zed.Stats.AttackDamage.Total;
            var A = Target.Stats.Armor.Total;
            AI = A / (100f + A);
            percdamage = ((Zed.Spells[3].CastInfo.SpellLevel * 15f) + 5f) / 100f;
            float finaldamage = findamage * percdamage;
            Maxdamage = damage + finaldamage;
            Aidamage = Aidamage - (Aidamage * AI);
            AddParticleTarget(Zed, unit, Int, unit);
            unit.TakeDamage(Zed, Maxdamage, DamageType.DAMAGE_TYPE_PHYSICAL, DamageSource.DAMAGE_SOURCE_SPELL, false);
        }
        public void OnUpdate(float diff)
        {
            t += diff;
            if (t > 0)
            {
                if (Aidamage >= Target.Stats.CurrentHealth)
                {
                    Int = "Zed_Base_R_Tar_pop_Kill.troy";
                    if (a != null)
                    {
                    }
                    else
                    {
                        a = AddParticleTarget(Zed, Target, "Zed_Base_R_buf_tell.troy", Target, 10);
                    }
                }
                else
                {
                    Int = "Zed_Base_R_Tar_pop_noKill.troy";
                }
                t = 0;
            }
        }
    }
}