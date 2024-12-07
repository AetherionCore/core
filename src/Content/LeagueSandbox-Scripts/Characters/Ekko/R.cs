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
using LeaguePackets.Game.Events;

namespace Spells
{
    public class EkkoR : ISpellScript
    {
        Buff Buff;
        public SpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
        {
            TriggersSpellCasts = true
        };

        public void OnActivate(ObjAIBase owner, Spell spell)
        {
            ApiEventManager.OnLevelUpSpell.AddListener(this, spell, OnLevelUp, true);
        }
        public void OnLevelUp(Spell spell)
        {
            var owner = spell.CastInfo.Owner as Champion;
            Buff = AddBuff("EkkoRMinion", 250000f, 1, spell, owner, owner);
            CreateTimer(0.1f, () =>
            {
                ApiEventManager.OnLevelUpSpell.RemoveListener(this);
            });
        }

        public void OnSpellPreCast(ObjAIBase owner, Spell spell, AttackableUnit target, Vector2 start, Vector2 end)
        {
            //Ekko = (Buff.BuffScript as Buffs.EkkoRMinion).EkkoSpawn();
        }

        public void OnSpellCast(Spell spell)
        {
            var owner = spell.CastInfo.Owner;
            AddBuff("EkkoR", 0.5f, 1, spell, owner, owner);
        }

        public void OnSpellPostCast(Spell spell)
        {
            var owner = spell.CastInfo.Owner;
        }
    }
}