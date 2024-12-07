using System.Numerics;
using GameServerCore.Enums;
using GameServerCore.Scripting.CSharp;
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
    internal class EkkoRT : IBuffGameScript
    {
        public BuffScriptMetaData BuffMetaData { get; set; } = new BuffScriptMetaData
        {
            BuffAddType = BuffAddType.REPLACE_EXISTING
        };

        public StatsModifier StatsModifier { get; private set; } = new StatsModifier();

        Minion Ekko;
        Spell Spell;
        ObjAIBase Owner;
        private Buff buff;
        AttackableUnit Unit;
        float m;
        float p;
        float Health;
        float V;
        //Vector2 P;
        Vector2 a;
        Vector2 a2;
        Vector2 a3;
        Vector2 a4;
        Vector2 a5;
        Vector2 a6;

        private readonly Particle P = Buffs.EkkoRInvuln.P;
        private readonly Particle P2 = Buffs.EkkoRInvuln.P2;
        private readonly Particle P3 = Buffs.EkkoRInvuln.P3;
        private readonly Particle P4 = Buffs.EkkoRInvuln.P4;
        private readonly Particle P5 = Buffs.EkkoRInvuln.P5;
        private readonly Particle P6 = Buffs.EkkoRInvuln.P6;
        public void OnActivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {
            Unit = unit;
            Spell = ownerSpell;
            Owner = ownerSpell.CastInfo.Owner;
            V = Owner.Stats.MoveSpeed.Total;
            var pp = P.Position;
            var pp2 = P2.Position;
            var pp3 = P3.Position;
            var pp4 = P4.Position;
            var pp5 = P5.Position;
            var pp6 = P6.Position;
            a = pp;
            a2 = pp2;
            a3 = pp3;
            a4 = pp4;
            a5 = pp5;
            a6 = pp6;
            //ApiEventManager.OnMoveEnd.AddListener(this, Owner, OnMoveEnd, true);
            //Unit.UpdateMoveOrder(OrderType.PetHardMove);
            Unit.SetWaypoints(GetPath(Unit.Position, pp));
            //ForceMovement(Unit, null, pp, V, 0, 0, 0);; m = 0f;

        }
        public void OnMoveEnd(AttackableUnit unit)
        {
            //ForceMovement(Unit, null, pp2, V, 0, 0, 0);; m = 0f;
        }
        public void OnDeactivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {
            if (Spell.CastInfo.Owner is Champion owner)
            {

            }
        }
        public void OnUpdate(float diff)
        {
            p += diff;
            m += diff;
            //if (p >= 600f && Unit != null)
            //{
            //P = Owner.Position;
            //p = 0f;
            //}        
            if (m >= 600.0f && Unit != null)
            {
                Unit.SetWaypoints(GetPath(Unit.Position, a2));
            }
            if (m >= 1200.0f && Unit != null)
            {
                Unit.SetWaypoints(GetPath(Unit.Position, a3));
            }
            if (m >= 1800.0f && Unit != null)
            {
                Unit.SetWaypoints(GetPath(Unit.Position, a4));
            }
            if (m >= 2400.0f && Unit != null)
            {
                Unit.SetWaypoints(GetPath(Unit.Position, a5));
            }
            if (m >= 3200.0f && Unit != null)
            {
                Unit.SetWaypoints(GetPath(Unit.Position, a6));
            }
            if (m >= 3800.0f && Unit != null)
            {
                //ForceMovement(Unit, null,pp6, V, 0, 0, 0);; m = 0f;
                m = 0f;
            }
        }
    }
}