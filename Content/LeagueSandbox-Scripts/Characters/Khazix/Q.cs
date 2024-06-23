using GameServerCore.Enums;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using LeagueSandbox.GameServer.Scripting.CSharp;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using LeagueSandbox.GameServer.GameObjects.SpellNS;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;
using System.Linq;
using System.Numerics;

namespace Spells
{
    public class KhazixQ : ISpellScript
    {
        float Damage;
        private ObjAIBase Khazix;
        private AttackableUnit Target;
        public SpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
        {
            IsDamagingSpell = true,
            TriggersSpellCasts = true
        };
        public void OnSpellPostCast(Spell spell)
        {
            Khazix = spell.CastInfo.Owner as Champion;
            Target = Khazix.Spells[0].CastInfo.Targets[0].Unit;
            Damage = 45 + (25 * Khazix.Spells[0].CastInfo.SpellLevel) + (Khazix.Stats.AttackDamage.FlatBonus * 1.2f);
            Target.TakeDamage(Khazix, Damage, DamageType.DAMAGE_TYPE_PHYSICAL, DamageSource.DAMAGE_SOURCE_ATTACK, false);
            AddParticleTarget(Khazix, Target, "Khazix_Base_Q_MultiEnemy_Tar.troy", Target, 1f, 1f);
        }
    }
}