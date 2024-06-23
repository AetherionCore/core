using GameServerCore.Enums;
using LeagueSandbox.GameServer.GameObjects.StatsNS;
using LeagueSandbox.GameServer.API;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.Scripting.CSharp;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.GameObjects;
using LeagueSandbox.GameServer.GameObjects.SpellNS;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using GameServerLib.GameObjects.AttackableUnits;

namespace Buffs
{
    internal class FioraDuelistHotParticle : IBuffGameScript
    {
        Buff Duelist;
        Particle Heal;
        ObjAIBase Fiora;
        public BuffScriptMetaData BuffMetaData { get; set; } = new BuffScriptMetaData
        {
            BuffType = BuffType.COMBAT_ENCHANCER,
            BuffAddType = BuffAddType.STACKS_AND_RENEWS,
            MaxStacks = 4
        };
        public StatsModifier StatsModifier { get; private set; } = new StatsModifier();
        public void OnActivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {
            Duelist = buff;
            RemoveParticle(Heal);
            Fiora = ownerSpell.CastInfo.Owner as Champion;
            ApiEventManager.OnHitUnit.AddListener(this, Fiora, OnHitUnit, false);
            Heal = AddParticleTarget(Fiora, Fiora, "fiora_heal_buf", Fiora, 25000f, 1);
            switch (Duelist.StackCount) { case 2: AddParticleTarget(Fiora, Fiora, "fiora_heal2_buf", Fiora); return; }
        }
        public void OnHitUnit(DamageData damageData)
        {
            Fiora.Stats.CurrentHealth += 7 + Fiora.Stats.Level;
        }
        public void OnDeactivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {
            RemoveParticle(Heal);
        }
    }
}