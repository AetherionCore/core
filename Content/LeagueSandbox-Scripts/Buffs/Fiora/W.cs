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
using GameServerLib.GameObjects.AttackableUnits;

namespace Buffs
{
    internal class FioraRiposte : IBuffGameScript
    {
        public BuffScriptMetaData BuffMetaData { get; set; } = new BuffScriptMetaData
        {
            BuffType = BuffType.COMBAT_ENCHANCER
        };

        public StatsModifier StatsModifier { get; private set; } = new StatsModifier();

        Buff thisBuff;
        Particle highlander;
        string particle;
        float findamage;

        public void OnActivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {
            thisBuff = buff;
            if (unit is ObjAIBase owner)
            {
                var ELevel = owner.GetSpell("FioraQ").CastInfo.SpellLevel;
                SealSpellSlot(owner, SpellSlotType.SpellSlots, 1, SpellbookType.SPELLBOOK_CHAMPION, true);
                ApiEventManager.OnTakeDamage.AddListener(this, owner, TakeDamage, false);
            }
        }
        public void TakeDamage(DamageData damageData)
        {
            findamage -= damageData.Damage;
            thisBuff.DeactivateBuff();
        }
        public void OnDeactivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {
            RemoveParticle(highlander);
            if (buff.TimeElapsed >= buff.Duration)
            {
                ApiEventManager.OnTakeDamage.RemoveListener(this);
                //ApiEventManager.OnLaunchAttack.RemoveListener(this);
            }
            if (unit is ObjAIBase ai)
            {
                SealSpellSlot(ai, SpellSlotType.SpellSlots, 1, SpellbookType.SPELLBOOK_CHAMPION, false);
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