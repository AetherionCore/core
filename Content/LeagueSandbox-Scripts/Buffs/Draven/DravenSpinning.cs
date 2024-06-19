using GameServerCore.Enums;
using LeagueSandbox.GameServer.GameObjects.StatsNS;
using GameServerCore.Scripting.CSharp;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using LeagueSandbox.GameServer.Scripting.CSharp;
using LeagueSandbox.GameServer.GameObjects;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.GameObjects.SpellNS;
using LeagueSandbox.GameServer.API;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;

namespace Buffs
{
    internal class DravenSpinning : IBuffGameScript
    {
        public BuffScriptMetaData BuffMetaData { get; set; } = new BuffScriptMetaData
        {
            BuffType = BuffType.COMBAT_ENCHANCER
        };

        public BuffAddType BuffAddType => BuffAddType.STACKS_AND_CONTINUE;
        public int MaxStacks => 2;
        public bool IsHidden => false;

        public StatsModifier StatsModifier { get; private set; } = new StatsModifier();

        ObjAIBase _owner;

        public void OnActivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {
            _owner = unit as ObjAIBase;
            ApiEventManager.OnHitUnitByAnother.AddListener(this, _owner, TargetExecute, true);
            PlayAnimation(unit, "SPELL3A");
        }

        private void TargetExecute(AttackableUnit source, AttackableUnit target)
        {
            target.TakeDamage(_owner, _owner.Stats.AttackDamage.Total, DamageType.DAMAGE_TYPE_PHYSICAL, DamageSource.DAMAGE_SOURCE_ATTACK, false);
        }

        public void OnDeactivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {
            ApiEventManager.OnHitUnit.RemoveListener(this, _owner);
        }

        public void OnUpdate(float diff)
        {
        }
    }
}
