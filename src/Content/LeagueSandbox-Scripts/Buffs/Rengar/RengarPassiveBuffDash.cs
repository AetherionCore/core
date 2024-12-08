using System.Numerics;
using GameServerCore.Enums;
using GameServerCore.Scripting.CSharp;
using GameServerLib.GameObjects.AttackableUnits;
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
    internal class RengarPassiveBuffDash : IBuffGameScript
    {
        public BuffScriptMetaData BuffMetaData { get; set; } = new BuffScriptMetaData
        {
            BuffAddType = BuffAddType.REPLACE_EXISTING
        };

        public StatsModifier StatsModifier { get; private set; } = new StatsModifier();

        private Buff ThisBuff;
        private Spell Spell;
        AttackableUnit Target;
        private ObjAIBase owner;
        private float ticks = 0;
        private float damage;
        Buff thisBuff;
        Particle P;

        public void OnActivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {
            thisBuff = buff;
            owner = ownerSpell.CastInfo.Owner;
            SetStatus(owner, StatusFlags.Ghosted, true);
            Spell = ownerSpell;
            ApiEventManager.OnMoveEnd.AddListener(this, owner, OnMoveEnd, true);
            var Target = Spell.CastInfo.Targets[0].Unit;
            var dist = System.Math.Abs(Vector2.Distance(Target.Position, owner.Position));
            var distt = dist - 125;
            var time = distt / 2400;
            var targetPos = GetPointFromUnit(owner, distt);
            FaceDirection(targetPos, Spell.CastInfo.Owner, true);
            PlayAnimation(owner, "dash1", 4f);
            ForceMovement(Spell.CastInfo.Owner, null, targetPos, 2400, 0, 120, 0);
            AddParticleTarget(owner, owner, "Rengar_Base_P_Leap_Dust.troy", owner);
            AddParticleTarget(owner, owner, "Rengar_Base_P_Leap_Grass.troy", owner);
        }

        public void OnMoveEnd(AttackableUnit unit)
        {
            SetStatus(owner, StatusFlags.Ghosted, false);
            RemoveBuff(thisBuff);
            RemoveParticle(P);
            //StopAnimation(owner, "", true, true, true);
        }
    }
}