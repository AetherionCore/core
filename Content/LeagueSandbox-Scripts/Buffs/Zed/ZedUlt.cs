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
    internal class ZedUlt : IBuffGameScript
    {
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
            Zed.StopMovement();
            AddParticleTarget(Zed, Zed, "Zed_Base_R_cas.troy", Zed, 10f);
            SealSpellSlot(Zed, SpellSlotType.SpellSlots, 3, SpellbookType.SPELLBOOK_CHAMPION, true);
            Minion Shadow = AddMinion(Zed, "ZedShadow", "ZedShadow", Zed.Position, Zed.Team, Zed.SkinID, true, false);
            AddBuff("ZedRShadowBuff", 6.0f, 1, Ult, Shadow, Zed);
        }
        public void OnDeactivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {
            AddBuff("ZedUltDash", 6.0f, 1, Ult, Zed, Zed, false);
        }
    }
}