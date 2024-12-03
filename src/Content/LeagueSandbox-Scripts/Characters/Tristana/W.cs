using System.Linq;
using GameServerCore;
using GameServerCore.Enums;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using LeagueSandbox.GameServer.Scripting.CSharp;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using LeagueSandbox.GameServer.GameObjects.SpellNS;
using LeagueSandbox.GameServer.API;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.Buildings;
using System.Numerics;

namespace Spells
{
    public class RocketJump : ISpellScript
    {
        Spell Jump;
        Vector2 truecoords;
        ObjAIBase Tristana;
        public SpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
        {
            CastingBreaksStealth = true,
            DoesntBreakShields = true,
            TriggersSpellCasts = true,
            IsDamagingSpell = true,
            NotSingleTargetSpell = true
        };

        public void OnSpellPostCast(Spell spell)
        {
            Jump = spell;
            Tristana = spell.CastInfo.Owner as Champion;
            PlayAnimation(Tristana, "spell2");
            FaceDirection(truecoords, Tristana, true);
            SetStatus(Tristana, StatusFlags.Ghosted, true);
            ApiEventManager.OnMoveEnd.AddListener(this, Tristana, OnMoveEnd, true);
            var Cursor = new Vector2(spell.CastInfo.TargetPosition.X, spell.CastInfo.TargetPosition.Z);
            var current = new Vector2(Tristana.Position.X, Tristana.Position.Y);
            var distance = Cursor - current;
            if (distance.Length() > 900f)
            {
                distance = Vector2.Normalize(distance);
                var range = distance * 900f;
                truecoords = current + range;
            }
            else
            {
                truecoords = Cursor;
            }
            var dist = System.Math.Abs(Vector2.Distance(truecoords, Tristana.Position));
            ForceMovement(Tristana, null, truecoords, dist * 1.22f, 0, dist * 0.05f, 0);
            AddParticle(Tristana, null, "tristana_rocketJump_cas.troy", Tristana.Position);
            AddParticleTarget(Tristana, Tristana, "tristana_rocketJump_cas_sparks.troy", Tristana);
        }
        public void OnMoveEnd(AttackableUnit unit)
        {
            Tristana.SetDashingState(false);
            SetStatus(Tristana, StatusFlags.Ghosted, false);
            PlayAnimation(Tristana, "spell3_landing", 0.2f);
            StopAnimation(Tristana, "spell3", true, true, true);
            AddParticle(Tristana, null, "tristana_rocketJump_land.troy", Tristana.Position);
            var damage = 25 + (45 * Jump.CastInfo.SpellLevel) + (Tristana.Stats.AbilityPower.Total * 0.8f);
            var units = GetUnitsInRange(Tristana.Position, 350f, true);
            for (int i = 0; i < units.Count; i++)
            {
                if (units[i].Team != Tristana.Team && !(units[i] is ObjBuilding || units[i] is BaseTurret))
                {
                    units[i].TakeDamage(Tristana, damage, DamageType.DAMAGE_TYPE_MAGICAL, DamageSource.DAMAGE_SOURCE_ATTACK, false);
                    AddParticleTarget(Tristana, units[i], "tristana_rocketJump_unit_tar.troy", units[i], 1f);
                    AddBuff("RocketJump", 0.5f * (1 + Jump.CastInfo.SpellLevel), 1, Jump, units[i], Tristana);
                }
            }
        }
    }
}