using System.Numerics;
using GameServerCore.Enums;
using GameServerCore.Scripting.CSharp;
using LeaguePackets.Game.Events;
using LeagueSandbox.GameServer.API;
using LeagueSandbox.GameServer.GameObjects;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using LeagueSandbox.GameServer.GameObjects.SpellNS;
using LeagueSandbox.GameServer.GameObjects.StatsNS;
using LeagueSandbox.GameServer.Scripting.CSharp;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;

namespace Buffs
{
    public class EkkoPassive : IBuffGameScript
    {
        public BuffScriptMetaData BuffMetaData { get; set; } = new BuffScriptMetaData
        {
            BuffType = BuffType.DAMAGE,
            BuffAddType = BuffAddType.STACKS_AND_RENEWS,
            MaxStacks = 3
        };

        public StatsModifier StatsModifier { get; private set; }

        AttackableUnit Unit;
        ObjAIBase owner;
        Buff buff;
        Particle p;
        Particle p1;
        Particle p2;
        Spell Spell;

        public void OnActivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {
            Spell = ownerSpell;
            owner = ownerSpell.CastInfo.Owner as Champion;
            Unit = unit;
            switch (buff.StackCount)
            {
                case 1:
                    if (unit is Champion)
                    {
                        p = AddParticleTarget(ownerSpell.CastInfo.Owner, unit, "Ekko_Base_P_Stack1.troy", unit, buff.Duration);
                    }
                    else
                    {
                        p = AddParticleTarget(ownerSpell.CastInfo.Owner, unit, "Ekko_Base_P_Stack1_Minion.troy", unit, buff.Duration);
                    }
                    break;
                case 2:
                    if (unit is Champion)
                    {
                        p1 = AddParticleTarget(ownerSpell.CastInfo.Owner, unit, "Ekko_Base_P_Stack2.troy", unit, buff.Duration);
                        p2 = AddParticleTarget(ownerSpell.CastInfo.Owner, unit, "Ekko_Base_P_Stack3_Warning.troy", unit, buff.Duration);
                    }
                    else
                    {
                        p1 = AddParticleTarget(ownerSpell.CastInfo.Owner, unit, "Ekko_Base_P_Stack2_Minion.troy", unit, buff.Duration);
                        p2 = AddParticleTarget(ownerSpell.CastInfo.Owner, unit, "Ekko_Base_P_Stack3_Warning.troy", unit, buff.Duration);
                    }
                    AddBuff("EkkoPassiveSpellShieldCheck", 3f, 1, Spell, unit, owner);
                    break;
                case 3:
                    RemoveBuff(unit, "EkkoPassive");
                    AddBuff("EkkoPassiveSlow", 3f, 1, Spell, unit, owner);
                    if (unit is Champion)
                    {
                        AddBuff("EkkoPassiveSpeed", 3f, 1, Spell, owner, owner);
                    }
                    return;
            }
        }
        public void OnDeactivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {
            if (buff.TimeElapsed >= buff.Duration)
            {
                RemoveBuff(unit, "EkkoPassive");
            }
            RemoveParticle(p);
            RemoveParticle(p1);
            RemoveParticle(p2);
        }

        public void OnUpdate(float diff)
        {
        }
    }
}