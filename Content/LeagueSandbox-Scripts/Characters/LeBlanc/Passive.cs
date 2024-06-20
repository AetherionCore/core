using GameServerCore.Enums;
using LeagueSandbox.GameServer.GameObjects.StatsNS;
using LeagueSandbox.GameServer.API;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.Scripting.CSharp;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.GameObjects;
using LeagueSandbox.GameServer.GameObjects.SpellNS;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;

namespace Buffs
{
    internal class CharScriptLeblanc : ICharScript
    {
        Spell Spell;
        int counter;
        public StatsModifier StatsModifier { get; private set; } = new StatsModifier();
        public void OnActivate(ObjAIBase owner, Spell spell = null)
        {
            Spell = spell;
            AddBuff("LeblancPassive", 25000f, 1, Spell, Spell.CastInfo.Owner, Spell.CastInfo.Owner, true);
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
