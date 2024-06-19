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

namespace Buffs
{
    internal class PowerFist : IBuffGameScript
    {
        public BuffScriptMetaData BuffMetaData { get; set; } = new BuffScriptMetaData
        {
            BuffType = BuffType.COMBAT_ENCHANCER
        };

        public BuffAddType BuffAddType => BuffAddType.RENEW_EXISTING;
        public int MaxStacks => 1;
        public bool IsHidden => false;

        public StatsModifier StatsModifier { get; private set; } = new StatsModifier();

        private Buff thisBuff;
        private Spell thisSpell;

        public void OnActivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {
            if (unit is ObjAIBase ai)
            {
                thisBuff = buff;
                thisSpell = ownerSpell;
                SealSpellSlot(ai, SpellSlotType.SpellSlots, 2, SpellbookType.SPELLBOOK_CHAMPION, true);
                // SetDodgePiercing(ai, true);
                ai.CancelAutoAttack(true);
                //ai.SetAutoAttackSpell(ai.GetSpell((byte)SpellSlotType.ExtraSlots + 1), true);
                //ApiEventManager.OnHitUnitByAnother.AddListener(this, ai, OnHitUnit, true);
            }
        }

        public void OnHitUnit(AttackableUnit target, bool isCrit)
        {
            if (thisSpell == null)
            {
                return;
            }

            var ad = thisSpell.CastInfo.Owner.Stats.AttackDamage.Total;

            if (!(target is BaseTurret))
            {
                // BreakSpellShields(target);
                AddBuff("PowerFistSlow", 0.5f, 1, thisSpell, target, thisSpell.CastInfo.Owner);
            }
            //
            //if (thisBuff != null)
            //{
            //    thisBuff.DeactivateBuff();
            //}
        }

        public void OnDeactivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {
            if (unit is ObjAIBase ai)
            {
                ai.CancelAutoAttack(true);
                ai.ResetAutoAttackSpell();
                SealSpellSlot(ai, SpellSlotType.SpellSlots, 2, SpellbookType.SPELLBOOK_CHAMPION, false);
                // SetDodgePiercing(ai, false);
                ApiEventManager.OnHitUnit.RemoveListener(this, ai);
            }
        }

        public void OnUpdate(float diff)
        {
        }
    }
}