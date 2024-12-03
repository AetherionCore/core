using GameServerCore.Enums;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.Scripting.CSharp;
using System;
using LeagueSandbox.GameServer.GameObjects;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using LeagueSandbox.GameServer.GameObjects.SpellNS;
using LeagueSandbox.GameServer.GameObjects.StatsNS;
using LeagueSandbox.GameServer.API;
using System.Numerics;
using GameServerLib.GameObjects.AttackableUnits;

namespace Buffs
{
    internal class LeblancSoulShackleM : IBuffGameScript
    {
        Particle P;
        Particle P2;
        Particle P3;
        Buff Shackle;
        ObjAIBase Leblanc;
        AttackableUnit Unit;
        public BuffScriptMetaData BuffMetaData { get; set; } = new BuffScriptMetaData
        {
            BuffType = BuffType.DAMAGE,
            BuffAddType = BuffAddType.REPLACE_EXISTING
        };


        public StatsModifier StatsModifier { get; private set; } = new StatsModifier();

        public void OnActivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {
            Unit = unit;
            Shackle = buff;
            Leblanc = ownerSpell.CastInfo.Owner as Champion;
            ApiEventManager.OnDeath.AddListener(this, Unit, OnDeath, true);
            P = AddParticleTarget(Leblanc, Leblanc, "LeBlanc_Base_RE_chain", Unit, buff.Duration, 1, "L_BUFFBONE_GLB_HAND_LOC", "C_BuffBone_Glb_Center_Loc");
            P2 = AddParticleTarget(Leblanc, Unit, "LeBlanc_Base_RE_buf", Unit, buff.Duration, 1, "C_BuffBone_Glb_Center_Loc");
            P3 = AddParticleTarget(Leblanc, Unit, "LeBlanc_Base_RE_indicator", Unit, buff.Duration, 1);
        }
        public void OnDeath(DeathData deathData)
        {
            Shackle.DeactivateBuff();
        }
        public void OnDeactivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {
            RemoveParticle(P);
            RemoveParticle(P2);
            RemoveParticle(P3);
            AddBuff("LeblancSoulShackleNetM", 1.5f, 1, ownerSpell, Unit, Leblanc);
        }
    }
}