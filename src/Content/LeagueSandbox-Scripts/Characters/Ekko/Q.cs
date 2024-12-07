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

namespace Spells
{
    public class EkkoQ : ISpellScript
    {
        ObjAIBase Owner;

        public SpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
        {
            TriggersSpellCasts = true,
            IsDamagingSpell = true
        };

        public static Vector2 end;


        public void OnSpellCast(Spell spell)
        {
            var owner = spell.CastInfo.Owner;

        }

        public void OnSpellPostCast(Spell spell)
        {
            var owner = spell.CastInfo.Owner;
            var ownerSkinID = owner.SkinID;
            var start = GetPointFromUnit(owner, 25f);
            var end = GetPointFromUnit(owner, 700f);
            FaceDirection(end, owner);
            //SpellCast(owner, 0, SpellSlotType.ExtraSlots, end, Vector2.Zero, true, start);
            SpellCast(owner, 0, SpellSlotType.ExtraSlots, end, end, true, Vector2.Zero);
        }
    }

    public class EkkoQMis : ISpellScript
    {
        public SpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
        {
            IsDamagingSpell = true,
            TriggersSpellCasts = true

            // TODO
        };

        SpellMissile m;
        Spell Spell;

        public static List<AttackableUnit> UnitsHit = new List<AttackableUnit>();


        public void OnSpellPreCast(ObjAIBase owner, Spell spell, AttackableUnit target, Vector2 start, Vector2 end)
        {
            UnitsHit.Clear();
            Spell = spell;
            var missile = spell.CreateSpellMissile(new MissileParameters
            {
                Type = MissileType.Circle,
                OverrideEndPosition = end
            });

            //CreateTimer((float) 0.25f , () =>{AddParticleTarget(owner, missile, "Ekko_Base_Q_Aoe_Dilation", missile);	;});			  
            ApiEventManager.OnSpellMissileEnd.AddListener(this, missile, OnMissileEnd, false);
            ApiEventManager.OnSpellHit.AddListener(this, spell, TargetExecute, true);
        }

        public void TargetExecute(Spell spell, AttackableUnit target, SpellMissile missile, SpellSector sector)
        {
            m = missile;
            if (!UnitsHit.Contains(target))
            {
                UnitsHit.Add(target);
                var owner = spell.CastInfo.Owner;
                float ap = owner.Stats.AbilityPower.Total;
                float damage = 40 + (spell.CastInfo.SpellLevel - 1) * 20 + ap;
                if (!target.HasBuff("EkkoPassiveSlow"))
                {
                    AddBuff("EkkoPassive", 6f, 1, spell, target, owner);
                }
                else
                {
                }
                target.TakeDamage(owner, damage, DamageType.DAMAGE_TYPE_MAGICAL, DamageSource.DAMAGE_SOURCE_ATTACK, false);
                AddParticleTarget(owner, target, "Ekko_Base_Q_Mis_Tar", target);
                if (target is Champion)
                {
                    //ApiEventManager.OnSpellMissileEnd.RemoveListener(this);				
                    missile.SetToRemove();
                }
            }
        }

        public void OnMissileEnd(SpellMissile missile)
        {
            var owner = missile.CastInfo.Owner;
            SpellCast(owner, 5, SpellSlotType.ExtraSlots, GetPointFromUnit(missile, 375), Vector2.Zero, true, missile.Position);
        }

        public void Slow(Spell spell)
        {
            var owner = spell.CastInfo.Owner;
            //AddBuff("", 2.24f, 1, spell, Time, Time, false);
            //AddParticlePos(owner, "Ekko_Base_Q_Aoe_Dilation.troy",missile.Position, missile.Position, 1);
            //AddParticle(owner, missile, "Ekko_Base_Q_Aoe_Resolve", missile.Position);
            SpellCast(owner, 5, SpellSlotType.ExtraSlots, GetPointFromUnit(m, 375), Vector2.Zero, true, m.Position);
            //SpellCast(owner, 2, SpellSlotType.ExtraSlots, owner.Position, owner.Position, true, missile.Position);
        }
    }

    public class EkkoQReturnDead : ISpellScript
    {
        public SpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
        {
            IsDamagingSpell = true,
            TriggersSpellCasts = true

            // TODO
        };

        Particle P;

        public List<AttackableUnit> UnitsHit = Spells.EkkoQMis.UnitsHit;

        public void OnActivate(ObjAIBase owner, Spell spell)
        {
            ApiEventManager.OnSpellHit.AddListener(this, spell, TargetExecute, false);
        }

