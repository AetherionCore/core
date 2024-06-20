using GameServerCore.Enums;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.Scripting.CSharp;
using System;
using LeagueSandbox.GameServer.GameObjects;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using LeagueSandbox.GameServer.GameObjects.SpellNS;
using LeagueSandbox.GameServer.GameObjects.StatsNS;

namespace Buffs
{
    internal class LeblancSoulShackle : IBuffGameScript
    {
        public BuffScriptMetaData BuffMetaData { get; set; } = new BuffScriptMetaData
        {
            BuffType = BuffType.COMBAT_DEHANCER,
            BuffAddType = BuffAddType.REPLACE_EXISTING
        };

        public StatsModifier StatsModifier { get; private set; }

        Particle p;
        Particle p2;
        Spell spell;

        public void OnActivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {
            p = AddParticleTarget(ownerSpell.CastInfo.Owner, unit, "LeBlanc_Base_E_buf", unit, buff.Duration, 1, "CHEST");
            AddParticleTarget(ownerSpell.CastInfo.Owner, unit, "LeBlanc_Base_E_indicator", unit, 10f, 1, "C_BuffBone_Glb_Center_Loc");
            p2 = AddParticleTarget(ownerSpell.CastInfo.Owner, unit, "", unit, buff.Duration);
            //TODO: Find the overhead particle effects
        }

        public void OnDeactivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {
            var owner = ownerSpell.CastInfo.Owner;
            var spellLevel = owner.GetSpell("LeblancSoulShackle").CastInfo.SpellLevel;
            var AP = owner.Stats.AbilityPower.Total * 0.5f;
            var MAXAP = owner.Stats.AbilityPower.Total * 0.65f;
            var QLevel = owner.GetSpell("LeblancChaosOrb").CastInfo.SpellLevel;
            var RQLevel = owner.GetSpell("LeblancSoulShackle").CastInfo.SpellLevel;
            var damage = 40 + 25f * (spellLevel - 1) + AP;
            var damagemax = 55 + 25f * (QLevel - 1) + AP;
            var QMarkdamage = damage + damagemax;
            var damagemaxx = 100 + 100f * (spellLevel - 1) + MAXAP;
            var RQMarkdamage = damage + damagemaxx;
            if (unit.HasBuff("LeblancChaosOrb"))
            {
                unit.RemoveBuffsWithName("LeblancChaosOrb");
                unit.TakeDamage(owner, QMarkdamage, DamageType.DAMAGE_TYPE_MAGICAL, DamageSource.DAMAGE_SOURCE_ATTACK, true);
            }
            else if (unit.HasBuff("LeblancChaosOrbM"))
            {
                unit.RemoveBuffsWithName("LeblancChaosOrbM");
                unit.TakeDamage(owner, RQMarkdamage, DamageType.DAMAGE_TYPE_MAGICAL, DamageSource.DAMAGE_SOURCE_ATTACK, true);
            }
            else
            {
                unit.TakeDamage(owner, damage, DamageType.DAMAGE_TYPE_MAGICAL, DamageSource.DAMAGE_SOURCE_ATTACK, false);
            }
            AddBuff("LeblancEDeBuff", 1.5f, 1, ownerSpell, unit, owner);
            AddParticleTarget(owner, unit, "LeBlanc_Base_Q_tar", unit);
            AddParticleTarget(owner, unit, "LeBlanc_Base_E_buf", unit);
            AddParticleTarget(owner, unit, "", unit);
            AddParticleTarget(owner, unit, "LeBlanc_Base_E_tar_02", unit);
            RemoveParticle(p);
            RemoveParticle(p2);
        }

        public void OnPreAttack(Spell spell)
        {
        }

        public void OnUpdate(float diff)
        {
        }
    }
}