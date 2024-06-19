using System.Numerics;
using GameServerCore.Enums;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using LeagueSandbox.GameServer.Scripting.CSharp;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.API;
using Buffs;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using LeagueSandbox.GameServer.GameObjects.SpellNS;
using LeagueSandbox.GameServer.GameObjects.SpellNS.Missile;
using LeagueSandbox.GameServer.GameObjects.SpellNS.Sector;

namespace Spells
{
    public class CaitlynYordleTrap : ISpellScript
    {
        public SpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
        {
            TriggersSpellCasts = true
        };

        public void OnActivate(ObjAIBase owner, Spell spell)
        {
            ApiEventManager.OnSpellHit.AddListener(this, spell, ExecuteSpell, false);
        }

        public void ExecuteSpell(Spell spell, AttackableUnit unit, SpellMissile mis, SpellSector sector)
        {
            unit.Stats.SetActionState(ActionState.CAN_MOVE, false);
            unit.StopMovement();
            CreateTimer(0.5f, () => { unit.StopMovement(); });
            CreateTimer(0.25f, () => { unit.StopMovement(); });
            CreateTimer(1.5f, () => { unit.Stats.SetActionState(ActionState.CAN_MOVE, true); });
            sector.SetToRemove();
            m1.TakeDamage(unit, 5000, DamageType.DAMAGE_TYPE_TRUE, DamageSource.DAMAGE_SOURCE_INTERNALRAW, false);
            m1 = null;
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

        public AttackableUnit unit;
        private Minion m1;
        private SpellSector sec;

        public void OnSpellPostCast(Spell spell)
        {
            var spellLVL = spell.CastInfo.SpellLevel;
            var owner = spell.CastInfo.Owner;
            unit = owner;
            var spellPos = new Vector2(spell.CastInfo.TargetPosition.X, spell.CastInfo.TargetPosition.Z);
            if (m1 == null)
            {
                m1 = AddMinion((Champion)owner, "CaitlynTrap", "CaitlynTrap", spellPos);
                m1.SetStatus(StatusFlags.Ghosted, true);
                m1.SetStatus(StatusFlags.Targetable, false);
                m1.IsVisibleByTeam(owner.Team);
                sec = spell.CreateSpellSector(new SectorParameters
                {
                    Length = 15f,
                    Tickrate = 100,
                    CanHitSameTargetConsecutively = true,
                    OverrideFlags = SpellDataFlags.AffectHeroes | SpellDataFlags.AffectEnemies | SpellDataFlags.IgnoreEnemyMinion,
                    //OverrideFlags = SpellDataFlags.AffectEnemies | SpellDataFlags.AffectNeutral | SpellDataFlags.AffectMinions | SpellDataFlags.AffectHeroes,
                    Type = SectorType.Area
                });
            }
            else
            {
                if (m1 != null)
                {
                    m1.TakeDamage(unit, 5000, DamageType.DAMAGE_TYPE_TRUE, DamageSource.DAMAGE_SOURCE_INTERNALRAW, false);
                    m1 = null;
                    m1 = AddMinion((Champion)owner, "CaitlynTrap", "CaitlynTrap", spellPos);
                    m1.SetStatus(StatusFlags.Ghosted, true);
                    m1.SetStatus(StatusFlags.Targetable, false);
                    m1.IsVisibleByTeam(owner.Team);
                    sec.SetToRemove();
                    sec = spell.CreateSpellSector(new SectorParameters
                    {
                        Length = 15f,
                        Tickrate = 100,
                        CanHitSameTargetConsecutively = true,
                        OverrideFlags = SpellDataFlags.AffectHeroes | SpellDataFlags.AffectEnemies | SpellDataFlags.IgnoreEnemyMinion,
                        //OverrideFlags = SpellDataFlags.AffectEnemies | SpellDataFlags.AffectNeutral | SpellDataFlags.AffectMinions | SpellDataFlags.AffectHeroes,
                        Type = SectorType.Area
                    });
                }
            }
        }

        public void OnSpellChannel(Spell spell)
        {
        }

        public void OnSpellChannelCancel(Spell spell, ChannelingStopSource source)
        {
        }

        public void OnSpellPostChannel(Spell spell)
        {
        }

        public void OnUpdate(float diff)
        {
            //if (procced = true)
            //{
            //    m.TakeDamage(unit, 5000, DamageType.DAMAGE_TYPE_TRUE, DamageSource.DAMAGE_SOURCE_INTERNALRAW, false);
            //}
        }
    }
}