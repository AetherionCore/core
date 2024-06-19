using GameServerCore.Enums;
using LeagueSandbox.GameServer.Scripting.CSharp;
using LeagueSandbox.GameServer.API;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using LeagueSandbox.GameServer.GameObjects.SpellNS;
using LeagueSandbox.GameServer.GameObjects.SpellNS.Missile;
using LeagueSandbox.GameServer.GameObjects.SpellNS.Sector;
using System.Numerics;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;

namespace Spells
{
    public class CassiopeiaNoxiousBlast : ISpellScript
    {
        public SpellScriptMetadata ScriptMetadata => new SpellScriptMetadata()
        {
            TriggersSpellCasts = true,
            CastingBreaksStealth = true,
            DoesntBreakShields = true,
            IsDamagingSpell = true,
            NotSingleTargetSpell = true,
        };

        public void OnActivate(ObjAIBase owner, Spell spell)
        {
            _owner = owner;
            _spell = spell;
            ApiEventManager.OnSpellHit.AddListener(this, spell, TargetExecute, false);
        }

        public SpellSector DamageSector;

        ObjAIBase _owner;
        Spell _spell;

        public void OnDeactivate(ObjAIBase owner, Spell spell)
        {
        }

        public void OnSpellPreCast(ObjAIBase owner, Spell spell, AttackableUnit target, Vector2 start, Vector2 end)
        {
            //AddBuff("CassiopeiaDeadlyCadence", float.MaxValue, 1, spell, owner, owner, true);
        }

        public void OnSpellCast(Spell spell)
        {
        }

        public void OnSpellPostCast(Spell spell)
        {
            var owner = spell.CastInfo.Owner;
            var targetPos = GetPointFromUnit(owner, 850.0f);
            SpellCast(owner, 0, SpellSlotType.ExtraSlots, targetPos, targetPos, false, Vector2.Zero);
            var spellpos = new Vector2(spell.CastInfo.TargetPositionEnd.X, spell.CastInfo.TargetPositionEnd.Z);

            AddParticle(owner, null, "Cassiopeia_Base_Q_Hit_Green.troy", spellpos, lifetime: 0.5f);
            AddParticle(owner, null, "Cassiopeia_Base_Q_Hit_Red.troy", spellpos, lifetime: 0.5f);
            AddParticle(owner, null, "CassNoxiousBlast_cas.troy", spellpos, lifetime: 0.5f);
            AddParticle(owner, null, "CassNoxiousBlast_glow.troy", spellpos, lifetime: 0.5f);
            AddParticle(owner, null, "CassNoxiousBlast_tar.troy", spellpos, lifetime: 0.5f);
            DamageSector = spell.CreateSpellSector(new SectorParameters
            {
                Length = 75f,
                Tickrate = 2,
                CanHitSameTargetConsecutively = false,
                OverrideFlags = SpellDataFlags.AffectEnemies | SpellDataFlags.AffectNeutral | SpellDataFlags.AffectMinions | SpellDataFlags.AffectHeroes,
                Type = SectorType.Area,
                Lifetime = 0.5f
            });
        }

        public void OnSpellChannel(Spell spell)
        {
        }

        public void TargetExecute(Spell spell, AttackableUnit target, SpellMissile missile, SpellSector sector)
        {
            var owner = spell.CastInfo.Owner;

            AddBuff("CassiopeiaPoisonTicker", 4f, 1, spell, target, owner);
            if (target is Champion)
            {
                AddBuff("CassiopeiaNoxiousBlastHaste", 3f, 1, spell, owner, owner);
            }
        }

        public void OnSpellChannelCancel(Spell spell, ChannelingStopSource source)
        {
        }

        public void OnSpellPostChannel(Spell spell)
        {
        }

        public void OnUpdate(float diff)
        {
            if (_owner.HasBuff("CassiopeiaDeadlyCadence"))
            {
                var sc = _owner.GetBuffWithName("CassiopeiaDeadlyCadence").StackCount;
                if (sc >= 100)
                {
                    if (!_owner.HasBuff("Passive100"))
                    {
                        AddBuff("Passive100", 5.0f, 1, _spell, _owner, _owner, false);
                    }
                }
                if (sc >= 250)
                {
                    if (!_owner.HasBuff("Passive250"))
                    {
                        AddBuff("Passive250", 5.0f, 1, _spell, _owner, _owner, false);
                    }
                }
                if (sc >= 500)
                {
                    if (!_owner.HasBuff("Passive500"))
                    {
                        AddBuff("Passive500", 5.0f, 1, _spell, _owner, _owner, false);
                    }
                }
            }
        }
    }
}