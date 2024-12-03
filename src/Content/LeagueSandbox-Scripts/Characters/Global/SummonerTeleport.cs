using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using LeagueSandbox.GameServer.Scripting.CSharp;
using System.Numerics;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using LeagueSandbox.GameServer.GameObjects.SpellNS;
using GameServerCore.Enums;

namespace Spells
{
    public class SummonerTeleport : ISpellScript
    {
        private ObjAIBase Owner;
        private AttackableUnit Target;
        public SpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
        {
            CastingBreaksStealth = false,
            ChannelDuration = 4.0f,
            TriggersSpellCasts = true,
            NotSingleTargetSpell = false
        };

        public void OnSpellChannel(Spell spell)
        {
            Target = spell.CastInfo.Targets[0].Unit;
            Owner = spell.CastInfo.Owner as Champion;
            var p101 = AddParticleTarget(Owner, Owner, "Summoner_Teleport_purple.troy", Owner);
            var p102 = AddParticleTarget(Owner, Owner, "teleport.troy", Owner);
            var p103 = AddParticleTarget(Owner, Target, "Teleport_target.troy", Target);
            var p104 = AddParticleTarget(Owner, Target, "Scroll_Teleport.troy", Target);
        }

        public void OnSpellChannelCancel(Spell spell, ChannelingStopSource reason)
        {
        }

        public void OnSpellPostChannel(Spell spell)
        {
            Target = spell.CastInfo.Targets[0].Unit;
            Owner = spell.CastInfo.Owner as Champion;
            TeleportTo(Owner, Target.Position.X, Target.Position.Y);
            AddParticleTarget(Owner, Owner, "TeleportArrive", Owner, flags: 0);
            var p201 = AddParticleTarget(Owner, Owner, "Summoner_TeleportArrive_purple.troy", Owner);
            var p202 = AddParticleTarget(Owner, Owner, "teleportarrive.troy", Owner);
            var p203 = AddParticleTarget(Owner, Owner, "scroll_teleportarrive.troy", Owner);
        }
    }
}