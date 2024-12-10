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
using LeaguePackets.Game;

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
        ObjAIBase Owner;
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
            Owner = owner;
        }
        public void OnSpellCast(Spell spell)
        {
            var Owner = spell.CastInfo.Owner;
            if (Owner.HasBuff("NasusQStacks"))
            {
                int StackDamage = Owner.GetBuffWithName("NasusQStacks").StackCount;
                float ownerdamage = spell.CastInfo.Owner.Stats.AttackDamage.Total;
                float damage = 15 + 25 * Owner.GetSpell("NasusQ").CastInfo.SpellLevel + StackDamage + ownerdamage;
                Target.TakeDamage(Owner, damage, DamageType.DAMAGE_TYPE_PHYSICAL, DamageSource.DAMAGE_SOURCE_ATTACK, false);
                AddParticleTarget(Owner, Target, "Nasus_Base_Q_Tar.troy", Target);
                if (Target.IsDead)
                {
                    for (int i = 0; i < 3; ++i)
                        AddBuff("NasusQStacks", 2500000f, 1, spell, Owner, Owner);
                    //SetSpellToolTipVar<int>(Owner, 1, StackDamage, SpellbookType.SPELLBOOK_CHAMPION, 0, SpellSlotType.SpellSlots);
                    StackDamage = Owner.GetBuffWithName("NasusQStacks").StackCount;
                    LogInfo($"StackDamage: {StackDamage}!");
                    SetBuffToolTipVar(Owner.GetBuffWithName("NasusQStacks"), 0, StackDamage);
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
                    for (int i = 0; i < 3; ++i)
                        AddBuff("NasusQStacks", 2500000f, 1, spell, Owner, Owner);
                    int StackDamage = Owner.GetBuffWithName("NasusQStacks").StackCount;
                    //SetSpellToolTipVar<int>(Owner, 0, StackDamage, SpellbookType.SPELLBOOK_CHAMPION, 0, SpellSlotType.SpellSlots);
                    LogInfo($"StackDamage2: {StackDamage}!");
                    SetBuffToolTipVar(Owner.GetBuffWithName("NasusQStacks"), 0, StackDamage);
                }
            }
        }
        public void OnSpellPostCast(Spell spell)
        {
            var Owner = spell.CastInfo.Owner;
            int StackDamage = Owner.GetBuffWithName("NasusQStacks")?.StackCount ?? 0;
            //SetSpellToolTipVar(Owner, 1, StackDamage, SpellbookType.SPELLBOOK_CHAMPION, 0, SpellSlotType.SpellSlots);
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