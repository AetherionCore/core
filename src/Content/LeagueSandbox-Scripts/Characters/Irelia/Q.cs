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
    public class IreliaGatotsu : ISpellScript
    {
        Spell Gatotsu;
        Particle Trail1;
        Particle Trail2;
        ObjAIBase Irelia;
        AttackableUnit Target;
        public SpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
        {
            TriggersSpellCasts = true,
            IsDamagingSpell = true
        };

        public void OnSpellPreCast(ObjAIBase owner, Spell spell, AttackableUnit target, Vector2 start, Vector2 end)
        {
            Target = target;
            Gatotsu = spell;
            Irelia = owner = spell.CastInfo.Owner as Champion;
            SetStatus(Irelia, StatusFlags.Ghosted, true);
            ApiEventManager.OnMoveEnd.AddListener(this, Irelia, OnMoveEnd, true);
        }
        public void OnSpellCast(Spell spell)
        {
            var dist = System.Math.Abs(Vector2.Distance(Target.Position, Irelia.Position));
            var distt = dist - 125;
            var targetPos = GetPointFromUnit(Irelia, distt);
            PlayAnimation(Irelia, "Spell1");
            FaceDirection(targetPos, Irelia, true);
            ForceMovement(Irelia, null, targetPos, 2200, 0, 0, 0);
            AddParticle(Irelia, null, "irelia_gotasu_cas.troy", Irelia.Position, lifetime: 10f);
            AddParticle(Irelia, null, "irelia_gotasu_cast_01.troy", Irelia.Position, lifetime: 10f);
            AddParticle(Irelia, null, "irelia_gotasu_cast_02.troy", Irelia.Position, lifetime: 10f);
            Trail1 = AddParticleTarget(Irelia, Irelia, "irelia_gotasu_dash_01.troy", Irelia);
            Trail2 = AddParticleTarget(Irelia, Irelia, "irelia_gotasu_dash_02.troy", Irelia);
        }
        public void OnMoveEnd(AttackableUnit owner)
        {
            RemoveParticle(Trail1);
            RemoveParticle(Trail2);
            Irelia.SetDashingState(false);
            SetStatus(Irelia, StatusFlags.Ghosted, false);
            StopAnimation(Irelia, "Spell1", true, true, true);
            Target.TakeDamage(Irelia, 20 + (30 * (Gatotsu.CastInfo.SpellLevel - 1)) + Irelia.Stats.AttackDamage.Total, DamageType.DAMAGE_TYPE_PHYSICAL, DamageSource.DAMAGE_SOURCE_SPELL, false);
            AddParticleTarget(Irelia, Target, "irelia_gotasu_tar.troy", Target, 10f);
            if (Target.IsDead)
            {
                Gatotsu.SetCooldown(0f, true);
                Irelia.Stats.CurrentMana += 35;
                AddParticleTarget(Irelia, Irelia, "irelia_gotasu_mana_refresh.troy", Irelia);
                AddParticleTarget(Irelia, Irelia, "irelia_gotasu_ability_indicator.troy", Irelia);
            }
        }
    }
}