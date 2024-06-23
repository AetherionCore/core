using GameServerCore.Enums;
using LeagueSandbox.GameServer.GameObjects.StatsNS;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.Scripting.CSharp;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.GameObjects;
using LeagueSandbox.GameServer.GameObjects.SpellNS;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using System.Collections.Generic;
using LeagueSandbox.GameServer.API;
using GameServerLib.GameObjects.AttackableUnits;

namespace Buffs
{
    internal class DariusHemo : IBuffGameScript
    {
        float Damage;
        float ADratio;
        Buff StackBuff;
        Particle Trail;
        Particle Trail6;
        Particle Counter;
        ObjAIBase Darius;
        Particle MaxStack;
        AttackableUnit Unit;
        float TimeSinceLastTick;
        public BuffScriptMetaData BuffMetaData { get; set; } = new BuffScriptMetaData
        {
            BuffType = BuffType.DAMAGE,
            BuffAddType = BuffAddType.STACKS_AND_RENEWS,
            MaxStacks = 5
        };
        Dictionary<int, KeyValuePair<string, string>> Particles = new Dictionary<int, KeyValuePair<string, string>>
            {
               { 1, new KeyValuePair<string, string>("darius_Base_hemo_counter_01", "darius_Base_hemo_bleed_trail_only1") },
               { 2, new KeyValuePair<string, string>("darius_Base_hemo_counter_02", "darius_Base_hemo_bleed_trail_only2") },
               { 3, new KeyValuePair<string, string>("darius_Base_hemo_counter_03", "darius_Base_hemo_bleed_trail_only3") },
               { 4, new KeyValuePair<string, string>("darius_Base_hemo_counter_04", "darius_Base_hemo_bleed_trail_only4") },
               { 5, new KeyValuePair<string, string>("darius_Base_hemo_counter_05", "darius_Base_hemo_bleed_trail_only5") },
            };

        public StatsModifier StatsModifier { get; private set; } = new StatsModifier();

        public void OnActivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {
            StackBuff = buff;
            Unit = unit;
            RemoveParticle(Trail);
            RemoveParticle(Trail6);
            RemoveParticle(Counter);
            RemoveParticle(MaxStack);
            Darius = ownerSpell.CastInfo.Owner as Champion;
            Damage = (18 + (Darius.Stats.AttackDamage.FlatBonus * 0.3f) + Darius.Stats.Level) / 4;
            Trail = AddParticleTarget(Darius, unit, Particles[buff.StackCount].Value, unit, 25000);
            Counter = AddParticleTarget(Darius, unit, Particles[buff.StackCount].Key, unit, 25000, 1, "C_BuffBone_Glb_Center_Loc");
            switch (buff.StackCount)
            {
                case 1:
                    if (unit is Champion)
                    {
                        Speed(ownerSpell);
                    }
                    break;
                case 5:
                    MaxStack = AddParticleTarget(Darius, unit, "darius_Base_passive_overhead_max_stack.troy", unit, 25000);
                    Trail6 = AddParticleTarget(Darius, unit, "darius_Base_hemo_bleed_trail_only6.troy", unit, 25000);
                    break;
            }
            ApiEventManager.OnDeath.AddListener(this, unit, OnDeath, true);
        }
        public void Speed(Spell spell)
        {
            AddBuff("DariusHemoMarker", 5.0f, 1, spell, Darius, Darius);
        }
        public void OnDeath(DeathData deathData)
        {
            StackBuff.DeactivateBuff();
        }
        public void OnDeactivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {
            RemoveParticle(Trail);
            RemoveParticle(Trail6);
            RemoveParticle(Counter);
            RemoveParticle(MaxStack);
        }
        public void OnUpdate(float diff)
        {
            TimeSinceLastTick += diff;
            if (TimeSinceLastTick >= 1000.0f && Unit != null)
            {
                Unit.TakeDamage(Darius, Damage * StackBuff.StackCount, DamageType.DAMAGE_TYPE_MAGICAL, DamageSource.DAMAGE_SOURCE_SPELL, false);
                TimeSinceLastTick = 0f;
            }
        }
    }
}