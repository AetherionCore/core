using LeagueSandbox.GameServer.Scripting.CSharp;
using System.Numerics;
using LeagueSandbox.GameServer.API;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using LeagueSandbox.GameServer.GameObjects.SpellNS;
using GameServerCore.Enums;
using LeagueSandbox.GameServer.GameObjects.SpellNS.Missile;
using LeagueSandbox.GameServer.GameObjects.SpellNS.Sector;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;

namespace Spells
{
    public class NasusQ : ISpellScript
    {
        public SpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
        {
            TriggersSpellCasts = true,
            NotSingleTargetSpell = true
            // TODO
        };

        public void OnActivate(ObjAIBase owner, Spell spell)
        {
            ApiEventManager.OnLevelUpSpell.AddListener(this, spell, OnLevelUp, true);
        }
        public void OnLevelUp(Spell spell)
        {
            var owner = spell.CastInfo.Owner;
        }
        public void OnDeactivate(ObjAIBase owner, Spell spell)
        {
        }

        public void OnSpellPreCast(ObjAIBase owner, Spell spell, AttackableUnit target, Vector2 start, Vector2 end)
        {
            AddBuff("NasusQ", 6.0f, 1, spell, owner, owner);
        }

        public void OnSpellCast(Spell spell)
        {
        }

        public void OnSpellPostCast(Spell spell)
        {
        }

        public void OnSpellChannel(Spell spell)
        {
        }

        public void OnSpellChannelCancel(Spell spell, ChannelingStopSource reason)
        {
        }

        public void OnSpellPostChannel(Spell spell)
        {
        }

        public void OnUpdate(float diff)
        {
        }
    }
    public class NasusQAttack : ISpellScript
    {
        AttackableUnit Target;
        public SpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
        {
            TriggersSpellCasts = true,
            NotSingleTargetSpell = true,
            IsDamagingSpell = true,
            // TODO
        };
        public void OnActivate(ObjAIBase owner, Spell spell)
        {
        }
        public void TargetExecute(Spell spell, AttackableUnit target, SpellMissile missile, SpellSector sector)
        {
        }
        public void OnDeactivate(ObjAIBase owner, Spell spell)
        {
        }
        public void OnSpellPreCast(ObjAIBase owner, Spell spell, AttackableUnit target, Vector2 start, Vector2 end)
        {
            Target = target;
        }
        public void OnSpellCast(Spell spell)
        {
            var Owner = spell.CastInfo.Owner;
            if (Owner.HasBuff("NasusQStacks"))
            {
                float StackDamage = Owner.GetBuffWithName("NasusQStacks").StackCount;
                float ownerdamage = spell.CastInfo.Owner.Stats.AttackDamage.Total;
                float damage = 15 + 25 * Owner.GetSpell("NasusQ").CastInfo.SpellLevel + StackDamage + ownerdamage;
                Target.TakeDamage(Owner, damage, DamageType.DAMAGE_TYPE_PHYSICAL, DamageSource.DAMAGE_SOURCE_ATTACK, false);
                AddParticleTarget(Owner, Target, "Nasus_Base_Q_Tar.troy", Target);
                if (Target.IsDead)
                {
                    AddBuff("NasusQStacks", 2500000f, 3, spell, Owner, Owner);
                }

            }
            else
            {
                float ownerdamage = spell.CastInfo.Owner.Stats.AttackDamage.Total;
                float damage = 15 + 25 * Owner.GetSpell("NasusQ").CastInfo.SpellLevel + ownerdamage;
                Target.TakeDamage(Owner, damage, DamageType.DAMAGE_TYPE_PHYSICAL, DamageSource.DAMAGE_SOURCE_ATTACK, false);
                AddParticleTarget(Owner, Target, "Nasus_Base_Q_Tar.troy", Target);
                if (Target.IsDead)
                {
                    AddBuff("NasusQStacks", 2500000f, 3, spell, Owner, Owner);
                }
            }
        }
        public void OnSpellPostCast(Spell spell)
        {
        }
        public void OnSpellChannel(Spell spell)
        {
        }
        public void OnSpellChannelCancel(Spell spell, ChannelingStopSource reason)
        {
        }
        public void OnSpellPostChannel(Spell spell)
        {
        }
        public void OnUpdate(float diff)
        {
        }
    }
}