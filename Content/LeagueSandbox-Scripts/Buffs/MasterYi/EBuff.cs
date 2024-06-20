using GameServerCore.Enums;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using LeagueSandbox.GameServer.GameObjects;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.GameObjects.SpellNS;
using LeagueSandbox.GameServer.GameObjects.StatsNS;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.Scripting.CSharp;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using LeagueSandbox.GameServer.API;

namespace Buffs
{
    internal class WujuStyleSuperChargedVisual : IBuffGameScript
    {
        public BuffScriptMetaData BuffMetaData { get; set; } = new BuffScriptMetaData
        {
            BuffType = BuffType.COMBAT_ENCHANCER,
            BuffAddType = BuffAddType.REPLACE_EXISTING
        };

        private ObjAIBase Owner;
        private Spell daspell;
        private ObjAIBase daowner;

        public StatsModifier StatsModifier { get; private set; } = new StatsModifier();

        Buff thisBuff;
        AttackableUnit Target;

        public void OnActivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {
            Owner = ownerSpell.CastInfo.Owner;
            daowner = Owner;
            daspell = ownerSpell;
            SealSpellSlot(daowner, SpellSlotType.SpellSlots, 2, SpellbookType.SPELLBOOK_CHAMPION, true);
            ApiEventManager.OnLaunchAttack.AddListener(this, ownerSpell.CastInfo.Owner, TargetTakePoison, false);
        }

        public void TargetTakePoison(Spell spell)
        {
            var owner = daspell.CastInfo.Owner as Champion;
            Target = spell.CastInfo.Targets[0].Unit;
            var Elevel = owner.GetSpell("WujuStyle").CastInfo.SpellLevel;
            var AD = owner.Stats.AttackDamage.Total * 0.35f;
            var Damage = 30 + (10 * (Elevel - 1)) + AD;
            Target.TakeDamage(owner, Damage, DamageType.DAMAGE_TYPE_TRUE, DamageSource.DAMAGE_SOURCE_PERIODIC, false);
        }

        public void OnDeactivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {
            ApiEventManager.OnLaunchAttack.RemoveListener(this, unit as ObjAIBase);
            SealSpellSlot(ownerSpell.CastInfo.Owner, SpellSlotType.SpellSlots, 2, SpellbookType.SPELLBOOK_CHAMPION, false);
        }

        public void OnUpdate(float diff)
        {
        }
    }
}