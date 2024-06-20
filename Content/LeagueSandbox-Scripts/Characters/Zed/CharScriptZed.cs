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

namespace CharScripts
{
    public class CharScriptZed : ICharScript
    {
        Spell Spell;
		AttackableUnit Target;
        public void OnActivate(ObjAIBase owner, Spell spell = null)

        {
            Spell = spell;
            {
                ApiEventManager.OnLaunchAttack.AddListener(this, owner, OnLaunchAttack, false);
            }
        }
        public void OnLaunchAttack(Spell spell)        
        {
			var owner = spell.CastInfo.Owner;
            Target = spell.CastInfo.Targets[0].Unit;
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
        public void OnDeactivate(ObjAIBase owner, Spell spell = null)
        {
        }
        public void OnUpdate(float diff)
        {
        }
    }
}