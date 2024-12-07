using System.Numerics;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using LeagueSandbox.GameServer.Scripting.CSharp;
using GameServerCore.Scripting.CSharp;
using GameServerCore.Enums;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.GameObjects.SpellNS;

namespace Spells
{
    public class EkkoE : ISpellScript
    {
        public SpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
        {
            CastingBreaksStealth = true,
            DoesntBreakShields = true,
            TriggersSpellCasts = true,
            IsDamagingSpell = false,
            NotSingleTargetSpell = true
        };

        public void OnSpellPreCast(ObjAIBase owner, Spell spell, AttackableUnit target, Vector2 start, Vector2 end)
        {
            owner.SetTargetUnit(null, true);
            owner.CancelAutoAttack(false, false);
            SetStatus(owner, StatusFlags.Ghosted, true);
        }

        public void OnSpellCast(Spell spell)
        {
            var owner = spell.CastInfo.Owner;
            var current = new Vector2(owner.Position.X, owner.Position.Y);
            var spellPos = new Vector2(spell.CastInfo.TargetPosition.X, spell.CastInfo.TargetPosition.Z);
            var dist = Vector2.Distance(current, spellPos);

            if (dist > 325.0f)
            {
                dist = 325.0f;
            }

            FaceDirection(spellPos, owner, true);
            var trueCoords = GetPointFromUnit(owner, dist);
            PlayAnimation(owner, "Spell3", 0.26f);
            AddParticleTarget(owner, owner, ".troy", owner, 10f);
            AddParticleTarget(owner, owner, "Ekko_Base_E_Blur.troy", owner, 10f);
            AddParticleTarget(owner, owner, "Ekko_Base_E_Appear.troy", owner, 10f);
            AddParticle(owner, null, "Ekko_Base_E_Roll_Blur.troy", owner.Position, 10f);
            ForceMovement(owner, null, trueCoords, 1250, 0, 0, 0);
            AddBuff("EkkoEAttackBuff", 4.0f, 1, spell, owner, owner);
        }
    }

    public class EkkoEAttack : ISpellScript
    {
        AttackableUnit Target;

        public SpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
        {
            TriggersSpellCasts = true
            // TODO
        };

        public void OnSpellPreCast(ObjAIBase owner, Spell spell, AttackableUnit target, Vector2 start, Vector2 end)
        {
            Target = target;
            AddParticleTarget(owner, owner, "Ekko_Base_E_Blur.troy", owner, 10f);
            AddParticle(owner, null, "Ekko_Base_E_Appear.troy", owner.Position, 10f);
            AddParticle(owner, owner, "Ekko_Base_E_Roll_Blur.troy", owner.Position, 10f);
            AddParticleTarget(owner, owner, "Ekko_Base_E_Roll_Blur.troy", owner, 10f);
            AddParticle(owner, owner, "Ekko_Base_E_Blur.troy", owner.Position, 10f);
        }

        public void OnSpellCast(Spell spell)
        {
            var owner = spell.CastInfo.Owner;
            var dist = System.Math.Abs(Vector2.Distance(Target.Position, owner.Position));
            var distt = dist - 125f;
            var truepos = GetPointFromUnit(owner, distt);
            var ap = owner.Stats.AbilityPower.Total * 0.6f;
            var damage = 30f + owner.GetSpell("EkkoE").CastInfo.SpellLevel * 10f + ap;
            Target.TakeDamage(owner, damage, DamageType.DAMAGE_TYPE_MAGICAL, DamageSource.DAMAGE_SOURCE_SPELL, false);
            AddParticle(owner, null, ".troy", owner.Position, lifetime: 10f);
            AddParticleTarget(owner, Target, "ekko_base_e_tar.troy", Target);
            TeleportTo(owner, truepos.X, truepos.Y);
        }
    }
}