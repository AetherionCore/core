using System.Numerics;
using GameServerCore.Enums;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.GameObjects;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using LeagueSandbox.GameServer.GameObjects.SpellNS;
using LeagueSandbox.GameServer.GameObjects.StatsNS;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using LeagueSandbox.GameServer.Scripting.CSharp;

namespace Buffs
{
    class ZedPassiveToolTip : IBuffGameScript
    {
        public BuffScriptMetaData BuffMetaData { get; set; } = new BuffScriptMetaData
        {
            BuffAddType = BuffAddType.REPLACE_EXISTING
        };

        public StatsModifier StatsModifier { get; private set; } = new StatsModifier();

        Particle p;

        public void OnActivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {
            var damage = unit.Stats.HealthPoints.Total * 0.1f;
            unit.TakeDamage(ownerSpell.CastInfo.Owner, damage, DamageType.DAMAGE_TYPE_MAGICAL, DamageSource.DAMAGE_SOURCE_PERIODIC, false);
            AddParticleTarget(ownerSpell.CastInfo.Owner, unit, "zed_passive_proc_tar.troy", unit);
            AddParticleTarget(ownerSpell.CastInfo.Owner, unit, "Zed_Passive_Proc_Tar_Noblood.troy", unit);
            AddParticleTarget(ownerSpell.CastInfo.Owner, unit, "Zed_Passive_Stage1.troy", unit);
        }

        public void OnDeactivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {
            RemoveParticle(p);
        }

        public void OnPreAttack(Spell spell)
        {

        }

        public void OnUpdate(float diff)
        {
        }
    }
}