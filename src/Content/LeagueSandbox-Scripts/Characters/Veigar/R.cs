using System.Numerics;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using LeagueSandbox.GameServer.Scripting.CSharp;
using GameServerCore.Scripting.CSharp;
using GameServerCore.Enums;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.GameObjects.SpellNS;
using LeagueSandbox.GameServer.GameObjects.SpellNS.Missile;
using LeagueSandbox.GameServer.GameObjects.SpellNS.Sector;
using LeagueSandbox.GameServer.API;
using System.Collections.Generic;
using LeagueSandbox.GameServer.GameObjects;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.Buildings;
using GameServerLib.GameObjects.AttackableUnits;
using LeaguePackets.Game.Common;
using LeagueSandbox.GameServer.GameObjects.StatsNS;

namespace Spells
{
    public class VeigarPrimordialBurst : ISpellScript
    {
        StatsModifier statsModifier = new StatsModifier();

        public SpellScriptMetadata ScriptMetadata => new SpellScriptMetadata()
        {
            TriggersSpellCasts = true,
            IsDamagingSpell = true,
            MissileParameters = new MissileParameters
            {
                Type = MissileType.Target
            }
        };

        public void OnActivate(ObjAIBase owner, Spell spell)
        {
            ApiEventManager.OnSpellHit.AddListener(this, spell, TargetExecute, false);
        }

        public void TargetExecute(Spell spell, AttackableUnit target, SpellMissile missile, SpellSector sector)
        {
            var owner = spell.CastInfo.Owner;
            var ownerSkinID = owner.SkinID;
            var APratio = owner.Stats.AbilityPower.Total * 1.2f;
            var targetAP = target.Stats.AbilityPower.Total * 0.8f;
            var damage = 250 + ((spell.CastInfo.SpellLevel - 1) * 125) + APratio + targetAP;
            var StacksPerLevel = spell.CastInfo.SpellLevel;

            target.TakeDamage(owner, damage, DamageType.DAMAGE_TYPE_MAGICAL, DamageSource.DAMAGE_SOURCE_SPELL, false);

            string particles;
            if (ownerSkinID == 8)
            {
                particles = "Veigar_Skin08_R_tar.troy";
            }
            else
            {
                particles = "Veigar_Base_R_tar.troy";
            }

            if (!target.IsDead)
            {
                AddParticleTarget(owner, target, particles, target, 1f);
            }
            else
            {
                var buffer = owner.Stats.AbilityPower.FlatBonus;

                statsModifier.AbilityPower.FlatBonus += (StacksPerLevel + 2) - buffer;
                owner.AddStatModifier(statsModifier);

                if (ownerSkinID == 8)
                {
                    AddParticle(owner, target, "Veigar_Skin08_R_tar.troy", target.Position, 1f);

                }
                else
                {
                    AddParticle(owner, target, "Veigar_Base_R_tar.troy", target.Position, 1f);
                }
            }
        }
    }
}