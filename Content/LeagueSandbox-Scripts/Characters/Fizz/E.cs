using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using LeagueSandbox.GameServer.Scripting.CSharp;
using GameServerCore.Enums;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using LeagueSandbox.GameServer.GameObjects.SpellNS;
using System.Numerics;
using LeagueSandbox.GameServer.GameObjects.SpellNS.Missile;
using LeagueSandbox.GameServer.GameObjects.SpellNS.Sector;
using GameServerCore.Scripting.CSharp;

namespace Spells
{
    public class FizzJump : ISpellScript
    {

        public SpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
        {
            TriggersSpellCasts = true
            // TODO
        };

        public void OnActivate(ObjAIBase owner, Spell spell)
        {
        }

        public void OnDeactivate(ObjAIBase owner, Spell spell)
        {
        }

        public void OnSpellPreCast(ObjAIBase owner, Spell spell, AttackableUnit target, Vector2 start, Vector2 end)
        {
        }

        public void OnSpellCast(Spell spell)
        {
            var owner = spell.CastInfo.Owner;
            AddBuff("FizzJump", 1f, 1, spell, owner, owner);
            AddBuff("FizzJumpTwo", 1.5f, 1, spell, owner, owner);
            AddBuff("FizzTrickSlam", 1f, 1, spell, owner, owner);
        }

        public void OnSpellPostCast(Spell spell)
        {	
		    var owner = spell.CastInfo.Owner;
		    spell.SetCooldown(0.1f, true);
			var Cursor = new Vector2(spell.CastInfo.TargetPosition.X, spell.CastInfo.TargetPosition.Z);
            var current = new Vector2(owner.Position.X, owner.Position.Y);
            var distance = Cursor - current;
            Vector2 truecoords;
            if (distance.Length() > 400f)
            {
                distance = Vector2.Normalize(distance);
                var range = distance * 400f;
                truecoords = current + range;
            }
            else
            {
                truecoords = Cursor;
            }
			var dist = System.Math.Abs(Vector2.Distance(truecoords, owner.Position));
            var time = dist / 1400f;
			PlayAnimation(owner, "spell3a");
			owner.Stats.SetActionState(ActionState.CAN_MOVE, false);	 
			AddParticleTarget(owner, owner, ".troy", owner, 0.5f);
			AddParticle(owner, null, ".troy", owner.Position);
			FaceDirection(truecoords, spell.CastInfo.Owner, true);
            ForceMovement(owner, null, truecoords, 1000, 0, 30, 0);
        }
		public void OnUnitUpdateMove(Spell spell)
        {
        }
		public void TargetExecute(Spell spell, AttackableUnit target, SpellMissile missile, SpellSector sector)
        { 
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
	public class FizzJumpTwo : ISpellScript
    {

        public SpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
        {
            TriggersSpellCasts = true
            // TODO
        };

        public void OnActivate(ObjAIBase owner, Spell spell)
        {
        }

        public void OnDeactivate(ObjAIBase owner, Spell spell)
        {
        }

        public void OnSpellPreCast(ObjAIBase owner, Spell spell, AttackableUnit target, Vector2 start, Vector2 end)
        {
        }

        public void OnSpellCast(Spell spell)
        {
        }

        public void OnSpellPostCast(Spell spell)
        {
            var owner = spell.CastInfo.Owner;
			var Cursor = new Vector2(spell.CastInfo.TargetPosition.X, spell.CastInfo.TargetPosition.Z);
            var current = new Vector2(owner.Position.X, owner.Position.Y);
            var distance = Cursor - current;
            Vector2 truecoords;
            if (distance.Length() > 400f)
            {
                distance = Vector2.Normalize(distance);
                var range = distance * 400f;
                truecoords = current + range;
            }
            else
            {
                truecoords = Cursor;
            }
			var dist = System.Math.Abs(Vector2.Distance(truecoords, owner.Position));
            var time = dist / 1400f;
			PlayAnimation(owner, "spell3c");
			owner.Stats.SetActionState(ActionState.CAN_MOVE, false);	 
			AddParticleTarget(owner, owner, ".troy", owner, 0.5f);
			AddParticle(owner, null, ".troy", owner.Position);
			FaceDirection(truecoords, spell.CastInfo.Owner, true);
            ForceMovement(owner, null, truecoords, 1000, 0, 30, 0);			
        }
		public void OnUnitUpdateMove(Spell spell)
        {
        }
		public void TargetExecute(Spell spell, AttackableUnit target, SpellMissile missile, SpellSector sector)
        { 
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