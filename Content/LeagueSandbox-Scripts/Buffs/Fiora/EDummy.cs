using GameServerCore.Enums;
using LeagueSandbox.GameServer.GameObjects.StatsNS;
using LeagueSandbox.GameServer.API;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.Scripting.CSharp;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.GameObjects;
using LeagueSandbox.GameServer.GameObjects.SpellNS;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;

namespace Buffs
{
    internal class FioraFlurryDummy : IBuffGameScript
    {
        public BuffScriptMetaData BuffMetaData { get; set; } = new BuffScriptMetaData
        {
            BuffType = BuffType.COMBAT_ENCHANCER
        };

        public StatsModifier StatsModifier { get; private set; } = new StatsModifier();

        Buff thisBuff;
        Particle highlander;
        string particle;

        public void OnActivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {
            thisBuff = buff;
            if (unit is ObjAIBase owner)
            {
                StatsModifier.AttackSpeed.PercentBonus = StatsModifier.MoveSpeed.PercentBonus += 15f / 100f;
                unit.AddStatModifier(StatsModifier);
            }
        }
        public void OnDeactivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {
            RemoveParticle(highlander);
        }

        private void OnAutoAttack(AttackableUnit target, bool isCrit)
        {
        }

        public void OnUpdate(float diff)
        {
        }
    }
}