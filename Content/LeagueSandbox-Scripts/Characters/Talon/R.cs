using GameServerCore.Enums;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using LeagueSandbox.GameServer.Scripting.CSharp;
using System.Numerics;
using LeagueSandbox.GameServer.API;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using LeagueSandbox.GameServer.GameObjects.SpellNS;
using LeagueSandbox.GameServer.GameObjects.SpellNS.Missile;
using LeagueSandbox.GameServer.GameObjects.SpellNS.Sector;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.Buildings;
using LeagueSandbox.GameServer.GameObjects.StatsNS;
using LeagueSandbox.GameServer.GameObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace Spells
{
    public class TalonShadowAssault : ISpellScript
    {
		float timeSinceLastTick = 1000f;
        public StatsModifier StatsModifier { get; private set; } = new StatsModifier();

        Buff HandlerBuff;

        public SpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
        {
            TriggersSpellCasts = true,
            IsDamagingSpell = true
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
            var owner = spell.CastInfo.Owner as Champion;
            PlayAnimation(owner, "Spell4");
			AddBuff("TalonShadowAssaultBuff", 2.5f, 1, spell, owner, owner, false);
            for (int bladeCount = 0; bladeCount <= 7; bladeCount++)
            {				           
                var start = GetPointFromUnit(owner, 25f, bladeCount * 45f);
				var end = GetPointFromUnit(owner, 615f, bladeCount * 45f);
				SpellCast(owner, 3, SpellSlotType.ExtraSlots, end, Vector2.Zero, true, start);
				SpellCast(owner, 5, SpellSlotType.ExtraSlots, end, Vector2.Zero, true, start);			            			
            }			
            AddParticleTarget(owner, owner, "talon_ult_cas.troy", owner, 10f);
        }
		public void SetSpellToggle(bool toggle)
        {
        }

        public void OnSpellPostCast(Spell spell)
        {
            spell.SetCooldown(0.25f, true);
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

    public class TalonShadowAssaultMisOne : ISpellScript
    {
        public SpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
        {
            MissileParameters = new MissileParameters
            {
                Type = MissileType.Circle
            },
            IsDamagingSpell = true,
            TriggersSpellCasts = true
        };
        public List<AttackableUnit> UnitsHit = new List<AttackableUnit>();

        public void OnActivate(ObjAIBase owner, Spell spell)
        {
            ApiEventManager.OnSpellHit.AddListener(this, spell, TargetExecute, false);
        }

        public void OnDeactivate(ObjAIBase owner, Spell spell)
        {
        }

        public void OnSpellPreCast(ObjAIBase owner, Spell spell, AttackableUnit target, Vector2 start, Vector2 end)
        {
            UnitsHit.Clear();
        }

        public void OnMissileEnd(SpellMissile missile)
        {           
        }

        public void TargetExecute(Spell spell, AttackableUnit target, SpellMissile missile, SpellSector sector)
        {
            var owner = spell.CastInfo.Owner;
            var ADratio = owner.Stats.AttackDamage.Total * 0.75f;
            var damage = 120 + 50f * (owner.GetSpell("TalonCutthroat").CastInfo.SpellLevel - 1) + ADratio;
            var ELevel = owner.GetSpell("TalonCutthroat").CastInfo.SpellLevel;
			var damageamp = 0.03f * (ELevel - 1);
			if (target.HasBuff("TalonDamageAmp"))
            {
				damage = damage + damage * damageamp;
            }
            if (!UnitsHit.Contains(target) && target != spell.CastInfo.Owner)
            {
				if (target.Team != owner.Team && !(target is ObjBuilding || target is BaseTurret))
				{
                UnitsHit.Add(target);
                target.TakeDamage(owner, damage, DamageType.DAMAGE_TYPE_PHYSICAL, DamageSource.DAMAGE_SOURCE_ATTACK, false);
                AddParticleTarget(owner, target, "talon_ult_tar.troy", target, 1f);
				}
            }
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
	public class TalonShadowAssaultMisOneHalf : ISpellScript
    {
		Spell spell;
        public SpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
        {
            MissileParameters = new MissileParameters
            {
                Type = MissileType.Circle
            },
            IsDamagingSpell = true,
            TriggersSpellCasts = true

            // TODO
        };
        public List<AttackableUnit> UnitsHit = new List<AttackableUnit>();

        public void OnActivate(ObjAIBase owner, Spell spell)
        {
            
        }
        public void OnDeactivate(ObjAIBase owner, Spell spell)
        {
        }

        public void OnSpellPreCast(ObjAIBase owner, Spell spell, AttackableUnit target, Vector2 start, Vector2 end)
        {
            UnitsHit.Clear();
            var missile = spell.CreateSpellMissile(new MissileParameters
            {
                Type = MissileType.Circle,
                OverrideEndPosition = end
            });

            ApiEventManager.OnSpellMissileEnd.AddListener(this, missile, OnMissileEnd, true);
        }

        public void OnMissileEnd(SpellMissile missile)
        {
            var owner = missile.CastInfo.Owner;
            //owner.GetSpell("TalonShadowAssaultToggle").SetCooldown(0f);			
            Minion Blade = AddMinion(owner, "TestCube", "TestCube", missile.Position, owner.Team, owner.SkinID, true, false);
			AddBuff("TalonShadowAssaultMisBuff", 2.24f, 1, spell, Blade, Blade, false);
			AddBuff("TalonShadowAssault", 2.24f, 1, spell, Blade, Blade, false);
        }
        public void TargetExecute(Spell spell, AttackableUnit target, SpellMissile missile, SpellSector sector)
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
    public class TalonShadowAssaultMisTwo : ISpellScript
    {
        public SpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
        {
            MissileParameters = new MissileParameters
            {
                Type = MissileType.Circle
            },
            IsDamagingSpell = true,
            TriggersSpellCasts = true
        };
        public List<AttackableUnit> UnitsHit = new List<AttackableUnit>();

        public void OnActivate(ObjAIBase owner, Spell spell)
        {
            ApiEventManager.OnSpellHit.AddListener(this, spell, TargetExecute, false);
        }

        public void OnDeactivate(ObjAIBase owner, Spell spell)
        {
        }

        public void OnSpellPreCast(ObjAIBase owner, Spell spell, AttackableUnit target, Vector2 start, Vector2 end)
        {
            UnitsHit.Clear();
            var missile = spell.CreateSpellMissile(new MissileParameters
            {
                Type = MissileType.Circle,
                OverrideEndPosition = end
            });
        }
     
        public void TargetExecute(Spell spell, AttackableUnit target, SpellMissile missile, SpellSector sector)
        {
			if (target == missile.CastInfo.Owner)
            {
				missile.SetToRemove();            
            }
            var owner = spell.CastInfo.Owner;         
            var ADratio = owner.Stats.AttackDamage.Total * 0.75f;
            var damage = 120 + 50f * (owner.GetSpell("TalonCutthroat").CastInfo.SpellLevel - 1) + ADratio;
            var ELevel = owner.GetSpell("TalonCutthroat").CastInfo.SpellLevel;
			var damageamp = 0.03f * (ELevel - 1);
			if (target.HasBuff("TalonDamageAmp"))
            {
				damage = damage + damage * damageamp;
            }

            if (!UnitsHit.Contains(target) && target != spell.CastInfo.Owner)
            {
				if (target.Team != owner.Team && !(target is ObjBuilding || target is BaseTurret))
				{
                UnitsHit.Add(target);
                target.TakeDamage(owner, damage, DamageType.DAMAGE_TYPE_PHYSICAL, DamageSource.DAMAGE_SOURCE_ATTACK, false);
                AddParticleTarget(owner, target, "talon_ult_tar.troy", target, 1f);
				}
            }
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
	public class TalonShadowAssaultToggle : ISpellScript
    {
        public SpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
        {
            TriggersSpellCasts = true,
            IsDamagingSpell = true
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
}

