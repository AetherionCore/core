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

namespace Spells
{
    public class OrianaIzunaCommand : ISpellScript
    {
        public SpellScriptMetadata ScriptMetadata => new SpellScriptMetadata()
        {
            TriggersSpellCasts = true,
        };

        ObjAIBase _orianna;
        Buffs.OriannaBallHandler _ballHandler;

        public void OnActivate(ObjAIBase owner, Spell spell)
        {
            _orianna = owner;
        }

        public void OnSpellPreCast(ObjAIBase owner, Spell spell, AttackableUnit target, Vector2 start, Vector2 end)
        {
            _ballHandler = (owner.GetBuffWithName("OriannaBallHandler").BuffScript as Buffs.OriannaBallHandler);
        }

        public void OnSpellCast(Spell spell)
        {
            //TODO: Determine if Orianna should walk in range and cast spell or leave it so that she just walks to the cast location if it casted out of her spell range.
            var spellPos = new Vector2(spell.CastInfo.TargetPosition.X, spell.CastInfo.TargetPosition.Z);


            if (_ballHandler.GetIsAttached())
            {
                SpellCast(_orianna, 0, SpellSlotType.ExtraSlots, spellPos, spellPos, false, _ballHandler.GetAttachedChampion().Position, overrideForceLevel: spell.CastInfo.SpellLevel);
            }
            else
            {
                SpellCast(_orianna, 0, SpellSlotType.ExtraSlots, spellPos, spellPos, false, _ballHandler.GetBall().Position, overrideForceLevel: spell.CastInfo.SpellLevel);
            }

            //TODO: Think of better way of handling Orianna Model Changing between spells.
            //Maybe use a internal buff to keep track of overall model state instead of setting it within individual spells.
            if (_orianna.Model == "Orianna")
            {
                _orianna.ChangeModel("OriannaNoBall");
            }

            //TODO: Clean up animation. Slight sliding as animtion fully plays while moving. Spell does not stop Orianna's Movement commands.
            _orianna.PlayAnimation("Spell1", 1f, 0, 0);
        }

        public void OnSpellPostCast(Spell spell)
        {
            var coolDown = new[] { 6f, 5.25f, 4.5f, 3.75f, 3f }[spell.CastInfo.SpellLevel - 1];
            spell.SetCooldown(coolDown);

            //Live servers have her manna cost decrease but in gametooltips for the 4.20 gameclient show her mana cost being 50 at all spell levels.
            //var manaCost = new[] { 30, 35, 40, 45, 50 }[spell.CastInfo.SpellLevel - 1];
            _orianna.Stats.CurrentMana -= 50;
        }
    }

    public class OrianaIzuna : ISpellScript
    {
        public SpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
        {
            TriggersSpellCasts = true,
            NotSingleTargetSpell = true,
            IsDamagingSpell = true,
        };

        private ObjAIBase _orianna;
        private Spell _spell;
        private Vector2 _spellPos;
        private SpellMissile _missile;
        private Buffs.OriannaBallHandler _ballHandler;
        private SpellSector _damageSector;

        public void OnActivate(ObjAIBase owner, Spell spell)
        {
            _orianna = owner;
            _spell = spell;
            ApiEventManager.OnSpellHit.AddListener(this, spell, TargetExecute, false);
        }

        public void OnSpellPreCast(ObjAIBase owner, Spell spell, AttackableUnit target, Vector2 start, Vector2 end)
        {
            _spellPos = new Vector2(spell.CastInfo.TargetPosition.X, spell.CastInfo.TargetPosition.Z);
            _ballHandler = (owner.GetBuffWithName("OriannaBallHandler").BuffScript as Buffs.OriannaBallHandler);
            DisableAbilityCheck();
        }

        public void OnSpellPostCast(Spell spell)
        {
            var ballPos = _ballHandler.GetBall().Position;
            if (ballPos == _spellPos)
            {
                CreateDamageSector();
                AddParticlePos(_orianna, "Oriana_Izuna_nova", ballPos, ballPos, 1f, bone: "BUFFBONE_CSTM_WEAPONA");
            }
            else
            {
                if (_ballHandler.GetIsAttached())
                {
                    _ballHandler.GetAttachedChampion().RemoveBuffsWithName("OrianaGhost");
                    _ballHandler.GetAttachedChampion().RemoveBuffsWithName("OrianaGhostSelf");
                }


                _ballHandler.SetFlightState(true);


                _missile = spell.CreateSpellMissile(new MissileParameters
                {
                    Type = MissileType.Circle,
                    OverrideEndPosition = _spellPos,
                });
                ApiEventManager.OnSpellMissileEnd.AddListener(this, _missile, OnMissileFinish, true);

                _ballHandler.SetRenderState(false);

                if (_ballHandler.GetAttachedChampion() != null)
                {
                    _ballHandler.GetAttachedChampion().RemoveBuffsWithName("OrianaGhost");
                    _ballHandler.GetAttachedChampion().RemoveBuffsWithName("OrianaGhostSef");
                }
            }
        }

        private int _targetHitCount = 0;
        private List<AttackableUnit> _targetsHit = new List<AttackableUnit>();

