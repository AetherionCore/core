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
using LeagueSandbox.GameServer.API;
using System.Numerics;
using GameServerLib.GameObjects.AttackableUnits;

namespace Buffs
{
    public class LeblancSoulShackleNet : IBuffGameScript
    {
        Buff Net;
        Particle P;
        Particle P2;
        Particle P3;
        bool IsCrit;
        float Damage;
        float BaseDamage;
        float QOrbDamage;
        float ROrbDamage;
        ObjAIBase Leblanc;
        AttackableUnit Unit;
        public BuffScriptMetaData BuffMetaData { get; set; } = new BuffScriptMetaData
        {
            BuffType = BuffType.DAMAGE,
            BuffAddType = BuffAddType.REPLACE_EXISTING
        };


        public StatsModifier StatsModifier { get; private set; } = new StatsModifier();

        public void OnActivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {
            Unit = unit;
            Net = buff;
            Unit.StopMovement();
            SetStatus(Unit, StatusFlags.CanMove, false);
            (Unit as ObjAIBase).SetTargetUnit(null, true);
            Leblanc = ownerSpell.CastInfo.Owner as Champion;
            ApiEventManager.OnDeath.AddListener(this, Unit, OnDeath, true);
            P = AddParticleTarget(Leblanc, Unit, "LeBlanc_Base_E_tar", Unit, buff.Duration, 1);
            P2 = AddParticleTarget(Leblanc, Unit, "LeBlanc_Base_E_buf", Unit, buff.Duration);
            P3 = AddParticleTarget(Leblanc, Unit, "LeBlanc_Base_E_tar_02", Unit, buff.Duration, 1);
            BaseDamage = 15f + (Leblanc.Spells[2].CastInfo.SpellLevel * 25f) + (Leblanc.Stats.AbilityPower.Total * 0.5f);
            QOrbDamage = BaseDamage + 30 + (Leblanc.Spells[0].CastInfo.SpellLevel * 25f) + (Leblanc.Stats.AbilityPower.Total * 0.65f);
            ROrbDamage = BaseDamage + (Leblanc.Spells[3].CastInfo.SpellLevel * 100f) + (Leblanc.Stats.AbilityPower.Total * 0.65f);
            if (Unit.HasBuff("LeblancChaosOrb")) { IsCrit = true; Damage = QOrbDamage; Unit.RemoveBuffsWithName("LeblancChaosOrb"); }
            else if (Unit.HasBuff("LeblancChaosOrbM")) { IsCrit = true; Damage = ROrbDamage; Unit.RemoveBuffsWithName("LeblancChaosOrbM"); }
            else { IsCrit = false; Damage = BaseDamage; }
            Unit.TakeDamage(Leblanc, Damage, DamageType.DAMAGE_TYPE_MAGICAL, DamageSource.DAMAGE_SOURCE_SPELL, IsCrit);
        }
        public void OnDeactivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {
            RemoveParticle(P);
            RemoveParticle(P2);
            RemoveParticle(P3);
            SetStatus(Unit, StatusFlags.CanMove, true);
            if (Unit is Monster M) { M.SetTargetUnit(Leblanc, true); }
            SpellCast(Unit as ObjAIBase, 2, SpellSlotType.SpellSlots, false, Leblanc, Vector2.Zero);
        }
        public void OnDeath(DeathData deathData)
        {
            Net.DeactivateBuff();
        }
    }
}