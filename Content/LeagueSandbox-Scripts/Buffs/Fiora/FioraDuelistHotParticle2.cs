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
    internal class FioraDuelistHotParticle2 : IBuffGameScript
    {
        Particle Heal;
        ObjAIBase Fiora;
        public BuffScriptMetaData BuffMetaData { get; set; } = new BuffScriptMetaData
        {
            BuffType = BuffType.COMBAT_ENCHANCER,
            BuffAddType = BuffAddType.REPLACE_EXISTING
        };
        public StatsModifier StatsModifier { get; private set; } = new StatsModifier();
        public void OnActivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {
            Fiora = ownerSpell.CastInfo.Owner as Champion;
            ApiEventManager.OnHitUnit.AddListener(this, Fiora, OnHitUnit, false);
            Heal = AddParticleTarget(Fiora, Fiora, "fiora_heal_buf", Fiora, 25000f, 1);
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