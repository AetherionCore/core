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
    internal class ZedUltBuff : IBuffGameScript
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
            SetStatus(Zed, StatusFlags.CanMove, false);
            SetStatus(Zed, StatusFlags.CanAttack, false);
            SetStatus(Zed, StatusFlags.Ghosted, true);
            SetStatus(Zed, StatusFlags.Targetable, false);
            for (byte i = 0; i < 4; i++)
            {
                SealSpellSlot(Zed, SpellSlotType.SpellSlots, i, SpellbookType.SPELLBOOK_CHAMPION, true);
            }
        }
        public void OnDeactivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {
            SetStatus(Zed, StatusFlags.CanMove, true);
            SetStatus(Zed, StatusFlags.CanAttack, true);
            SetStatus(Zed, StatusFlags.Ghosted, false);
            SetStatus(Zed, StatusFlags.Targetable, true);
            for (byte i = 0; i < 4; i++)
            {
                SealSpellSlot(Zed, SpellSlotType.SpellSlots, i, SpellbookType.SPELLBOOK_CHAMPION, false);
            }
        }
        public void OnUpdate(float diff) { Zed.SetTargetUnit(null, true); }
    }
}