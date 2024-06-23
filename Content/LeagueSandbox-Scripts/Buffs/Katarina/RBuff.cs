using GameServerCore.Enums;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.GameObjects;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using LeagueSandbox.GameServer.GameObjects.SpellNS;
using LeagueSandbox.GameServer.GameObjects.StatsNS;
using LeagueSandbox.GameServer.Scripting.CSharp;
using System.Linq;
using System.Numerics;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using GameServerCore;

namespace Buffs
{
    class KatarinaR : IBuffGameScript
    {
        public BuffScriptMetaData BuffMetaData { get; set; } = new BuffScriptMetaData
        {
            BuffType = BuffType.COMBAT_DEHANCER,
            BuffAddType = BuffAddType.RENEW_EXISTING,
            MaxStacks = 1
        };

        public StatsModifier StatsModifier { get; private set; } = new StatsModifier();


        private ObjAIBase Owner;
        float somerandomTick;
        AttackableUnit Target1;
        AttackableUnit Target2;
        AttackableUnit Target3;
        float finaldamage;
        Particle p;

        Spell spell;
        public void OnActivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {
            Champion champion = unit as Champion;
            Owner = ownerSpell.CastInfo.Owner;
            spell = ownerSpell;
            var owner = spell.CastInfo.Owner;
            var AP = spell.CastInfo.Owner.Stats.AbilityPower.Total * 0.25f;
            var AD = spell.CastInfo.Owner.Stats.AttackDamage.FlatBonus * 0.375f;
            float damage = 15f + ( 20f * spell.CastInfo.SpellLevel) + AP + AD;
            finaldamage = damage;
            p = AddParticleTarget(owner, owner, "Katarina_deathLotus_cas.troy", owner, lifetime: 2.5f, bone: "C_BUFFBONE_GLB_CHEST_LOC");


            var champs = GetChampionsInRange(owner.Position, 500f, true).OrderBy(enemy => Vector2.Distance(enemy.Position, owner.Position)).ToList();
            if (champs.Count > 3)
            {
                foreach (var enemy in champs.GetRange(0, 4)
                     .Where(x => x.Team == CustomConvert.GetEnemyTeam(owner.Team)))
                {
                    SpellCast(owner, 0, SpellSlotType.ExtraSlots, true, enemy, owner.Position);
                    if (Target1 == null) Target1 = enemy;
                    else if (Target2 == null) Target2 = enemy;
                    else if (Target3 == null) Target3 = enemy;
                    enemy.TakeDamage(Owner, finaldamage, DamageType.DAMAGE_TYPE_MAGICAL, DamageSource.DAMAGE_SOURCE_SPELLAOE, false);
                }
            }
            else
            {
                foreach (var enemy in champs.GetRange(0, champs.Count)
                    .Where(x => x.Team == CustomConvert.GetEnemyTeam(owner.Team)))
                {
                    SpellCast(owner, 0, SpellSlotType.ExtraSlots, true, enemy, owner.Position);
                    if (Target1 == null) Target1 = enemy;
                    else if (Target2 == null) Target2 = enemy;
                    else if (Target3 == null) Target3 = enemy;
                    enemy.TakeDamage(Owner, finaldamage, DamageType.DAMAGE_TYPE_MAGICAL, DamageSource.DAMAGE_SOURCE_SPELLAOE, false);
                }
            }
        }
        public void OnDeactivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {
            RemoveParticle(p);
            PlayAnimation(Owner, "Idle", 1);
        }

        public void OnUpdate(float diff)
        {
            somerandomTick += diff;
            if (somerandomTick >= 250f)
            {
                if (Target1 != null) Target1.TakeDamage(Owner, finaldamage, DamageType.DAMAGE_TYPE_MAGICAL, DamageSource.DAMAGE_SOURCE_SPELLAOE, false);
                if (Target2 != null) Target2.TakeDamage(Owner, finaldamage, DamageType.DAMAGE_TYPE_MAGICAL, DamageSource.DAMAGE_SOURCE_SPELLAOE, false);
                if (Target3 != null) Target3.TakeDamage(Owner, finaldamage, DamageType.DAMAGE_TYPE_MAGICAL, DamageSource.DAMAGE_SOURCE_SPELLAOE, false);
                somerandomTick = 0;
            }

        }
    }
}