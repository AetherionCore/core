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

namespace Buffs
{
    public class ZedSlow : IBuffGameScript
    {
        public BuffScriptMetaData BuffMetaData { get; set; } = new BuffScriptMetaData
        {
            BuffType = BuffType.COMBAT_ENCHANCER,
            BuffAddType = BuffAddType.REPLACE_EXISTING
        };

        public StatsModifier StatsModifier { get; private set; } = new StatsModifier();
        public StatsModifier StatsModifier2 { get; private set; } = new StatsModifier();

        Particle p;

        public void OnActivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {
            if (buff.StackCount == 1)
            {
                StatsModifier.MoveSpeed.PercentBonus -= 0.15f + 0.05f * (ownerSpell.CastInfo.SpellLevel - 1);
                unit.AddStatModifier(StatsModifier);
            }
            else if (buff.StackCount == 2)
            {
                StatsModifier2.MoveSpeed.PercentBonus -= 0.1f + 0.075f * (ownerSpell.CastInfo.SpellLevel - 1);
                unit.AddStatModifier(StatsModifier2);
            }
            p = AddParticleTarget(ownerSpell.CastInfo.Owner, null, "Global_Slow.troy", unit, buff.Duration);
        }

        public void OnDeactivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {
            if (buff.StackCount == 2)
            {
                unit.RemoveStatModifier(StatsModifier2);
            }
            RemoveParticle(p);
        }

        public void OnUpdate(float diff)
        {
        }
    }
}