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
    public class OrianaRedactCommand : ISpellScript
    {

        public SpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
        {
            TriggersSpellCasts = true,
        };

        ObjAIBase _orianna;
        Buffs.OriannaBallHandler _ballHandler;

        public void OnActivate(ObjAIBase owner, Spell spell)
        {
            _orianna = owner;
            ApiEventManager.OnLevelUpSpell.AddListener(owner, spell, SpellLevelUp, false);
        }

        private void SpellLevelUp(Spell spell)
        {
            _ballHandler = (_orianna.GetBuffWithName("OriannaBallHandler").BuffScript as Buffs.OriannaBallHandler);

            if (_ballHandler.GetIsAttached())
            {
                if (_ballHandler.GetAttachedChampion() == (Champion)_orianna)
                {
                    AddBuff("OrianaGhostSelf", 1.0f, 1, spell, _ballHandler.GetAttachedChampion(), _orianna, true);
                }
                else
                {
                    AddBuff("OrianaGhost", 1.0f, 1, spell, _ballHandler.GetAttachedChampion(), _orianna, true);
                }
            }
        }

        public void OnDeactivate(ObjAIBase owner, Spell spell)
        {
        }

        public void OnSpellPreCast(ObjAIBase owner, Spell spell, AttackableUnit target, Vector2 start, Vector2 end)
        {
            _ballHandler = (_orianna.GetBuffWithName("OriannaBallHandler").BuffScript as Buffs.OriannaBallHandler);

            if (_ballHandler.GetIsAttached())
            {
                if (_ballHandler.GetAttachedChampion() == (Champion)_orianna)
                {
                    if ((Champion)target == (Champion)_orianna)
                    {
                        AddBuff("OrianaRedactShield", 2.5f, 1, spell, _orianna, _orianna);
                    }
                    else
                    {
                        SpellCast(_orianna, 2, SpellSlotType.ExtraSlots, target.Position, target.Position, false, _orianna.Position, spell.CastInfo.Targets, overrideForceLevel: spell.CastInfo.SpellLevel);

                        if (_orianna.Model == "Orianna")
                        {
                            _orianna.ChangeModel("OriannaNoBall");
                        }
                    }
                }
                else
                {
                    if ((Champion)target == _ballHandler.GetAttachedChampion())
                    {
                        AddBuff("OrianaRedactShield", 2.5f, 1, spell, _ballHandler.GetAttachedChampion(), _orianna);
                    }
                    else
                    {
                        SpellCast(owner, 2, SpellSlotType.ExtraSlots, target.Position, target.Position, false, _ballHandler.GetAttachedChampion().Position, spell.CastInfo.Targets, overrideForceLevel: spell.CastInfo.SpellLevel);
                    }
                }
            }
            else
            {
                SpellCast(owner, 2, SpellSlotType.ExtraSlots, target.Position, target.Position, false, _ballHandler.GetBall().Position, spell.CastInfo.Targets, overrideForceLevel: spell.CastInfo.SpellLevel);
            }

            _orianna.PlayAnimation("Spell2", 1f, 0, 0);
        }

        public void OnSpellPostCast(Spell spell)
        {
            spell.SetCooldown(9, false);

            _orianna.Stats.CurrentMana -= 60;
        }
    }

    public class OrianaRedact : ISpellScript
    {
        public SpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
        {
            TriggersSpellCasts = true,
            NotSingleTargetSpell = true,
            IsDamagingSpell = true,
        };

        ObjAIBase _orianna;
        Champion _target;
        Spell _spell;
        Buffs.OriannaBallHandler _ballHandler;

        public void OnActivate(ObjAIBase owner, Spell spell)
        {
            _orianna = owner;
            _spell = spell;
            ApiEventManager.OnSpellHit.AddListener(this, spell, TargetExecute, false);
        }

        public void OnSpellPreCast(ObjAIBase owner, Spell spell, AttackableUnit target, Vector2 start, Vector2 end)
        {
            DisableAbilityCheck();

            _ballHandler = (owner.GetBuffWithName("OriannaBallHandler").BuffScript as Buffs.OriannaBallHandler);

            _target = (Champion)spell.CastInfo.Targets[0].Unit;

            if (_ballHandler.GetAttachedChampion() != null)
            {
                _ballHandler.GetAttachedChampion().RemoveBuffsWithName("OrianaGhost");
                _ballHandler.GetAttachedChampion().RemoveBuffsWithName("OrianaGhostSelf");
            }

            _ballHandler.SetFlightState(true);

            var missile = spell.CreateSpellMissile(new MissileParameters
            {
                Type = MissileType.Circle,
                OverrideEndPosition = spell.CastInfo.Targets[0].Unit.Position,
            });

            ApiEventManager.OnSpellMissileEnd.AddListener(this, missile, OnMissileFinish, true);
        }

        public void TargetExecute(Spell spell, AttackableUnit target, SpellMissile missile, SpellSector sector)
        {
            if (target.Team != _orianna.Team)
            {
                if (missile is SpellCircleMissile skillshot)
                {
                    var owner = spell.CastInfo.Owner;
                    var spellLevel = spell.CastInfo.SpellLevel - 1;
                    var baseDamage = new[] { 60, 90, 120, 150, 180 }[spellLevel];
                    var magicDamage = owner.Stats.AbilityPower.Total * .3f;
                    var damage = baseDamage + magicDamage;
                    //Shares target hit partixcle with Orianna Q: OrianaIzuna
                    AddParticleTarget(_orianna, target, "OrianaIzuna_tar", target, 1f, teamOnly: _orianna.Team, bone: "pelvis", targetBone: "pelvis");
                    target.TakeDamage(owner, damage, DamageType.DAMAGE_TYPE_MAGICAL, DamageSource.DAMAGE_SOURCE_SPELL, false);
                }
            }
        }

        public void OnMissileFinish(SpellMissile missile)
        {
            _ballHandler.SetFlightState(false);


            if (_target.IsDead || !_target.GetIsTargetableToTeam(_orianna.Team))
            {
                _ballHandler.DisableBall(true, (Champion)_orianna);

                AddBuff("OrianaGhostSelf", 1f, 1, missile.SpellOrigin, _orianna, _orianna, true);

                if (_orianna.Model == "OriannaNoBall")
                {
                    _orianna.ChangeModel("Orianna");
                }

                return;
            }

            _ballHandler.DisableBall(true, _target);

            AddBuff("OrianaRedactShield", 4f, 1, missile.SpellOrigin, _target, _orianna);

            if (_target == (Champion)_orianna)
            {
                AddBuff("OrianaGhostSelf", 1f, 1, missile.SpellOrigin, _target, _orianna, true);

                if (_orianna.Model == "OriannaNoBall")
                {
                    _orianna.ChangeModel("Orianna");
                }
            }
            else
            {
                AddBuff("OrianaGhost", 1f, 1, missile.SpellOrigin, _target, _orianna, true);
            }

            _ballHandler.SetAttachedChampion(_target);
            _ballHandler.SetAttachedState(true);

            EnableAbilityCheck();
        }


        bool acivateQ = false;
        bool acivateW = false;
        bool acivateE = false;
        bool acivateR = false;

        //Spell should disable Q, W, E, and R if not on CD.
        private void DisableAbilityCheck()
        {
            if (_orianna.GetSpell("OrianaIzunaCommand").CurrentCooldown <= 0)
            {
                _orianna.SetSpell("OrianaIzunaCommand", 0, false);
                acivateQ = true;
            }
            //Check W
            if (_orianna.GetSpell("OrianaDissonanceCommand").CurrentCooldown <= 0)
            {
                _orianna.SetSpell("OrianaDissonanceCommand", 1, false);
                acivateW = true;
            }
            //Check E
            if (_orianna.GetSpell("OrianaRedactCommand").CurrentCooldown <= 0)
            {
                //_orianna.SetSpell("OrianaRedactCommand", 2, false);
                //acivateE = true;
            }
            //Check R
            if (_orianna.GetSpell("OrianaDetonateCommand").CurrentCooldown <= 0)
            {
                _orianna.SetSpell("OrianaDetonateCommand", 3, false);
                acivateR = true;
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