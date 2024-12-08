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
    public class ZiggsE : ISpellScript
    {
        public SpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
        {
            AutoFaceDirection = false,
            TriggersSpellCasts = true
            // TODO
        };

        public void OnSpellPostCast(Spell spell)
        {
            var owner = spell.CastInfo.Owner as Champion;
            var spellPos = new Vector2(spell.CastInfo.TargetPosition.X, spell.CastInfo.TargetPosition.Z);
            SpellCast(owner, 2, SpellSlotType.ExtraSlots, spellPos, spellPos, true, Vector2.Zero);
        }
    }

    public class ZiggsE2 : ISpellScript
    {
        public SpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
        {
            // TODO
        };

        Spell S;

        public void OnSpellPreCast(ObjAIBase owner, Spell spell, AttackableUnit target, Vector2 start, Vector2 end)
        {
            S = spell;
            var missile = spell.CreateSpellMissile(new MissileParameters
            {
                Type = MissileType.Circle,
                OverrideEndPosition = end
            });

            ApiEventManager.OnSpellMissileEnd.AddListener(this, missile, OnMissileEnd, true);
        }

        public void OnMissileEnd(SpellMissile missile)
        {
            var owner = missile.CastInfo.Owner;
            for (int t = 0; t <= 7; t++)
            {
                var w = GetPointFromUnit(missile, 200f, t * 45f);
                SpellCast(owner, 3, SpellSlotType.ExtraSlots, w, Vector2.Zero, true, missile.Position);
            }
            for (int t = 0; t <= 2; t++)
            {
                var n = GetPointFromUnit(missile, 60f, t * 120f);
                SpellCast(owner, 3, SpellSlotType.ExtraSlots, n, Vector2.Zero, true, missile.Position);
            }
            AddParticle(owner, null, "ZiggsE_mis_groundhit.troy", missile.Position, 10f);
            //T = AddMinion(owner, "TestCube", "TestCube", missile.Position, owner.Team, owner.SkinID, true, false);
            //AddBuff("ZiggsE", 5f, 1, S, T, owner);
        }
    }

    public class ZiggsE3 : ISpellScript
    {
        public SpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
        {
            // TODO
        };

        Spell S;
        Minion T;

        public void OnSpellPreCast(ObjAIBase owner, Spell spell, AttackableUnit target, Vector2 start, Vector2 end)
        {
            S = spell;
            var missile = spell.CreateSpellMissile(new MissileParameters
            {
                Type = MissileType.Circle,
                OverrideEndPosition = end
            });

            ApiEventManager.OnSpellMissileEnd.AddListener(this, missile, OnMissileEnd, true);
        }

        public void OnMissileEnd(SpellMissile missile)
        {
            var owner = missile.CastInfo.Owner;
            T = AddMinion(owner, "TestCubeRender", "TestCube", missile.Position, owner.Team, owner.SkinID, true, false);
            AddBuff("ZiggsE", 10f, 1, S, T, owner);
        }
    }
}