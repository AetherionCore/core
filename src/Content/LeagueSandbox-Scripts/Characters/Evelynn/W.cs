using GameServerCore.Enums;
using LeagueSandbox.GameServer.API;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using LeagueSandbox.GameServer.GameObjects.SpellNS;
using LeagueSandbox.GameServer.GameObjects.SpellNS.Missile;
using LeagueSandbox.GameServer.GameObjects.SpellNS.Sector;
using LeagueSandbox.GameServer.Scripting.CSharp;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using System.Numerics;
using GameServerCore.Scripting.CSharp;

namespace Spells
{
    //Check for damage enemy reduce cd by 1 for a hit
    //Check for champion takedown and refresh cd
    public class EvelynnW : ISpellScript
    {
        public SpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
        {
            TriggersSpellCasts = true
        };

        private ObjAIBase _owner;

        public void OnActivate(ObjAIBase owner, Spell spell)
        {
            _owner = owner;
            ApiEventManager.OnSpellHit.AddListener(owner,spell,OnSpellHit,false);

        }

        private void OnSpellHit(Spell spell, AttackableUnit unit, SpellMissile missle, SpellSector sector)
        {
            AddBuff("EvelynnWPassive", 3f, 1, spell, spell.CastInfo.Targets[0].Unit, spell.CastInfo.Owner);
        }

        public void OnDeactivate(ObjAIBase owner, Spell spell)
        {
        }

        public void OnSpellPreCast(ObjAIBase owner, Spell spell, AttackableUnit target, Vector2 start, Vector2 end)
        {
        }

        public void OnSpellCast(Spell spell)
        {
            if (_owner.GetBuffWithName("Slow") != null) 
            {
                _owner.PlayAnimation("Spell2", .4f);
                _owner.RemoveBuffsWithName("Slow");
            }
        }

        public void OnSpellPostCast(Spell spell)
        {
            //Apply Ghosted
            AddBuff("EvelynnWActive", 3f, 1, spell, spell.CastInfo.Targets[0].Unit, spell.CastInfo.Owner);

            var manaCost = new[] { 0, 0, 0, 0, 0 }[spell.CastInfo.SpellLevel - 1];
            var coolDown = new[] { 15f, 15f, 15f, 15f, 15f }[spell.CastInfo.SpellLevel - 1];
            spell.SetCooldown(coolDown);
            spell.CastInfo.Owner.Stats.CurrentMana -= manaCost;
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
