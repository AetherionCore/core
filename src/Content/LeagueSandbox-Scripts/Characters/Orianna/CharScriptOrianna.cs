using System.Numerics;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using LeagueSandbox.GameServer.Scripting.CSharp;
using GameServerCore.Scripting.CSharp;
using GameServerCore.Enums;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.GameObjects.SpellNS;
using LeagueSandbox.GameServer.GameObjects.SpellNS.Missile;
using LeagueSandbox.GameServer.GameObjects.SpellNS.Sector;
using LeagueSandbox.GameServer.API;
using System.Collections.Generic;
using LeagueSandbox.GameServer.GameObjects;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.Buildings;
using GameServerLib.GameObjects.AttackableUnits;
using LeaguePackets.Game.Common;

namespace CharScripts
{
    public class CharScriptOrianna : ICharScript
    {
        ObjAIBase _orianna;
        Minion _ball;
        private AttackableUnit _passiveTarget = null;
        private AttackableUnit _currentTarget = null;
        Spell _spell;
        Buffs.OriannaBallHandler BallHandler;

        public void OnActivate(ObjAIBase owner, Spell spell = null)
        {
            _orianna = owner;
            _spell = spell;

            AddBuff("ClockworkWinding", 1f, 1, spell, owner, owner, true);

            BallHandler = (AddBuff("OriannaBallHandler", 1.0f, 1, spell, owner, owner, true).BuffScript as Buffs.OriannaBallHandler);
            BallHandler.SetAttachedChampion((Champion)owner);

            //ApiEventManager.OnDeath.AddListener(owner, owner, OnDeath, false);
            ApiEventManager.OnHitUnit.AddListener(this, _orianna, TargetExecute, false);
        }

        private void TargetExecute(DamageData data)
        {
            _currentTarget = data.Target;

            if (_passiveTarget == _currentTarget)
            {
                AddBuff("OrianaPowerDagger", 4f, 1, _spell, _orianna, _orianna);
            }
            else
            {
                _orianna.RemoveBuffsWithName("OrianaPowerDagger");
                _passiveTarget = _currentTarget;
            }
        }

        public void OnUpdate(float diff)
        {
            if (BallHandler.GetBall() == null)
            {
                BallHandler.SpawnBall(_orianna.Position);
            }
        }
    }

    public class CharScriptOriannaNoBall : ICharScript
    {
        ObjAIBase _owner;
        Minion oriannaBall;

        public void OnActivate(ObjAIBase owner, Spell spell = null)
        {
            _owner = owner;
            //ApiEventManager.OnDeath.AddListener(owner, owner, OnDeath, false);
            AddBuff("TheBall", 1f, 1, spell, owner, owner);
            AddBuff("ClockworkWinding", 1f, 1, spell, owner, owner, true);
        }

        public void OnUpdate(float diff)
        {

            foreach (var unit in GetUnitsInRange(_owner.Position, 1290f, true))
            {
                if (unit.HasBuff("OriannaBall") && unit.Team == _owner.Team)
                {
                    oriannaBall = (Minion)unit;
                    oriannaBall.Owner.GetBuffWithName("TheBall").Update(diff);
                    //SetStatus(oriannaBall, StatusFlags.NoRender, true);
                    //oriannaBall.TakeDamage(oriannaBall.Owner, 10000f, DamageType.DAMAGE_TYPE_TRUE, DamageSource.DAMAGE_SOURCE_INTERNALRAW, DamageResultType.RESULT_NORMAL);
                }
            }
        }
    }
}