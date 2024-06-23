using GameServerCore.Enums;
using LeagueSandbox.GameServer.Scripting.CSharp;
using LeagueSandbox.GameServer.API;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using LeagueSandbox.GameServer.GameObjects.SpellNS;
using GameServerLib.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.GameObjects;
using LeagueSandbox.GameServer.GameObjects.StatsNS;

namespace Buffs
{
    internal class Sheen : IBuffGameScript
    {
        Buff AttackBuff;
        ObjAIBase Itemowner;
        public BuffScriptMetaData BuffMetaData { get; set; } = new BuffScriptMetaData
        {
            BuffType = BuffType.COMBAT_ENCHANCER,
            BuffAddType = BuffAddType.REPLACE_EXISTING
        };


        public StatsModifier StatsModifier { get; private set; } = new StatsModifier();

        public void OnActivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {
            AttackBuff = buff;
            Itemowner = ownerSpell.CastInfo.Owner as Champion;
            ApiEventManager.OnHitUnit.AddListener(this, Itemowner, TargetExecute, false);
        }

        public void TargetExecute(DamageData data)
        {
            if (AttackBuff != null && AttackBuff.StackCount != 0 && !AttackBuff.Elapsed())
            {
                AttackBuff.DeactivateBuff();
                data.Target.TakeDamage(Itemowner, Itemowner.Stats.AttackDamage.BaseValue, DamageType.DAMAGE_TYPE_PHYSICAL, DamageSource.DAMAGE_SOURCE_SPELLAOE, false);
            }
        }
        public void OnDeactivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {
            ApiEventManager.OnHitUnit.RemoveListener(this);
        }
    }
}