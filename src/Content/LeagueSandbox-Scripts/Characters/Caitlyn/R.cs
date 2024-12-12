using GameServerCore.Enums;
using LeagueSandbox.GameServer.Scripting.CSharp;
using System.Numerics;
using LeagueSandbox.GameServer.API;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.GameObjects.SpellNS.Missile;
using LeagueSandbox.GameServer.GameObjects.SpellNS.Sector;
using LeagueSandbox.GameServer.GameObjects.SpellNS;

namespace Spells
{
    public class CaitlynAceintheHole : ISpellScript
    {
        ObjAIBase Owner;
        AttackableUnit Target;
        public SpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
        {
            ChannelDuration = 1.25f,
            TriggersSpellCasts = true
        };

        public void OnSpellPreCast(ObjAIBase owner, Spell spell, AttackableUnit target, Vector2 start, Vector2 end)
        {
            Owner = owner;
            Target = target as Champion;
        }

        public void OnSpellCast(Spell spell)
        {
            AddBuff("CaitlynAceintheHole", 1.5f, 1, spell, Target, Owner);
        }

        public void OnSpellPostChannel(Spell spell)
        {
            SpellCast(Owner, 0, SpellSlotType.ExtraSlots, true, Target, Vector2.Zero);
        }
    }

    public class CaitlynAceintheHoleMissile : ISpellScript
    {
        public SpellScriptMetadata ScriptMetadata => new SpellScriptMetadata()
        {
            TriggersSpellCasts = true,
            MissileParameters = new MissileParameters
            {
                Type = MissileType.Target
            }
        };

        public void OnActivate(ObjAIBase owner, Spell spell)
        {
            ApiEventManager.OnSpellHit.AddListener(this, spell, TargetExecute, false);
        }

        public void TargetExecute(Spell spell, AttackableUnit target, SpellMissile missile, SpellSector sector)
        {
            var owner = spell.CastInfo.Owner;
            var spellLevel = owner.GetSpell("CaitlynAceintheHole").CastInfo.SpellLevel;
            if (target != null && !target.IsDead)
            {
                var ADratio = owner.Stats.AttackDamage.FlatBonus * 2f;
                var damage = 25f + (225f * spellLevel) + ADratio;

                target.TakeDamage(owner, damage, DamageType.DAMAGE_TYPE_PHYSICAL, DamageSource.DAMAGE_SOURCE_SPELL, false);
                AddParticleTarget(owner, target, "caitlyn_ace_tar.troy", target, lifetime: 1f);
            }
            missile.SetToRemove();
        }
    }
}