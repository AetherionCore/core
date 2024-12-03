using GameServerCore.Enums;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using LeagueSandbox.GameServer.Scripting.CSharp;
using LeagueSandbox.GameServer.API;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using LeagueSandbox.GameServer.GameObjects.SpellNS;
using LeagueSandbox.GameServer.GameObjects.SpellNS.Missile;
using LeagueSandbox.GameServer.GameObjects.SpellNS.Sector;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.Buildings;
using GameServerLib.GameObjects.AttackableUnits;
using System.Numerics;

namespace Spells
{
    public class KhazixE : ISpellScript
    {
        float Dist;
        Spell Jump;
        float Damage;
        ObjAIBase Khazix;
        Vector2 Truecoords;
        public SpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
        {
            CastingBreaksStealth = true,
            DoesntBreakShields = true,
            TriggersSpellCasts = true,
            IsDamagingSpell = true,
            NotSingleTargetSpell = true
        };
        public void OnActivate(ObjAIBase owner, Spell spell)
        {
            Jump = spell;
            Khazix = owner = spell.CastInfo.Owner as Champion;
            ApiEventManager.OnKill.AddListener(this, Khazix, OnKill, false);
        }
        public void OnKill(DeathData deathData) { Khazix.Spells[2].SetCooldown(0); }
        public void OnSpellPostCast(Spell spell)
        {
            PlayAnimation(Khazix, "spell3");
            FaceDirection(Truecoords, Khazix, true);
            SetStatus(Khazix, StatusFlags.Ghosted, true);
            ApiEventManager.OnMoveEnd.AddListener(this, Khazix, OnMoveEnd, true);
            var Cursor = new Vector2(spell.CastInfo.TargetPosition.X, spell.CastInfo.TargetPosition.Z);
            var current = new Vector2(Khazix.Position.X, Khazix.Position.Y);
            var distance = Cursor - current;
            if (distance.Length() > 600f)
            {
                distance = Vector2.Normalize(distance);
                var range = distance * 600f;
                Truecoords = current + range;
            }
            else
            {
                Truecoords = Cursor;
            }
            Dist = System.Math.Abs(Vector2.Distance(Truecoords, Khazix.Position));
            ForceMovement(Khazix, null, Truecoords, 1100, 0, 30, 0);
            AddParticleTarget(Khazix, Khazix, "Khazix_Base_E_WeaponTrails", Khazix, size: 1, bone: "Weapon");
        }
        public void OnMoveEnd(AttackableUnit unit)
        {
            Khazix.SetDashingState(false);
            SetStatus(Khazix, StatusFlags.Ghosted, false);
            PlayAnimation(Khazix, "spell3_landing", 0.2f);
            StopAnimation(Khazix, "spell3", true, true, true);
            AddParticle(Khazix, null, "Khazix_Base_E_Land", Khazix.Position);
            Damage = 30 + (35 * Jump.CastInfo.SpellLevel) + (Khazix.Stats.AttackDamage.FlatBonus * 0.2f);
            var units = GetUnitsInRange(Khazix.Position, 350f, true);
            for (int i = 0; i < units.Count; i++)
            {
                if (units[i].Team != Khazix.Team && !(units[i] is ObjBuilding || units[i] is BaseTurret))
                {
                    units[i].TakeDamage(Khazix, Damage, DamageType.DAMAGE_TYPE_PHYSICAL, DamageSource.DAMAGE_SOURCE_ATTACK, false);
                    AddParticleTarget(Khazix, units[i], "Khazix_Base_E_Tar", units[i], 1f);
                    // AddBuff("RocketJump", 0.5f * (1 + Jump.CastInfo.SpellLevel), 1, Jump, units[i], Khazix);  
                }
            }
        }
    }
}