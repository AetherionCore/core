using GameServerCore.Enums;
using LeagueSandbox.GameServer.GameObjects.StatsNS;
using LeagueSandbox.GameServer.Scripting.CSharp;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.GameObjects;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.GameObjects.SpellNS;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using LeagueSandbox.GameServer.API;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;

namespace Buffs
{
    class ObduracyAttack : IBuffGameScript
    {
        public BuffScriptMetaData BuffMetaData { get; set; } = new BuffScriptMetaData
        {
            BuffAddType = BuffAddType.REPLACE_EXISTING
        };

        public StatsModifier StatsModifier { get; private set; } = new StatsModifier();

        Buff thisBuff;
        ObjAIBase Unit;
        Particle p;
        Spell ownerSpell;
        Particle p2;

        public void OnActivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {
            thisBuff = buff;
            if (unit is ObjAIBase ai)
            {
                Unit = ai;
                ApiEventManager.OnLaunchAttack.AddListener(this, ai, OnLaunchAttack, true);
                p = AddParticleTarget(ownerSpell.CastInfo.Owner, ownerSpell.CastInfo.Owner, ".troy", unit, buff.Duration, 1, "");
                p2 = AddParticleTarget(ownerSpell.CastInfo.Owner, ownerSpell.CastInfo.Owner, ".troy", unit, buff.Duration, 1, "");
                ai.SkipNextAutoAttack();
                ownerSpell.CastInfo.Owner.CancelAutoAttack(true);
            }
        }

        public void OnDeactivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {
            if (unit is ObjAIBase ai)
            {
                Unit = ai;
            }
            RemoveParticle(p);
            RemoveParticle(p2);
        }
        public void OnLaunchAttack(Spell spell)
        {
            if (!thisBuff.Elapsed() && thisBuff != null && Unit != null)
            {
                thisBuff.DeactivateBuff();
            }
        }
        public void OnUpdate(float diff)
        {

        }
    }
}