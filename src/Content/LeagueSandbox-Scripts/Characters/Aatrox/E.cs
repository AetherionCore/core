using GameServerCore.Enums;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using LeagueSandbox.GameServer.Scripting.CSharp;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using LeagueSandbox.GameServer.GameObjects.SpellNS;
using LeagueSandbox.GameServer.API;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.GameObjects.SpellNS.Missile;
using LeagueSandbox.GameServer.GameObjects.SpellNS.Sector;
using System.Collections.Generic;
using System.Numerics;

namespace Spells
{
    public class AatroxE : ISpellScript
    {
        public SpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
        {
            MissileParameters = new MissileParameters
            {
                Type = MissileType.Circle
            },
                TriggersSpellCasts = true,
                IsDamagingSpell = true
        };

        public void OnActivate(ObjAIBase owner, Spell spell)
        {
        }

        public void OnDeactivate(ObjAIBase owner, Spell spell)
        {
        }
        public void OnSpellPreCast(ObjAIBase owner, Spell spell, AttackableUnit target, Vector2 start, Vector2 end)
        {
            if (owner.HasBuff("AatroxR"))
            {
                OverrideAnimation(owner, "Spell3_ULT", "Spell3");
            }
            else
            {
                OverrideAnimation(owner, "Spell3", "Spell3_ULT");
            }
        }

        public void OnSpellCast(Spell spell)
        {
            var owner = spell.CastInfo.Owner;
            var Blood = owner.Stats.CurrentHealth * 0.05f;
            owner.Stats.CurrentMana += Blood;
            owner.TakeDamage(owner, Blood, DamageType.DAMAGE_TYPE_TRUE, DamageSource.DAMAGE_SOURCE_PERIODIC, false);

        }

        public void OnSpellPostCast(Spell spell)
        {
            var owner = spell.CastInfo.Owner;
            //if (owner.HasBuff("AatroxR"))
            //{
            //SetAnimStates(owner, new Dictionary<string, string> { { "Spell3", "Spell3_ULT" } });
            //}
            AddParticleTarget(owner, owner, "Aatrox_Base_E_Glow.troy", owner, bone: "HEAD");
            var ownerSkinID = owner.SkinID;
            for (int bladeCount = 0; bladeCount <= 0; bladeCount++)
            {
                var LPos = GetPointFromUnit(owner, 175f, -35f);
                var RPos = GetPointFromUnit(owner, 175f, 35f);
                var targetPos = GetPointFromUnit(owner, 1200.0f);
                SpellCast(spell.CastInfo.Owner, 1, SpellSlotType.ExtraSlots, targetPos, Vector2.Zero, true, LPos);
                SpellCast(spell.CastInfo.Owner, 1, SpellSlotType.ExtraSlots, targetPos, Vector2.Zero, true, RPos);

                SpellCast(owner, 0, SpellSlotType.ExtraSlots, targetPos, targetPos, true, Vector2.Zero);
            }
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
    public class AatroxEConeMissile2 : ISpellScript
    {
        public SpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
        {
            MissileParameters = new MissileParameters
            {
                Type = MissileType.Circle
            },
            IsDamagingSpell = true,
            TriggersSpellCasts = true

            // TODO
        };
        public List<AttackableUnit> UnitsHit = new List<AttackableUnit>();

        public void OnActivate(ObjAIBase owner, Spell spell)
        {
            ApiEventManager.OnSpellHit.AddListener(this, spell, TargetExecute, false);
        }

        public void OnDeactivate(ObjAIBase owner, Spell spell)
        {
        }
        public void OnSpellPreCast(ObjAIBase owner, Spell spell, AttackableUnit target, Vector2 start, Vector2 end)
        {
            UnitsHit.Clear();
        }
        public void OnMissileEnd(SpellMissile missile)
        {
        }

        public void TargetExecute(Spell spell, AttackableUnit target, SpellMissile missile, SpellSector sector)
        {
            var owner = spell.CastInfo.Owner;
            float ap = owner.Stats.AbilityPower.Total * 0.6f;
            float ad = owner.Stats.AttackDamage.Total * 0.6f;
            float t = 1.75f + (spell.CastInfo.SpellLevel - 1) * 0.25f;
            float damage = 75 + (spell.CastInfo.SpellLevel - 1) * 35 + ad + ap;
            target.TakeDamage(owner, damage, DamageType.DAMAGE_TYPE_MAGICAL, DamageSource.DAMAGE_SOURCE_ATTACK, false);
            AddBuff("AatroxESlow", t, 1, spell, target, owner);
            AddParticleTarget(owner, target, "Aatrox_Base_EMissile_Hit.troy", target);
        }

        public void OnSpellCast(Spell spell)
        {
        }

        public void OnSpellPostCast(Spell spell)
        {
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
    public class AatroxEConeMissile : ISpellScript
    {
        public SpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
        {
            MissileParameters = new MissileParameters
            {
                Type = MissileType.Circle
            },
            IsDamagingSpell = true,
            TriggersSpellCasts = true

            // TODO
        };
        public List<AttackableUnit> UnitsHit = new List<AttackableUnit>();

        public void OnActivate(ObjAIBase owner, Spell spell)
        {
            ApiEventManager.OnSpellHit.AddListener(this, spell, TargetExecute, false);
        }

        public void OnDeactivate(ObjAIBase owner, Spell spell)
        {
        }
        public void OnSpellPreCast(ObjAIBase owner, Spell spell, AttackableUnit target, Vector2 start, Vector2 end)
        {
            UnitsHit.Clear();
        }
        public void OnMissileEnd(SpellMissile missile)
        {
        }

        public void TargetExecute(Spell spell, AttackableUnit target, SpellMissile missile, SpellSector sector)
        {
            var owner = spell.CastInfo.Owner;
            float ap = owner.Stats.AbilityPower.Total * 0.6f;
            float ad = owner.Stats.AttackDamage.Total * 0.6f;
            float t = 1.75f + (spell.CastInfo.SpellLevel - 1) * 0.25f;
            float damage = 75 + (spell.CastInfo.SpellLevel - 1) * 35 + ad + ap;
            target.TakeDamage(owner, damage, DamageType.DAMAGE_TYPE_MAGICAL, DamageSource.DAMAGE_SOURCE_ATTACK, false);
            AddBuff("AatroxESlow", t, 1, spell, target, owner);
            AddParticleTarget(owner, target, "Aatrox_Base_EMissile_Hit.troy", target);
        }

        public void OnSpellCast(Spell spell)
        {
        }

        public void OnSpellPostCast(Spell spell)
        {
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