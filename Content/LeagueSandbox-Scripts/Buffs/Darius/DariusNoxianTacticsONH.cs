using GameServerCore.Enums;
using LeagueSandbox.GameServer.GameObjects.StatsNS;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.Scripting.CSharp;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.GameObjects;
using LeagueSandbox.GameServer.GameObjects.SpellNS;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using System.Collections.Generic;
using LeagueSandbox.GameServer.API;
using GameServerLib.GameObjects.AttackableUnits;
using System.Numerics;

namespace Buffs
{
    internal class DariusNoxianTacticsONH : IBuffGameScript
    {
        Buff AttackBuff;
        ObjAIBase Darius;
        float TrueCooldown;
        public BuffScriptMetaData BuffMetaData { get; set; } = new BuffScriptMetaData
        {
            BuffType = BuffType.COMBAT_ENCHANCER,
            BuffAddType = BuffAddType.REPLACE_EXISTING
        };


        public StatsModifier StatsModifier { get; private set; } = new StatsModifier();

        public void OnActivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {
            AttackBuff = buff;
            Darius = ownerSpell.CastInfo.Owner as Champion;
            Darius.SkipNextAutoAttack();
            Darius.CancelAutoAttack(true);
            StatsModifier.Range.FlatBonus = 50.0f;
            Darius.AddStatModifier(StatsModifier);
            OverrideAnimation(Darius, "Spell2_Run", "RUN");
            SealSpellSlot(Darius, SpellSlotType.SpellSlots, 1, SpellbookType.SPELLBOOK_CHAMPION, true);
            ApiEventManager.OnLaunchAttack.AddListener(this, Darius, OnLaunchAttack, false);
        }

        public void OnLaunchAttack(Spell spell)
        {
            if (AttackBuff != null && AttackBuff.StackCount != 0 && !AttackBuff.Elapsed())
            {
                Darius.SkipNextAutoAttack();
                SpellCast(Darius, 0, SpellSlotType.ExtraSlots, false, Darius.TargetUnit, Vector2.Zero);
                AttackBuff.DeactivateBuff();
            }
        }
        public void OnDeactivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {
            OverrideAnimation(Darius, "RUN", "Spell2_Run");
            ApiEventManager.OnLaunchAttack.RemoveListener(this);
            SealSpellSlot(Darius, SpellSlotType.SpellSlots, 1, SpellbookType.SPELLBOOK_CHAMPION, false);
            TrueCooldown = 8 * (1 + Darius.Stats.CooldownReduction.Total);
            Darius.Spells[1].SetCooldown(TrueCooldown, true);
        }
    }
}