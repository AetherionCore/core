using GameServerCore.Enums;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.API;
using LeagueSandbox.GameServer.GameObjects;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using LeagueSandbox.GameServer.GameObjects.SpellNS;
using LeagueSandbox.GameServer.GameObjects.SpellNS.Missile;
using LeagueSandbox.GameServer.GameObjects.SpellNS.Sector;
using LeagueSandbox.GameServer.GameObjects.StatsNS;
using LeagueSandbox.GameServer.Scripting.CSharp;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;

namespace Buffs
{
    class FizzJumpTwo : IBuffGameScript
    {
        public BuffScriptMetaData BuffMetaData { get; set; } = new BuffScriptMetaData
        {
            BuffAddType = BuffAddType.REPLACE_EXISTING
        };

        public StatsModifier StatsModifier { get; private set; } = new StatsModifier();

        AttackableUnit Unit;
        ObjAIBase owner;
        Particle p;
        Buff thisBuff;
        Particle p2;
        public void OnActivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {
            thisBuff = buff;
            owner = ownerSpell.CastInfo.Owner as Champion;
            Unit = unit;
            owner.Stats.SetActionState(ActionState.CAN_MOVE, false);
            p = AddParticleTarget(owner, unit, "", unit, buff.Duration, 1f);
            p2 = AddParticleTarget(owner, unit, "", unit, buff.Duration, 1f);
            ApiEventManager.OnSpellCast.AddListener(this, owner.GetSpell("FizzJumpTwo"), E2OnSpellCast);
        }
        public void E2OnSpellCast(Spell spell)
        {
            RemoveBuff(thisBuff);
        }

        public void OnDeactivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {
        }
        public void OnUpdate(float diff)
        {
        }
    }
}