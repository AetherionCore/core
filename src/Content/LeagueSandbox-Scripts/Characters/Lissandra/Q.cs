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
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.Buildings;
using System.Collections.Generic;

namespace Spells
{
    public class LissandraQ : ISpellScript
    {
        private Spell Ice;
        private Vector2 TargetPos;
        private ObjAIBase Lissandra;
        public SpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
        {
            TriggersSpellCasts = true,
            IsDamagingSpell = true
        };
        public void OnActivate(ObjAIBase owner, Spell spell)
        {
            Ice = spell;
            Lissandra = owner = spell.CastInfo.Owner as Champion;
        }
        public void OnSpellPreCast(ObjAIBase owner, Spell spell, AttackableUnit target, Vector2 start, Vector2 end)
        {
            FaceDirection(start, Lissandra, true);
        }
        public void OnSpellPostCast(Spell spell)
        {
            TargetPos = GetPointFromUnit(Lissandra, 850f);
            SpellCast(Lissandra, 1, SpellSlotType.ExtraSlots, TargetPos, TargetPos, false, Vector2.Zero);
        }
    }

    public class LissandraQMissile : ISpellScript
    {
        float Damage;
        private Spell Ice;
        private ObjAIBase Lissandra;
        private SpellMissile Missile;
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
        public void OnSpellPostCast(Spell spell)
        {
            UnitsHit.Clear();
            Lissandra = spell.CastInfo.Owner as Champion;
            Missile = spell.CreateSpellMissile(new MissileParameters { Type = MissileType.Circle });
        }

        public void TargetExecute(Spell spell, AttackableUnit target, SpellMissile missile, SpellSector sector)
        {
            Missile = missile;
            UnitsHit.Add(target);
            Missile.SetToRemove();
            AddBuff("LissandraQ", 1.5f, 1, spell, target, Lissandra, false);
            AddParticleTarget(Lissandra, target, "Lissandra_Base_Q_tar", target, 1f);
            Damage = 40 + (35f * Lissandra.Spells[0].CastInfo.SpellLevel) + (Lissandra.Stats.AbilityPower.Total * 0.65f);
            target.TakeDamage(Lissandra, Damage, DamageType.DAMAGE_TYPE_MAGICAL, DamageSource.DAMAGE_SOURCE_SPELL, false);
            SpellCast(Lissandra, 4, SpellSlotType.ExtraSlots, GetPointFromUnit(Missile, 700), Vector2.Zero, true, Missile.Position);
        }
    }
    public class LissandraQShards : ISpellScript
    {
        float Damage;
        private Spell Shards;
        private ObjAIBase Lissandra;
        private SpellMissile Missile;
        public List<AttackableUnit> UnitsHit = Spells.LissandraQMissile.UnitsHit;
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
            Lissandra = owner = spell.CastInfo.Owner as Champion;
            Missile = spell.CreateSpellMissile(new MissileParameters { Type = MissileType.Circle, OverrideEndPosition = end });
        }

        public void TargetExecute(Spell spell, AttackableUnit target, SpellMissile missile, SpellSector sector)
        {
            Missile = missile;
            Damage = 40 + (35f * Lissandra.Spells[0].CastInfo.SpellLevel) + (Lissandra.Stats.AbilityPower.Total * 0.65f);
            if (!UnitsHit.Contains(target) && target != Lissandra && target.Team != Lissandra.Team && !(target is ObjBuilding || target is BaseTurret))
            {
                target.TakeDamage(Lissandra, Damage, DamageType.DAMAGE_TYPE_MAGICAL, DamageSource.DAMAGE_SOURCE_SPELL, false);
                AddParticleTarget(Lissandra, target, "Lissandra_Base_Q_tar2", target, 1f);
            }
        }
    }
}