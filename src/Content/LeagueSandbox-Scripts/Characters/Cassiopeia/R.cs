using GameServerCore.Enums;
using LeagueSandbox.GameServer.Scripting.CSharp;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using LeagueSandbox.GameServer.GameObjects.SpellNS;
using LeagueSandbox.GameServer.GameObjects.SpellNS.Missile;
using LeagueSandbox.GameServer.API;
using LeagueSandbox.GameServer.GameObjects.SpellNS.Sector;
using System.Numerics;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;

namespace Spells
{
    public class CassiopeiaPetrifyingGaze : ISpellScript
    {
        public SpellScriptMetadata ScriptMetadata => new SpellScriptMetadata()
        {
            TriggersSpellCasts = true,
            IsDamagingSpell = true,
            NotSingleTargetSpell = true
        };
        public void OnActivate(ObjAIBase owner, Spell spell)
        {
            ApiEventManager.OnSpellHit.AddListener(this, spell, TargetExecute, false);
        }
        public void OnSpellPostCast(Spell spell)
        {
            var owner = spell.CastInfo.Owner;

            var spellPos = new Vector2(spell.CastInfo.TargetPosition.X, spell.CastInfo.TargetPosition.Z);
            FaceDirection(spellPos, owner, false);

            spell.CreateSpellSector(new SectorParameters
            {
                Length = 750f,
                SingleTick = true,
                ConeAngle = 40f,
                Type = SectorType.Cone
            });
        }
        public void TargetExecute(Spell spell, AttackableUnit target, SpellMissile missile, SpellSector sector)
        {
            var hitFacing = false;
            var point = GetPointFromUnit(target, 825);
            var champs = GetChampionsInRange(point, 825, true);

            var owner = spell.CastInfo.Owner;
            var ap = owner.Stats.AbilityPower.Total * 0.5f;
            var damage = 50 + spell.CastInfo.SpellLevel * 100 + ap;

            foreach (var champion in champs)
            {
                if (champion.NetId == spell.CastInfo.Owner.NetId)
                { 
                    AddParticleTarget(owner, target, "Cassiopeia_Base_R_tar.troy", target);
                    AddParticleTarget(owner, target, "CassDeadlyCadence_buf.troy", target, lifetime: 2f, bone: "C_BUFFBONE_GLB_HEAD_LOC");
                    AddParticleTarget(owner, target, "CassDeathDust.troy", target, lifetime: 2f, bone: "root");
                    AddParticleTarget(owner, target, "Cassiopeia_Base_R_PetrifyMiss_tar.troy", target, lifetime: 2f, bone: "C_BUFFBONE_GLB_HEAD_LOC");
                    target.TakeDamage(owner, damage, DamageType.DAMAGE_TYPE_MAGICAL, DamageSource.DAMAGE_SOURCE_SPELL, false);
                    AddBuff("Stun", 2f, 1, spell, target, owner);
                    hitFacing = true;
                }
            }
            if (hitFacing == false)
            {
                AddParticleTarget(owner, target, "Cassiopeia_Base_R_tar.troy", target);
                target.TakeDamage(owner, damage, DamageType.DAMAGE_TYPE_MAGICAL, DamageSource.DAMAGE_SOURCE_SPELL, false);
                AddBuff("CassiopeiaSlow", 2f, 1, spell, target, owner);
            }
        }
    }
}