using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.GameObjects;
using LeagueSandbox.GameServer.GameObjects.SpellNS;
using LeagueSandbox.GameServer.GameObjects.StatsNS;
using LeagueSandbox.GameServer.Scripting.CSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LeagueSandbox.GameServer.API;

namespace Buffs
{
    internal class Stealth : IBuffGameScript
    {
        public BuffScriptMetaData BuffMetaData => new BuffScriptMetaData();

        public StatsModifier StatsModifier => new StatsModifier();

        public void OnActivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {
            ApiFunctionManager.SetStatus(unit, GameServerCore.Enums.StatusFlags.Stealthed, true);
        }
    }
}
