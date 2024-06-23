using GameServerCore.Enums;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.GameObjects;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.GameObjects.SpellNS;
using LeagueSandbox.GameServer.GameObjects.StatsNS;
using LeagueSandbox.GameServer.Scripting.CSharp;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using LeagueSandbox.GameServer.API;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using LeagueSandbox.GameServer.GameObjects.SpellNS.Missile;
using LeagueSandbox.GameServer.GameObjects.SpellNS.Sector;
using System.Numerics;

namespace Spells
{
    public class LeblancSoulShackle : ISpellScript
    {
        bool IsCrit;
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

        public void OnActivate(ObjAIBase owner, Spell spell)
        {
            Leblanc = owner = spell.CastInfo.Owner as Champion;
            ApiEventManager.OnSpellHit.AddListener(this, spell, TargetExecute, false);
        }
        public void OnSpellCast(Spell spell)
        {
            AddParticleTarget(Leblanc, Leblanc, "LeBlanc_Base_E_cas", Leblanc, bone: "L_HAND");
            AddParticleTarget(Leblanc, Leblanc, "LeBlanc_Base_E_cas_02", Leblanc, bone: "L_HAND");
        }
        public void OnSpellPostCast(Spell spell)
        {
            Missile = spell.CreateSpellMissile(new MissileParameters { Type = MissileType.Circle, });
        }
        public void TargetExecute(Spell spell, AttackableUnit target, SpellMissile missile, SpellSector sector)
        {
            Missile = missile;
            Missile.SetToRemove();
            AddBuff("LeblancSoulShackle", 1.5f, 1, spell, target, Leblanc);
            AddParticleTarget(Leblanc, target, "LeBlanc_Base_Q_tar", target);
            AddParticleTarget(Leblanc, target, "LeBlanc_Base_E_tar_02", target);
            BaseDamage = 15f + (Leblanc.Spells[2].CastInfo.SpellLevel * 25f) + (Leblanc.Stats.AbilityPower.Total * 0.5f);
            QOrbDamage = BaseDamage + 30 + (Leblanc.Spells[0].CastInfo.SpellLevel * 25f) + (Leblanc.Stats.AbilityPower.Total * 0.65f);
            ROrbDamage = BaseDamage + (Leblanc.Spells[3].CastInfo.SpellLevel * 100f) + (Leblanc.Stats.AbilityPower.Total * 0.65f);
            if (target.HasBuff("LeblancChaosOrb")) { IsCrit = true; Damage = QOrbDamage; target.RemoveBuffsWithName("LeblancChaosOrb"); }
            else if (target.HasBuff("LeblancChaosOrbM")) { IsCrit = true; Damage = ROrbDamage; target.RemoveBuffsWithName("LeblancChaosOrbM"); }
            else { IsCrit = false; Damage = BaseDamage; }
            target.TakeDamage(Leblanc, Damage, DamageType.DAMAGE_TYPE_MAGICAL, DamageSource.DAMAGE_SOURCE_SPELL, IsCrit);
        }
    }
    public class LeblancSoulShackleM : ISpellScript
    {
        bool IsCrit;
        float Damage;
        Spell RESpell;
        float BaseDamage;
        float QOrbDamage;
        float ROrbDamage;
        ObjAIBase Leblanc;
        SpellMissile Missile;
        public SpellScriptMetadata ScriptMetadata => new SpellScriptMetadata()
        {
            TriggersSpellCasts = true
        };

        public void OnActivate(ObjAIBase owner, Spell spell)
        {
            RESpell = spell;
            Leblanc = owner = spell.CastInfo.Owner as Champion;
        }
        public void OnSpellCast(Spell spell)
        {
            AddParticleTarget(Leblanc, Leblanc, "LeBlanc_Base_RE_cas", Leblanc, bone: "L_HAND");
            AddParticleTarget(Leblanc, Leblanc, "LeBlanc_Base_RE_cas_02", Leblanc, bone: "L_HAND");
        }
        public void OnSpellPostCast(Spell spell)
        {
            Missile = spell.CreateSpellMissile(new MissileParameters { Type = MissileType.Circle, });
            ApiEventManager.OnSpellMissileHit.AddListener(this, Missile, TargetExecute, false);
        }
        public void TargetExecute(SpellMissile missile, AttackableUnit target)
        {
            Missile = missile;
            Missile.SetToRemove();
            AddBuff("LeblancSoulShackleM", 1.5f, 1, RESpell, target, Leblanc);
            AddParticleTarget(Leblanc, target, "LeBlanc_Base_RQ_tar", target);
            AddParticleTarget(Leblanc, target, "LeBlanc_Base_RE_tar_02", target);
            BaseDamage = (Leblanc.Spells[3].CastInfo.SpellLevel * 100f) + (Leblanc.Stats.AbilityPower.Total * 0.65f);
            QOrbDamage = BaseDamage + 30 + (Leblanc.Spells[0].CastInfo.SpellLevel * 25f) + (Leblanc.Stats.AbilityPower.Total * 0.65f);
            ROrbDamage = BaseDamage + (Leblanc.Spells[3].CastInfo.SpellLevel * 100f) + (Leblanc.Stats.AbilityPower.Total * 0.65f);
            if (target.HasBuff("LeblancChaosOrb")) { IsCrit = true; Damage = QOrbDamage; target.RemoveBuffsWithName("LeblancChaosOrb"); }
            else if (target.HasBuff("LeblancChaosOrbM")) { IsCrit = true; Damage = ROrbDamage; target.RemoveBuffsWithName("LeblancChaosOrbM"); }
            else { IsCrit = false; Damage = BaseDamage; }
            target.TakeDamage(Leblanc, Damage, DamageType.DAMAGE_TYPE_MAGICAL, DamageSource.DAMAGE_SOURCE_SPELL, IsCrit);
        }
    }
}