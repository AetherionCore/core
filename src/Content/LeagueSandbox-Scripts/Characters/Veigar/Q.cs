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
    public class VeigarBalefulStrike : ISpellScript
    {
        int ticks;
        ObjAIBase Owner;
        StatsModifier statsModifier = new StatsModifier();
        Spell Spell;
        float stacks;

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
            Owner = owner;

        }

        public void TargetExecute(Spell spell, AttackableUnit target, SpellMissile missile, SpellSector sector)
        {
            var owner = spell.CastInfo.Owner;
            var ownerSkinID = owner.SkinID;
            var APratio = owner.Stats.AbilityPower.Total * 0.6f;
            var damage = 80f + ((spell.CastInfo.SpellLevel - 1) * 45) + APratio;
            var StacksPerLevel = spell.CastInfo.SpellLevel;

            target.TakeDamage(owner, damage, DamageType.DAMAGE_TYPE_MAGICAL, DamageSource.DAMAGE_SOURCE_SPELL, false);
            if (ownerSkinID == 8)
            {
                AddParticleTarget(owner, null, "Veigar_Skin08_Q_tar.troy", target);
            }
            else
            {
                AddParticleTarget(owner, null, "Veigar_Base_Q_tar.troy", target);
            }

            if (target.IsDead)
            {
                if (target is Champion)
                {
                    var buffer = owner.Stats.AbilityPower.FlatBonus;

                    statsModifier.AbilityPower.FlatBonus = owner.Stats.AbilityPower.FlatBonus + (StacksPerLevel + 2) - buffer;
                    owner.AddStatModifier(statsModifier);
                }
                else
                {
                    var buffer = owner.Stats.AbilityPower.FlatBonus;

                    statsModifier.AbilityPower.FlatBonus = owner.Stats.AbilityPower.FlatBonus + 1f - buffer;
                    owner.AddStatModifier(statsModifier);
                }
                if (ownerSkinID == 8)
                {
                    AddParticleTarget(owner, null, "Veigar_Skin08_Q_powerup.troy", owner);
                }
                else
                {
                    AddParticleTarget(owner, null, "Veigar_Base_Q_powerup.troy", owner);
                }
            }
        }

        public void OnUpdate(float diff)
        {
            Owner.Stats.ManaRegeneration.FlatBonus = Owner.Stats.ManaRegeneration.BaseValue * ((100 / Owner.Stats.ManaPoints.Total) * ((Owner.Stats.ManaPoints.Total - Owner.Stats.CurrentMana) / 100)); //I'm too lazy to make this properly
        }
    }
}