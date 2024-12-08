using GameServerCore.Enums;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.API;
using LeagueSandbox.GameServer.Scripting.CSharp;
using System.Numerics;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using LeagueSandbox.GameServer.GameObjects.SpellNS.Missile;
using LeagueSandbox.GameServer.GameObjects.SpellNS.Sector;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.GameObjects.SpellNS;
using static LeaguePackets.Game.Common.CastInfo;

namespace Spells
{
    public class KatarinaQ : ISpellScript
    {
        Spell QMis;
        float Damage;
        float MarkDamage;
        ObjAIBase Katarina;
        SpellMissile Missile;
        AttackableUnit Target;
        public SpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
        {
            TriggersSpellCasts = true,
            IsDamagingSpell = true
        };
        public void OnSpellPostCast(Spell spell)
        {
            QMis = spell;
            Katarina = spell.CastInfo.Owner as Champion;
            Missile = spell.CreateSpellMissile(new MissileParameters { Type = MissileType.Chained, MaximumHits=4 });
            ApiEventManager.OnSpellMissileHit.AddListener(this, Missile, TargetExecute, false);
        }
        public void TargetExecute(SpellMissile missile, AttackableUnit target)
        {
            if (Target == null)
                Target = target;

            Damage = 45f + (QMis.CastInfo.SpellLevel * 35f) + (Katarina.Stats.AbilityPower.Total * 0.5f);
            target.TakeDamage(Katarina, Damage, DamageType.DAMAGE_TYPE_MAGICAL, DamageSource.DAMAGE_SOURCE_SPELL, false);
            //AddParticleTarget(owner, target, "katarina_bouncingBlades_tar.troy", target);

            var xx = GetClosestUnitsInRange(target, 300, true);
            var targetsHit = 0;
            foreach (var unit in xx)
            {
                if (unit.NetId != Katarina.NetId && !unit.IsDead && unit.Team != Katarina.Team
                    && unit is not BaseTurret && unit.NetId != target.NetId 
                    && !unit.HasBuff("KatarinaQMark"))
                {
                    LogInfo($"Hitting Second Target {unit.CharData.Name} - {unit.NetId}");
                    SpellCast(Katarina, 2, SpellSlotType.ExtraSlots, false, unit, target.Position);
                    break;
                    Damage = Damage * 0.9f;
                    unit.TakeDamage(Katarina, Damage, DamageType.DAMAGE_TYPE_MAGICAL, DamageSource.DAMAGE_SOURCE_SPELL, false);
                    AddBuff("KatarinaQMark", 4f, 1, QMis, unit, Katarina, false);
                    targetsHit++;
                }
                if (targetsHit >= 4)
                    break;
            }

            AddBuff("KatarinaQMark", 4f, 1, QMis, target, Katarina, false);
        }

    }
    public class KatarinaQMis : ISpellScript
    {
        Spell QMis;
        float Damage;
        float MarkDamage;
        ObjAIBase Katarina;
        SpellMissile Missile;
        AttackableUnit Target;
        public SpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
        {
            DoesntBreakShields = true,
            TriggersSpellCasts = true,
            IsDamagingSpell = true,
            NotSingleTargetSpell = false,
            PersistsThroughDeath = true,
            SpellDamageRatio = 1.0f,
        };


        public void OnSpellPostCast(Spell spell)
        {
            QMis = spell;
            Katarina = spell.CastInfo.Owner as Champion;
            Missile = spell.CreateSpellMissile(new MissileParameters
            {
                Type = MissileType.Chained,
                MaximumHits = 4,
                CanHitSameTarget = false,
                CanHitSameTargetConsecutively = false
            });
            ApiEventManager.OnSpellMissileHit.AddListener(this, Missile, TargetExecute, false);
        }
        public void TargetExecute(SpellMissile missile, AttackableUnit target)
        {
            AddBuff("KatarinaQMark", 4f, 1, QMis, target, Katarina, false);
        }
    }
}