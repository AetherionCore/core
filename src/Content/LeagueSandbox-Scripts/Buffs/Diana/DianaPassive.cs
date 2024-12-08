using GameServerCore.Enums;
using LeagueSandbox.GameServer.GameObjects.StatsNS;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.Scripting.CSharp;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.GameObjects;
using LeagueSandbox.GameServer.GameObjects.SpellNS;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using System.Collections.Generic;
using LeagueSandbox.GameServer.API;
using GameServerLib.GameObjects.AttackableUnits;
using LeaguePackets.Game.Events;

namespace Buffs
{
    internal class DianaPassive : IBuffGameScript
    {
        public BuffScriptMetaData BuffMetaData { get; set; } = new BuffScriptMetaData
        {
            BuffType = BuffType.DAMAGE,
            BuffAddType = BuffAddType.STACKS_AND_RENEWS,
            MaxStacks = 3
        };

        public StatsModifier StatsModifier { get; private set; }

        Particle p;
        Particle p1;
        Particle p2;
        Spell spell;
        AttackableUnit Unit;

        //IAttackableUnit target;
        public void OnActivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {
            Unit = unit;
            spell = ownerSpell;
            switch (buff.StackCount)
            {
                case 1:
                    break;
                case 2:
                    AddBuff("DianaPassiveDeathRecap", 3.1f, 1, spell, spell.CastInfo.Owner, spell.CastInfo.Owner);
                    break;
                case 3:
                    //spell.CastInfo.Owner.SetAutoAttackSpell("MasterYiDoubleStrike", false);             		
                    buff.DeactivateBuff();
                    break;
            }
        }

        public void OnDeactivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {
            if (buff.TimeElapsed >= buff.Duration)
            {
                RemoveBuff(unit, "DianaPassive");
            }
            RemoveBuff(unit, "AatroxWONHLifeBuff");
            RemoveParticle(p);
            RemoveParticle(p1);
            RemoveParticle(p2);
        }
    }
}