using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.Scripting.CSharp;
using System;
using System.Numerics;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using LeagueSandbox.GameServer.GameObjects.SpellNS;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using LeagueSandbox.GameServer.API;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.Buildings;

namespace CharScripts
{
    public class CharScriptGangplank : ICharScript
    {
        ObjAIBase Gangplank;
        AttackableUnit Target;
        public void OnActivate(ObjAIBase owner, Spell spell = null)
        {
            Gangplank = owner as Champion;
            ApiEventManager.OnLaunchAttack.AddListener(this, Gangplank, OnLaunchAttack, false);
        }
        public void OnLaunchAttack(Spell spell)
        {
            Target = spell.CastInfo.Targets[0].Unit;
            if (!(Target is ObjBuilding || Target is BaseTurret))
            {
                AddBuff("Scurvy", 3f, 1, spell, Target, Gangplank);
            }
        }
    }
}
