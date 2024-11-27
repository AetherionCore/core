using System;
using GameServerCore.Enums;
using GameServerCore.Scripting.CSharp;
using GameServerLib.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer;
using LeagueSandbox.GameServer.API;
using LeagueSandbox.GameServer.GameObjects;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using LeagueSandbox.GameServer.GameObjects.SpellNS;
using LeagueSandbox.GameServer.GameObjects.StatsNS;
using LeagueSandbox.GameServer.Scripting.CSharp;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;

namespace Buffs
{
    internal class ExaltedWithBaronNashor : IBuffGameScript
    {
        public BuffScriptMetaData BuffMetaData { get; set; } = new BuffScriptMetaData
        {
            BuffType = BuffType.COMBAT_ENCHANCER
        };

        public StatsModifier StatsModifier { get; private set; } = new StatsModifier();

        Buff thisBuff;
        Particle particle;
        private readonly Game _game;

        public void OnActivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {
            thisBuff = buff;

            if (_game == null)
            {
                // is null
                return;
            }

            if (unit is Champion champ)
            {
                particle = AddParticleTarget(unit, unit, "nashor_rune_buf", unit, buff.Duration);

                // Improved Recall is handled in ~
                // Minion buffs TBA


                /*
                Grants up to 40 attack damage and ability power (scales with game time)
                Improves Recall Recall:
                Reduces channel time by 4 seconds
                A successful recall restores 50% of your champion's maximum health / mana and grants +50% movement speed for 8 seconds.
                https://leagueoflegends.fandom.com/wiki/Hand_of_Baron?oldid=2170918
                */

                float gameTime = _game.GameTime;
                float bonusDamage = gameTime / 30;
                //bonusDamage -= 15;
                bonusDamage = Math.Min(bonusDamage, 20);
                bonusDamage = Math.Max(bonusDamage, 40);

                StatsModifier.AttackDamage.FlatBonus += bonusDamage;
                StatsModifier.AbilityPower.FlatBonus += bonusDamage;

                /* example:
                StatsModifier.ManaRegeneration.FlatBonus += 5 + unit.Stats.ManaPoints.Total * 0.05f;
                StatsModifier.CooldownReduction.FlatBonus += 0.1f;
                unit.AddStatModifier(StatsModifier);
                */
            }
            else
            {
                particle = AddParticleTarget(unit, unit, "nashor_rune_buf_big", unit, buff.Duration);
            }

            ApiEventManager.OnDeath.AddListener(this, unit, OnDeath, false);
        }

        public void OnDeath(DeathData deathData)
        {
            /*
            The buff is lost upon a champion's death and cannot be transferred to another champion, 
            unlike the Crest of Insight Crest of Insight and Crest of Cinders Crest of Cinders, 
            which are not team-wide buffs.

            https://leagueoflegends.fandom.com/wiki/Hand_of_Baron?oldid=2170918
            */
            //thisBuff.DeactivateBuff();
        }

        public void OnDeactivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {
            ApiEventManager.OnDeath.RemoveListener(this);
            RemoveParticle(particle);
        }

        public void OnUpdate(float diff)
        {

        }
    }
}
