using GameServerCore.Enums;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using LeagueSandbox.GameServer.Scripting.CSharp;
using System.Numerics;
using LeagueSandbox.GameServer.API;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using LeagueSandbox.GameServer.GameObjects.SpellNS;
using LeagueSandbox.GameServer.GameObjects.SpellNS.Missile;
using LeagueSandbox.GameServer.GameObjects;

namespace Spells
{
    public class TwitchExpunge : ISpellScript
    {
        Buff Venom;
        ObjAIBase Twitch;
        public SpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
        {
            TriggersSpellCasts = true,
            IsDamagingSpell = true
        };
        public void OnActivate(ObjAIBase owner, Spell spell)
        {
            Twitch = owner = spell.CastInfo.Owner as Champion;
            ApiEventManager.OnLevelUpSpell.AddListener(this, spell, OnLevelUp, true);
        }
        public void OnLevelUp(Spell spell)
        {
            AddBuff("TwitchExpunge", 250000f, 1, spell, Twitch, Twitch);
        }
        public void OnSpellPostCast(Spell spell)
        {
            foreach (AttackableUnit Unit in GetUnitsInRange(Twitch.Position, 1100f, true))
            {
                Venom = Unit.GetBuffWithName("TwitchDeadlyVenom");
                if (Venom != null && Venom.SourceUnit == Twitch)
                {
                    SpellCast(Twitch, 2, SpellSlotType.ExtraSlots, false, Unit, Vector2.Zero);
                }
            }
        }
    }
    public class TwitchEParticle : ISpellScript
    {
        ObjAIBase Twitch;
        SpellMissile Missile;
        public SpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
        {
            TriggersSpellCasts = true,
            IsDamagingSpell = true
        };
        public void OnSpellPostCast(Spell spell)
        {
            Twitch = spell.CastInfo.Owner as Champion;
            Missile = spell.CreateSpellMissile(new MissileParameters
            {
                Type = MissileType.Target,
            });
            ApiEventManager.OnSpellMissileHit.AddListener(this, Missile, TargetExecute, false);
        }
        public void TargetExecute(SpellMissile missile, AttackableUnit target)
        {
            if (Twitch.IsNextAutoCrit)
            {
                target.TakeDamage(Twitch, Twitch.Stats.AttackDamage.Total * 2, DamageType.DAMAGE_TYPE_PHYSICAL, DamageSource.DAMAGE_SOURCE_ATTACK, true);
            }
            else
            {
                target.TakeDamage(Twitch, Twitch.Stats.AttackDamage.Total, DamageType.DAMAGE_TYPE_PHYSICAL, DamageSource.DAMAGE_SOURCE_ATTACK, false);
            }
            AddParticleTarget(Twitch, target, "Twitch_Base_R_Tar.troy", target);
        }
    }
}