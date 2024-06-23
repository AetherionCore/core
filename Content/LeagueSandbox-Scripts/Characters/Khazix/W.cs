using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using LeagueSandbox.GameServer.Scripting.CSharp;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.GameObjects.SpellNS;
using GameServerCore.Enums;
using LeagueSandbox.GameServer.API;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.GameObjects.SpellNS.Missile;
using LeagueSandbox.GameServer.GameObjects.SpellNS.Sector;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.Buildings;
using System.Numerics;

namespace Spells
{
    public class KhazixW : ISpellScript
    {
        ObjAIBase Khazix;
        Vector2 TargetPos;
        public SpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
        {
            IsDamagingSpell = true,
            TriggersSpellCasts = true
        };
        public void OnSpellPostCast(Spell spell)
        {
            Khazix = spell.CastInfo.Owner as Champion;
            TargetPos = GetPointFromUnit(Khazix, 1150.0f);
            FaceDirection(TargetPos, Khazix);
            SpellCast(Khazix, 0, SpellSlotType.ExtraSlots, TargetPos, TargetPos, true, Vector2.Zero);
        }
    }
    public class KhazixWMissile : ISpellScript
    {
        float Damage;
        ObjAIBase Khazix;
        Vector2 TargetPos;
        SpellMissile Missile;
        public SpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
        {
            IsDamagingSpell = true,
            TriggersSpellCasts = true
        };
        public void OnActivate(ObjAIBase owner, Spell spell)
        {
            ApiEventManager.OnSpellHit.AddListener(this, spell, TargetExecute, false);
        }
        public void OnSpellPreCast(ObjAIBase owner, Spell spell, AttackableUnit target, Vector2 start, Vector2 end)
        {
            Khazix = spell.CastInfo.Owner as Champion;
            Missile = spell.CreateSpellMissile(new MissileParameters { Type = MissileType.Circle, OverrideEndPosition = end });
        }
        public void TargetExecute(Spell spell, AttackableUnit target, SpellMissile missile, SpellSector sector)
        {
            missile.SetToRemove();
            AddParticle(Khazix, null, "Khazix_Base_W_Tar", missile.Position);
            Damage = 50 + (Khazix.Spells[1].CastInfo.SpellLevel * 30) + Khazix.Stats.AttackDamage.FlatBonus;
            var units = GetUnitsInRange(missile.Position, 200f, true);
            for (int i = 0; i < units.Count; i++)
            {
                if (units[i].Team != Khazix.Team && !(units[i] is ObjBuilding || units[i] is BaseTurret))
                {
                    units[i].TakeDamage(Khazix, Damage, DamageType.DAMAGE_TYPE_PHYSICAL, DamageSource.DAMAGE_SOURCE_ATTACK, false);
                }
            }
        }
    }
}