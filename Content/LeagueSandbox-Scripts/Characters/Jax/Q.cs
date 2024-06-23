using System.Numerics;
using GameServerCore.Enums;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using LeagueSandbox.GameServer.Scripting.CSharp;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using LeagueSandbox.GameServer.GameObjects.SpellNS;
using LeagueSandbox.GameServer.GameObjects;
using System.Collections.Generic;
using GameServerCore.Enums;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.Scripting.CSharp;
using LeagueSandbox.GameServer.API;
using System;
using System.Numerics;
using LeagueSandbox.GameServer.GameObjects;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using LeagueSandbox.GameServer.GameObjects.SpellNS;
using LeagueSandbox.GameServer.GameObjects.StatsNS;

namespace Spells
{
    public class JaxLeapStrike : ISpellScript
    {
        float Damage;
        float WDamage;
        ObjAIBase Jax;
        Spell LeapStrike;
        AttackableUnit Target;
        public SpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
        {
            TriggersSpellCasts = true,
            IsDamagingSpell = true
        };
        public void OnSpellPreCast(ObjAIBase owner, Spell spell, AttackableUnit target, Vector2 start, Vector2 end)
        {
            Target = target;
            LeapStrike = spell;
            Jax = owner = spell.CastInfo.Owner as Champion;
            PlayAnimation(Jax, "spell2");
            SetStatus(Jax, StatusFlags.Ghosted, true);
        }

        public void OnSpellPostCast(Spell spell)
        {
            ApiEventManager.OnMoveEnd.AddListener(this, Jax, OnMoveEnd, true);
            ApiEventManager.OnMoveSuccess.AddListener(this, Jax, OnMoveSuccess, true);
            FaceDirection(Target.Position, Jax, true);
            AddParticleTarget(Jax, Jax, "Talon_Base_Q2_cas.troy", Jax);
            ForceMovement(Jax, null, Target.Position, 2200, 0, 100, 0);
        }
        public void OnMoveSuccess(AttackableUnit unit)
        {
            Jax.SetDashingState(false);
            WDamage = (40 * Jax.Spells[1].CastInfo.SpellLevel) + (Jax.Stats.AbilityPower.Total * 0.6f);
            Damage = 55 + (25f * Jax.Spells[0].CastInfo.SpellLevel) + (Jax.Stats.AttackDamage.FlatBonus * 1.2f) + (Jax.Stats.AbilityPower.FlatBonus * 0.6f);
            if (Jax.Team != Target.Team)
            {
                if (Target is Champion)
                {
                    Jax.SkipNextAutoAttack();
                    Jax.CancelAutoAttack(true, false);
                    Jax.SetTargetUnit(Target, true);
                    Jax.UpdateMoveOrder(OrderType.AttackTo, true);
                }
                Target.TakeDamage(Jax, Damage, DamageType.DAMAGE_TYPE_PHYSICAL, DamageSource.DAMAGE_SOURCE_SPELL, false);
                AddParticleTarget(Jax, Target, "jax_leapstrike_tar", Target, 10f, 1f);
            }
            if (Jax.HasBuff("JaxEmpowerTwo"))
            {
                Jax.RemoveBuffsWithName("JaxEmpowerTwo");
                Target.TakeDamage(Jax, WDamage, DamageType.DAMAGE_TYPE_MAGICAL, DamageSource.DAMAGE_SOURCE_ATTACK, false);
            }
        }
        public void OnMoveEnd(AttackableUnit owner)
        {
            Jax.SetDashingState(false);
            SetStatus(Jax, StatusFlags.Ghosted, false);
            //StopAnimation(owner, "spell1", true, true, true);
        }
    }
    public class JaxLeapStrikeAttack : ISpellScript
    {
        public SpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
        {
            TriggersSpellCasts = true,
            IsDamagingSpell = true
        };
    }
}