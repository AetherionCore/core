using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using GameServerCore.Enums;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using LeagueSandbox.GameServer.GameObjects.SpellNS;

namespace Passives
{
    public class FizzPassive : ICharScript
    {

        private Spell originspell;
        private ObjAIBase ownermain;

        public void OnActivate(ObjAIBase owner, Spell spell)
        {
            ownermain = owner;
        }

        public void OnDeactivate(ObjAIBase owner, Spell spell)
        {
        }

        public void OnUpdate(float diff)
        {
            if (!ownermain.Status.HasFlag(StatusFlags.Ghosted))
            {
                ownermain.SetStatus(GameServerCore.Enums.StatusFlags.Ghosted, true); // IDK IF THERE IS ANYTHING THAT REMOVES GHOSTING
                LogDebug("ghosted");
            }
        }
    }
}