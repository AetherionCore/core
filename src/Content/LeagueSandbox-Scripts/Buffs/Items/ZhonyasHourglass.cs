using GameServerCore.Enums;
using LeagueSandbox.GameServer.Scripting.CSharp;
using LeagueSandbox.GameServer.API;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using LeagueSandbox.GameServer.GameObjects.SpellNS;
using GameServerLib.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.GameObjects;
using LeagueSandbox.GameServer.GameObjects.StatsNS;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;

namespace Buffs
{
    internal class WoogletsWitchcap : IBuffGameScript
    {
        Spell Item;
        Particle Gold;
        public BuffScriptMetaData BuffMetaData { get; set; } = new BuffScriptMetaData
        {
            BuffType = BuffType.COMBAT_ENCHANCER,
            BuffAddType = BuffAddType.REPLACE_EXISTING
        };

        public StatsModifier StatsModifier { get; private set; } = new StatsModifier();
        public void OnActivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {
            Item = ownerSpell;
            if (ownerSpell.CastInfo.Owner is Champion c)
            {
                c.PauseAnimation(true);
                c.StopMovement();
                c.SetTargetUnit(null, true);
                SetStatus(c, StatusFlags.Stunned, true);
                SetStatus(c, StatusFlags.Invulnerable, true);
                c.Stats.SetActionState(ActionState.CAN_MOVE, false);
                c.Stats.SetActionState(ActionState.CAN_ATTACK, false);
                buff.SetStatusEffect(StatusFlags.Targetable, false);
                Gold = AddParticleTarget(c, c, "zhonyas_ring", c, buff.Duration);
                Gold = AddParticleTarget(c, c, "zhonyas_cylinder", c, buff.Duration);
                Gold = AddParticleTarget(c, c, "zhonya_ring_self_skin", c, buff.Duration, bone: "head");
                Gold = AddParticleTarget(c, c, "zhonyas_ring_activate", c, buff.Duration, bone: "head");
            }
        }

        public void OnDeactivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {
            RemoveParticle(Gold);
            if (ownerSpell.CastInfo.Owner is Champion c)
            {
                c.PauseAnimation(false);
                SetStatus(c, StatusFlags.Stunned, false);
                SetStatus(c, StatusFlags.Invulnerable, false);
                c.Stats.SetActionState(ActionState.CAN_MOVE, true);
                c.Stats.SetActionState(ActionState.CAN_ATTACK, true);
                buff.SetStatusEffect(StatusFlags.Targetable, true);
            }
        }
        public void OnUpdate(float diff) { Item.CastInfo.Owner.SetTargetUnit(null, true); }
    }
}