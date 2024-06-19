using GameServerCore.Enums;
using LeagueSandbox.GameServer.Scripting.CSharp;
using LeagueSandbox.GameServer.API;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using LeagueSandbox.GameServer.GameObjects.SpellNS;
using System.Numerics;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using GameServerLib.GameObjects.AttackableUnits;
using GameServerCore;
using LeagueSandbox.GameServer.GameObjects.SpellNS.Missile;
using LeagueSandbox.GameServer.GameObjects.SpellNS.Sector;

namespace Spells
{
    public class KatarinaE : ISpellScript
    {
        private AttackableUnit Target;

        public SpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
        {
            TriggersSpellCasts = true,
            IsDamagingSpell = true
        };

        public void OnActivate(ObjAIBase owner, Spell spell)
        {
        }

        public void OnDeactivate(ObjAIBase owner, Spell spell)
        {
        }

        public void OnSpellPreCast(ObjAIBase owner, Spell spell, AttackableUnit target, Vector2 start, Vector2 end)
        {
            Target = target;
            PlayAnimation(owner, "Spell2");
            if (target.Team != owner.Team)
            {
                float AP = owner.Stats.AbilityPower.Total * 0.4f;
                float damage = 45f + 25 * spell.CastInfo.SpellLevel + AP;
                var MarkAP = spell.CastInfo.Owner.Stats.AbilityPower.Total * 0.15f;
                float MarkDamage = 15f * (owner.GetSpell("KatarinaQ").CastInfo.SpellLevel) + MarkAP;

                if (target.HasBuff("KatarinaQMark"))
                {
                    target.TakeDamage(owner, MarkDamage, DamageType.DAMAGE_TYPE_MAGICAL, DamageSource.DAMAGE_SOURCE_PROC, false);
                    RemoveBuff(target, "KatarinaQMark");
                }

                Target.TakeDamage(owner, damage, DamageType.DAMAGE_TYPE_MAGICAL, DamageSource.DAMAGE_SOURCE_SPELL, false);
                AddParticleTarget(owner, null, "katarina_shadowStep_tar.troy", Target);
            }
            AddParticleTarget(owner, null, "katarina_shadowStep_cas.troy", owner);

            ForceMovement(owner, "Spell2", Vector2.Zero, 20, 20, 0.3f, 20);
            TeleportTo(owner, Target.Position.X, Target.Position.Y);
            AddBuff("KatarinaEReduction", 1.5f, 1, spell, owner, owner);
            PlayAnimation(owner, "Spell3", 1f);
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