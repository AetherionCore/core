using GameServerCore.Enums;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.API;
using LeagueSandbox.GameServer.GameObjects;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using LeagueSandbox.GameServer.GameObjects.SpellNS;
using LeagueSandbox.GameServer.GameObjects.StatsNS;
using LeagueSandbox.GameServer.Scripting.CSharp;
using System.Numerics;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;

namespace Buffs
{
    internal class ZedUltDashCloneMaker : IBuffGameScript
    {
        private Minion a;
        private Minion b;
        private Spell Ult;
        private ObjAIBase Zed;
        private readonly AttackableUnit Target = Spells.ZedUlt.Target;
        public BuffScriptMetaData BuffMetaData { get; set; } = new BuffScriptMetaData
        {
            BuffAddType = BuffAddType.REPLACE_EXISTING
        };
        public StatsModifier StatsModifier { get; private set; } = new StatsModifier();
        public void OnActivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {
            Ult = ownerSpell;
            Zed = ownerSpell.CastInfo.Owner as Champion;
            var m1 = GetPointFromUnit(Zed, 1200f, 30f);
            var m2 = GetPointFromUnit(Zed, 1200f, -30f);
            a = AddMinion((Champion)Zed, "ZedShadow", "ZedShadow", m1, Zed.Team, Zed.SkinID, true, false);
            b = AddMinion((Champion)Zed, "ZedShadow", "ZedShadow", m2, Zed.Team, Zed.SkinID, true, false);
            ForceMovement(a, null, Target.Position, 1400, 0, 0, 0);
            ForceMovement(b, null, Target.Position, 1400, 0, 0, 0);
            AddParticleTarget(Zed, a, "Zed_Base_R_Dash.troy", Zed);
            AddParticleTarget(Zed, b, "Zed_Base_R_Dash.troy", Zed);
        }
        public void OnDeactivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {
            a.Die(CreateDeathData(false, 0, a, a, DamageType.DAMAGE_TYPE_TRUE, DamageSource.DAMAGE_SOURCE_INTERNALRAW, 0.0f));
            b.Die(CreateDeathData(false, 0, b, b, DamageType.DAMAGE_TYPE_TRUE, DamageSource.DAMAGE_SOURCE_INTERNALRAW, 0.0f));
        }
    }
}