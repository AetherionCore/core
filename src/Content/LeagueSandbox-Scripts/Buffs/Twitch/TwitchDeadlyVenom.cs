using GameServerCore.Enums;
using LeagueSandbox.GameServer.GameObjects.StatsNS;
using LeagueSandbox.GameServer.Scripting.CSharp;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.GameObjects;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.GameObjects.SpellNS;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using LeagueSandbox.GameServer.API;
using System.Numerics;
using System.Linq;
using GameServerCore;
using System.Collections.Generic;
using GameServerCore.Enums;
using GameServerLib.GameObjects.AttackableUnits;

namespace Buffs
{
    internal class TwitchDeadlyVenom : IBuffGameScript
    {
        Buff Bleed;
        float Damage;
        Particle Stack;
        Particle StackM;
        ObjAIBase Twitch;
        float Time = 900f;
        AttackableUnit Unit;
        public BuffScriptMetaData BuffMetaData { get; set; } = new BuffScriptMetaData
        {
            BuffType = BuffType.DAMAGE,
            BuffAddType = BuffAddType.STACKS_AND_RENEWS,
            MaxStacks = 6
        };


        public StatsModifier StatsModifier { get; private set; } = new StatsModifier();
        Dictionary<int, KeyValuePair<string, string>> Particles = new Dictionary<int, KeyValuePair<string, string>>
            {
               { 1, new KeyValuePair<string, string>("twitch_poison_counter_01", "twitch_poison_counter_minion_01") },
               { 2, new KeyValuePair<string, string>("twitch_poison_counter_02", "twitch_poison_counter_minion_02") },
               { 3, new KeyValuePair<string, string>("twitch_poison_counter_03", "twitch_poison_counter_minion_03") },
               { 4, new KeyValuePair<string, string>("twitch_poison_counter_04", "twitch_poison_counter_minion_04") },
               { 5, new KeyValuePair<string, string>("twitch_poison_counter_05", "twitch_poison_counter_minion_05") },
               { 6, new KeyValuePair<string, string>("twitch_poison_counter_06", "twitch_poison_counter_minion_06") },
            };
        public void OnActivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {
            Unit = unit;
            Bleed = buff;
            RemoveParticle(Stack);
            RemoveParticle(StackM);
            Twitch = ownerSpell.CastInfo.Owner as Champion;
            ApiEventManager.OnDeath.AddListener(this, Unit, OnDeath, true);
            Stack = AddParticleTarget(Twitch, unit, Particles[buff.StackCount].Key, unit, 25000, 1, "C_BuffBone_Glb_Center_Loc");
            StackM = AddParticleTarget(Twitch, unit, Particles[buff.StackCount].Value, unit, 25000, 1, "C_BuffBone_Glb_Center_Loc");
        }
        public void OnDeath(DeathData deathData)
        {
            Bleed.DeactivateBuff();
            Twitch.Spells[0].SetCooldown(0, true);
        }
        public void OnDeactivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {
            RemoveParticle(Stack);
            RemoveParticle(StackM);
        }
        public void OnUpdate(float diff)
        {
            Time += diff;
            if (Time >= 1000.0f && Unit != null)
            {
                Unit.TakeDamage(Twitch, Bleed.StackCount * (Twitch.Stats.Level + (Twitch.Stats.AbilityPower.FlatBonus * 0.3f)), DamageType.DAMAGE_TYPE_TRUE, DamageSource.DAMAGE_SOURCE_PERIODIC, false);
                Time = 0f;
            }
        }
    }
}