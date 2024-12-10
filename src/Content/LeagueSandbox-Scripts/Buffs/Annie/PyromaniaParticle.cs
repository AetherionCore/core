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
    internal class Pyromania_Particle : IBuffGameScript
    {
        public BuffScriptMetaData BuffMetaData { get; set; } = new BuffScriptMetaData
        {
            BuffAddType = BuffAddType.REPLACE_EXISTING,
            BuffType = BuffType.COMBAT_ENCHANCER
        };

        public StatsModifier StatsModifier => new StatsModifier();

        public float StunDuration;

        public void OnActivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {
            var owner = unit;
            if (owner.Stats.Level >= 1)
                StunDuration = 1.25f;
            if (owner.Stats.Level >= 6)
                StunDuration = 1.5f;
            if (owner.Stats.Level >= 11)
                StunDuration = 1.75f;

            LogInfo($"Activating Pyromania_Particle");
            AddParticleTarget(unit, unit, "StunReady.troy", unit, 25000f, bone: "chest");
        }

        public void OnDeactivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {
            RemoveParticleByName(unit.NetId, "StunReady.troy");
        }
    }
}