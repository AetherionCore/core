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

namespace CharScripts
{
    public class CharScriptLeblanc : ICharScript
    {
        ObjAIBase Leblanc;
        public void OnActivate(ObjAIBase owner, Spell spell = null)
        {
            Leblanc = owner as Champion;
            AddBuff("LeblancPassive", 25000f, 1, spell, Leblanc, Leblanc, true);
        }
    }
}
