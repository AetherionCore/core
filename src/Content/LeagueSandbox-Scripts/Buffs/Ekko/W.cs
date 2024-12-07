using GameServerCore.Enums;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.Buildings;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.GameObjects.SpellNS.Sector;
using LeagueSandbox.GameServer.GameObjects.SpellNS;
using LeagueSandbox.GameServer.GameObjects.StatsNS;
using LeagueSandbox.GameServer.GameObjects;
using LeagueSandbox.GameServer.Scripting.CSharp;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;

namespace Buffs
{
    class EkkoW : IBuffGameScript
    {
        public BuffScriptMetaData BuffMetaData { get; set; } = new BuffScriptMetaData
        {
            BuffAddType = BuffAddType.RENEW_EXISTING
        };

        public StatsModifier StatsModifier { get; private set; } = new StatsModifier();
        float T;
        Buff WB;
        Spell S;
        ObjAIBase Owner;
        Minion W;
        Particle P;
        public SpellSector AOE;
        AttackableUnit U;

        public void OnActivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {
            U = unit;
            WB = buff;
            S = ownerSpell;
            Owner = ownerSpell.CastInfo.Owner;
            P = AddParticle(Owner, null, "Ekko_Base_W_Detonate_Slow.troy", unit.Position, 10f);
            var units = GetUnitsInRange(P.Position, 350f, true);
            for (int i = 0; i < units.Count; i++)
            {
                if (units[i].Team != Owner.Team && !(units[i] is ObjAIBase || units[i] is BaseTurret))
                {
                    AddBuff("EkkoSlow", 2f, 1, S, units[i], Owner, false);
                }
            }
        }
        public void OnDeactivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {
            unit.TakeDamage(unit, 100000, DamageType.DAMAGE_TYPE_TRUE, DamageSource.DAMAGE_SOURCE_SPELLAOE, false);
        }
        public void Boom(Spell spell)
        {
            WB.DeactivateBuff();
            if (spell.CastInfo.Owner is Champion c)
            {
                AddParticle(c, null, "", P.Position, 10f);
                AddParticle(c, null, "Ekko_Base_W_Detonate.troy", P.Position, 10f);
                var units = GetUnitsInRange(P.Position, 350f, true);
                for (int i = 0; i < units.Count; i++)
                {
                    if (units[i].Team != c.Team && !(units[i] is ObjBuilding || units[i] is BaseTurret))
                    {
                        AddBuff("EkkoWStun", 2.5f, 1, S, units[i], c, false);
                        AddParticleTarget(c, units[i], "Ekko_Base_W_Crit_Tar", units[i], 10);
                        //AddParticleTarget(c, units[i], "Ekko_Base_W_Shield_HitDodge", units[i]);
                    }
                }
            }
        }
        public void OnUpdate(float diff)
        {
            T += diff;
            if (T >= 1)
            {
                T = 0;
                if (S.CastInfo.Owner is Champion c)
                {
                    var units = GetUnitsInRange(P.Position, 350f, true);
                    for (int i = 0; i < units.Count; i++)
                    {
                        if (units[i] == c)
                        {
                            Boom(S);
                            AddBuff("EkkoWShield", 2f, 1, S, c, c, false);
                        }
                    }
                }
            }
        }
    }
}