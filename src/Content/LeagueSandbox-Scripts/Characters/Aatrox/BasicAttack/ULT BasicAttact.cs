using GameServerCore.Enums;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using LeagueSandbox.GameServer.Scripting.CSharp;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.Buildings;
using LeagueSandbox.GameServer.GameObjects.SpellNS;
using LeagueSandbox.GameServer.API;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;
using System.Numerics;

namespace Spells
{
    public class AatroxBasicAttack : ISpellScript
    {
        public SpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
        {
            TriggersSpellCasts = true
            // TODO
        };

        public void OnActivate(ObjAIBase owner, Spell spell)
        {
        }

        public void OnDeactivate(ObjAIBase owner, Spell spell)
        {
        }

        public void OnSpellPreCast(ObjAIBase owner, Spell spell, AttackableUnit target, Vector2 start, Vector2 end)
        {
            ApiEventManager.OnLaunchAttack.AddListener(this, owner, OnLaunchAttack, true);
        }

        public void OnLaunchAttack(Spell spell)
        {
            spell.CastInfo.Owner.SetAutoAttackSpell("AatroxBasicAttack2", false);
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

    public class AatroxBasicAttack5 : ISpellScript
    {
        public SpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
        {
            TriggersSpellCasts = true

            // TODO
        };

        public void OnActivate(ObjAIBase owner, Spell spell)
        {
        }

        public void OnDeactivate(ObjAIBase owner, Spell spell)
        {
        }

        public void OnSpellPreCast(ObjAIBase owner, Spell spell, AttackableUnit target, Vector2 start, Vector2 end)
        {
            ApiEventManager.OnLaunchAttack.AddListener(this, owner, OnLaunchAttack, true);
        }

        public void OnLaunchAttack(Spell spell)
        {
            spell.CastInfo.Owner.SetAutoAttackSpell("AatroxBasicAttack", false);
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
    public class AatroxBasicAttack6 : ISpellScript
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

        public void OnDeactivate(ObjAIBase owner, Spell spell)
        {
        }

        public void OnSpellPreCast(ObjAIBase owner, Spell spell, AttackableUnit target, Vector2 start, Vector2 end)
        {
            Target = target;
            ApiEventManager.OnLaunchAttack.AddListener(this, owner, OnLaunchAttack, true);
            if (owner.HasBuff("AatroxR") && owner.HasBuff("AatroxWONHLifeBuff"))
            {
                OverrideAnimation(owner, "Spell2_ULT", "Attack6");
            }
            else
            {
                OverrideAnimation(owner, "Attack6", "Spell2_ULT");
            }
        }

        public void OnLaunchAttack(Spell spell)
        {
            var owner = spell.CastInfo.Owner as Champion;
            if (owner.HasBuff("AatroxWONHLifeBuff"))
            {
                float heal = 25 + (5 * (owner.GetSpell("AatroxWONHLifeBuff").CastInfo.SpellLevel - 1));
                float BBlood = owner.Stats.HealthPoints.Total * 0.5f;
                float Blood = owner.Stats.HealthPoints.Total * 0.025f;
                float ap = owner.Stats.AbilityPower.Total * 0.7f;
                float XBlood = owner.Stats.CurrentHealth;
                if (BBlood >= XBlood)
                {
                    owner.Stats.CurrentHealth += (heal + ap + Blood) * 2;
                }
                owner.Stats.CurrentHealth += heal + ap + Blood;
                AddParticleTarget(owner, owner, "Global_Heal.troy", owner);
                AddParticleTarget(owner, owner, "Aatrox_Base_W_Buff_Life_sound.troy", owner);
                AddParticleTarget(owner, owner, "Aatrox_Base_W_Life_Self.troy", owner);
                AddParticle(owner, Target, "Aatrox_Base_W_Active_Hit.troy", Target.Position, 6f);
                AddParticleTarget(owner, Target, "Aatrox_Base_W_Active_Hit_Life.troy", Target, 6f);
                AddParticleTarget(owner, Target, "Aatrox_Base_W_Life__Passive_Hit.troy", Target, 6f);
                AddParticleTarget(owner, Target, "Aatrox_Base_W_Life_Passive_Hit.troy", Target, 6f);
                AddParticleTarget(owner, Target, "Aatrox_Base_W_hit_impact_tar_bloodless.troy", Target, 6f);
            }
            if (owner.HasBuff("AatroxWONHPowerBuff"))
            {
                float MBlood = owner.Stats.CurrentHealth * 0.1f;
                owner.Stats.CurrentMana += MBlood;
                owner.TakeDamage(owner, MBlood, DamageType.DAMAGE_TYPE_TRUE, DamageSource.DAMAGE_SOURCE_PERIODIC, false);
                var ADratio = owner.Stats.AttackDamage.Total * 1f;
                var damage = 60 + (35 * (spell.CastInfo.SpellLevel - 1) + ADratio);
                Target.TakeDamage(owner, damage, DamageType.DAMAGE_TYPE_PHYSICAL, DamageSource.DAMAGE_SOURCE_PERIODIC, false);
                //AddParticleTarget(owner, owner, "Global_Heal.troy", owner);
                AddParticleTarget(owner, owner, "Aatrox_Base_W_Buff_Power_sound.troy", owner);
                AddParticleTarget(Target, owner, "Aatrox_Base_W_Power_Passive_Hit.troy", Target);
                AddParticle(owner, Target, "Aatrox_Base_W_Active_Hit.troy", Target.Position, 6f);
                AddParticleTarget(owner, Target, "Aatrox_Base_W_Active_Hit_Power.troy", Target, 6f);
                AddParticleTarget(owner, Target, "Aatrox_Base_W_hit_impact_tar_bloodless.troy", Target, 6f);
            }
            spell.CastInfo.Owner.SetAutoAttackSpell("AatroxBasicAttack", false);
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
}