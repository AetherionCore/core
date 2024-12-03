﻿using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using LeagueSandbox.GameServer.Scripting.CSharp;
using GameServerCore.Enums;
using System.Numerics;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.GameObjects.SpellNS;
using LeagueSandbox.GameServer.GameObjects.StatsNS;
using LeagueSandbox.GameServer.API;

namespace ItemPassives
{
    public class ItemID_3057 : IItemScript
    {
        ObjAIBase Itemowner;
        public StatsModifier StatsModifier { get; private set; } = new StatsModifier();
        public void OnActivate(ObjAIBase owner)
        {
            Itemowner = owner as Champion;
            for (byte i = 0; i < 4; i++) { ApiEventManager.OnSpellCast.AddListener(this, Itemowner.Spells[i], BuffAdd, false); }
        }
        public void BuffAdd(Spell spell)
        {
            AddBuff("Sheen", 10.0f, 1, spell, Itemowner, Itemowner);
        }
        public void OnDeactivate(ObjAIBase owner)
        {
            ApiEventManager.OnSpellCast.RemoveListener(this);
        }
    }
}