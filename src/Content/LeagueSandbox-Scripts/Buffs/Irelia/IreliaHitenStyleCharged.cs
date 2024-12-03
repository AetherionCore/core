using LeagueSandbox.GameServer.GameObjects.StatsNS;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.Scripting.CSharp;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using LeagueSandbox.GameServer.GameObjects;
using LeagueSandbox.GameServer.GameObjects.SpellNS;
using GameServerCore.Enums;
using LeagueSandbox.GameServer.API;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using System.Numerics;

namespace Buffs
{
    internal class IreliaHitenStyleCharged : IBuffGameScript
    {
        Spell Charged;
        ObjAIBase Irelia;
        public BuffScriptMetaData BuffMetaData { get; set; } = new BuffScriptMetaData
        {
            BuffType = BuffType.COMBAT_ENCHANCER,
            BuffAddType = BuffAddType.REPLACE_EXISTING
        };
        public StatsModifier StatsModifier { get; private set; } = new StatsModifier();
        public void OnActivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {
            Charged = ownerSpell;
            Irelia = ownerSpell.CastInfo.Owner as Champion;
            Irelia.SkipNextAutoAttack();
            Irelia.CancelAutoAttack(true);
            ApiEventManager.OnLaunchAttack.AddListener(this, Irelia, OnLaunchAttack, false);
        }
        public void OnLaunchAttack(Spell spell)
        {
            Irelia.Stats.CurrentHealth += 6 + (4 * Irelia.Spells[1].CastInfo.SpellLevel);
            Irelia.TargetUnit.TakeDamage(Irelia, 15 * Irelia.Spells[1].CastInfo.SpellLevel, DamageType.DAMAGE_TYPE_TRUE, DamageSource.DAMAGE_SOURCE_ATTACK, false);
        }
        public void OnDeactivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {
            AddBuff("IreliaHitenStyle", 250000f, 1, Charged, Irelia, Irelia);
            ApiEventManager.OnLaunchAttack.RemoveListener(this, Irelia);
        }
    }
}