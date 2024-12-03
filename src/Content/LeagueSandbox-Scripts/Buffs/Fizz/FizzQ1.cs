using GameServerCore.Enums;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.GameObjects;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using LeagueSandbox.GameServer.GameObjects.SpellNS;
using LeagueSandbox.GameServer.GameObjects.StatsNS;
using LeagueSandbox.GameServer.Scripting.CSharp;
using System.Numerics;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;

namespace Buffs
{
    class FizzQ1 : IBuffGameScript
    {
        public BuffScriptMetaData BuffMetaData { get; set; } = new BuffScriptMetaData
        {
            BuffAddType = BuffAddType.REPLACE_EXISTING
        };

        public StatsModifier StatsModifier { get; private set; } = new StatsModifier();

        ObjAIBase Unit;
        private Buff ThisBuff;
        private readonly AttackableUnit target = Spells.FizzPiercingStrike._target;

        public void OnActivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {
            ThisBuff = buff;
            var owner = ownerSpell.CastInfo.Owner;
            var ad = owner.Stats.AbilityPower.Total * 0.35f;
            var Wdamage = 30 + 10 * owner.GetSpell("FizzPiercingStrike").CastInfo.SpellLevel + ad;
            var damage = 10f + owner.GetSpell("FizzBasicAttack").CastInfo.SpellLevel * 10f + owner.Stats.AttackDamage.Total;
            var QWdamage = Wdamage + damage;
            AddParticleTarget(owner, owner, "Fizz_PiercingStrike.troy", owner, buff.Duration);
            AddParticleTarget(owner, target, "Fizz_PiercingStrike_tar.troy", target);
            var current = new Vector2(owner.Position.X, owner.Position.Y);
            var to = Vector2.Normalize(new Vector2(target.Position.X, target.Position.Y) - current);
            var range = to * 550;
            var trueCoords = current + range;
            buff.SetStatusEffect(StatusFlags.Ghosted, true);
            ForceMovement(unit, null, trueCoords, 1400, 0, 0, 0);
            if (owner.HasBuff("FizzSeastonePassive"))
            {
                target.TakeDamage(unit, QWdamage, DamageType.DAMAGE_TYPE_MAGICAL, DamageSource.DAMAGE_SOURCE_SPELL, false);
                owner.RemoveBuffsWithName("FizzSeastonePassive");
            }
            else
            {
                target.TakeDamage(unit, damage, DamageType.DAMAGE_TYPE_MAGICAL, DamageSource.DAMAGE_SOURCE_SPELL, false);
            }
            if (owner.HasBuff("FizzSeastoneTrident"))
            {
                AddBuff("FizzSeastoneTridentActive", 3f, 1, ownerSpell, target, owner);
            }
            else
            {
            }
        }

        public void OnDeactivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {
        }

        public void OnUpdate(float diff)
        {
        }
    }
}