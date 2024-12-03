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
    internal class XenZhaoBattleCry : IBuffGameScript
    {
        Particle BlueL;
        Particle Blue5;
        Particle Blue1;
        Particle GlowD;
        ObjAIBase XinZhao;
        Particle Intimidate;
        public BuffScriptMetaData BuffMetaData { get; set; } = new BuffScriptMetaData
        {
            BuffType = BuffType.COMBAT_ENCHANCER,
            BuffAddType = BuffAddType.REPLACE_EXISTING,
        };
        public StatsModifier StatsModifier { get; private set; } = new StatsModifier();
        public void OnActivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {
            XinZhao = ownerSpell.CastInfo.Owner as Champion;//BUFFBONE_GLB_WEAPON_1
            AddParticleTarget(XinZhao, XinZhao, "xen_ziou_battleCry_cas.troy", XinZhao, buff.Duration, 1);
            BlueL = AddParticleTarget(XinZhao, XinZhao, "xenziou_battle_cry_weapon_01.troy", XinZhao, buff.Duration, 1, "BUFFBONE_CSTM_WEAPON_1", "BUFFBONE_CSTM_WEAPON_5");
            GlowD = AddParticleTarget(XinZhao, XinZhao, "xen_ziou_battleCry_cas_02.troy", XinZhao, buff.Duration, 1, "BUFFBONE_CSTM_WEAPON_1");
            Blue5 = AddParticleTarget(XinZhao, XinZhao, "xen_ziou_battleCry_cas_03.troy", XinZhao, buff.Duration, 1, "BUFFBONE_CSTM_WEAPON_5");
            Blue1 = AddParticleTarget(XinZhao, XinZhao, "xen_ziou_battleCry_cas_04.troy", XinZhao, buff.Duration, 1, "BUFFBONE_CSTM_WEAPON_1");
            AddParticleTarget(XinZhao, XinZhao, "xen_ziou_battleCry_cas_05.troy", XinZhao, buff.Duration, 1);
            StatsModifier.AttackSpeed.PercentBonus = 0.4f + (0.1f * ownerSpell.CastInfo.SpellLevel);
            XinZhao.AddStatModifier(StatsModifier);
        }
        public void OnDeactivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {
            RemoveParticle(BlueL);
            RemoveParticle(Blue5);
            RemoveParticle(Blue1);
            RemoveParticle(GlowD);
        }
    }
}