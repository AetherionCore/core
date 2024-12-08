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
using System;

namespace Spells
{
    public class OrianaDetonateCommand : ISpellScript
    {
        public SpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
        {
            TriggersSpellCasts = true,
        };

        private ObjAIBase _orianna;
        private Spell _spell;

        private Buffs.OriannaBallHandler _ballHandler;
        private bool _queuedCast = false;
        private SpellSector _detonateSector;

        public void OnActivate(ObjAIBase owner, Spell spell)
        {
            _orianna = owner;
            _spell = spell;
        }

        public void OnSpellPreCast(ObjAIBase owner, Spell spell, AttackableUnit target, Vector2 start, Vector2 end)
        {
            _ballHandler = (_orianna.GetBuffWithName("OriannaBallHandler").BuffScript as Buffs.OriannaBallHandler);
        }

        public void OnSpellCast(Spell spell)
        {
            if (_ballHandler.GetIsAttached())
            {
                var attachPos = _ballHandler.GetAttachedChampion().Position;
                SpellCast(_orianna, 3, SpellSlotType.ExtraSlots, attachPos, attachPos, false, attachPos, overrideForceLevel: spell.CastInfo.SpellLevel);
            }
            else
            {
                if (_ballHandler.GetFlightState())
                {
                    _queuedCast = true;
                }
                else
                {
                    var ballPos = _ballHandler.GetBall().Position;
                    SpellCast(_orianna, 3, SpellSlotType.ExtraSlots, ballPos, ballPos, false, ballPos, overrideForceLevel: spell.CastInfo.SpellLevel);
                }
            }
        }

        public void OnSpellChannel(Spell spell)
        {
            var spellLevel = spell.CastInfo.SpellLevel - 1;
            var coolDown = new[] { 120f, 105f, 90f }[spellLevel];
            spell.SetCooldown(coolDown);

            var manaCost = new[] { 100, 125, 150 }[spellLevel];
            _orianna.Stats.CurrentMana -= manaCost;
        }
    }

    public class OrianaDissonanceWave : ISpellScript
    {
        public SpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
        {
            TriggersSpellCasts = true,
            IsDamagingSpell = true,
            NotSingleTargetSpell = true,
        };

        private ObjAIBase _orianna;
        private Spell _spell;
        private SectorParameters _enemySector;

        private Buffs.OriannaBallHandler _ballHandler;

        public void OnActivate(ObjAIBase owner, Spell spell)
        {
            _orianna = owner;
            _spell = spell;
            ApiEventManager.OnSpellHit.AddListener(this, spell, TargetExecute, false);
        }

        public void OnSpellPreCast(ObjAIBase owner, Spell spell, AttackableUnit target, Vector2 start, Vector2 end)
        {
            Console.WriteLine("Triggered: OrianaDissonanceWave");
            _ballHandler = (_orianna.GetBuffWithName("OriannaBallHandler").BuffScript as Buffs.OriannaBallHandler);
        }

        public void OnSpellCast(Spell spell)
        {
            if (_ballHandler.GetIsAttached())
            {
                ExcuteSpell(_ballHandler.GetAttachedChampion().Position);
            }
            else
            {
                ExcuteSpell(_ballHandler.GetBall().Position);
            }
        }

        public void OnSpellPostCast(Spell spell)
        {
            _outerSector.SetToRemove();
        }

        private void ExcuteSpell(Vector2 position)
        {
            var tempMinion = AddMinion(_orianna, "TestCubeRender", "OriannaRMarker", position, _orianna.Team, ignoreCollision: true, targetable: false);
            AddBuff("ExpirationTimer", 1.0f, 1, _spell, tempMinion, _orianna);

            _outerSector = _spell.CreateSpellSector(new SectorParameters
            {
                BindObject = tempMinion,
                Length = 600,
                CanHitSameTarget = false,
                CanHitSameTargetConsecutively = false,
                Type = SectorType.Area,
                SingleTick = true,
                OverrideFlags = SpellDataFlags.AffectEnemies | SpellDataFlags.AffectNeutral | SpellDataFlags.AffectMinions | SpellDataFlags.AffectHeroes,
            });
        }

        private SpellSector _outerSector;

        private void TargetExecute(Spell spell, AttackableUnit target, SpellMissile missile, SpellSector sector)
        {
            if (_ballHandler.GetIsAttached())
            {
                target.FaceDirection(new Vector3(_ballHandler.GetAttachedChampion().Position.X, 0, _ballHandler.GetAttachedChampion().Position.Y));
            }
            else
            {
                target.FaceDirection(new Vector3(_ballHandler.GetBall().Position.X, 0, _ballHandler.GetBall().Position.Y));
            }

            //AddBuff("Stun", .75f, 1, _spell, target, _orianna);

            //ForceMovement(target, "STUNNED", _ballHandler.GetBall().Position, 800f, 500f, .4f, 0f);

            var spellLevel = spell.CastInfo.SpellLevel - 1;
            var baseDamage = new[] { 150, 225, 300, }[spellLevel];
            var magicDamage = _orianna.Stats.AbilityPower.Total * .7f;
            var finalDamage = baseDamage + magicDamage;

            //TODO: Find ult hit particle
            //target.TakeDamage(_orianna, finalDamage, DamageType.DAMAGE_TYPE_MAGICAL, DamageSource.DAMAGE_SOURCE_SPELL, false);

        }
    }
}