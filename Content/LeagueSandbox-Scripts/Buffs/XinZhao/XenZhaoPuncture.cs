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

namespace Buffs
{
    internal class XenZhaoPuncture : IBuffGameScript
    {
        ObjAIBase XinZhao;
        Particle Intimidate;
        public BuffScriptMetaData BuffMetaData { get; set; } = new BuffScriptMetaData
        {
            BuffType = BuffType.DAMAGE,
            BuffAddType = BuffAddType.REPLACE_EXISTING
        };
        public StatsModifier StatsModifier { get; private set; } = new StatsModifier();
        public void OnActivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {
            RemoveParticle(Intimidate);
            XinZhao = ownerSpell.CastInfo.Owner as Champion;
            StatsModifier.Armor.PercentBonus -= 0.15f;
            unit.AddStatModifier(StatsModifier);
            AddBuff("XinZhaoPassive", 3.0f, 1, ownerSpell, XinZhao, XinZhao);
            Intimidate = AddParticleTarget(XinZhao, unit, "xen_ziou_intimidate", unit, buff.Duration);
        }
        public void OnDeactivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {
            RemoveParticle(Intimidate);
        }
    }
}