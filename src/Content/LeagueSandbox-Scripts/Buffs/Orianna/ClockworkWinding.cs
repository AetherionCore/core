using System.Numerics;
using GameServerCore.Enums;
using GameServerCore.Scripting.CSharp;
using GameServerLib.GameObjects.AttackableUnits;
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
    class ClockworkWinding : IBuffGameScript
    {
        private ObjAIBase _owner;
        private Spell _spell;
        public BuffScriptMetaData BuffMetaData { get; set; } = new BuffScriptMetaData
        {
            BuffType = BuffType.COMBAT_ENCHANCER,
            BuffAddType = BuffAddType.STACKS_AND_RENEWS,
            MaxStacks = 1
        };

        public StatsModifier StatsModifier { get; private set; } = new StatsModifier();

        public void OnActivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {
            _owner = ownerSpell.CastInfo.Owner;
            _spell = ownerSpell;
            ApiEventManager.OnHitUnit.AddListener(this, _owner, TargetExecute, false);
        }

        private void TargetExecute(DamageData data)
        {
            data.Target.TakeDamage(_owner, CalculateDamage(), DamageType.DAMAGE_TYPE_MAGICAL, DamageSource.DAMAGE_SOURCE_ATTACK, false);
        }

        public void OnDeactivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {
            ApiEventManager.OnHitUnit.RemoveListener(this, _owner);
        }

        private float CalculateDamage()
        {
            var ownerLevel = _owner.Stats.Level;
            var baseDamage = 0;

            if (ownerLevel >= 16)
            {
                baseDamage = 50;
            }
            else if (ownerLevel >= 13)
            {
                baseDamage = 42;
            }
            else if (ownerLevel >= 10)
            {
                baseDamage = 34;
            }
            else if (ownerLevel >= 7)
            {
                baseDamage = 26;
            }
            else if (ownerLevel >= 4)
            {
                baseDamage = 18;
            }
            else if (ownerLevel >= 1)
            {
                baseDamage = 10;
            }

            return baseDamage + (_owner.Stats.AbilityPower.Total * .15f);
        }

        public void OnUpdate(float diff)
        {
        }
    }
}