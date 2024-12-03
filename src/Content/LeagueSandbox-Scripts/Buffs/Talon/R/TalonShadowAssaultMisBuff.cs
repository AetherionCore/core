using GameServerCore.Enums;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using LeagueSandbox.GameServer.GameObjects;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.GameObjects.SpellNS;
using LeagueSandbox.GameServer.GameObjects.StatsNS;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.Scripting.CSharp;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using LeagueSandbox.GameServer.API;
using System.Numerics;

namespace Buffs
{
    internal class TalonShadowAssaultMisBuff : IBuffGameScript
    {
        public BuffScriptMetaData BuffMetaData { get; set; } = new BuffScriptMetaData
        {
            BuffType = BuffType.COMBAT_ENCHANCER,
            BuffAddType = BuffAddType.REPLACE_EXISTING
        };

        public StatsModifier StatsModifier { get; private set; } = new StatsModifier();

        Buff ThisBuff;
        Minion Blade;
        Particle p;
        int previousIndicatorState;

        public void OnActivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {
            ThisBuff = buff;
            Blade = unit as Minion;
            var ownerSkinID = Blade.Owner.SkinID;
            string particles;
            unit.AddStatModifier(StatsModifier);
            buff.SetStatusEffect(StatusFlags.Targetable, false);
            buff.SetStatusEffect(StatusFlags.Ghosted, true);
            switch ((Blade.Owner as ObjAIBase).SkinID)
            {
                case 3:
                    particles = "talon_ult_blade_hold_dragon.troy";
                    break;

                case 4:
                    particles = "Talon_Skin04_ult_blade_hold.troy";
                    break;

                default:
                    particles = "Talon_Base_R_Blade_Hold.troy";
                    break;
            }
            ApiEventManager.OnSpellCast.AddListener(this, Blade.Owner.GetSpell("TalonShadowAssaultToggle"), R2OnSpellCast);
            ApiEventManager.OnSpellCast.AddListener(this, Blade.Owner.GetSpell("TalonRake"), R2OnSpellCast);
            ApiEventManager.OnSpellPostCast.AddListener(this, Blade.Owner.GetSpell("TalonRake"), R2OnSpellCast);
            ApiEventManager.OnSpellPostCast.AddListener(this, Blade.Owner.GetSpell("TalonCutthroat"), R2OnSpellCast);
            ApiEventManager.OnSpellPostCast.AddListener(this, Blade.Owner.GetSpell("TalonBasicAttack"), R2OnSpellCast);
            ApiEventManager.OnSpellPostCast.AddListener(this, Blade.Owner.GetSpell("TalonBasicAttack2"), R2OnSpellCast);
            ApiEventManager.OnSpellPostCast.AddListener(this, Blade.Owner.GetSpell("TalonCritAttack"), R2OnSpellCast);
            var direction = new Vector3(Blade.Owner.Position.X, 0, Blade.Owner.Position.Y);
            p = AddParticle(Blade.Owner, null, particles, Blade.Position, buff.Duration, 1f, "C_BuffBone_Glb_Center_Loc", "C_BuffBone_Glb_Center_Loc", direction);
            AddParticleTarget(Blade.Owner, Blade, particles, Blade, buff.Duration, 1, "C_BuffBone_Glb_Center_Loc");
            //p = AddParticleTarget(Blade.Owner, Blade.Owner, "", Blade, buff.Duration, flags: FXFlags.TargetDirection);
        }
        public void R2OnSpellCast(Spell spell)
        {
            Blade.Owner.RemoveBuffsWithName("TalonShadowAssaultBuff");
            ThisBuff.DeactivateBuff();
        }

        public void OnDeactivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {
            if (Blade != null && !Blade.IsDead)
            {
                if (p != null)
                {
                    p.SetToRemove();
                }
                Blade.Owner.RemoveBuffsWithName("TalonShadowAssaultToggle");
                SetStatus(Blade, StatusFlags.NoRender, true);
                AddParticle(Blade.Owner, null, "", Blade.Position);
                SpellCast(Blade.Owner, 4, SpellSlotType.ExtraSlots, true, Blade.Owner, Blade.Position);
                Blade.TakeDamage(Blade, 10000f, DamageType.DAMAGE_TYPE_TRUE, DamageSource.DAMAGE_SOURCE_INTERNALRAW, DamageResultType.RESULT_NORMAL);
            }
        }
        public void OnUpdate(float diff)
        {
        }
    }
}