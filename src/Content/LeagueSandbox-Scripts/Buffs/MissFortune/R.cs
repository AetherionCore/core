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
    internal class MissFortuneBulletTime : IBuffGameScript
    {
        public BuffScriptMetaData BuffMetaData { get; set; } = new BuffScriptMetaData
        {
            BuffType = BuffType.HEAL
        };

        public StatsModifier StatsModifier { get; private set; } = new StatsModifier();

        ObjAIBase owner;
        float tickTime;
        Spell S;
        Vector2 targetPos;

        public void OnActivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {
            S = ownerSpell;
            owner = ownerSpell.CastInfo.Owner;
            owner.AddStatModifier(StatsModifier);
            for (int bladeCount = 0; bladeCount <= 8; bladeCount++)
            {
                targetPos = GetPointFromUnit(owner, 1200f, (-24f + (bladeCount * 8f)));
                //SpellCast(owner, 1, SpellSlotType.ExtraSlots, end, Vector2.Zero, true, start);
                //SpellCast(owner, 1, SpellSlotType.ExtraSlots, end, end, true, Vector2.Zero);				 
            }
            //targetPos = new Vector2(ownerSpell.CastInfo.TargetPosition.X, ownerSpell.CastInfo.TargetPosition.Z);
        }

        public void OnDeactivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {
            ApiEventManager.RemoveAllListenersForOwner(this);
        }

        public void OnUpdate(float diff)
        {
            if (tickTime >= 0.0f)
            {
                for (int bladeCount = 0; bladeCount <= 8; bladeCount++)
                {
                    targetPos = GetPointFromUnit(owner, 1200f, (-24f + (bladeCount * 8f)));
                    SpellCast(owner, 2, SpellSlotType.ExtraSlots, targetPos, targetPos, false, Vector2.Zero);
                    //SpellCast(owner, 1, SpellSlotType.ExtraSlots, end, Vector2.Zero, true, start);
                    //SpellCast(owner, 1, SpellSlotType.ExtraSlots, end, end, true, Vector2.Zero);				 
                }
                tickTime = -250;
            }
            tickTime += diff;
        }
    }
}