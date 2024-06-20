using GameServerCore.Enums;
using LeagueSandbox.GameServer.GameObjects.StatsNS;
using LeagueSandbox.GameServer.Scripting.CSharp;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.GameObjects;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.GameObjects.SpellNS;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using LeagueSandbox.GameServer.API;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.Buildings.AnimatedBuildings;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.Buildings;

namespace Buffs
{
    class ObduracyBuff : IBuffGameScript
    {
        public BuffScriptMetaData BuffMetaData { get; set; } = new BuffScriptMetaData
        {
            BuffType = BuffType.COMBAT_ENCHANCER,
            BuffAddType = BuffAddType.REPLACE_EXISTING
        };

        private ObjAIBase Owner;
        private Spell daspell;
        private ObjAIBase daowner;
        AttackableUnit Unit;

        public StatsModifier StatsModifier { get; private set; } = new StatsModifier();

        Buff thisBuff;

        public void OnActivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {
            Unit = unit;
            thisBuff = buff;
            Owner = ownerSpell.CastInfo.Owner;
            daowner = Owner;
            daspell = ownerSpell;
            AddParticleTarget(daowner, unit, "Malphite_Enrage_buf", unit, buff.Duration, 1, "L_HAND");
            AddParticleTarget(daowner, unit, "Malphite_Enrage_buf", unit, buff.Duration, 1, "R_HAND");
            AddParticleTarget(daowner, unit, "Malphite_Enrage_glow", unit, buff.Duration, 1, "L_HAND");
            AddParticleTarget(daowner, unit, "Malphite_Enrage_glow", unit, buff.Duration, 1, "R_HAND");
            SealSpellSlot(daowner, SpellSlotType.SpellSlots, 1, SpellbookType.SPELLBOOK_CHAMPION, true);
            ApiEventManager.OnLaunchAttack.AddListener(this, ownerSpell.CastInfo.Owner, OnLaunchAttack, false);
        }

        public void OnLaunchAttack(Spell spell)
        {
            if (!thisBuff.Elapsed() && thisBuff != null && Unit != null)
            {
                var Owner = daspell.CastInfo.Owner as Champion;
                var Elevel = Owner.GetSpell("Obduracy").CastInfo.SpellLevel;
                var AD = Owner.Stats.AbilityPower.Total * 0.1f;
                var Damage = 30 + (15 * (Elevel - 1)) + AD;
                //target.TakeDamage(owner, Damage, DamageType.DAMAGE_TYPE_TRUE, DamageSource.DAMAGE_SOURCE_PERIODIC, false);
                AddParticleTarget(Owner, spell.CastInfo.Owner.TargetUnit, "TiamatMelee_itm_hydra.troy", Owner, 1f, 0.7f);
                var units = GetUnitsInRange(spell.CastInfo.Owner.TargetUnit.Position, 250f, true);
                for (int i = 0; i < units.Count; i++)
                {
                    if (!(units[i].Team == Owner.Team || units[i] is BaseTurret || units[i] is ObjBuilding || units[i] is Inhibitor))
                    {
                        units[i].TakeDamage(Owner, Damage, DamageType.DAMAGE_TYPE_PHYSICAL, DamageSource.DAMAGE_SOURCE_SPELLAOE, false);
                        AddParticleTarget(Owner, units[i], ".troy", Owner, 10f);
                    }
                }
            }
        }

        public void OnDeactivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {
            ApiEventManager.OnHitUnit.RemoveListener(this, unit as ObjAIBase);
            //beidongdonghua
            //AddParticleTarget(unit, unit, "Malphite_Base_Obduracy_off", unit, buff.Duration, 1);
            SealSpellSlot(ownerSpell.CastInfo.Owner, SpellSlotType.SpellSlots, 1, SpellbookType.SPELLBOOK_CHAMPION, false);
        }

        public void OnUpdate(float diff)
        {
        }
    }
}