using GameServerCore.Enums;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.API;
using LeagueSandbox.GameServer.GameObjects;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using LeagueSandbox.GameServer.GameObjects.SpellNS;
using LeagueSandbox.GameServer.GameObjects.SpellNS.Missile;
using LeagueSandbox.GameServer.GameObjects.SpellNS.Sector;
using LeagueSandbox.GameServer.GameObjects.StatsNS;
using LeagueSandbox.GameServer.Scripting.CSharp;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;

namespace Buffs
{
    class FizzSeastonePassive : IBuffGameScript
    {
        public BuffScriptMetaData BuffMetaData { get; set; } = new BuffScriptMetaData
        {
            BuffType = BuffType.COMBAT_ENCHANCER,
            BuffAddType = BuffAddType.REPLACE_EXISTING
        };

        public StatsModifier StatsModifier { get; private set; } = new StatsModifier();

        Buff thisBuff;
        ObjAIBase Unit;
        Particle p;
        Spell ownerSpell;
        Particle p2;
        AttackableUnit Target;
        public void OnActivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {
            thisBuff = buff;
            if (unit is ObjAIBase ai)
            {
                Unit = ai;
                SealSpellSlot(ai, SpellSlotType.SpellSlots, 1, SpellbookType.SPELLBOOK_CHAMPION, true);
                ApiEventManager.OnLaunchAttack.AddListener(this, ai, TargetExecute, true);
                //SetAnimStates(ai, new Dictionary<string, string> { { "Attack1", "Spell1" } });
                //p = AddParticleTarget(ownerSpell.CastInfo.Owner, ownerSpell.CastInfo.Owner, "Rengar_Base_Q_Buf_AttackSpeed.troy", unit, buff.Duration, 1, "WEAPON");
                p = AddParticleTarget(ownerSpell.CastInfo.Owner, ownerSpell.CastInfo.Owner, ".troy", unit, buff.Duration, 1, "");
                p2 = AddParticleTarget(ownerSpell.CastInfo.Owner, ownerSpell.CastInfo.Owner, ".troy", unit, buff.Duration, 1, "");
                ai.SkipNextAutoAttack();
                ownerSpell.CastInfo.Owner.CancelAutoAttack(true);
            }
            buff.SetStatusEffect(StatusFlags.Ghosted, true);
            StatsModifier.Range.FlatBonus += 50;
            unit.AddStatModifier(StatsModifier);
        }

        public void OnDeactivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {
            if (unit is ObjAIBase ai)
            {
                Unit = ai;
                //ApiEventManager.OnLaunchAttack.RemoveListener(this);
                SealSpellSlot(ai, SpellSlotType.SpellSlots, 1, SpellbookType.SPELLBOOK_CHAMPION, false);
            }
            RemoveParticle(p);
            RemoveParticle(p2);
        }
        public void TargetExecute(Spell spell)
        {
            if (!thisBuff.Elapsed() && thisBuff != null && Unit != null)
            {
                Target = spell.CastInfo.Targets[0].Unit;
                float ad = Unit.Stats.AbilityPower.Total * 0.35f;
                float damage = 30 + 10 * Unit.GetSpell("FizzSeastonePassive").CastInfo.SpellLevel + ad;
                Target.TakeDamage(Unit, damage, DamageType.DAMAGE_TYPE_MAGICAL, DamageSource.DAMAGE_SOURCE_SPELL, false);
                AddParticleTarget(Unit, Target, "fizz_seastoneactive_hit_sound.troy", Unit);
                AddParticleTarget(Unit, Target, "Fizz_MarinerDoom.troy", Unit);
                AddParticleTarget(Unit, Target, "Fizz_PiercingStrike_tar.troy", Unit);
                AddParticleTarget(Unit, Target, "Fizz_PiercingStrike.troy", Unit);
                thisBuff.DeactivateBuff();
            }
        }
        public void OnUpdate(float diff)
        {

        }
    }
}