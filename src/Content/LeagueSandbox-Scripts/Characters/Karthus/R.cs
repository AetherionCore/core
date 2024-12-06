using GameServerCore.Enums;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using LeagueSandbox.GameServer.Scripting.CSharp;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.GameObjects.SpellNS;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;
using System.Numerics;

namespace Spells
{
    public class KarthusFallenOne : ISpellScript
    {
        ObjAIBase Owner;
        float TimeSinceLastTick = 0;
        bool limiter = false;
        float damage;

        public SpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
        {
            TriggersSpellCasts = true,
            ChannelDuration = 3f,
        };

        public void OnSpellPreCast(ObjAIBase owner, Spell spell, AttackableUnit target, Vector2 start, Vector2 end)
        {
            Owner = owner;
        }

        public void OnSpellCast(Spell spell)
        {
            var owner = spell.CastInfo.Owner;
            var champions = GetChampionsInRange(owner.Position, 20000, true);
            for (int i = 0; i < champions.Count; i++)
            {
                if (champions[i].Team != owner.Team)
                {
                    AddParticleTarget(owner, champions[i], "Karthus_Base_R_Target.troy", champions[i], 3f);
                }
            }
        }

        public void OnSpellPostCast(Spell spell)
        {
            var owner = spell.CastInfo.Owner;
            var ap = owner.Stats.AbilityPower.Total * 0.6f;
            damage = 100 + spell.CastInfo.SpellLevel * 150 + ap;
            TimeSinceLastTick = 0;
            limiter = true;
        }

        public void OnUpdate(float diff)
        {
            TimeSinceLastTick += diff;
            if (TimeSinceLastTick > 2700f && limiter == true && Owner != null)
            {
                var champions = GetChampionsInRange(Owner.Position, 20000, true);
                for (int i = 0; i < champions.Count; i++)
                {
                    if (champions[i].Team != Owner.Team && !champions[i].IsDead)
                    {
                        champions[i].TakeDamage(Owner, damage, DamageType.DAMAGE_TYPE_MAGICAL, DamageSource.DAMAGE_SOURCE_SPELLAOE, false);
                        AddParticleTarget(Owner, champions[i], "Karthus_Base_R_Explosion.troy", champions[i], 1f);
                    }
                }
                limiter = false;
            }
        }
    }
}