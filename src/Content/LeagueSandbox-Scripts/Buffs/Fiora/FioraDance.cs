using GameServerCore.Enums;
using LeagueSandbox.GameServer.GameObjects.StatsNS;
using LeagueSandbox.GameServer.API;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.Scripting.CSharp;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.GameObjects;
using LeagueSandbox.GameServer.GameObjects.SpellNS;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using System.Numerics;

namespace Buffs
{
    internal class FioraDance : IBuffGameScript
    {
        Buff Dance;
        float Time;
        ObjAIBase Fiora;
        Spell FioraDanceS;
        AttackableUnit Unit;
        public BuffScriptMetaData BuffMetaData { get; set; } = new BuffScriptMetaData
        {
            BuffAddType = BuffAddType.STACKS_AND_RENEWS,
            MaxStacks = 5
        };

        public StatsModifier StatsModifier { get; private set; } = new StatsModifier();

        public void OnActivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {
            Dance = buff;
            FioraDanceS = ownerSpell;
            Fiora = ownerSpell.CastInfo.Owner as Champion;
            SetStatus(Fiora, StatusFlags.CanMove, false);
            SetStatus(Fiora, StatusFlags.CanAttack, false);
            SetStatus(Fiora, StatusFlags.Ghosted, true);
            SetStatus(Fiora, StatusFlags.Targetable, false);
            // SetStatus(Fiora, StatusFlags.NoRender, true);
            // SetStatus(Fiora, StatusFlags.ForceRenderParticles, true);
            Unit = Fiora.Spells[3].CastInfo.Targets[0].Unit;
            FaceDirection(Unit.Position, Fiora, true);
            SpellCast(Fiora, 1, SpellSlotType.ExtraSlots, false, Unit, Vector2.Zero);
            for (byte i = 0; i < 4; i++) { SealSpellSlot(Fiora, SpellSlotType.SpellSlots, i, SpellbookType.SPELLBOOK_CHAMPION, true); }
            switch (buff.StackCount) { case 5: Fiora.RemoveBuffsWithName("FioraDance"); return; }
        }
        public void OnDeactivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {
            SetStatus(Fiora, StatusFlags.CanMove, true);
            SetStatus(Fiora, StatusFlags.CanAttack, true);
            SetStatus(Fiora, StatusFlags.Ghosted, false);
            SetStatus(Fiora, StatusFlags.Targetable, true);
            // SetStatus(Fiora, StatusFlags.NoRender, false);
            // SetStatus(Fiora, StatusFlags.ForceRenderParticles, false);
            for (byte i = 0; i < 4; i++) { SealSpellSlot(Fiora, SpellSlotType.SpellSlots, i, SpellbookType.SPELLBOOK_CHAMPION, false); }
        }
        public void OnUpdate(float diff)
        {
            Time += diff;
            Fiora.SetTargetUnit(null, true);
            if (Time >= 450.0f && Unit != null)
            {
                AddBuff("FioraDance", 2.25f, 1, FioraDanceS, Fiora, Fiora);
                Time = 0f;
            }
        }
    }
}