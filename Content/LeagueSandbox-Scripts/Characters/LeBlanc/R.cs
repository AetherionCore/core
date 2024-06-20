using GameServerCore.Enums;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.API;
using LeagueSandbox.GameServer.Scripting.CSharp;
using System.Numerics;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using LeagueSandbox.GameServer.GameObjects.SpellNS.Missile;
using LeagueSandbox.GameServer.GameObjects.SpellNS.Sector;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.GameObjects.SpellNS;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.Buildings;

namespace Spells
{
	public class LeblancMimic : ISpellScript
    {
        public SpellScriptMetadata ScriptMetadata => new SpellScriptMetadata()
        {
        };

        public void OnActivate(ObjAIBase owner, Spell spell)
        {
			ApiEventManager.OnLevelUpSpell.AddListener(this, spell, OnLevelUp, true);
        }
		public void OnLevelUp (Spell spell)
        {
			var owner = spell.CastInfo.Owner as Champion;
            AddBuff("LeblancMimic", 250000f, 1, spell, owner, owner);
			CreateTimer(0.1f, () =>
            {
			ApiEventManager.OnLevelUpSpell.RemoveListener(this);
			});
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
        }

        public void OnSpellPostCast(Spell spell)
        {
        }

        public void TargetExecute(Spell spell, AttackableUnit target, SpellMissile missile, SpellSector sector)
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
    public class LeblancChaosOrbM : ISpellScript
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
        }

        public void OnSpellPostCast(Spell spell)
        {
        }

