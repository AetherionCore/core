using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using LeagueSandbox.GameServer.API;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using LeagueSandbox.GameServer.GameObjects.SpellNS;
using GameServerLib.GameObjects.AttackableUnits;

namespace CharScripts
{
    public class CharScriptTwitch : ICharScript
    {
        Spell Passive;
        ObjAIBase Twitch;
        AttackableUnit Target;
        public void OnActivate(ObjAIBase owner, Spell spell = null)
        {
            Passive = spell;
            Twitch = owner as Champion;
            ApiEventManager.OnHitUnit.AddListener(this, Twitch, OnHitUnit, false);
        }
        public void OnHitUnit(DamageData damageData)
        {  		
            AddBuff("TwitchDeadlyVenom", 6f, 1, Passive, damageData.Target, Twitch);       			
        }
    }
}