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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace Spells
{
    public class TalonRake : ISpellScript
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
            //PlayAnimation(owner, "Spell1");
        }

        public void OnSpellCast(Spell spell)
        {
            var owner = spell.CastInfo.Owner as Champion;  
             for (int bladeCount = 0; bladeCount <= 2; bladeCount++)
             {              
                 var start = GetPointFromUnit(owner, 25f,  (-20f + (bladeCount * 20f)));
				 var end = GetPointFromUnit(owner, 700f,  (-20f + (bladeCount * 20f)));
				 SpellCast(owner, 1, SpellSlotType.ExtraSlots, end, Vector2.Zero, true, start);       
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

    public class TalonRakeMissileOne : ISpellScript
    {
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

            ApiEventManager.OnSpellMissileEnd.AddListener(this, missile, OnMissileEnd, true);
        }

        public void TargetExecute(Spell spell, AttackableUnit target, SpellMissile missile, SpellSector sector)
        {
            var owner = spell.CastInfo.Owner;
            var spellLevel = owner.GetSpell("TalonRake").CastInfo.SpellLevel;
            var ADratio = owner.Stats.AttackDamage.Total * 0.6f;
            var damage = 30 + 25f*(spellLevel - 1) + ADratio;
            var ELevel = owner.GetSpell("TalonCutthroat").CastInfo.SpellLevel;
			var damageamp = 0.03f * (ELevel - 1);
			if (target.HasBuff("TalonDamageAmp"))
            {
				damage = damage + damage * damageamp;
            }
            if (!UnitsHit.Contains(target))
            {
                UnitsHit.Add(target);
                target.TakeDamage(owner, damage, DamageType.DAMAGE_TYPE_PHYSICAL, DamageSource.DAMAGE_SOURCE_ATTACK, false);
                AddBuff("TalonSlow", 2f, 1, spell, target, owner); //TODO: Find Proper Name
                AddParticleTarget(owner, target, "talon_w_tar.troy", target, 1f);
            }
        }

        public void OnMissileEnd(SpellMissile missile)
        {
            var owner = missile.CastInfo.Owner;
            SpellCast(owner, 2, SpellSlotType.ExtraSlots, true, owner, missile.Position);
            //SpellCast(owner, 2, SpellSlotType.ExtraSlots, owner.Position, owner.Position, true, missile.Position);
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
    public class TalonRakeMissileTwo : ISpellScript
    {
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
            ApiEventManager.OnSpellHit.AddListener(this, spell, TargetExecute, false);
        }

        public void OnDeactivate(ObjAIBase owner, Spell spell)
        {
        }

        public void OnSpellPreCast(ObjAIBase owner, Spell spell, AttackableUnit target, Vector2 start, Vector2 end)
        {
            UnitsHit.Clear();
        }

        public void TargetExecute(Spell spell, AttackableUnit target, SpellMissile missile, SpellSector sector)
        {
            if (target == missile.CastInfo.Owner)
            {
				missile.SetToRemove();
            }
            var owner = spell.CastInfo.Owner;
            var spellLevel = owner.GetSpell("TalonRake").CastInfo.SpellLevel;
            var ADratio = owner.Stats.AttackDamage.Total * 0.6f;
            var damage = 30 + 25f * (spellLevel - 1) + ADratio;
			var ELevel = owner.GetSpell("TalonCutthroat").CastInfo.SpellLevel;
			var damageamp = 0.03f * (ELevel - 1);
			if (target.HasBuff("TalonDamageAmp"))
            {
				damage = damage + damage * damageamp;
            }
            if (!UnitsHit.Contains(target))
            {
				if (target.Team != owner.Team && !(target is ObjBuilding || target is BaseTurret))
				{
                UnitsHit.Add(target);
                target.TakeDamage(owner, damage, DamageType.DAMAGE_TYPE_PHYSICAL, DamageSource.DAMAGE_SOURCE_ATTACK, false);
                AddBuff("TalonSlow", 2f, 1, spell, target, owner); //TODO: Find Proper Name
                AddParticleTarget(owner, target, "talon_w_tar.troy", target, 1f);
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
}
