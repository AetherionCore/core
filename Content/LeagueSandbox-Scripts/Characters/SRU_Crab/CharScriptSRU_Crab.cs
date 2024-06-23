using GameServerCore.Enums;
using GameServerCore.Scripting.CSharp;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using LeagueSandbox.GameServer.API;
using LeagueSandbox.GameServer.GameObjects;
using GameServerLib.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using LeagueSandbox.GameServer.GameObjects.SpellNS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace CharScripts
{
    internal class CharScriptSru_Crab : ICharScript
    {
		bool isScuttleWard = false;
        bool hasPathEnded = false;
        Minion ScuttleCrab;
		
        public void OnActivate(ObjAIBase owner, Spell spell = null)
        {
            ScuttleCrab = owner as Minion;

            OverrideAnimation(owner, "CRAB_SPAWN", "IDLE1");
            AddBuff("RegenerationAura", 25000.0f, 1, null, owner, owner);

            if (ScuttleCrab.Name != "FakeCrab")
            {
                ApiEventManager.OnDeath.AddListener(this, owner, OnDeath, true);
            }
            else
            {
                isScuttleWard = true;
                //ApiEventManager.OnUnitUpdateMoveOrder.AddListener(this, owner, OnUpdateOrder, true);
                owner.PlayAnimation("crab_burrow", 0.0f, 0.0f, 1.0f, (AnimationFlags)136);

                OverrideAnimation(owner, "WARD_RUN (UNCOMPRESSED)", "RUN");
                OverrideAnimation(owner, "ward_run (Uncompressed)", "IDLE1");
            }
        }

        public void OnUpdateOrder(ObjAIBase unit, OrderType order)
        {
            if (order == OrderType.Stop)
            {
                PlayAnimation(ScuttleCrab, "ward_run_toground", 0.0f, 0.0f, 1.0f, (AnimationFlags)168);

                OverrideAnimation(ScuttleCrab, "ward_hide", "IDLE1");
                OverrideAnimation(ScuttleCrab, "", "RUN");

                AddUnitPerceptionBubble(ScuttleCrab, 525.0f, 75.0f, ScuttleCrab.Team);
            }
        }

        public void OnDeath(DeathData deathData)
        {
            Monster monster = deathData.Unit as Monster;

            monster.PlayAnimation("crab_hide", 5.0f, 0.0f, 1.0f, (AnimationFlags)133);

            SetStatus(monster, StatusFlags.NoRender, true);

            var minion = AddMinion(null, "Sru_Crab", "FakeCrab", deathData.Unit.Position, deathData.Killer.Team, ignoreCollision: true, targetable: true);
            minion.SetWaypoints(new List<Vector2> { { new Vector2(monster.Camp.Position.X, monster.Camp.Position.Z) } });
        }

        public void OnDeactivate(ObjAIBase owner, Spell spell = null)
        {
        }

        public void OnUpdate(float diff)
        {
            if (!hasPathEnded && isScuttleWard && ScuttleCrab.Position == ScuttleCrab.Waypoints[0])
            {
                PlayAnimation(ScuttleCrab, "ward_run_toground", 0.0f, 0.0f, 1.0f, (AnimationFlags)168);

                OverrideAnimation(ScuttleCrab, "ward_hide", "IDLE1");
                OverrideAnimation(ScuttleCrab, "", "RUN");

                AddUnitPerceptionBubble(ScuttleCrab, 525.0f, 75.0f, ScuttleCrab.Team);
                hasPathEnded = true;
            }
        }
    }
}
