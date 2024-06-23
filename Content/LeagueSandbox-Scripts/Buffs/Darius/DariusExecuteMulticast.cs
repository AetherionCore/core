using GameServerCore.Enums;
using LeagueSandbox.GameServer.GameObjects.StatsNS;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.Scripting.CSharp;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.GameObjects;
using LeagueSandbox.GameServer.GameObjects.SpellNS;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using System.Collections.Generic;
using LeagueSandbox.GameServer.API;
using GameServerLib.GameObjects.AttackableUnits;

namespace Buffs
{
    internal class DariusExecuteMulticast : IBuffGameScript
    {
        Particle Self;
        float TrueCooldown;
        public BuffScriptMetaData BuffMetaData { get; set; } = new BuffScriptMetaData
        {
            BuffType = BuffType.COMBAT_ENCHANCER,//AURA WUYINGYINGFUGAI
            BuffAddType = BuffAddType.REPLACE_EXISTING
        };
        public StatsModifier StatsModifier { get; private set; } = new StatsModifier();
        public void OnActivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {
            Self = AddParticleTarget(unit, unit, "darius_R_blood_spatter_self.troy", unit, 10f);
            (unit as ObjAIBase).Spells[3].SetCooldown(0f, false);
        }
        public void OnDeactivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {
            RemoveParticle(Self);
            TrueCooldown = (140 - (20 * (unit as ObjAIBase).Spells[3].CastInfo.SpellLevel)) * (1 + (unit as ObjAIBase).Stats.CooldownReduction.Total);
            (unit as ObjAIBase).Spells[3].SetCooldown(TrueCooldown, false);
        }
    }
}