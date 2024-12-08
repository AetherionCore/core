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
    class PowerBall : IBuffGameScript
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
        float DamageManaTimer;
        float T = 0.15f;
        float M;

        public StatsModifier StatsModifier { get; private set; } = new StatsModifier();

        public void OnActivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {
            owner = ownerSpell.CastInfo.Owner;
            ibuff = buff;
            spell = ownerSpell;
            if (unit.Model == "Rammus")
            {
                unit.ChangeModel("RammusPB");
            }

            if (unit is ObjAIBase obj)
            {
                StatsModifier.MoveSpeed.PercentBonus += T;
                obj.AddStatModifier(StatsModifier);
            }
        }

        public void OnDeactivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {
            AOE(ownerSpell);
            if (unit.Model == "RammusPB")
            {
                unit.ChangeModel("Rammus");
            }
            if (unit is ObjAIBase obj)
            {
                ApiEventManager.OnPreAttack.RemoveListener(this, obj as ObjAIBase);
                ApiEventManager.OnCollision.RemoveListener(this, obj as ObjAIBase);

            }
            CreateTimer((float)0.0001, () =>
            {
                PlayAnimation(unit, "spell1", 0.3f);
            });
        }

        public void AOE(Spell spell)
        {
            var owner = spell.CastInfo.Owner;
            var ap = owner.Stats.AbilityPower.Total * 0.8f;
            var damage = 70 + spell.CastInfo.SpellLevel * 45 + ap;
            AddParticleTarget(owner, owner, "PowerBallStop", owner);
            var units = GetUnitsInRange(GetPointFromUnit(owner, 125f), 260f, true);
            for (int i = 0; i < units.Count; i++)
            {
                if (units[i].Team != owner.Team && !(units[i] is ObjBuilding || units[i] is BaseTurret))
                {
                    units[i].TakeDamage(owner, damage, DamageType.DAMAGE_TYPE_MAGICAL, DamageSource.DAMAGE_SOURCE_SPELL, false);
                    AddBuff("Stun", 1f, 1, spell, units[i], owner);
                    AddParticleTarget(owner, units[i], "PowerBallHit", units[i], 10f, 1, "");
                }
            }
        }

        public void OnUpdate(float diff)
        {
            if (owner != null && ibuff != null && spell != null)
            {
                DamageManaTimer += diff;

                if (DamageManaTimer >= 10f)
                {
                    M = T * 1.2f;
                    var units = GetUnitsInRange(GetPointFromUnit(owner, 125f), 75f, true);
                    for (int i = 0; i < units.Count; i++)
                    {
                        if (units[i].Team != owner.Team && !(units[i] is ObjBuilding || units[i] is BaseTurret))
                        {
                            ibuff.DeactivateBuff();
                        }
                    }
                    DamageManaTimer = 0;
                }
            }
        }
    }
}