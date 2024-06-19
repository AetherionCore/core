using GameServerCore.Enums;
using LeagueSandbox.GameServer.Scripting.CSharp;
using LeagueSandbox.GameServer.API;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using LeagueSandbox.GameServer.GameObjects.SpellNS;
using System.Numerics;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using GameServerLib.GameObjects.AttackableUnits;
using GameServerCore;
using LeagueSandbox.GameServer.GameObjects.SpellNS.Missile;
using LeagueSandbox.GameServer.GameObjects.SpellNS.Sector;

namespace Spells
{
    public class KatarinaR : ISpellScript
    {

        public SpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
        {
            NotSingleTargetSpell = true,
            TriggersSpellCasts = true,
            ChannelDuration = 2.5f,
        };

        private Vector2 basepos;
        public SpellSector DamageSector;
        ObjAIBase Owner;

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
        public void TargetExecute(Spell spell, AttackableUnit target, SpellMissile swag, SpellSector sector)
        {


        }


        public void OnSpellChannel(Spell spell)
        {
            var owner = spell.CastInfo.Owner;
            AddBuff("KatarinaR", 2.5f, 1, spell, owner, owner);
        }

        public void OnSpellChannelCancel(Spell spell, ChannelingStopSource reason)
        {
			var owner = spell.CastInfo.Owner;
            owner.RemoveBuffsWithName("KatarinaR");
        }

        public void OnSpellPostChannel(Spell spell)
        {
        }

        public void OnUpdate(float diff)
        {
        }
    }






    public class KatarinaRMis : ISpellScript
    {
        public SpellScriptMetadata ScriptMetadata => new SpellScriptMetadata()
        {
            TriggersSpellCasts = true,
            IsDamagingSpell = true,
            MissileParameters = new MissileParameters
            {
                Type = MissileType.Target
            }
        };

        public void OnActivate(ObjAIBase owner, Spell spell)
        {
			ApiEventManager.OnSpellHit.AddListener(this, spell, TargetExecute, false);
        }

        public void TargetExecute(Spell spell, AttackableUnit target, SpellMissile missile, SpellSector sector)
        {
			var owner = spell.CastInfo.Owner;
            var AP = owner.Stats.AbilityPower.Total * 0.25f;
            var AD = owner.Stats.AttackDamage.FlatBonus * 0.375f;
            float damage = 15f + ( 20f * spell.CastInfo.SpellLevel) + AP + AD;
			target.TakeDamage(owner, damage, DamageType.DAMAGE_TYPE_MAGICAL, DamageSource.DAMAGE_SOURCE_SPELLAOE, false);
			AddParticleTarget(owner, target, "katarina_deathLotus_tar.troy", target);         	
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
