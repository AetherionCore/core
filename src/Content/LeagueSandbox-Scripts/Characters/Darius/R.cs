using GameServerCore.Enums;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.Scripting.CSharp;
using System.Numerics;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using LeagueSandbox.GameServer.GameObjects.SpellNS;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;


namespace Spells
{
    public class DariusExecute : ISpellScript
    {
        byte C;
        float AD;
        float Dist;
        float Damage;
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
            //Darius.SetTargetUnit(null, true);
            FaceDirection(Target.Position, Darius, true);
            AD = Darius.Stats.AttackDamage.FlatBonus * 0.75f;
            Dist = System.Math.Abs(Vector2.Distance(Target.Position, Darius.Position));
            if (Dist > 125f)
            {
                //ForceMovement(Darius, null, GetPointFromUnit(Darius,Dist - 125), 1200, 0, 0, 0);
            }
            AddParticleTarget(Darius, Darius, "darius_R_cast_axe.troy", Darius, 10, 1, "weapon");
        }
        public void OnSpellPostCast(Spell spell)
        {
            Damage = Target.HasBuff("DariusHemo")
                ? ((70 + (90 * spell.CastInfo.SpellLevel) + AD) * (Target.GetBuffWithName("DariusHemo").StackCount * 0.2f)) + (70 + (90 * spell.CastInfo.SpellLevel) + AD)
                : 70 + (90 * spell.CastInfo.SpellLevel) + AD;
            Target.TakeDamage(Darius, Damage, DamageType.DAMAGE_TYPE_TRUE, DamageSource.DAMAGE_SOURCE_SPELL, false);
            if (Target.IsDead)
            {
                AddParticleTarget(Darius, Darius, "darius_Base_r_refresh_01", Darius, 2.5f);
                CreateTimer((float)0.25, () =>
                {
                    AddBuff("DariusExecuteMulticast", 20.0f, 1, spell, Darius, Darius);
                });
            }
            AddBuff("DariusHemo", 5.0f, 1, spell, Target, Darius);
            AddParticleTarget(Darius, Target, "darius_Base_R_tar", Target);
            AddParticleTarget(Darius, Target, "darius_Base_R_tar_02.troy", Target, 10f);
            AddParticleTarget(Darius, Target, "darius_Base_R_tar_03.troy", Target, 10f);
        }
    }
}