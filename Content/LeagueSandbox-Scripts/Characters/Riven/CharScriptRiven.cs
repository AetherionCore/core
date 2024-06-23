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
    public class CharScriptRiven : ICharScript
    {
        ObjAIBase Riven;
        AttackableUnit Target;
        public void OnActivate(ObjAIBase owner, Spell spell = null)
        {
            Riven = owner as Champion;
            for (byte i = 0; i < 4; i++)
            {
                ApiEventManager.OnSpellCast.AddListener(this, Riven.Spells[i], AddPassiveBuff);
            }
        }
        public void AddPassiveBuff(Spell spell)
        {
            AddBuff("RivenPassiveAABoost", 6f, 1, spell, Riven, Riven);
        }
    }
}