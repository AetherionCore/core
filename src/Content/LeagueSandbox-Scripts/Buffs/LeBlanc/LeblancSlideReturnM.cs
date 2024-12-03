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
    internal class LeblancSlideReturnM : IBuffGameScript
    {
        Particle P;
        Buff Return;
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
            Return = buff;
            Leblanc = ownerSpell.CastInfo.Owner as Champion;
            ApiEventManager.OnSpellCast.AddListener(this, Leblanc.GetSpell("LeblancSlideReturnM"), W2OnSpellCast);
            P = AddParticle(Leblanc, null, "LeBlanc_Base_RW_return_indicator", Unit.Position, buff.Duration, 1);
        }
        public void W2OnSpellCast(Spell spell)
        {
            Return.DeactivateBuff();
            TeleportTo(Leblanc, Unit.Position.X, Unit.Position.Y);
            AddParticle(Leblanc, null, "LeBlanc_Base_W_return_activation.troy", Leblanc.Position);
        }
        public void OnDeactivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {
            if (Leblanc != null && !Leblanc.IsDead)
            {
                if (P != null) { P.SetToRemove(); }
                Leblanc.RemoveBuffsWithName("LeblancSlideM");
                Unit.Die(CreateDeathData(false, 0, Unit, Unit, DamageType.DAMAGE_TYPE_TRUE, DamageSource.DAMAGE_SOURCE_INTERNALRAW, 0.0f));
            }
        }
    }
}