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
    internal class ZedRShadowBuff : IBuffGameScript
    {
        ObjAIBase Zed;
        Minion Shadow;
        Buff ShadowBuff;
        Particle CurrentIndicator;
        public BuffScriptMetaData BuffMetaData { get; set; } = new BuffScriptMetaData
        {
            BuffType = BuffType.COMBAT_ENCHANCER,
            BuffAddType = BuffAddType.REPLACE_EXISTING
        };
        public StatsModifier StatsModifier { get; private set; } = new StatsModifier();
        public void OnActivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {
            ShadowBuff = buff;
            Shadow = unit as Minion;
            Zed = Shadow.Owner as Champion;
            AddParticleTarget(Zed, Shadow, "zed_base_w_tar.troy", Shadow);
            ApiEventManager.OnSpellCast.AddListener(this, Zed.GetSpell("ZedR2"), R2OnSpellCast);
            ApiEventManager.OnSpellCast.AddListener(this, Zed.GetSpell("ZedShuriken"), QOnSpellCast);
            ApiEventManager.OnSpellPostCast.AddListener(this, Zed.GetSpell("ZedShuriken"), QOnSpellPostCast);
            ApiEventManager.OnSpellCast.AddListener(this, Zed.GetSpell("ZedPBAOEDummy"), EOnSpellCast);
            CurrentIndicator = AddParticleTarget(Zed, Zed, "zed_shadowindicatornearbloop.troy", Shadow, buff.Duration, flags: FXFlags.TargetDirection);
        }
        public void QOnSpellCast(Spell spell)
        {
            if (Shadow != null && !Shadow.IsDead)
            {
                PlayAnimation(Shadow, "Spell1");
                var targetPos = new Vector2(spell.CastInfo.TargetPositionEnd.X, spell.CastInfo.TargetPositionEnd.Z);
                FaceDirection(targetPos, Shadow);
            }
        }

        public void QOnSpellPostCast(Spell spell)
        {
            if (Shadow != null && !Shadow.IsDead)
            {
                var targetPos = GetPointFromUnit(Shadow, 950f);
                SpellCast(Zed, 1, SpellSlotType.ExtraSlots, targetPos, Vector2.Zero, true, Shadow.Position);
            }
        }
        public void EOnSpellCast(Spell spell)
        {
            var ownerSkinID = Shadow.Owner.SkinID;
            if (Shadow != null && !Shadow.IsDead)
            {
                SpellCast(Zed, 2, SpellSlotType.ExtraSlots, true, Shadow, Vector2.Zero);
                PlayAnimation(Shadow, "Spell3", 0.5f);
                AddParticle(Zed, null, "Zed_Base_E_cas.troy", Shadow.Position);
            }
        }
        public void R2OnSpellCast(Spell spell)
        {
            var ownerPos = Zed.Position;
            CurrentIndicator = AddParticleTarget(Zed, Zed, "zed_shadowindicatorfar.troy", Shadow, ShadowBuff.Duration, flags: FXFlags.TargetDirection);
            if (Shadow != null && !Shadow.IsDead)
            {
                TeleportTo(Zed, Shadow.Position.X, Shadow.Position.Y);
                TeleportTo(Shadow, ownerPos.X, ownerPos.Y);
                AddParticleTarget(Zed, Shadow.Owner, "zed_base_cloneswap.troy", Zed);
                AddParticleTarget(Zed, Shadow, "zed_base_cloneswap.troy", Shadow);
            }
            Zed.RemoveBuffsWithName("ZedRHandler");
        }
        public void OnDeactivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {
            RemoveParticle(CurrentIndicator);
            if (Shadow != null && !Shadow.IsDead)
            {
                if (CurrentIndicator != null)
                {
                    CurrentIndicator.SetToRemove();
                }
                SetStatus(Shadow, StatusFlags.NoRender, true);
                CurrentIndicator.SetToRemove();
                AddParticle(Zed, null, "zed_base_clonedeath.troy", Shadow.Position);
                Shadow.Die(CreateDeathData(false, 0, Shadow, Shadow, DamageType.DAMAGE_TYPE_TRUE, DamageSource.DAMAGE_SOURCE_INTERNALRAW, 0.0f));
            }
        }
    }
}