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
    class EvelynnWPassive : IBuffGameScript
    {
        public BuffScriptMetaData BuffMetaData { get; set; } = new BuffScriptMetaData
        {
            BuffType = BuffType.HASTE,
            BuffAddType = BuffAddType.STACKS_AND_RENEWS,
            MaxStacks = 4
        };

        public StatsModifier StatsModifier { get; private set; } = new StatsModifier();

        Particle p0;
        Particle p1;

        public void OnActivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {
            p0 = AddParticleTarget(ownerSpell.CastInfo.Owner, unit, "Evelynn_W_cas", unit, buff.Duration, bone: "BUFFBONE_CSTM_SHIELD_TOP");

            var percentMS = 0f;
            var particleStack = "";
            switch (buff.StackCount) {
                case 1:
                    particleStack = "Evelynn_W_passive_01";
                    percentMS = .3f;
                    break;
                case 2:
                    particleStack = "Evelynn_W_passive_02";
                    percentMS = .4f;
                    break;
                case 3:
                    particleStack = "Evelynn_W_passive_03";
                    percentMS = .5f;
                    break;
                case 4:
                    particleStack = "Evelynn_W_passive_04";
                    percentMS = .6f;
                    break;
            }

            p1 = AddParticleTarget(ownerSpell.CastInfo.Owner, unit, particleStack, unit, buff.Duration, bone: "root");
            StatsModifier.MoveSpeed.PercentBonus += percentMS;
            unit.AddStatModifier(StatsModifier);
        }

        public void OnDeactivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {
            RemoveParticle(p0);
            RemoveParticle(p1);
        }

        public void OnPreAttack(Spell spell)
        {

        }

        public void OnUpdate(float diff)
        {
        }
    }
}
