using GameServerCore.Enums;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.GameObjects;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.GameObjects.SpellNS;
using LeagueSandbox.GameServer.GameObjects.StatsNS;
using LeagueSandbox.GameServer.Scripting.CSharp;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;

namespace Buffs
{
    internal class RunePrison : IBuffGameScript
    {
        public BuffScriptMetaData BuffMetaData { get; set; } = new BuffScriptMetaData
        {
            BuffType = BuffType.COMBAT_DEHANCER,
            BuffAddType = BuffAddType.REPLACE_EXISTING,
            MaxStacks = 1
        };

        public StatsModifier StatsModifier { get; private set; } = new StatsModifier();

        private Particle buff1;
        private Particle buff2;

        public void OnActivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {
            var caster = ownerSpell.CastInfo.Owner;
            buff1 = AddParticleTarget(caster, unit, "RunePrison_cas.troy", unit);
            buff2 = AddParticleTarget(caster, unit, "RunePrison_tar.troy", unit);
            unit.SetStatus(StatusFlags.CanMove, false);
            unit.SetStatus(StatusFlags.Rooted, true);
            unit.StopMovement();
        }

        public void OnDeactivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {
            RemoveParticle(buff1);
            RemoveParticle(buff2);
            unit.SetStatus(StatusFlags.CanMove, true);
            unit.SetStatus(StatusFlags.Rooted, false);
        }

        public void OnUpdate(float diff)
        {
        }
    }
}