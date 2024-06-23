using GameServerCore.Enums;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.API;
using LeagueSandbox.GameServer.Scripting.CSharp;
using System.Numerics;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using LeagueSandbox.GameServer.GameObjects.SpellNS;
using LeagueSandbox.GameServer.GameObjects.SpellNS.Missile;
using LeagueSandbox.GameServer.GameObjects.SpellNS.Sector;

namespace Spells
{
    public class DariusNoxianTacticsONH : ISpellScript
    {
        public SpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
        {
            TriggersSpellCasts = true,
            IsDamagingSpell = true
        };

        public void OnSpellPreCast(ObjAIBase owner, Spell spell, AttackableUnit target, Vector2 start, Vector2 end)
        {
            owner.CancelAutoAttack(true);
            AddBuff("DariusNoxianTacticsONH", 6.0f, 1, spell, owner, owner);
        }
        public void OnSpellPostCast(Spell spell)
        {
            spell.SetCooldown(0, true);
        }
    }
    public class DariusNoxianTacticsONHAttack : ISpellScript
    {
        float WDamage;
        ObjAIBase Darius;
        AttackableUnit Target;
        public SpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
        {
            TriggersSpellCasts = true,
            IsDamagingSpell = true
        };

        public void OnSpellPreCast(ObjAIBase owner, Spell spell, AttackableUnit target, Vector2 start, Vector2 end)
        {
            Target = target;
            Darius = owner = spell.CastInfo.Owner as Champion;
        }
        public void OnSpellCast(Spell spell)
        {
            WDamage = Darius.Spells[1].CastInfo.SpellLevel * Darius.Stats.AttackDamage.Total * 0.2f;
            if (Darius.IsNextAutoCrit)
            {
                Target.TakeDamage(Darius, WDamage * 2, DamageType.DAMAGE_TYPE_PHYSICAL, DamageSource.DAMAGE_SOURCE_ATTACK, true);
            }
            else
            {
                Target.TakeDamage(Darius, WDamage, DamageType.DAMAGE_TYPE_PHYSICAL, DamageSource.DAMAGE_SOURCE_SPELL, false);
            }
            AddParticleTarget(Darius, Target, "darius_Base_W_tar.troy", Target, 3f);
            AddBuff("DariusNoxianTacticsSlow", 3f, 1, spell, Target, Darius);
        }
    }
}