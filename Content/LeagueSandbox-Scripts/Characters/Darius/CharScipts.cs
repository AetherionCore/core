using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.API;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using LeagueSandbox.GameServer.GameObjects.SpellNS;
using GameServerLib.GameObjects.AttackableUnits;

namespace CharScripts
{
    public class CharScriptDarius : ICharScript
    {
        Spell Spell;
        AttackableUnit Target;
        ObjAIBase Owner;
        public void OnActivate(ObjAIBase owner, Spell spell)
        {
            Spell = spell;
            Owner = owner;
            ApiEventManager.OnHitUnit.AddListener(this, owner, OnHitUnit, false);
        }

        public void OnHitUnit(DamageData damageData)
        {
            Target = damageData.Target;
            var owner = Owner;
            AddBuff("DariusHemo", 5.100006f, 1, Spell, Target, owner);
        }

        public void OnDeactivate(ObjAIBase owner, Spell spell = null)
        {
        }
        public void OnUpdate(float diff)
        {
        }
    }
}