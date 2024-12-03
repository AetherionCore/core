using System.Linq;
using GameServerCore;
using GameServerCore.Enums;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using LeagueSandbox.GameServer.Scripting.CSharp;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.GameObjects.SpellNS;
using LeagueSandbox.GameServer.API;
using LeagueSandbox.GameServer.GameObjects.SpellNS.Missile;
using LeagueSandbox.GameServer.GameObjects.SpellNS.Sector;

namespace Spells
{
    public class BusterShot : ISpellScript
    {
        float Damage;
        ObjAIBase Tristana;
        SpellMissile Missile;
        public SpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
        {
            TriggersSpellCasts = true,
            IsDamagingSpell = true
        };
        public void OnActivate(ObjAIBase owner, Spell spell)
        {
            Tristana = owner = spell.CastInfo.Owner as Champion;
            ApiEventManager.OnSpellHit.AddListener(this, spell, TargetExecute, false);
        }
        public void OnSpellPostCast(Spell spell)
        {
            Missile = spell.CreateSpellMissile(new MissileParameters { Type = MissileType.Target });
        }
        public void TargetExecute(Spell spell, AttackableUnit target, SpellMissile missile, SpellSector sector)
        {
            FaceDirection(Tristana.Position, target);
            Damage = 200 + (100 * spell.CastInfo.SpellLevel) + (Tristana.Stats.AbilityPower.Total * 1.5f);
            target.TakeDamage(Tristana, Damage, DamageType.DAMAGE_TYPE_MAGICAL, DamageSource.DAMAGE_SOURCE_SPELL, false);
            AddParticleTarget(Tristana, target, "tristana_bustershot_tar", target, 1f, 1f);
            AddParticleTarget(Tristana, target, "tristana_busterShot_unit_impact", target, 1f, 1f);
            AddParticleTarget(Tristana, Tristana, "BusterShot_cas.troy", Tristana, 1f, 1f);
            ForceMovement(target, "RUN", GetPointFromUnit(target, -(400 + (200 * spell.CastInfo.SpellLevel))), 2200, 0, 0, 0);
            missile.SetToRemove();
        }
    }
}