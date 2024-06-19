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

namespace Buffs
{
    internal class FioraFlurry : IBuffGameScript
    {
        public BuffScriptMetaData BuffMetaData { get; set; } = new BuffScriptMetaData
        {
            BuffType = BuffType.COMBAT_ENCHANCER
        };

        public StatsModifier StatsModifier { get; private set; } = new StatsModifier();

        Buff thisBuff;
        Particle highlander;
        string particle;

        public void OnActivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {
            thisBuff = buff;
            if (unit is ObjAIBase owner)
            {
                var ELevel = owner.GetSpell("FioraRiposte").CastInfo.SpellLevel;
                SealSpellSlot(owner, SpellSlotType.SpellSlots, 2, SpellbookType.SPELLBOOK_CHAMPION, true);
                ApiEventManager.OnLaunchAttack.AddListener(this, owner, OnLaunchAttack, false);
                ApiEventManager.OnSpellPostCast.AddListener(this, owner.GetSpell("FioraQ"), OnDash);
                StatsModifier.AttackSpeed.PercentBonus = StatsModifier.AttackSpeed.PercentBonus += (45f + ELevel * 15f) / 100f;
                unit.AddStatModifier(StatsModifier);
            }
        }
        public void OnDash(Spell spell)
        {
            if (thisBuff != null && thisBuff.StackCount != 0 && !thisBuff.Elapsed())
            {
                var owner = spell.CastInfo.Owner as Champion;
                AddBuff("FioraFlurryDummy", 3.0f, 1, spell, owner, owner);
            }
        }
        public void OnLaunchAttack(Spell spell)
        {

            if (thisBuff != null && thisBuff.StackCount != 0 && !thisBuff.Elapsed())
            {
                var owner = spell.CastInfo.Owner as Champion;
                AddBuff("FioraFlurryDummy", 3.0f, 1, spell, owner, owner);
            }
        }
        public void OnDeactivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {
            RemoveParticle(highlander);
            if (buff.TimeElapsed >= buff.Duration)
            {
                ApiEventManager.OnSpellPostCast.RemoveListener(this);
                ApiEventManager.OnLaunchAttack.RemoveListener(this);
            }
            if (unit is ObjAIBase ai)
            {
                SealSpellSlot(ai, SpellSlotType.SpellSlots, 2, SpellbookType.SPELLBOOK_CHAMPION, false);
            }
        }

        private void OnAutoAttack(AttackableUnit target, bool isCrit)
        {
        }

        public void OnUpdate(float diff)
        {
        }
    }
}
