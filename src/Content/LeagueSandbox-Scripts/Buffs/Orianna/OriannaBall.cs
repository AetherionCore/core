using System.Numerics;
using GameServerCore.Enums;
using GameServerCore.Scripting.CSharp;
using GameServerLib.GameObjects.AttackableUnits;
using LeaguePackets.Game.Common;
using LeaguePackets.Game.Events;
using LeagueSandbox.GameServer.API;
using LeagueSandbox.GameServer.GameObjects;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.Buildings;
using LeagueSandbox.GameServer.GameObjects.SpellNS;
using LeagueSandbox.GameServer.GameObjects.StatsNS;
using LeagueSandbox.GameServer.Scripting.CSharp;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;

namespace Buffs
{
    class OriannaBall : IBuffGameScript
    {
        public BuffScriptMetaData BuffMetaData { get; set; } = new BuffScriptMetaData
        {
            BuffType = BuffType.COMBAT_ENCHANCER,
            BuffAddType = BuffAddType.REPLACE_EXISTING,
            MaxStacks = 1
        };

        ObjAIBase _owner;
        Buff ThisBuff;
        Buffs.OriannaBallHandler BallHandler;
        Minion _ball;
        Particle currentIndicator;
        int previousIndicatorState;

        public StatsModifier StatsModifier { get; private set; } = new StatsModifier();

        public void OnActivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {
            _owner = ownerSpell.CastInfo.Owner;
            ThisBuff = buff;
            BallHandler = (_owner.GetBuffWithName("OriannaBallHandler").BuffScript as Buffs.OriannaBallHandler);
            _ball = unit as Minion;

            buff.SetStatusEffect(StatusFlags.Targetable, false);
            buff.SetStatusEffect(StatusFlags.Ghosted, true);

            AddParticleTarget(_ball.Owner, _ball, "zed_base_w_tar", _ball);

            currentIndicator = AddParticleTarget(_ball.Owner, _ball.Owner, "OrianaBallIndicatorFar", _ball, 5f, flags: FXFlags.TargetDirection);
        }

        public void OnDeactivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {
            if (_ball != null && !_ball.IsDead)
            {
                if (currentIndicator != null)
                {
                    currentIndicator.SetToRemove();
                }

                SetStatus(_ball, StatusFlags.NoRender, true);
                AddParticle(_ball.Owner, null, "zed_base_clonedeath", _ball.Position);
                //Ball.TakeDamage(Ball.Owner, 10000f, DamageType.DAMAGE_TYPE_TRUE, DamageSource.DAMAGE_SOURCE_INTERNALRAW, DamageResultType.RESULT_NORMAL);
            }
        }

        public int GetIndicatorState()
        {
            var dist = Vector2.Distance(_ball.Owner.Position, _ball.Position);
            var state = 0;

            if (!_ball.Owner.HasBuff("TheBall"))
            {
                return state;
            }

            if (dist >= 1290.0f)
            {
                state = 0;
            }
            else if (dist >= 1200.0f)
            {
                state = 1;
            }
            else if (dist >= 1000.0f)
            {
                state = 2;
            }
            else if (dist >= 0f)
            {
                state = 3;
            }

            return state;
        }

        public string GetIndicatorName(int state)
        {
            switch (state)
            {
                case 1:
                    {
                        return "OrianaBallIndicatorFar";
                    }
                case 2:
                    {
                        return "OrianaBallIndicatorMedium";
                    }
                case 3:
                    {
                        return "OrianaBallIndicatorNear";
                    }
                default:
                    {
                        return "OrianaBallIndicatorFar";
                    }
            }
        }

        int state;
        public void OnUpdate(float diff)
        {
            state = GetIndicatorState();

            if (!BallHandler.GetIsAttached())
            {
                if (state == 0)
                {
                    //SpellCast(_owner,3,SpellSlotType.ExtraSlots,true,Ball,Ball.Position);
                }
                else
                {
                    if (state != previousIndicatorState)
                    {
                        previousIndicatorState = state;
                        if (currentIndicator != null)
                        {
                            currentIndicator.SetToRemove();
                        }

                        currentIndicator = AddParticleTarget(_ball.Owner, _ball.Owner, GetIndicatorName(state), _ball, ThisBuff.Duration - ThisBuff.TimeElapsed, flags: FXFlags.TargetDirection);
                    }
                }
            }

        }
    }
}