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
    internal class LeblancSlideReturn : IBuffGameScript
    {
        public BuffScriptMetaData BuffMetaData { get; set; } = new BuffScriptMetaData
        {
            BuffType = BuffType.COMBAT_ENCHANCER,
            BuffAddType = BuffAddType.REPLACE_EXISTING
        };

        public StatsModifier StatsModifier { get; private set; }

        Buff ThisBuff;
        Minion LeBlanc;
        Particle p;
        int previousIndicatorState;

        public void OnActivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {
            ThisBuff = buff;
            LeBlanc = unit as Minion;
            var ownerSkinID = LeBlanc.Owner.SkinID;
            string particles;
            unit.AddStatModifier(StatsModifier);
            buff.SetStatusEffect(StatusFlags.Targetable, false);
            buff.SetStatusEffect(StatusFlags.Ghosted, true);
            switch ((LeBlanc.Owner as ObjAIBase).SkinID)
            {
                case 3:
                    particles = "LeBlanc_Base_W_return_indicator.troy";
                    break;

                case 4:
                    particles = "LeBlanc_Base_W_return_indicator.troy";
                    break;

                default:
                    particles = "LeBlanc_Base_W_return_indicator.troy";
                    break;
            }
            ApiEventManager.OnSpellCast.AddListener(this, LeBlanc.Owner.GetSpell("LeblancSlideReturn"), W2OnSpellCast);
            var direction = new Vector3(LeBlanc.Owner.Position.X, 0, LeBlanc.Owner.Position.Y);
            p = AddParticle(LeBlanc.Owner, null, particles, LeBlanc.Position, buff.Duration);
        }
        public void W2OnSpellCast(Spell spell)
        {
            if (LeBlanc != null && !LeBlanc.IsDead)
            {
                LeBlanc.Owner.RemoveBuffsWithName("LeblancSlide");
                TeleportTo(LeBlanc.Owner, LeBlanc.Position.X, LeBlanc.Position.Y);
                LeBlanc.TakeDamage(LeBlanc, 10000f, DamageType.DAMAGE_TYPE_TRUE, DamageSource.DAMAGE_SOURCE_INTERNALRAW, DamageResultType.RESULT_NORMAL);
                AddParticle(LeBlanc.Owner, null, "LeBlanc_Base_W_return_activation.troy", LeBlanc.Owner.Position);
            }
            ThisBuff.DeactivateBuff();
            if (p != null)
            {
                p.SetToRemove();
            }
        }

        public void OnDeactivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {
            if (LeBlanc != null && !LeBlanc.IsDead)
            {
                if (p != null)
                {
                    p.SetToRemove();
                }
                SetStatus(LeBlanc, StatusFlags.NoRender, true);
                AddParticle(LeBlanc.Owner, null, "LeBlanc_Base_W_return_activation.troy", LeBlanc.Owner.Position);
                LeBlanc.TakeDamage(LeBlanc, 10000f, DamageType.DAMAGE_TYPE_TRUE, DamageSource.DAMAGE_SOURCE_INTERNALRAW, DamageResultType.RESULT_NORMAL);
            }
        }
        public void OnUpdate(float diff)
        {
        }
    }
}