using System;
using GameServerCore.Enums;
using LeagueSandbox.GameServer.Scripting.CSharp;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using LeagueSandbox.GameServer.GameObjects.SpellNS;
using System.Numerics;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;

namespace Spells
{
    public class BurningAgony : ISpellScript
    {
        private ObjAIBase Owner;
        private float timeSinceLastTick = 1000f;
        private Spell Spell;

        public SpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
        {
            TriggersSpellCasts = true,
        };

        public void OnActivate(ObjAIBase owner, Spell spell)
        {
            Owner = owner;
            Spell = spell;
        }

        public void OnDeactivate(ObjAIBase owner, Spell spell)
        {
        }

        public void OnSpellPreCast(ObjAIBase owner, Spell spell, AttackableUnit target, Vector2 start, Vector2 end)
        {
        }

        public void OnSpellCast(Spell spell)
        {
            if (!Owner.HasBuff("BurningAgony"))
            {
                AddBuff("BurningAgony", 1, 1, spell, Owner, Owner, true);
            }
            else
            {
                RemoveBuff(Owner, "BurningAgony");
            }
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
            timeSinceLastTick += diff;
            if (timeSinceLastTick >= 1000.0f && Owner != null && Spell != null && Owner.HasBuff("BurningAgony"))
            {
                float selfDMG = 5f + (5f * Spell.CastInfo.SpellLevel);
                if (Owner.Stats.CurrentHealth > selfDMG)
                {
                    Owner.Stats.CurrentHealth -= selfDMG;
                }
                else
                {
                    Owner.Stats.CurrentHealth = 1f;
                }
                timeSinceLastTick = 0f;
            }
        }
    }
}