using System.Numerics;
using GameServerCore.Enums;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using LeagueSandbox.GameServer.Scripting.CSharp;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using LeagueSandbox.GameServer.GameObjects.SpellNS;
using LeagueSandbox.GameServer.API;

namespace Spells
{
    public class AkaliShadowDance : ISpellScript
    {
        Spell Spell;
        float Damage;
        float Ampdamage;
        ObjAIBase Akali;
        AttackableUnit Target;
        public SpellScriptMetadata ScriptMetadata => new SpellScriptMetadata() { TriggersSpellCasts = true };
        public void OnSpellPreCast(ObjAIBase owner, Spell spell, AttackableUnit target, Vector2 start, Vector2 end)
        {
            Spell = spell;
            Target = target;
            Akali = owner = spell.CastInfo.Owner as Champion;
        }
        public void OnSpellCast(Spell spell)
        {
            PlayAnimation(Akali, "Spell4", 0.5f);
            SetStatus(Akali, StatusFlags.Ghosted, true);
            AddParticleTarget(Akali, Akali, "akali_shadowDance_mis", Akali);
            AddParticleTarget(Akali, Akali, "akali_shadowDance_return", Akali);
            AddParticle(Akali, null, "akali_shadowDance_return_02", Akali.Position);
            AddParticle(Akali, null, "akali_shadowDance_cas", Akali.Position);
            ApiEventManager.OnMoveEnd.AddListener(this, Akali, OnMoveEnd, true);
            ForceMovement(Akali, null, Target.Position, 2200f, 0, 0f, 0, movementOrdersType: ForceMovementOrdersType.CANCEL_ORDER);
        }

        public void OnMoveEnd(AttackableUnit unit)
        {
            Akali.SetDashingState(false);
            SetStatus(Akali, StatusFlags.Ghosted, false);
            StopAnimation(Akali, "Spell4", true, true, true);
            AddParticleTarget(Akali, Target, "akali_shadowDance_tar", Target);
            Damage = 25 + (Spell.CastInfo.SpellLevel * 75) + (Akali.Stats.AbilityPower.Total * 0.5f);
            Target.TakeDamage(Akali, Damage, DamageType.DAMAGE_TYPE_MAGICAL, DamageSource.DAMAGE_SOURCE_SPELL, false);
            Ampdamage = 20 + (25 * Akali.Spells[0].CastInfo.SpellLevel) + (Akali.Stats.AbilityPower.Total * 0.5f);
            if (Target.HasBuff("AkaliMota"))
            {
                Target.TakeDamage(Akali, Ampdamage, DamageType.DAMAGE_TYPE_MAGICAL, DamageSource.DAMAGE_SOURCE_ATTACK, false);
                AddParticleTarget(Akali, Target, "akali_mark_impact_tar", Target, 10f);
                RemoveBuff(Target, "AkaliMota");
            }
            if (Target.Team != Akali.Team && Target is Champion)
            {
                Akali.SetTargetUnit(Target, true);
                Akali.UpdateMoveOrder(OrderType.AttackTo, true);
            }
        }
    }
}