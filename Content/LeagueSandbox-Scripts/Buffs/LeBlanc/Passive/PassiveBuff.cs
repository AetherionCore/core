using GameServerCore.Enums;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.Scripting.CSharp;
using System;
using LeagueSandbox.GameServer.GameObjects;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using LeagueSandbox.GameServer.GameObjects.SpellNS;
using LeagueSandbox.GameServer.GameObjects.StatsNS;
using LeagueSandbox.GameServer.API;
using System.Numerics;
using GameServerLib.GameObjects.AttackableUnits;

namespace Buffs
{
    internal class LeblancPassive : IBuffGameScript
    {
        public BuffScriptMetaData BuffMetaData { get; set; } = new BuffScriptMetaData
        {
            BuffType = BuffType.COMBAT_ENCHANCER,
            BuffAddType = BuffAddType.REPLACE_EXISTING
        };

        public StatsModifier StatsModifier { get; private set; }

        Minion Leblanc;
        Spell Spell;
        private Buff buff;

        public void OnActivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {
            Spell = ownerSpell;
            if (ownerSpell.CastInfo.Owner is Champion owner)
            {
                ApiEventManager.OnTakeDamage.AddListener(this, owner, OnTakeDamage, false);
            }

        }
        public void OnTakeDamage(DamageData damageData)
        {
            if (Spell.CastInfo.Owner is Champion owner)
            {
                var currentHealth = owner.Stats.CurrentHealth;
                var limitHealth = owner.Stats.HealthPoints.Total * 0.4;
                if (limitHealth >= currentHealth)
                {
                    if (owner.HasBuff("LeblancPassive"))
                    {
                        owner.RemoveBuffsWithName("LeblancPassive");
                    }
                }
            }
        }
        public void OnDeactivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {
            if (Spell.CastInfo.Owner is Champion owner)
            {
                var ownerSkinID = owner.SkinID;
                var Cursor = new Vector2(ownerSpell.CastInfo.TargetPosition.X, ownerSpell.CastInfo.TargetPosition.Z);
                var current = new Vector2(owner.Position.X, owner.Position.Y);
                var distance = Cursor - current;
                Vector2 truecoords;
                if (distance.Length() > 25000f)
                {
                    distance = Vector2.Normalize(distance);
                    var range = distance * 25000f;
                    truecoords = current + range;
                }
                else
                {
                    truecoords = Cursor;
                }
                AddParticleTarget(owner, owner, "LeBlanc_Base_P_poof", owner, 10f);
                AddParticleTarget(owner, owner, "", owner, buff.Duration);
                Minion Leblanc = AddMinion(owner, "Leblanc", "Leblanc", owner.Position, owner.Team, owner.SkinID, false, true);
                AddBuff("", 25000f, 1, ownerSpell, Leblanc, owner, false);
                AddBuff("", 1f, 1, ownerSpell, owner, owner, false);
                AddBuff("", 1f, 1, ownerSpell, Leblanc, owner, false);
                CreateTimer(0.5f, () =>
                {
                    ForceMovement(Leblanc, "RUN", Leblanc.Owner.Position, 400, 0, 0, 0);
                });
                CreateTimer(1f, () =>
                {
                    ForceMovement(Leblanc, "RUN", Leblanc.Owner.Position, 400, 0, 0, 0);
                });
                CreateTimer(1.5f, () =>
                {
                    ForceMovement(Leblanc, "RUN", Leblanc.Owner.Position, 400, 0, 0, 0);
                });
                CreateTimer(2.5f, () =>
                {
                    ForceMovement(Leblanc, "RUN", Leblanc.Owner.Position, 400, 0, 0, 0);
                });
                AddParticleTarget(owner, Leblanc, "LeBlanc_Base_P_poof", owner);
                AddParticleTarget(owner, Leblanc, "LeBlanc_Base_P_image", owner, buff.Duration);
                CreateTimer(8f, () =>
                {
                    Leblanc.TakeDamage(Leblanc.Owner, 10000f, DamageType.DAMAGE_TYPE_TRUE, DamageSource.DAMAGE_SOURCE_INTERNALRAW, DamageResultType.RESULT_NORMAL);
                    AddParticleTarget(owner, Leblanc, "LeBlanc_Base_P_imageDeath", owner, buff.Duration);
                });
                if (Leblanc.IsDead)
                {
                    AddParticle(owner, null, "LeBlanc_Base_P_imageDeath.troy", Leblanc.Position);
                    AddParticleTarget(owner, Leblanc, "LeBlanc_Base_P_imageDeath", owner, buff.Duration);
                }
                AddParticleTarget(owner, Leblanc, "", owner, 25000f);
                AddBuff("LeblancPassiveCooldown", 3f, 1, ownerSpell, owner, owner, false);
            }
        }
        public void OnUpdate(float diff)
        {
        }
    }
}