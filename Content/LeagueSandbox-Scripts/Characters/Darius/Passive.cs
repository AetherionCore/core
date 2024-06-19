using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.API;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using LeagueSandbox.GameServer.GameObjects.SpellNS;

namespace Passives
{
    public class DariusHemoMarker : ICharScript
    {
        private Spell Spell;

        public void OnActivate(ObjAIBase owner, Spell spell)
        {
            Spell = spell;
            //ApiEventManager.OnHitUnitByAnother.AddListener(this, OnHitUnit);
        }

        public void OnHitUnit(AttackableUnit target, bool IsCrit)
        {
            var owner = Spell.CastInfo.Owner;
            AddBuff("DariusHemoMarker", 5f, 1, Spell, target, owner); // Assuming AddBuff is a method defined elsewhere in your code
        }

        public void OnDeactivate(ObjAIBase owner, Spell spell)
        {
            //ApiEventManager.OnHitUnitByAnother.RemoveListener(this, OnHitUnit);
        }

        public void OnUpdate(float diff)
        {
            // Implement if needed
        }

        private void AddBuff(string buffName, float duration, int stacks, Spell spell, AttackableUnit target, ObjAIBase owner)
        {
            // Implement your AddBuff logic here
        }
    }
}
