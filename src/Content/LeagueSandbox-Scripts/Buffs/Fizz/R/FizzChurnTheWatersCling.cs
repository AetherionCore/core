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
    class FizzChurnTheWatersCling : IBuffGameScript
    {
        public BuffScriptMetaData BuffMetaData { get; set; } = new BuffScriptMetaData
        {
            BuffType = BuffType.COMBAT_ENCHANCER,
            BuffAddType = BuffAddType.REPLACE_EXISTING
        };

        public StatsModifier StatsModifier { get; private set; } = new StatsModifier();

        Buff ThisBuff;
        Minion FizzBait;
        Particle p;
        Particle p2;
        Particle p3;
        int previousIndicatorState;

        public void OnActivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {
            ThisBuff = buff;
            FizzBait = unit as Minion;
            var ownerSkinID = FizzBait.Owner.SkinID;
            string particles;
            p = AddParticleTarget(FizzBait.Owner, FizzBait, "Fizz_UltimateMissile_Orbit", FizzBait, buff.Duration);
            p2 = AddParticleTarget(FizzBait.Owner, FizzBait, "Fizz_Ring_Green.troy", FizzBait, buff.Duration);
        }
        public void OnDeactivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {
            if (FizzBait != null && !FizzBait.IsDead)
            {
                AddParticle(FizzBait.Owner, null, "Fizz_SharkSplash", FizzBait.Position);
                AddParticle(FizzBait.Owner, null, "Fizz_SharkSplash_Ground", FizzBait.Position);
                if (p != null)
                {
                    p.SetToRemove();
                    p2.SetToRemove();
                }
                SetStatus(FizzBait, StatusFlags.NoRender, true);
                AddParticle(FizzBait.Owner, null, "", FizzBait.Position);
                FizzBait.TakeDamage(FizzBait, 10000f, DamageType.DAMAGE_TYPE_TRUE, DamageSource.DAMAGE_SOURCE_INTERNALRAW, DamageResultType.RESULT_NORMAL);
                var units = GetUnitsInRange(FizzBait.Position, 260f, true);
                for (int i = 0; i < units.Count; i++)
                {
                    if (units[i].Team != FizzBait.Owner.Team && !(units[i] is ObjBuilding || units[i] is BaseTurret))
                    {
                        var AP = FizzBait.Owner.Stats.AbilityPower.Total * 0.65f;
                        var RLevel = FizzBait.Owner.GetSpell("FizzJump").CastInfo.SpellLevel;
                        var damage = 200 + (125 * (RLevel - 1)) + AP;
                        units[i].TakeDamage(FizzBait.Owner, damage, DamageType.DAMAGE_TYPE_MAGICAL, DamageSource.DAMAGE_SOURCE_SPELL, false);
                        AddParticleTarget(FizzBait.Owner, units[i], ".troy", units[i], 1f);
                        AddParticleTarget(FizzBait.Owner, units[i], ".troy", units[i], 1f);
                    }
                }
            }
        }
        public void OnUpdate(float diff)
        {
        }
    }
}