using GameServerCore.Enums;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using LeagueSandbox.GameServer.GameObjects;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.GameObjects.SpellNS;
using LeagueSandbox.GameServer.GameObjects.StatsNS;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.Scripting.CSharp;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using System.Numerics;
using LeagueSandbox.GameServer.GameObjects.SpellNS.Missile;
using LeagueSandbox.GameServer.GameObjects.SpellNS.Sector;
using LeagueSandbox.GameServer.API;
using System.Collections.Generic;

namespace Spells
{
    public class RivenFengShuiEngine : ISpellScript
    {
        ObjAIBase Riven;
        Spell FengShuiEngine;
        public SpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
        {
            TriggersSpellCasts = true,
            IsDamagingSpell = true
        };

        public void OnSpellPostCast(Spell spell)
        {
            FengShuiEngine = spell;
            Riven = spell.CastInfo.Owner as Champion;
            FengShuiEngine.SetCooldown(0.5f, true);
            AddBuff("RivenFengShuiEngine", 15f, 1, FengShuiEngine, Riven, Riven);
        }
    }
    public class RivenIzunaBlade : ISpellScript
    {
        ObjAIBase Riven;
        Spell FengShuiEngine;
        public SpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
        {
            TriggersSpellCasts = true,
            IsDamagingSpell = true
        };

        public void OnSpellPostCast(Spell spell)
        {
            FengShuiEngine = spell;
            Riven = spell.CastInfo.Owner as Champion;
            var truePos = GetPointFromUnit(Riven, 1000f);
            var targetPos = GetPointFromUnit(Riven, 900f, -13f);
            var Pos = GetPointFromUnit(Riven, 900f, 13f);
            SpellCast(Riven, 0, SpellSlotType.ExtraSlots, truePos, truePos, true, Vector2.Zero);
            SpellCast(Riven, 0, SpellSlotType.ExtraSlots, targetPos, targetPos, true, Vector2.Zero);
            SpellCast(Riven, 0, SpellSlotType.ExtraSlots, Pos, Pos, true, Vector2.Zero);
            Riven.RemoveBuffsWithName("RivenFengShuiEngine");
        }
    }
    public class RivenLightsaberMissile : ISpellScript
    {
        float Damage;
        ObjAIBase Riven;
        Spell FengShuiEngine;
        public List<AttackableUnit> UnitsHit = new List<AttackableUnit>();
        public SpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
        {
            TriggersSpellCasts = true,
            IsDamagingSpell = true,
            MissileParameters = new MissileParameters { Type = MissileType.Circle }
        };
        public void OnActivate(ObjAIBase owner, Spell spell)
        {
            FengShuiEngine = spell;
            Riven = owner = spell.CastInfo.Owner as Champion;
            ApiEventManager.OnSpellHit.AddListener(this, spell, TargetExecute, false);
        }
        public void OnSpellPreCast(ObjAIBase owner, Spell spell, AttackableUnit target, Vector2 start, Vector2 end)
        {
            UnitsHit.Clear();
        }
        public void TargetExecute(Spell spell, AttackableUnit target, SpellMissile missile, SpellSector sector)
        {
            switch (Riven.SkinID)
            {
                case 3:
                    break;

                case 4:
                    break;
                case 5:
                    break;

                default:
                    break;
            }
            Damage = 130 + (20 * Riven.Spells[3].CastInfo.SpellLevel) + (Riven.Stats.AttackDamage.FlatBonus * 0.6f);
            if (!UnitsHit.Contains(target))
            {
                UnitsHit.Add(target);
                target.TakeDamage(Riven, Damage, DamageType.DAMAGE_TYPE_PHYSICAL, DamageSource.DAMAGE_SOURCE_ATTACK, false);
                AddParticleTarget(Riven, target, "Riven_Base_R_Tar.troy", target, 1f);
                AddParticleTarget(Riven, target, "Riven_Base_R_Tar_Minion.troy", target, 1f);
            }
        }
    }
    public class RivenLightsaberMissileSide : RivenIzunaBlade { }
}