        public void TargetExecute(Spell spell, AttackableUnit target, SpellMissile missile, SpellSector sector)
        {
            var owner = spell.CastInfo.Owner;
			var spellLevel = owner.GetSpell("LeblancSoulShackle").CastInfo.SpellLevel;
            var QLevel = owner.GetSpell("LeblancChaosOrb").CastInfo.SpellLevel;
            var AP = owner.Stats.AbilityPower.Total * 0.65f;
            var damage = 100 + 100f*(spell.CastInfo.SpellLevel - 1) + AP;
			var Qdamage = 55 + 25f*(QLevel - 1) + AP;
			var QMarkdamage = Qdamage + damage;
			var RQMarkdamage = damage * 2f;
            if (target.HasBuff("LeblancChaosOrb"))
            {
				target.TakeDamage(owner, QMarkdamage, DamageType.DAMAGE_TYPE_MAGICAL, DamageSource.DAMAGE_SOURCE_ATTACK,true);
				target.RemoveBuffsWithName("LeblancChaosOrb");
				AddBuff("LeblancChaosOrbM", 3.5f, 1, spell, target, owner);
            }
			else if (target.HasBuff("LeblancChaosOrbM"))
                     {
				       target.TakeDamage(owner, RQMarkdamage, DamageType.DAMAGE_TYPE_MAGICAL, DamageSource.DAMAGE_SOURCE_ATTACK,true);
				       target.RemoveBuffsWithName("LeblancChaosOrbM");
					   AddBuff("LeblancChaosOrbM", 3.5f, 1, spell, target, owner);
                     }
					 else
						 {
                           target.TakeDamage(owner, damage, DamageType.DAMAGE_TYPE_MAGICAL, DamageSource.DAMAGE_SOURCE_ATTACK, false);
						 }
			AddParticleTarget(owner, target, "LeBlanc_Base_RQ_tar", target);
            AddBuff("LeblancChaosOrbM", 3.5f, 1, spell, target, owner);
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
	
    public class LeblancSlideM: ISpellScript
    {

        public SpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
        {
            TriggersSpellCasts = true
        };

        public void OnActivate(ObjAIBase owner, Spell spell)
        {
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
			AddBuff("LeblancSlideM", 4.0f, 1, spell, owner, owner);
			Minion Leblanc = AddMinion(owner, "TestCube", "TestCube", owner.Position, owner.Team, owner.SkinID, true, false);
			AddBuff("LeblancSlideReturnM", 4.0f, 1, spell, Leblanc, owner);
        }

        public void OnSpellPostCast(Spell spell)
        {
			spell.SetCooldown(0.5f, true);
			var owner = spell.CastInfo.Owner;
			var Cursor = new Vector2(spell.CastInfo.TargetPosition.X, spell.CastInfo.TargetPosition.Z);
            var current = new Vector2(owner.Position.X, owner.Position.Y);
            var distance = Cursor - current;
            Vector2 truecoords;
            if (distance.Length() > 600f)
            {
                distance = Vector2.Normalize(distance);
                var range = distance * 600f;
                truecoords = current + range;
            }
            else
            {
                truecoords = Cursor;
            }
			var dist = System.Math.Abs(Vector2.Distance(truecoords, owner.Position));
            var time = dist / 1450f;
			owner.Stats.SetActionState(ActionState.CAN_MOVE, false);	 
			AddParticleTarget(owner, owner, "LeBlanc_Base_RW_mis.troy", owner, 0.5f);
			AddParticle(owner, null, "LeBlanc_Base_RW_cas.troy", owner.Position,time);
			FaceDirection(truecoords, spell.CastInfo.Owner, true);
            ForceMovement(owner, null, truecoords, 1450, 0, 0, 0);
            PlayAnimation(owner, "Spell2",time);
			//var time1 = dist + 0.2f;
			CreateTimer((float) time , () =>
            {
            if (spell.CastInfo.Owner is Champion c)
            {
				//c.GetSpell(1).LowerCooldown(20);
				StopAnimation(owner, "Spell2");
			    AddParticle(c, null, "LeBlanc_Base_RW_aoe_impact_02.troy", c.Position);

                var units = GetUnitsInRange(c.Position, 260f, true);
                for (int i = 0; i < units.Count; i++)
                {
                    if (units[i].Team != c.Team && !(units[i] is ObjBuilding || units[i] is BaseTurret))
                    {
						var SpellLevel = c.GetSpell("LeblancSoulShackle").CastInfo.SpellLevel;
						var QLevel = c.GetSpell("LeblancChaosOrb").CastInfo.SpellLevel;
						var AP = c.Stats.AbilityPower.Total * 0.6f;
						var MAXAP = c.Stats.AbilityPower.Total * 0.65f;
						var damage = 150 + (150 * (SpellLevel - 1)) + AP;
						var damageQMake=55 + 25f*(QLevel - 1) + AP;
						var damageeQW = damage + damageQMake;
						var damageRQMake=100 + 100f*(SpellLevel - 1)+ MAXAP;
						var damageeRQW = damage + damageRQMake;
						if (units[i].HasBuff("LeblancChaosOrb"))
                            {
							units[i].TakeDamage(c, damageeQW, DamageType.DAMAGE_TYPE_MAGICAL, DamageSource.DAMAGE_SOURCE_SPELL, true);
							AddParticleTarget(c, units[i], "LeBlanc_Base_RW_tar.troy", units[i], 1f);
				            AddParticleTarget(c, units[i], "LeBlanc_Base_RW_tar_02.troy", units[i], 1f);
					        units[i].RemoveBuffsWithName("LeblancChaosOrb");
                            }
						if (units[i].HasBuff("LeblancChaosOrbM"))
                            {
							units[i].TakeDamage(c, damageeRQW, DamageType.DAMAGE_TYPE_MAGICAL, DamageSource.DAMAGE_SOURCE_SPELL, true);
							AddParticleTarget(c, units[i], "LeBlanc_Base_RW_tar.troy", units[i], 1f);
				            AddParticleTarget(c, units[i], "LeBlanc_Base_RW_tar_02.troy", units[i], 1f);
					        units[i].RemoveBuffsWithName("LeblancChaosOrbM");
                            }
						else
							{
                            units[i].TakeDamage(c, damage, DamageType.DAMAGE_TYPE_MAGICAL, DamageSource.DAMAGE_SOURCE_SPELL, false);
						    AddParticleTarget(c, units[i], "LeBlanc_Base_RW_tar.troy", units[i], 1f);
				            AddParticleTarget(c, units[i], "LeBlanc_Base_RW_tar_02.troy", units[i], 1f);
						    }
                    }
                }             
            }
           });			
        }
		public void TargetExecute(Spell spell, AttackableUnit target, SpellMissile missile, SpellSector sector)
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
	public class LeblancSlideReturnM: ISpellScript
    {

       public SpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
        {
            TriggersSpellCasts = true
        };

        public void OnActivate(ObjAIBase owner, Spell spell)
        {
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
    public class LeblancSoulShackleM : ISpellScript
    {
        public SpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
        {
            TriggersSpellCasts = true,
            MissileParameters = new MissileParameters
            {
                Type = MissileType.Circle
            }
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
			AddParticleTarget(owner, owner, "LeBlanc_Base_RE_cas", owner, bone:"R_HAND");
			AddParticleTarget(owner, owner, "LeBlanc_Base_RE_cas_02", owner, bone:"R_HAND");
        }

        public void OnSpellPostCast(Spell spell)
        {
        }

        public void TargetExecute(Spell spell, AttackableUnit target, SpellMissile missile, SpellSector sector)
        {
            var owner = spell.CastInfo.Owner;
            if (missile is SpellCircleMissile skillshot)
            {
            var spellLevel = owner.GetSpell("LeblancSoulShackle").CastInfo.SpellLevel;
            var AP = owner.Stats.AbilityPower.Total * 0.65f;
            var damage = 100 + 100f*(spellLevel - 1) + AP;
		    var QLevel = owner.GetSpell("LeblancChaosOrb").CastInfo.SpellLevel;
			var MAXAP = spell.CastInfo.Owner.Stats.AbilityPower.Total * 0.65f;    
			var damagemax=55 + 25f*(QLevel - 1) + AP;
			var QMarkdamage = damage + damagemax;	
			var damagemaxx=100 + 100f*(spellLevel - 1)+ MAXAP;
			var RQMarkdamage = damage + damagemaxx;
                if (target.HasBuff("LeblancChaosOrb"))
                {							
				    target.RemoveBuffsWithName("LeblancChaosOrb");
					target.TakeDamage(owner, QMarkdamage, DamageType.DAMAGE_TYPE_MAGICAL, DamageSource.DAMAGE_SOURCE_ATTACK, true);
                }
			    else if (target.HasBuff("LeblancChaosOrbM"))
                {
					target.RemoveBuffsWithName("LeblancChaosOrbM");
					target.TakeDamage(owner, RQMarkdamage, DamageType.DAMAGE_TYPE_MAGICAL, DamageSource.DAMAGE_SOURCE_ATTACK, true);
                }
				else
				{
				    target.TakeDamage(owner, damage, DamageType.DAMAGE_TYPE_MAGICAL, DamageSource.DAMAGE_SOURCE_ATTACK, false);
                }
				AddParticleTarget(owner, target, "LeBlanc_Base_RQ_tar", target);
				AddParticleTarget(owner, owner, "LeBlanc_Base_RE_chain", target, lifetime: 1.5f,1,"R_HAND","C_BuffBone_Glb_Center_Loc");
				//AddParticleTarget(owner, target, "LeBlanc_Base_RE_indicator", owner, lifetime: 1.5f);
				AddParticleTarget(owner, target, "LeBlanc_Base_RE_tar_02", target);
                AddBuff("LeblancSoulShackleM", 1.5f, 1, spell, target, owner);
				missile.SetToRemove();
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