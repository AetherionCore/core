using GameServerCore.Enums;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using LeagueSandbox.GameServer.Scripting.CSharp;
using System.Numerics;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using LeagueSandbox.GameServer.GameObjects.SpellNS;
using LeagueSandbox.GameServer.GameObjects.SpellNS.Missile;
using LeagueSandbox.GameServer.API;
using LeagueSandbox.GameServer.GameObjects.SpellNS.Sector;
using LeagueSandbox.GameServer.GameObjects;

namespace Spells
{
    public class RiftWalk : ISpellScript
    {
        
        public SpellScriptMetadata ScriptMetadata => new SpellScriptMetadata()
        {
            CastingBreaksStealth = true,
            DoesntBreakShields = true,
            TriggersSpellCasts = true,
            IsDamagingSpell = true,
            NotSingleTargetSpell = true
        };

        Buff Buff;

        public void OnActivate(ObjAIBase owner, Spell spell)
        {
            ApiEventManager.OnSpellHit.AddListener(this, spell, TargetExecute, false);
        }

        public void OnSpellPostCast(Spell spell)
        {
            var owner = spell.CastInfo.Owner;
            var trueCoords = new Vector2(spell.CastInfo.TargetPosition.X, spell.CastInfo.TargetPosition.Z);
            var startPos = owner.Position;

            var to = trueCoords - startPos;
            if (to.Length() > 700f)
            {
                trueCoords = GetPointFromUnit(owner, 475f);
            }
            PlayAnimation(owner, "Spell3", 0, 0, 1);
            AddBuff("RiftWalk", 20.0f, 1, spell, owner, owner);
            TeleportTo(owner, trueCoords.X, trueCoords.Y);
            AddParticle(owner, null, "Kassadin_Base_R_appear.troy", owner.Position);

            var AOEdmg = spell.CreateSpellSector(new SectorParameters
            {
                Length = 250f,
                SingleTick = true,
                OverrideFlags = SpellDataFlags.AffectEnemies | SpellDataFlags.AffectNeutral | SpellDataFlags.AffectMinions | SpellDataFlags.AffectHeroes,
                Type = SectorType.Area
            });
        }

        public void TargetExecute(Spell spell, AttackableUnit target, SpellMissile missile, SpellSector sector)
        {
            var buff = spell.CastInfo.Owner.GetBuffWithName("RiftWalk");
            float MANA = spell.CastInfo.Owner.Stats.ManaPoints.Total * 0.02f + (0.01f * buff.StackCount);
            float damage = 60f + 20f * spell.CastInfo.SpellLevel + MANA + (30f * spell.CastInfo.SpellLevel) * buff.StackCount;
            //TODO: Find a way to increase damage and ManaCost based on stacks

            target.TakeDamage(spell.CastInfo.Owner, damage, DamageType.DAMAGE_TYPE_MAGICAL, DamageSource.DAMAGE_SOURCE_SPELLAOE, false);
        }
    }
}