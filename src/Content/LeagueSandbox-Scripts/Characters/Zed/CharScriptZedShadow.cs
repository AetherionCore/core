using GameServerCore.Enums;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using LeagueSandbox.GameServer.Scripting.CSharp;
using System.Numerics;
using LeagueSandbox.GameServer.API;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using LeagueSandbox.GameServer.GameObjects.SpellNS;
using LeagueSandbox.GameServer.GameObjects.SpellNS.Missile;
using LeagueSandbox.GameServer.GameObjects.SpellNS.Sector;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.Buildings;
using GameServerLib.GameObjects.AttackableUnits;

namespace CharScripts
{
    public class CharScriptZedShadow : ICharScript
    {
        ObjAIBase Shadow;
        AttackableUnit Target;
        public void OnActivate(ObjAIBase owner, Spell spell = null)
        {
            Shadow = owner as Minion;
            ApiEventManager.OnDeath.AddListener(this, Shadow, OnDeath, true);
        }
        public void OnDeath(DeathData data)
        {
            SetStatus(Shadow, StatusFlags.NoRender, true);
            AddParticleTarget(Shadow, Shadow, "Become_Transparent.troy", Shadow, 10, 10);
        }
    }
}