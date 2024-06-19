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
    public class IreliaGatotsu : ISpellScript
    {
        AttackableUnit Target;
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
            Target = target;
			owner.CancelAutoAttack(false);
        }

        public void OnSpellCast(Spell spell)
        {       
        }

        public void OnSpellPostCast(Spell spell)
        {
            var owner = spell.CastInfo.Owner;     
            var ad = owner.Stats.AttackDamage.Total * 1.2f;
            var damage = 20 + 30* (spell.CastInfo.SpellLevel-1) + ad;
            var dist = System.Math.Abs(Vector2.Distance(Target.Position, owner.Position));
			var distt = dist - 1;
			var targetPos = GetPointFromUnit(owner,distt);
            var time = dist / 2200f;
			PlayAnimation(owner, "Spell1",time);
			AddBuff("Ghosted", time, 1, spell, owner, owner);
			CreateTimer((float) time , () =>
            {                           
            Target.TakeDamage(owner, damage, DamageType.DAMAGE_TYPE_PHYSICAL, DamageSource.DAMAGE_SOURCE_SPELL, false);
			AddParticleTarget(owner, Target, "irelia_gotasu_tar.troy", Target, 10f);
			if (Target.IsDead)
            {
			AddParticleTarget(owner, owner, "irelia_gotasu_mana_refresh.troy", owner, time);
			AddParticleTarget(owner, owner, "irelia_gotasu_ability_indicator.troy", owner, time);
			}
            });
			FaceDirection(targetPos, owner, true);
            ForceMovement(owner, null, targetPos, 2200, 0, 0, 0);
			AddParticle(owner, null, "irelia_gotasu_cas.troy", owner.Position, lifetime: 10f);
			AddParticle(owner, null, "irelia_gotasu_cast_01.troy", owner.Position, lifetime: 10f);
			AddParticle(owner, null, "irelia_gotasu_cast_02.troy", owner.Position, lifetime: 10f);
			AddParticleTarget(owner, owner, "irelia_gotasu_dash_01.troy", owner, time);
			AddParticleTarget(owner, owner, "irelia_gotasu_dash_02.troy", owner, time); 			
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