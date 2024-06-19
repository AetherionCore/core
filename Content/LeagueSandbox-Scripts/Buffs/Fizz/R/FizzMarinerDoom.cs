using GameServerCore.Enums;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.API;
using LeagueSandbox.GameServer.GameObjects;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.Buildings;
using LeagueSandbox.GameServer.GameObjects.SpellNS;
using LeagueSandbox.GameServer.GameObjects.SpellNS.Missile;
using LeagueSandbox.GameServer.GameObjects.SpellNS.Sector;
using LeagueSandbox.GameServer.GameObjects.StatsNS;
using LeagueSandbox.GameServer.Scripting.CSharp;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;

namespace Buffs
{
    class FizzMarinerDoom : IBuffGameScript
    {
        public BuffScriptMetaData BuffMetaData { get; set; } = new BuffScriptMetaData
        {
            BuffType = BuffType.COMBAT_ENCHANCER,
            BuffAddType = BuffAddType.REPLACE_EXISTING
        };

        public StatsModifier StatsModifier { get; private set; } = new StatsModifier();

        Buff ThisBuff;
        Particle p;
        Particle p2;
        Particle p3;
        int previousIndicatorState;

        public void OnActivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {
            ThisBuff = buff;
            var owner = ownerSpell.CastInfo.Owner;
            string particles;
            p = AddParticleTarget(owner, unit, "Fizz_UltimateMissile_Orbit", unit, buff.Duration);
            p2 = AddParticleTarget(owner, unit, "Fizz_Ring_Green.troy", unit, buff.Duration);
        }
        public void OnDeactivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {
            if (unit != null && !unit.IsDead)
            {
                var owner = ownerSpell.CastInfo.Owner;
                Minion FizzShark = AddMinion(owner, "FizzShark", "FizzShark", unit.Position, owner.Team, owner.SkinID, true, false);
                AddBuff("FizzSharkBuff", 0.001f, 1, ownerSpell, FizzShark, FizzShark.Owner, false);
                AddParticleTarget(owner, unit, "Fizz_SharkSplash", unit);
                AddParticleTarget(owner, unit, "Fizz_SharkSplash_Ground", unit);
                if (p != null)
                {
                    p.SetToRemove();
                    p2.SetToRemove();
                }
                var units = GetUnitsInRange(unit.Position, 260f, true);
                for (int i = 0; i < units.Count; i++)
                {
                    if (units[i].Team != owner.Team && !(units[i] is ObjBuilding || units[i] is BaseTurret))
                    {
                        var AP = owner.Stats.AbilityPower.Total * 0.65f;
                        var RLevel = owner.GetSpell("FizzJump").CastInfo.SpellLevel;
                        var damage = 200 + (125 * (RLevel - 1)) + AP;
                        units[i].TakeDamage(owner, damage, DamageType.DAMAGE_TYPE_MAGICAL, DamageSource.DAMAGE_SOURCE_SPELL, false);
                        AddParticleTarget(owner, units[i], ".troy", units[i], 1f);
                        AddParticleTarget(owner, units[i], ".troy", units[i], 1f);
                    }
                }
            }
        }
        public void OnUpdate(float diff)
        {
        }
    }
}