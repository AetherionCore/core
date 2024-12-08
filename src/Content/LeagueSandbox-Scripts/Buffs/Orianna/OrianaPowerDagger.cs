using System.Numerics;
using GameServerCore.Enums;
using GameServerCore.Scripting.CSharp;
using GameServerLib.GameObjects.AttackableUnits;
using LeaguePackets.Game.Common;
using LeaguePackets.Game.Events;
using LeagueSandbox.GameServer.API;
using LeagueSandbox.GameServer.GameObjects;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.Buildings;
using LeagueSandbox.GameServer.GameObjects.SpellNS;
using LeagueSandbox.GameServer.GameObjects.StatsNS;
using LeagueSandbox.GameServer.Scripting.CSharp;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;

namespace Buffs
{
    class OrianaPowerDagger : IBuffGameScript
    {
        private ObjAIBase thisOwner;
        private Spell thisSpell;
        private Buff thisBuff;
        private float Damage = 0f;
        public BuffScriptMetaData BuffMetaData { get; set; } = new BuffScriptMetaData
        {
            BuffType = BuffType.COMBAT_ENCHANCER,
            BuffAddType = BuffAddType.STACKS_AND_RENEWS,
            MaxStacks = 2
        };

        public StatsModifier StatsModifier { get; private set; } = new StatsModifier();

        public void OnActivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {
            thisOwner = ownerSpell.CastInfo.Owner;
            thisSpell = ownerSpell;
            thisBuff = buff;
            ApiEventManager.OnHitUnit.AddListener(this, thisOwner, TargetExecute, false);
        }

        private void TargetExecute(DamageData data)
        {
            var damage = CalculatekDamage(thisOwner.Stats.Level) * thisBuff.StackCount;
            data.Target.TakeDamage(thisOwner, damage, DamageType.DAMAGE_TYPE_MAGICAL, DamageSource.DAMAGE_SOURCE_ATTACK, false);
        }

        public void OnDeactivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {
            ApiEventManager.OnHitUnit.RemoveListener(this, thisOwner);
        }

        private float CalculatekDamage(int ownerLevel)
        {
            var baseDamage = 0f;

            if (ownerLevel >= 16)
            {
                baseDamage = 10f;
            }
            else if (ownerLevel >= 13)
            {
                baseDamage = 8.4f;
            }
            else if (ownerLevel >= 10)
            {
                baseDamage = 6.8f;
            }
            else if (ownerLevel >= 7)
            {
                baseDamage = 5.2f;
            }
            else if (ownerLevel >= 4)
            {
                baseDamage = 3.6f;
            }
            else if (ownerLevel >= 1)
            {
                baseDamage = 2f;
            }

            return baseDamage + (thisOwner.Stats.AbilityPower.Total * .03f);
        }
    }
}