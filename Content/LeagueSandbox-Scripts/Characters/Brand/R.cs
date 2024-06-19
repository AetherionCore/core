using GameServerCore.Enums;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using LeagueSandbox.GameServer.Scripting.CSharp;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using LeagueSandbox.GameServer.GameObjects.SpellNS;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;
using System.Numerics;
using LeagueSandbox.GameServer.API;
using LeagueSandbox.GameServer.GameObjects.SpellNS.Missile;
using LeagueSandbox.GameServer.GameObjects.SpellNS.Sector;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.Buildings;
using System;

namespace Spells
{
    public class BrandWildfire : ISpellScript
    {
        public SpellScriptMetadata ScriptMetadata => new SpellScriptMetadata()
        {
            CastingBreaksStealth = true,
            DoesntBreakShields = true,
            TriggersSpellCasts = true,
            IsDamagingSpell = true,
            NotSingleTargetSpell = false,
            SpellDamageRatio = 1.0f,
            SpellFXOverrideSkins = new string[] { "FrostFireBrand" },
            MissileParameters = new MissileParameters
            {
                Type = MissileType.Target
            }
        };

        private bool doOnce = false;

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
        }

        public void OnSpellPostCast(Spell spell)
        {
        }

        public void TargetExecute(Spell spell, AttackableUnit target, SpellMissile missile, SpellSector sector)
        {
            var owner = spell.CastInfo.Owner;
            var ap = owner.Stats.AbilityPower.Total * 0.5f;
            var damage = 50f + spell.CastInfo.SpellLevel * 100f + ap;

            //RemoveBuff(owner, "BrandWildfire");

            if (target is ObjAIBase aiTarget)
            {
                //AddBuff("BrandWildfire", 4.0f, 1, spell, owner, aiTarget);

                doOnce = false;

                var units = GetUnitsInRange(target.Position, 600.0f, true).FindAll(unit => unit != target && unit.Team != owner.Team && !(unit is ObjBuilding || unit is BaseTurret));

                // Randomize units list.
                Random rng = new Random();
                int n = units.Count;
                while (n > 1)
                {
                    n--;
                    int k = rng.Next(n + 1);
                    AttackableUnit value = units[k];
                    units[k] = units[n];
                    units[n] = value;
                }

                foreach (var unit in units)
                {
                    if (unit == target || unit.Team == owner.Team || unit is ObjBuilding || unit is BaseTurret)
                    {
                        continue;
                    }

                    if (!doOnce)
                    {
                        if (!unit.Status.HasFlag(StatusFlags.Stealthed))
                        {
                            if (HasBuff(target, "BrandWildfire"))
                            {
                                // Extra spell 5 no longer exists?
                                //SpellCast(owner, 4, SpellSlotType.ExtraSlots, true, unit, target.Position);
                            }
                            else
                            {
                                SpellCast(owner, 1, SpellSlotType.ExtraSlots, true, unit, target.Position);
                            }

                            doOnce = true;
                        }
                        else
                        {
                            if (unit.IsVisibleByTeam(owner.Team))
                            {
                                if (HasBuff(target, "BrandWildfire"))
                                {
                                    // Extra spell 5 no longer exists?
                                    //SpellCast(owner, 4, SpellSlotType.ExtraSlots, true, unit, target.Position);
                                }
                                else
                                {
                                    SpellCast(owner, 1, SpellSlotType.ExtraSlots, true, unit, target.Position);
                                }

                                doOnce = true;
                            }
                        }
                    }
                }

                if (HasBuff(target, "BrandWildfire"))
                {
                    if (owner.SkinID == 3)
                    {
                        AddParticleTarget(owner, target, "BrandConflagration_tar_frost.troy", target, flags: 0);
                    }
                    else
                    {
                        AddParticleTarget(owner, target, "BrandConflagration_tar.troy", target, flags: 0);
                    }

                    // BreakSpellShields(target);

                    AddBuff("BrandWildfire", 4.0f, 1, spell, target, owner);

                    target.TakeDamage(owner, damage, DamageType.DAMAGE_TYPE_MAGICAL, DamageSource.DAMAGE_SOURCE_SPELLAOE, false);
                }
                else
                {
                    // BreakSpellShields(target);

                    AddBuff("BrandWildfire", 4.0f, 1, spell, target, owner);

                    target.TakeDamage(owner, damage, DamageType.DAMAGE_TYPE_MAGICAL, DamageSource.DAMAGE_SOURCE_SPELLAOE, false);

                    if (owner.SkinID == 3)
                    {
                        AddParticleTarget(owner, target, "BrandConflagration_tar_frost.troy", target, flags: 0);
                    }
                    else
                    {
                        AddParticleTarget(owner, target, "BrandConflagration_tar.troy", target, flags: 0);
                    }
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
        }
    }

    public class BrandWildfireMissile : ISpellScript
    {
        public SpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
        {
            DoesntBreakShields = true,
            TriggersSpellCasts = true,
            IsDamagingSpell = true,
            NotSingleTargetSpell = false,
            PersistsThroughDeath = true,
            SpellDamageRatio = 1.0f,
            SpellFXOverrideSkins = new string[] { "FrostFireBrand" },
            MissileParameters = new MissileParameters
            {
                Type = MissileType.Target,
            }
        };

        private bool doOnce = false;

        public void OnActivate(ObjAIBase owner, Spell spell)
        {
        }

        public void OnDeactivate(ObjAIBase owner, Spell spell)
        {
        }

        public void OnSpellPreCast(ObjAIBase owner, Spell spell, AttackableUnit target, Vector2 start, Vector2 end)
        {
            LogInfo("BrandWildfireMissile OnSpellPreCast called.");
            ApiEventManager.OnSpellHit.AddListener(this, spell, TargetExecute, true);
        }

        public void TargetExecute(Spell spell, AttackableUnit target, SpellMissile missile, SpellSector sector)
        {
            LogInfo("BrandWildfireMissile TargetExecute called.");
            var owner = spell.CastInfo.Owner;
            var ap = owner.Stats.AbilityPower.Total * 0.5f;
            var damage = 50f + spell.CastInfo.SpellLevel * 100f + ap;

            AddBuff("CrashFixStacks", 4.0f, 1, spell, owner, owner);

            doOnce = false;

            if (owner.GetBuffWithName("CrashFixStacks").StackCount <= 5)
            {
                var units = GetUnitsInRange(target.Position, 600.0f, true).FindAll(unit => unit != target && unit.Team != owner.Team && !(unit is ObjBuilding || unit is BaseTurret));

                // Randomize units list.
                Random rng = new Random();
                int n = units.Count;
                while (n > 1)
                {
                    n--;
                    int k = rng.Next(n + 1);
                    AttackableUnit value = units[k];
                    units[k] = units[n];
                    units[n] = value;
                }

                foreach (var unit in units)
                {
                    if (unit == target || unit.Team == owner.Team || unit is ObjBuilding || unit is BaseTurret)
                    {
                        continue;
                    }

                    if (!doOnce)
                    {
                        if (!unit.Status.HasFlag(StatusFlags.Stealthed))
                        {
                            if (HasBuff(target, "BrandWildfire"))
                            {
                                // Extra spell 5 no longer exists?
                                //SpellCast(owner, 4, SpellSlotType.ExtraSlots, true, unit, target.Position);
                            }
                            else
                            {
                                SpellCast(owner, 1, SpellSlotType.ExtraSlots, true, unit, target.Position);
                            }

                            doOnce = true;
                        }
                        else
                        {
                            if (unit.IsVisibleByTeam(owner.Team))
                            {
                                if (HasBuff(target, "BrandWildfire"))
                                {
                                    // Extra spell 5 no longer exists?
                                    //SpellCast(owner, 4, SpellSlotType.ExtraSlots, true, unit, target.Position);
                                }
                                else
                                {
                                    SpellCast(owner, 1, SpellSlotType.ExtraSlots, true, unit, target.Position);
                                }

                                doOnce = true;
                            }
                        }
                    }
                }
            }

            if (HasBuff(target, "BrandWildfire"))
            {
                if (owner.SkinID == 3)
                {
                    AddParticleTarget(owner, target, "BrandConflagration_tar_frost.troy", target, flags: 0);
                }
                else
                {
                    AddParticleTarget(owner, target, "BrandConflagration_tar.troy", target, flags: 0);
                }

                // BreakSpellShields(target);

                AddBuff("BrandWildfire", 4.0f, 1, spell, target, owner);

                target.TakeDamage(owner, damage, DamageType.DAMAGE_TYPE_MAGICAL, DamageSource.DAMAGE_SOURCE_SPELLAOE, false);
            }
            else
            {
                // BreakSpellShields(target);

                AddBuff("BrandWildfire", 4.0f, 1, spell, target, owner);

                target.TakeDamage(owner, damage, DamageType.DAMAGE_TYPE_MAGICAL, DamageSource.DAMAGE_SOURCE_SPELLAOE, false);

                if (owner.SkinID == 3)
                {
                    AddParticleTarget(owner, target, "BrandConflagration_tar_frost.troy", target, flags: 0);
                }
                else
                {
                    AddParticleTarget(owner, target, "BrandConflagration_tar.troy", target, flags: 0);
                }
            }
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
}