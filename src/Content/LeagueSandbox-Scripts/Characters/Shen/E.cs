using GameServerCore;
using LeagueSandbox.GameServer.Scripting.CSharp;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using GameServerCore.Enums;
using System.Numerics;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using LeagueSandbox.GameServer.GameObjects.SpellNS;
using LeagueSandbox.GameServer.API;
using LeagueSandbox.GameServer.GameObjects;
using System.Collections.Generic;


namespace Spells
{
    public class ShenShadowDash : ISpellScript
    {
        float Dist;
        Spell Dash;
        float Damage;
        Particle Cas;
        Particle Mis;
        ObjAIBase Shen;
        Vector2 SpellPos;
        Vector2 TrueCoords;
        public static List<AttackableUnit> UnitsHit = new List<AttackableUnit>();
        public SpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
        {
            CastingBreaksStealth = true,
            DoesntBreakShields = true,
            TriggersSpellCasts = true,
            IsDamagingSpell = false,
            NotSingleTargetSpell = true
        };

        public void OnSpellCast(Spell spell)
        {
            Dash = spell;
            UnitsHit.Clear();
            Shen = spell.CastInfo.Owner as Champion;
            SetStatus(Shen, StatusFlags.Ghosted, true);
            SpellPos = new Vector2(spell.CastInfo.TargetPosition.X, spell.CastInfo.TargetPosition.Z);
            Dist = Vector2.Distance(Shen.Position, SpellPos);
            if (Dist > 575.0f) { Dist = 575.0f; }
            PlayAnimation(Shen, "Spell1", 0.3f);
            FaceDirection(SpellPos, Shen, true);
            TrueCoords = GetPointFromUnit(Shen, Dist);
            ForceMovement(Shen, null, TrueCoords, 1800, 0, 0, 0);
            ApiEventManager.OnMoveEnd.AddListener(this, Shen, OnMoveEnd, true);
            ApiEventManager.OnCollision.AddListener(this, Shen, OnCollision, false);
            Cas = AddParticleTarget(Shen, Shen, "Shen_shadowdash_cas", Shen, 10f);
            Mis = AddParticleTarget(Shen, Shen, "shen_shadowDash_mis", Shen, 10f);
        }
        public void OnCollision(GameObject owner, GameObject target)
        {
            Damage = 15f + (Shen.Spells[2].CastInfo.SpellLevel * 35f) + (Shen.Stats.AbilityPower.Total * 0.5f);
            if ((target as AttackableUnit).Team != Shen.Team)
            {
                if (!UnitsHit.Contains(target as AttackableUnit))
                {
                    UnitsHit.Add(target as AttackableUnit);
                    AddBuff("Shen Shadow Dash", 1.5f, 1, Dash, target as AttackableUnit, Shen);
                    AddParticleTarget(Shen, target as AttackableUnit, "shen_shadowDash_unit_impact", target as AttackableUnit, 10f);
                    if (target as AttackableUnit is Champion)
                    {
                        (target as AttackableUnit).TakeDamage(Shen, Damage, DamageType.DAMAGE_TYPE_MAGICAL, DamageSource.DAMAGE_SOURCE_SPELL, false);
                    }
                }
            }
        }
        public void OnMoveEnd(AttackableUnit owner)
        {
            RemoveParticle(Cas);
            RemoveParticle(Mis);
            Shen.SetDashingState(false);
            SetStatus(Shen, StatusFlags.Ghosted, false);
            ApiEventManager.OnCollision.RemoveListener(this, Shen);
        }
    }
}