using System.Numerics;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using LeagueSandbox.GameServer.Scripting.CSharp;
using GameServerCore.Scripting.CSharp;
using GameServerCore.Enums;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.GameObjects.SpellNS;
using LeagueSandbox.GameServer.GameObjects.SpellNS.Missile;
using LeagueSandbox.GameServer.GameObjects.SpellNS.Sector;
using LeagueSandbox.GameServer.API;
using System.Collections.Generic;
using LeagueSandbox.GameServer.GameObjects;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.Buildings;

namespace Spells
{
    public class MonkeyKingBasicAttack : ISpellScript
    {
        private AttackableUnit Target = null;

        public SpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
        {
            TriggersSpellCasts = true
        };

        public void OnSpellPreCast(ObjAIBase owner, Spell spell, AttackableUnit target, Vector2 start, Vector2 end)
        {
            Target = target;
            if (owner.HasBuff("MonkeyKingDoubleAttack"))
            {
                OverrideAnimation(owner, "Crit", "Attack1");
            }
            else
            {
                OverrideAnimation(owner, "Attack1", "Crit");
            }
            ApiEventManager.OnLaunchAttack.AddListener(this, owner, OnLaunchAttack, true);
        }

        public void OnLaunchAttack(Spell spell)
        {
            var owner = spell.CastInfo.Owner;
            var spellLevel = owner.GetSpell("MonkeyKingDoubleAttack").CastInfo.SpellLevel;
            var ADratio = owner.Stats.AttackDamage.Total * 0.3f;
            var damage = (30 * spellLevel) + ADratio;
            if (owner.HasBuff("MonkeyKingDoubleAttack"))
            {
                Target.TakeDamage(owner, damage, DamageType.DAMAGE_TYPE_PHYSICAL, DamageSource.DAMAGE_SOURCE_ATTACK, false);
            }
            else
            {
            }
        }

        public void OnSpellCast(Spell spell)
        {
            var owner = spell.CastInfo.Owner;
        }

        public void OnSpellPostCast(Spell spell)
        {
            var owner = spell.CastInfo.Owner;
        }
    }
}

    public class MonkeyKingBasicAttack2 : ISpellScript
    {
        private AttackableUnit Target = null;

        public SpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
        {
            TriggersSpellCasts = true
        };

        public void OnSpellPreCast(ObjAIBase owner, Spell spell, AttackableUnit target, Vector2 start, Vector2 end)
        {
            Target = target;
            if (owner.HasBuff("MonkeyKingDoubleAttack"))
            {
                OverrideAnimation(owner, "Crit", "Attack2");
            }
            else
            {
                OverrideAnimation(owner, "Attack2", "Crit");
            }
            ApiEventManager.OnLaunchAttack.AddListener(this, owner, OnLaunchAttack, true);
        }

        public void OnLaunchAttack(Spell spell)
        {
            var owner = spell.CastInfo.Owner;
            var spellLevel = owner.GetSpell("MonkeyKingDoubleAttack").CastInfo.SpellLevel;
            var ADratio = owner.Stats.AttackDamage.Total * 0.3f;
            var damage = (30 * spellLevel) + ADratio;
            if (owner.HasBuff("MonkeyKingDoubleAttack"))
            {
                Target.TakeDamage(owner, damage, DamageType.DAMAGE_TYPE_PHYSICAL, DamageSource.DAMAGE_SOURCE_ATTACK, false);
            }
            else
            {
            }
        }
    }
public class MonkeyKingCritAttack : ISpellScript
{
    private AttackableUnit Target = null;

    public SpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
    {
        TriggersSpellCasts = true
    };

    public void OnSpellPreCast(ObjAIBase owner, Spell spell, AttackableUnit target, Vector2 start, Vector2 end)
    {
        Target = target;
        ApiEventManager.OnLaunchAttack.AddListener(this, owner, OnLaunchAttack, true);
    }

    public void OnLaunchAttack(Spell spell)
    {
        var owner = spell.CastInfo.Owner;
        var spellLevel = owner.GetSpell("MonkeyKingDoubleAttack").CastInfo.SpellLevel;
        var ADratio = owner.Stats.AttackDamage.Total * 0.3f;
        var damage = (30 * spellLevel) + ADratio;
        var damager = damage * 2;
        if (owner.HasBuff("MonkeyKingDoubleAttack"))
        {
            Target.TakeDamage(owner, damager, DamageType.DAMAGE_TYPE_PHYSICAL, DamageSource.DAMAGE_SOURCE_ATTACK, true);
        }
        else
        {
        }
    }
}