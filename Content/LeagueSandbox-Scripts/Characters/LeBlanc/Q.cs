using GameServerCore.Enums;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.GameObjects;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.GameObjects.SpellNS;
using LeagueSandbox.GameServer.GameObjects.StatsNS;
using LeagueSandbox.GameServer.Scripting.CSharp;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using LeagueSandbox.GameServer.API;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using LeagueSandbox.GameServer.GameObjects.SpellNS.Missile;
using LeagueSandbox.GameServer.GameObjects.SpellNS.Sector;
using System.Numerics;

namespace Spells
{
    public class LeblancChaosOrb : ISpellScript
    {
        public SpellScriptMetadata ScriptMetadata => new SpellScriptMetadata()
        {
            MissileParameters = new MissileParameters
            {
                Type = MissileType.Target
            },
            TriggersSpellCasts = true

            // TODO
        };

        public void OnActivate(ObjAIBase owner, Spell spell)
        {
            ApiEventManager.OnSpellHit.AddListener(this, spell, TargetExecute, false);
        }

        public void OnDeactivate(ObjAIBase owner, Spell spell)
        {
        }

        public void OnSpellPreCast(ObjAIBase owner, Spell spell, AttackableUnit target, Vector2 start, Vector2 end)
        {
        }

        public void OnSpellCast(Spell spell)
        {
			var owner = spell.CastInfo.Owner;
			//if (!owner.HasBuff("LeblancSlideM") && owner.GetSpell("LeblancMimic").CastInfo.SpellLevel >= 1 )
             //{
			if (!owner.HasBuff("LeblancSlideM")&&owner.HasBuff("LeblancMimic"))
             {
             owner.SetSpell("LeblancChaosOrbM", 3, true);
             }			 
        }

        public void OnSpellPostCast(Spell spell)
        {
        }

        public void TargetExecute(Spell spell, AttackableUnit target, SpellMissile missile, SpellSector sector)
        {
            var owner = spell.CastInfo.Owner;
			var QLevel = owner.GetSpell("LeblancChaosOrb").CastInfo.SpellLevel;
            var RLevel = owner.GetSpell("LeblancSoulShackle").CastInfo.SpellLevel;
            var AP = owner.Stats.AbilityPower.Total * 0.4f;
			var MAXAP = spell.CastInfo.Owner.Stats.AbilityPower.Total * 0.65f;
			var damagemax=100 + 100f*(RLevel - 1)+ MAXAP;
            var damage = 55 + 25f*(QLevel - 1) + AP;
			var QMarkdamage = damage * 2f;
			var RQMarkdamage = damage + damagemax;
            if (target.HasBuff("LeblancChaosOrb"))
            {
				target.TakeDamage(owner, QMarkdamage, DamageType.DAMAGE_TYPE_MAGICAL, DamageSource.DAMAGE_SOURCE_ATTACK, true);
				target.RemoveBuffsWithName("LeblancChaosOrb");
				AddBuff("LeblancChaosOrb", 3.5f, 1, spell, target, owner);
            }
			else if (target.HasBuff("LeblancChaosOrbM"))
            {
				target.TakeDamage(owner, RQMarkdamage, DamageType.DAMAGE_TYPE_MAGICAL, DamageSource.DAMAGE_SOURCE_ATTACK, true);
			    target.RemoveBuffsWithName("LeblancChaosOrbM");
				AddBuff("LeblancChaosOrb", 3.5f, 1, spell, target, owner);
            }
			else
			{
            target.TakeDamage(owner, damage, DamageType.DAMAGE_TYPE_MAGICAL, DamageSource.DAMAGE_SOURCE_ATTACK, false);
			}
			AddParticleTarget(owner, target, "LeBlanc_Base_Q_tar", target);
            AddBuff("LeblancChaosOrb", 3.5f, 1, spell, target, owner);
            missile.SetToRemove();
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
