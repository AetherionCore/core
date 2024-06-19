using GameServerCore.Enums;
using LeagueSandbox.GameServer.GameObjects.StatsNS;
using LeagueSandbox.GameServer.Scripting.CSharp;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.GameObjects;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.GameObjects.SpellNS;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using LeagueSandbox.GameServer.GameObjects.SpellNS.Sector;
using LeagueSandbox.GameServer.API;
using LeagueSandbox.GameServer.GameObjects.SpellNS.Missile;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;

namespace Buffs
{
    class BurningAgony : IBuffGameScript
    {
        public BuffScriptMetaData BuffMetaData { get; set; } = new BuffScriptMetaData
        {
            BuffType = BuffType.COMBAT_ENCHANCER
        };

        public BuffAddType BuffAddType => BuffAddType.RENEW_EXISTING;
        public int MaxStacks => 1;
        public bool IsHidden => false;

        public StatsModifier StatsModifier { get; private set; } = new StatsModifier();

        private ObjAIBase Owner;
        private Particle p;
        private Particle p2;
        public SpellSector DRMundoWAOE;

        public void OnActivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {
            Owner = ownerSpell.CastInfo.Owner;
            //ApiEventManager.OnSpellSectorHit.AddListener(this, new KeyValuePair<Spell, ObjAIBase>(ownerSpell, Owner), TargetExecute, false);
            ApiEventManager.OnSpellHit.AddListener(this, ownerSpell, TargetExecute, false);
            StatsModifier.Tenacity.FlatBonus += 5 + ownerSpell.CastInfo.SpellLevel;
            unit.AddStatModifier(StatsModifier);

            DRMundoWAOE = ownerSpell.CreateSpellSector(new SectorParameters
            {
                BindObject = ownerSpell.CastInfo.Owner,
                Length = 160f,
                Tickrate = 1,
                CanHitSameTargetConsecutively = true,
                OverrideFlags = SpellDataFlags.AffectEnemies | SpellDataFlags.AffectNeutral | SpellDataFlags.AffectMinions | SpellDataFlags.AffectHeroes,
                Type = SectorType.Area
            });

            p = AddParticleTarget(Owner, unit, "dr_mundo_burning_agony_cas_01.troy", unit, buff.Duration);
            p2 = AddParticleTarget(Owner, unit, "dr_mundo_burning_agony_cas_02.troy", unit, buff.Duration);
        }

        public void TargetExecute(Spell ownerSpell, AttackableUnit target, SpellMissile mis, SpellSector sector)
        {
            float AP = Owner.Stats.AbilityPower.Total * 0.2f;
            float damage = 20f + (15 * ownerSpell.CastInfo.SpellLevel) + AP;

            target.TakeDamage(Owner, damage, DamageType.DAMAGE_TYPE_MAGICAL, DamageSource.DAMAGE_SOURCE_SPELLAOE, false);
        }

        public void OnDeactivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {
            ApiEventManager.OnSpellHit.RemoveListener(this);
            DRMundoWAOE.SetToRemove();

            RemoveParticle(p);
            RemoveParticle(p2);
        }

        public void OnUpdate(float diff)
        {
        }
    }
}