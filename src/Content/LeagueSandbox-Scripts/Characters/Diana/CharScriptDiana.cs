using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using LeagueSandbox.GameServer.GameObjects.SpellNS;
using LeagueSandbox.GameServer.API;

namespace CharScripts
{
    public class CharScriptDiana : ICharScript
    {
        ObjAIBase diana = null;
        float stanceTime = 500;
        float stillTime = 0;
        bool beginStance = false;
        bool stance = false;

        Spell Spell;
        public void OnActivate(ObjAIBase owner, Spell spell = null)
        {
            Spell = spell;
            {
                ApiEventManager.OnLaunchAttack.AddListener(this, owner, OnLaunchAttack, false);
            }
            var ownerSkinID = owner.SkinID;
        }

        public void OnLaunchAttack(Spell spell)
        {
            var owner = Spell.CastInfo.Owner;
            AddBuff("DianaPassive", 4f, 1, Spell, owner, owner);
        }

        public void OnDeactivate(ObjAIBase owner, Spell spell = null)
        {
            ApiEventManager.OnHitUnit.RemoveListener(this);
        }
    }
}