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
    public class MonkeyKingNimbus : ISpellScript
    {
        AttackableUnit Target;
        public SpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
        {
            TriggersSpellCasts = true
            // TODO
        };

        public void OnSpellPreCast(ObjAIBase owner, Spell spell, AttackableUnit target, Vector2 start, Vector2 end)
        {
            Target = target;
        }

        public void OnSpellCast(Spell spell)
        {
        }

        public void OnSpellPostCast(Spell spell)
        {
            var owner = spell.CastInfo.Owner;
            var ap = spell.CastInfo.Owner.Stats.AbilityPower.Total * 0.6f;
            var damage = 70 * spell.CastInfo.SpellLevel + ap;
            var dist = System.Math.Abs(Vector2.Distance(Target.Position, owner.Position));
            var distt = dist - 125f;
            var time = distt / 1400f;
            var truepos = GetPointFromUnit(owner, distt);

            Minion M = AddMinion((Champion)owner, "MonkeyKingFlying", "MonkeyKingFlying", owner.Position, owner.Team, owner.SkinID, true, false);

            var xx = GetClosestUnitInRange(Target, 300, true);

            if (xx != owner && !xx.IsDead) ForceMovement(M, null, xx.Position, 1400, 0, 0, 0);

            var dist2 = System.Math.Abs(Vector2.Distance(xx.Position, owner.Position));
            var time2 = dist2 / 1400f;

            PlayAnimation(owner, "Spell3", 0.3f);
            AddParticle(owner, null, ".troy", owner.Position, lifetime: 10f);
            AddParticleTarget(owner, owner, "MonkeyKing_Base_E_Mis_Self.troy", owner, time);
            AddParticleTarget(owner, M, "MonkeyKing_Base_E_Mis_Self.troy", M, time);
            ForceMovement(owner, null, truepos, 1400, 0, 0, 0);

            CreateTimer((float)time, () =>
            {
                Target.TakeDamage(owner, damage, DamageType.DAMAGE_TYPE_MAGICAL, DamageSource.DAMAGE_SOURCE_SPELLAOE, false);
                AddParticleTarget(owner, Target, "MonkeyKing_Base_E_Tar.troy", owner);
            });

            CreateTimer((float)time2, () =>
            {
                if (xx != owner) xx.TakeDamage(owner, damage, DamageType.DAMAGE_TYPE_MAGICAL, DamageSource.DAMAGE_SOURCE_SPELLAOE, false);
                AddParticleTarget(owner, xx, "MonkeyKing_Base_E_Tar.troy", owner, 10f);
                M.TakeDamage(M, 100000, DamageType.DAMAGE_TYPE_TRUE, DamageSource.DAMAGE_SOURCE_SPELLAOE, false);
                AddParticleTarget(owner, M, "Become_Transparent.troy", M, 100f);
                SetStatus(M, StatusFlags.NoRender, true);
            });
        }
    }
}