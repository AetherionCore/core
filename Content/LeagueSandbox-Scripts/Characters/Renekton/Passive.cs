using GameServerCore.Enums;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using LeagueSandbox.GameServer.GameObjects;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.GameObjects.SpellNS;
using LeagueSandbox.GameServer.GameObjects.StatsNS;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.Scripting.CSharp;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using System.Numerics;
using LeagueSandbox.GameServer.GameObjects.SpellNS.Missile;
using LeagueSandbox.GameServer.GameObjects.SpellNS.Sector;
using LeagueSandbox.GameServer.API;

namespace CharScripts
{
    public class CharScriptRenekton : ICharScript
    {
		Spell Spell;
		int counter;
		public StatsModifier StatsModifier { get; private set; } = new StatsModifier();
        public void OnActivate(ObjAIBase owner, Spell spell = null)
        {
			Spell = spell;
			var Health = owner.Stats.CurrentMana * 1f;
			var Blood = owner.Stats.ManaPoints.Total * 0.5f;
			var Health2 = owner.Stats.CurrentMana;       
            if (Health2 >= Blood)
			{
				//AddBuff("", 4.0f, 1, spell, owner, owner);
			}			
			else
			{
			}
			ApiEventManager.OnLaunchAttack.AddListener(this, owner, OnLaunchAttack, false);
            owner.Stats.CurrentMana -= Health; 	
        }
		public void OnLaunchAttack(Spell spell)        
        {
			var owner = spell.CastInfo.Owner;
            owner.Stats.CurrentMana += 10f;			
        }       
        public void OnDeactivate(ObjAIBase owner, Spell spell = null)
        {
        }
        public void OnUpdate(float diff)
        {
        }
    }
}

