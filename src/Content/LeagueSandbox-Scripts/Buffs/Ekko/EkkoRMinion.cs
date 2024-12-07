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
    internal class EkkoRMinion : IBuffGameScript
    {
        public BuffScriptMetaData BuffMetaData { get; set; } = new BuffScriptMetaData
        {
            BuffAddType = BuffAddType.REPLACE_EXISTING
        };

        public StatsModifier StatsModifier { get; private set; } = new StatsModifier();

        Minion Ekko;
        Spell Spell;
        ObjAIBase Owner;
        private Buff buff;
        float timeSinceLastTick = 1000f;

        public void OnActivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {
            Spell = ownerSpell;
            if (ownerSpell.CastInfo.Owner is Champion owner)
            {
                Ekko = AddMinion(owner, owner.Model, owner.Model, owner.Position, owner.Team, owner.SkinID, true, false);
                Ekko.SetTargetUnit(owner, true);
                Ekko.UpdateMoveOrder(OrderType.AttackTo, true);
                AddBuff("EkkoRInvuln", 250000f, 1, Spell, Ekko, owner);
                //AddParticleTarget(owner, Ekko, "Become_Transparent.troy", Ekko, 25000f, 1);				
                AddParticleTarget(owner, owner, "Ekko_Base_R_ChargeIndicator.troy", owner, 25000f, 1);
                AddParticleTarget(owner, owner, "Ekko_Base_R_ChargeUp.troy", owner, 25000f, 1);
                AddParticleTarget(owner, owner, "Ekko_Base_R_RewindIndicator.troy", owner, 25000f, 1, "HEAD");
                AddParticleTarget(owner, owner, "Ekko_Base_R_TimeDevice_Active.troy", owner, 25000f, 1, "HEAD");
            }

        }
        public void OnDeactivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {
            if (Spell.CastInfo.Owner is Champion owner)
            {

            }
        }
        public void OnUpdate(float diff)
        {
            timeSinceLastTick += diff;

            if (timeSinceLastTick >= 0f)
            {
                AddParticleTarget(Owner, Ekko, "Become_Transparent.troy", Ekko, 1f, 1);
                timeSinceLastTick = -1000f;
            }
        }
    }
}