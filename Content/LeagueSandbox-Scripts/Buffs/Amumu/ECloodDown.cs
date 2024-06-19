using GameServerCore.Enums;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.GameObjects;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using LeagueSandbox.GameServer.GameObjects.SpellNS;
using LeagueSandbox.GameServer.GameObjects.StatsNS;
using LeagueSandbox.GameServer.Scripting.CSharp;
using LeagueSandbox.GameServer.API;
using GameServerLib.GameObjects.AttackableUnits;

namespace Buffs
{
    internal class Tantrum : IBuffGameScript
    {
        public BuffScriptMetaData BuffMetaData { get; set; } = new BuffScriptMetaData
        {
            BuffType = BuffType.COMBAT_ENCHANCER,
            BuffAddType = BuffAddType.REPLACE_EXISTING
        };

        public StatsModifier StatsModifier { get; private set; }

        ObjAIBase Owner;
        AttackableUnit Target;
        Spell rspell;
        bool didcast = false;
        float findamage;

        private readonly AttackableUnit target;

        public void OnActivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {
            var owner = ownerSpell.CastInfo.Owner;
            Owner = owner;
            Target = unit;
            rspell = ownerSpell;
            ApiEventManager.OnTakeDamage.AddListener(this, unit, TakeDamage, false);
        }
        public void TakeDamage(DamageData damageData)
        {
			var owner = rspell.CastInfo.Owner;
			owner.GetSpell("Tantrum").LowerCooldown(1);
        }

        public void OnDeactivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {
         
        }
        public void OnUpdate(float diff)
        {         
        }
    }
}
