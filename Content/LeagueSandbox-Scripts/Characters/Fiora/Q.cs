using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using LeagueSandbox.GameServer.Scripting.CSharp;
using GameServerCore.Enums;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using LeagueSandbox.GameServer.GameObjects.SpellNS;
using System.Numerics;

namespace Spells
{
    public class FioraQ : ISpellScript
    {
        AttackableUnit Target;
        public SpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
        {
            TriggersSpellCasts = true,
            IsDamagingSpell = true
        };

        public void OnActivate(ObjAIBase owner, Spell spell)
        {
        }

        public void OnDeactivate(ObjAIBase owner, Spell spell)
        {
        }

        public void OnSpellPreCast(ObjAIBase owner, Spell spell, AttackableUnit target, Vector2 start, Vector2 end)
        {
            Target = target;
            owner.CancelAutoAttack(false);
        }

        public void OnSpellCast(Spell spell)
        {
        }

        public void OnSpellPostCast(Spell spell)
        {
            var owner = spell.CastInfo.Owner;
            if (!owner.HasBuff("FioraQCD"))
            {
                AddBuff("FioraQCD", 4, 1, spell, owner, owner);
            }
            SpellCast(owner, 0, SpellSlotType.ExtraSlots, false, owner.TargetUnit, Vector2.Zero);
        }

        public void OnSpellChannel(Spell spell)
        {
        }

        public void OnSpellChannelCancel(Spell spell, ChannelingStopSource reason)
        {
        }

        public void OnSpellPostChannel(Spell spell)
        {
        }

        public void OnUpdate(float diff)
        {
        }
    }
    public class FioraQLunge : ISpellScript
    {
        AttackableUnit Target;
        public SpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
        {
            TriggersSpellCasts = true,
            IsDamagingSpell = true
        };

        public void OnActivate(ObjAIBase owner, Spell spell)
        {
        }

        public void OnDeactivate(ObjAIBase owner, Spell spell)
        {
        }

        public void OnSpellPreCast(ObjAIBase owner, Spell spell, AttackableUnit target, Vector2 start, Vector2 end)
        {
            Target = target;
            owner.CancelAutoAttack(false);
        }

        public void OnSpellCast(Spell spell)
        {
        }

        public void OnSpellPostCast(Spell spell)
        {
            var owner = spell.CastInfo.Owner;
            var ad = owner.Stats.AttackDamage.Total * 1.2f;
            var damage = 40 + 25 * (owner.GetSpell("FioraQ").CastInfo.SpellLevel - 1) + ad;
            var dist = System.Math.Abs(Vector2.Distance(Target.Position, owner.Position));
            var distt = dist - 1;
            var targetPos = GetPointFromUnit(owner, distt);
            var time = dist / 2200f;
            PlayAnimation(owner, "Spell1", time);
            AddBuff("Ghosted", time, 1, spell, owner, owner);
            CreateTimer((float)time, () =>
            {
                Target.TakeDamage(owner, damage, DamageType.DAMAGE_TYPE_PHYSICAL, DamageSource.DAMAGE_SOURCE_SPELL, false);
                AddParticleTarget(owner, Target, "FioraQLunge_tar.troy", Target, 10f);
            });
            FaceDirection(targetPos, owner, true);
            ForceMovement(owner, null, targetPos, 2200, 0, 0, 0);
            AddParticleTarget(owner, owner, "FioraQLunge_dashtrail.troy", owner, time);
            AddParticleTarget(owner, owner, "Fiora_Dance_windup.troy", owner, time);
        }

        public void OnSpellChannel(Spell spell)
        {
        }

        public void OnSpellChannelCancel(Spell spell, ChannelingStopSource reason)
        {
        }

        public void OnSpellPostChannel(Spell spell)
        {
        }

        public void OnUpdate(float diff)
        {
        }
    }
}