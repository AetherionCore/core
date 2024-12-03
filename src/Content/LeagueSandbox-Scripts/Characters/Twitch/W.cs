using GameServerCore.Enums;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using LeagueSandbox.GameServer.Scripting.CSharp;
using System.Numerics;
using LeagueSandbox.GameServer.API;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using LeagueSandbox.GameServer.GameObjects.SpellNS;
using LeagueSandbox.GameServer.GameObjects.SpellNS.Missile;
using LeagueSandbox.GameServer.GameObjects.SpellNS.Sector;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.Buildings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace Spells
{
    public class TwitchVenomCask : ISpellScript
    {
        ObjAIBase Twitch;
        Vector2 Truecoords;
        public SpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
        {
            TriggersSpellCasts = true,
            IsDamagingSpell = true
        };
        public void OnSpellPostCast(Spell spell)
        {
            Twitch = spell.CastInfo.Owner as Champion;
            var Cursor = new Vector2(spell.CastInfo.TargetPosition.X, spell.CastInfo.TargetPosition.Z);
            var current = new Vector2(Twitch.Position.X, Twitch.Position.Y);
            var distance = Cursor - current;
            if (distance.Length() > 1200)
            {
                distance = Vector2.Normalize(distance);
                var range = distance * 1200;
                Truecoords = current + range;
            }
            else
            {
                Truecoords = Cursor;
            }
            SpellCast(Twitch, 1, SpellSlotType.ExtraSlots, Truecoords, Truecoords, true, Vector2.Zero);
        }
    }
    public class TwitchVenomCaskMissile : ISpellScript
    {
        private Spell Cask;
        private ObjAIBase Twitch;
        private SpellMissile Missile;
        public SpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
        {
            IsDamagingSpell = true,
            TriggersSpellCasts = true
        };

        public void OnSpellPreCast(ObjAIBase owner, Spell spell, AttackableUnit target, Vector2 start, Vector2 end)
        {
            Cask = spell;
            Twitch = owner = spell.CastInfo.Owner as Champion;
            Missile = spell.CreateSpellMissile(new MissileParameters { Type = MissileType.Circle, OverrideEndPosition = end });
            ApiEventManager.OnSpellMissileEnd.AddListener(this, Missile, OnMissileEnd, true);
        }

        public void OnMissileEnd(SpellMissile missile)
        {
            Missile = missile;
            Minion W = AddMinion(Twitch, "TestCubeRender", "TestCubeRender", Missile.Position, Twitch.Team, Twitch.SkinID, true, false);
            AddBuff("TwitchVenomCask", 3f, 1, Cask, W, Twitch, false);
        }
    }
}