using GameServerCore.Enums;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using LeagueSandbox.GameServer.Scripting.CSharp;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using LeagueSandbox.GameServer.GameObjects.SpellNS;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;
using System.Numerics;
using LeagueSandbox.GameServer.API;

namespace Spells

{
    public class AmumuBasicAttack : ISpellScript
    {
        public SpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
        {

        };

        ObjAIBase daowner;
        Spell daspell;
		AttackableUnit Target;

        public void OnActivate(ObjAIBase owner, Spell spell)
        {
        }

        public void OnDeactivate(ObjAIBase owner, Spell spell)
        {
        }

        public void OnSpellPreCast(ObjAIBase owner, Spell spell, AttackableUnit target, Vector2 start, Vector2 end)
        {
			Target = target;
            daspell = spell;
            ApiEventManager.OnLaunchAttack.AddListener(this, daowner, OnLaunchAttack, false);
        }

        public void OnLaunchAttack(Spell spell)
        {

            AddBuff("CursedTouch", 4f, 1, daspell, Target, spell.CastInfo.Owner);
        }

        private void SpellCast(ObjAIBase owner, int v1, SpellSlotType extraSlots, bool v2, AttackableUnit target, Vector2 position)
        {
        }

        private void AddParticleTarget(ObjAIBase owner, AttackableUnit target1, string v, AttackableUnit target2, string bone)
        {
        }

        public void OnSpellCast(Spell spell)
        {
        }

        public void OnSpellPostCast(Spell spell)
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
	public class AmumuBasicAttack2 : ISpellScript
    {
        public SpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
        {

        };
        ObjAIBase daowner;
        Spell daspell;
		AttackableUnit Target;
        public void OnActivate(ObjAIBase owner, Spell spell)
        {
        }

        public void OnDeactivate(ObjAIBase owner, Spell spell)
        {
        }

        public void OnSpellPreCast(ObjAIBase owner, Spell spell, AttackableUnit target, Vector2 start, Vector2 end)
        {
			Target = target;
            daspell = spell;
            ApiEventManager.OnLaunchAttack.AddListener(this, daowner, OnLaunchAttack, false);
        }

        public void OnLaunchAttack(Spell spell)
        {

            AddBuff("CursedTouch", 4f, 1, daspell, Target, spell.CastInfo.Owner);
        }

        private void SpellCast(ObjAIBase owner, int v1, SpellSlotType extraSlots, bool v2, AttackableUnit target, Vector2 position)
        {
        }

        private void AddParticleTarget(ObjAIBase owner, AttackableUnit target1, string v, AttackableUnit target2, string bone)
        {
        }

        public void OnSpellCast(Spell spell)
        {
        }

        public void OnSpellPostCast(Spell spell)
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