using GameServerCore.Enums;
using LeagueSandbox.GameServer.Scripting.CSharp;
using LeagueSandbox.GameServer.API;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using LeagueSandbox.GameServer.GameObjects.SpellNS;
using System.Numerics;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using LeagueSandbox.GameServer.GameObjects;

namespace Spells
{
    public class IreliaEquilibriumStrike : ISpellScript
    {
        Buff DeBuff;
        string Tar1;
        string Tar2;
        float Damage;
        ObjAIBase Irelia;
        Particle DeBuffP;
        AttackableUnit Target;
        public SpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
        {
            TriggersSpellCasts = true,
            IsDamagingSpell = true
        };

        public void OnSpellPreCast(ObjAIBase owner, Spell spell, AttackableUnit target, Vector2 start, Vector2 end)
        {
            Target = target;
            Irelia = owner = spell.CastInfo.Owner as Champion;
        }
        public void OnSpellPostCast(Spell spell)
        {
            switch (Irelia.SkinID)
            {
                case 4:
                    Tar1 = "irelia_frostblade_equilibriumstrike_tar.troy";
                    Tar2 = "irelia_frostblade_equilibriumstrike_tar_02.troy";
                    break;

                default:
                    Tar1 = "irelia_equilibriumStrike_tar_01.troy";
                    Tar2 = "irelia_equilibriumStrike_tar_02.troy";
                    break;
            }
            Damage = 30 + (50 * spell.CastInfo.SpellLevel) + (Irelia.Stats.AbilityPower.Total * 0.5f);
            Target.TakeDamage(Irelia, Damage, DamageType.DAMAGE_TYPE_MAGICAL, DamageSource.DAMAGE_SOURCE_SPELL, false);
            if (Irelia.Stats.CurrentHealth / Irelia.Stats.HealthPoints.Total > Target.Stats.CurrentHealth / Target.Stats.HealthPoints.Total)
            {
                DeBuff = AddBuff("Slow", 0.8f + (0.2f * spell.CastInfo.SpellLevel), 1, spell, Target, Irelia);
                DeBuffP = AddParticleTarget(Irelia, Target, Tar2, Target);
            }
            else
            {
                DeBuff = AddBuff("Stun", 0.8f + (0.2f * spell.CastInfo.SpellLevel), 1, spell, Target, Irelia);
                DeBuffP = AddParticleTarget(Irelia, Target, Tar1, Target);
            }
            ApiEventManager.OnBuffDeactivated.AddListener(this, DeBuff, ReMove, false);
            Add(spell);
        }
        public void Add(Spell spell)
        {
            AddBuff("SlowDumny", 0.8f + (0.2f * spell.CastInfo.SpellLevel), 1, spell, Target, Irelia);
        }
        public void ReMove(Buff buff)
        {
            RemoveParticle(DeBuffP);
        }

    }
}