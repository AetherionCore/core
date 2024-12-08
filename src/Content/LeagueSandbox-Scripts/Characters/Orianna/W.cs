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
using LeaguePackets.Game.Events;

namespace Spells
{
    public class OrianaDissonanceCommand : ISpellScript
    {
        public SpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
        {
            TriggersSpellCasts = true,
            IsDamagingSpell = true,
            NotSingleTargetSpell = true,
        };

        private ObjAIBase _orianna;
        private Spell _spell;

        private Buffs.OriannaBallHandler _ballHandler;

        private Vector2 _spellPos;
        private SpellSector _allyBuffSector;
        private SpellSector _enemyDamageSector;
        private SpellSector _enemySlowSector;

        private bool _queuedCast = false;

        public void OnActivate(ObjAIBase owner, Spell spell)
        {
            _orianna = owner;
            _spell = spell;
            ApiEventManager.OnSpellHit.AddListener(_orianna, spell, TargetExecute, false);
        }

        public void OnDeactivate(ObjAIBase owner, Spell spell)
        {
        }

        public void OnSpellPreCast(ObjAIBase owner, Spell spell, AttackableUnit target, Vector2 start, Vector2 end)
        {
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
                if (_ballHandler.GetFlightState())
                {
                    _queuedCast = true;
                }
                else
                {
                    ExcuteSpell(_ballHandler.GetBall().Position);
                }
            }
        }

        private void ExcuteSpell(Vector2 position)
        {
            var tempMinion = AddMinion(_orianna, "TestCubeRender", "OriannaWMarker", position, _orianna.Team, ignoreCollision: true, targetable: false);
            AddBuff("ExpirationTimer", 3.0f, 1, _spell, tempMinion, _orianna);

            CreateWSpellSectors(tempMinion);

            TeamId enemyTeamId;
            if (_orianna.Team == TeamId.TEAM_BLUE)
            {
                enemyTeamId = TeamId.TEAM_PURPLE;
            }
            else
            {
                enemyTeamId = TeamId.TEAM_BLUE;
            }

            AddParticlePos(_orianna, "OrianaDissonance_ally_green", position, position, lifetime: 3f, teamOnly: _orianna.Team);
            //AddParticlePos(_orianna, "OrianaDissonance_ball_green", position, position, lifetime: 3f, teamOnly: _orianna.Team);
            //AddParticlePos(_orianna, "OrianaDissonance_cas_green", position, position, lifetime: 3f, teamOnly: _orianna.Team);

            AddParticlePos(_orianna, "OrianaDissonance_ally_red", position, position, lifetime: 3f, teamOnly: enemyTeamId);
            //AddParticlePos(_orianna, "OrianaDissonance_ball_red", position, position, lifetime: 3f, teamOnly: enemyTeamId);
            //AddParticlePos(_orianna, "OrianaDissonance_cas_red", position, position, lifetime: 3f, teamOnly: enemyTeamId);


            _orianna.PlayAnimation("Spell2", 1f, 0, 0);
        }

        public void OnSpellPostCast(Spell spell)
        {
            if (!_queuedCast)
            {
                _spell.SetCooldown(9.0f);

                var manaCost = new[] { 70, 80, 90, 100, 110 }[_spell.CastInfo.SpellLevel - 1];
                _orianna.Stats.CurrentMana -= manaCost;
            }
        }

        private void TriggerCosts()
        {
            _spell.SetCooldown(9.0f);

            var manaCost = new[] { 70, 80, 90, 100, 110 }[_spell.CastInfo.SpellLevel - 1];
            _orianna.Stats.CurrentMana -= manaCost;
        }

        private void TargetExecute(Spell spell, AttackableUnit target, SpellMissile missile, SpellSector sector)
        {
            if (_allyBuffSector != null && sector == _allyBuffSector)
            {
                AddBuff("OrianaDissonanceAlly", 2.0f, 1, spell, target, _orianna);
                return;
            }

            AddBuff("OrianaDissonanceEnemy", 2.0f, 1, spell, target, _orianna);

            if (sector == _enemyDamageSector)
            {
                var spellLevel = spell.CastInfo.SpellLevel - 1;
                var baseDamage = new[] { 60, 105, 150, 195, 240 }[spellLevel];
                var magicDamage = _orianna.Stats.AbilityPower.Total * .7f;
                var finalDamage = baseDamage + magicDamage;

                //TODO: Find particle used on champs damaged by W. Looked to be a shared particle with Ultimate on Damage particle. this is a placeholder particle
                AddParticleTarget(_orianna, target, "Oriana_ts_tar.troy", target, 1f, teamOnly: _orianna.Team, bone: "pelvis", targetBone: "pelvis");
                target.TakeDamage(_orianna, finalDamage, DamageType.DAMAGE_TYPE_MAGICAL, DamageSource.DAMAGE_SOURCE_SPELL, false);
            }
        }

        public void CreateWSpellSectors(Minion bindMinion)
        {
            _allyBuffSector = _spell.CreateSpellSector(new SectorParameters
            {
                BindObject = bindMinion,
                Length = 225,
                OverrideFlags = SpellDataFlags.AffectFriends | SpellDataFlags.AffectMinions | SpellDataFlags.AffectHeroes,
                Lifetime = 3f,
                CanHitSameTargetConsecutively = true,
                Type = SectorType.Area,
            });

            _enemySlowSector = _spell.CreateSpellSector(new SectorParameters
            {
                BindObject = bindMinion,
                Length = 225,
                OverrideFlags = SpellDataFlags.AffectEnemies | SpellDataFlags.AffectNeutral | SpellDataFlags.AffectMinions | SpellDataFlags.AffectHeroes,
                Lifetime = 3f,
                CanHitSameTargetConsecutively = true,
                Type = SectorType.Area,
            });

            _enemyDamageSector = _spell.CreateSpellSector(new SectorParameters
            {
                BindObject = bindMinion,
                Length = 225,
                OverrideFlags = SpellDataFlags.AffectEnemies | SpellDataFlags.AffectNeutral | SpellDataFlags.AffectMinions | SpellDataFlags.AffectHeroes,
                Type = SectorType.Area,
            });
        }

        public void OnUpdate(float diff)
        {
            if (_queuedCast && !_ballHandler.GetFlightState())
            {
                ExcuteSpell(_ballHandler.GetBall().Position);
                _queuedCast = false;
                OnSpellPostCast(_spell);

            }
        }
    }
}