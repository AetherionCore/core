using GameServerCore.Enums;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.GameObjects;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.GameObjects.SpellNS;
using LeagueSandbox.GameServer.GameObjects.StatsNS;
using LeagueSandbox.GameServer.Scripting.CSharp;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using System.Numerics;
using LeagueSandbox.GameServer.GameObjects.SpellNS.Missile;
using LeagueSandbox.GameServer.GameObjects.SpellNS.Sector;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.Buildings;
using LeagueSandbox.GameServer.API;

namespace Spells
{
    public class LeblancSlide : ISpellScript
    {
        float AP;
        float Dist;
        Minion POS;
        bool IsCrit;
        Particle Mis;
        float Damage;
        float BaseDamage;
        float QOrbDamage;
        float ROrbDamage;
        ObjAIBase Leblanc;
        SpellMissile Missile;
        public SpellScriptMetadata ScriptMetadata => new SpellScriptMetadata()
        {
            TriggersSpellCasts = true
        };
        public void OnSpellPreCast(ObjAIBase owner, Spell spell, AttackableUnit target, Vector2 start, Vector2 end)
        {
            Leblanc = owner = spell.CastInfo.Owner as Champion;
            ApiEventManager.OnMoveEnd.AddListener(this, Leblanc, OnMoveEnd, true);
            ApiEventManager.OnMoveSuccess.AddListener(this, Leblanc, OnMoveSuccess, true);
            Dist = Vector2.Distance(Leblanc.Position, start);
            PlayAnimation(Leblanc, "Spell2");
            FaceDirection(start, owner, true);
            SetStatus(Leblanc, StatusFlags.Ghosted, true);
            if (Dist > spell.SpellData.CastRangeDisplayOverride)
            {
                start = GetPointFromUnit(owner, spell.SpellData.CastRangeDisplayOverride);
            }
            ForceMovement(Leblanc, null, start, 1450, 0, 0, 0);
        }
        public void OnSpellCast(Spell spell)
        {
            AddBuff("LeblancSlide", 4.0f, 1, spell, Leblanc, Leblanc);
            AddBuff("LeblancSlideMove", 4.0f, 1, spell, Leblanc, Leblanc);
            AddParticle(Leblanc, null, "LeBlanc_Base_W_cas.troy", Leblanc.Position);
            Mis = AddParticleTarget(Leblanc, Leblanc, "LeBlanc_Base_W_mis.troy", Leblanc);
            POS = AddMinion(Leblanc, "TestCube", "TestCube", Leblanc.Position, Leblanc.Team, Leblanc.SkinID, true, false);
            AddBuff("LeblancSlideReturn", 4.0f, 1, spell, POS, Leblanc);
        }
        public void OnSpellPostCast(Spell spell) { spell.SetCooldown(0.5f, true); }
        public void OnMoveEnd(AttackableUnit owner)
        {
            SetStatus(Leblanc, StatusFlags.Ghosted, false);
            StopAnimation(Leblanc, "Spell2", true, true, true);
            RemoveParticle(Mis);
        }
        public void OnMoveSuccess(AttackableUnit owner)
        {
            AP = Leblanc.Stats.AbilityPower.Total * 0.65f;
            BaseDamage = 45 + (40 * Leblanc.Spells[1].CastInfo.SpellLevel) + AP;
            QOrbDamage = 30 + (25 * Leblanc.Spells[0].CastInfo.SpellLevel) + AP;
            ROrbDamage = (150f * Leblanc.Spells[3].CastInfo.SpellLevel) + AP;
            AddParticle(Leblanc, null, "LeBlanc_Base_W_aoe_impact_02.troy", Leblanc.Position);
            var AOE = GetUnitsInRange(Leblanc.Position, 260f, true);
            for (int i = 0; i < AOE.Count; i++)
            {
                if (AOE[i].Team != Leblanc.Team && !(AOE[i] is ObjBuilding || AOE[i] is BaseTurret))
                {
                    AddParticleTarget(Leblanc, AOE[i], "LeBlanc_Base_W_tar.troy", AOE[i], 1f);
                    AddParticleTarget(Leblanc, AOE[i], "LeBlanc_Base_W_tar_02.troy", AOE[i], 1f);
                    if (AOE[i].HasBuff("LeblancChaosOrb")) { IsCrit = true; Damage = BaseDamage + QOrbDamage; AOE[i].RemoveBuffsWithName("LeblancChaosOrb"); }
                    else if (AOE[i].HasBuff("LeblancChaosOrbM")) { IsCrit = true; Damage = BaseDamage + ROrbDamage; AOE[i].RemoveBuffsWithName("LeblancChaosOrbM"); }
                    else { IsCrit = false; Damage = BaseDamage; }
                    AOE[i].TakeDamage(Leblanc, Damage, DamageType.DAMAGE_TYPE_MAGICAL, DamageSource.DAMAGE_SOURCE_SPELL, IsCrit);
                }
            }
        }
    }
    public class LeblancSlideReturn : ISpellScript
    {
        public SpellScriptMetadata ScriptMetadata => new SpellScriptMetadata()
        {
            TriggersSpellCasts = true
        };
    }
    public class LeblancSlideM : ISpellScript
    {
        float AP;
        float Dist;
        Minion POS;
        bool IsCrit;
        Particle Mis;
        float Damage;
        float BaseDamage;
        float QOrbDamage;
        float ROrbDamage;
        ObjAIBase Leblanc;
        SpellMissile Missile;
        public SpellScriptMetadata ScriptMetadata => new SpellScriptMetadata()
        {
            TriggersSpellCasts = true
        };
        public void OnSpellPreCast(ObjAIBase owner, Spell spell, AttackableUnit target, Vector2 start, Vector2 end)
        {
            Leblanc = owner = spell.CastInfo.Owner as Champion;
            ApiEventManager.OnMoveEnd.AddListener(this, Leblanc, OnMoveEnd, true);
            ApiEventManager.OnMoveSuccess.AddListener(this, Leblanc, OnMoveSuccess, true);
            Dist = Vector2.Distance(Leblanc.Position, start);
            PlayAnimation(Leblanc, "Spell2");
            FaceDirection(start, owner, true);
            SetStatus(Leblanc, StatusFlags.Ghosted, true);
            if (Dist > spell.SpellData.CastRangeDisplayOverride)
            {
                start = GetPointFromUnit(owner, spell.SpellData.CastRangeDisplayOverride);
            }
            ForceMovement(Leblanc, null, start, 1450, 0, 0, 0);
        }
        public void OnSpellCast(Spell spell)
        {
            AddBuff("LeblancSlideM", 4.0f, 1, spell, Leblanc, Leblanc);
            AddBuff("LeblancSlideMoveM", 4.0f, 1, spell, Leblanc, Leblanc);
            AddParticle(Leblanc, null, "LeBlanc_Base_RW_cas.troy", Leblanc.Position);
            Mis = AddParticleTarget(Leblanc, Leblanc, "LeBlanc_Base_RW_mis.troy", Leblanc);
            POS = AddMinion(Leblanc, "TestCube", "TestCube", Leblanc.Position, Leblanc.Team, Leblanc.SkinID, true, false);
            AddBuff("LeblancSlideReturnM", 4.0f, 1, spell, POS, Leblanc);
        }
        public void OnSpellPostCast(Spell spell) { spell.SetCooldown(0.5f, true); }
        public void OnMoveEnd(AttackableUnit owner)
        {
            SetStatus(Leblanc, StatusFlags.Ghosted, false);
            StopAnimation(Leblanc, "Spell2", true, true, true);
            RemoveParticle(Mis);
        }
        public void OnMoveSuccess(AttackableUnit owner)
        {
            AP = Leblanc.Stats.AbilityPower.Total * 0.65f;
            BaseDamage = (150 * Leblanc.Spells[3].CastInfo.SpellLevel) + AP;
            QOrbDamage = 30 + (25 * Leblanc.Spells[0].CastInfo.SpellLevel) + AP;
            ROrbDamage = (150f * Leblanc.Spells[3].CastInfo.SpellLevel) + AP;
            AddParticle(Leblanc, null, "LeBlanc_Base_RW_aoe_impact_02.troy", Leblanc.Position);
            var AOE = GetUnitsInRange(Leblanc.Position, 260f, true);
            for (int i = 0; i < AOE.Count; i++)
            {
                if (AOE[i].Team != Leblanc.Team && !(AOE[i] is ObjBuilding || AOE[i] is BaseTurret))
                {
                    AddParticleTarget(Leblanc, AOE[i], "LeBlanc_Base_RW_tar.troy", AOE[i], 1f);
                    AddParticleTarget(Leblanc, AOE[i], "LeBlanc_Base_RW_tar_02.troy", AOE[i], 1f);
                    if (AOE[i].HasBuff("LeblancChaosOrb")) { IsCrit = true; Damage = BaseDamage + QOrbDamage; AOE[i].RemoveBuffsWithName("LeblancChaosOrb"); }
                    else if (AOE[i].HasBuff("LeblancChaosOrbM")) { IsCrit = true; Damage = BaseDamage + ROrbDamage; AOE[i].RemoveBuffsWithName("LeblancChaosOrbM"); }
                    else { IsCrit = false; Damage = BaseDamage; }
                    AOE[i].TakeDamage(Leblanc, Damage, DamageType.DAMAGE_TYPE_MAGICAL, DamageSource.DAMAGE_SOURCE_SPELL, IsCrit);
                }
            }
        }
    }
    public class LeblancSlideReturnM : ISpellScript
    {
        public SpellScriptMetadata ScriptMetadata => new SpellScriptMetadata()
        {
            TriggersSpellCasts = true
        };
    }
}