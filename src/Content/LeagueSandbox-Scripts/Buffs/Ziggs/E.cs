using System.Numerics;
using GameServerCore.Enums;
using GameServerCore.Scripting.CSharp;
using GameServerLib.GameObjects.AttackableUnits;
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
    class ZiggsE : IBuffGameScript
    {
        float T;
        Particle P;
        Spell S;
        Buff Ebuff;
        ObjAIBase Owner;
        AttackableUnit U;

        public BuffScriptMetaData BuffMetaData { get; set; } = new BuffScriptMetaData
        {
            BuffType = BuffType.COMBAT_ENCHANCER,
            BuffAddType = BuffAddType.REPLACE_EXISTING,
        };

        public StatsModifier StatsModifier { get; private set; } = new StatsModifier();

        public void OnActivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {
            U = unit;
            Ebuff = buff;
            S = ownerSpell;
            Owner = ownerSpell.CastInfo.Owner;
            P = AddParticle(Owner, null, "ZiggsE_Mis_Small.troy", unit.Position, 10f);
            P = AddParticle(Owner, null, "ZiggsE_mis_mineopen.troy", unit.Position, 10f);
        }
        public void Boom(Spell spell)
        {
            Ebuff.DeactivateBuff();
            if (spell.CastInfo.Owner is Champion c)
            {
                AddParticle(c, null, "", P.Position, 10f);
                AddParticle(c, null, ".troy", P.Position, 10f);
                var units = GetUnitsInRange(P.Position, 20f, true);
                var damage = 15 + (25 * spell.CastInfo.SpellLevel) + (c.Stats.AbilityPower.Total * 0.3f);
                for (int i = 0; i < units.Count; i++)
                {
                    if (units[i].Team != c.Team && !(units[i] is ObjBuilding || units[i] is BaseTurret))
                    {
                        AddBuff("", 2.5f, 1, S, units[i], c, false);
                        AddParticleTarget(c, units[i], "ZiggsE_tar", units[i], 10);
                        AddParticle(c, null, "ZiggsEMine.troy", U.Position, 10f);
                        units[i].TakeDamage(c, damage, DamageType.DAMAGE_TYPE_MAGICAL, DamageSource.DAMAGE_SOURCE_SPELL, false);
                        //AddParticleTarget(c, units[i], "Ekko_Base_W_Shield_HitDodge", units[i]);
                    }
                }
            }
        }
        public void OnDeactivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {
            RemoveParticle(P);
            unit.Die(CreateDeathData(false, 0, unit, unit, DamageType.DAMAGE_TYPE_TRUE, DamageSource.DAMAGE_SOURCE_INTERNALRAW, 0.0f));
        }

        public void OnUpdate(float diff)
        {
            T += diff;
            if (T >= 1)
            {
                T = 0;
                if (S.CastInfo.Owner is Champion c)
                {
                    var units = GetUnitsInRange(P.Position, 25f, true);
                    for (int i = 0; i < units.Count; i++)
                    {
                        if (units[i].Team != c.Team && !(units[i] is ObjBuilding || units[i] is BaseTurret))
                        {
                            Boom(S);
                        }
                    }
                }
            }
        }
    }
}