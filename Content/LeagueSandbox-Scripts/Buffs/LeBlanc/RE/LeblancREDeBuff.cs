using GameServerCore.Enums;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.Scripting.CSharp;
using System;
using LeagueSandbox.GameServer.GameObjects;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using LeagueSandbox.GameServer.GameObjects.SpellNS;
using LeagueSandbox.GameServer.GameObjects.StatsNS;
using LeagueSandbox.GameServer.API;
using System.Numerics;

namespace Buffs
{
    internal class LeblancREDeBuff : IBuffGameScript
    {
        public BuffScriptMetaData BuffMetaData { get; set; } = new BuffScriptMetaData
        {
            BuffType = BuffType.COMBAT_DEHANCER,
            BuffAddType = BuffAddType.REPLACE_EXISTING
        };

        public StatsModifier StatsModifier { get; private set; }

        private Particle buff1;
        private Particle buff2;

        public void OnActivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {
            var caster = ownerSpell.CastInfo.Owner;
            unit.StopMovement();
            if (unit is ObjAIBase ai)
            {
                ai.SetTargetUnit(null, true);
            }

            buff1 = AddParticleTarget(caster, unit, "LeBlanc_Base_RE_tar", unit, 1.5f);
            buff2 = AddParticleTarget(caster, unit, "", unit);
            unit.Stats.SetActionState(ActionState.CAN_MOVE, false);
        }

        public void OnDeactivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {
            RemoveParticle(buff1);
            RemoveParticle(buff2);
            unit.Stats.SetActionState(ActionState.CAN_MOVE, true);
            if (unit is Monster ai && buff.SourceUnit is Champion ch)
            {
                ai.SetTargetUnit(ch, true);
            }
        }

        public void OnUpdate(float diff)
        {
        }
    }
}