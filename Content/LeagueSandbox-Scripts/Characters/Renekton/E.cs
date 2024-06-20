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
    public class RenektonSliceAndDice : ISpellScript
    {
        public SpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
        {
            CastingBreaksStealth = true,
            DoesntBreakShields = true,
            TriggersSpellCasts = true,
            IsDamagingSpell = false,
            NotSingleTargetSpell = true
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
			PlayAnimation(owner, "Spell3",0.4f);
			ApiEventManager.OnMoveEnd.AddListener(this, owner, DashFin, true);
			var ownerSkinID = owner.SkinID;
			var Blood = owner.Stats.ManaPoints.Total * 0.5f;
			var Health = owner.Stats.CurrentMana;       
            if (Health >= Blood)
			{
				owner.Stats.CurrentMana -= 50f;
				AddParticleTarget(owner, owner, "Renekton_Base_E_rage.troy", owner);
			}			
			else
			{
				AddParticleTarget(owner, owner, "Renekton_Base_E_cas.troy", owner);
			}
			spell.CreateSpellSector(new SectorParameters
                    {
                        BindObject = owner,
                        Length = 450f,
                        Width = 80f,
                        PolygonVertices = new Vector2[]
                    {
                    new Vector2(-1, 0),
                    new Vector2(-1, 1),
                    new Vector2(1, 1),
                    new Vector2(1, 0)
                    },
                        SingleTick = true,
                        Type = SectorType.Polygon
                    });
            var spellPos = new Vector2(spell.CastInfo.TargetPosition.X, spell.CastInfo.TargetPosition.Z);

            FaceDirection(spellPos, owner, true);
            var trueCoords = GetPointFromUnit(owner, spell.SpellData.CastRangeDisplayOverride);

            ForceMovement(owner, null, trueCoords, 1400, 0, 0, 0);         
        }
		public void TargetExecute(Spell spell, AttackableUnit target, SpellMissile missile, SpellSector sector)
        {
            var owner = spell.CastInfo.Owner;
            var ADratio = owner.Stats.AttackDamage.Total;
            var Damage = ADratio + (spell.CastInfo.SpellLevel * 20);
            target.TakeDamage(owner, Damage, DamageType.DAMAGE_TYPE_PHYSICAL, DamageSource.DAMAGE_SOURCE_SPELL, false);
			owner.Stats.CurrentMana += 10f;
            AddParticleTarget(owner, target, "Renekton_Base_E_tar.troy", target);
            AddBuff("RenektonSliceAndDiceDelay", 4.0f, 1, spell, owner, owner);
        }
		public void DashFin(AttackableUnit owner)
        {
            if (owner is ObjAIBase c)
            {
                StopAnimation(c, "Spell3");				 
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
	public class RenektonDice : ISpellScript
    {
        public SpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
        {
            CastingBreaksStealth = true,
            DoesntBreakShields = true,
            TriggersSpellCasts = true,
            IsDamagingSpell = false,
            NotSingleTargetSpell = true
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
			owner.RemoveBuffsWithName("RenektonSliceAndDiceDelay");
			PlayAnimation(owner, "Spell3",0.4f);
			ApiEventManager.OnMoveEnd.AddListener(this, owner, DashFin, true);
			if (owner.HasBuff("RenektonReignOfTheTyrant"))
            {
				AddParticleTarget(owner, owner, "Renekton_Base_E_rage.troy", owner, 1f,1,"C_BuffBone_Glb_Center_Loc");
			}
			else
			{
				AddParticleTarget(owner, owner, "Renekton_Base_E_cas.troy", owner, 1f,1,"C_BuffBone_Glb_Center_Loc");
			}
			spell.CreateSpellSector(new SectorParameters
                    {
                        BindObject = owner,
                        Length = 450f,
                        Width = 80f,
                        PolygonVertices = new Vector2[]
                    {
                    new Vector2(-1, 0),
                    new Vector2(-1, 1),
                    new Vector2(1, 1),
                    new Vector2(1, 0)
                    },
                        SingleTick = true,
                        Type = SectorType.Polygon
                    });
            var spellPos = new Vector2(spell.CastInfo.TargetPosition.X, spell.CastInfo.TargetPosition.Z);

            FaceDirection(spellPos, owner, true);
            var trueCoords = GetPointFromUnit(owner, spell.SpellData.CastRangeDisplayOverride);

            ForceMovement(owner, null, trueCoords, 1400, 0, 0, 0);         
        }
		public void TargetExecute(Spell spell, AttackableUnit target, SpellMissile missile, SpellSector sector)
        {
            var owner = spell.CastInfo.Owner;
            var ADratio = owner.Stats.AttackDamage.Total;
            var Damage = ADratio + (spell.CastInfo.SpellLevel * 20);
            target.TakeDamage(owner, Damage, DamageType.DAMAGE_TYPE_PHYSICAL, DamageSource.DAMAGE_SOURCE_SPELL, false);
            AddParticleTarget(owner, target, "Renekton_Base_E_tar.troy", target);
        }
		public void DashFin(AttackableUnit owner)
        {
            if (owner is ObjAIBase c)
            {
                StopAnimation(c, "Spell3");				 
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
