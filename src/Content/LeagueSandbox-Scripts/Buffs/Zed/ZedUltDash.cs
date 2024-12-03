using GameServerCore.Enums;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.API;
using LeagueSandbox.GameServer.GameObjects;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using LeagueSandbox.GameServer.GameObjects.SpellNS;
using LeagueSandbox.GameServer.GameObjects.StatsNS;
using LeagueSandbox.GameServer.Scripting.CSharp;
using System.Numerics;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;

namespace Buffs
{
    internal class ZedUltDash : IBuffGameScript
    {
        private Spell Ult;
        private Particle P;
        private ObjAIBase Zed;
        private Buff DashBuff;
        private readonly AttackableUnit Target = Spells.ZedUlt.Target;
        public BuffScriptMetaData BuffMetaData { get; set; } = new BuffScriptMetaData
        {
            BuffAddType = BuffAddType.REPLACE_EXISTING
        };
        public StatsModifier StatsModifier { get; private set; } = new StatsModifier();
        public void OnActivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {
            DashBuff = buff;
            Ult = ownerSpell;
            Zed = ownerSpell.CastInfo.Owner as Champion;
            FaceDirection(Target.Position, Zed, true);
            SetStatus(Zed, StatusFlags.Ghosted, true);
            ApiEventManager.OnMoveEnd.AddListener(this, Zed, OnMoveEnd, true);
            var dist = System.Math.Abs(Vector2.Distance(Target.Position, Zed.Position));
            var distt = dist + 150;
            var targetPos = GetPointFromUnit(Zed, distt);
            FaceDirection(targetPos, Zed, true);
            P = AddParticleTarget(Zed, Zed, "Zed_Base_R_Dash.troy", Zed);
            AddBuff("ZedUltDashCloneMaker", 0.5f, 1, Ult, Zed, Zed);
            if (dist <= 350f)
            {
                ForceMovement(Zed, null, targetPos, 1700f, 0, 0, 0);
            }
            else
            {
                ForceMovement(Zed, null, targetPos, dist * 3, 0, 0, 0);
            }
        }
        public void OnMoveEnd(AttackableUnit unit)
        {
            if (Zed.Team != Target.Team && Target is Champion)
            {
                Zed.SetTargetUnit(Target, true);
                Zed.UpdateMoveOrder(OrderType.AttackTo, true);
            }
            SetStatus(Zed, StatusFlags.Ghosted, false);
            RemoveBuff(DashBuff);
            Zed.RemoveBuffsWithName("ZedUltBuff");
            RemoveParticle(P);
            SealSpellSlot(Zed, SpellSlotType.SpellSlots, 3, SpellbookType.SPELLBOOK_CHAMPION, false);
            AddBuff("ZedUltExecute", 3f, 1, Ult, Target, Zed);
            StopAnimation(Zed, "spell4_strike", true, true, true);
        }
    }
}