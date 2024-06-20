using GameServerCore.Enums;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using LeagueSandbox.GameServer.GameObjects;
using            GameServerLib.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using LeagueSandbox.GameServer.GameObjects.SpellNS;
using LeagueSandbox.GameServer.GameObjects.StatsNS;
using GameServerCore.Scripting.CSharp;
using System;
using LeagueSandbox.GameServer.API;
using LeagueSandbox.GameServer.Scripting.CSharp;

namespace Buffs
{
    internal class MasterYiPassive : IBuffGameScript
    {
        public BuffScriptMetaData BuffMetaData { get; set; } = new BuffScriptMetaData
        {
            BuffType = BuffType.DAMAGE,
            BuffAddType = BuffAddType.STACKS_AND_RENEWS,
            MaxStacks = 4
        };

        public StatsModifier StatsModifier { get; private set; } = new StatsModifier();

        Particle p;
        Particle p1;
        Particle p2;
        Spell spell;
        AttackableUnit Unit;

        public void OnActivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {
            Unit = unit;
            spell = ownerSpell;
            switch (buff.StackCount)
            {
                case 1:

                    break;
                case 2:
                    break;
                case 3:
                    AddBuff("MasterYiDoubleStrike", 3.1f, 1, spell, spell.CastInfo.Owner, spell.CastInfo.Owner);
                    break;
                case 4:
                    //spell.CastInfo.Owner.SetAutoAttackSpell("MasterYiDoubleStrike", false);               		
                    buff.DeactivateBuff();
                    break;
            }
        }


        public void OnDeactivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {
            if (buff.TimeElapsed >= buff.Duration)
            {
                RemoveBuff(unit, "MasterYiPassive");
            }
            RemoveBuff(unit, "AatroxWONHLifeBuff");
            RemoveParticle(p);
            RemoveParticle(p1);
            RemoveParticle(p2);
        }

        public void OnUpdate(float diff)
        {
        }
    }
}