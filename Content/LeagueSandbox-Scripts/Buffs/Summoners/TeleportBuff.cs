using GameServerCore.Enums;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using LeagueSandbox.GameServer.GameObjects;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using LeagueSandbox.GameServer.GameObjects.SpellNS;
using LeagueSandbox.GameServer.GameObjects.StatsNS;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.Scripting.CSharp;
using LeaguePackets.Game.Events;
using Spells;

namespace Buffs
{
    internal class TeleportBuff : IBuffGameScript
    {
        public BuffScriptMetaData BuffMetaData { get; set; } = new BuffScriptMetaData
        {
            BuffType = BuffType.INTERNAL
        };

        public BuffAddType BuffAddType => BuffAddType.REPLACE_EXISTING;
        public int MaxStacks => 1;
        public bool IsHidden => false;

        public StatsModifier StatsModifier { get; private set; } = new StatsModifier();

        public void OnActivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {
            var test = buff.SourceUnit;
            AddParticle(unit, null, "global_ss_teleport_blue.troy", unit.Position, lifetime: 4.0f);
            AddParticle(test, null, "global_ss_teleport_target_blue.troy", test.Position, lifetime: 4.0f);
            if (test is Minion)
            {
                test.SetStatus(StatusFlags.CanMove, false);
                test.StopMovement();
            }
            unit.StopMovement();
            unit.SetStatus(StatusFlags.CanMove, false);
            unit.SetStatus(StatusFlags.CanCast, false);
            unit.SetStatus(StatusFlags.CanAttack, false);
        }

        public void OnDeactivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {
            var test = buff.SourceUnit;
            TeleportTo(unit as ObjAIBase, SummonerTeleport.endpos.X, SummonerTeleport.endpos.Y);
            AddParticle(unit, null, "global_ss_teleport_flash_blue.troy", unit.Position, lifetime: 4.0f);
            AddParticle(unit, null, "global_ss_teleport_sparkleslinger.troy", unit.Position, lifetime: 4.0f);
            AddParticle(unit, null, "global_ss_teleport_arrive_blue.troy", unit.Position, lifetime: 4.0f);
            if (test is Minion)
            {
                test.SetStatus(StatusFlags.CanMove, true);
                //test.StopMovement();
            }
            unit.SetStatus(StatusFlags.CanMove, true);
            unit.SetStatus(StatusFlags.CanCast, true);
            unit.SetStatus(StatusFlags.CanAttack, true);
        }

        public void OnUpdate(float diff)
        {
        }
    }
}