using System.Numerics;
using GameServerCore.Enums;
using GameServerCore.Scripting.CSharp;
using LeaguePackets.Game.Events;
using LeagueSandbox.GameServer.API;
using LeagueSandbox.GameServer.GameObjects;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.Buildings;
using LeagueSandbox.GameServer.GameObjects.SpellNS;
using LeagueSandbox.GameServer.GameObjects.SpellNS.Missile;
using LeagueSandbox.GameServer.GameObjects.SpellNS.Sector;
using LeagueSandbox.GameServer.GameObjects.StatsNS;
using LeagueSandbox.GameServer.Scripting.CSharp;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;

namespace Buffs
{
    class MonkeyKingDecoyClone : IBuffGameScript
    {
        public BuffScriptMetaData BuffMetaData { get; set; } = new BuffScriptMetaData
        {
            BuffAddType = BuffAddType.RENEW_EXISTING
        };

        public StatsModifier StatsModifier { get; private set; } = new StatsModifier();

        ObjAIBase Owner;
        Particle p;
        Particle p2;
        Buff thisBuff;
        public void OnActivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {
            thisBuff = buff;
            Owner = ownerSpell.CastInfo.Owner;
            OverrideAnimation(unit, "spell2", "death");
            AddParticleTarget(Owner, unit, "MonkeyKing_Base_W_Copy.troy", unit, buff.Duration, 1);

        }
        public void OnDeactivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {
            RemoveBuff(thisBuff);
            unit.TakeDamage(unit, 1000000, DamageType.DAMAGE_TYPE_TRUE, DamageSource.DAMAGE_SOURCE_SPELL, false);
            SetStatus(unit, StatusFlags.NoRender, true);
            if (ownerSpell.CastInfo.Owner is Champion c)
            {
                AddParticle(unit, null, "MonkeyKing_Base_W_Cas_Team_ID_Green.troy", unit.Position);
                AddParticle(unit, null, "MonkeyKing_Base_W_Death_Team_ID_Green.troy", unit.Position);
                AddParticleTarget(unit, unit, "Become_Transparent.troy", unit);
                AddParticleTarget(c, c, ".troy", c, 10f);
                var damage = 65 + (35 * (ownerSpell.CastInfo.SpellLevel - 1)) + (c.Stats.AttackDamage.Total * 0.2f);
                var units = GetUnitsInRange(c.Position, 350f, true);
                for (int i = 0; i < units.Count; i++)
                {
                    if (units[i].Team != c.Team && !(units[i] is ObjBuilding || units[i] is BaseTurret))
                    {
                        units[i].TakeDamage(c, damage, DamageType.DAMAGE_TYPE_PHYSICAL, DamageSource.DAMAGE_SOURCE_ATTACK, false);
                        AddParticleTarget(c, units[i], "MonkeyKing_Base_W_Tar_Decoy.troy", units[i], 1f);
                        AddParticleTarget(c, units[i], ".troy", units[i], 1f);
                    }
                }
            }
        }
    }
}