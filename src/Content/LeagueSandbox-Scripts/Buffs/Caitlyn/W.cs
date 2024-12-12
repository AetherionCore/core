using GameServerCore.Enums;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using LeagueSandbox.GameServer.GameObjects.StatsNS;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.Scripting.CSharp;
using LeagueSandbox.GameServer.GameObjects;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.GameObjects.SpellNS;
using LeagueSandbox.GameServer.API;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.Buildings;

namespace Buffs
{
    class CaitlynTrap : IBuffGameScript
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
        ObjAIBase owner;
        AttackableUnit Unit;
        Particle p;

        public StatsModifier StatsModifier { get; private set; } = new StatsModifier();

        public void OnActivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {
            Unit = unit;
            owner = ownerSpell.CastInfo.Owner;
            ibuff = buff;
            spell = ownerSpell;

            if (unit is ObjAIBase obj)
            {
                p = AddParticleTarget(owner, obj, "caitlyn_Base_yordleTrap_idle", obj, 100f, 1, "");
                ApiEventManager.OnCollision.AddListener(this, obj, Publish, false);
            }

        }

        public void OnDeactivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {
            RemoveParticle(p);
            if (unit is ObjAIBase obj)
            {
                unit.TakeDamage(unit, 1000000, DamageType.DAMAGE_TYPE_TRUE, DamageSource.DAMAGE_SOURCE_SPELL, false);
                AddParticleTarget(owner, unit, "caitlyn_Base_yordleTrap_trigger_sound", unit, 10f, 1, "");
                ApiEventManager.OnPreAttack.RemoveListener(this, obj as ObjAIBase);
                ApiEventManager.OnCollision.RemoveListener(this, obj as ObjAIBase);

            }
        }

        public void Publish(GameObject owner, GameObject target)
        {
            if (target.Team != owner.Team && !(target is ObjBuilding || target is BaseTurret))
            {
                AOE(spell);
                ibuff.DeactivateBuff();
            }
        }

        public void AOE(Spell spell)
        {
            var owner = spell.CastInfo.Owner;
            var ap = owner.Stats.AbilityPower.Total * 0.6f;
            var damage = 80 + spell.CastInfo.SpellLevel * 50 + ap;
            var units = GetUnitsInRange(Unit.Position, 100f, true);
            for (int i = 0; i < units.Count; i++)
            {
                if (units[i].Team != owner.Team && !(units[i] is ObjBuilding || units[i] is BaseTurret))
                {
                    units[i].TakeDamage(owner, damage, DamageType.DAMAGE_TYPE_MAGICAL, DamageSource.DAMAGE_SOURCE_SPELL, false);
                    AddBuff("Stun", 1.5f, 1, spell, units[i], owner);
                    AddParticleTarget(owner, units[i], "caitlyn_Base_yordleTrap_impact_debuf", units[i], 1.5f, 1, "");
                    AddParticleTarget(owner, units[i], "caitlyn_Base_yordleTrap_trigger", units[i], 10f, 1, "");
                    AddParticleTarget(owner, units[i], "caitlyn_Base_yordleTrap_trigger_02", units[i], 10f, 1, "");
                }
            }
        }
    }
}
