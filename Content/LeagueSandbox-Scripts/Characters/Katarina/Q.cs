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

namespace Spells
{
    public class KatarinaQ : ISpellScript
    {
        public SpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
        {
            MissileParameters = new MissileParameters
            {
                Type = MissileType.Chained,
                MaximumHits = 4,
                CanHitSameTarget = false,
                CanHitSameTargetConsecutively = false
            },
            IsDamagingSpell = true,
            TriggersSpellCasts = true
            // TODO
        };

        public void OnActivate(ObjAIBase owner, Spell spell)
        {
            ApiEventManager.OnSpellHit.AddListener(this, spell, TargetExecute, false);
        }

        public void TargetExecute(Spell spell, AttackableUnit target, SpellMissile missile, SpellSector sector)
        {
            var owner = spell.CastInfo.Owner as Champion;
            var ap = owner.Stats.AbilityPower.Total * 0.5f;
            var damage = 45f + spell.CastInfo.SpellLevel * 35f + ap;
            target.TakeDamage(owner, damage, DamageType.DAMAGE_TYPE_MAGICAL, DamageSource.DAMAGE_SOURCE_SPELL, false);
            //AddParticleTarget(owner, target, "katarina_bouncingBlades_tar.troy", target);
            AddBuff("KatarinaQMark", 4f, 1, spell, target, owner, false);
            var xx = GetClosestUnitInRange(target, 300, true);
            if (xx != owner && !xx.IsDead) SpellCast(owner, 2, SpellSlotType.ExtraSlots, true, xx, target.Position);
            if (missile is SpellChainMissile chainMissile && chainMissile.ObjectsHit.Count > 4) missile.SetToRemove();
        }

        public void OnDeactivate(ObjAIBase owner, Spell spell)
        {
        }

        public void OnSpellPreCast(ObjAIBase owner, Spell spell, AttackableUnit target, Vector2 start, Vector2 end)
        {
        }

        public void OnSpellCast(Spell spell)
        {
        }

        public void OnSpellPostCast(Spell spell)
        {
        }

        public void OnSpellChannel(Spell spell)
        {
        }

        public void OnSpellChannelCancel(Spell spell, ChannelingStopSource reason)
        {
        }

        public void OnSpellPostChannel(Spell spell)
        {
        }

        public void OnUpdate(float diff)
        {
        }
    }

    public class KatarinaQMis : ISpellScript
    {
        public SpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
        {
            DoesntBreakShields = true,
            TriggersSpellCasts = true,
            IsDamagingSpell = true,
            NotSingleTargetSpell = false,
            PersistsThroughDeath = true,
            SpellDamageRatio = 1.0f,
            MissileParameters = new MissileParameters
            {
                Type = MissileType.Target,
            }
        };

        private AttackableUnit firstTarget;

        public void OnActivate(ObjAIBase owner, Spell spell)
        {
        }

        public void OnDeactivate(ObjAIBase owner, Spell spell)
        {
        }

        public void OnSpellPreCast(ObjAIBase owner, Spell spell, AttackableUnit target, Vector2 start, Vector2 end)
        {
            firstTarget = target;
            ApiEventManager.OnSpellHit.AddListener(this, spell, TargetExecute, true);
        }

        private int bounce = 1;

        //CAN CRASH!!! CARE
        public void TargetExecute(Spell spell, AttackableUnit target, SpellMissile missile, SpellSector sector)
        {
            //if (firstTarget == target)
            //{
            //    return;
            //}
            AddBuff("KatarinaQMark", 4f, 1, spell, target, spell.CastInfo.Owner, false);
            /*var x = GetClosestUnitInRange(target, 600, true);
             if (x.IsDead == false)
             {
                 var owner = spell.CastInfo.Owner;
                 var ap = owner.Stats.AbilityPower.Total * 0.5f;
                 var damage = 45f + spell.CastInfo.SpellLevel * 35f + ap;
                 target.TakeDamage(owner, damage, DamageType.DAMAGE_TYPE_MAGICAL, DamageSource.DAMAGE_SOURCE_SPELL, false);
                 if (bounce != 3)
                 {
                     bounce++;
                     SpellCast(owner, 2, SpellSlotType.ExtraSlots, true, x, target.Position);
                     AddBuff("KatarinaQMark", 4f, 1, spell, target, owner, false);
                 }
                 else
                 {
                     bounce = 0;
                 }
             } */
        }

        public void OnSpellCast(Spell spell)
        {
        }

        public void OnSpellPostCast(Spell spell)
        {
        }

        public void OnSpellChannel(Spell spell)
        {
        }

        public void OnSpellChannelCancel(Spell spell, ChannelingStopSource reason)
        {
        }

        public void OnSpellPostChannel(Spell spell)
        {
        }

        public void OnUpdate(float diff)
        {
        }
    }
}