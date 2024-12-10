using System.Numerics;
using GameServerCore.Enums;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.API;
using LeagueSandbox.GameServer.Scripting.CSharp;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using LeagueSandbox.GameServer.GameObjects.StatsNS;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using LeagueSandbox.GameServer.GameObjects.SpellNS;
using LeagueSandbox.GameServer.GameObjects;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;

namespace Buffs
{
    class VolibearQ : IBuffGameScript
    {
        public BuffScriptMetaData BuffMetaData { get; set; } = new BuffScriptMetaData
        {
            BuffType = BuffType.COMBAT_ENCHANCER,
            BuffAddType = BuffAddType.REPLACE_EXISTING
        };
        public StatsModifier StatsModifier { get; private set; } = new StatsModifier();

        Particle pbuff;
        Particle pbuff2;
        Particle pbuff3;
        Buff thisBuff;
        ObjAIBase owner;

        public void OnActivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {
            thisBuff = buff;
            if (unit is ObjAIBase ai)
            {
                var owner = ownerSpell.CastInfo.Owner as Champion;
                OverrideAnimation(ai, "spell1_run", "RUN");
                StatsModifier.MoveSpeed.PercentBonus += 0.4f;
                AddParticleTarget(owner, ai, "Volibear_Q_cas", ai, buff.Duration, 1f);
                AddParticleTarget(owner, ai, "Volibear_Q_cas_02", ai, buff.Duration, 1f);
                pbuff = AddParticleTarget(owner, ai, "volibear_Q_attack_buf", ai, buff.Duration, 1f, "R_HAND");
                pbuff2 = AddParticleTarget(owner, ai, "volibear_Q_attack_buf", ai, buff.Duration, 1f, "L_HAND");
                pbuff3 = AddParticleTarget(owner, ai, "volibear_q_speed_buf", ai, buff.Duration, 1f);
                StatsModifier.Range.FlatBonus = 50.0f;
                unit.AddStatModifier(StatsModifier);
                SealSpellSlot(owner, SpellSlotType.SpellSlots, 0, SpellbookType.SPELLBOOK_CHAMPION, true);
                ApiEventManager.OnLaunchAttack.AddListener(this, owner, OnLaunchAttack, false);
                owner.SkipNextAutoAttack();
                owner.CancelAutoAttack(false, true);
            }
        }

        public void OnDeactivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {
            var owner = ownerSpell.CastInfo.Owner as Champion;
            OverrideAnimation(unit, "RUN", "spell1_run");
            OverrideAnimation(unit, "IDLE1", "spell1_idle");
            RemoveParticle(pbuff);
            RemoveParticle(pbuff2);
            RemoveParticle(pbuff3);
            RemoveBuff(thisBuff);
            if (buff.TimeElapsed >= buff.Duration)
            {
                ApiEventManager.OnLaunchAttack.RemoveListener(this);
            }
            if (unit is ObjAIBase ai)
            {
                SealSpellSlot(ai, SpellSlotType.SpellSlots, 0, SpellbookType.SPELLBOOK_CHAMPION, false);
            }
        }

        public void OnLaunchAttack(Spell spell)
        {
            if (thisBuff != null && thisBuff.StackCount != 0 && !thisBuff.Elapsed())
            {
                spell.CastInfo.Owner.RemoveBuff(thisBuff);
                var owner = spell.CastInfo.Owner as Champion;
                spell.CastInfo.Owner.SkipNextAutoAttack();
                SpellCast(spell.CastInfo.Owner, 0, SpellSlotType.ExtraSlots, false, spell.CastInfo.Owner.TargetUnit, Vector2.Zero);
                SealSpellSlot(owner, SpellSlotType.SpellSlots, 0, SpellbookType.SPELLBOOK_CHAMPION, false);
                thisBuff.DeactivateBuff();
            }
        }
        public void OnUpdate(float diff)
        {
        }
    }
}