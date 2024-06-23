using LeagueSandbox.GameServer.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.GameObjects.StatsNS;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;

namespace ItemPassives
{
    public class ItemID_3031 : IItemScript
    {
        float Damage;
        ObjAIBase Itemowner;
        AttackableUnit Target;
        public StatsModifier StatsModifier { get; private set; } = new StatsModifier();
        public void OnActivate(ObjAIBase owner)
        {
            Itemowner = owner as Champion;
            StatsModifier.CriticalDamage.PercentBonus = 0.25f;
            Itemowner.AddStatModifier(StatsModifier);
        }
    }
}