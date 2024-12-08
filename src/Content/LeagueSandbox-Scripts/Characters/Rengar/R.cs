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
    public class RengarR : ISpellScript
    {
        public SpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
        {
            TriggersSpellCasts = true,
            NotSingleTargetSpell = true
            // TODO
        };

        public void OnSpellPreCast(ObjAIBase owner, Spell spell, AttackableUnit target, Vector2 start, Vector2 end)
        {
            AddBuff("RengarR", 7.0f, 1, spell, owner, owner);
            AddParticleTarget(owner, owner, "Rengar_Base_R_Cas.troy", owner);
        }
    }

    public class RengarPassiveBuffDash : ISpellScript
    {
        AttackableUnit Target;
        bool toRemove;
        ObjAIBase owner;
        Spell originSpell;
        public SpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
        {
            TriggersSpellCasts = true,
            NotSingleTargetSpell = true,
            IsDamagingSpell = true,
            // TODO
        };


        public void OnSpellPreCast(ObjAIBase owner, Spell spell, AttackableUnit target, Vector2 start, Vector2 end)
        {
            owner = spell.CastInfo.Owner;
            Target = target;
            originSpell = spell;
            SetStatus(owner, StatusFlags.Ghosted, true);
            ApiEventManager.OnMoveEnd.AddListener(this, owner, OnMoveEnd, true);
            //ApiEventManager.OnMoveSuccess.AddListener(this, owner, OnMoveSuccess, true);
        }

        public void OnSpellCast(Spell spell)
        {
            var owner = spell.CastInfo.Owner as Champion;
            var Target = spell.CastInfo.Targets[0].Unit;
            var truecoords = Target.Position;
            FaceDirection(truecoords, spell.CastInfo.Owner, true);
            ForceMovement(spell.CastInfo.Owner, null, truecoords, 2400, 0, 120, 0);
            toRemove = false;
        }

        public void OnMoveEnd(AttackableUnit unit)
        {
            toRemove = true;
            SetStatus(unit, StatusFlags.Ghosted, false);
        }
    }
}