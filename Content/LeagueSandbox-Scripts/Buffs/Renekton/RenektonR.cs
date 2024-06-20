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
    internal class RenektonReignOfTheTyrant : IBuffGameScript
    {
        public BuffScriptMetaData BuffMetaData { get; set; } = new BuffScriptMetaData
        {
            BuffType = BuffType.COMBAT_ENCHANCER,
            BuffAddType = BuffAddType.REPLACE_EXISTING,
            MaxStacks = 1
        };

        public StatsModifier StatsModifier { get; private set; } = new StatsModifier();

        Particle p;
        Particle p2;
        ObjAIBase Owner;
        public SpellSector AOE;

        public void OnActivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {
            if (unit is Champion c)
            {
                Owner = c;
                ApiEventManager.OnSpellHit.AddListener(this, ownerSpell, TargetExecute, false);
                AOE = ownerSpell.CreateSpellSector(new SectorParameters
                {
                    BindObject = c,
                    Length = 270f,
                    Tickrate = 1,
                    CanHitSameTargetConsecutively = true,
                    OverrideFlags = SpellDataFlags.AffectEnemies | SpellDataFlags.AffectNeutral | SpellDataFlags.AffectMinions | SpellDataFlags.AffectHeroes,
                    Type = SectorType.Area
                });
                AddParticleTarget(c, c, "Renekton_Base_R_cas", c);
                p = AddParticleTarget(c, c, "Renekton_Base_R_buf", c);
                p.SetToRemove();
                p2 = AddParticleTarget(c, c, "Renekton_Base_R_weapon", c, buff.Duration, 1, "WEAPON");
                //OverrideAnimation(unit, "Run_ULT", "RUN");
                var HealthBuff = 200f + 200f * ownerSpell.CastInfo.SpellLevel;
                StatsModifier.Size.BaseBonus = StatsModifier.Size.BaseBonus + 0.4f;
                StatsModifier.HealthPoints.BaseBonus += HealthBuff;
                unit.AddStatModifier(StatsModifier);
                unit.Stats.CurrentHealth += HealthBuff;
            }
        }
        public void TargetExecute(Spell spell, AttackableUnit target, SpellMissile missile, SpellSector sector)
        {
            var AP = Owner.Stats.AbilityPower.Total * 0.12f;
            var damage = 11f + (8f * Owner.GetSpell("RenektonSliceAndDice").CastInfo.SpellLevel - 1) + AP;

            target.TakeDamage(Owner, damage, DamageType.DAMAGE_TYPE_MAGICAL, DamageSource.DAMAGE_SOURCE_SPELLAOE, false);
        }
        public void OnDeactivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {
            //OverrideAnimation(unit, "RUN", "Run_ULT");
            AOE.SetToRemove();
            RemoveParticle(p);
            RemoveParticle(p2);
        }

        public void OnUpdate(float diff)
        {

        }
    }
}
