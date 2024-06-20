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
    internal class LeblancPassiveCooldown : IBuffGameScript
    {
        public BuffScriptMetaData BuffMetaData { get; set; } = new BuffScriptMetaData
        {
            BuffType = BuffType.COMBAT_ENCHANCER,
            BuffAddType = BuffAddType.REPLACE_EXISTING
        };

        public StatsModifier StatsModifier { get; private set; }

        Spell Spell;

        public void OnActivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {
            Spell = ownerSpell;
        }

        public void OnDeactivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {
            if (Spell.CastInfo.Owner is Champion owner)
            {
                AddBuff("LeblancPassive", 25000f, 1, ownerSpell, owner, owner, false);
            }
        }

        public void OnUpdate(float diff)
        {

        }
    }
}