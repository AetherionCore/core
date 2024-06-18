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
    class EvelynnWActive : IBuffGameScript
    {
        public BuffScriptMetaData BuffMetaData { get; set; } = new BuffScriptMetaData
        {
            BuffType = BuffType.HASTE,
            BuffAddType = BuffAddType.RENEW_EXISTING,
            MaxStacks = 1
        };

        public StatsModifier StatsModifier { get; private set; } = new StatsModifier();

        public void OnActivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {
            AddParticleTarget(ownerSpell.CastInfo.Owner, unit, "Evelynn_W_cas", unit, buff.Duration, bone: "BUFFBONE_CSTM_SHIELD_TOP");
            AddParticleTarget(ownerSpell.CastInfo.Owner, unit, "Evelynn_W_active_buf", unit, buff.Duration, bone: "BUFFBONE_GLB_GROUND_LOC");

            var percentMS = new[] { .3f, .4f, .5f, .6f, .7f}[ownerSpell.CastInfo.SpellLevel - 1];
            StatsModifier.MoveSpeed.PercentBonus += percentMS;
            unit.AddStatModifier(StatsModifier);
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
