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
    public class ZedBasicAttack : ISpellScript
    {
		AttackableUnit Target;
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
			Target = target;
			//ApiEventManager.OnLaunchAttack.AddListener(this, owner, OnLaunchAttack, false);
			float BBlood = target.Stats.HealthPoints.Total * 0.5f;
			float XBlood = target.Stats.CurrentHealth;
			if (BBlood >= XBlood && !Target.HasBuff("ZedPassiveToolTip") && Target.Team != owner.Team && !(Target is ObjBuilding || Target is BaseTurret))
			{
				OverrideAnimation(owner, "attack_passive", "Attack1");
			}
			else
			{
				OverrideAnimation(owner, "Attack1", "attack_passive");
			}
        }
        public void OnLaunchAttack(Spell spell)
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
    public class ZedBasicAttack2 : ISpellScript
    {
		AttackableUnit Target;
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
			Target = target;
			//ApiEventManager.OnLaunchAttack.AddListener(this, owner, OnLaunchAttack, false);
			float BBlood = target.Stats.HealthPoints.Total * 0.5f;
			float XBlood = target.Stats.CurrentHealth;
			if (BBlood >= XBlood && !Target.HasBuff("ZedPassiveToolTip") && Target.Team != owner.Team && !(Target is ObjBuilding || Target is BaseTurret))
			{
				OverrideAnimation(owner, "attack_passive", "Attack2");
			}
			else
			{
				OverrideAnimation(owner, "Attack2", "attack_passive");
			}
        }
        public void OnLaunchAttack(Spell spell)
        {
			var owner = spell.CastInfo.Owner;
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
	public class ZedCritAttack : ISpellScript
    {
		AttackableUnit Target;
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
			Target = target;
			//ApiEventManager.OnLaunchAttack.AddListener(this, owner, OnLaunchAttack, false);
			float BBlood = target.Stats.HealthPoints.Total * 0.5f;
			float XBlood = target.Stats.CurrentHealth;
			if (BBlood >= XBlood && !Target.HasBuff("ZedPassiveToolTip") && Target.Team != owner.Team && !(Target is ObjBuilding || Target is BaseTurret))
			{
				OverrideAnimation(owner, "attack_passive", "Crit");
			}
			else
			{
				OverrideAnimation(owner, "Crit", "attack_passive");
			}
        }
        public void OnLaunchAttack(Spell spell)
        {
			var owner = spell.CastInfo.Owner;
			float BBlood = Target.Stats.HealthPoints.Total * 0.5f;
			float XBlood = Target.Stats.CurrentHealth;
			if (BBlood >= XBlood && !Target.HasBuff("ZedPassiveToolTip") && Target.Team != owner.Team && !(Target is ObjBuilding || Target is BaseTurret))
			{		
				AddBuff("ZedPassiveToolTip", 10f, 1, spell, Target, owner);
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

