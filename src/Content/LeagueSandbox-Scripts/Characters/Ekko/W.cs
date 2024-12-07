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
using LeaguePackets.Game.Events;

namespace Spells
{
    public class EkkoW : ISpellScript
    {
        Spell spell;
        Particle P;
        Vector2 truecoords;
        public SpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
        {
            AutoFaceDirection = false,
            CastingBreaksStealth = true,
            DoesntBreakShields = true,
            TriggersSpellCasts = true,
            IsDamagingSpell = false,
            NotSingleTargetSpell = true
        };

        public void OnSpellCast(Spell spell)
        {
            var owner = spell.CastInfo.Owner;
            var Cursor = new Vector2(spell.CastInfo.TargetPosition.X, spell.CastInfo.TargetPosition.Z);
            var current = new Vector2(owner.Position.X, owner.Position.Y);
            var distance = Cursor - current;
            if (distance.Length() > 1600)
            {
                distance = Vector2.Normalize(distance);
                var range = distance * 1600;
                truecoords = current + range;
            }
            else
            {
                truecoords = Cursor;
            }
            AddParticle(owner, null, "Ekko_Base_W_Cas.troy", truecoords);
            CreateTimer((float)3f, () => { AOE(spell); });
            //AddBuff("LeblancSlideReturn", 4.0f, 1, spell, owner, owner);		
        }

        public void OnSpellPostCast(Spell spell)
        {
            var owner = spell.CastInfo.Owner;
            var targetPos = GetPointFromUnit(owner, -125f);
            CreateTimer((float)1.75f, () => { SpellCast(owner, 4, SpellSlotType.ExtraSlots, truecoords, Vector2.Zero, true, targetPos); });
            AddParticle(owner, null, "Ekko_Base_W_Indicator.troy", truecoords, 10);
            AddParticleTarget(owner, owner, "Ekko_Base_W_Branch_Timeline.troy", owner, 10);

        }
        public void AOE(Spell spell)
        {
            if (spell.CastInfo.Owner is Champion c)
            {
                Minion W = AddMinion(c, "TestCube", "TestCube", truecoords, c.Team, c.SkinID, true, false);
                AddBuff("EkkoW", 2f, 1, spell, W, c, false);
            }
        }
    }

    public class EkkoWMis : ISpellScript
    {
        public SpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
        {
            MissileParameters = new MissileParameters
            {
                Type = MissileType.Circle
            },
            IsDamagingSpell = true
            // TODO
        };

        //Vector2 direction;
        Spell Spell;

        public void OnSpellPreCast(ObjAIBase owner, Spell spell, AttackableUnit target, Vector2 start, Vector2 end)
        {
            Spell = spell;
            var missile = spell.CreateSpellMissile(new MissileParameters
            {
                Type = MissileType.Circle,
                OverrideEndPosition = end
            });
        }
    }
}