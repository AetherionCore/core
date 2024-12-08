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
    class DefensiveBallCurl : IBuffGameScript
    {
        public BuffScriptMetaData BuffMetaData { get; set; } = new BuffScriptMetaData
        {
            BuffType = BuffType.COMBAT_ENCHANCER,
            BuffAddType = BuffAddType.REPLACE_EXISTING
        };

        float Speed;
        AttackableUnit Target;
        private Spell spell;
        Buff ibuff;
        Particle p;

        public StatsModifier StatsModifier { get; private set; } = new StatsModifier();

        public void OnActivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {
            ibuff = buff;
            spell = ownerSpell;
            if (unit.Model == "Rammus")
            {
                unit.ChangeModel("RammusDBC");
            }

            if (unit is ObjAIBase obj)
            {
                StatsModifier.MoveSpeed.PercentBonus -= 0.15f;
                obj.AddStatModifier(StatsModifier);
                p = AddParticleTarget(obj, obj, "defensiveballcurl_buf", obj, 10f, 1, "");
                ApiEventManager.OnTakeDamage.AddListener(this, obj, TakeDamage, false);
            }
        }

        public void TakeDamage(DamageData damageData)
        {
            if (damageData.Target.HasBuff("DefensiveBallCurl") && !(damageData.Attacker is ObjBuilding || damageData.Attacker is BaseTurret))
            {
                var damage = 50f;
                damageData.Attacker.TakeDamage(damageData.Target, damage, DamageType.DAMAGE_TYPE_MAGICAL, DamageSource.DAMAGE_SOURCE_SPELL, false);
                AddParticleTarget(damageData.Target, damageData.Attacker, "thornmail_tar", damageData.Attacker, 10f, 1, "");
            }
        }

        public void OnDeactivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {
            RemoveParticle(p);
            if (unit.Model == "RammusDBC")
            {
                unit.ChangeModel("Rammus");
            }
        }
    }
}