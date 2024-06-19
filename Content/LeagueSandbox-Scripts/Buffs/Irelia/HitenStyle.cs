using LeagueSandbox.GameServer.GameObjects.StatsNS;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.Scripting.CSharp;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using LeagueSandbox.GameServer.GameObjects;
using LeagueSandbox.GameServer.GameObjects.SpellNS;
using GameServerCore.Enums;
using LeagueSandbox.GameServer.API;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;

namespace Buffs
{
    class IreliaHitenStyleCharged : IBuffGameScript
    {
        public BuffScriptMetaData BuffMetaData { get; set; } = new BuffScriptMetaData
        {
            BuffType = BuffType.COMBAT_ENCHANCER,
            BuffAddType = BuffAddType.REPLACE_EXISTING
        };

        AttackableUnit Target;
        private Spell spell;

        public StatsModifier StatsModifier { get; private set; } = new StatsModifier();

        public void OnActivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {
            spell = ownerSpell;
            AddParticleTarget(unit, unit, "irelia_hitenStlye_active_glow.troy", unit, 6f, 1, "WEAPON");

            if (unit is ObjAIBase obj)
            {
                SealSpellSlot(obj, SpellSlotType.SpellSlots, 1, SpellbookType.SPELLBOOK_CHAMPION, true);
                ApiEventManager.OnLaunchAttack.AddListener(this, obj, OnLaunchAttack, false);
            }
        }
        public void OnLaunchAttack(Spell spell)
        {
            var owner = spell.CastInfo.Owner;
            Target = spell.CastInfo.Targets[0].Unit;
            float damage = 20 + (30 * (owner.GetSpell("IreliaHitenStyle").CastInfo.SpellLevel - 1));
            float heal = 10 * spell.CastInfo.SpellLevel;
            owner.Stats.CurrentHealth += heal;
            Target.TakeDamage(owner, damage, DamageType.DAMAGE_TYPE_TRUE, DamageSource.DAMAGE_SOURCE_ATTACK, false);
        }
        public void OnDeactivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {
            ApiEventManager.OnLaunchAttack.RemoveListener(this, unit as ObjAIBase);
            SealSpellSlot(ownerSpell.CastInfo.Owner, SpellSlotType.SpellSlots, 1, SpellbookType.SPELLBOOK_CHAMPION, false);
        }

        public void OnUpdate(float diff)
        {

        }
    }
}