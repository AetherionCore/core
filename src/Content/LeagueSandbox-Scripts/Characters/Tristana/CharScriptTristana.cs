using GameServerCore.Enums;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using LeagueSandbox.GameServer.Scripting.CSharp;
using System.Numerics;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.GameObjects.SpellNS;
using LeagueSandbox.GameServer.GameObjects.SpellNS.Missile;
using LeagueSandbox.GameServer.GameObjects.StatsNS;
using GameServerLib.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.API;

namespace CharScripts
{
    public class CharScriptTristana : ICharScript
    {
        Spell Passive;
        ObjAIBase Tristana;
        AttackableUnit Target;
        public StatsModifier StatsModifier { get; private set; } = new StatsModifier();
        public void OnActivate(ObjAIBase owner, Spell spell = null)
        {
            Passive = spell;
            Tristana = owner as Champion;
            ApiEventManager.OnKill.AddListener(this, Tristana, OnKill, false);
            ApiEventManager.OnLevelUp.AddListener(this, Tristana, OnLevelUp, false);
        }
        public void OnKill(DeathData deathData)
        {
            Tristana.Spells[1].SetCooldown(0f, true);
        }
        public void OnLevelUp(AttackableUnit target)
        {
            StatsModifier.Range.FlatBonus = 7.5f;
            Tristana.AddStatModifier(StatsModifier);
        }
    }
}