using GameServerCore.Enums;
using LeagueSandbox.GameServer.GameObjects.StatsNS;
using LeagueSandbox.GameServer.Scripting.CSharp;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.GameObjects;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.GameObjects.SpellNS;
using LeagueSandbox.GameServer.API;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using LeagueSandbox.GameServer.GameObjects.SpellNS.Missile;
using LeagueSandbox.GameServer.GameObjects.SpellNS.Sector;
using System.Numerics;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;

namespace Buffs
{
    internal class RivenPassiveAABoost : IBuffGameScript
    {
        ObjAIBase Riven;
        Buff PassiveAABoost;
        public BuffScriptMetaData BuffMetaData { get; set; } = new BuffScriptMetaData
        {
            BuffType = BuffType.COMBAT_ENCHANCER,
            BuffAddType = BuffAddType.STACKS_AND_CONTINUE,
            MaxStacks = 3
        };

        public StatsModifier StatsModifier { get; private set; } = new StatsModifier();

        public void OnActivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {
            PassiveAABoost = buff;
            Riven = ownerSpell.CastInfo.Owner as Champion;
            ApiEventManager.OnLaunchAttack.AddListener(this, Riven, OnLaunchAttack, false);
        }
        public void OnLaunchAttack(Spell spell)
        {
            if (PassiveAABoost != null && PassiveAABoost.StackCount != 0 && !PassiveAABoost.Elapsed())
            {
                if (Riven.IsNextAutoCrit)
                {
                    Riven.TargetUnit.TakeDamage(Riven, Riven.Stats.AttackDamage.Total * 0.3f * 2, DamageType.DAMAGE_TYPE_PHYSICAL, DamageSource.DAMAGE_SOURCE_SPELL, false);
                }
                else
                {
                    Riven.TargetUnit.TakeDamage(Riven, Riven.Stats.AttackDamage.Total * 0.3f, DamageType.DAMAGE_TYPE_PHYSICAL, DamageSource.DAMAGE_SOURCE_SPELL, false);
                }
                PassiveAABoost.DeactivateBuff();
            }
        }
    }
}