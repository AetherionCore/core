using GameServerCore.Enums;
using GameServerCore.Scripting.CSharp;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using LeagueSandbox.GameServer.API;
using LeagueSandbox.GameServer.GameObjects;
using GameServerLib.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using LeagueSandbox.GameServer.GameObjects.SpellNS;

namespace CharScripts
{
    internal class CharScriptDragon : ICharScript
    {
        public void OnActivate(ObjAIBase owner, Spell spell = null)
        {
            AddBuff("ResistantSkin", 25000.0f, 1, null, owner, owner, false);

            if (owner is Monster)
            {
                ApiEventManager.OnDeath.AddListener(this, owner, OnDeath, true);
            }
        }

        public void OnDeath(DeathData deathData)
        {
            foreach (var player in GetAllPlayersFromTeam(deathData.Killer.Team))
            {
                player.AddGold(player, 130);
                /*
                 * https://gamefaqs.gamespot.com/boards/954437-league-of-legends/57390708
                 * Dragon = 650 gold for the team, that won't be there all game and is available to both teams.
                 * 650 / 5 = 130
                 */
            }
        }
    }
}