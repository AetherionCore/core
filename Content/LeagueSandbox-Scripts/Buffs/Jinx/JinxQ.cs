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
    class JinxQ : IBuffGameScript
    {
        public BuffScriptMetaData BuffMetaData { get; set; } = new BuffScriptMetaData
        {
            BuffType = BuffType.COMBAT_ENCHANCER,
            BuffAddType = BuffAddType.REPLACE_EXISTING,
            MaxStacks = 1,
        };

        public StatsModifier StatsModifier { get; private set; } = new StatsModifier();

        ObjAIBase _owner;
        
        public void OnActivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {
            _owner = unit as ObjAIBase;

            //Require method for swaping spell icons: _owner.SetSpellIcon("JinxQ", 0, 0, false);
            ownerSpell.SetSpellToggle(true);
            //_owner.SetAutoAttackSpell("JinxQAttack", false);
            StatsModifier.Range.FlatBonus = CalculateRangeBonus(ownerSpell.CastInfo.SpellLevel);
            StatsModifier.AttackSpeed.PercentBonus = -.1f;

            unit.AddStatModifier(StatsModifier);
        }

        private float CalculateRangeBonus(int spellLevel)
        {
            float bRange;

            switch (spellLevel)
            {
                case 1:
                    bRange = 75f;
                    break;
                case 2:
                    bRange = 100f;
                    break;
                case 3:
                    bRange = 125;
                    break;
                case 4:
                    bRange = 150f;
                    break;
                case 5:
                    bRange = 175f;
                    break;
                default:
                    bRange = 0.0f;
                    break;
            }
            return bRange;
        }

        public void OnDeactivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {
            ownerSpell.SetSpellToggle(false);
        }

        public void OnUpdate(float diff)
        {
        }
    }
}
