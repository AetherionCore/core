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
    internal class LeblancMIApplicator : IBuffGameScript
    {
        Buff Passive;
        ObjAIBase Leblanc;
        AttackableUnit Unit;
        public BuffScriptMetaData BuffMetaData { get; set; } = new BuffScriptMetaData
        {
            BuffType = BuffType.COMBAT_ENCHANCER,
            BuffAddType = BuffAddType.REPLACE_EXISTING
        };


        public StatsModifier StatsModifier { get; private set; } = new StatsModifier();

        public void OnActivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {
            Unit = unit;
            Passive = buff;
            Leblanc = ownerSpell.CastInfo.Owner as Champion;
            ApiEventManager.OnDeath.AddListener(this, unit, OnDeath, true);
            AddParticleTarget(Leblanc, unit, "LeBlanc_Base_P_poof", Leblanc);
            ApiEventManager.OnLaunchAttack.AddListener(this, Leblanc, OnLaunchAttack, false);
            AddParticleTarget(Leblanc, unit, "LeBlanc_Base_P_image", Leblanc, 25000f, teamOnly: Leblanc.Team);
        }
        public void OnLaunchAttack(Spell spell)
        {

            if (Passive != null && Passive.StackCount != 0 && !Passive.Elapsed())
            {
                (Unit as Pet).SetTargetUnit(spell.CastInfo.Targets[0].Unit, true);
            }
        }
        public void OnDeath(DeathData data)
        {
            if (Passive != null && !Passive.Elapsed())
            {
                Passive.DeactivateBuff();
            }
        }
        public void OnDeactivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {
            ApiEventManager.OnLaunchAttack.RemoveListener(this);
            unit.Die(CreateDeathData(false, 0, unit, unit, DamageType.DAMAGE_TYPE_TRUE, DamageSource.DAMAGE_SOURCE_INTERNALRAW, 0.0f));
            AddParticle(Leblanc, unit, "LeBlanc_Base_P_imageDeath", unit.Position);
        }
    }
}