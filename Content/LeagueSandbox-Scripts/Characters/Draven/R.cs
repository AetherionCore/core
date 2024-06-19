using GameServerCore.Enums;
using LeagueSandbox.GameServer.Scripting.CSharp;
using LeagueSandbox.GameServer.API;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using LeagueSandbox.GameServer.GameObjects.SpellNS;
using System.Numerics;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using LeagueSandbox.GameServer.GameObjects.SpellNS.Missile;
using LeagueSandbox.GameServer.GameObjects.SpellNS.Sector;
using System.Collections.Generic;
using GameServerCore;

namespace Spells
{
    public class DravenRCast : ISpellScript
    {
        public SpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
        {
            TriggersSpellCasts = true,
            // TODO
        };

        public void OnActivate(ObjAIBase owner, Spell spell)
        {
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
        static internal bool toggle = true;
        internal static SpellMissile mis;
        public void OnSpellPostCast(Spell spell)
        {
            toggle = !toggle;
            if (toggle == false)
            {
                var endPos = GetPointFromUnit(spell.CastInfo.Owner, 925);
                SpellCast(spell.CastInfo.Owner, 1, SpellSlotType.ExtraSlots, endPos, endPos, false, Vector2.Zero);
                spell.SetCooldown(0.0f, true);
            }
            if (toggle == true)
            {
                var misPos = mis.Position;
                mis.SetToRemove();
                SpellCast(spell.CastInfo.Owner, 1, SpellSlotType.ExtraSlots, spell.CastInfo.Owner.Position, spell.CastInfo.Owner.Position, true, misPos);
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
        }
    }

    public class DravenR : ISpellScript
    {
        public SpellScriptMetadata ScriptMetadata => new SpellScriptMetadata()
        {
            TriggersSpellCasts = true,
            MissileParameters = new MissileParameters
            {
                Type = MissileType.Circle
            }
            // TODO
        };
        ObjAIBase _owner;
        public void OnActivate(ObjAIBase owner, Spell spell)
        {
            _owner = owner;
            ApiEventManager.OnSpellHit.AddListener(this, spell, TargetExecute, false);
            ApiEventManager.OnLaunchMissileByAnother.AddListener(this, owner, CastSpell, false);
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
        }

        // Add a method with the correct signature for the event listener
        public void CastSpell(AttackableUnit unit1, AttackableUnit unit2)
        {
            if (unit1 is ObjAIBase owner && unit2 is ObjAIBase targetOwner)
            {
                // Retrieve the corresponding spell instance
                var spell = targetOwner.GetSpell("DravenR");
                if (spell != null)
                {
                    CastSpell(owner, spell, DravenRCast.mis);
                }
            }
        }

        // Adjust the signature of CastSpell method to match the expected delegate
        public void CastSpell(ObjAIBase owner, Spell spell, SpellMissile missile)
        {
            DravenRCast.mis = missile;
        }

        public void TargetExecute(Spell spell, AttackableUnit target, SpellMissile missile, SpellSector sector)
        {
            var owner = spell.CastInfo.Owner;
            var ad = owner.Stats.AttackDamage.Total * 1.1f + (spell.CastInfo.Owner.GetSpell("DravenDoubleShot").CastInfo.SpellLevel * 100) + 40;
            target.TakeDamage(owner, (float)ad, DamageType.DAMAGE_TYPE_PHYSICAL, DamageSource.DAMAGE_SOURCE_SPELL, false);
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
            if (DravenRCast.mis != null)
            {
                if (DravenRCast.toggle == true)
                {
                    if (Extensions.IsVectorWithinRange(_owner.Position, DravenRCast.mis.Position, 100))
                    {
                        DravenRCast.mis.SetToRemove();
                    }
                }
            }
        }
    }
}
