using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using LeagueSandbox.GameServer.GameObjects.SpellNS;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.GameObjects.StatsNS;

namespace CharScripts
{
    public class CharScriptAatrox : ICharScript
    {
        Spell Spell;
        int counter;
        public StatsModifier StatsModifier { get; private set; } = new StatsModifier();
        public void OnActivate(ObjAIBase owner, Spell spell = null)
        {
            Spell = spell;
            var Health = owner.Stats.CurrentMana * 1f;
            owner.Stats.CurrentMana -= Health;
            AddBuff("AatroxPassive", 25000f, 1, Spell, Spell.CastInfo.Owner, Spell.CastInfo.Owner, true);
        }
        public void OnLevelUp(AttackableUnit owner)
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
