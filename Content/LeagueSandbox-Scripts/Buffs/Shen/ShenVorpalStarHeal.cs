using GameServerCore.Enums;
using LeagueSandbox.GameServer.GameObjects.StatsNS;
using LeagueSandbox.GameServer.Scripting.CSharp;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.GameObjects;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.GameObjects.SpellNS;
using GameServerCore.Enums;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.API;
using LeagueSandbox.GameServer.GameObjects;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using LeagueSandbox.GameServer.GameObjects.SpellNS;
using LeagueSandbox.GameServer.GameObjects.SpellNS.Missile;
using LeagueSandbox.GameServer.GameObjects.SpellNS.Sector;
using LeagueSandbox.GameServer.GameObjects.StatsNS;
using LeagueSandbox.GameServer.Scripting.CSharp;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using System.Numerics;

namespace Buffs
{
    internal class ShenVorpalStarHeal : IBuffGameScript
    {
        float Time;
        float Health;
        Particle Heal;
        ObjAIBase Shen;
        Spell VorpalStar;
        AttackableUnit Unit;
        public BuffScriptMetaData BuffMetaData { get; set; } = new BuffScriptMetaData
        {
            BuffType = BuffType.HEAL,
            BuffAddType = BuffAddType.REPLACE_EXISTING
        };
        public StatsModifier StatsModifier { get; private set; } = new StatsModifier();
        public void OnActivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {
            Unit = unit;
            VorpalStar = ownerSpell;
            Shen = buff.SourceUnit as Champion;
            Heal = AddParticleTarget(Shen, unit, "shen_vorpalStar_lifetap_tar_02", unit, buff.Duration);
            Health = (2 + (4 * Shen.Spells[0].CastInfo.SpellLevel) + (Shen.Stats.HealthPoints.Total * 0.015f)) / 9;
        }
        public void OnDeactivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {
            RemoveParticle(Heal);
        }
        public void OnUpdate(float diff)
        {
            Time += diff;
            if (Time >= 300)
            {
                Time = 0;
                Unit.TakeHeal(Shen, Health, VorpalStar);
            }
        }
    }
}