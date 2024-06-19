using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using LeagueSandbox.GameServer.Scripting.CSharp;
using GameServerCore.Enums;
using LeagueSandbox.GameServer.API;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using LeagueSandbox.GameServer.GameObjects.SpellNS;
using LeagueSandbox.GameServer.GameObjects.SpellNS.Missile;
using LeagueSandbox.GameServer.GameObjects.SpellNS.Sector;
using System.Numerics;

namespace Spells
{
    public class FioraDance : ISpellScript
    {
        AttackableUnit Target;
        public SpellScriptMetadata ScriptMetadata => new SpellScriptMetadata()
        {
            TriggersSpellCasts = true,
            IsDamagingSpell = true,
        };

        public void OnActivate(ObjAIBase owner, Spell spell)
        {
        }
        public void TargetExecute(Spell spell, AttackableUnit target, SpellMissile missile, SpellSector sector)
        {
        }

        public void OnDeactivate(ObjAIBase owner, Spell spell)
        {
        }

        public void OnSpellPreCast(ObjAIBase owner, Spell spell, AttackableUnit target, Vector2 start, Vector2 end)
        {
            Target = target;
            AddBuff("FioraDanceStrike", 0.8f, 1, spell, owner, owner);
        }

        public void OnSpellCast(Spell spell)
        {
        }

        public void OnSpellPostCast(Spell spell)
        {
            var owner = spell.CastInfo.Owner as Champion;
            SpellCast(owner, 1, SpellSlotType.ExtraSlots, false, owner.TargetUnit, Vector2.Zero);
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
    public class FioraDanceStrike : ISpellScript
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
            var APratio = owner.Stats.AttackDamage.Total * 0.5f;
            var damage = 125 * (spell.CastInfo.SpellLevel) + APratio;
            var damage2 = damage * 0.4f;
            var dist = System.Math.Abs(Vector2.Distance(Target.Position, owner.Position));
            var distt = dist + 300;
            var dist2 = dist - 300;
            var targetPos = GetPointFromUnit(owner, distt);
            var targetPos2 = GetPointFromUnit(owner, dist2);
            var o = owner.Position;
            var xx = GetClosestUnitInRange(Target, 300, true);
            var yy = GetClosestUnitInRange(xx, 300, true);
            var zz = GetClosestUnitInRange(yy, 300, true);
            PlayAnimation(owner, "spell4a");
            AddParticleTarget(owner, owner, "Fiora_Dance_cas.troy", owner);
            TeleportTo(owner, targetPos.X, targetPos.Y);
            AddParticleTarget(owner, Target, "Fiora_Dance_tar.troy", Target, 1f, 1f);
            AddParticleTarget(owner, owner, ".troy", owner, 1f, 1f);
            Target.TakeDamage(owner, damage, DamageType.DAMAGE_TYPE_PHYSICAL, DamageSource.DAMAGE_SOURCE_SPELL, false);
            FaceDirection(targetPos, owner, true);
            AddParticleTarget(owner, owner, ".troy", owner);
            AddParticleTarget(owner, owner, "Fiora_Dance_windup.troy", owner);
            var randPoint1 = new Vector2(owner.Position.X + (40.0f), owner.Position.Y + 40.0f);
            AddParticleTarget(owner, owner, "Fiora_Dance_windup.troy", owner, 10f);
            ForceMovement(owner, null, randPoint1, 20, 0, 150, 0);
            CreateTimer((float)0.3, () =>
            {
                owner.SetDashingState(false);
                PlayAnimation(owner, "spell4f");
                var Pos = GetPointFromUnit(owner, 600);
                //TeleportTo(owner, Target.Position.X, Target.Position.Y);  			
                AddParticleTarget(owner, owner, "Fiora_Dance_windup.troy", owner, 10f);
                AddParticleTarget(owner, Target, "Fiora_Dance_tar.troy", Target, 1f, 1f);
                AddParticleTarget(owner, owner, ".troy", owner, 1f, 1f);
                Target.TakeDamage(owner, damage2, DamageType.DAMAGE_TYPE_PHYSICAL, DamageSource.DAMAGE_SOURCE_SPELL, false);
                TeleportTo(owner, targetPos2.X, targetPos2.Y);
            });
            CreateTimer((float)0.5, () =>
            {
                owner.SetDashingState(false);
                PlayAnimation(owner, "spell4a");
                AddParticleTarget(owner, owner, "Fiora_Dance_windup.troy", owner, 10f);
                var randPoint2 = new Vector2(owner.Position.X + (40.0f), owner.Position.Y + 40.0f);
                ForceMovement(owner, null, randPoint2, 20, 0, 150, 0);
            });
            CreateTimer((float)0.7, () =>
            {
                owner.SetDashingState(false);
                PlayAnimation(owner, "spell4f");
                //TeleportTo(owner, Target.Position.X, Target.Position.Y);  			
                AddParticleTarget(owner, owner, "Fiora_Dance_windup.troy", owner, 10f);
                AddParticleTarget(owner, Target, "Fiora_Dance_tar.troy", Target, 1f, 1f);
                AddParticleTarget(owner, owner, ".troy", owner, 1f, 1f);
                Target.TakeDamage(owner, damage2, DamageType.DAMAGE_TYPE_PHYSICAL, DamageSource.DAMAGE_SOURCE_SPELL, false);
                TeleportTo(owner, targetPos.X, targetPos.Y);
            });
            CreateTimer((float)0.9, () =>
            {
                owner.SetDashingState(false);
                PlayAnimation(owner, "spell4c");
                AddParticleTarget(owner, owner, "Fiora_Dance_windup.troy", owner, 10f);
            });
            CreateTimer((float)1.1, () =>
            {
                owner.SetDashingState(false);
                PlayAnimation(owner, "spell4a");
                var Pos = GetPointFromUnit(owner, 600);
                //TeleportTo(owner, Target.Position.X, Target.Position.Y);  			
                AddParticleTarget(owner, owner, "Fiora_Dance_windup.troy", owner, 10f);
                AddParticleTarget(owner, Target, "Fiora_Dance_tar.troy", Target, 1f, 1f);
                AddParticleTarget(owner, owner, ".troy", owner, 1f, 1f);
                Target.TakeDamage(owner, damage2, DamageType.DAMAGE_TYPE_PHYSICAL, DamageSource.DAMAGE_SOURCE_SPELL, false);
                TeleportTo(owner, targetPos2.X, targetPos2.Y);
            });
            CreateTimer((float)1.3, () =>
            {
                owner.SetDashingState(false);
                PlayAnimation(owner, "spell4f");
                AddParticleTarget(owner, owner, "Fiora_Dance_windup.troy", owner, 10f);
                //var randPoint2 = new Vector2(owner.Position.X + (40.0f), owner.Position.Y + 40.0f);	
                //ForceMovement(owner, null, randPoint1, 110, 0, 150, 0);
            });
            CreateTimer((float)1.5, () =>
            {
                owner.SetDashingState(false);
                PlayAnimation(owner, "spell4f");
                //TeleportTo(owner, Target.Position.X, Target.Position.Y);  			
                AddParticleTarget(owner, owner, "Fiora_Dance_windup.troy", owner, 10f);
                AddParticleTarget(owner, Target, "Fiora_Dance_tar.troy", Target, 1f, 1f);
                AddParticleTarget(owner, owner, ".troy", owner, 1f, 1f);
                Target.TakeDamage(owner, damage2, DamageType.DAMAGE_TYPE_PHYSICAL, DamageSource.DAMAGE_SOURCE_SPELL, false);
                TeleportTo(owner, Target.Position.X, Target.Position.Y);
            });
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