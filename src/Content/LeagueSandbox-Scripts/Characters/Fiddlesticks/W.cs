using System.Numerics;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using GameServerCore.Enums;
using LeagueSandbox.GameServer.Scripting.CSharp;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using LeagueSandbox.GameServer.GameObjects.SpellNS;
using LeagueSandbox.GameServer.GameObjects.SpellNS.Missile;
using LeagueSandbox.GameServer.GameObjects;

namespace Spells
{
    public class Drain : ISpellScript
    {
        public SpellScriptMetadata ScriptMetadata => new SpellScriptMetadata()
        {
        };

        public void OnActivate(ObjAIBase owner, Spell spell)
        {
        }

        public void OnDeactivate(ObjAIBase owner, Spell spell)
        {
        }

        private Vector2 basepos;
        private bool cancelled;
        private Particle p;

        public void OnSpellPreCast(ObjAIBase owner, Spell spell, AttackableUnit target, Vector2 start, Vector2 end)
        {
            basepos = owner.Position;
            PlayAnimation(owner, "Spell2", startTime: 0.5f);
            p = AddParticleTarget(owner, target, "Drain.troy", owner, lifetime: 5.0f);
            for (var i = 0.0f; i < 5.0; i += 1f)
            {
                CreateTimer(i, () => { ApplyDrainDamage(owner, spell, target); });
            }
            CreateTimer(5.1f, () => { cancelled = false; });
        }

        private void ApplyDrainDamage(ObjAIBase owner, Spell spell, AttackableUnit target)
        {
            if (cancelled == true)
            {
                RemoveParticle(p);
            }
            if (owner.Position.X != basepos.X)
            {
                cancelled = true;
            }
            if (owner.Position.Y != basepos.Y)
            {
                cancelled = true;
            }
            if (target.Team != owner.Team)
            {
                var damage = 60;
                var ap = owner.Stats.AbilityPower.Total * 0.45f;
                if (!cancelled)
                {
                    target.TakeDamage(owner, ap + damage, DamageType.DAMAGE_TYPE_MAGICAL, DamageSource.DAMAGE_SOURCE_SPELL, false);
                }
            }
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

        public void OnSpellChannelCancel(Spell spell, ChannelingStopSource source)
        {
        }

        public void OnSpellPostChannel(Spell spell)
        {
        }

        public void OnUpdate(float diff)
        {
            if (cancelled == true)
            {
                p.SetToRemove();
            }
        }
    }
}