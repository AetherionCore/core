using GameServerCore.Enums;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.API;
using LeagueSandbox.GameServer.GameObjects;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using LeagueSandbox.GameServer.GameObjects.SpellNS;
using LeagueSandbox.GameServer.GameObjects.SpellNS.Missile;
using LeagueSandbox.GameServer.GameObjects.SpellNS.Sector;
using LeagueSandbox.GameServer.GameObjects.StatsNS;
using LeagueSandbox.GameServer.Scripting.CSharp;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;

namespace Buffs
{
    class AuraofDespair : IBuffGameScript
    {
        public BuffScriptMetaData BuffMetaData { get; set; } = new BuffScriptMetaData
        {
            BuffType = BuffType.COMBAT_ENCHANCER,
            BuffAddType = BuffAddType.REPLACE_EXISTING
        };

        public StatsModifier StatsModifier { get; private set; } = new StatsModifier();

        private float DamageManaTimer;
        private Spell originSpell;
        private float[] manaCost = { 8.0f, 8.0f, 8.0f, 8.0f, 8.0f };
        private ObjAIBase Owner;
        private Buff thisBuff;
        public SpellSector AuraAmumu;

        public void OnActivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {
            Owner = ownerSpell.CastInfo.Owner;
            originSpell = ownerSpell;
            thisBuff = buff;
            ApiEventManager.OnSpellHit.AddListener(this, ownerSpell, TargetExecute, false);

            AuraAmumu = ownerSpell.CreateSpellSector(new SectorParameters
            {
                BindObject = ownerSpell.CastInfo.Owner,
                Length = 300f,
                Tickrate = 1,
                CanHitSameTargetConsecutively = true,
                OverrideFlags = SpellDataFlags.AffectEnemies | SpellDataFlags.AffectNeutral | SpellDataFlags.AffectMinions | SpellDataFlags.AffectHeroes,
                Type = SectorType.Area
            });
        }

        public void TargetExecute(Spell spell, AttackableUnit target, SpellMissile missile, SpellSector sector)
        {
            float ap = Owner.Stats.AbilityPower.Total;
            float lvlmaxhp = (((0.0025f * (ap / 100)) + (0.00425f + 0.00075f * spell.CastInfo.SpellLevel)) * target.Stats.HealthPoints.Total);
            var damage = 4 + spell.CastInfo.SpellLevel * 2 + lvlmaxhp;

            target.TakeDamage(Owner, damage, DamageType.DAMAGE_TYPE_MAGICAL, DamageSource.DAMAGE_SOURCE_SPELLAOE, false);
        }

        public void OnDeactivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {
            ApiEventManager.OnSpellHit.RemoveListener(this);
            AuraAmumu.SetToRemove();
        }

        public void OnUpdate(float diff)
        {
            if (Owner != null && thisBuff != null && originSpell != null)
            {
                DamageManaTimer += diff;

                if (DamageManaTimer >= 500f)
                {
                    if (manaCost[originSpell.CastInfo.SpellLevel - 1] > Owner.Stats.CurrentMana)
                    {
                        RemoveBuff(thisBuff);
                    }
                    else
                    {
                        Owner.Stats.CurrentMana -= manaCost[originSpell.CastInfo.SpellLevel - 1];
                    }

                    DamageManaTimer = 0;
                }
            }
        }
    }
}