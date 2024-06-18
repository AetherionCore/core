using GameServerCore.Enums;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.GameObjects;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.GameObjects.SpellNS;
using LeagueSandbox.GameServer.GameObjects.StatsNS;
using LeagueSandbox.GameServer.Scripting.CSharp;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using GameServerLib.GameObjects.AttackableUnits;

namespace Buffs
{
    internal class EvelynnPassive : IBuffGameScript
    {
        public BuffScriptMetaData BuffMetaData { get; set; } = new BuffScriptMetaData
        {
            BuffType = BuffType.COMBAT_ENCHANCER
        };

        GameObject _owner;
        Buff _revealedDebuff;
        Buff _shadowWalk;
        Buff _hateSpikeMarker;

        public StatsModifier StatsModifier { get; private set; }

        public void OnActivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {
            _owner = unit;
            _shadowWalk = AddBuff("ShadowWalk", 0f, 1, ownerSpell, unit, ownerSpell.CastInfo.Owner, true);
            _hateSpikeMarker = AddBuff("EvelynnHateSpikeMarker", 0f, 1, ownerSpell, unit, ownerSpell.CastInfo.Owner, true);
        }

        public void OnDeactivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {

        }

        public void OnDeath(DeathData deathData)
        {

        }
        public void OnUpdate(float diff)
        {

        }
    }
}
