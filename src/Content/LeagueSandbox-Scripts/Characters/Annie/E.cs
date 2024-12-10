using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using LeagueSandbox.GameServer.Scripting.CSharp;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using LeagueSandbox.GameServer.GameObjects.SpellNS;

﻿namespace Spells
{
    public class MoltenShield : ISpellScript
    {
        public SpellScriptMetadata ScriptMetadata => new SpellScriptMetadata()
        {
            TriggersSpellCasts = true
        };

        public void OnSpellPostCast(Spell spell)
        {
            var owner = spell.CastInfo.Owner;
            AddBuff("Pyromania", 250000f, 1, spell, owner, owner);
            var buff = owner.GetBuffWithName("Pyromania");
            NotifyBuffStacks(buff);

            AddBuff("MoltenShield", 5.0f, 1, spell, owner, owner);
            if (owner is Champion ch)
            {
                Pet tibbers = ch.GetPet();
                if (tibbers != null && !tibbers.IsDead)
                {
                    AddBuff("MoltenShield", 5.0f, 1, spell, tibbers, owner);
                }
            }
        }
    }
}