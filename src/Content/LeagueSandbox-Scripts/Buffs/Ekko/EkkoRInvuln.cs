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
    internal class EkkoRInvuln : IBuffGameScript
    {
        public BuffScriptMetaData BuffMetaData { get; set; } = new BuffScriptMetaData
        {
            BuffAddType = BuffAddType.REPLACE_EXISTING
        };

        public StatsModifier StatsModifier { get; private set; } = new StatsModifier();

        Spell Spell;
        ObjAIBase Owner;
        private Buff buff;
        AttackableUnit Unit;
        float dist;
        float h;
        float m;
        float p;
        float p1;
        float p2;
        float p3;
        float p4;
        float p5;
        float p6;
        float Health;
        //Vector2 P;
        public static Particle P;
        public static Particle P2;
        public static Particle P3;
        public static Particle P4;
        public static Particle P5;
        public static Particle P6;

        public void OnActivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {
            Unit = unit;
            Owner = ownerSpell.CastInfo.Owner;
            Spell = ownerSpell;
            if (ownerSpell.CastInfo.Owner is Champion owner)
            {
                SetStatus(Unit, StatusFlags.CanMove, true);
                AddParticle(owner, Unit, "Ekko_Base_R_TrailEnd.troy", Unit.Position, 25000f, 1);
                ApiEventManager.OnSpellCast.AddListener(this, Owner.GetSpell("EkkoR"), ROnSpellCast);
                ApiEventManager.OnSpellPostCast.AddListener(this, Owner.GetSpell("EkkoR"), ROnSpellPostCast);
            }

        }
        public void ROnSpellCast(Spell spell)
        {
            var owner = spell.CastInfo.Owner;
            var ROP = Unit.Position;
            (Unit as Minion).SetTargetUnit(null, true);
            AddParticle(owner, null, "Ekko_Base_R_AOERing", Unit.Position);
            AddParticle(owner, null, "Ekko_Base_R_Tar_Impact", Unit.Position);
            CreateTimer((float)4, () =>
            {
                Unit.SetWaypoints(GetPath(Unit.Position, ROP));
                ForceMovement(Unit, null, ROP, dist, 0, 0, 0);
            });
        }

        public void ROnSpellPostCast(Spell spell)
        {
            var owner = spell.CastInfo.Owner;
            PlayAnimation(owner, "Spell4_end");
            var spellPos = Unit.Position;
            dist = (System.Math.Abs(Vector2.Distance(Unit.Position, owner.Position))) * 4;
            ForceMovement(owner, null, spellPos, dist, 0, 0, 0);
            var H = Health - owner.Stats.CurrentHealth;
            if (H >= 0) { owner.Stats.CurrentHealth += H; }
            AddParticleTarget(Owner, Owner, "Ekko_Base_R_TrailEndPose1", Owner);
            AddParticleTarget(Owner, Owner, "Ekko_Base_R_TrailEndPose2", Owner);
            AddParticleTarget(Owner, Owner, "Ekko_Base_R_TrailEndPose3", Owner);
            AddParticleTarget(Owner, Owner, "Ekko_Base_R_TrailEndPose4", Owner);
            AddParticleTarget(Owner, Owner, "Ekko_Base_R_TrailEndPose5", Owner);
            AddParticleTarget(Owner, Owner, "Ekko_Base_R_TrailEndPose6", Owner);
            if (P != null && P2 != null && P3 != null && P4 != null && P5 != null && P6 != null)
            {
                AddParticle(Owner, null, "Ekko_Base_R_RewindPose1", P.Position);
                AddParticle(Owner, null, "Ekko_Base_R_RewindPose2", P2.Position);
                AddParticle(Owner, null, "Ekko_Base_R_RewindPose3", P3.Position);
                AddParticle(Owner, null, "Ekko_Base_R_RewindPose4", P4.Position);
                AddParticle(Owner, null, "Ekko_Base_R_RewindPose5", P5.Position);
                AddParticle(Owner, null, "Ekko_Base_R_RewindPose6", P6.Position);
                AddParticle(Owner, null, "Ekko_Base_R_TrailEndPose1", P.Position);
                AddParticle(Owner, null, "Ekko_Base_R_TrailEndPose2", P2.Position);
                AddParticle(Owner, null, "Ekko_Base_R_TrailEndPose3", P3.Position);
                AddParticle(Owner, null, "Ekko_Base_R_TrailEndPose4", P4.Position);
                AddParticle(Owner, null, "Ekko_Base_R_TrailEndPose5", P5.Position);
                AddParticle(Owner, null, "Ekko_Base_R_TrailEndPose6", P6.Position);
            }
        }
        public void OnDeactivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {
            if (Spell.CastInfo.Owner is Champion owner) { }
        }
        public void OnUpdate(float diff)
        {
            h += diff;
            p += diff; p1 += diff; p2 += diff; p3 += diff; p4 += diff; p5 += diff; p6 += diff;
            m += diff;
            if (h >= 4000.0f && Unit != null) { Health = Owner.Stats.CurrentHealth; h = 0f; }
            if (p >= 600f && Unit != null) { P = AddParticle(Owner, null, "", Owner.Position); p = -3400f; }
            if (p1 >= 1200f && Unit != null) { P2 = AddParticle(Owner, null, "", Owner.Position); p1 = -2800f; }
            if (p2 >= 1800f && Unit != null) { P3 = AddParticle(Owner, null, "", Owner.Position); p2 = -2200f; }
            if (p3 >= 2400f && Unit != null) { P4 = AddParticle(Owner, null, "", Owner.Position); p3 = -1600f; }
            if (p4 >= 3000f && Unit != null) { P5 = AddParticle(Owner, null, "", Owner.Position); p4 = -1000f; }
            if (p5 >= 3600f && Unit != null) { P6 = AddParticle(Owner, null, "", Owner.Position); p5 = -400f; }
            if (p6 >= 4400f && Unit != null) { p6 = 0f; }
            //if (p >= 600f && Unit != null) { P = Owner.Position; p = 0f; } 
            if (m >= 4000.0f && Unit != null)
            {
                Unit.SetWaypoints(GetPath(Unit.Position, P.Position));
                //AddBuff("EkkoRT", 4f, 1, Spell, Unit, Owner, false);; m = 0f; 			

            }
            if (m >= 4600.0f && Unit != null)
            {
                Unit.SetWaypoints(GetPath(Unit.Position, P2.Position));
            }
            if (m >= 5200.0f && Unit != null)
            {
                Unit.SetWaypoints(GetPath(Unit.Position, P3.Position));
            }
            if (m >= 5800.0f && Unit != null)
            {
                Unit.SetWaypoints(GetPath(Unit.Position, P4.Position));
            }
            if (m >= 6400.0f && Unit != null)
            {
                Unit.SetWaypoints(GetPath(Unit.Position, P5.Position));
            }
            if (m >= 7200.0f && Unit != null)
            {
                Unit.SetWaypoints(GetPath(Unit.Position, P6.Position));
                //ForceMovement(Unit, null,pp6, V, 0, 0, 0);; m = 0f;
                m = 3400f;
            }
            //if (m >= 4000.0f && Unit != null) { ForceMovement(Unit, null, P, 450, 0, 0, 0);; m = 0f; }	
        }
    }
}