using LeagueSandbox.GameServer.Scripting.CSharp;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using GameServerCore.Scripting.CSharp;
using GameServerCore.Enums;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using LeagueSandbox.GameServer.GameObjects.SpellNS;
using LeagueSandbox.GameServer.GameObjects;
using static LeaguePackets.Game.Common.CastInfo;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;
using System.Numerics;

namespace Spells
{
    public class Recall : ISpellScript
    {
        public SpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
        {
            CastingBreaksStealth = true,
            ChannelDuration = 8.0f,
            TriggersSpellCasts = false,
            NotSingleTargetSpell = true
        };

        Particle recallParticle;

        public void OnSpellPreCast(ObjAIBase owner, Spell spell, AttackableUnit target, Vector2 start, Vector2 end)
        {
            float recallTime = spell.CastInfo.Owner.HasBuff("ExaltedWithBaronNashor") ? 4.0f : 8f;
            ScriptMetadata.ChannelDuration = recallTime;
        }

        public void OnSpellChannel(Spell spell)
        {
            float recallTime = spell.CastInfo.Owner.HasBuff("ExaltedWithBaronNashor") ? 4.0f : 8f;
            ScriptMetadata.ChannelDuration = recallTime;
            LogInfo($"RecallTime: {recallTime}! - {spell.CastInfo.Owner.GetBuffNames()}");
            var owner = spell.CastInfo.Owner;
            recallParticle = AddParticleTarget(owner, owner, "TeleportHome", owner, recallTime, flags: 0);
            AddBuff("Recall", recallTime - 0.1f, 1, spell, owner, owner);
            owner.IconInfo.ChangeBorder("Recall", "recall");
        }

        public void OnSpellChannelCancel(Spell spell, ChannelingStopSource reason)
        {
            var owner = spell.CastInfo.Owner;
            recallParticle?.SetToRemove();
            RemoveBuff(owner, "Recall");
            owner.IconInfo.ResetBorder();
        }

        public void OnSpellPostChannel(Spell spell)
        {
            var owner = spell.CastInfo.Owner as Champion;
            owner.Recall();
            AddParticleTarget(owner, owner, "TeleportArrive", owner, flags: 0);
            if (owner.HasBuff("ExaltedWithBaronNashor"))
            {
                AddBuff("BaronNasahorSpeed", 8f, 1, spell, owner, owner);
                owner.TakeHeal(owner, owner.Stats.HealthPoints.Total*0.5f, spell);
            }
            
            owner.IconInfo.ResetBorder();
        }
    }


    public class RecallImproved : ISpellScript
    {
        public SpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
        {
            CastingBreaksStealth = true,
            ChannelDuration = 4.0f,
            TriggersSpellCasts = false,
            NotSingleTargetSpell = true
        };

        Particle recallParticle;

        public void OnSpellChannel(Spell spell)
        {
            var owner = spell.CastInfo.Owner;
            recallParticle = AddParticleTarget(owner, owner, "teleporthomeimproved", owner, 4f, flags: 0);
            AddBuff("Recall", 4 - 0.1f, 1, spell, owner, owner);
            owner.IconInfo.ChangeBorder("Recall", "recall");
        }

        public void OnSpellChannelCancel(Spell spell, ChannelingStopSource reason)
        {
            var owner = spell.CastInfo.Owner;
            recallParticle.SetToRemove();
            RemoveBuff(owner, "Recall");
            owner.IconInfo.ResetBorder();
        }

        public void OnSpellPostChannel(Spell spell)
        {
            var owner = spell.CastInfo.Owner as Champion;
            owner.Recall();
            AddParticleTarget(owner, owner, "TeleportArrive", owner, flags: 0);
            if (owner.HasBuff("ExaltedWithBaronNashor"))
            {
                AddBuff("BaronNasahorSpeed", 8f, 1, spell, owner, owner);
                owner.TakeHeal(owner, owner.Stats.HealthPoints.Total * 0.5f, spell);
            }

            owner.IconInfo.ResetBorder();
        }
    }
}
