using System.Numerics;
using GameServerCore.Enums;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using LeagueSandbox.GameServer.Scripting.CSharp;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.API;
using Buffs;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using LeagueSandbox.GameServer.GameObjects.SpellNS;
using LeagueSandbox.GameServer.GameObjects.SpellNS.Missile;
using LeagueSandbox.GameServer.GameObjects.SpellNS.Sector;


namespace Passives
{
    public class Headshot_Marker : ICharScript
    {
        private ObjAIBase _owner;

        public void OnActivate(ObjAIBase owner, Spell spell)
        {
            _owner = owner;
            //ApiEventManager.OnHitUnitByAnother.AddListener(this, owner, TargetExecute, false);
        }

        int i = 0;

        public void TargetExecute(AttackableUnit target, bool isCrit)
        {
            i++;
            if (i == 6)
            {
                i = 0;
                if (target is Minion)
                {
                    target.TakeDamage(_owner, _owner.Stats.AttackDamage.Total, DamageType.DAMAGE_TYPE_PHYSICAL, DamageSource.DAMAGE_SOURCE_ATTACK, isCrit);
                }
                target.TakeDamage(_owner, _owner.Stats.AttackDamage.Total * 0.5f, DamageType.DAMAGE_TYPE_PHYSICAL, DamageSource.DAMAGE_SOURCE_ATTACK, isCrit);
                _owner.RemoveBuffsWithName("CaitlynHeadshotReady");
            }
            LogDebug(i.ToString());
        }

        public void OnDeactivate(ObjAIBase owner, Spell spell)
        {
        }

        public void OnUpdate(float diff)
        {
        }
    }
}