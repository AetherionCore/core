using GameServerCore.Enums;
using GameServerCore.Scripting.CSharp;
using LeaguePackets.Game.Events;
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
    internal class Deceive : IBuffGameScript
    {
        public BuffScriptMetaData BuffMetaData { get; set; } = new BuffScriptMetaData
        {
            BuffType = BuffType.DAMAGE,
            BuffAddType = BuffAddType.RENEW_EXISTING
        };
        public BuffType BuffType => BuffType.DAMAGE;
        public BuffAddType BuffAddType => BuffAddType.RENEW_EXISTING;
        public int MaxStacks => 1;
        public bool IsHidden => false;
        public StatsModifier StatsModifier { get; private set; } = new StatsModifier();

        Buff thisBuff;
        Particle p;
        public void OnActivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {
            thisBuff = buff;
            if (unit is Champion champion)
            {
                ownerSpell.SetSpellToggle(true);
                ApiEventManager.OnPreAttack.AddListener(this, champion, OnPreAttack, true);
            }
            StatsModifier.CriticalChance.FlatBonus += 1f;
            StatsModifier.CriticalDamage.FlatBonus += -0.6f + (0.2f * (ownerSpell.CastInfo.SpellLevel - 1)); //Figure out later why this won't work
            unit.AddStatModifier(StatsModifier);
        }
        public void OnPreAttack(Spell spell)
        {
            if (thisBuff != null)
            {
                thisBuff.DeactivateBuff();
            }
            RemoveParticle(p);
        }

        public void OnDeactivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {
            var champion = unit as Champion;
            ownerSpell.SetCooldown(ownerSpell.GetCooldown());
            champion.GetSpell(ownerSpell.SpellName).SetSpellToggle(false);
        }

        public void OnUpdate(float diff)
        {
        }
    }
}