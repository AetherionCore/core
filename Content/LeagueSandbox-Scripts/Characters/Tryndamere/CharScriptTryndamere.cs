using GameServerCore.Scripting.CSharp;
using GameServerLib.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.API;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using LeagueSandbox.GameServer.GameObjects.SpellNS;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;


namespace CharScripts
{

    public class CharScriptTryndamere : ICharScript

    {
        Spell Spell;
        public void OnActivate(ObjAIBase owner, Spell spell)
        {
            Spell = spell;
            {
                AddBuff("BattleFury", 1, 1, spell, owner, owner, true);
                ApiEventManager.OnHitUnit.AddListener(this, owner, OnHitUnit, false);
            }
        }

        public void OnHitUnit(DamageData damageData)
        {
            var owner = Spell.CastInfo.Owner;
            var mana = 5;
            owner.Stats.CurrentMana += mana;
            AddBuff("BattleFury", 1, 1, Spell, owner, owner, true);
        }

        public void OnDeactivate(ObjAIBase owner, Spell spell)
        {
            ApiEventManager.OnHitUnit.RemoveListener(this);
        }
    }
}