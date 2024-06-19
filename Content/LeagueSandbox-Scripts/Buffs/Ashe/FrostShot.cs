using GameServerCore.Enums;
using LeagueSandbox.GameServer.GameObjects.StatsNS;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.Scripting.CSharp;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.GameObjects;
using LeagueSandbox.GameServer.GameObjects.SpellNS;

namespace Buffs
{
    internal class FrostShot : IBuffGameScript
    {
        public BuffScriptMetaData BuffMetaData { get; set; } = new BuffScriptMetaData
        {
            BuffType = BuffType.COMBAT_ENCHANCER
        };

        public StatsModifier StatsModifier { get; private set; } = new StatsModifier();

        public BuffAddType BuffAddType => BuffAddType.REPLACE_EXISTING;
        public int MaxStacks => 1;
        public bool IsHidden => false;

        public void OnActivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {
        }

        public void OnDeactivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {
        }

        public void OnPreAttack(Spell spell)
        {
        }

        public void OnUpdate(float diff)
        {
        }
    }
}