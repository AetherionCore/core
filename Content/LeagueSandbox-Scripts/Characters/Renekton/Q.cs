using GameServerCore.Enums;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using LeagueSandbox.GameServer.GameObjects;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.GameObjects.SpellNS;
using LeagueSandbox.GameServer.GameObjects.StatsNS;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.Scripting.CSharp;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using System.Numerics;
using LeagueSandbox.GameServer.GameObjects.SpellNS.Missile;
using LeagueSandbox.GameServer.GameObjects.SpellNS.Sector;
using LeagueSandbox.GameServer.API;

namespace Spells
{
    public class RenektonCleave : ISpellScript
    {
        public SpellScriptMetadata ScriptMetadata => new SpellScriptMetadata()
        {
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
        }

        public void OnSpellPostCast(Spell spell)
        {
			var owner = spell.CastInfo.Owner;
			var ownerSkinID = owner.SkinID;
			var Blood = owner.Stats.ManaPoints.Total * 0.5f;
			var Health = owner.Stats.CurrentMana;        
            if (Health >= Blood)
			{
				AddParticleTarget(owner, owner, "Renekton_Base_Q_cas_rage.troy", owner, 1f,1,"C_BuffBone_Glb_Center_Loc");
				owner.Stats.CurrentMana -= 50f;
			}			
			else
			{
				AddParticleTarget(owner, owner, "Renekton_Base_Q_cas.troy", owner, 1f,1,"C_BuffBone_Glb_Center_Loc");
			}
            PlayAnimation(owner, "Spell1", 0.8f);        
            spell.CreateSpellSector(new SectorParameters
            {
                Length = 260f,
                SingleTick = true,
                Type = SectorType.Area
            });
        }
        public void TargetExecute(Spell spell, AttackableUnit target, SpellMissile missile, SpellSector sector)
        {
            var owner = spell.CastInfo.Owner;
            if (owner != target)
            {
                var AD = spell.CastInfo.Owner.Stats.AttackDamage.Total * 0.6f;
                var damage = 30 + spell.CastInfo.SpellLevel * 30 + AD;
                target.TakeDamage(owner, damage, DamageType.DAMAGE_TYPE_PHYSICAL, DamageSource.DAMAGE_SOURCE_SPELLAOE, false);
                AddParticleTarget(owner, null, "Renekton_Base_Q_tar.troy", target); 
                owner.Stats.CurrentMana += 10f;				
            }
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