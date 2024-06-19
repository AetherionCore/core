using GameServerCore.Enums;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.API;
using LeagueSandbox.GameServer.Scripting.CSharp;
using System.Numerics;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using LeagueSandbox.GameServer.GameObjects.SpellNS;
using LeagueSandbox.GameServer.GameObjects.SpellNS.Missile;
using LeagueSandbox.GameServer.GameObjects.SpellNS.Sector;

namespace Spells
{
    public class DariusNoxianTacticsONH : ISpellScript
    {
        private ObjAIBase Owner;

        public SpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
        {
            TriggersSpellCasts = true,
        };

        public void OnActivate(ObjAIBase owner, Spell spell)
        {
            Owner = owner;
            ApiEventManager.OnPreAttack.AddListener(this, owner, ChangeAnim, false);
            //ApiEventManager.OnHitUnitByAnother.AddListener(this, owner, TargetExecute, false);
        }

        public void ChangeAnim(Spell spell)
        {
            if (Applied == 0)
            {
                spell.CastInfo.Owner.PlayAnimation("Spell2", 1.5f, flags: AnimationFlags.Override);
                CreateTimer(0.5f, () => { spell.CastInfo.Owner.StopAnimation("Spell2", fade: true); });
            }
        }

        public void OnDeactivate(ObjAIBase owner, Spell spell)
        {
        }

        public void OnSpellPreCast(ObjAIBase owner, Spell spell, AttackableUnit target, Vector2 start, Vector2 end)
        {
            Applied = 0;
        }

        internal static int Applied = 1;

        public void OnSpellCast(Spell spell)
        {
            var owner = spell.CastInfo.Owner;
            AddBuff("DariusNoxianTacticsActive", 4f, 1, spell, Owner, Owner, false);
        }

        public void TargetExecute(AttackableUnit unit, bool arg2)
        {
            var owner = Owner;
            var ADratio = owner.Stats.AttackDamage.PercentBonus * 0.3f;
            if (Applied != 1)
            {
                //unit.TakeDamage(owner, damage, DamageType.DAMAGE_TYPE_PHYSICAL, DamageSource.DAMAGE_SOURCE_SPELL, false);
                Applied = 1;
                //CreateTimer((float)6, () => { Applied = 1; });
            }
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