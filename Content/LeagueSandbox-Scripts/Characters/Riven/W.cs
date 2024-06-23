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

namespace Spells
{
    public class RivenMartyr : ISpellScript
    {
        float Damage;
        Spell Martyr;
        ObjAIBase Riven;
        Buff TriCleaveBuff;
        public SpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
        {
            TriggersSpellCasts = true,
            IsDamagingSpell = true
        };
        public void OnActivate(ObjAIBase owner, Spell spell)
        {
            Martyr = spell;
            Riven = owner = spell.CastInfo.Owner as Champion;
            ApiEventManager.OnSpellHit.AddListener(this, spell, TargetExecute, false);
        }
        public void OnSpellPreCast(ObjAIBase owner, Spell spell, AttackableUnit target, Vector2 start, Vector2 end)
        {
            Martyr.CreateSpellSector(new SectorParameters
            {
                Length = 260f,
                SingleTick = true,
                Type = SectorType.Area,
                OverrideFlags = SpellDataFlags.AffectEnemies | SpellDataFlags.AffectNeutral | SpellDataFlags.AffectMinions | SpellDataFlags.AffectHeroes
            });
        }
        public void OnSpellPostCast(Spell spell)
        {
            if (Riven.HasBuff("RivenFengShuiEngine"))
            {
                AddParticleTarget(Riven, Riven, "exile_W_weapon_cas.troy", Riven, bone: "weapon");
                AddParticle(Riven, null, "Riven_Base_W_Ult_Cas.troy", Riven.Position);
                AddParticle(Riven, null, "Riven_Base_W_Ult_Cas_Ground.troy.troy", Riven.Position);
                AddParticleTarget(Riven, Riven, "exile_W_weapon_cas.troy", Riven, bone: "weapon");
            }
            else
            {
                AddParticle(Riven, null, "Riven_Base_W_Cast.troy", Riven.Position);
                AddParticle(Riven, null, "exile_W_cast_02.troy", Riven.Position);
                AddParticleTarget(Riven, Riven, "exile_W_weapon_cas.troy", Riven, bone: "weapon");
            }
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
            var AP = Riven.Stats.AbilityPower.Total * 0.25f;
            var AD = Riven.Stats.AttackDamage.Total * 0.6f;
            Damage = 5f + (spell.CastInfo.SpellLevel * 35f) + AP + AD;
            target.TakeDamage(Riven, Damage, DamageType.DAMAGE_TYPE_PHYSICAL, DamageSource.DAMAGE_SOURCE_SPELLAOE, false);
            AddParticleTarget(Riven, target, "exile_W_tar_02.troy", target, 1f);
            AddBuff("Stun", 0.75f, 1, spell, target, Riven);

        }
    }
}