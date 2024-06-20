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

namespace CharScripts
{
    public class CharScriptTalon : ICharScript
    {
        Spell Spell;
		AttackableUnit Target;
        public void OnActivate(ObjAIBase owner, Spell spell = null)

        {
            Spell = spell;
            {
                ApiEventManager.OnLaunchAttack.AddListener(this, owner, OnLaunchAttack, false);
            }
        }
        public void OnLaunchAttack(Spell spell)        
        {
			var owner = spell.CastInfo.Owner;
            Target = spell.CastInfo.Targets[0].Unit;
			var damage = owner.Stats.AttackDamage.Total * 0.1f ;
			var ELevel = owner.GetSpell("TalonCutthroat").CastInfo.SpellLevel;
			var damageamp = 0.03f * (ELevel - 1);
			var Edamage = owner.Stats.AttackDamage.Total * damageamp ;
			if (Target.HasBuff("TalonSlow")||Target.HasBuff("TalonESlow")||Target.HasBuff("Stun")||Target.HasBuff("Slow")|| Target.HasBuff("Disarm")|| Target.HasBuff("Silence")|| Target.HasBuff("Blind")|| Target.HasBuff("Pulverize"))
			{
				Target.TakeDamage(owner, damage, DamageType.DAMAGE_TYPE_PHYSICAL, DamageSource.DAMAGE_SOURCE_PERIODIC, false);
			}
			if (Target.HasBuff("TalonDamageAmp"))
            {
				Target.TakeDamage(owner, Edamage, DamageType.DAMAGE_TYPE_PHYSICAL, DamageSource.DAMAGE_SOURCE_PERIODIC, false);
            }
        }       
        public void OnDeactivate(ObjAIBase owner, Spell spell = null)
        {
        }
        public void OnUpdate(float diff)
        {
        }
    }
}

