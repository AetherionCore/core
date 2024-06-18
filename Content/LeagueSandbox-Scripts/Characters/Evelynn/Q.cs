using GameServerCore.Enums;
using LeagueSandbox.GameServer.API;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using LeagueSandbox.GameServer.GameObjects.SpellNS;
using LeagueSandbox.GameServer.GameObjects.SpellNS.Missile;
using LeagueSandbox.GameServer.GameObjects.SpellNS.Sector;
using LeagueSandbox.GameServer.Scripting.CSharp;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using System.Numerics;
using GameServerCore.Scripting.CSharp;

namespace Spells
{
    public class EvelynnQ : ISpellScript
    {
        public SpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
        {
            AutoFaceDirection = false,
            TriggersSpellCasts = true,
            IsDamagingSpell = true
        };

        private ObjAIBase _owner;
        private Spell _spell;

        public void OnActivate(ObjAIBase owner, Spell spell)
        {
            _owner = owner;
            _spell = spell;

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
            var startPos = owner.Position;
            var target = GetClosestUnitInRange(owner, 500f, true);

            //var endPos = GetPointFromUnit(target, -500f);

            if (target != null)
            {
                if (!(target is BaseTurret) && target.GetIsTargetableToTeam(owner.Team))
                {
                    SpellCast(owner, 3, SpellSlotType.ExtraSlots, owner.Position, target.Position, true, Vector2.Zero);
                }
            }

        }

        public void OnSpellPostCast(Spell spell)
        {
            var manaCost = new[] { 12, 18, 24, 30, 36 }[spell.CastInfo.SpellLevel - 1];
            var coolDown = new[] { 1.5f, 1.5f, 1.5f, 1.5f, 1.5f }[spell.CastInfo.SpellLevel - 1];
            spell.SetCooldown(coolDown);
            _owner.Stats.CurrentMana -= manaCost;
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

        bool CanCastQ = false;
        public void OnUpdate(float diff)
        {
            var targets = GetUnitsInRange(_owner.Position, (_owner.GetSpell("EvelynnQ").SpellData.CastRangeDisplayOverride - 10), true);

            foreach (var target in targets)
            {
                if (_owner.Team != target.Team)
                {
                    if (!(target is BaseTurret) && !(target is LaneTurret) && !(target is AzirTurret))
                    {
                        if (target.GetIsTargetableToTeam(_owner.Team))
                        {
                            if(!CanCastQ)
                            {
                                _owner.SetSpell("EvelynnQ", 0, true);
                                CanCastQ = true;
                            }
                            
                            break;
                        }
                    }
                }
                _owner.SetSpell("EvelynnQ", 0, false);
                CanCastQ = false;
            }
        }
    }


    public class HateSpikeLineMissile : ISpellScript
    {
        public SpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
        {
            MissileParameters = new MissileParameters{Type = MissileType.Circle},
            IsDamagingSpell = true,
            
        };

        //Vector2 direction;
        ObjAIBase _owner;
        public void OnActivate(ObjAIBase owner, Spell spell)
        {
            _owner = owner;
            ApiEventManager.OnSpellHit.AddListener(this, spell, TargetExecute, false);
        }

        public void OnDeactivate(ObjAIBase owner, Spell spell)
        {
        }

        public void OnSpellPreCast(ObjAIBase owner, Spell spell, AttackableUnit target, Vector2 start, Vector2 end)
        {
        }

        public void OnSpellCast(Spell spell)
        {
        }

        public void OnSpellPostCast(Spell spell)
        {
            var owner = spell.CastInfo.Owner;
        }

        public void TargetExecute(Spell spell, AttackableUnit target, SpellMissile missile, SpellSector sector)
        {
            var owner = spell.CastInfo.Owner;

            //DAMAGE CALC
            var spellLevel = spell.CastInfo.SpellLevel - 1;

            var flat = new[] { 40, 50, 60, 70, 80 }[spellLevel];
            var ad = new[] { 
                    owner.Stats.AttackDamage.FlatBonus * .5f,
                    owner.Stats.AttackDamage.FlatBonus * .55f,
                    owner.Stats.AttackDamage.FlatBonus * .6f,
                    owner.Stats.AttackDamage.FlatBonus * .65f,
                    owner.Stats.AttackDamage.FlatBonus * .7f
                }[spellLevel];
            var ap = new[] {
                    owner.Stats.AbilityPower.Total * .35f,
                    owner.Stats.AbilityPower.Total * .4f,
                    owner.Stats.AbilityPower.Total * .45f,
                    owner.Stats.AbilityPower.Total * .5f,
                    owner.Stats.AbilityPower.Total * .55f
                }[spellLevel];
            var damage = flat + ad + ap;

            AddParticle(owner, target, "Evelynn_Q_tar", target.Position, bone: "BUFFBONE_GLB_GROUND_LOC", lifetime: 2.0f);
            target.TakeDamage(owner, damage, DamageType.DAMAGE_TYPE_MAGICAL, DamageSource.DAMAGE_SOURCE_SPELL, false);
            //AddParticle(owner, target, "Evelynn_Q_mis", target.Position, bone: "BUFFBONE_GLB_GROUND_LOC", lifetime: 2.0f);
            
            //AddParticleTarget(owner, target, "Evelynn_HateSpike", target, 1.0f);
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

