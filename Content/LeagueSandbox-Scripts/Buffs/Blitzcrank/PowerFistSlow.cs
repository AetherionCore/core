using GameServerCore.Enums;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using LeagueSandbox.GameServer.GameObjects.StatsNS;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.Scripting.CSharp;
using LeagueSandbox.GameServer.GameObjects;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.GameObjects.SpellNS;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using System.Numerics;
using System;

namespace Buffs
{
    internal class PowerFistSlow : IBuffGameScript
    {
        public BuffScriptMetaData BuffMetaData { get; set; } = new BuffScriptMetaData
        {
            BuffType = BuffType.STUN
        };

        public BuffAddType BuffAddType => BuffAddType.STACKS_AND_OVERLAPS;
        public int MaxStacks => 1;
        public bool IsHidden => false;

        public StatsModifier StatsModifier { get; private set; } = new StatsModifier();

        private Particle hitParticle;

        public void OnActivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {
            var randOffset = (float)new Random().NextDouble();
            var randPoint = new Vector2(unit.Position.X + (80.0f), unit.Position.Y + 80.0f);

            var xy = unit as ObjAIBase;
            xy.SetTargetUnit(null);

            ForceMovement(unit, "", randPoint, 90.0f, 80.0f, 20.0f, 0.0f);
            buff.SetStatusEffect(StatusFlags.CanAttack | StatusFlags.CanCast | StatusFlags.CanMove, false);
            // ApplyAssistMarker(ownerSpell.CastInfo.Owner, unit, 10.0f);
            hitParticle = AddParticleTarget(ownerSpell.CastInfo.Owner, unit, "Powerfist_tar.troy", unit, buff.Duration, targetBone: "head", teamOnly: unit.Team, flags: 0);
        }

        public void OnDeactivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {
            buff.SetStatusEffect(StatusFlags.CanMove | StatusFlags.CanAttack | StatusFlags.CanCast, true);
            hitParticle.SetToRemove();
        }

        public void OnUpdate(float diff)
        {
        }
    }
}
