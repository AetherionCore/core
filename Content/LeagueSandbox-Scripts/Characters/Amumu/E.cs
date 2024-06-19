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
    public class Tantrum : ISpellScript
    {
        public SpellScriptMetadata ScriptMetadata => new SpellScriptMetadata()
        {
            TriggersSpellCasts = true
            // TODO
        };

        public void OnActivate(ObjAIBase owner, Spell spell)
        {
            ApiEventManager.OnSpellHit.AddListener(this, spell, TargetExecute, false);
			ApiEventManager.OnLevelUpSpell.AddListener(this, spell, OnLevelUp, true);
        }
		public void OnLevelUp (Spell spell)
        {
			var owner = spell.CastInfo.Owner;
            AddBuff("Tantrum", 250000.0f, 1, spell, owner, owner);
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
            var sector = spell.CreateSpellSector(new SectorParameters
            {
                Length = 450f,
                SingleTick = true,
                Type = SectorType.Area
            });
			AddParticleTarget(owner, owner, "Tantrum_cas", owner, 10f);
        }
        public void TargetExecute(Spell spell, AttackableUnit target, SpellMissile missile, SpellSector sector)
        {
            var owner = spell.CastInfo.Owner;
            var AP = spell.CastInfo.Owner.Stats.AbilityPower.Total * 0.5f;
            var AD = spell.CastInfo.Owner.Stats.AttackDamage.Total * 0.6f;
            var damage = 50 + owner.GetSpell("Tantrum").CastInfo.SpellLevel * 25 + AP;
            target.TakeDamage(owner, damage, DamageType.DAMAGE_TYPE_MAGICAL, DamageSource.DAMAGE_SOURCE_SPELLAOE, false);
            AddParticleTarget(owner, target, "Amumu_Sadrobot_Tantrum_tar", target, 1f);
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
