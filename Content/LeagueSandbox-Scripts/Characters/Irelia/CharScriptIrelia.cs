using GameServerCore.Enums;
using LeagueSandbox.GameServer.Scripting.CSharp;
using LeagueSandbox.GameServer.API;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using LeagueSandbox.GameServer.GameObjects.SpellNS;
using System.Numerics;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using GameServerLib.GameObjects.AttackableUnits;
using GameServerCore;
using LeagueSandbox.GameServer.GameObjects.SpellNS.Missile;
using LeagueSandbox.GameServer.GameObjects.SpellNS.Sector;

namespace CharScripts
{
    public class CharScriptIrelia : ICharScript
    {
        Spell Passive;
        ObjAIBase Irelia;
        public void OnActivate(ObjAIBase owner, Spell spell = null)
        {
            Passive = spell;
            Irelia = owner as Champion;
            AddBuff("Voracity", 25000f, 1, Passive, Irelia, Irelia);
        }
    }
}