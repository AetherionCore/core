using GameServerCore.Enums;
using LeagueSandbox.GameServer.GameObjects.StatsNS;
using LeagueSandbox.GameServer.Scripting.CSharp;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.GameObjects;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.GameObjects.SpellNS;
using LeagueSandbox.GameServer.API;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using LeagueSandbox.GameServer.GameObjects.SpellNS.Missile;
using LeagueSandbox.GameServer.GameObjects.SpellNS.Sector;
using System.Numerics;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;

namespace Buffs
{
    internal class Crowstorm : IBuffGameScript
    {
        public BuffScriptMetaData BuffMetaData { get; set; } = new BuffScriptMetaData
        {
            BuffType = BuffType.COMBAT_ENCHANCER
        };

        public BuffAddType BuffAddType => BuffAddType.RENEW_EXISTING;
        public int MaxStacks => 1;
        public bool IsHidden => false;
        private Particle red;
        private Particle green;
        private Spell originSpell;

        public StatsModifier StatsModifier { get; private set; } = new StatsModifier();

        private ObjAIBase Owner;
        public SpellSector DRMundoWAOE;

        public void OnActivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {
            Owner = ownerSpell.CastInfo.Owner;
            originSpell = ownerSpell;
            ApiEventManager.OnSpellHit.AddListener(this, ownerSpell, TargetExecute, false);
            var spellPos = new Vector2(originSpell.CastInfo.TargetPositionEnd.X, originSpell.CastInfo.TargetPositionEnd.Z);
            red = AddParticle(Owner, Owner, "Crowstorm_green_cas.troy", spellPos, lifetime: buff.Duration);
            green = AddParticle(Owner, Owner, "Crowstorm_red_cas", spellPos, lifetime: buff.Duration);

            DRMundoWAOE = ownerSpell.CreateSpellSector(new SectorParameters
            {
                BindObject = ownerSpell.CastInfo.Owner,
                Length = 800f,
                Tickrate = 2,
                CanHitSameTargetConsecutively = true,
                OverrideFlags = SpellDataFlags.AffectEnemies | SpellDataFlags.AffectNeutral | SpellDataFlags.AffectMinions | SpellDataFlags.AffectHeroes,
                Type = SectorType.Area
            });
        }

        public void TargetExecute(Spell spell, AttackableUnit target, SpellMissile missile, SpellSector sector)
        {
            float AP = Owner.Stats.AbilityPower.Total * 0.45f;
            float damage = 25f + (100 * spell.CastInfo.SpellLevel) + AP;

            target.TakeDamage(Owner, damage, DamageType.DAMAGE_TYPE_MAGICAL, DamageSource.DAMAGE_SOURCE_SPELLAOE, false);
        }

        public void OnDeactivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {
            RemoveParticle(red);
            RemoveParticle(green);
            ApiEventManager.OnSpellHit.RemoveListener(this);
            DRMundoWAOE.SetToRemove();
        }

        public void OnUpdate(float diff)
        {
        }
    }
}