using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using LeagueSandbox.GameServer.GameObjects.SpellNS;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.API;
using GameServerLib.GameObjects.AttackableUnits;

namespace CharScripts
{

    public class CharScriptAhri : ICharScript

    {
        Spell Spell;
        AttackableUnit Target;
        ObjAIBase Owner;
        public void OnActivate(ObjAIBase owner, Spell spell)
        {
            Spell = spell;
            Owner = owner;
            ApiEventManager.OnHitUnit.AddListener(this, owner, OnHitUnit, false);
            AddParticleTarget(owner, owner, "Ahri_Orb.troy", owner, lifetime: 9999999f, bone: "r_hand");
        }


        public void OnHitUnit(DamageData damageData)
        {

        }

        public void OnDeactivate(ObjAIBase owner, Spell spell = null)
        {
        }
        public void OnUpdate(float diff)
        {
        }
    }
}