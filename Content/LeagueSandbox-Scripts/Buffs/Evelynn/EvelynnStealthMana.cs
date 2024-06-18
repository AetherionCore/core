using GameServerCore.Enums;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.GameObjects;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.GameObjects.SpellNS;
using LeagueSandbox.GameServer.GameObjects.StatsNS;
using LeagueSandbox.GameServer.Scripting.CSharp;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using GameServerLib.GameObjects.AttackableUnits;

namespace Buffs
{
    internal class EvelynnStealthMana : IBuffGameScript
    {
        //TODO: Add mana regen
        public BuffScriptMetaData BuffMetaData { get; set; } = new BuffScriptMetaData
        {
            BuffType = BuffType.INTERNAL
        };

        public StatsModifier StatsModifier { get; private set; }

        Particle pbuff;
        Particle p0;
        public void OnActivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {
            pbuff = AddParticleTarget(ownerSpell.CastInfo.Owner, unit, "evelynnmana", unit, buff.Duration, bone: "BUFFBONE_CSTM_SHIELD_TOP");
            //Mana is busted for some reason?
            //StatsModifier.ManaRegeneration.PercentBonus += unit.Stats.ManaPoints.Total * 0.01f;
            //unit.AddStatModifier(StatsModifier);
        }

        public void OnDeactivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {            
            RemoveParticle(p0);
            //unit.RemoveStatModifier(StatsModifier);
        }

        public void OnDeath(DeathData deathData)
        {

        }
        public void OnUpdate(float diff)
        {

        }
    }
}
