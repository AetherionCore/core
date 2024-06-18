using GameServerCore.Enums;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.GameObjects;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.GameObjects.SpellNS;
using LeagueSandbox.GameServer.GameObjects.StatsNS;
using LeagueSandbox.GameServer.Scripting.CSharp;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using GameServerLib.GameObjects.AttackableUnits;
using System.Collections.Generic;

namespace Buffs
{
    internal class ShadowWalk  : IBuffGameScript
    {
        public BuffScriptMetaData BuffMetaData { get; set; } = new BuffScriptMetaData
        {
            BuffType = BuffType.INTERNAL
            //BuffType =  BuffType.COMBAT_ENCHANCER,
            //BuffAddType = BuffAddType.STACKS_AND_CONTINUE
        };

        public StatsModifier StatsModifier { get; private set; }

        Particle p0;
        Buff _stealth;
        Buff _stealthRing;
        Dictionary<string, string> PowPowAnimPairs;
        Dictionary<string, string> FishbonesAnimPairs;

        public void OnActivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {
            _stealth = AddBuff("EvelynnStealth", 0f, 1, ownerSpell, unit, ownerSpell.CastInfo.Owner, true);
            _stealthRing = AddBuff("EvelynnStealthRing", 25000f, 1, ownerSpell, unit, ownerSpell.CastInfo.Owner, true);
            unit.SetAnimStates(new Dictionary<string, string> { { "evelynn_run", "evelynn_run_sneak" } });
        }

        public void OnDeactivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {
            _stealthRing.DeactivateBuff();
            unit.SetAnimStates(new Dictionary<string, string> {});
        }

        public void OnDeath(DeathData deathData)
        {

        }
        public void OnUpdate(float diff)
        {

        }
    }
}
