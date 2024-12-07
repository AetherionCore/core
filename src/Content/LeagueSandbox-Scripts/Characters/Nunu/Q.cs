using System.Numerics;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using LeagueSandbox.GameServer.Scripting.CSharp;
using GameServerCore.Scripting.CSharp;
using GameServerCore.Enums;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.GameObjects.SpellNS;
using LeagueSandbox.GameServer.GameObjects.SpellNS.Missile;
using LeagueSandbox.GameServer.GameObjects.SpellNS.Sector;
using LeagueSandbox.GameServer.API;
using System.Collections.Generic;
using LeagueSandbox.GameServer.GameObjects;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.Buildings;

namespace Spells
{
    public class Consume : ISpellScript
    {
        AttackableUnit Target;

        public SpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
        {
            TriggersSpellCasts = true,
            IsDamagingSpell = true
            // TODO
        };


        public void OnSpellPreCast(ObjAIBase owner, Spell spell, AttackableUnit target, Vector2 start, Vector2 end)
        {
            Target = target;
        }

        public void OnSpellPostCast(Spell spell)
        {
            var owner = spell.CastInfo.Owner;
            float damage = 250 + 150 * spell.CastInfo.SpellLevel;
            float heal = 25 + 45 * spell.CastInfo.SpellLevel;
            owner.Stats.CurrentHealth += heal;


            Target.TakeDamage(owner, damage, DamageType.DAMAGE_TYPE_TRUE, DamageSource.DAMAGE_SOURCE_SPELL, false);

            AddParticleTarget(owner, Target, "yeti_Consume_tar.troy", Target, 1f, 1f);
        }
    }
}