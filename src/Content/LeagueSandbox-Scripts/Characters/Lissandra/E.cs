using GameServerCore.Enums;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using LeagueSandbox.GameServer.Scripting.CSharp;
using System.Numerics;
using LeagueSandbox.GameServer.API;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using LeagueSandbox.GameServer.GameObjects.SpellNS;
using LeagueSandbox.GameServer.GameObjects.SpellNS.Missile;
using LeagueSandbox.GameServer.GameObjects.SpellNS.Sector;
using System.Collections.Generic;
using LeagueSandbox.GameServer.GameObjects;

namespace Spells
{
    public class LissandraE : ISpellScript
    {
        private Minion Ice;
        private Particle End;
        private Vector2 TargetPos;
        private ObjAIBase Lissandra;
        public SpellMissile Missile = Spells.LissandraEMissile.Missile;
        public SpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
        {
            TriggersSpellCasts = true,
            IsDamagingSpell = true
        };
        public void OnSpellPreCast(ObjAIBase owner, Spell spell, AttackableUnit target, Vector2 start, Vector2 end)
        {
            Lissandra = owner = spell.CastInfo.Owner as Champion;
            if (!Lissandra.HasBuff("LissandraE")) { FaceDirection(start, Lissandra, true); }
        }
        public void OnSpellPostCast(Spell spell)
        {
            if (!Lissandra.HasBuff("LissandraE"))
            {
                AddBuff("LissandraE", 2f, 1, spell, Lissandra, Lissandra, false);
            }
        }
    }

    public class LissandraEMissile : ISpellScript
    {
        float Damage;
        private Spell Ice;
        private Vector2 TargetPos;
        private ObjAIBase Lissandra;
        public static SpellMissile Missile;
        public static List<AttackableUnit> UnitsHit = new List<AttackableUnit>();
        public SpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
        {
            IsDamagingSpell = true,
            TriggersSpellCasts = true
        };

        public void OnActivate(ObjAIBase owner, Spell spell)
        {
            ApiEventManager.OnSpellHit.AddListener(this, spell, TargetExecute, false);
        }
        public void OnSpellPreCast(ObjAIBase owner, Spell spell, AttackableUnit target, Vector2 start, Vector2 end)
        {
            TargetPos = end;
        }
        public void OnSpellPostCast(Spell spell)
        {
            UnitsHit.Clear();
            Lissandra = spell.CastInfo.Owner as Champion;
            Missile = spell.CreateSpellMissile(new MissileParameters { Type = MissileType.Circle, OverrideEndPosition = TargetPos });
            ApiEventManager.OnSpellMissileEnd.AddListener(this, Missile, OnMissileEnd, true);
            ApiEventManager.OnBuffDeactivated.AddListener(this, Lissandra.GetBuffWithName("LissandraE"), OnBuffDeactivation, true);
        }
        public void OnMissileEnd(SpellMissile missile)
        {
            Lissandra.GetBuffWithName("LissandraE").DeactivateBuff();
        }
        public void OnBuffDeactivation(Buff buff)
        {
            Missile.SetToRemove();
        }
        public void TargetExecute(Spell spell, AttackableUnit target, SpellMissile missile, SpellSector sector)
        {
            Missile = missile;
            Damage = 25 + (45f * Lissandra.Spells[2].CastInfo.SpellLevel) + (Lissandra.Stats.AbilityPower.Total * 0.6f);
            target.TakeDamage(Lissandra, Damage, DamageType.DAMAGE_TYPE_MAGICAL, DamageSource.DAMAGE_SOURCE_SPELL, false);
            AddParticleTarget(Lissandra, target, "Lissandra_Base_E_tar", target, 1f);
        }
    }
}