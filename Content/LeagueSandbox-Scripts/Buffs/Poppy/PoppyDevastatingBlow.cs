using GameServerCore.Enums;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.API;
using LeagueSandbox.GameServer.GameObjects;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using LeagueSandbox.GameServer.GameObjects.SpellNS;
using LeagueSandbox.GameServer.GameObjects.SpellNS.Missile;
using LeagueSandbox.GameServer.GameObjects.SpellNS.Sector;
using LeagueSandbox.GameServer.GameObjects.StatsNS;
using LeagueSandbox.GameServer.Scripting.CSharp;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using GameServerLib.GameObjects.AttackableUnits;

namespace Buffs
{
    internal class PoppyDevastatingBlow : IBuffGameScript
    {
        public BuffScriptMetaData BuffMetaData { get; set; } = new BuffScriptMetaData
        {
            BuffType = BuffType.COMBAT_ENCHANCER,
            BuffAddType = BuffAddType.REPLACE_EXISTING,
            MaxStacks = 1
        };

        private ObjAIBase Owner;
        private float damage2;
        Spell Spell;
        Particle p;
        AttackableUnit target;

        public StatsModifier StatsModifier { get; private set; } = new StatsModifier();

        public void OnActivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {
            var Owner = ownerSpell.CastInfo.Owner;
            Owner.CancelAutoAttack(true);
            Spell = ownerSpell;
            AddParticleTarget(Owner, Owner, "Poppy_DevastatingBlow_buf.troy", Owner, 5f);

            ApiEventManager.OnHitUnit.AddListener(Owner, ownerSpell.CastInfo.Owner, TargetTakeDamage, true);
            Owner.SkipNextAutoAttack();
        }
        public void TargetTakeDamage(DamageData damage)
        {
            var Owner = damage.Attacker;
            target = damage.Target;
            var ap = Owner.Stats.AbilityPower.Total * 0.60f;
            var ad = Owner.Stats.AttackDamage.Total;
            var targetHealth = target.Stats.HealthPoints.Total * 0.08f;
            var basedmg = Spell.CastInfo.SpellLevel * 20;
            var basepluspercent = targetHealth + basedmg;
            if ((basepluspercent) > (75 * Spell.CastInfo.SpellLevel)) basepluspercent = Spell.CastInfo.SpellLevel * 75;
            damage2 = basepluspercent + ap + ad;
            target.TakeDamage(Owner, damage2, DamageType.DAMAGE_TYPE_MAGICAL, DamageSource.DAMAGE_SOURCE_ATTACK, false);
            AddParticleTarget(Owner, target, "Poppy_DevastatingBlow_tar.troy", target, 0.5f);
            Owner.RemoveBuffsWithName("PoppyDevastatingBlow");
        }
        public void OnDeactivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {
            var Owner = ownerSpell.CastInfo.Owner;
            SealSpellSlot(Owner, SpellSlotType.SpellSlots, 0, SpellbookType.SPELLBOOK_CHAMPION, false);
        }
    }
}