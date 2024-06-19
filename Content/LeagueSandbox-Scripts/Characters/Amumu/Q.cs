using GameServerCore.Enums;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using LeagueSandbox.GameServer.Scripting.CSharp;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using LeagueSandbox.GameServer.GameObjects.SpellNS;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.GameObjects.SpellNS.Missile;
using LeagueSandbox.GameServer.GameObjects.SpellNS.Sector;
using System.Numerics;
using LeagueSandbox.GameServer.API;

namespace Spells
{
    public class BandageToss : ISpellScript
    {
        public SpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
        {
            TriggersSpellCasts = true,
            IsDamagingSpell = true
        };

        private ObjAIBase _owner;
        private Spell _spell;

        public void OnActivate(ObjAIBase owner, Spell spell)
        {
            _owner = owner;
            _spell = spell;
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
            var owner = spell.CastInfo.Owner as Champion;
            var ownerSkinID = owner.SkinID;
            var targetPos = GetPointFromUnit(owner, 1150.0f);      
            FaceDirection(targetPos, owner);     
            SpellCast(owner, 0, SpellSlotType.ExtraSlots, targetPos, targetPos, false, Vector2.Zero);
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
            //SetSpellToolTipVar(_owner, 2, _owner.Stats.AttackDamage.Total * _spell.SpellData.AttackDamageCoefficient, SpellbookType.SPELLBOOK_CHAMPION, 0, SpellSlotType.SpellSlots);
        }
    }

    public class SadMummyBandageToss : ISpellScript
    {
        public SpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
        {
            MissileParameters = new MissileParameters
            {
                Type = MissileType.Circle
            },
            IsDamagingSpell = true
            // TODO
        };

        //Vector2 direction;

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
            var owner = spell.CastInfo.Owner;
			var dist = System.Math.Abs(Vector2.Distance(target.Position, owner.Position));
			var time = dist/1400f;
			var time2 = time + 1f;
			PlayAnimation(owner, "Spell2");
			AddBuff("Ghosted", time2, 1, spell, target, owner);
            //var ad = owner.Stats.AttackDamage.Total * spell.SpellData.AttackDamageCoefficient;
            var ap = owner.Stats.AbilityPower.Total * 0.7f;
            var damage = 30 + owner.GetSpell("BandageToss").CastInfo.SpellLevel * 50 + ap;
            target.TakeDamage(owner, damage, DamageType.DAMAGE_TYPE_MAGICAL, DamageSource.DAMAGE_SOURCE_ATTACK, false);       
            AddParticleTarget(owner, target, "BandageToss_tar", target);
			AddBuff("Stun", 1f, 1, spell, target, owner);
			ForceMovement(owner, null, target.Position, 1400, 0, 0, 0);
			CreateTimer((float) time , () =>
            {  
			StopAnimation(owner, "Spell2");
			});
            missile.SetToRemove();

            // SpellBuffAdd EzrealRisingSpellForce
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
