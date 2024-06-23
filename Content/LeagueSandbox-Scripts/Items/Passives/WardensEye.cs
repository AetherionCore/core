using GameServerCore.Enums;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.API;
using LeagueSandbox.GameServer.GameObjects;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using LeagueSandbox.GameServer.GameObjects.SpellNS;
using LeagueSandbox.GameServer.GameObjects.StatsNS;
using LeagueSandbox.GameServer.Scripting.CSharp;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using GameServerLib.GameObjects.AttackableUnits;

namespace ItemPassives
{
    public class ItemID_1503 : IItemScript
    {
        public StatsModifier StatsModifier { get; private set; } = new StatsModifier();
        Region revealStealthed;
        public void OnActivate(ObjAIBase owner)
        {
            // UNIQUE Passive - True Sight: Can see invisible units within 1000 units.
            revealStealthed = AddUnitPerceptionBubble(owner, 1000.0f, 25000f, owner.Team, true);
            ApiEventManager.OnDeath.AddListener(this, owner, OnTowerDeath);
        }
        public void OnTowerDeath(DeathData death)
        {
            revealStealthed.SetToRemove();
        }
    }
}