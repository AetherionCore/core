using System.Numerics;
using GameServerCore.Enums;
using GameServerCore.Scripting.CSharp;
using GameServerLib.GameObjects.AttackableUnits;
using LeaguePackets.Game.Events;
using LeagueSandbox.GameServer.API;
using LeagueSandbox.GameServer.GameObjects;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.Buildings;
using LeagueSandbox.GameServer.GameObjects.SpellNS;
using LeagueSandbox.GameServer.GameObjects.StatsNS;
using LeagueSandbox.GameServer.Scripting.CSharp;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;

namespace Buffs
{
    class RengarR : IBuffGameScript
    {
        public BuffScriptMetaData BuffMetaData { get; set; } = new BuffScriptMetaData
        {
            BuffType = BuffType.COMBAT_ENCHANCER,
            BuffAddType = BuffAddType.REPLACE_EXISTING
        };

        public StatsModifier StatsModifier { get; private set; } = new StatsModifier();

        Particle pbuff;
        Particle pbuff2;
        Buff thisBuff;
        AttackableUnit Target;

        public void OnActivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {
            thisBuff = buff;
            var owner = ownerSpell.CastInfo.Owner as Champion;
            StatsModifier.MoveSpeed.PercentBonus += 0.4f;
            StatsModifier.Range.FlatBonus = 700.0f;
            unit.AddStatModifier(StatsModifier);
            pbuff = AddParticleTarget(unit, unit, "Rengar_Base_R_Buf.troy", unit, buff.Duration);
            AddParticleTarget(unit, unit, "Rengar_Base_R_Alert.troy", unit, buff.Duration);
            AddParticleTarget(unit, unit, "Rengar_Base_R_Alert_Sound.troy", unit, buff.Duration);
            SealSpellSlot(owner, SpellSlotType.SpellSlots, 3, SpellbookType.SPELLBOOK_CHAMPION, true);
            if (unit is ObjAIBase ai)
            {
                ai.SkipNextAutoAttack();
                ai.CancelAutoAttack(true, true);
            }
            ApiEventManager.OnPreAttack.AddListener(this, owner, OnPreAttack, true);
        }

        public void OnDeactivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {
            RemoveParticle(pbuff);
            var owner = ownerSpell.CastInfo.Owner as Champion;
            SealSpellSlot(owner, SpellSlotType.SpellSlots, 3, SpellbookType.SPELLBOOK_CHAMPION, false);
            if (buff.TimeElapsed >= buff.Duration)
            {
                ApiEventManager.OnPreAttack.RemoveListener(this);
            }
        }

        public void OnPreAttack(Spell spell)
        {

            var owner = spell.CastInfo.Owner as Champion;
            owner.SkipNextAutoAttack();
            owner.CancelAutoAttack(true, true);
            AddBuff("RengarPassiveBuffDash", 2.0f, 1, spell, owner, owner);
            //SpellCast(spell.CastInfo.Owner, 0, SpellSlotType.ExtraSlots, false, spell.CastInfo.Owner.TargetUnit, Vector2.Zero);
            SealSpellSlot(owner, SpellSlotType.SpellSlots, 3, SpellbookType.SPELLBOOK_CHAMPION, false);

            if (thisBuff != null)
            {
                thisBuff.DeactivateBuff();
            }
        }
    }
}