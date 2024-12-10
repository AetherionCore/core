using GameServerCore.Enums;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using LeagueSandbox.GameServer.Scripting.CSharp;
using System.Numerics;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.API;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using LeagueSandbox.GameServer.GameObjects.SpellNS;
using LeagueSandbox.GameServer.GameObjects.SpellNS.Missile;
using LeagueSandbox.GameServer.GameObjects.SpellNS.Sector;
using Buffs;

namespace Spells
{
    public class Incinerate : ISpellScript
    {
        public SpellScriptMetadata ScriptMetadata => new SpellScriptMetadata()
        {
            TriggersSpellCasts = true,
            IsDamagingSpell = true,
            NotSingleTargetSpell = true
            // TODO
        };

        bool isGoneStun;
        float stunDuration;
        public void OnActivate(ObjAIBase owner, Spell spell)
        {
            ApiEventManager.OnSpellHit.AddListener(this, spell, TargetExecute, false);
        }

        public void OnSpellPreCast(ObjAIBase owner, Spell spell, AttackableUnit target, Vector2 start, Vector2 end)
        {
            var pyromarker = owner.GetBuffWithName("Pyromania_Particle");
            if (pyromarker != null && pyromarker.BuffScript is Pyromania_Particle p)
            {
                stunDuration = p.StunDuration;
                RemoveBuff(pyromarker);
            }

            isGoneStun = pyromarker != null;
        }

        public void OnSpellPostCast(Spell spell)
        {
            var owner = spell.CastInfo.Owner;

            var spellPos = new Vector2(spell.CastInfo.TargetPosition.X, spell.CastInfo.TargetPosition.Z);
            FaceDirection(spellPos, owner, false);

            var sector = spell.CreateSpellSector(new SectorParameters
            {
                Length = 625f,
                SingleTick = true,
                ConeAngle = 24.76f,
                Type = SectorType.Cone
            });

            AddParticle(owner, null, "IIncinerate_buf", GetPointFromUnit(owner, 625f));
            AddParticleTarget(owner, owner, "Incinerate_cas", owner);

            AddBuff("Pyromania", 250000f, 1, spell, owner, owner);
            var buff = owner.GetBuffWithName("Pyromania");
            NotifyBuffStacks(buff);
        }

        public void TargetExecute(Spell spell, AttackableUnit target, SpellMissile missile, SpellSector sector)
        {
            var owner = spell.CastInfo.Owner;

            var ap = owner.Stats.AbilityPower.Total * 0.8f;
            var damage = 70 + (spell.CastInfo.SpellLevel * 45) + ap;

            target.TakeDamage(owner, damage, DamageType.DAMAGE_TYPE_MAGICAL, DamageSource.DAMAGE_SOURCE_SPELL, false);
            if (isGoneStun)
            {
                AddBuff("Stun", stunDuration, 1, spell, target, owner);
            }
        }
    }
}