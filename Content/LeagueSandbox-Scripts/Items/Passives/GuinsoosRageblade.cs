using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.GameObjects.StatsNS;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.GameObjects.SpellNS;
using GameServerCore.Enums;
using LeagueSandbox.GameServer.API;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.Buildings;
using System.Numerics;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;

namespace ItemPassives
{
    public class ItemID_3124 : IItemScript
    {
        ObjAIBase Itemowner;
        public StatsModifier StatsModifier { get; private set; } = new StatsModifier();
        public void OnActivate(ObjAIBase owner)
        {
            Itemowner = owner as Champion;
            ApiEventManager.OnLaunchAttack.AddListener(this, owner, OnLaunchAttack, false);
        }
        public void OnLaunchAttack(Spell spell)
        {
            AddBuff("Rageblade", 8f, 1, spell, Itemowner, Itemowner);
        }
        public void OnDeactivate(ObjAIBase owner)
        {
            ApiEventManager.OnHitUnit.RemoveListener(this);
        }
    }
}