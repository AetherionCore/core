using GameServerCore.Enums;
using LeagueSandbox.GameServer.GameObjects.StatsNS;
using LeagueSandbox.GameServer.Scripting.CSharp;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.GameObjects;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.GameObjects.SpellNS;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using System.Numerics;
using LeagueSandbox.GameServer.API;

namespace Buffs
{
    internal class XenZhaoComboAutoFinish : IBuffGameScript
    {
        int Slot;
        Buff Talon;
        Particle P1;
        Particle P2;
        Particle P3;
        Particle P4;
        ObjAIBase XinZhao;
        float TrueCooldown;
        AttackableUnit Unit;
        public BuffScriptMetaData BuffMetaData { get; set; } = new BuffScriptMetaData
        {
            BuffType = BuffType.COMBAT_ENCHANCER,
            BuffAddType = BuffAddType.STACKS_AND_RENEWS,
            MaxStacks = 3
        };
        public StatsModifier StatsModifier { get; private set; } = new StatsModifier();
        public void OnActivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {
            Talon = buff;
            RemoveParticle(P1);
            RemoveParticle(P2);
            RemoveParticle(P3);
            RemoveParticle(P4);
            Slot = Talon.StackCount - 1;
            XinZhao = ownerSpell.CastInfo.Owner as Champion;
            P1 = AddParticleTarget(XinZhao, XinZhao, "xenZiou_ChainAttack_cas_01.troy", XinZhao, Talon.Duration, 1, "L_Hand");
            P2 = AddParticleTarget(XinZhao, XinZhao, "xenZiou_ChainAttack_cas_01.troy", XinZhao, Talon.Duration, 1, "R_Hand");
            P3 = AddParticleTarget(XinZhao, XinZhao, "xenZiou_ChainAttack_indicator.troy", XinZhao, Talon.Duration, 1, "R_HAND");
            P4 = AddParticleTarget(XinZhao, XinZhao, "xenZiou_ChainAttack_indicator.troy", XinZhao, Talon.Duration, 1, "L_HAND");
            SealSpellSlot(XinZhao, SpellSlotType.SpellSlots, 0, SpellbookType.SPELLBOOK_CHAMPION, true);
            ApiEventManager.OnLaunchAttack.AddListener(this, XinZhao, OnLaunchAttack, false);
        }
        public void OnLaunchAttack(Spell spell)
        {
            Unit = XinZhao.TargetUnit;
            for (byte i = 1; i < 4; i++)
            {
                XinZhao.Spells[i].LowerCooldown(1);
            }
            if (Talon != null && Talon.StackCount != 0 && !Talon.Elapsed())
            {
                SpellCast(XinZhao, Slot, SpellSlotType.ExtraSlots, false, Unit, Vector2.Zero);
            }
        }
        public void OnDeactivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {
            RemoveParticle(P1);
            RemoveParticle(P2);
            RemoveParticle(P3);
            RemoveParticle(P4);
            SealSpellSlot(XinZhao, SpellSlotType.SpellSlots, 0, SpellbookType.SPELLBOOK_CHAMPION, false);
            if (Talon.TimeElapsed >= Talon.Duration) { ApiEventManager.OnLaunchAttack.RemoveListener(this); }
            TrueCooldown = (10 - XinZhao.Spells[0].CastInfo.SpellLevel) * (1 + XinZhao.Stats.CooldownReduction.Total);
            XinZhao.Spells[0].SetCooldown(TrueCooldown, true);
        }
    }
}