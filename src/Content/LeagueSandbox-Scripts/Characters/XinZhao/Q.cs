using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using LeagueSandbox.GameServer.Scripting.CSharp;
using System.Numerics;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.GameObjects.SpellNS;
using GameServerCore.Enums;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.Buildings;

namespace Spells
{
    public class XenZhaoComboTarget : ISpellScript
    {
        public SpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
        {
            TriggersSpellCasts = true,
            IsDamagingSpell = true
        };

        public void OnSpellPreCast(ObjAIBase owner, Spell spell, AttackableUnit target, Vector2 start, Vector2 end)
        {
            owner.CancelAutoAttack(true);
            AddBuff("XenZhaoComboAutoFinish", 5f, 1, spell, owner, owner, false);
        }
        public void OnSpellPostCast(Spell spell)
        {
            //spell.SetCooldown(0, true);
        }
    }
    public class XenZhaoThrust : ISpellScript
    {
        float QDamage;
        ObjAIBase XinZhao;
        AttackableUnit Target;
        public SpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
        {
            TriggersSpellCasts = true,
            IsDamagingSpell = true
        };

        public void OnSpellPreCast(ObjAIBase owner, Spell spell, AttackableUnit target, Vector2 start, Vector2 end)
        {
            Target = target;
            XinZhao = owner = spell.CastInfo.Owner as Champion;
        }
        public void OnSpellCast(Spell spell)
        {
            QDamage = (15 * XinZhao.Spells[0].CastInfo.SpellLevel) + (XinZhao.Stats.AttackDamage.FlatBonus * 0.2f);
            AddParticleTarget(XinZhao, Target, "XenZiou_Wind_ChainAttack01", Target, 10f, 1f);
            if (XinZhao.IsNextAutoCrit)
            {
                Target.TakeDamage(XinZhao, QDamage * 2, DamageType.DAMAGE_TYPE_PHYSICAL, DamageSource.DAMAGE_SOURCE_ATTACK, true);
            }
            else
            {
                Target.TakeDamage(XinZhao, QDamage, DamageType.DAMAGE_TYPE_PHYSICAL, DamageSource.DAMAGE_SOURCE_ATTACK, false);
            }
        }
        public void OnSpellPostCast(Spell spell)
        {
            AddBuff("XenZhaoComboAutoFinish", 5f, 1, spell, XinZhao, XinZhao, false);
        }
    }
    public class XenZhaoThrust2 : ISpellScript
    {
        float QDamage;
        ObjAIBase XinZhao;
        AttackableUnit Target;
        public SpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
        {
            TriggersSpellCasts = true,
            IsDamagingSpell = true
        };

        public void OnSpellPreCast(ObjAIBase owner, Spell spell, AttackableUnit target, Vector2 start, Vector2 end)
        {
            Target = target;
            XinZhao = owner = spell.CastInfo.Owner as Champion;
        }
        public void OnSpellCast(Spell spell)
        {
            QDamage = (15 * XinZhao.Spells[0].CastInfo.SpellLevel) + (XinZhao.Stats.AttackDamage.FlatBonus * 0.2f);
            AddParticleTarget(XinZhao, Target, "XenZiou_Wind_ChainAttack02", Target, 10f, 1f);
            if (XinZhao.IsNextAutoCrit)
            {
                Target.TakeDamage(XinZhao, QDamage * 2, DamageType.DAMAGE_TYPE_PHYSICAL, DamageSource.DAMAGE_SOURCE_ATTACK, true);
            }
            else
            {
                Target.TakeDamage(XinZhao, QDamage, DamageType.DAMAGE_TYPE_PHYSICAL, DamageSource.DAMAGE_SOURCE_ATTACK, false);
            }
        }
        public void OnSpellPostCast(Spell spell)
        {
            AddBuff("XenZhaoComboAutoFinish", 5f, 1, spell, XinZhao, XinZhao, false);
        }
    }
    public class XenZhaoThrust3 : ISpellScript
    {
        float QDamage;
        ObjAIBase XinZhao;
        AttackableUnit Target;
        public SpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
        {
            TriggersSpellCasts = true,
            IsDamagingSpell = true
        };

        public void OnSpellPreCast(ObjAIBase owner, Spell spell, AttackableUnit target, Vector2 start, Vector2 end)
        {
            Target = target;
            XinZhao = owner = spell.CastInfo.Owner as Champion;
        }
        public void OnSpellCast(Spell spell)
        {
            QDamage = (15 * XinZhao.Spells[0].CastInfo.SpellLevel) + (XinZhao.Stats.AttackDamage.FlatBonus * 0.2f);
            AddParticleTarget(XinZhao, Target, "XenZiou_Wind_ChainAttack03", Target, 10f, 1f);
            if (XinZhao.IsNextAutoCrit)
            {
                Target.TakeDamage(XinZhao, QDamage * 2, DamageType.DAMAGE_TYPE_PHYSICAL, DamageSource.DAMAGE_SOURCE_ATTACK, true);
            }
            else
            {
                Target.TakeDamage(XinZhao, QDamage, DamageType.DAMAGE_TYPE_PHYSICAL, DamageSource.DAMAGE_SOURCE_ATTACK, false);
            }
            if (!(Target is ObjBuilding || Target is BaseTurret)) { ForceMovement(Target, "RUN", new Vector2(Target.Position.X + 8f, Target.Position.Y + 8f), 13f, 0, 16.5f, 0); }
        }
        public void OnSpellPostCast(Spell spell)
        {
            if (XinZhao.HasBuff("XenZhaoComboAutoFinish")) { XinZhao.RemoveBuffsWithName("XenZhaoComboAutoFinish"); }
        }
    }
}