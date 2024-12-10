using System;
using GameServerCore.Enums;
using GameServerCore.Scripting.CSharp;
using GameServerLib.GameObjects.AttackableUnits;
using LeaguePackets.Game;
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

        public ExaltedWithBaronNashor() { }

        public void OnActivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {
            thisBuff = buff;

            if (unit is Champion champ)
            {
                particle = AddParticleTarget(unit, unit, "nashor_rune_buf", unit, buff.Duration);

                /* 
                    To be added:

                    - Minion promote aura

                    - Improved Recall

                        Improved Recall data is missing:
                        Should it be handled in:
                            ~\Content\LeagueSandbox-Scripts\Characters\Global\Recall.cs
                        By using:
                            "if (owner.HasBuff("ExaltedWithBaronNashor"))" ?

                        More information:
                        - Grants up to 40 attack damage and ability power (scales with game time)
                        - Improves Recall:
                            - Reduces channel time by 4 seconds
                            - A successful recall:
                                - Restores 50% of your champion's maximum health/mana
                                - Grants +50% movement speed for 8 seconds.

                    Reference:
                    https://leagueoflegends.fandom.com/wiki/Hand_of_Baron?oldid=2170918
                */
                var _game = unit.GetGame();
                if (_game == null)
                {
                    LogDebug("_game is not initialized in 'ExaltedWithBaronNashor.cs'.");
                    return;
                }
                else
                {
                    float gameTime = _game.GameTime;
                    float bonusDamage = gameTime / 30;
                    bonusDamage -= 15;
                    bonusDamage = Math.Min(bonusDamage, 20);
                    bonusDamage = Math.Max(bonusDamage, 40);

                    StatsModifier.AttackDamage.FlatBonus += bonusDamage;
                    StatsModifier.AbilityPower.FlatBonus += bonusDamage;
                }
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
            */

            thisBuff.DeactivateBuff();
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