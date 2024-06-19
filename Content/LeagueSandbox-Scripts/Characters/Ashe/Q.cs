using GameServerCore.Enums;
using LeagueSandbox.GameServer.Scripting.CSharp;
using LeagueSandbox.GameServer.API;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using LeagueSandbox.GameServer.GameObjects.SpellNS;
using System.Numerics;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using LeagueSandbox.GameServer.GameObjects;

namespace Spells
{
    public class FrostShot : ISpellScript
    {
        public SpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
        {
            TriggersSpellCasts = true,
            CastingBreaksStealth = true,
            DoesntBreakShields = true,
            IsDamagingSpell = true,
            NotSingleTargetSpell = true,
            SpellToggleSlot = 4
        };

        private Buff thisBuff;
        ObjAIBase _owner;
        public void OnActivate(ObjAIBase owner, Spell spell)
        {
            _owner = owner;
            //ApiEventManager.OnHitUnitByAnother.AddListener(this, owner, TargetExecute, false);
        }

        private void TargetExecute(AttackableUnit unit, bool crit)
        {
            if (!_owner.HasBuff(thisBuff))
            {
                return;
            }
            LogDebug("has buff");
            AddBuff("AsheQ", 2.0f, 1, _owner.GetSpell("FrostShot"), unit, _owner);
            _owner.Stats.CurrentMana -= 8;
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
            var owner = spell.CastInfo.Owner;

            if (owner.HasBuff("FrostShot"))
            {
                owner.RemoveBuffsWithName("FrostShot");
            }
            else
            {
                thisBuff = AddBuff("FrostShot", float.MaxValue, 1, spell, owner, owner, true);
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
}