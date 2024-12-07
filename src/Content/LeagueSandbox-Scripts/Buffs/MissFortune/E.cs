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
using LeagueSandbox.GameServer.GameObjects.StatsNS;
using LeagueSandbox.GameServer.Scripting.CSharp;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;

namespace Buffs
{
    class MissFortuneScattershot : IBuffGameScript
    {
        public BuffScriptMetaData BuffMetaData { get; set; } = new BuffScriptMetaData
        {
            BuffAddType = BuffAddType.RENEW_EXISTING
        };

        public StatsModifier StatsModifier { get; private set; } = new StatsModifier();

        float T;
        float AP;
        float damage;
        ObjAIBase Owner;
        Minion E;
        Spell S;
        Particle P;

        public void OnActivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {
            S = ownerSpell;
            Owner = ownerSpell.CastInfo.Owner;
            AP = Owner.Stats.AbilityPower.Total * 0.8f;
            damage = (35f + (55f * Owner.GetSpell("MissFortuneScattershot").CastInfo.SpellLevel) + AP) / 12;
            P = AddParticle(Owner, null, "MissFortune_Base_E_Hit.troy", unit.Position, 3f, 1);
        }

        public void OnDeactivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {
            RemoveParticle(P);
            unit.TakeDamage(unit, 100000, DamageType.DAMAGE_TYPE_TRUE, DamageSource.DAMAGE_SOURCE_SPELLAOE, false);
        }

        public void OnUpdate(float diff)
        {
            T += diff;
            if (T >= 0)
            {
                T = -250;
                if (S.CastInfo.Owner is Champion c)
                {
                    var units = GetUnitsInRange(P.Position, 350f, true);
                    for (int i = 0; i < units.Count; i++)
                    {
                        if (units[i].Team != c.Team && !(units[i] is ObjBuilding || units[i] is BaseTurret))
                        {
                            AddBuff("", 2.5f, 1, S, units[i], c, false);
                            AddParticleTarget(c, units[i], "MissFortune_Base_E_Unit_Tar", units[i], 10);
                            units[i].TakeDamage(c, damage, DamageType.DAMAGE_TYPE_MAGICAL, DamageSource.DAMAGE_SOURCE_SPELLAOE, false);
                            //AddParticleTarget(c, units[i], "Ekko_Base_W_Shield_HitDodge", units[i]);
                        }
                    }
                }
            }
        }
    }
}