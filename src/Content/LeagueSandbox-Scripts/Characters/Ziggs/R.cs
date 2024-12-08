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
    public class ZiggsR : ISpellScript
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

        public void OnSpellCast(Spell spell)
        {
            var owner = spell.CastInfo.Owner as Champion;
            var ownerSkinID = owner.SkinID;
            var targetPos = new Vector2(spell.CastInfo.TargetPosition.X, spell.CastInfo.TargetPosition.Z);
            var ownerPos = owner.Position;
            var distance = Vector2.Distance(ownerPos, targetPos);
            FaceDirection(targetPos, owner);

            if (distance > 5000.0)
            {
                targetPos = GetPointFromUnit(owner, 5000.0f);
            }
            AddParticle(owner, null, "ZiggsR_green_preindicator.troy", targetPos, 10f);
        }

        public void OnSpellPostCast(Spell spell)
        {
            var owner = spell.CastInfo.Owner as Champion;
            var ownerSkinID = owner.SkinID;
            var targetPos = new Vector2(spell.CastInfo.TargetPosition.X, spell.CastInfo.TargetPosition.Z);
            var ownerPos = owner.Position;
            var distance = Vector2.Distance(ownerPos, targetPos);
            FaceDirection(targetPos, owner);

            if (distance > 5000.0)
            {
                targetPos = GetPointFromUnit(owner, 5000.0f);
            }
            SpellCast(owner, 0, SpellSlotType.ExtraSlots, targetPos, targetPos, false, Vector2.Zero);
            AddParticle(owner, null, "ZiggsR_flames_green_team.troy", targetPos, 10f);
        }
    }

    public class ZiggsRBoom : ISpellScript
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
            ApiEventManager.OnSpellMissileEnd.AddListener(this, missile, OnMissileEnd, true);
        }

        public void OnMissileEnd(SpellMissile missile)
        {
            var owner = missile.CastInfo.Owner;
            AddParticle(owner, null, "ZiggsR_Nova.troy", missile.Position, 10f);
            AddParticle(owner, null, ".troy", missile.Position, 10f);
            if (Spell.CastInfo.Owner is Champion c)
            {
                var damage = 80 + (50 * (Spell.CastInfo.SpellLevel - 1)) + (c.Stats.AbilityPower.Total);
                var units = GetUnitsInRange(missile.Position, 450f, true);
                for (int i = 0; i < units.Count; i++)
                {
                    if (units[i].Team != c.Team && !(units[i] is ObjBuilding || units[i] is BaseTurret))
                    {
                        units[i].TakeDamage(c, damage, DamageType.DAMAGE_TYPE_MAGICAL, DamageSource.DAMAGE_SOURCE_SPELL, false);
                        AddParticleTarget(c, units[i], "ZiggsR_tar", units[i]);
                    }
                }
            }
        }
    }
}