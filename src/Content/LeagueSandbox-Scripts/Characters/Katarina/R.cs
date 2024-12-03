using GameServerCore.Enums;
using LeagueSandbox.GameServer.Scripting.CSharp;
using LeagueSandbox.GameServer.API;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using LeagueSandbox.GameServer.GameObjects.SpellNS;
using System.Numerics;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using GameServerLib.GameObjects.AttackableUnits;
using GameServerCore;
using LeagueSandbox.GameServer.GameObjects.SpellNS.Missile;
using LeagueSandbox.GameServer.GameObjects.SpellNS.Sector;

namespace Spells
{
    public class KatarinaR : ISpellScript
    {
        ObjAIBase Katarina;
        public SpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
        {
            NotSingleTargetSpell = true,
            TriggersSpellCasts = true,
            ChannelDuration = 2.5f,
        };
        public void OnSpellChannel(Spell spell)
        {
            Katarina = spell.CastInfo.Owner as Champion;
            AddBuff("KatarinaR", 2.5f, 1, spell, Katarina, Katarina);
        }

        public void OnSpellChannelCancel(Spell spell, ChannelingStopSource reason)
        {
            Katarina.RemoveBuffsWithName("KatarinaR");
        }
    }
    public class KatarinaRMis : ISpellScript
    {
        float Damage;
        float MarkDamage;
        private Spell RMis;
        private ObjAIBase Katarina;
        public SpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
        {
            MissileParameters = new MissileParameters
            {
                Type = MissileType.Target
            },
            TriggersSpellCasts = true,
            IsDamagingSpell = true
        };
        public void OnActivate(ObjAIBase owner, Spell spell)
        {
            RMis = spell;
            Katarina = owner = spell.CastInfo.Owner as Champion;
            ApiEventManager.OnSpellHit.AddListener(this, RMis, TargetExecute, false);
        }
        public void TargetExecute(Spell spell, AttackableUnit target, SpellMissile missile, SpellSector sector)
        {
            Damage = 15 + (20f * Katarina.Spells[3].CastInfo.SpellLevel) + (Katarina.Stats.AbilityPower.FlatBonus * 0.25f) + (Katarina.Stats.AttackDamage.FlatBonus * 0.375f);
            target.TakeDamage(Katarina, Damage, DamageType.DAMAGE_TYPE_PHYSICAL, DamageSource.DAMAGE_SOURCE_SPELL, false);
            MarkDamage = (15f * Katarina.Spells[0].CastInfo.SpellLevel) + (Katarina.Stats.AbilityPower.FlatBonus * 0.15f);
            if (target.HasBuff("KatarinaQMark"))
            {
                target.TakeDamage(Katarina, MarkDamage, DamageType.DAMAGE_TYPE_MAGICAL, DamageSource.DAMAGE_SOURCE_PROC, false);
                RemoveBuff(target, "KatarinaQMark");
            }
            target.TakeDamage(Katarina, Damage, DamageType.DAMAGE_TYPE_MAGICAL, DamageSource.DAMAGE_SOURCE_SPELLAOE, false);
            AddParticleTarget(Katarina, target, "katarina_deathLotus_tar.troy", target, 1f);
        }
    }
}