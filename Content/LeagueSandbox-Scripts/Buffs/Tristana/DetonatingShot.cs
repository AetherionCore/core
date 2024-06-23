using GameServerCore.Enums;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.GameObjects;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using LeagueSandbox.GameServer.GameObjects.SpellNS;
using LeagueSandbox.GameServer.GameObjects.StatsNS;
using LeagueSandbox.GameServer.Scripting.CSharp;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using GameServerLib.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.API;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.Buildings;

namespace Buffs
{
    internal class DetonatingShot : IBuffGameScript
    {
        float Damage;
        ObjAIBase Tristana;
        AttackableUnit Unit;
        public BuffScriptMetaData BuffMetaData { get; set; } = new BuffScriptMetaData
        {
            BuffType = BuffType.AURA,
            BuffAddType = BuffAddType.REPLACE_EXISTING
        };
        public StatsModifier StatsModifier { get; private set; } = new StatsModifier();
        public void OnActivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {
            Unit = unit;
            Tristana = ownerSpell.CastInfo.Owner as Champion;
            ApiEventManager.OnKill.AddListener(this, Tristana, Boom, false);
            ApiEventManager.OnKillUnit.AddListener(this, Tristana, Boom, false);
        }
        public void Boom(DeathData deathData)
        {
            Unit = deathData.Unit;
            AddParticleTarget(Tristana, Unit, "DetonatingShot_buf.troy", Unit);
            Damage = 25 + (25 * Tristana.Spells[2].CastInfo.SpellLevel) + (Tristana.Stats.AbilityPower.Total * 0.25f);
            var units = GetUnitsInRange(Unit.Position, 250f, true);
            for (int i = 0; i < units.Count; i++)
            {
                if (units[i].Team != Tristana.Team && !(units[i] is ObjBuilding || units[i] is BaseTurret))
                {
                    units[i].TakeDamage(Tristana, Damage, DamageType.DAMAGE_TYPE_MAGICAL, DamageSource.DAMAGE_SOURCE_SPELL, false);
                }
            }
        }
    }
}