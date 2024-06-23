using GameServerCore.Enums;
using LeagueSandbox.GameServer.GameObjects.StatsNS;
using LeagueSandbox.GameServer.Scripting.CSharp;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.GameObjects;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.GameObjects.SpellNS;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using LeagueSandbox.GameServer.API;
using System.Numerics;
using System.Linq;
using GameServerCore;

namespace Buffs
{
    internal class TwitchHideInShadows : IBuffGameScript
    {
        Particle Haste;
        Buff ShadowsBuff;
        ObjAIBase Twitch;
        float TrueCooldown;
        public BuffScriptMetaData BuffMetaData { get; set; } = new BuffScriptMetaData
        {
            BuffType = BuffType.COMBAT_ENCHANCER,
            BuffAddType = BuffAddType.REPLACE_EXISTING
        };
        public StatsModifier StatsModifier { get; private set; } = new StatsModifier();

        public void OnActivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {
            ShadowsBuff = buff;
            Twitch = ownerSpell.CastInfo.Owner as Champion;
            Twitch.SkipNextAutoAttack();
            Twitch.CancelAutoAttack(false, true);
            StatsModifier.MoveSpeed.PercentBonus += 0.1f;
            Twitch.AddStatModifier(StatsModifier);
            Haste = AddParticleTarget(Twitch, Twitch, "Twitch_Base_Q_Haste", Twitch, lifetime: buff.Duration);
            AddParticle(Twitch, null, "Twitch_Base_Q_Bamf", Twitch.Position, lifetime: buff.Duration);
            AddParticle(Twitch, null, "Twitch_Base_Q_Cas_Invisible", Twitch.Position, lifetime: buff.Duration);
            ApiEventManager.OnLaunchAttack.AddListener(this, Twitch, OnLaunchAttack, false);
            SealSpellSlot(Twitch, SpellSlotType.SpellSlots, 0, SpellbookType.SPELLBOOK_CHAMPION, true);
        }

        public void OnLaunchAttack(Spell spell)
        {
            if (ShadowsBuff != null && ShadowsBuff.StackCount != 0 && !ShadowsBuff.Elapsed())
            {
                ShadowsBuff.DeactivateBuff();
            }
        }
        public void OnDeactivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {
            RemoveParticle(Haste);
            OverrideAnimation(Twitch, "Run", "Run_STEALTH");
            if (buff.TimeElapsed >= buff.Duration)
            {
                ApiEventManager.OnLaunchAttack.RemoveListener(this);
            }
            AddBuff("TwitchHideInShadowsBuff", 5f, 1, ownerSpell, Twitch, Twitch);
            AddParticle(Twitch, null, "Twitch_Base_Q_Invisiible_Outro", Twitch.Position, lifetime: buff.Duration);
            SealSpellSlot(Twitch, SpellSlotType.SpellSlots, 0, SpellbookType.SPELLBOOK_CHAMPION, false);
            TrueCooldown = 16 * (1 + Twitch.Stats.CooldownReduction.Total);
            Twitch.Spells[0].SetCooldown(TrueCooldown, true);
        }
    }
}