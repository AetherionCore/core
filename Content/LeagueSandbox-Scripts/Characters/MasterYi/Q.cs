using GameServerCore.Enums;
using LeagueSandbox.GameServer.GameObjects.StatsNS;
using LeagueSandbox.GameServer.Scripting.CSharp;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.GameObjects;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.GameObjects.SpellNS;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using LeagueSandbox.GameServer.API;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using System.Numerics;
using LeagueSandbox.GameServer.GameObjects.SpellNS.Missile;
using LeagueSandbox.GameServer.GameObjects.SpellNS.Sector;

namespace Spells
{
    public class AlphaStrike: ISpellScript
    {
        public static AttackableUnit _target = null;
		Buff HandlerBuff;
        Minion Shadow;
        public SpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
        {
            TriggersSpellCasts = true
        };

        public void OnActivate(ObjAIBase owner, Spell spell)
        {
			ApiEventManager.OnSpellHit.AddListener(this, spell, TargetExecute, false);
        }

        public void OnDeactivate(ObjAIBase owner, Spell spell)
        {
			SetStatus(owner, StatusFlags.NoRender, false);
        }

        public void OnSpellPreCast(ObjAIBase owner, Spell spell, AttackableUnit target, Vector2 start, Vector2 end)
        {
			_target = target;
			AddBuff("AlphaStrikeTeleport", 3f, 1, spell, owner, owner);
        }

        public void OnSpellCast(Spell spell)
        {
        }

        public void OnSpellPostCast(Spell spell)
        {
			var owner = spell.CastInfo.Owner;
			var randPoint1 = new Vector2(owner.Position.X + (10.0f), owner.Position.Y + 10.0f);	
			//ForceMovement(owner, null, randPoint1, 0.5f, 0, -280, 0);
			SpellCast(owner, 2, SpellSlotType.ExtraSlots, false, _target, Vector2.Zero);
			owner.StopMovement();
        }
		public void TargetExecute(Spell spell, AttackableUnit target, SpellMissile missile, SpellSector sector)
        {
            var owner = spell.CastInfo.Owner;

            //SpellCast(owner, 2, SpellSlotType.ExtraSlots, false, target, missile.Position);

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
	public class AlphaStrikeBounce : ISpellScript
    {
		Spell spell;
        public SpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
        {
            TriggersSpellCasts = true,
            IsDamagingSpell = true,       
            MissileParameters = new MissileParameters
            {
                Type = MissileType.Chained,
                MaximumHits = 4
            }
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

        public void TargetExecute(Spell spell, AttackableUnit target, SpellMissile missile, SpellSector sector)
        {
            var owner = spell.CastInfo.Owner as Champion;
            var spellLevel = owner.GetSpell("AlphaStrike").CastInfo.SpellLevel;              
            var ADratio = owner.Stats.AttackDamage.Total*0.9f;
            var damage =  -10 + 35 * spellLevel  + ADratio;
            AddParticleTarget(owner, target, "MasterYi_Base_Q_Tar.troy", target, 1f, 1f);
            AddParticleTarget(owner, target, ".MasterYi_Base_Q_Tar_Mark", target, 1f, 1f);
			AddParticleTarget(owner, target, "MasterYi_Base_Q_Hit.troy", target, 3f, 1f);
			AddParticleTarget(owner, target, "MasterYi_Base_Q_Crit_tar.troy", target, 3f, 1f);
			AddParticleTarget(owner, target, "MasterYi_Base_Q_Cas.troy", target, 3f, 1f);
			AddParticleTarget(owner, target, ".MasterYi_Base_Q_Mis", target, 3f, 1f);
			target.TakeDamage(owner, damage, DamageType.DAMAGE_TYPE_PHYSICAL, DamageSource.DAMAGE_SOURCE_SPELL, false);
			ApiEventManager.OnSpellMissileEnd.AddListener(this, missile, OnMissileEnd, true);
        }
		public void OnMissileEnd(SpellMissile missile)
        {
			var owner = missile.CastInfo.Owner as Champion;
			owner.RemoveBuffsWithName("AlphaStrikeTeleport");	        
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