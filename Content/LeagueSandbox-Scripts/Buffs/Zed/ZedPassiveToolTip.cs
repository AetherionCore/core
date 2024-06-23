using GameServerCore.Enums;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.API;
using LeagueSandbox.GameServer.GameObjects;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using LeagueSandbox.GameServer.GameObjects.SpellNS;
using LeagueSandbox.GameServer.GameObjects.StatsNS;
using LeagueSandbox.GameServer.Scripting.CSharp;
using System.Numerics;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;

namespace Buffs
{
    internal class ZedPassiveToolTip : IBuffGameScript
    {
        ObjAIBase Zed;
        public BuffScriptMetaData BuffMetaData { get; set; } = new BuffScriptMetaData
        {
            BuffAddType = BuffAddType.REPLACE_EXISTING
        };
        public StatsModifier StatsModifier { get; private set; } = new StatsModifier();
        public void OnActivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {
            Zed = ownerSpell.CastInfo.Owner as Champion;
            AddParticleTarget(Zed, unit, "Zed_Passive_Stage1.troy", unit);
            AddParticleTarget(Zed, unit, "zed_passive_proc_tar.troy", unit);
            AddParticleTarget(Zed, unit, "Zed_Passive_Proc_Tar_Noblood.troy", unit);
            unit.TakeDamage(Zed, unit.Stats.HealthPoints.Total * 0.1f, DamageType.DAMAGE_TYPE_MAGICAL, DamageSource.DAMAGE_SOURCE_PERIODIC, false);
        }
    }
}