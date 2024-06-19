using GameServerCore.Enums;
using LeagueSandbox.GameServer.GameObjects.StatsNS;
using LeagueSandbox.GameServer.API;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.Scripting.CSharp;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.GameObjects;
using LeagueSandbox.GameServer.GameObjects.SpellNS;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
﻿
namespace Buffs
{
    internal class FioraQCD : IBuffGameScript
    {
        public BuffScriptMetaData BuffMetaData { get; set; } = new BuffScriptMetaData
        {
            BuffAddType = BuffAddType.REPLACE_EXISTING
        };

        public StatsModifier StatsModifier { get; private set; } = new StatsModifier();

        private Buff ThisBuff;

        public void OnActivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {
            ThisBuff = buff;
            if (unit is ObjAIBase owner)
            {
                owner.SetSpell("FioraQ", 0, true);
                ApiEventManager.OnSpellPostCast.AddListener(this, owner.GetSpell("FioraQ"), Q2OnSpellCast);
            }
        }
        public void Q2OnSpellCast(Spell spell)
        {
            ThisBuff.DeactivateBuff();
        }

        public void OnDeactivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {
            if (unit is ObjAIBase owner)
            {
                var t = 16 - 2 * (owner.GetSpell("FioraQ").CastInfo.SpellLevel - 1);
                owner.Spells[0].SetCooldown(t);
            }
        }

        public void OnUpdate(float diff)
        {
        }
    }
}