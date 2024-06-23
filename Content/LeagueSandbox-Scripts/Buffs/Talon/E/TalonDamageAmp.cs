using GameServerCore.Enums;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using LeagueSandbox.GameServer.GameObjects;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.GameObjects.SpellNS;
using LeagueSandbox.GameServer.GameObjects.StatsNS;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.Scripting.CSharp;
using GameServerLib.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.API;

namespace Buffs
{
    internal class TalonDamageAmp : IBuffGameScript
    {
        Buff Amp;
        AttackableUnit Unit;
        public BuffScriptMetaData BuffMetaData { get; set; } = new BuffScriptMetaData
        {
            BuffType = BuffType.DAMAGE,
            BuffAddType = BuffAddType.REPLACE_EXISTING
        };


        public StatsModifier StatsModifier { get; private set; } = new StatsModifier();

        public void OnActivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {
            Amp = buff;
            Unit = unit;
            ApiEventManager.OnDeath.AddListener(this, Unit, OnDeath, true);
        }
        public void OnDeath(DeathData deathData)
        {
            Amp.DeactivateBuff();
        }
        public void OnDeactivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {
            RemoveBuff(Amp);
        }
        public void OnUpdate(float diff)
        {
        }
    }
}