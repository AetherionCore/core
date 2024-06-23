using System;
using GameServerCore.Enums;
using LeagueSandbox.GameServer.Scripting.CSharp;
using LeagueSandbox.GameServer.API;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using LeagueSandbox.GameServer.GameObjects.SpellNS;
using LeagueSandbox.GameServer.GameObjects.SpellNS.Missile;
using LeagueSandbox.GameServer.GameObjects.SpellNS.Sector;
using System.Numerics;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using System.Collections.Generic;

namespace Spells
{
    public class Volley : ISpellScript
    {
        ObjAIBase Ashe;
        public SpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
        {
            TriggersSpellCasts = true,
            IsDamagingSpell = true
        };

        public void OnSpellPostCast(Spell spell)
        {
            Ashe = spell.CastInfo.Owner as Champion;
            for (int arrowCount = 0; arrowCount < 8; arrowCount++)
            {
                var targetPos = GetPointFromUnit(Ashe, 1200f, -24.32f + (arrowCount * 8.103f));
                SpellCast(Ashe, 0, SpellSlotType.ExtraSlots, targetPos, targetPos, true, Vector2.Zero);
            }
        }
    }
    public class VolleyAttack : ISpellScript
    {
        float Damage;
        ObjAIBase Ashe;
        SpellMissile Missile;
        public List<AttackableUnit> UnitsHit = new List<AttackableUnit>();
        public SpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
        {
            TriggersSpellCasts = true,
            IsDamagingSpell = true
        };
        public void OnActivate(ObjAIBase owner, Spell spell)
        {
            Ashe = spell.CastInfo.Owner as Champion;
            ApiEventManager.OnSpellHit.AddListener(this, spell, TargetExecute, false);
        }
        public void OnSpellPreCast(ObjAIBase owner, Spell spell, AttackableUnit target, Vector2 start, Vector2 end)
        {
            UnitsHit.Clear();
            Missile = spell.CreateSpellMissile(new MissileParameters { Type = MissileType.Circle, OverrideEndPosition = end });
        }

        public void TargetExecute(Spell spell, AttackableUnit target, SpellMissile missile, SpellSector sector)
        {
            Damage = 30 + (10 * Ashe.Spells[1].CastInfo.SpellLevel) + Ashe.Stats.AttackDamage.FlatBonus;
            if (!UnitsHit.Contains(target))
            {
                UnitsHit.Add(target);
                target.TakeDamage(Ashe, Damage, DamageType.DAMAGE_TYPE_PHYSICAL, DamageSource.DAMAGE_SOURCE_ATTACK, false);
                AddBuff("FrostArrow", 2f, 1, spell, target, Ashe);
                AddParticleTarget(Ashe, target, "Ashe_Base_W_tar.troy", target, 1f);
                missile.SetToRemove();
            }
        }
    }
}