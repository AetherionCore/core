using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using LeagueSandbox.GameServer.Scripting.CSharp;
using System.Numerics;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.GameObjects.SpellNS;
﻿using GameServerCore.Enums;
using LeagueSandbox.GameServer.GameObjects;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.Buildings;
using LeagueSandbox.GameServer.API;

namespace Spells
{
    public class XenZhaoSweep : ISpellScript
    {
        float Damage;
        Spell Sweep;
        Particle Trail;
        ObjAIBase XinZhao;
        AttackableUnit Target;
        public SpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
        {
            TriggersSpellCasts = true,
            IsDamagingSpell = true
        };

        public void OnSpellPreCast(ObjAIBase owner, Spell spell, AttackableUnit target, Vector2 start, Vector2 end)
        {
            Sweep = spell;
            Target = target;
            XinZhao = owner = spell.CastInfo.Owner as Champion;
            ApiEventManager.OnMoveEnd.AddListener(this, XinZhao, OnMoveEnd, true);
        }
        public void OnSpellCast(Spell spell)
        {
            PlayAnimation(XinZhao, "Spell3");
            SetStatus(XinZhao, StatusFlags.Ghosted, true);
            Damage = 35 + (35 * spell.CastInfo.SpellLevel) + (XinZhao.Stats.AbilityPower.Total * 0.6f);
            var dist = System.Math.Abs(Vector2.Distance(Target.Position, XinZhao.Position));
            var distt = dist - 125f;
            var truepos = GetPointFromUnit(XinZhao, distt);
            Trail = AddParticleTarget(XinZhao, XinZhao, "xenZiou_AudaciousCharge_self_trail_01.troy", XinZhao);
            ForceMovement(XinZhao, null, truepos, 2200, 0, 0, 0);
        }
        public void OnMoveEnd(AttackableUnit owner)
        {
            RemoveParticle(Trail);
            XinZhao.SetDashingState(false);
            SetStatus(XinZhao, StatusFlags.Ghosted, false);
            StopAnimation(XinZhao, "Spell3", true, true, true);
            if (!(Target is ObjBuilding || Target is BaseTurret) && !XinZhao.HasBuff("XinZhaoPassive"))
            {
                AddBuff("XenZhaoPuncture", 3.0f, 1, Sweep, Target, XinZhao);
            }
            if (!(Target is ObjBuilding || Target is BaseTurret) && Target.HasBuff("XenZhaoPuncture"))
            {
                AddBuff("XenZhaoPuncture", 3.0f, 1, Sweep, Target, XinZhao);
            }
            AddParticleTarget(XinZhao, Target, "xenZiou_AudaciousCharge_tar_unit_instant.troy", Target);
            var units = GetUnitsInRange(Target.Position, 250f, true);
            for (int i = 0; i < units.Count; i++)
            {
                if (!(units[i].Team == XinZhao.Team || units[i] is BaseTurret || units[i] is ObjBuilding))
                {
                    AddBuff("XinEDebuff", 2.0f, 1, Sweep, units[i], XinZhao);
                    AddParticleTarget(XinZhao, units[i], "xenZiou_AudaciousCharge_tar_03_unit_tar.troy", units[i]);
                    units[i].TakeDamage(XinZhao, Damage, DamageType.DAMAGE_TYPE_MAGICAL, DamageSource.DAMAGE_SOURCE_SPELLAOE, false);
                }
            }
        }
    }
}