        public void TargetExecute(Spell spell, AttackableUnit target, SpellMissile missile, SpellSector sector)
        {
            if (!_targetsHit.Contains(target))
            {
                _targetHitCount++;

                var owner = spell.CastInfo.Owner;
                var spellLevel = spell.CastInfo.SpellLevel - 1;
                var baseDamage = new[] { 60, 90, 120, 150, 180 }[spellLevel];
                var magicDamage = owner.Stats.AbilityPower.Total * .5f;
                var damage = baseDamage + magicDamage;

                var reductionAmount = 0;
                if (_targetHitCount >= 7)
                {
                    reductionAmount = 7;
                }
                else
                {
                    reductionAmount = _targetHitCount;
                }
                var finalDamage = damage * (1.10f - (.1f * reductionAmount));
                AddParticleTarget(_orianna, target, "OrianaIzuna_tar", target, 1f, teamOnly: _orianna.Team, bone: "pelvis", targetBone: "pelvis");
                target.TakeDamage(owner, finalDamage, DamageType.DAMAGE_TYPE_MAGICAL, DamageSource.DAMAGE_SOURCE_SPELL, false);
                _targetsHit.Add(target);
            }
        }

        Particle allyMarker;
        Particle enemyMarker;
        public void OnMissileFinish(SpellMissile missile)
        {
            var paritclePos = _ballHandler.MoveBall(_spellPos, true);

            _ballHandler.SetFlightState(false);

            AddParticlePos(_orianna, "Oriana_Izuna_nova", paritclePos, paritclePos, 1f, bone: "BUFFBONE_CSTM_WEAPONA");

            _damageSector = missile.SpellOrigin.CreateSpellSector(new SectorParameters
            {
                Length = 10000,
                SingleTick = true,
                OverrideFlags = SpellDataFlags.AffectEnemies | SpellDataFlags.AffectNeutral | SpellDataFlags.AffectMinions | SpellDataFlags.AffectHeroes,
                BindObject = _ballHandler.GetBall(),
                Type = SectorType.Area,
            });

            _targetHitCount = 0;

            TeamId enemyTeamId;
            if (_orianna.Team == TeamId.TEAM_BLUE)
            {
                enemyTeamId = TeamId.TEAM_PURPLE;
            }
            else
            {
                enemyTeamId = TeamId.TEAM_BLUE;
            }

            //TODO figure out which bonesets the markers to surround the ball and not be on the ground
            //Possible it shouldn't be around the ball.On Live servers they use updated paticle indicators so this might just be a ground particle for this patch.
            //TODO: Better particle management.
            if (enemyMarker == null || allyMarker == null)
            {
                allyMarker = AddParticleTarget(_orianna, _ballHandler.GetBall(), "oriana_ball_glow_green", _ballHandler.GetBall(), 2300f, teamOnly: _orianna.Team, bone: "BUFFBONE_CSTM_WEAPONA");
                enemyMarker = AddParticlePos(_orianna, "oriana_ball_glow_red", _ballHandler.GetBall().Position, _ballHandler.GetBall().Position, 2300f, teamOnly: enemyTeamId, bone: "BUFFBONE_CSTM_WEAPONA");
            }
            else
            {
                //allyMarker.SetToRemove();
                enemyMarker.SetToRemove();
                //allyMarker = AddParticlePos(_owner, "oriana_ball_glow_green.troy", BallHandler.GetBall().Position, BallHandler.GetBall().Position, 2300f, teamOnly: _owner.Team, bone: "BUFFBONE_CSTM_WEAPONA");
                enemyMarker = AddParticlePos(_orianna, "oriana_ball_glow_red", _ballHandler.GetBall().Position, _ballHandler.GetBall().Position, 2300f, teamOnly: enemyTeamId, bone: "BUFFBONE_CSTM_WEAPONA");
            }
            _damageSector.SetToRemove();
            _targetsHit.Clear();

            EnableAbilityCheck();
        }

        private void CreateDamageSector()
        {

            _damageSector = _spell.CreateSpellSector(new SectorParameters
            {
                Length = 200,
                OverrideFlags = SpellDataFlags.AffectEnemies | SpellDataFlags.AffectNeutral | SpellDataFlags.AffectMinions | SpellDataFlags.AffectHeroes,
                BindObject = _ballHandler.GetBall(),
                Type = SectorType.Area,
            });
        }

        bool acivateQ = false;
        bool acivateW = false;
        bool acivateE = false;
        bool acivateR = false;
        //Spell should only disable Q and E if not on CD asince W and R can be queued cast.
        //Not sure if queue casting was present during this patch so will just asume it was.
        private void DisableAbilityCheck()
        {
            //Check Q
            if (_orianna.GetSpell("OrianaIzunaCommand").CurrentCooldown <= 0)
            {
                //_orianna.SetSpell("OrianaIzunaCommand", 0, false);
                //acivateQ = true;
            }
            //Check W
            if (_orianna.GetSpell("OrianaDissonanceCommand").CurrentCooldown <= 0)
            {
                //_orianna.SetSpell("OrianaDissonanceCommand", 1, false);
                //acivateW = true;
            }
            //Check E
            if (_orianna.GetSpell("OrianaRedactCommand").CurrentCooldown <= 0)
            {
                _orianna.SetSpell("OrianaRedactCommand", 2, false);
                acivateE = true;
            }
            //Check R
            if (_orianna.GetSpell("OrianaDetonateCommand").CurrentCooldown <= 0)
            {
                //_orianna.SetSpell("OrianaDetonateCommand", 3, false);
                //acivateR = true;
            }
        }
        private void EnableAbilityCheck()
        {
            //Check Q
            if (acivateQ)
            {
                _orianna.SetSpell("OrianaIzunaCommand", 0, true);
                acivateQ = false;
            }
            //Check W
            if (acivateW)
            {
                _orianna.SetSpell("OrianaDissonanceCommand", 1, true);
                acivateW = false;
            }
            //Check E
            if (acivateE)
            {
                _orianna.SetSpell("OrianaRedactCommand", 2, true);
                acivateE = false;
            }
            //Check R
            if (acivateR)
            {
                _orianna.SetSpell("OrianaDetonateCommand", 3, true);
                acivateR = false;
            }
        }
    }
}