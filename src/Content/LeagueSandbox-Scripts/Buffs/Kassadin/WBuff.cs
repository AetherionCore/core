using GameServerCore.Enums;
using LeagueSandbox.GameServer.GameObjects.StatsNS;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.Scripting.CSharp;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.GameObjects;
using LeagueSandbox.GameServer.GameObjects.SpellNS;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using System.Collections.Generic;
using LeagueSandbox.GameServer.API;
using GameServerLib.GameObjects.AttackableUnits;
using LeaguePackets.Game.Events;

namespace Buffs
{
    internal class NetherBlade : IBuffGameScript
    {
        public BuffScriptMetaData BuffMetaData { get; set; } = new BuffScriptMetaData
        {
            BuffType = BuffType.SLOW,
            BuffAddType = BuffAddType.REPLACE_EXISTING
        };

        public StatsModifier StatsModifier { get; private set; } = new StatsModifier();

        Buff thisBuff;
        ObjAIBase Unit;
        Particle p;
        Particle p2;
        public void OnActivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {
            thisBuff = buff;
            if (unit is ObjAIBase ai)
            {
                Unit = ai;

                ApiEventManager.OnHitUnit.AddListener(this, ai, TargetExecute, true);
                p = AddParticleTarget(ownerSpell.CastInfo.Owner, null, "Kassadin_Base_W_buf.troy", unit, buff.Duration, 1, "R_hand", "R_hand");
                p2 = AddParticleTarget(ownerSpell.CastInfo.Owner, null, "Kassadin_Netherblade.troy", unit, buff.Duration, 1, "R_hand", "R_hand");
                ai.SkipNextAutoAttack();
            }

            StatsModifier.Range.FlatBonus += 50;
            unit.AddStatModifier(StatsModifier);
        }

        public void OnDeactivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {
            //ApiEventManager.OnHitUnit.RemoveListener(this);

            RemoveParticle(p);
            RemoveParticle(p2);
        }
        public void TargetExecute(DamageData damageData)
        {
            if (!thisBuff.Elapsed() && thisBuff != null && Unit != null)
            {
                float ap = Unit.Stats.AbilityPower.Total * 0.6f;
                float damage = 15 + 25 * Unit.GetSpell("NullLance").CastInfo.SpellLevel + ap;
                float manaHeal = (Unit.Stats.ManaPoints.Total - Unit.Stats.CurrentMana) * 0.03f + 0.01f * Unit.GetSpell("NullLance").CastInfo.SpellLevel;
                if (damageData.Target is Champion)
                {
                    manaHeal *= 5;
                }
                damageData.Target.TakeDamage(Unit, damage, DamageType.DAMAGE_TYPE_MAGICAL, DamageSource.DAMAGE_SOURCE_ATTACK, false);
                Unit.Stats.CurrentMana += manaHeal;
                thisBuff.DeactivateBuff();
            }
        }
    }
}