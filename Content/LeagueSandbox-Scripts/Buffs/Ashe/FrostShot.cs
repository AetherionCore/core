using GameServerCore.Enums;
using LeagueSandbox.GameServer.GameObjects.StatsNS;
using LeagueSandbox.GameServer.API;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.Scripting.CSharp;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.GameObjects;
using LeagueSandbox.GameServer.GameObjects.SpellNS;
using LeagueSandbox.GameServer.API;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using LeagueSandbox.GameServer.GameObjects.SpellNS;
using GameServerLib.GameObjects.AttackableUnits;
using System.Numerics;

namespace Buffs
{
    class FrostShot : IBuffGameScript
    {
        Buff Shot;
        Spell Frost;
        ObjAIBase Ashe;
        public BuffScriptMetaData BuffMetaData { get; set; } = new BuffScriptMetaData
        {
            BuffType = BuffType.AURA,
            BuffAddType = BuffAddType.RENEW_EXISTING
        };

        public StatsModifier StatsModifier { get; private set; } = new StatsModifier();
        public void OnActivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {
            Shot = buff;
            Frost = ownerSpell;
            Ashe = ownerSpell.CastInfo.Owner as Champion;
            ApiEventManager.OnLaunchAttack.AddListener(this, Ashe, OnLaunchAttack, false);
            ApiEventManager.OnHitUnit.AddListener(this, Ashe, OnHitUnit, false);
        }
        public void OnLaunchAttack(Spell spell)
        {
            if (Ashe.Stats.CurrentMana >= 8f) { Ashe.Stats.CurrentMana -= 8f; } else { Shot.DeactivateBuff(); }
            if (Shot != null && Shot.StackCount != 0 && !Shot.Elapsed())
            {
                SpellCast(Ashe, 1, SpellSlotType.ExtraSlots, false, Ashe.TargetUnit, Vector2.Zero);
            }
        }
        public void OnHitUnit(DamageData damageData)
        {
            if (Ashe.Stats.CurrentMana >= 8f) { AddBuff("FrostArrow", 2f, 1, Frost, damageData.Target, Ashe, false); }
        }
        public void OnDeactivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {
            ApiEventManager.OnPreAttack.RemoveListener(this);
            ApiEventManager.OnLaunchAttack.RemoveListener(this);
        }
    }
}