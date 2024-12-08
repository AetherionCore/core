using System.Numerics;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using LeagueSandbox.GameServer.Scripting.CSharp;
using GameServerCore.Scripting.CSharp;
using GameServerCore.Enums;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.GameObjects.SpellNS;
using LeagueSandbox.GameServer.GameObjects.SpellNS.Missile;
using LeagueSandbox.GameServer.GameObjects.SpellNS.Sector;
using LeagueSandbox.GameServer.API;
using System.Collections.Generic;
using LeagueSandbox.GameServer.GameObjects;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.Buildings;
using GameServerLib.GameObjects.AttackableUnits;
using LeaguePackets.Game.Common;
using LeagueSandbox.GameServer.GameObjects.StatsNS;

namespace Spells
{
    public class XerathLocusOfPower2 : ISpellScript
    {
        public static AttackableUnit Target = null;
        Spell Spell;
        ObjAIBase Owner;

        public SpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
        {
            ChannelDuration = 8f,
            TriggersSpellCasts = true,
        };

        public void OnSpellPreCast(ObjAIBase owner, Spell spell, AttackableUnit target, Vector2 start, Vector2 end)
        {
            Spell = spell;
            Owner = owner;
            Target = target;
            owner.CancelAutoAttack(false, true);
            owner.SetTargetUnit(null, true);
        }

        public void OnSpellCast(Spell spell)
        {
            var owner = spell.CastInfo.Owner;
        }

        public void OnSpellPostCast(Spell spell)
        {
            var owner = spell.CastInfo.Owner;
            spell.SetCooldown(0.5f, true);
        }

        public void OnSpellChannel(Spell spell)
        {
            var owner = spell.CastInfo.Owner;
            owner.SetSpell("XerathLocusPulse", 3, true);
        }

        public void OnSpellChannelCancel(Spell spell, ChannelingStopSource reason)
        {
            var owner = spell.CastInfo.Owner;
            owner.SetSpell("XerathLocusOfPower2", 3, true);
        }
    }

    public class XerathLocusPulse : ISpellScript
    {
        public SpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
        {
            TriggersSpellCasts = true
        };
    }
}