using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using LeagueSandbox.GameServer.Scripting.CSharp;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.GameObjects.SpellNS;
using GameServerCore.Enums;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.GameObjects.SpellNS.Missile;
using System.Numerics;

namespace Spells
{
    public class Headbutt : ISpellScript
    {
        public SpellScriptMetadata ScriptMetadata => new SpellScriptMetadata()
        {
            CastingBreaksStealth = true,
            DoesntBreakShields = true,
            TriggersSpellCasts = true,
            IsDamagingSpell = false,
            NotSingleTargetSpell = true
            // TODO
        };

        public static AttackableUnit _target = null;

        public void OnActivate(ObjAIBase owner, Spell spell)
        {
            // ApiEventManager.OnFinishDash.AddListener(this, owner, AlistarPush, false);
        }

        public void OnDeactivate(ObjAIBase owner, Spell spell)
        {
        }

        private ObjAIBase _owner;

        public void OnSpellPreCast(ObjAIBase owner, Spell spell, AttackableUnit target, Vector2 start, Vector2 end)
        {
            AddBuff("Trample", 4.0f, 1, spell, owner, owner);
            var to = Vector2.Normalize(target.Position - owner.Position);
            owner.SetTargetUnit(null);
            ForceMovement(owner, "Spell2", new Vector2(target.Position.X - to.X * 100f, target.Position.Y - to.Y * 100f), 1500, 0, 0, 0);
            _target = target;
            _owner = owner;
        }

        public void OnSpellCast(Spell spell)
        {
        }

        public void AlistarPush(AttackableUnit unit)
        {
            LogDebug("yo");
            FaceDirection(_owner.Position, _target);
            var x = GetPointFromUnit(_target, -600);

            var xy = _target as ObjAIBase;
            xy.SetTargetUnit(null);

            ForceMovement(_target, "run", x, 1500, 0, 0, 0);
        }

        public void OnSpellPostCast(Spell spell)
        {
        }

        public void ApplyEffects(ObjAIBase owner, AttackableUnit target, Spell spell, SpellMissile missile)
        {
        }

        public void OnSpellChannel(Spell spell)
        {
        }

        public void OnSpellChannelCancel(Spell spell, ChannelingStopSource source)
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