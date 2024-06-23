using LeagueSandbox.GameServer.GameObjects.StatsNS;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.Scripting.CSharp;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using LeagueSandbox.GameServer.GameObjects;
using LeagueSandbox.GameServer.GameObjects.SpellNS;
using GameServerCore.Enums;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using GameServerLib.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.API;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.Buildings;

namespace Buffs
{
    internal class JaxCounterStrike : IBuffGameScript
    {
        float Damage;
        ObjAIBase Jax;
        Buff AttackBuff;
        float TrueCooldown;
        public BuffScriptMetaData BuffMetaData { get; set; } = new BuffScriptMetaData
        {
            BuffType = BuffType.COMBAT_ENCHANCER,
            BuffAddType = BuffAddType.REPLACE_EXISTING
        };


        public StatsModifier StatsModifier { get; private set; } = new StatsModifier();

        public void OnActivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {
            AttackBuff = buff;
            Jax = ownerSpell.CastInfo.Owner as Champion;
            PlayAnimation(Jax, "spell3", buff.Duration);
            ApiEventManager.OnTakeDamage.AddListener(this, Jax, TakeDamage, false);
        }
        public void TakeDamage(DamageData damageData)
        {

        }
        public void OnDeactivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {
            StopAnimation(Jax, "spell3");
            PlayAnimation(Jax, "spell3b", 0.3f);
            AddParticleTarget(Jax, Jax, "Counterstrike_cas.troy", Jax, 1f, 1);
            Damage = 25 + (30 * Jax.Spells[2].CastInfo.SpellLevel) + (Jax.Stats.AttackDamage.FlatBonus * 0.5f);
            var units = GetUnitsInRange(Jax.Position, 400f, true);
            for (int i = 0; i < units.Count; i++)
            {
                if (!(units[i].Team == Jax.Team || units[i] is BaseTurret || units[i] is ObjBuilding))
                {
                    units[i].TakeDamage(Jax, Damage, DamageType.DAMAGE_TYPE_PHYSICAL, DamageSource.DAMAGE_SOURCE_SPELLAOE, false);
                    AddParticleTarget(Jax, units[i], "globalhit_orange_tar.troy", units[i], 1f);
                    AddBuff("Stun", 1f, 1, Jax.Spells[2], units[i], Jax, false);
                }
            }
            TrueCooldown = (18 - (Jax.Spells[0].CastInfo.SpellLevel * 2)) * (1 + Jax.Stats.CooldownReduction.Total);
            Jax.Spells[2].SetCooldown(TrueCooldown, true);
        }
    }
}