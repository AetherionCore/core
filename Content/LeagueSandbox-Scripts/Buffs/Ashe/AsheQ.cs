using GameServerCore.Enums;
using LeagueSandbox.GameServer.GameObjects.StatsNS;
using LeagueSandbox.GameServer.API;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.Scripting.CSharp;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.GameObjects;
using LeagueSandbox.GameServer.GameObjects.SpellNS;

namespace Buffs
{
    internal class AsheQ : IBuffGameScript
    {
        public BuffScriptMetaData BuffMetaData { get; set; } = new BuffScriptMetaData
        {
            BuffType = BuffType.SLOW
        };

        public StatsModifier StatsModifier { get; private set; } = new StatsModifier();

        public BuffAddType BuffAddType => BuffAddType.REPLACE_EXISTING;
        public int MaxStacks => 1;
        public bool IsHidden => false;
        public void OnActivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {
            var spellLevel = ownerSpell.CastInfo.Owner.GetSpell("FrostShot").CastInfo.SpellLevel * 0.05;
            StatsModifier.MoveSpeed.PercentBonus = (float)(StatsModifier.MoveSpeed.PercentBonus - 0.10f - spellLevel);
            unit.AddStatModifier(StatsModifier);
            AddParticleTarget(ownerSpell.CastInfo.Owner, unit, "Ashe_Base_W_tar.troy", unit, 1f);
            // ApplyAssistMarker
        }

        public void OnDeactivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {
        }

        public void OnUpdate(float diff)
        {
        }
    }
}