using GameServerCore.Enums;
using LeagueSandbox.GameServer.GameObjects.StatsNS;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.Scripting.CSharp;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using LeagueSandbox.GameServer.GameObjects;
using LeagueSandbox.GameServer.GameObjects.SpellNS;

namespace Buffs
{
    class JinxQRamp : IBuffGameScript
    {
        public BuffScriptMetaData BuffMetaData { get; set; } = new BuffScriptMetaData
        {
            BuffType = BuffType.COMBAT_ENCHANCER,
            BuffAddType = BuffAddType.STACKS_AND_CONTINUE,
            MaxStacks = 3
        };

        public StatsModifier StatsModifier { get; private set; } = new StatsModifier();

        private ObjAIBase _owner;
        public void OnActivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {
            _owner = ownerSpell.CastInfo.Owner;

            StatsModifier.AttackSpeed.PercentBaseBonus = CalculateBonusAttackSpeed(_owner.GetSpell("JinxQ").CastInfo.SpellLevel);

            _owner.AddStatModifier(StatsModifier);
        }
        public void OnDeactivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {
            //_owner.RemoveStatModifier(StatsModifier);
        }

        private float CalculateBonusAttackSpeed(int spellLevel)
        {
            float bAS;

            switch (spellLevel)
            {
                case 1:
                    bAS = .3f;
                    break;
                case 2:
                    bAS = .55f;
                    break;
                case 3:
                    bAS = .80f;
                    break;
                case 4:
                    bAS = 1.05f;
                    break;
                case 5:
                    bAS = 1.3f;
                    break;
                default:
                    bAS = 0.0f;
                    break;
            }
            return bAS;
        }

        public void OnUpdate(float diff)
        {
        }
    }
}
