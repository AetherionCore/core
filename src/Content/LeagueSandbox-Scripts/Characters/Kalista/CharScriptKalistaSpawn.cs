using GameServerCore;
using GameServerCore.Enums;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.API;
using System;
using System.Numerics;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using LeagueSandbox.GameServer.GameObjects.SpellNS;

namespace CharScripts
{
    public class CharScriptKalistaSpawn : ICharScript
    {
        Spell Spell;
        AttackableUnit Target;
        public void OnActivate(ObjAIBase owner, Spell spell = null)
        {
            AddParticleTarget(owner, owner, "Kalista_Base_W_Alerted.troy", owner, int.MaxValue);
            AddParticleTarget(owner, owner, "Kalista_Base_W_Avatar.troy", owner, int.MaxValue);
            AddParticleTarget(owner, owner, "Kalista_Base_W_Glow.troy", owner, int.MaxValue);
            AddParticleTarget(owner, owner, "Kalista_Base_W_Glow2.troy", owner, int.MaxValue);
            AddParticleTarget(owner, owner, "Kalista_Base_W_Glow.troy", owner, int.MaxValue);
            AddParticleTarget(owner, owner, "Kalista_Base_W_ViewCone.troy", owner, int.MaxValue);
        }
    }
}