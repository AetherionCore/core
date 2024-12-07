using System.Numerics;
using GameServerCore.Enums;
using GameServerCore.Scripting.CSharp;
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
    public class EkkoWStun : IBuffGameScript
    {
        public BuffScriptMetaData BuffMetaData { get; set; } = new BuffScriptMetaData
        {
            BuffType = BuffType.COMBAT_ENCHANCER,
            BuffAddType = BuffAddType.REPLACE_EXISTING
        };

        public StatsModifier StatsModifier { get; private set; }

        Particle stun;

        public void OnActivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {
            //Change this back to buff.SetStatusEffect when it's removal get's fixed
            unit.PauseAnimation(true);
            unit.StopMovement();
            (unit as ObjAIBase).SetTargetUnit(null, true);
            SetStatus(unit, StatusFlags.Stunned, true);
            stun = AddParticleTarget(ownerSpell.CastInfo.Owner, unit, "LOC_Stun", unit, buff.Duration);
            stun = AddParticleTarget(ownerSpell.CastInfo.Owner, unit, "Ekko_Base_W_Stun_Tar", unit, buff.Duration);
        }

        public void OnDeactivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {
            unit.PauseAnimation(false);
            SetStatus(unit, StatusFlags.Stunned, false);
            RemoveParticle(stun);
        }
    }
}