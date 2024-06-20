using GameServerCore.Enums;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using LeagueSandbox.GameServer.Scripting.CSharp;
using System.Numerics;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using LeagueSandbox.GameServer.GameObjects.SpellNS;
using LeagueSandbox.GameServer.GameObjects.SpellNS.Missile;
using LeagueSandbox.GameServer.GameObjects.SpellNS.Sector;
using LeagueSandbox.GameServer.GameObjects;

namespace Spells
{
    public class ZedUlt: ISpellScript
    {
        AttackableUnit Target;
		Buff HandlerBuff;
        Minion Shadow;
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
			Target = target;
			var ownerSkinID = owner.SkinID;
            Minion Zed = AddMinion((Champion)owner, "Zed", "Zed", owner.Position, owner.Team, owner.SkinID, true, false);
			SetStatus(Zed, StatusFlags.NoRender, true);
            PlayAnimation(owner, "Spawn");			
			var dist = System.Math.Abs(Vector2.Distance(Target.Position, owner.Position));
			var distt = dist + 1;
			var time1 = distt / 1400f;
			var time2 = time1 + 0.7f;
			SealSpellSlot(owner, SpellSlotType.SpellSlots, 3, SpellbookType.SPELLBOOK_CHAMPION, true);
			AddBuff("ZedBuffer", time2, 1, spell, owner, owner);
			AddBuff("ZedRHandler", 6.0f, 1, spell, owner, owner, false);
			AddBuff("ZedR2", 5.9f, 1, spell, owner, owner);
			CreateTimer(0.7f, () =>
            {
			SetStatus(owner, StatusFlags.NoRender, true);
			SetStatus(Zed, StatusFlags.NoRender, false);
			PlayAnimation(Zed, "spell4_strike");			
            FaceDirection(target.Position, spell.CastInfo.Owner, true);
            ForceMovement(Zed, null, target.Position, 1400f, 0, 0, 0);
			if (ownerSkinID == 1)
                {
                AddParticleTarget(owner, Zed, "Zed_Skin01_R_Dash.troy", owner, time2);
                }
				else
				{
                AddParticleTarget(owner, Zed, "Zed_R_Dash.troy", owner, time2);
				}
			});
			CreateTimer(time2, () =>
            {
			AddBuff("ZedUltExecute", 3f, 1, spell, target, owner);
			AddBuff("Ghosted", 3f, 1, spell, owner, owner);
			SetStatus(Zed, StatusFlags.NoRender, true);
			SetStatus(owner, StatusFlags.NoRender, false);
			Zed.TakeDamage(Zed, 10000f, DamageType.DAMAGE_TYPE_TRUE, DamageSource.DAMAGE_SOURCE_INTERNALRAW, DamageResultType.RESULT_NORMAL);
			SealSpellSlot(owner, SpellSlotType.SpellSlots, 3, SpellbookType.SPELLBOOK_CHAMPION, false);
			});
        }

        public void OnSpellCast(Spell spell)
        {
			var owner = spell.CastInfo.Owner;
        }

        public void OnSpellPostCast(Spell spell)
        {
			spell.SetCooldown(0.5f, true);
			var owner = spell.CastInfo.Owner;
			owner.StopMovement();
			var dist = System.Math.Abs(Vector2.Distance(Target.Position, owner.Position));
			var distt = dist + 1;
			var time1 = distt / 1400f;
			var time2 = time1 + 0.7f;
			var targetPos = GetPointFromUnit(owner,distt);
			var randPoint1 = new Vector2(owner.Position.X + (10.0f), owner.Position.Y + 10.0f);	
			ForceMovement(owner, null, randPoint1, 0.5f, 0, -280, 0);
			AddParticleTarget(owner, Target, "Zed_Ult_TargetMarker_tar.troy", Target, 10f);		
			Minion Shadow = AddMinion(owner, "ZedShadow", "ZedShadow", owner.Position, owner.Team, owner.SkinID, true, false);
			AddBuff("ZedRShadowBuff", 6.0f, 1, spell, Shadow, owner);
			CreateTimer(0.7f, () =>
            {
			PlayAnimation(owner, "spell4_strike");
			owner.SetDashingState(false);	
            FaceDirection(targetPos, spell.CastInfo.Owner, true);
            ForceMovement(owner, null, targetPos, 1400f, 0, 0, 0);
            AddParticleTarget(owner, owner, "Zed_R_Dash.troy", owner, time2);
			});
			CreateTimer(time2, () =>
            {
                PlayAnimation(owner, "spell4_leadin");					
			    for (int bladeCount = 0; bladeCount <= 2; bladeCount++)
                  {	                			  
                    var targetPosReturn = GetPointFromUnit(owner, 600f, bladeCount * 120f);
					Minion m = AddMinion((Champion)owner, "ZedShadow", "ZedShadow", targetPosReturn, owner.Team, owner.SkinID, true, false);				
					PlayAnimation(m, "spell4_strike");
					AddParticleTarget(owner, m, "Zed_R_Dash.troy", m);
                    var targetPos = GetPointFromUnit(Target, 100f);
			        ForceMovement(m, null, targetPos, 1400, 0, 0, 0);					
					AddBuff("ZedUltDashCloneMaker", 65f, 1, spell, m, m);
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
	public class ZedUltDash: ISpellScript
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
	public class ZedR2: ISpellScript
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

}