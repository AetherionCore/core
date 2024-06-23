using GameServerCore.Enums;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using LeagueSandbox.GameServer.Scripting.CSharp;
using System.Numerics;
using LeagueSandbox.GameServer.API;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using LeagueSandbox.GameServer.GameObjects.SpellNS;
using LeagueSandbox.GameServer.GameObjects.SpellNS.Missile;
using LeagueSandbox.GameServer.GameObjects.SpellNS.Sector;

namespace Spells
{
    public class TalonNoxianDiplomacy : ISpellScript
    {
        public SpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
        {
            TriggersSpellCasts = true,
            IsDamagingSpell = true
        };

        public void OnSpellPreCast(ObjAIBase owner, Spell spell, AttackableUnit target, Vector2 start, Vector2 end)
        {
            owner.CancelAutoAttack(true);
            AddBuff("TalonNoxianDiplomacyBuff", 6.0f, 1, spell, owner, owner);
        }
        public void OnSpellPostCast(Spell spell)
        {
            spell.SetCooldown(0, true);
        }
    }
    public class TalonNoxianDiplomacyAttack : ISpellScript
    {
        float QDamage;
        ObjAIBase Talon;
        AttackableUnit Target;
        public SpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
        {
            TriggersSpellCasts = true,
            IsDamagingSpell = true
        };

        public void OnSpellPreCast(ObjAIBase owner, Spell spell, AttackableUnit target, Vector2 start, Vector2 end)
        {
            Target = target;
            Talon = owner = spell.CastInfo.Owner as Champion;
        }
        public void OnSpellCast(Spell spell)
        {
            AddParticleTarget(Talon, Target, "Talon_Base_Q_Bleed", Target, 10f, 1f);
            if (Target is Champion) { AddBuff("TalonBleedDebuff", 6f, 1, spell, Target, Talon); }
            QDamage = Target.HasBuff("TalonDamageAmp")
                ? ((30 * Talon.Spells[0].CastInfo.SpellLevel) + (Talon.Stats.AttackDamage.Total * 0.3f)) * (1 + (0.03f * Talon.Spells[2].CastInfo.SpellLevel))
                : (30 * Talon.Spells[0].CastInfo.SpellLevel) + (Talon.Stats.AttackDamage.Total * 0.3f);
            if (Talon.IsNextAutoCrit)
            {
                Target.TakeDamage(Talon, QDamage * 2, DamageType.DAMAGE_TYPE_PHYSICAL, DamageSource.DAMAGE_SOURCE_ATTACK, true);
            }
            else
            {
                Target.TakeDamage(Talon, QDamage, DamageType.DAMAGE_TYPE_PHYSICAL, DamageSource.DAMAGE_SOURCE_ATTACK, false);
            }
        }
    }
}