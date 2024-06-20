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
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.Buildings;

namespace Spells
{
    internal class RivenTriCleave : ISpellScript
    {
        public SpellScriptMetadata ScriptMetadata => new SpellScriptMetadata()
        {
            TriggersSpellCasts = true
        };

        public void OnActivate(ObjAIBase owner, Spell spell)
        {          
        }     
        public void OnDeactivate(ObjAIBase owner, Spell spell)
        {
        }

        public void DashFin(AttackableUnit unit)
        {
			if (unit is ObjAIBase _owner)
            {
			var QLevel = _owner.GetSpell("RivenTriCleave").CastInfo.SpellLevel;
            var damage = 10 + (20 * (QLevel - 1)) + (_owner.Stats.AttackDamage.Total * 0.6f);
			if(dash == 1)
            {
				_owner.SkipNextAutoAttack();
                AddParticle(_owner, null, "exile_Q_01_detonate.troy", GetPointFromUnit(_owner, 125f));
				var units = GetUnitsInRange(GetPointFromUnit(_owner, 80f), 260f, true);
                for (int i = 0; i < units.Count; i++)
                {
                if (units[i].Team != _owner.Team && !(units[i] is ObjBuilding || units[i] is BaseTurret))
                    {					     
                         units[i].TakeDamage(_owner, damage, DamageType.DAMAGE_TYPE_PHYSICAL, DamageSource.DAMAGE_SOURCE_SPELL, false);
						 AddParticleTarget(_owner, units[i], "RivenQ_tar.troy", units[i], 10f,1,"");
				         AddParticleTarget(_owner, units[i], "exile_Q_tar_01.troy", units[i], 10f,1,"");
						 AddParticleTarget(_owner, units[i], "exile_Q_tar_04.troy", units[i], 10f,1,"");
                    }	
                }
            }
			if(dash == 2)
            {
				_owner.SkipNextAutoAttack();
                AddParticle(_owner, null, "exile_Q_02_detonate.troy", GetPointFromUnit(_owner, 125f));
				var units = GetUnitsInRange(GetPointFromUnit(_owner, 80f), 260f, true);
                for (int i = 0; i < units.Count; i++)
                {
                if (units[i].Team != _owner.Team && !(units[i] is ObjBuilding || units[i] is BaseTurret))
                    {					     
                         units[i].TakeDamage(_owner, damage, DamageType.DAMAGE_TYPE_PHYSICAL, DamageSource.DAMAGE_SOURCE_SPELL, false);
						 AddParticleTarget(_owner, units[i], "RivenQ_tar.troy", units[i], 10f,1,"");
				         AddParticleTarget(_owner, units[i], "exile_Q_tar_02.troy", units[i], 10f,1,"");
						 AddParticleTarget(_owner, units[i], "exile_Q_tar_04.troy", units[i], 10f,1,"");
                    }	
                }
            }
            if(dash == 3)
            {
				_owner.SkipNextAutoAttack();
				AddParticle(_owner, null, "exile_Q_03_detonate.troy", GetPointFromUnit(_owner, 125f)); 
                var units = GetUnitsInRange(GetPointFromUnit(_owner, 80f), 300f, true);
                for (int i = 0; i < units.Count; i++)
                {
                if (units[i].Team != _owner.Team && !(units[i] is ObjBuilding || units[i] is BaseTurret))
                    {					     
                         units[i].TakeDamage(_owner, damage, DamageType.DAMAGE_TYPE_PHYSICAL, DamageSource.DAMAGE_SOURCE_SPELL, false);
						 AddBuff("Pulverize", 0.75f, 1, _spell, units[i], _owner);
						 AddParticleTarget(_owner, units[i], "RivenQ_tar.troy", units[i], 10f,1,"");
				         AddParticleTarget(_owner, units[i], "exile_Q_tar_03.troy", units[i], 10f,1,"");
						 AddParticleTarget(_owner, units[i], "exile_Q_tar_04.troy", units[i], 10f,1,"");
                    }	
                }				
            }
			}
        }
        SpellSector DamageSector;
        int q = 0;
        ObjAIBase _owner;
        Spell _spell;
        public void OnSpellPreCast(ObjAIBase owner, Spell spell, AttackableUnit target, Vector2 start, Vector2 end)
        {
            _owner = owner;
            _spell = spell;
            var x = GetChampionsInRange(end, 200, true);
            foreach(var champ in x)
            {
                if(champ.Team != owner.Team)
                {
                    FaceDirection(champ.Position, owner);
                }
            }
        }

        public void OnSpellCast(Spell spell)
        {
        }
        int dash = 0;
        public void OnSpellPostCast(Spell spell)
        {
            var owner = spell.CastInfo.Owner;		
            AddBuff("RivenTriCleave", 4.0f, 1, spell, owner, owner as ObjAIBase);
			ApiEventManager.OnMoveEnd.AddListener(this, owner, DashFin, true);
            var getbuff = owner.GetBuffWithName("RivenTriCleave");
            switch (getbuff.StackCount)
            {
                case 1:
                    dash = 1;
                    PlayAnimation(owner, "Spell1A", 0.75f);
                    ForceMovement(owner, "Spell1A", GetPointFromUnit(owner, 225), 700, 0, 0, 0);
                    AddParticle(owner, owner, "Riven_Base_Q_01_Wpn_Trail.troy", owner.Position, bone: "chest");              
                    return;
                case 2:
                    dash = 2;
                    PlayAnimation(owner, "Spell1B", 0.75f);
                    ForceMovement(owner, "Spell1B", GetPointFromUnit(owner, 225), 700, 0, 0, 0);
                    AddParticle(owner, owner, "Riven_Base_Q_02_Wpn_Trail.troy", owner.Position, bone: "chest");              
                    return;
                case 3:
                    dash = 3;
                    PlayAnimation(owner, "Spell1C", 0.75f);
                    ForceMovement(owner, "Spell1C", GetPointFromUnit(owner, 250), 700, 0, 50, 0);
                    AddParticle(owner, owner, "Riven_Base_Q_03_Wpn_Trail.troy", owner.Position, size: -1);
					getbuff.DeactivateBuff();				
                    return;
            }
        }

        public void OnSpellChannel(Spell spell)
        {
        }

        public void OnSpellChannelCancel(Spell spell, ChannelingStopSource source)
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
