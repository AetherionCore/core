using GameServerCore.Enums;
using LeagueSandbox.GameServer.GameObjects.StatsNS;
using LeagueSandbox.GameServer.API;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.Scripting.CSharp;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.GameObjects;
using LeagueSandbox.GameServer.GameObjects.SpellNS;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
﻿
namespace Buffs
{
    internal class FioraDanceStrike : IBuffGameScript
    {
        public BuffScriptMetaData BuffMetaData { get; set; } = new BuffScriptMetaData
        {
            BuffAddType = BuffAddType.REPLACE_EXISTING
        };

        public StatsModifier StatsModifier { get; private set; } = new StatsModifier();

        private Buff ThisBuff;
        AttackableUnit Unit;

        public void OnActivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {
            Unit = unit;
            ThisBuff = buff;
            var owner = ownerSpell.CastInfo.Owner as Champion;
            owner.StopMovement();
            //owner.SetDashingState(true);			
            AddParticleTarget(owner, owner, "", owner, buff.Duration);
            SealSpellSlot(owner, SpellSlotType.SpellSlots, 0, SpellbookType.SPELLBOOK_CHAMPION, true);
            SealSpellSlot(owner, SpellSlotType.SpellSlots, 1, SpellbookType.SPELLBOOK_CHAMPION, true);
            SealSpellSlot(owner, SpellSlotType.SpellSlots, 2, SpellbookType.SPELLBOOK_CHAMPION, true);
            SealSpellSlot(owner, SpellSlotType.SpellSlots, 3, SpellbookType.SPELLBOOK_CHAMPION, true);
            PlayAnimation(owner, "", 3f);
            buff.SetStatusEffect(StatusFlags.Targetable, false);
            //unit.Stats.SetActionState(ActionState.CAN_MOVE, false);
            SealSpellSlot(owner, SpellSlotType.SummonerSpellSlots, 0, SpellbookType.SPELLBOOK_SUMMONER, true);
            SealSpellSlot(owner, SpellSlotType.SummonerSpellSlots, 1, SpellbookType.SPELLBOOK_SUMMONER, true);
            //buff.SetStatusEffect(StatusFlags.Stunned, true);
            unit.Stats.SetActionState(ActionState.CAN_ATTACK, false);
            buff.SetStatusEffect(StatusFlags.Ghosted, true);
        }

        public void OnDeactivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {
            var owner = ownerSpell.CastInfo.Owner as Champion;
            //owner.SetDashingState(false);
            AddParticleTarget(owner, owner, "", owner);
            StopAnimation(owner, "");
            buff.SetStatusEffect(StatusFlags.Targetable, true);
            //unit.Stats.SetActionState(ActionState.CAN_MOVE, true);
            unit.Stats.SetActionState(ActionState.CAN_ATTACK, true);
            buff.SetStatusEffect(StatusFlags.Ghosted, false);
            //buff.SetStatusEffect(StatusFlags.Stunned, false);
            SealSpellSlot(owner, SpellSlotType.SummonerSpellSlots, 0, SpellbookType.SPELLBOOK_SUMMONER, false);
            SealSpellSlot(owner, SpellSlotType.SummonerSpellSlots, 1, SpellbookType.SPELLBOOK_SUMMONER, false);
            SealSpellSlot(owner, SpellSlotType.SpellSlots, 0, SpellbookType.SPELLBOOK_CHAMPION, false);
            SealSpellSlot(owner, SpellSlotType.SpellSlots, 1, SpellbookType.SPELLBOOK_CHAMPION, false);
            SealSpellSlot(owner, SpellSlotType.SpellSlots, 2, SpellbookType.SPELLBOOK_CHAMPION, false);
            SealSpellSlot(owner, SpellSlotType.SpellSlots, 3, SpellbookType.SPELLBOOK_CHAMPION, false);
        }
        public void OnUpdate(float diff)
        {
        }
    }
}