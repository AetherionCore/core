using GameServerCore.Enums;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using LeagueSandbox.GameServer.Scripting.CSharp;
using System.Numerics;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.API;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using LeagueSandbox.GameServer.GameObjects.SpellNS;
using LeagueSandbox.GameServer.GameObjects.SpellNS.Sector;
using LeagueSandbox.GameServer.GameObjects.SpellNS.Missile;
using LeagueSandbox.GameServer.GameObjects;

namespace Spells
{
    public class AzirR : ISpellScript
    {
        Spell spell;
        Minion Soldier1;
        Minion Soldier2;
        Minion Soldier3;
        Minion Soldier4;
        Minion Soldier5;
        Minion Soldier6;
        public SpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
        {
            TriggersSpellCasts = true
            // TODO
        };
        Minion a;
        public void OnActivate(ObjAIBase owner, Spell spell)
        {
        }

        public void OnDeactivate(ObjAIBase owner, Spell spell)
        {
        }

        public void OnSpellPreCast(ObjAIBase owner, Spell spell, AttackableUnit target, Vector2 start, Vector2 end)
        {
        }

        public void OnSpellCast(Spell spell)
        {
            var owner = spell.CastInfo.Owner;
            var current = new Vector2(owner.Position.X, owner.Position.Y);
            var spellPos = new Vector2(spell.CastInfo.TargetPosition.X, spell.CastInfo.TargetPosition.Z);
            FaceDirection(spellPos, owner, true);
            var CubeP = GetPointFromUnit(owner, -250f);
            var CubeP2 = GetPointFromUnit(owner, 450f);
            Particle Cube = AddParticle(owner, null, "", CubeP, lifetime: 1);
            Particle Cube2 = AddParticle(owner, null, "", CubeP2, lifetime: 1);
            FaceDirection(owner.Position, Cube, true);
            FaceDirection(owner.Position, Cube2, true);
            var Start1 = GetPointFromUnit(Cube, 50f, 90);
            var Start2 = GetPointFromUnit(Cube, 50f, -90);
            var Start3 = GetPointFromUnit(Cube, 150f, 90);
            var Start4 = GetPointFromUnit(Cube, 150f, -90);
            var Start5 = GetPointFromUnit(Cube, 250f, 90);
            var Start6 = GetPointFromUnit(Cube, 250f, -90);
            var End1 = GetPointFromUnit(Cube2, 50f, 90);
            var End2 = GetPointFromUnit(Cube2, 50f, -90);
            var End3 = GetPointFromUnit(Cube2, 150f, 90);
            var End4 = GetPointFromUnit(Cube2, 150f, -90);
            var End5 = GetPointFromUnit(Cube2, 250f, 90);
            var End6 = GetPointFromUnit(Cube2, 250f, -90);
            Soldier1 = AddMinion(owner, "AzirUltSoldier", "AzirUltSoldier", Start1, owner.Team, owner.SkinID, true, false);
            Soldier2 = AddMinion(owner, "AzirUltSoldier", "AzirUltSoldier", Start2, owner.Team, owner.SkinID, true, false);
            Soldier3 = AddMinion(owner, "AzirUltSoldier", "AzirUltSoldier", Start3, owner.Team, owner.SkinID, true, false);
            Soldier4 = AddMinion(owner, "AzirUltSoldier", "AzirUltSoldier", Start4, owner.Team, owner.SkinID, true, false);
            Soldier5 = AddMinion(owner, "AzirUltSoldier", "AzirUltSoldier", Start5, owner.Team, owner.SkinID, true, false);
            Soldier6 = AddMinion(owner, "AzirUltSoldier", "AzirUltSoldier", Start6, owner.Team, owner.SkinID, true, false);
            ForceMovement(Soldier1, null, End2, 1400, 0, 0, 0);
            ForceMovement(Soldier2, null, End1, 1400, 0, 0, 0);
            ForceMovement(Soldier3, null, End4, 1400, 0, 0, 0);
            ForceMovement(Soldier4, null, End3, 1400, 0, 0, 0);
            ForceMovement(Soldier5, null, End6, 1400, 0, 0, 0);
            ForceMovement(Soldier6, null, End5, 1400, 0, 0, 0);
            AddBuff("AzirR", 6f, 1, spell, Soldier1, owner); AddBuff("AzirR", 6f, 1, spell, Soldier2, owner); AddBuff("AzirR", 6f, 1, spell, Soldier3, owner);
            AddBuff("AzirR", 6f, 1, spell, Soldier4, owner); AddBuff("AzirR", 6f, 1, spell, Soldier5, owner); AddBuff("AzirR", 6f, 1, spell, Soldier6, owner);
            AddParticleTarget(owner, owner, ".troy", owner, 0.5f);
            AddParticle(owner, null, ".troy", owner.Position);
        }

        public void OnSpellPostCast(Spell spell)
        {
        }
        public void TargetExecute(Spell spell, AttackableUnit target, SpellMissile missile, SpellSector sector)
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