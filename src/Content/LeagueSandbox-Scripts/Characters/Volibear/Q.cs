
using GameServerCore.Enums;
using GameServerCore.Scripting.CSharp;
using GameServerLib.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.API;
using LeagueSandbox.GameServer.Content;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.Buildings.AnimatedBuildings;
using LeagueSandbox.GameServer.GameObjects.SpellNS;
using LeagueSandbox.GameServer.Scripting.CSharp;
using System;
using System.Numerics;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;

namespace Spells
{
    public class VolibearQ : ISpellScript
    {
        public SpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
        {
            TriggersSpellCasts = true,
            NotSingleTargetSpell = true
            // TODO
        };

        public void OnActivate(ObjAIBase owner, Spell spell)
        {
            ApiEventManager.OnPreAttack.AddListener(this, owner, ChangeAnim, false);
        }

        public void ChangeAnim(Spell spell)
        {
            if (VolibearQAttack.Applied == 0)
            {
                LogInfo($"Applying Spell Now!");
                spell.CastInfo.Owner.PlayAnimation("Spell2", 0.5f, flags: AnimationFlags.Override);
                //CreateTimer(0.5f, () => { spell.CastInfo.Owner.StopAnimation("Spell1", fade: true); });
            }
        }

        public void OnDeactivate(ObjAIBase owner, Spell spell)
        {
        }

        /*public void OnSpellPreCast(ObjAIBase owner, Spell spell, AttackableUnit target, Vector2 start, Vector2 end)
        {
            AddBuff("VolibearQ", 6.0f, 1, spell, owner, owner);
        }*/

        public void OnSpellCast(Spell spell)
        {
            var owner = spell.CastInfo.Owner;
            PlayAnimation(owner, "Spell1", 0.5f);
            owner.CancelAutoAttack(true);
            AddBuff("VolibearQ", 4.0f, 1, spell, owner, owner);
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

    public class VolibearQAttack : ISpellScript
    {
        public SpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
        {
            TriggersSpellCasts = true,
            NotSingleTargetSpell = true,
            IsDamagingSpell = true,
        };

        private Spell originspell;
        private ObjAIBase ownermain;
        internal static int Applied = 1;

        public void OnActivate(ObjAIBase owner, Spell spell)
        {
            originspell = spell;
            ownermain = owner;
            ApiEventManager.OnHitUnit.AddListener(this, owner, TargetExecute, false);
        }

        public void TargetExecute(DamageData data)
        {
            var owner = ownermain;
            //owner.PlayAnimation("Spell2", 0.5f, flags: AnimationFlags.Override);

            var ADratio = owner.Stats.AttackDamage.PercentBonus * 0.3f;
            var damage = 40f + (30f * (originspell.CastInfo.SpellLevel - 1)) + ADratio;
            if (Applied != 1)
            {
                var unit = originspell.CastInfo.Targets[0].Unit;
                if (!(unit is BaseTurret or Inhibitor or Nexus))
                {
                    unit.TakeDamage(owner, damage, DamageType.DAMAGE_TYPE_PHYSICAL, DamageSource.DAMAGE_SOURCE_SPELL, false);

                    if (!unit.Status.HasFlag(StatusFlags.Immovable))
                    {
                        var x = GetPointFromUnit(owner, -200);
                        ForceMovement(unit, "Spell1", x, 500, 0, 20, 0, movementOrdersType: ForceMovementOrdersType.CANCEL_ORDER);
                        var xy = unit as ObjAIBase;
                        xy.SetTargetUnit(null);
                    }
                }
                Applied = 1;
                //CreateTimer((float)6, () => { Applied = 1; });
            }
        }

        public void OnDeactivate(ObjAIBase owner, Spell spell)
        {
        }

        public void OnSpellPreCast(ObjAIBase owner, Spell spell, AttackableUnit target, Vector2 start, Vector2 end)
        {
            Applied = 0;
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