using GameServerCore.Enums;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.Scripting.CSharp;
using System;
using LeagueSandbox.GameServer.GameObjects;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using LeagueSandbox.GameServer.GameObjects.SpellNS;
using LeagueSandbox.GameServer.GameObjects.StatsNS;
using LeagueSandbox.GameServer.API;
using System.Numerics;

namespace Buffs
{
    internal class LeblancSlideM : IBuffGameScript
    {
        Buff Slide;
        ObjAIBase Leblanc;
        AttackableUnit Unit;
        public BuffScriptMetaData BuffMetaData { get; set; } = new BuffScriptMetaData
        {
            BuffType = BuffType.COMBAT_ENCHANCER,
            BuffAddType = BuffAddType.REPLACE_EXISTING
        };


        public StatsModifier StatsModifier { get; private set; } = new StatsModifier();

        public void OnActivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {
            Unit = unit;
            Slide = buff;
            Leblanc = ownerSpell.CastInfo.Owner as Champion;
            Leblanc.SetSpell("LeblancSlideReturnM", 3, true);
            ApiEventManager.OnSpellCast.AddListener(this, Leblanc.GetSpell("LeblancSlideReturnM"), W2OnSpellCast);
        }
        public void W2OnSpellCast(Spell spell)
        {
            Slide.DeactivateBuff();
        }
        public void OnDeactivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {
            Leblanc.SetSpell("LeblancSlideM", 3, true);
            Leblanc.RemoveBuffsWithName("LeblancSlideMoveM");
        }
    }
}