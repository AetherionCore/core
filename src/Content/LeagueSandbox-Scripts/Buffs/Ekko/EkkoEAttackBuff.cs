using System.Numerics;
using GameServerCore.Enums;
using GameServerCore.Scripting.CSharp;
using LeaguePackets.Game.Events;
using LeagueSandbox.GameServer.API;
using LeagueSandbox.GameServer.GameObjects;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using LeagueSandbox.GameServer.GameObjects.SpellNS;
using LeagueSandbox.GameServer.GameObjects.StatsNS;
using LeagueSandbox.GameServer.Scripting.CSharp;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;

namespace Buffs
{
    internal class EkkoEAttackBuff : IBuffGameScript
    {
        public BuffScriptMetaData BuffMetaData { get; set; } = new BuffScriptMetaData
        {
            BuffType = BuffType.COMBAT_ENCHANCER,
            BuffAddType = BuffAddType.REPLACE_EXISTING
        };

        public StatsModifier StatsModifier { get; private set; } = new StatsModifier();

        Particle activate;
        Particle activate2;
        Buff thisBuff;
        ObjAIBase owner;

        public void OnActivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {
            thisBuff = buff;
            var owner = ownerSpell.CastInfo.Owner;
            StatsModifier.Range.FlatBonus = 350.0f;
            if (unit is ObjAIBase ai)
            {
                OverrideAnimation(ai, "Spell3_Dash_to_Run", "Run");
            }
            StatsModifier.MoveSpeed.PercentBonus = 0.2f + (0.1f * ownerSpell.CastInfo.SpellLevel);
            unit.AddStatModifier(StatsModifier);
            SealSpellSlot(owner, SpellSlotType.SpellSlots, 2, SpellbookType.SPELLBOOK_CHAMPION, true);
            ApiEventManager.OnLaunchAttack.AddListener(this, owner, OnLaunchAttack, false);
            ApiEventManager.OnPreAttack.AddListener(this, owner, OnPreAttack, false);
            activate = AddParticleTarget(owner, unit, "ekko_base_e_indicator", unit, buff.Duration);
            AddParticleTarget(unit, unit, "ekko_base_e_vanish", unit);
            activate2 = AddParticleTarget(owner, unit, "ekko_base_e_weapon_glow", unit, buff.Duration, 1, "weapon");
            owner.SkipNextAutoAttack();
            owner.CancelAutoAttack(true, true);
        }

        public void OnPreAttack(Spell spell)
        {

            if (thisBuff != null && thisBuff.StackCount != 0 && !thisBuff.Elapsed())
            {
                var owner = spell.CastInfo.Owner as Champion;
                PlayAnimation(owner, "Spell3_Attack", 1.5f);
            }
        }

        public void OnLaunchAttack(Spell spell)
        {

            if (thisBuff != null && thisBuff.StackCount != 0 && !thisBuff.Elapsed())
            {
                var owner = spell.CastInfo.Owner as Champion;
                spell.CastInfo.Owner.SkipNextAutoAttack();
                SpellCast(spell.CastInfo.Owner, 3, SpellSlotType.ExtraSlots, false, spell.CastInfo.Owner.TargetUnit, Vector2.Zero);
                SealSpellSlot(owner, SpellSlotType.SpellSlots, 2, SpellbookType.SPELLBOOK_CHAMPION, false);
                thisBuff.DeactivateBuff();
            }
        }
        public void OnDeactivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {
            RemoveParticle(activate);
            RemoveParticle(activate2);

            if (buff.TimeElapsed >= buff.Duration)
            {
                ApiEventManager.OnLaunchAttack.RemoveListener(this);
            }

            if (unit is ObjAIBase ai)
            {
                OverrideAnimation(ai, "Run", "Spell3_Dash_to_Run");
                SealSpellSlot(ai, SpellSlotType.SpellSlots, 2, SpellbookType.SPELLBOOK_CHAMPION, false);
            }
        }
    }
}