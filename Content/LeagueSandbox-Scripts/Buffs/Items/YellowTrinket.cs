using GameServerCore.Enums;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.GameObjects.SpellNS;
using LeagueSandbox.GameServer.GameObjects;
using LeagueSandbox.GameServer.GameObjects.StatsNS;
using LeagueSandbox.GameServer.Scripting.CSharp;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using GameServerLib.GameObjects.AttackableUnits;

namespace Buffs
{
    internal class YellowTrinket : IBuffGameScript
    {
        public BuffScriptMetaData BuffMetaData { get; set; } = new BuffScriptMetaData
        {
            BuffType = BuffType.INTERNAL,
            BuffAddType = BuffAddType.RENEW_EXISTING
        };

        public StatsModifier StatsModifier { get; private set; } = new StatsModifier();

        AttackableUnit Unit;
        Spell spell;
        float timeSinceLastTick = 0f;
        float counter;
        Region revealStealthed;

        public void OnActivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {
            revealStealthed = AddUnitPerceptionBubble(unit, 1000.0f, 25000f, unit.Team, true);
            Unit = unit;
            spell = ownerSpell;
            Unit.Stats.ManaRegeneration.PercentBonus = -1;
            Unit.Stats.CurrentMana = 60f;
        }

        public void OnUpdate(float diff)
        {
            timeSinceLastTick += diff;

            if(timeSinceLastTick >= 60000.0f)
            {
                Unit.TakeDamage(spell.CastInfo.Owner, 10000f, DamageType.DAMAGE_TYPE_TRUE, DamageSource.DAMAGE_SOURCE_INTERNALRAW, DamageResultType.RESULT_NORMAL);
            }
            //This would be used if the ward's ManaPoints were being properly read
            /*if (timeSinceLastTick >= 1000.0f)
            {
                Unit.Stats.ManaPoints.FlatBonus -= 1;
                if(Unit.Stats.CurrentMana == 0)
                {
                  Unit.Die(Unit);
                }
            }*/

        }

        public void OnDeactivate(DeathData death)
        {
            revealStealthed.SetToRemove();
        }
    }
}