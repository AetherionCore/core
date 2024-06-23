using LeagueSandbox.GameServer.GameObjects.StatsNS;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.Scripting.CSharp;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using LeagueSandbox.GameServer.GameObjects;
using LeagueSandbox.GameServer.GameObjects.SpellNS;
using GameServerCore.Enums;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using LeagueSandbox.GameServer.GameObjects;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.GameObjects.SpellNS;
using LeagueSandbox.GameServer.GameObjects.StatsNS;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.Scripting.CSharp;

namespace Buffs
{
    internal class JaxRelentlessAssault : IBuffGameScript
    {
        Particle Buf;
        ObjAIBase Jax;
        public BuffScriptMetaData BuffMetaData { get; set; } = new BuffScriptMetaData
        {
            BuffType = BuffType.COMBAT_ENCHANCER,
            BuffAddType = BuffAddType.REPLACE_EXISTING
        };


        public StatsModifier StatsModifier { get; private set; } = new StatsModifier();

        public void OnActivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {
            Jax = ownerSpell.CastInfo.Owner as Champion;
            Buf = AddParticleTarget(Jax, Jax, "JaxRelentlessAssault_buf.troy", Jax, buff.Duration, 1f);
            StatsModifier.Armor.FlatBonus = 0.3f * Jax.Stats.AttackDamage.FlatBonus;
            StatsModifier.MagicResist.FlatBonus = 0.2f * Jax.Stats.AbilityPower.FlatBonus;
            StatsModifier.Armor.PercentBonus = 0.25f + (0.1f * Jax.Spells[3].CastInfo.SpellLevel);
            StatsModifier.MagicResist.PercentBonus = 0.25f + (0.1f * Jax.Spells[3].CastInfo.SpellLevel);
            Jax.AddStatModifier(StatsModifier);
        }
        public void OnDeactivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {
            RemoveParticle(Buf);
        }
    }
}