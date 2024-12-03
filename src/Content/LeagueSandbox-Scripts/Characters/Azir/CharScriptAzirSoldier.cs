using GameServerCore.Enums;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using LeagueSandbox.GameServer.Scripting.CSharp;
using System.Numerics;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.API;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using LeagueSandbox.GameServer.GameObjects.SpellNS;
using GameServerLib.GameObjects.AttackableUnits;

namespace CharScripts
{
    public class CharScriptAzirSoldier : ICharScript
    {
        Spell Spell;
        AttackableUnit Target;
        ObjAIBase Owner;
        public void OnActivate(ObjAIBase owner, Spell spell = null)
        {
            Owner = owner;
            ApiEventManager.OnDeath.AddListener(this, owner, OnDeath, true);
        }
        public void OnDeath(DeathData data)
        {
            AddParticleTarget(Owner, Owner, "Azir_Base_W_SoldierTimeout.troy", Owner, 10, 10);
            AddParticleTarget(Owner, Owner, "Azir_Base_W_Soldier_Outline.troy", Owner, 10, 10);
        }
        public void OnDeactivate(ObjAIBase owner, Spell spell = null)
        {
        }
        public void OnUpdate(float diff)
        {
        }
    }
}