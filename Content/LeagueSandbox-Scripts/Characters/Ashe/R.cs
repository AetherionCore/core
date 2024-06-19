using System;
using GameServerCore.Enums;
using LeagueSandbox.GameServer.Scripting.CSharp;
using LeagueSandbox.GameServer.API;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using LeagueSandbox.GameServer.GameObjects.SpellNS;
using LeagueSandbox.GameServer.GameObjects.SpellNS.Missile;
using LeagueSandbox.GameServer.GameObjects.SpellNS.Sector;
using System.Numerics;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;

namespace Spells
{
    public class EnchantedCrystalArrow : ISpellScript
    {
        public SpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
        {
            MissileParameters = new MissileParameters
            {
                Type = MissileType.Circle
            },
            IsDamagingSpell = true,
            TriggersSpellCasts = true
        };

        public void OnActivate(ObjAIBase owner, Spell spell)
        {
            ApiEventManager.OnSpellHit.AddListener(this, spell, TargetExecute, false);
        }

        public void OnDeactivate(ObjAIBase owner, Spell spell)
        {
        }

        public void OnSpellPreCast(ObjAIBase owner, Spell spell, AttackableUnit target, Vector2 start, Vector2 end)
        {
        }

        public void OnSpellCast(Spell spell)
        {
        }

        public void OnSpellPostCast(Spell spell)
        {
        }

        public void TargetExecute(Spell spell, AttackableUnit target, SpellMissile missile, SpellSector sector)
        {
            var owner = spell.CastInfo.Owner;
            var bonusAd = owner.Stats.AttackDamage.Total - owner.Stats.AttackDamage.BaseValue;
            var ap = owner.Stats.AbilityPower.Total * 0.9f;
            var damage = 200 + spell.CastInfo.SpellLevel * 150 + bonusAd + ap;

            target.TakeDamage(owner, damage, DamageType.DAMAGE_TYPE_MAGICAL, DamageSource.DAMAGE_SOURCE_SPELL, false);
            missile.SetToRemove();

            AddParticleTarget(owner, target, "Ashe_Base_R_tar.troy", target, lifetime: 1f);
            AddBuff("Stun", 1.5f, 1, spell, target, owner);
        }

        public void OnSpellChannel(Spell spell)
        {
        }

        public void OnSpellChannelCancel(Spell spell, ChannelingStopSource source)
        {
        }

        public void OnSpellPostChannel(Spell spell)
        {
        }

        public void OnUpdate(float diff)
        {
        }
    }
}