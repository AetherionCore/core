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
using LeagueSandbox.GameServer.GameObjects.SpellNS.Missile;
using LeagueSandbox.GameServer.GameObjects.SpellNS.Sector;

namespace Spells
{
    public class MalphiteBasicAttack : ISpellScript
    {
		private AttackableUnit Target = null;
        public SpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
        {
            TriggersSpellCasts = true
            // TODO
        };

        public void OnActivate(ObjAIBase owner, Spell spell)
        {
			ApiEventManager.OnLaunchAttack.AddListener(this, owner, OnLaunchAttack, true);
        }

        public void OnDeactivate(ObjAIBase owner, Spell spell)
        {
        }

        public void OnSpellPreCast(ObjAIBase owner, Spell spell, AttackableUnit target, Vector2 start, Vector2 end)
        {
          Target = target;			
		  if (owner.HasBuff("ObduracyAttack"))
			{
				OverrideAnimation(owner, "Spell2", "Attack1");
			}
			else
			{
				OverrideAnimation(owner, "Attack1", "Spell2");
			}
        }

        public void OnLaunchAttack(Spell spell)
        {
			var owner = spell.CastInfo.Owner;
			float ad = owner.Stats.AttackDamage.Total * 0.6f;
            float damage = 15 + 15 * owner.GetSpell("SeismicShard").CastInfo.SpellLevel + ad;
			if (owner.HasBuff("ObduracyAttack"))
            {
			    spell.CastInfo.Owner.TargetUnit.TakeDamage(owner, damage, DamageType.DAMAGE_TYPE_PHYSICAL, DamageSource.DAMAGE_SOURCE_ATTACK, false);
			    AddParticleTarget(owner, spell.CastInfo.Owner.TargetUnit, "Malphite_Base_CleaveEnragedHit.troy", owner);
				AddParticleTarget(owner, spell.CastInfo.Owner.TargetUnit, "Malphite_Base_CleaveHit.troy", owner);
			}
			else
			{
			}
           
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

    public class MalphiteBasicAttack2 : ISpellScript
    {
		private AttackableUnit Target = null;
        public SpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
        {
            TriggersSpellCasts = true

            // TODO
        };

        public void OnActivate(ObjAIBase owner, Spell spell)
        {
			ApiEventManager.OnLaunchAttack.AddListener(this, owner, OnLaunchAttack, true);
        }

        public void OnDeactivate(ObjAIBase owner, Spell spell)
        {
        }

        public void OnSpellPreCast(ObjAIBase owner, Spell spell, AttackableUnit target, Vector2 start, Vector2 end)
        {
		  Target = target;
          if (owner.HasBuff("ObduracyAttack"))
			{
				OverrideAnimation(owner, "Spell2", "Attack2");
			}
			else
			{
				OverrideAnimation(owner, "Attack2", "Spell2");
			}
        }

        public void OnLaunchAttack(Spell spell)
        {
            var owner = spell.CastInfo.Owner;
			float ad = owner.Stats.AttackDamage.Total * 0.6f;
            float damage = 15 + 15 * owner.GetSpell("Obduracy").CastInfo.SpellLevel + ad;
            if (owner.HasBuff("ObduracyAttack"))
            {
			    spell.CastInfo.Owner.TargetUnit.TakeDamage(owner, damage, DamageType.DAMAGE_TYPE_PHYSICAL, DamageSource.DAMAGE_SOURCE_ATTACK, false);
			    AddParticleTarget(owner, spell.CastInfo.Owner.TargetUnit, "Malphite_Base_CleaveEnragedHit.troy", owner);
				AddParticleTarget(owner, spell.CastInfo.Owner.TargetUnit, "Malphite_Base_CleaveHit.troy", owner);
			}
			else
			{
			}
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
	public class MalphiteCritAttack : ISpellScript
    {
		private AttackableUnit Target = null;
        public SpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
        {
            TriggersSpellCasts = true
            // TODO
        };

        public void OnActivate(ObjAIBase owner, Spell spell)
        {
			ApiEventManager.OnLaunchAttack.AddListener(this, owner, OnLaunchAttack, true);
        }

        public void OnDeactivate(ObjAIBase owner, Spell spell)
        {
        }

        public void OnSpellPreCast(ObjAIBase owner, Spell spell, AttackableUnit target, Vector2 start, Vector2 end)
        { 
          Target = target;		
		  if (owner.HasBuff("ObduracyAttack"))
			{
				OverrideAnimation(owner, "Spell2", "Crit");
			}
			else
			{
				OverrideAnimation(owner, "Crit", "Spell2");
			}
        }

        public void OnLaunchAttack(Spell spell)
        {
			var owner = spell.CastInfo.Owner;
			float ad = owner.Stats.AttackDamage.Total * 0.6f;
            float damag = 15 + 15 * owner.GetSpell("SeismicShard").CastInfo.SpellLevel + ad;
            float damage = damag * 2f;
			if (owner.HasBuff("ObduracyAttack"))
            {
			    spell.CastInfo.Owner.TargetUnit.TakeDamage(owner, damage, DamageType.DAMAGE_TYPE_PHYSICAL, DamageSource.DAMAGE_SOURCE_ATTACK, true);
			    AddParticleTarget(owner, spell.CastInfo.Owner.TargetUnit, "Malphite_Base_CleaveEnragedHit.troy", owner);
				AddParticleTarget(owner, spell.CastInfo.Owner.TargetUnit, "Malphite_Base_CleaveHit.troy", owner);
			}
			else
			{
			}
           
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

