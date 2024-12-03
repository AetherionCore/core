using System.Numerics;
using GameServerCore.Enums;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using LeagueSandbox.GameServer.Scripting.CSharp;
using LeagueSandbox.GameServer.API;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using LeagueSandbox.GameServer.GameObjects.SpellNS;
using LeagueSandbox.GameServer.GameObjects.SpellNS.Missile;
using LeagueSandbox.GameServer.GameObjects.SpellNS.Sector;

namespace Spells
{
    public class JinxR : ISpellScript
    {
        public SpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
        {
            TriggersSpellCasts = true,
            MissileParameters = new MissileParameters()
            {
                Type = MissileType.Circle,
            },
        };

        ObjAIBase _owner;
        public void OnActivate(ObjAIBase owner, Spell spell)
        {
            _owner = owner;
            ApiEventManager.OnSpellHit.AddListener(this, spell, TargetExecute, false);
        }

        private void TargetExecute(Spell spell, AttackableUnit target, SpellMissile missile, SpellSector sector)
        {
            if ((target is Champion))
            {
                AddBuff("JinxWSight", 2f, 1, spell, target, _owner);
                var owner = spell.CastInfo.Owner;
                var ad = owner.Stats.AttackDamage.Total * spell.SpellData.AttackDamageCoefficient;
                var damage = spell.CastInfo.SpellLevel * 10 + ad;

                target.TakeDamage(owner, 100f, DamageType.DAMAGE_TYPE_PHYSICAL, DamageSource.DAMAGE_SOURCE_ATTACK, false);

                //She has several confusing partical names so was using this as a tmp stand in
                ///I think she actually applies several particals to a target but will need too check.
                AddParticleTarget(owner, target, "Ezreal_mysticshot_tar", target);

                missile.SetToRemove();
            }


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
            SetSpellToolTipVar(_owner, 0, _owner.Stats.AttackDamage.Total - _owner.Stats.AttackDamage.BaseValue, SpellbookType.SPELLBOOK_CHAMPION, 3, SpellSlotType.SpellSlots);
        }
    }
}