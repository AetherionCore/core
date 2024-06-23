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
using LeagueSandbox.GameServer.GameObjects.SpellNS.Missile;
using System.Numerics;
using System.Linq;
using GameServerCore;

namespace Buffs
{
    internal class TwitchFullAutomatic : IBuffGameScript
    {
        Particle RBuff;
        Buff Automatic;
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
            Automatic = buff;
            Twitch = ownerSpell.CastInfo.Owner as Champion;
            Twitch.SkipNextAutoAttack();
            Twitch.CancelAutoAttack(false, true);
            StatsModifier.Range.FlatBonus = 300.0f;
            StatsModifier.AttackDamage.FlatBonus = (Twitch.Spells[3].CastInfo.SpellLevel * 15) + 25f;
            unit.AddStatModifier(StatsModifier);
            RBuff = AddParticleTarget(Twitch, Twitch, "Twitch_Base_R_Buff", Twitch, lifetime: buff.Duration);
            AddParticle(Twitch, null, "Twitch_Base_R_Cas", Twitch.Position, lifetime: buff.Duration);
            ApiEventManager.OnLaunchAttack.AddListener(this, Twitch, OnLaunchAttack, false);
            ApiEventManager.OnLaunchMissile.AddListener(this, Twitch.AutoAttackSpell, OnLaunchMissile, false);
            ApiEventManager.OnLaunchMissile.AddListener(this, Twitch.GetSpell("TwitchCritAttack"), OnLaunchMissile, false);
            SealSpellSlot(Twitch, SpellSlotType.SpellSlots, 3, SpellbookType.SPELLBOOK_CHAMPION, true);
        }

        public void OnLaunchAttack(Spell spell)
        {
            if (Automatic != null && Automatic.StackCount != 0 && !Automatic.Elapsed())
            {
                SpellCast(Twitch, 0, SpellSlotType.ExtraSlots, GetPointFromUnit(Twitch, 1100f), GetPointFromUnit(Twitch, 1100f), false, Vector2.Zero);
            }
        }
        public void OnLaunchMissile(Spell spell, SpellMissile missile)
        {
            if (Automatic != null && Automatic.StackCount != 0 && !Automatic.Elapsed())
            {
                missile.SetToRemove();
            }
        }
        public void OnDeactivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {
            RemoveParticle(RBuff);
            if (buff.TimeElapsed >= buff.Duration)
            {
                ApiEventManager.OnLaunchAttack.RemoveListener(this);
                ApiEventManager.OnLaunchMissile.RemoveListener(this);
            }
            SealSpellSlot(Twitch, SpellSlotType.SpellSlots, 3, SpellbookType.SPELLBOOK_CHAMPION, false);
            TrueCooldown = 16 * (1 + Twitch.Stats.CooldownReduction.Total);
            Twitch.Spells[3].SetCooldown(TrueCooldown, true);
        }
    }
}