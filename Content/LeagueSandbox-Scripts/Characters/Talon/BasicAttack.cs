using GameServerCore.Enums;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using LeagueSandbox.GameServer.Scripting.CSharp;
using System.Numerics;
using LeagueSandbox.GameServer.API;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using LeagueSandbox.GameServer.GameObjects.SpellNS;
using LeagueSandbox.GameServer.GameObjects.SpellNS.Missile;
using LeagueSandbox.GameServer.GameObjects.SpellNS.Sector;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.Buildings;

namespace Spells
{
    public class TalonBasicAttack : ISpellScript
    {
		private AttackableUnit Target = null;
        public SpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
        {
            TriggersSpellCasts = true
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
			if (owner.HasBuff("TalonNoxianDiplomacyBuff"))
            {
				OverrideAnimation(owner, "Spell1", "Attack1");
			}
			else
			{
				OverrideAnimation(owner, "Attack1", "Spell1");
			}
            ApiEventManager.OnLaunchAttack.AddListener(this, owner, OnLaunchAttack, true);
        }

        public void OnLaunchAttack(Spell spell)
        {
			var owner = spell.CastInfo.Owner;
			var spellLevel = owner.GetSpell("TalonNoxianDiplomacy").CastInfo.SpellLevel;
            var ADratio = owner.Stats.AttackDamage.Total * 0.3f;
            var damage =(30 * spellLevel) + ADratio;
			var ELevel = owner.GetSpell("TalonCutthroat").CastInfo.SpellLevel;
			var damageamp = 0.03f * (ELevel - 1);         
			var damages = 0.03f * (ELevel - 1);
            var MarkADratio = owner.Stats.AttackDamage.Total * damages;
			if (owner.HasBuff("TalonNoxianDiplomacyBuff"))
            {
			    Target.TakeDamage(owner, damage, DamageType.DAMAGE_TYPE_PHYSICAL, DamageSource.DAMAGE_SOURCE_ATTACK, false);
				if (Target.Team != owner.Team && !(Target is ObjBuilding || Target is BaseTurret))
				{
				AddBuff("TalonBleedDebuff", 6f, 1, spell, Target, owner);
				}
			}
			else
			{
			}
			if (Target.HasBuff("TalonDamageAmp"))
            {
				Target.TakeDamage(owner, MarkADratio, DamageType.DAMAGE_TYPE_PHYSICAL, DamageSource.DAMAGE_SOURCE_ATTACK, false);
			}
			//spell.CastInfo.Owner.SetAutoAttackSpell("TalonBasicAttack2", false);
        }

        public void OnSpellCast(Spell spell)
        {
			var owner = spell.CastInfo.Owner;
        }

        public void OnSpellPostCast(Spell spell)
        {
			var owner = spell.CastInfo.Owner;
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

    public class TalonBasicAttack2 : ISpellScript
    {
		private AttackableUnit Target = null;
        public SpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
        {
            TriggersSpellCasts = true
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
			if (owner.HasBuff("TalonNoxianDiplomacyBuff"))
            {
				OverrideAnimation(owner, "Spell1", "Attack2");
			}
			else
			{
				OverrideAnimation(owner, "Attack2", "Spell1");
			}
            ApiEventManager.OnLaunchAttack.AddListener(this, owner, OnLaunchAttack, true);
        }

        public void OnLaunchAttack(Spell spell)
        {
			var owner = spell.CastInfo.Owner;
			var spellLevel = owner.GetSpell("TalonNoxianDiplomacy").CastInfo.SpellLevel;
            var ADratio = owner.Stats.AttackDamage.Total * 0.3f;
            var damage =(30 * spellLevel) + ADratio;
			var ELevel = owner.GetSpell("TalonCutthroat").CastInfo.SpellLevel;
			var damageamp = 0.03f * (ELevel - 1);         
			var damages = 0.03f * (ELevel - 1);
            var MarkADratio = owner.Stats.AttackDamage.Total * damages;
			if (owner.HasBuff("TalonNoxianDiplomacyBuff"))
            {
			    Target.TakeDamage(owner, damage, DamageType.DAMAGE_TYPE_PHYSICAL, DamageSource.DAMAGE_SOURCE_ATTACK, false);
			    if (Target.Team != owner.Team && !(Target is ObjBuilding || Target is BaseTurret))
				{
				AddBuff("TalonBleedDebuff", 6f, 1, spell, Target, owner);
				}
			}
			else
			{
			}
			if (Target.HasBuff("TalonDamageAmp"))
            {
				Target.TakeDamage(owner, MarkADratio, DamageType.DAMAGE_TYPE_PHYSICAL, DamageSource.DAMAGE_SOURCE_ATTACK, false);
			}
			//spell.CastInfo.Owner.SetAutoAttackSpell("TalonBasicAttack2", false);
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
	public class TalonCritAttack : ISpellScript
    {
		private AttackableUnit Target = null;
        public SpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
        {
            TriggersSpellCasts = true
        };
        public void OnActivate(ObjAIBase owner, Spell spell)
        {
        }

        public void OnDeactivate(ObjAIBase owner, Spell spell)
        {
			owner.SetAutoAttackSpell("TalonBasicAttack2", false);
        }

        public void OnSpellPreCast(ObjAIBase owner, Spell spell, AttackableUnit target, Vector2 start, Vector2 end)
        {
			Target = target;
			if (owner.HasBuff("TalonNoxianDiplomacyBuff"))
            {
				OverrideAnimation(owner, "Spell1", "Crit");
			}
			else
			{
				OverrideAnimation(owner, "Crit", "Spell1");
			}
            ApiEventManager.OnLaunchAttack.AddListener(this, owner, OnLaunchAttack, true);
        }

        public void OnLaunchAttack(Spell spell)
        {
			var owner = spell.CastInfo.Owner;
			var spellLevel = owner.GetSpell("TalonNoxianDiplomacy").CastInfo.SpellLevel;
            var ADratio = owner.Stats.AttackDamage.Total * 0.3f;
            var damage =(30 * spellLevel) + ADratio;
			var ELevel = owner.GetSpell("TalonCutthroat").CastInfo.SpellLevel;
			var damageamp = 0.03f * (ELevel - 1);         
			var damages = 0.03f * (ELevel - 1);
			var damager =damage * 2;
            var MarkADratio = owner.Stats.AttackDamage.Total * damages;
			if (owner.HasBuff("TalonNoxianDiplomacyBuff"))
            {
			    Target.TakeDamage(owner, damager, DamageType.DAMAGE_TYPE_PHYSICAL, DamageSource.DAMAGE_SOURCE_ATTACK, true);
			    if (Target.Team != owner.Team && !(Target is ObjBuilding || Target is BaseTurret))
				{
				AddBuff("TalonBleedDebuff", 6f, 1, spell, Target, owner);
				}
			}
			else
			{
			}
			if (Target.HasBuff("TalonDamageAmp"))
            {
				Target.TakeDamage(owner, MarkADratio, DamageType.DAMAGE_TYPE_PHYSICAL, DamageSource.DAMAGE_SOURCE_ATTACK, false);
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
