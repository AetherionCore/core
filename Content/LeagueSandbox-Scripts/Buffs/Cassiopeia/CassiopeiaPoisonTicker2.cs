using GameServerCore.Enums;
using LeagueSandbox.GameServer.GameObjects.StatsNS;
using LeagueSandbox.GameServer.API;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.Scripting.CSharp;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.GameObjects;
using LeagueSandbox.GameServer.GameObjects.SpellNS;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;

namespace Buffs
{
    internal class CassiopeiaPoisonTicker2 : IBuffGameScript
    {
        public BuffScriptMetaData BuffMetaData { get; set; } = new BuffScriptMetaData
        {
            BuffType = BuffType.POISON
        };

        public BuffAddType BuffAddType => BuffAddType.REPLACE_EXISTING;
        public int MaxStacks => 1;
        public bool IsHidden => false;


        public StatsModifier StatsModifier { get; private set; } = new StatsModifier();

        private ObjAIBase owner;
        private ObjAIBase _spell;
        private AttackableUnit Unit;
        private Particle p;
        private float damage;
        private float timeSinceLastTick = 500f;

        public void OnActivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {
            owner = ownerSpell.CastInfo.Owner;
            Unit = unit;
            float APratio = owner.Stats.AbilityPower.Total * 0.1f;
            damage = 5.0f + (5.0f * ownerSpell.CastInfo.SpellLevel) + APratio;
            p = AddParticleTarget(owner, unit, "Global_Poison.troy", unit, buff.Duration, bone: "BUFFBONE_GLB_CHANNEL_LOC");
            StatsModifier.MoveSpeed.PercentBonus -= 0.35f + 0.05f * ownerSpell.CastInfo.SpellLevel;

            unit.AddStatModifier(StatsModifier);
        }

        public void OnDeactivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {
            RemoveParticle(p);
        }

        public void OnUpdate(float diff)
        {
            timeSinceLastTick += diff;
            if (timeSinceLastTick >= 1000f && !Unit.IsDead && Unit != null)
            {
                Unit.TakeDamage(owner, damage, DamageType.DAMAGE_TYPE_MAGICAL, DamageSource.DAMAGE_SOURCE_PERIODIC, false);
                if (Unit is Champion)
                {
                    AddBuff("CassiopeiaDeadlyCadence", float.MaxValue, 1, owner.GetSpell("CassiopeiaNoxiousBlast"), owner, owner, true);
                }
                timeSinceLastTick = 0;
            }
        }
    }
}