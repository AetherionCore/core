using GameServerCore.Enums;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using LeagueSandbox.GameServer.GameObjects;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.GameObjects.SpellNS;
using LeagueSandbox.GameServer.GameObjects.StatsNS;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.Scripting.CSharp;
using LeagueSandbox.GameServer.API;                   
using GameServerLib.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;

namespace Buffs
{
    internal class VisionWard : IBuffGameScript
    {
        public BuffScriptMetaData BuffMetaData { get; set; } = new BuffScriptMetaData
        {
            BuffType = BuffType.INTERNAL,
            BuffAddType = BuffAddType.RENEW_EXISTING
        };

        public StatsModifier StatsModifier { get; private set; } = new StatsModifier();
        Region revealStealthed;

        public void OnActivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {
            revealStealthed = AddUnitPerceptionBubble(unit, 1000.0f, 25000f, unit.Team, true);
            ApiEventManager.OnDeath.AddListener(this, unit, OnDeactivate);

            ApiEventManager.OnPreTakeDamage.AddListener(this, unit, OnPreTakeDamage, false);
        }
        public void OnDeactivate(DeathData death)
        {
            revealStealthed.SetToRemove();
        }

        public void OnPreTakeDamage(DamageData damage)
        {
            var attacker = damage.Attacker;
            var owner = damage.Target;

            if (attacker is not BaseTurret && damage.DamageSource == DamageSource.DAMAGE_SOURCE_ATTACK && damage.Damage > 1f)
            {
                damage.PostMitigationDamage = 1f;
            }
        }
    }
}