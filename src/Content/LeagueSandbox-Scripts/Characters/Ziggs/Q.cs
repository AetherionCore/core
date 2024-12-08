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
    public class ZiggsQ : ISpellScript
    {
        public SpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
        {
            TriggersSpellCasts = true,
            IsDamagingSpell = false
        };

        private ObjAIBase _owner;
        private Spell _spell;
        public void OnActivate(ObjAIBase owner, Spell spell)
        {
            _owner = owner;
            _spell = spell;
        }

        public void OnSpellPostCast(Spell spell)
        {
            var owner = spell.CastInfo.Owner as Champion;
            var ownerSkinID = owner.SkinID;
            var targetPos = new Vector2(spell.CastInfo.TargetPosition.X, spell.CastInfo.TargetPosition.Z);
            var ownerPos = owner.Position;
            var distance = Vector2.Distance(ownerPos, targetPos);
            FaceDirection(targetPos, owner);

            if (distance > 800.0)
            {
                targetPos = GetPointFromUnit(owner, 800.0f);
            }
            SpellCast(owner, 4, SpellSlotType.ExtraSlots, targetPos, targetPos, false, Vector2.Zero);
            AddParticle(owner, null, ".troy", targetPos, 10f);
        }
    }

    public class ZiggsQSpell : ISpellScript
    {
        public SpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
        {
            // TODO
        };

        Vector2 POS;
        Spell Spell;
        ObjAIBase missile;
        ObjAIBase Owner;

        public void OnSpellPreCast(ObjAIBase owner, Spell spell, AttackableUnit target, Vector2 start, Vector2 end)
        {
            Spell = spell;
            var P = owner.Position;
            POS = P;
            Owner = spell.CastInfo.Owner as Champion;
            var missile = spell.CreateSpellMissile(new MissileParameters
            {
                Type = MissileType.Arc,
                OverrideEndPosition = end
            });
            ApiEventManager.OnSpellMissileEnd.AddListener(this, missile, OnMissileEnd, true);
        }

        public void OnMissileEnd(SpellMissile missile)
        {
            var owner = missile.CastInfo.Owner;
            var dist = System.Math.Abs(Vector2.Distance(POS, missile.Position));
            AddParticle(owner, null, "ZiggsQBounce.troy", missile.Position, 10f);
            AddParticle(owner, null, ".troy", missile.Position, 10f);
            SpellCast(owner, 5, SpellSlotType.ExtraSlots, GetPointFromUnit(missile, dist / 2), Vector2.Zero, true, missile.Position);
        }
    }

    public class ZiggsQSpell2 : ISpellScript
    {
        public SpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
        {
            // TODO
        };

        Vector2 POS;
        Spell Spell;

        public void OnSpellPreCast(ObjAIBase owner, Spell spell, AttackableUnit target, Vector2 start, Vector2 end)
        {
            Spell = spell;
            var P = owner.Position;
            POS = P;
            var missile = spell.CreateSpellMissile(new MissileParameters
            {
                Type = MissileType.Arc,
                OverrideEndPosition = end
            });
            ApiEventManager.OnSpellMissileEnd.AddListener(this, missile, OnMissileEnd, true);
        }

        public void OnMissileEnd(SpellMissile missile)
        {
            var owner = missile.CastInfo.Owner;
            var dist = System.Math.Abs(Vector2.Distance(POS, missile.Position));
            AddParticle(owner, null, "ZiggsQBounce2.troy", missile.Position, 10f);
            AddParticle(owner, null, ".troy", missile.Position, 10f);
            SpellCast(owner, 6, SpellSlotType.ExtraSlots, GetPointFromUnit(missile, dist / 6), Vector2.Zero, true, missile.Position);
        }
    }
    public class ZiggsQSpell3 : ISpellScript
    {
        public SpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
        {
            // TODO
        };

        Vector2 POS;
        Spell Spell;

        public void OnSpellPreCast(ObjAIBase owner, Spell spell, AttackableUnit target, Vector2 start, Vector2 end)
        {
            Spell = spell;
            var P = owner.Position;
            POS = P;
            var missile = spell.CreateSpellMissile(new MissileParameters
            {
                Type = MissileType.Arc,
                OverrideEndPosition = end
            });
            ApiEventManager.OnSpellMissileEnd.AddListener(this, missile, OnMissileEnd, true);
        }
        public void OnMissileEnd(SpellMissile missile)
        {
            var owner = missile.CastInfo.Owner;
            //var dist = System.Math.Abs(Vector2.Distance(POS, missile.Position));
            //AddParticle(owner, null, "ZiggsQBounce.troy", missile.Position,10f);
            AddParticle(owner, null, "ZiggsQExplosion.troy", missile.Position, 10f);
            //SpellCast(owner, 6, SpellSlotType.ExtraSlots, GetPointFromUnit(missile, dist/6), Vector2.Zero, true, missile.Position);
        }
    }
}