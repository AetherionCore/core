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
using LeaguePackets.Game.Events;

namespace Buffs
{
    class AzirR : IBuffGameScript
    {
        public BuffScriptMetaData BuffMetaData { get; set; } = new BuffScriptMetaData
        {
            BuffType = BuffType.COMBAT_ENCHANCER,
            BuffAddType = BuffAddType.REPLACE_EXISTING
        };

        public StatsModifier StatsModifier { get; private set; } = new StatsModifier();

        Buff ThisBuff;
        ObjAIBase Owner;

        public void OnActivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {
            ThisBuff = buff;
            Owner = ownerSpell.CastInfo.Owner;
            PlayAnimation(unit, "Spawn");
            //unit.UpdateMoveOrder(OrderType.AttackTo, true);						
            AddParticleTarget(Owner, unit, "", Owner);
            AddParticleTarget(Owner, unit, "", Owner);
            AddParticleTarget(Owner, unit, "", Owner);
            AddParticleTarget(Owner, unit, "", Owner);
            AddParticleTarget(Owner, unit, "", Owner, 10, 1, "head", "head");
        }
        public void OnDeactivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {
            AddParticle(Owner, null, "", unit.Position);
            if (unit != null && !unit.IsDead)
            {
                unit.TakeDamage(unit, 10000f, DamageType.DAMAGE_TYPE_TRUE, DamageSource.DAMAGE_SOURCE_INTERNALRAW, DamageResultType.RESULT_NORMAL);
            }
        }
        public void OnUpdate(float diff)
        {
        }
    }
}