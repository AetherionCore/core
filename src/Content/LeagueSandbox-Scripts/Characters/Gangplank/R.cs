using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.Scripting.CSharp;
using System;
using System.Numerics;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using LeagueSandbox.GameServer.GameObjects.SpellNS;
using LeagueSandbox.GameServer.GameObjects.SpellNS.Missile;
using LeagueSandbox.GameServer.GameObjects.SpellNS.Sector;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using LeagueSandbox.GameServer.API;
using GameServerCore.Enums;

namespace Spells
{
    public class CannonBarrage : ISpellScript
    {
        public SpellScriptMetadata ScriptMetadata => new SpellScriptMetadata()
        {
        };

        public void OnActivate(ObjAIBase owner, Spell spell)
        {
            ApiEventManager.OnSpellHit.AddListener(this, spell, TargetExecute, false);
        }

        public void TargetExecute(Spell spell, AttackableUnit target, SpellMissile missile, SpellSector sector)
        {
            var ap = spell.CastInfo.Owner.Stats.AbilityPower.Total * 0.2f;
            var damage = 75 + (45 * (spell.CastInfo.SpellLevel - 1)) + ap;
            target.TakeDamage(spell.CastInfo.Owner, damage, DamageType.DAMAGE_TYPE_MAGICAL, DamageSource.DAMAGE_SOURCE_SPELLAOE, false);
            AddBuff("CannonBarrageSlow", 1.25f, 1, spell, target, spell.CastInfo.Owner);
            var particle = AddParticlePos(spell.CastInfo.Owner, "pirate_cannonBarrage_tar.troy", target.Position, target.Position, lifetime: 1.0f);
        }

        public void OnSpellPreCast(ObjAIBase owner, Spell spell, AttackableUnit target, Vector2 start, Vector2 end)
        {
            owner.StopMovement();

            var spellPos = new Vector2(spell.CastInfo.TargetPosition.X, spell.CastInfo.TargetPosition.Z);
            var mushroom = AddMinion(owner, "TestCubeRender", "TestCubeRender", spellPos);

            var Champs = GetChampionsInRange(owner.Position, 50000, true);

            var sec = spell.CreateSpellSector(new SectorParameters
            {
                BindObject = mushroom,
                Length = 600f,
                Tickrate = 1,
                CanHitSameTargetConsecutively = true,
                OverrideFlags = SpellDataFlags.AffectEnemies | SpellDataFlags.AffectNeutral | SpellDataFlags.AffectMinions | SpellDataFlags.AffectHeroes,
                Type = SectorType.Area
            });
            var particle = AddParticlePos(owner, "pirate_cannonBarrage_aoe_indicator_green.troy", spellPos, spellPos, lifetime: 7.0f);
            var i = 1.0f;
            while (i <= 7.0f)
            {
                CreateTimer(i, () =>
                {
                    var randOffsetX = (float)new Random().Next(-400, 400);
                    var randOffsetY = (float)new Random().Next(-400, 400);

                    var randOffsetX2 = (float)new Random().Next(-400, 400);
                    var randOffsetY2 = (float)new Random().Next(-400, 400);

                    var randOffsetX3 = (float)new Random().Next(-400, 400);
                    var randOffsetY3 = (float)new Random().Next(-400, 400);

                    var randOffsetX4 = (float)new Random().Next(-400, 400);
                    var randOffsetY4 = (float)new Random().Next(-400, 400);

                    var randOffsetX5 = (float)new Random().Next(-400, 400);
                    var randOffsetY5 = (float)new Random().Next(-400, 400);

                    var randOffsetX6 = (float)new Random().Next(-400, 400);
                    var randOffsetY6 = (float)new Random().Next(-400, 400);

                    var randPoint = new Vector2(spellPos.X + randOffsetX, spellPos.Y + randOffsetY);
                    var randPoint2 = new Vector2(spellPos.X + randOffsetX2, spellPos.Y + randOffsetY2);
                    var randPoint3 = new Vector2(spellPos.X + randOffsetX3, spellPos.Y + randOffsetY3);
                    var randPoint4 = new Vector2(spellPos.X + randOffsetX4, spellPos.Y + randOffsetY4);
                    var randPoint5 = new Vector2(spellPos.X + randOffsetX5, spellPos.Y + randOffsetY5);
                    var randPoint6 = new Vector2(spellPos.X + randOffsetX6, spellPos.Y + randOffsetY6);

                    var particle1 = AddParticlePos(owner, "pirate_cannonBarrage_point.troy", randPoint, randPoint, lifetime: 1.0f);
                    var particle2 = AddParticlePos(owner, "pirate_cannonBarrage_point.troy", randPoint2, randPoint2, lifetime: 1.0f);
                    var particle3 = AddParticlePos(owner, "pirate_cannonBarrage_point.troy", randPoint3, randPoint3, lifetime: 1.0f);
                    var particle4 = AddParticlePos(owner, "pirate_cannonBarrage_tar.troy", randPoint4, randPoint4, lifetime: 1.0f);
                    var particle5 = AddParticlePos(owner, "pirate_cannonBarrage_tar.troy", randPoint5, randPoint5, lifetime: 1.0f);
                    var particle6 = AddParticlePos(owner, "pirate_cannonBarrage_tar.troy", randPoint6, randPoint6, lifetime: 1.0f);



                });
                LogDebug(i.ToString());
                i++;
            }
            CreateTimer(7.0f, () => { mushroom.TakeDamage(owner, 50000, DamageType.DAMAGE_TYPE_TRUE, DamageSource.DAMAGE_SOURCE_INTERNALRAW, false); sec.SetToRemove(); });
        }
    }
}
namespace Spells
{
    public class CannonBarrageBall : ISpellScript
    {
        public SpellScriptMetadata ScriptMetadata => new SpellScriptMetadata()
        {
        };
    }
}