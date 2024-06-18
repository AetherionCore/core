using System.Numerics;
using GameServerCore.Enums;
using LeagueSandbox.GameServer.Scripting.CSharp;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using LeagueSandbox.GameServer.GameObjects.SpellNS;
using LeagueSandbox.GameServer.GameObjects;
using GameServerCore.Scripting.CSharp;

namespace Spells
{
    public class EvelynnPassive : ISpellScript
    {
        public SpellScriptMetadata ScriptMetadata => new SpellScriptMetadata()
        {
           
        };

        Buff _revealedDebuff;
        Buff _shadowWalk;
        Buff _hateSpikeMarker;
        public void OnActivate(ObjAIBase owner, Spell spell)
        {
            //_shadowWalk = AddBuff("ShadowWalk", 23f, 1, spell, owner, owner, false);
            //_hateSpikeMarker = AddBuff("EvelynnHateSpikeMarker", 0f, 1, spell, owner, owner, true);
        }
        public void OnDeactivate(ObjAIBase owner, Spell spell)
        {
            //_revealedDebuff = AddBuff("ShadowWalkRevealedDebuff", 5f, 1, spell, owner, owner, false);
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
}

