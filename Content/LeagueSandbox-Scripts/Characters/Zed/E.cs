using GameServerCore.Enums;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using LeagueSandbox.GameServer.Scripting.CSharp;
using System.Numerics;
using LeagueSandbox.GameServer.API;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using LeagueSandbox.GameServer.GameObjects.SpellNS;
using LeagueSandbox.GameServer.GameObjects.SpellNS.Missile;
using LeagueSandbox.GameServer.GameObjects.SpellNS.Sector;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.Buildings;

namespace Spells
{
    public class ZedPBAOEDummy : ISpellScript
    {
        public SpellScriptMetadata ScriptMetadata => new SpellScriptMetadata()
        {
            TriggersSpellCasts = true
            // TODO
        };

        public void OnActivate(ObjAIBase owner, Spell spell)
        {
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
            var owner = spell.CastInfo.Owner;
			var ownerSkinID = owner.SkinID;
			if (ownerSkinID == 0)
            {
                AddParticleTarget(owner, null, "Zed_E_cas.troy", owner);
            }
			
			if (ownerSkinID == 1)
            {
                AddParticleTarget(owner, null, "Zed_Skin01_E_cas.troy", owner);
            }
			if (ownerSkinID == 2)
            {
                AddParticleTarget(owner, null, "Zed_E_cas.troy", owner);
            }
            PlayAnimation(owner, "Spell3", 0.5f);        
            spell.CreateSpellSector(new SectorParameters
            {
                Length = 250f,
                SingleTick = true,
                Type = SectorType.Area
            });
        }

        public void OnSpellPostCast(Spell spell)
        {
        }
        public void TargetExecute(Spell spell, AttackableUnit target, SpellMissile missile, SpellSector sector)
        {
            var owner = spell.CastInfo.Owner;
            if (owner != target)
            {
                var AD = spell.CastInfo.Owner.Stats.AttackDamage.Total * 0.6f;
                var damage = 40 + spell.CastInfo.SpellLevel * 30 + AD;
                target.TakeDamage(owner, damage, DamageType.DAMAGE_TYPE_PHYSICAL, DamageSource.DAMAGE_SOURCE_SPELLAOE, false);
                AddParticleTarget(owner, null, "Zed_E_tar.troy", target);

                owner.GetSpell("ZedShadowDash").LowerCooldown(2);
                if (target.HasBuff("ZedSlow"))
                {
                    AddBuff("ZedSlow", 1.5f, 1, spell, target, owner);
                }
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
    public class ZedPBAOE : ISpellScript
    {
        public SpellScriptMetadata ScriptMetadata => new SpellScriptMetadata()
        {
            TriggersSpellCasts = true,
            IsDamagingSpell = true
            // TODO
        };

        public void OnActivate(ObjAIBase owner, Spell spell)
        {
            ApiEventManager.OnSpellHit.AddListener(this, spell, TargetExecute, false);
        }

        public void OnDeactivate(ObjAIBase owner, Spell spell)
        {
        }

        public void OnSpellPreCast(ObjAIBase owner, Spell spell, AttackableUnit target, Vector2 start, Vector2 end)
        {
            var ownerSkinID = owner.SkinID;
			if (ownerSkinID == 0)
            {
                AddParticleTarget(owner, null, "Zed_E_cas.troy", owner);
            }
			
			if (ownerSkinID == 1)
            {
                AddParticleTarget(owner, null, "Zed_Skin01_E_cas.troy", owner);
            }
			if (ownerSkinID == 2)
            {
                AddParticleTarget(owner, null, "Zed_E_cas.troy", owner);
            }
            PlayAnimation(owner, "Spell3", 0.5f);        
            spell.CreateSpellSector(new SectorParameters
            {
                Length = 250f,
                SingleTick = true,
                Type = SectorType.Area
            });
        }

        public void OnSpellCast(Spell spell)
        {
        }

        public void OnSpellPostCast(Spell spell)
        {
        }
        public void TargetExecute(Spell spell, AttackableUnit target, SpellMissile missile, SpellSector sector)
        {
            var owner = spell.CastInfo.Owner;
            if (owner != target)
            {
                var AD = spell.CastInfo.Owner.Stats.AttackDamage.Total * 0.6f;
                var damage = 40 + spell.CastInfo.SpellLevel * 30 + AD;

                target.TakeDamage(owner, damage, DamageType.DAMAGE_TYPE_PHYSICAL, DamageSource.DAMAGE_SOURCE_SPELLAOE, false);
                AddParticleTarget(owner, null, "Zed_E_tar.troy", target);
                if (!target.HasBuff("ZedSlow"))
                {
                    AddBuff("ZedSlow", 1.5f, 1, spell, target, owner);
                }
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
}
