using System.Numerics;
using GameServerCore.Enums;
using GameServerCore.Scripting.CSharp;
using GameServerLib.GameObjects.AttackableUnits;
using LeaguePackets.Game.Common;
using LeaguePackets.Game.Events;
using LeagueSandbox.GameServer.API;
using LeagueSandbox.GameServer.GameObjects;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.Buildings;
using LeagueSandbox.GameServer.GameObjects.SpellNS;
using LeagueSandbox.GameServer.GameObjects.StatsNS;
using LeagueSandbox.GameServer.Scripting.CSharp;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;

namespace Buffs
{
    class OrianaGhost : IBuffGameScript
    {
        public BuffScriptMetaData BuffMetaData { get; set; } = new BuffScriptMetaData
        {
            BuffType = BuffType.COMBAT_ENCHANCER,
            BuffAddType = BuffAddType.REPLACE_EXISTING,
            MaxStacks = 1
        };

        public StatsModifier StatsModifier { get; private set; } = new StatsModifier()
        {
        };

        ObjAIBase _orianna;
        Buffs.OriannaBallHandler _ballHandler;
        Particle _bind;
        Particle _ring;

        public void OnActivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {
            _orianna = ownerSpell.CastInfo.Owner;
            _ballHandler = (_orianna.GetBuffWithName("OriannaBallHandler").BuffScript as Buffs.OriannaBallHandler);
            //ApiEventManager.OnDeath.AddListener(this, unit, TargetExecute, false);

            var spellLevel = ownerSpell.CastInfo.SpellLevel - 1;
            var bonusResistances = new[] { 10, 15, 20, 25, 30 }[spellLevel];
            StatsModifier.Armor.FlatBonus = bonusResistances;
            StatsModifier.MagicResist.FlatBonus = bonusResistances;
            unit.AddStatModifier(StatsModifier);

            _bind = AddParticleTarget(ownerSpell.CastInfo.Owner, unit, "Oriana_Ghost_bind", unit, 2300f, flags: FXFlags.TargetDirection);
            _ring = AddParticleTarget(ownerSpell.CastInfo.Owner, unit, "OriannaEAllyRangeRing", unit, 2300f, flags: FXFlags.TargetDirection, teamOnly: ownerSpell.CastInfo.Owner.Team);
        }

        private void TargetExecute(DamageData damageData)
        {
            _ballHandler.GetAttachedChampion().RemoveBuffsWithName("OrianaGhost");
            _ballHandler.DropBall();
        }

        public void OnDeactivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {
            ApiEventManager.OnDeath.RemoveListener(this);
            _bind.SetToRemove();
            _ring.SetToRemove();
        }
    }
}