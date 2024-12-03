using System.Numerics;
using GameServerCore.Enums;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using LeagueSandbox.GameServer.Scripting.CSharp;
using LeagueSandbox.GameServer.API;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using LeagueSandbox.GameServer.GameObjects.SpellNS;
using LeagueSandbox.GameServer.GameObjects.SpellNS.Missile;
using LeagueSandbox.GameServer.GameObjects.SpellNS.Sector;

namespace Spells
{
    public class JaxRelentlessAssault : ISpellScript
    {
        ObjAIBase Jax;
        Spell RelentlessAssault;
        public SpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
        {
            TriggersSpellCasts = true,
            IsDamagingSpell = true
        };
        public void OnActivate(ObjAIBase owner, Spell spell)
        {
            RelentlessAssault = spell;
            Jax = spell.CastInfo.Owner as Champion;
            ApiEventManager.OnLevelUpSpell.AddListener(this, spell, OnLevelUpSpell, true);
        }
        public void OnLevelUpSpell(Spell spell)
        {
            ApiEventManager.OnLaunchAttack.AddListener(this, Jax, OnLaunchAttack, false);
        }
        public void OnLaunchAttack(Spell spell)
        {
            AddBuff("JaxRelentlessAttack", 3f, 1, RelentlessAssault, Jax, Jax);
        }
        public void OnSpellCast(Spell spell)
        {
            AddBuff("JaxRelentlessAssault", 8f, 1, spell, Jax, Jax, false);
        }
    }
    public class JaxRelentlessAttack : ISpellScript
    {
        float Damage;
        ObjAIBase Jax;
        public SpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
        {
            TriggersSpellCasts = true,
            IsDamagingSpell = true
        };
        public void OnSpellCast(Spell spell)
        {
            Jax = spell.CastInfo.Owner as Champion;
            Damage = 60 + (40 * Jax.Spells[3].CastInfo.SpellLevel) + (Jax.Stats.AbilityPower.Total * 0.7f);
            Jax.TargetUnit.TakeDamage(Jax, Damage, DamageType.DAMAGE_TYPE_MAGICAL, DamageSource.DAMAGE_SOURCE_ATTACK, false);
            AddParticleTarget(Jax, Jax.TargetUnit, "RelentlessAssault_tar.troy", Jax.TargetUnit, 1f);
        }
    }
}