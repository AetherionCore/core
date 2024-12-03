using GameServerCore;
using LeagueSandbox.GameServer.Scripting.CSharp;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using GameServerCore.Enums;
using System.Numerics;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.Buildings.AnimatedBuildings;
using LeagueSandbox.GameServer.GameObjects.SpellNS;
using System;


namespace Spells
{
    public class Deceive : ISpellScript
    {
        Vector2 teleportTo;
        public SpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
        {
            TriggersSpellCasts = true
        };

        public void OnActivate(ObjAIBase owner, Spell spell)
        {
        }

        public void OnDeactivate(ObjAIBase owner, Spell spell)
        {
        }

        public void OnSpellPreCast(ObjAIBase owner, Spell spell, AttackableUnit target, Vector2 start, Vector2 end)
        {
            teleportTo = new Vector2(end.X, end.Y);
            float targetPosDistance = Math.Abs((float)Math.Sqrt(Math.Pow(owner.Position.X - teleportTo.X, 2f) + Math.Pow(owner.Position.Y - teleportTo.Y, 2f)));
            FaceDirection(teleportTo, owner);
            teleportTo = GetPointFromUnit(owner, Math.Min(targetPosDistance, 400f));
        }

        public void OnSpellCast(Spell spell)
        {
            var owner = spell.CastInfo.Owner;
            AddParticle(owner, null, "JackintheboxPoof2.troy", owner.Position, 2f);
        }

        public void OnSpellPostCast(Spell spell)
        {
            var owner = spell.CastInfo.Owner;
            TeleportTo(spell.CastInfo.Owner, teleportTo.X, teleportTo.Y);
            AddBuff("Deceive", 3.5f, 1, spell, owner, owner);
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