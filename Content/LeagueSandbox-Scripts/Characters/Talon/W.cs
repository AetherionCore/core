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
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.Buildings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace Spells
{
    public class TalonRake : ISpellScript
    {
        private Spell Rake;
        private ObjAIBase Talon;
        public SpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
        {
            TriggersSpellCasts = true,
            IsDamagingSpell = true
        };
        public void OnActivate(ObjAIBase owner, Spell spell)
        {
            Rake = spell;
            Talon = owner = spell.CastInfo.Owner as Champion;
        }
        public void OnSpellPreCast(ObjAIBase owner, Spell spell, AttackableUnit target, Vector2 start, Vector2 end)
        {
            if (Talon.HasBuff("TalonShadowAssaultBuff")) { Talon.RemoveBuffsWithName("TalonShadowAssaultBuff"); }
        }
        public void OnSpellCast(Spell spell)
        {
            for (int bladeCount = 0; bladeCount <= 2; bladeCount++)
            {
                var End = GetPointFromUnit(Talon, 700f, -20f + (bladeCount * 20f));
                SpellCast(Talon, 1, SpellSlotType.ExtraSlots, End, End, true, Vector2.Zero);
            }
        }
    }

    public class TalonRakeMissileOne : ISpellScript
    {
        float Damage;
        private Spell Rake;
        private ObjAIBase Talon;
        private SpellMissile Missile;
        public List<AttackableUnit> UnitsHit = new List<AttackableUnit>();
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
            UnitsHit.Clear();
            Talon = owner = spell.CastInfo.Owner as Champion;
            Missile = spell.CreateSpellMissile(new MissileParameters { Type = MissileType.Circle, OverrideEndPosition = end });
            if (Missile != null) { AddParticleTarget(Talon, Missile, "Talon_Skin29_B_T.troy", Missile, 25000f); }
            ApiEventManager.OnSpellMissileEnd.AddListener(this, Missile, OnMissileEnd, true);
        }

        public void TargetExecute(Spell spell, AttackableUnit target, SpellMissile missile, SpellSector sector)
        {
            Damage = 5 + (25f * Talon.Spells[1].CastInfo.SpellLevel) + (Talon.Stats.AttackDamage.FlatBonus * 0.6f);
            if (target.HasBuff("TalonDamageAmp")) { Damage = Damage + (Damage * 0.03f * Talon.Spells[2].CastInfo.SpellLevel); }
            if (!UnitsHit.Contains(target) && target != Talon && target.Team != Talon.Team && !(target is ObjBuilding || target is BaseTurret))
            {
                UnitsHit.Add(target);
                target.TakeDamage(Talon, Damage, DamageType.DAMAGE_TYPE_PHYSICAL, DamageSource.DAMAGE_SOURCE_SPELL, false);
                AddBuff("TalonSlow", 2f, 1, spell, target, Talon);
                AddParticleTarget(Talon, target, "talon_w_tar.troy", target, 1f);
            }
        }

        public void OnMissileEnd(SpellMissile missile)
        {
            SpellCast(Talon, 2, SpellSlotType.ExtraSlots, true, Talon, missile.Position);
        }
    }
    public class TalonRakeMissileTwo : ISpellScript
    {
        float Damage;
        private Spell Rake;
        private ObjAIBase Talon;
        private SpellMissile Missile;
        public List<AttackableUnit> UnitsHit = new List<AttackableUnit>();
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
            UnitsHit.Clear();
            Talon = owner = spell.CastInfo.Owner as Champion;
            Missile = spell.CreateSpellMissile(new MissileParameters { Type = MissileType.Circle });
            if (Missile != null) { AddParticleTarget(Talon, Missile, "Talon_Skin29_B_T.troy", Missile, 25000f); }
        }
        public void TargetExecute(Spell spell, AttackableUnit target, SpellMissile missile, SpellSector sector)
        {
            Damage = 5 + (25f * Talon.Spells[1].CastInfo.SpellLevel) + (Talon.Stats.AttackDamage.FlatBonus * 0.6f);
            if (target.HasBuff("TalonDamageAmp")) { Damage = Damage + (Damage * 0.03f * Talon.Spells[2].CastInfo.SpellLevel); }
            if (!UnitsHit.Contains(target) && target != Talon && target.Team != Talon.Team && !(target is ObjBuilding || target is BaseTurret))
            {
                UnitsHit.Add(target);
                target.TakeDamage(Talon, Damage, DamageType.DAMAGE_TYPE_PHYSICAL, DamageSource.DAMAGE_SOURCE_SPELL, false);
                AddBuff("TalonSlow", 2f, 1, spell, target, Talon);
                AddParticleTarget(Talon, target, "talon_w_tar.troy", target, 1f);
            }
            if (target == Talon) { missile.SetToRemove(); }
        }
    }
}