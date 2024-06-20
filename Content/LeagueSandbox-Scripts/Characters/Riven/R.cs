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
using System.Collections.Generic;

namespace Spells
{
    internal class RivenFengShuiEngine : ISpellScript
    {
        public SpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
        {
            TriggersSpellCasts = true
            // TODO
        };

        string pcastname;
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
			spell.SetCooldown(0.5f, true);	
            var owner = spell.CastInfo.Owner;
			AddBuff("RivenR2", 15f, 1, spell, spell.CastInfo.Owner, spell.CastInfo.Owner);
            AddBuff("RivenFengShuiEngine", 15f, 1, spell, spell.CastInfo.Owner, spell.CastInfo.Owner);			
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
    public class RivenIzunaBlade : ISpellScript
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
        }

        public void OnSpellPostCast(Spell spell)
        {
             var owner = spell.CastInfo.Owner as Champion;               		 
			 var truePos = GetPointFromUnit(owner, 1000f);
			 var targetPos = GetPointFromUnit(owner, 900f, -13f);
			 var Pos = GetPointFromUnit(owner, 900f, 13f);
			 SpellCast(owner, 5, SpellSlotType.ExtraSlots, truePos, truePos, true, Vector2.Zero);
			 SpellCast(owner, 3, SpellSlotType.ExtraSlots, targetPos, targetPos, true, Vector2.Zero);
             SpellCast(owner, 3, SpellSlotType.ExtraSlots, Pos, Pos, true, Vector2.Zero);				 
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

    public class RivenLightsaberMissile : ISpellScript
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
            var spellLevel = owner.GetSpell("RivenMartyr").CastInfo.SpellLevel;
            var ADratio = owner.Stats.AttackDamage.Total * 0.6f;
            var damage = 30 + 25f*(spellLevel - 1) + ADratio;         
            if (!UnitsHit.Contains(target))
            {
                UnitsHit.Add(target);
                target.TakeDamage(owner, damage, DamageType.DAMAGE_TYPE_PHYSICAL, DamageSource.DAMAGE_SOURCE_ATTACK, false);
                AddParticleTarget(owner, target, "Riven_Base_R_Tar.troy", target, 1f);
				AddParticleTarget(owner, target, "Riven_Base_R_Tar_Minion.troy", target, 1f);
            }
        }

        public void OnMissileEnd(SpellMissile missile)
        {
            var owner = missile.CastInfo.Owner;
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
	public class RivenLightsaberMissileSide : ISpellScript
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
            var spellLevel = owner.GetSpell("RivenMartyr").CastInfo.SpellLevel;
            var ADratio = owner.Stats.AttackDamage.Total * 0.6f;
            var damage = 30 + 25f*(spellLevel - 1) + ADratio;         
            if (!UnitsHit.Contains(target))
            {
                UnitsHit.Add(target);
                target.TakeDamage(owner, damage, DamageType.DAMAGE_TYPE_PHYSICAL, DamageSource.DAMAGE_SOURCE_ATTACK, false);
                AddParticleTarget(owner, target, "Riven_Base_R_Tar.troy", target, 1f);
				AddParticleTarget(owner, target, "Riven_Base_R_Tar_Minion.troy", target, 1f);
            }
        }

        public void OnMissileEnd(SpellMissile missile)
        {
            var owner = missile.CastInfo.Owner;
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
}
