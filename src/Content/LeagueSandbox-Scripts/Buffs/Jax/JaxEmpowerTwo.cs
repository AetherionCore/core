using LeagueSandbox.GameServer.GameObjects.StatsNS;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.Scripting.CSharp;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using LeagueSandbox.GameServer.GameObjects;
using LeagueSandbox.GameServer.GameObjects.SpellNS;
﻿using GameServerCore.Enums;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using System.Numerics;
using LeagueSandbox.GameServer.API;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.Buildings;

namespace Buffs
{
    internal class JaxEmpowerTwo : IBuffGameScript
    {
        Particle Cas;
        Particle Buf;
        Buff AttackBuff;
        ObjAIBase Jax;
        float TrueCooldown;
        public BuffScriptMetaData BuffMetaData { get; set; } = new BuffScriptMetaData
        {
            BuffType = BuffType.COMBAT_ENCHANCER,
            BuffAddType = BuffAddType.REPLACE_EXISTING
        };


        public StatsModifier StatsModifier { get; private set; } = new StatsModifier();

        public void OnActivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {
            AttackBuff = buff;
            Jax = ownerSpell.CastInfo.Owner as Champion;
            Jax.SkipNextAutoAttack();
            Jax.CancelAutoAttack(false, true);
            StatsModifier.Range.FlatBonus = 50.0f;
            Jax.AddStatModifier(StatsModifier);
            ApiEventManager.OnPreAttack.AddListener(this, Jax, OnPreAttack, false);
            ApiEventManager.OnLaunchAttack.AddListener(this, Jax, OnLaunchAttack, false);
            SealSpellSlot(Jax, SpellSlotType.SpellSlots, 1, SpellbookType.SPELLBOOK_CHAMPION, true);
            Cas = AddParticleTarget(Jax, Jax, "armsmaster_empower_self_01", Jax, buff.Duration, 1, bone: "BUFFBONE_CSTM_WEAPON_1", "BUFFBONE_CSTM_WEAPON_2");
            Buf = AddParticleTarget(Jax, Jax, "armsmaster_empower_buf", Jax, buff.Duration, 1, bone: "BUFFBONE_CSTM_WEAPON_1", "BUFFBONE_CSTM_WEAPON_2");
        }
        public void OnPreAttack(Spell spell)
        {
            if (AttackBuff != null && AttackBuff.StackCount != 0 && !AttackBuff.Elapsed())
            {
                //PlaySound("Play_vo_Talon_TalonNoxianDiplomacyAttack_OnCast", Jax);
            }
        }

        public void OnLaunchAttack(Spell spell)
        {
            if (AttackBuff != null && AttackBuff.StackCount != 0 && !AttackBuff.Elapsed() && !(Jax.TargetUnit is ObjBuilding || Jax.TargetUnit is BaseTurret))
            {
                Jax.SkipNextAutoAttack();
                SpellCast(Jax, 1, SpellSlotType.ExtraSlots, false, Jax.TargetUnit, Vector2.Zero);
                AttackBuff.DeactivateBuff();
            }
        }
        public void OnDeactivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {
            RemoveParticle(Cas);
            RemoveParticle(Buf);
            if (buff.TimeElapsed >= buff.Duration)
            {
                ApiEventManager.OnPreAttack.RemoveListener(this);
                ApiEventManager.OnLaunchAttack.RemoveListener(this);
            }
            SealSpellSlot(Jax, SpellSlotType.SpellSlots, 1, SpellbookType.SPELLBOOK_CHAMPION, false);
            TrueCooldown = (9 - Jax.Spells[0].CastInfo.SpellLevel) * (1 + Jax.Stats.CooldownReduction.Total);
            Jax.Spells[1].SetCooldown(TrueCooldown, true);
        }
    }
}