using GameServerCore.Enums;
using GameServerCore.Scripting.CSharp;
using GameServerLib.GameObjects.AttackableUnits;
using LeaguePackets.Game.Events;
using LeagueSandbox.GameServer.API;
using LeagueSandbox.GameServer.GameObjects;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.Buildings;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.Buildings.AnimatedBuildings;
using LeagueSandbox.GameServer.GameObjects.SpellNS;
using LeagueSandbox.GameServer.GameObjects.StatsNS;
using LeagueSandbox.GameServer.Scripting.CSharp;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;

namespace Buffs
{
    internal class MordekaiserMaceOfSpades : IBuffGameScript
    {
        public BuffScriptMetaData BuffMetaData { get; set; } = new BuffScriptMetaData
        {
            BuffType = BuffType.COMBAT_ENCHANCER,
            BuffAddType = BuffAddType.REPLACE_EXISTING
        };

        public StatsModifier StatsModifier { get; private set; } = new StatsModifier();

        Spell Spell;
        Buff Buff;

        public void OnActivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {
            Spell = ownerSpell;
            Buff = buff;
            ownerSpell.CastInfo.Owner.CancelAutoAttack(true);
            if (unit is ObjAIBase obj)
            {
                ApiEventManager.OnHitUnit.AddListener(this, obj, TargetExecute, true);
            }
        }

        public void TargetExecute(DamageData damageData)
        {
            var owner = damageData.Attacker;
            var ADratio = owner.Stats.AttackDamage.FlatBonus;
            var APratio = owner.Stats.AbilityPower.Total * 0.4f;
            var damage = 80f + (30 * (Spell.CastInfo.SpellLevel - 1)) + ADratio + APratio;
            bool isCrit = false;

            AddParticleTarget(owner, owner, "mordakaiser_siphonOfDestruction_self.troy", owner, 1f);

            var units = GetUnitsInRange(owner.Position, 300f, true);
            for (var i = units.Count - 1; i >= 0; i--)
            {
                if (units[i].Team == owner.Team || units[i] is BaseTurret || units[i] is Nexus || units[i] is ObjBuilding || units[i] is LaneTurret)
                {
                    units.RemoveAt(i);
                }
            }

            string particles = "mordakaiser_maceOfSpades_tar.troy";
            for (var i = 0; i < units.Count; i++)
            {
                if ((units.Count) == 1)
                {
                    damage *= 1.65f;
                    isCrit = true;
                    particles = "mordakaiser_maceOfSpades_tar2.troy";
                }
                units[i].TakeDamage(owner, damage, DamageType.DAMAGE_TYPE_MAGICAL, DamageSource.DAMAGE_SOURCE_ATTACK, isCrit);
                AddParticleTarget(owner, owner, particles, units[i], 1f);
            }

            Buff.DeactivateBuff();
        }
        public void OnDeactivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {
            ApiEventManager.OnHitUnit.RemoveListener(this);
        }

        public void OnPreAttack(Spell spell)
        {

        }

        public void OnUpdate(float diff)
        {
        }
    }
}
