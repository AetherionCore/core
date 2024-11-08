using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using LeagueSandbox.GameServer.Scripting.CSharp;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using LeagueSandbox.GameServer.GameObjects.SpellNS;
using GameServerCore.Enums;
using LeagueSandbox.GameServer.API;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;
using System.Numerics;
using LeagueSandbox.GameServer.GameObjects.SpellNS.Missile;
using LeagueSandbox.GameServer.GameObjects.SpellNS.Sector;


namespace Spells
{
    public class ThreshE : ISpellScript
    {
        public SpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
        {
			NotSingleTargetSpell = true,
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
			//FaceDirection(end, owner,true);
			//PlayAnimation(owner, "Spell1_in");
        }

        public void OnSpellCast(Spell spell)
        {
            //var owner = spell.CastInfo.Owner;
            //AddParticleTarget(owner, owner, "", owner, bone: "");
        }

        public void OnSpellPostCast(Spell spell)
        {
            var owner = spell.CastInfo.Owner as Champion;
			//PlayAnimation(owner, "Thresh_spell1_out");
            var ownerSkinID = owner.SkinID;
			var Start = GetPointFromUnit(owner, -450f);
            var End = GetPointFromUnit(owner, 450f);
			FaceDirection(End, owner,true);
            var ownerPos = owner.Position;
            SpellCast(owner, 5, SpellSlotType.ExtraSlots, End, Vector2.Zero, true, Start);   
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

    public class ThreshEMissile1 : ISpellScript
    {
        public SpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
        {
            MissileParameters = new MissileParameters
            {
                Type = MissileType.Circle
            },
			TriggersSpellCasts = true,
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
            var ap = owner.Stats.AbilityPower.Total * 0.65f;
            var damage = 40 + spell.CastInfo.Owner.GetSpell("ThreshE").CastInfo.SpellLevel * 40 + ap;
            target.TakeDamage(owner, damage, DamageType.DAMAGE_TYPE_MAGICAL, DamageSource.DAMAGE_SOURCE_SPELL, false);
            ForceMovement(target, null, GetPointFromUnit(missile, 200), 160, 0, 10, 0);        	
            AddParticleTarget(owner, target, "", target);

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