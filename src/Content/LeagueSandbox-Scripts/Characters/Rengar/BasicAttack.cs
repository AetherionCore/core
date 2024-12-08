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
using GameServerLib.GameObjects.AttackableUnits;
using LeaguePackets.Game.Common;
using LeagueSandbox.GameServer.GameObjects.StatsNS;

namespace Spells
{
    public class RengarBasicAttack : ISpellScript
    {
        AttackableUnit Target;

        public SpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
        {
            TriggersSpellCasts = true
            // TODO
        };

        public void OnSpellPreCast(ObjAIBase owner, Spell spell, AttackableUnit target, Vector2 start, Vector2 end)
        {
            Target = target;
            ApiEventManager.OnLaunchAttack.AddListener(this, owner, OnLaunchAttack, true);
            if (owner.HasBuff("RengarQBuff"))
            {
                OverrideAnimation(owner, "Spell1", "Attack1");
            }
            else
            {
                OverrideAnimation(owner, "Attack1", "Spell1");
            }
        }

        public void OnLaunchAttack(Spell spell)
        {
            var owner = spell.CastInfo.Owner;
            float QLevel = (owner.GetSpell("RengarQ").CastInfo.SpellLevel - 1) * 0.05f;
            float damage = ((30 * owner.GetSpell("RengarQ").CastInfo.SpellLevel) + owner.Stats.AttackDamage.Total * QLevel);
            if (owner.HasBuff("RengarQBuff"))
            {
                Target.TakeDamage(owner, damage, DamageType.DAMAGE_TYPE_PHYSICAL, DamageSource.DAMAGE_SOURCE_ATTACK, false);
                if (!owner.HasBuff("RengarFerocityManager"))
                {
                    AddBuff("RengarManager", 8.0f, 1, spell, owner, owner);
                }
                AddParticleTarget(owner, Target, "Rengar_Base_Q_Tar.troy", owner);
            }
            else if (owner.HasBuff("RengarQEmp"))
            {
                Target.TakeDamage(owner, damage, DamageType.DAMAGE_TYPE_PHYSICAL, DamageSource.DAMAGE_SOURCE_ATTACK, false);
                AddParticleTarget(owner, Target, "Rengar_Base_Q_Tar.troy", owner);
            }
        }
    }

    public class RengarCritAttack : ISpellScript
    {
        AttackableUnit Target;

        public SpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
        {
            TriggersSpellCasts = true
            // TODO
        };


        public void OnSpellPreCast(ObjAIBase owner, Spell spell, AttackableUnit target, Vector2 start, Vector2 end)
        {
            Target = target;
            ApiEventManager.OnLaunchAttack.AddListener(this, owner, OnLaunchAttack, true);
            if (owner.HasBuff("RengarQBuff"))
            {
                OverrideAnimation(owner, "Spell1", "Crit");
            }
            else
            {
                OverrideAnimation(owner, "Crit", "Spell1");
            }
        }

        public void OnLaunchAttack(Spell spell)
        {
            var owner = spell.CastInfo.Owner;
            float QLevel = (owner.GetSpell("RengarQ").CastInfo.SpellLevel - 1) * 0.05f;
            float damage = ((30 * owner.GetSpell("RengarQ").CastInfo.SpellLevel) + owner.Stats.AttackDamage.Total * QLevel) * 2;
            if (owner.HasBuff("RengarQBuff"))
            {
                Target.TakeDamage(owner, damage, DamageType.DAMAGE_TYPE_PHYSICAL, DamageSource.DAMAGE_SOURCE_ATTACK, true);
                AddParticleTarget(owner, Target, "Rengar_Base_Q_Tar.troy", owner);
                if (!owner.HasBuff("RengarFerocityManager"))
                {
                    AddBuff("RengarManager", 8.0f, 1, spell, owner, owner);
                }
            }
            else if (owner.HasBuff("RengarQEmp"))
            {
                Target.TakeDamage(owner, damage, DamageType.DAMAGE_TYPE_PHYSICAL, DamageSource.DAMAGE_SOURCE_ATTACK, true);
                AddParticleTarget(owner, Target, "Rengar_Base_Q_Tar.troy", owner);
            }
        }
    }
}