        public void OnSpellPreCast(ObjAIBase owner, Spell spell, AttackableUnit target, Vector2 start, Vector2 end)
        {
            var missile = spell.CreateSpellMissile(new MissileParameters
            {
                Type = MissileType.Circle,
                OverrideEndPosition = end
            });
            if (missile != null)
            {
                AddParticleTarget(owner, missile, "Ekko_Base_Q_Aoe_Dilation", missile, 25000, 0.85f);
                //P = AddParticle(owner, owner, "Ekko_Base_Q_Mis_Throw.troy", owner.Position);
                //P = AddParticle(owner, missile, "Ekko_Base_Q_Mis_02.troy", missile.Position,lifetime : 0f);
            }
            //if (owner.SkinID == 11 ){AddParticleTarget(owner, missile, "Ekko_Skin11_Q_Mis_03", missile);}			
            CreateTimer((float)1.9f, () => { if (missile != null) { AddParticleTarget(owner, missile, "Ekko_Base_Q_Aoe_Resolve", missile); } });
            ApiEventManager.OnSpellMissileEnd.AddListener(this, missile, OnMissileEnd, true);
        }

        public void TargetExecute(Spell spell, AttackableUnit target, SpellMissile missile, SpellSector sector)
        {
            if (!UnitsHit.Contains(target))
            {
                UnitsHit.Add(target);
                var owner = spell.CastInfo.Owner;
                var ownerSkinID = owner.SkinID;
                float ap = owner.Stats.AbilityPower.Total;
                float damage = 40 + (spell.CastInfo.SpellLevel - 1) * 20 + ap;
                if (!target.HasBuff("EkkoPassiveSlow"))
                {
                    AddBuff("EkkoPassive", 6f, 1, spell, target, owner);
                }
                else
                {
                }
                target.TakeDamage(owner, damage, DamageType.DAMAGE_TYPE_MAGICAL, DamageSource.DAMAGE_SOURCE_ATTACK, false);
                AddParticleTarget(owner, target, "Ekko_Base_Q_Mis_Tar", target);
            }
        }

        public void OnMissileEnd(SpellMissile missile)
        {
            //RemoveParticle(P);
            var owner = missile.CastInfo.Owner;
            Particle M = AddParticlePos(owner, "", missile.Position, missile.Position);
            //AddBuff("", 2.24f, 1, spell, Time, Time, false);
            //AddParticlePos(owner, "Ekko_Base_Q_Aoe_Dilation.troy",missile.Position, missile.Position, 1);
            SpellCast(owner, 1, SpellSlotType.ExtraSlots, true, owner, M.Position);
            //SpellCast(owner, 2, SpellSlotType.ExtraSlots, owner.Position, owner.Position, true, missile.Position);
        }
    }

    public class EkkoQReturn : ISpellScript
    {
        public SpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
        {
            MissileParameters = new MissileParameters
            {
                Type = MissileType.Target
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

        public void OnSpellPreCast(ObjAIBase owner, Spell spell, AttackableUnit target, Vector2 start, Vector2 end)
        {
            UnitsHit.Clear();
            var missile = spell.CreateSpellMissile(new MissileParameters
            {
                Type = MissileType.Circle,
                OverrideEndPosition = end
            });
            ApiEventManager.OnSpellMissileEnd.AddListener(this, missile, OnMissileEnd, true);
            //AddParticle(owner, missile, "Ekko_Base_Q_Aoe_Resolve", missile.Position);
        }

        public void TargetExecute(Spell spell, AttackableUnit target, SpellMissile missile, SpellSector sector)
        {
            if (target == missile.CastInfo.Owner)
            {
                missile.SetToRemove();
            }
            var owner = spell.CastInfo.Owner;
            var spellLevel = owner.GetSpell("EkkoQ").CastInfo.SpellLevel;
            var APratio = owner.Stats.AbilityPower.Total * 0.6f;
            var damage = 50 + 25f * (spellLevel - 1) + APratio;
            if (!UnitsHit.Contains(target))
            {
                if (target.Team != owner.Team && !(target is ObjAIBase || target is BaseTurret))
                {
                    UnitsHit.Add(target);
                    target.TakeDamage(owner, damage, DamageType.DAMAGE_TYPE_MAGICAL, DamageSource.DAMAGE_SOURCE_ATTACK, false);
                    if (!target.HasBuff("EkkoPassiveSlow"))
                    {
                        AddBuff("EkkoPassive", 6f, 1, spell, target, owner);
                    }
                    else
                    {
                    }
                    AddParticleTarget(owner, target, "Ekko_Base_Q_Mis_Return_Tar.troy", target, 1f);
                    AddParticleTarget(owner, target, "Ekko_Base_Q_Mis_Return_Hit_Sound.troy", target, 1f);
                }
            }
        }

        public void OnMissileEnd(SpellMissile missile)
        {
            var owner = missile.CastInfo.Owner;
            PlayAnimation(owner, "Spell1_Catch", 0.8f);
        }
    }
}