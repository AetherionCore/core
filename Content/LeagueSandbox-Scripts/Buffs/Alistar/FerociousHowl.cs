using GameServerCore.Enums;
using LeagueSandbox.GameServer.GameObjects.StatsNS;
using LeagueSandbox.GameServer.Scripting.CSharp;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.GameObjects;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.GameObjects.SpellNS;

namespace Buffs
{
    class FerociousHowl : IBuffGameScript
    {
        public BuffScriptMetaData BuffMetaData { get; set; } = new BuffScriptMetaData
        {
            BuffType = BuffType.HASTE
        };

        public BuffAddType BuffAddType => BuffAddType.REPLACE_EXISTING;
        public int MaxStacks => 1;
        public bool IsHidden => false;

        public StatsModifier StatsModifier { get; private set; } = new StatsModifier();

        public void OnActivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {
            var owner = ownerSpell.CastInfo.Owner;

            float AD = 50.0f + 15.0f * ownerSpell.CastInfo.SpellLevel;
            float hpmax = (0.8f + 0.2f * ownerSpell.CastInfo.SpellLevel) * unit.Stats.CurrentHealth;

            //StatsModifier.Size.PercentBonus = StatsModifier.Size.PercentBonus + 1;
            StatsModifier.AttackDamage.FlatBonus += AD;
            //StatsModifier.HealthPoints.FlatBonus += hpmax;
            StatsModifier.Armor.FlatBonus += 100;
            StatsModifier.MagicResist.FlatBonus += 100;
            unit.AddStatModifier(StatsModifier);
            //TODO:make damage reduction
        }

        public void OnDeactivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {
        }

        public void OnUpdate(float diff)
        {
        }
    }
}