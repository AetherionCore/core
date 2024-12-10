using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using LeagueSandbox.GameServer.GameObjects.SpellNS;
using LeagueSandbox.GameServer.API;
using GameServerLib.GameObjects.AttackableUnits;
using Buffs;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.GameObjects;
using System;

namespace CharScripts
{
    public class CharScriptAnnie : ICharScript
    {
        Spell Spell;
        ObjAIBase unit;
        public void OnActivate(ObjAIBase owner, Spell spell)
        {
            unit = owner;
            Spell = spell;

            AddBuff("Pyromania Marker", 25000f, 1, spell, unit, unit, false);
        }


        public void OnDeactivate(ObjAIBase owner, Spell spell)
        {
            ApiEventManager.OnHitUnit.RemoveListener(this);
        }

        public void OnUpdate(float diff)
        {
            //if (unit != null)
            //{
            //    //AddBuff("Pyromania Marker", 250000f, 1, null, unit, unit);

            //    var buffCount = unit.GetBuffWithName("Pyromania")?.StackCount ?? 0;
            //    LogInfo($"Pyromania Counter {buffCount}");
            //    if (buffCount >= 4)
            //    {
            //        RemoveParticleByName(unit.NetId, "StunReady.troy");
            //        //AddParticleTarget(unit, unit, "StunReady.troy", unit, 25000f, bone: "chest");
            //        RemoveBuff(unit, "Pyromania");
            //    }
            //}
        }
    }
}