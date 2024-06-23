using GameServerCore.Enums;
using LeagueSandbox.GameServer.GameObjects.StatsNS;
using LeagueSandbox.GameServer.Scripting.CSharp;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.GameObjects;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.GameObjects.SpellNS;
using LeagueSandbox.GameServer.API;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using LeagueSandbox.GameServer.GameObjects.SpellNS.Missile;
using LeagueSandbox.GameServer.GameObjects.SpellNS.Sector;
using System.Numerics;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;

namespace Buffs
{
    internal class RivenFeint : IBuffGameScript
    {
        public BuffScriptMetaData BuffMetaData { get; set; } = new BuffScriptMetaData
        {
            BuffAddType = BuffAddType.REPLACE_EXISTING
        };

        public StatsModifier StatsModifier { get; private set; } = new StatsModifier();

        string pmodelname;
        Particle pmodel;
        Particle pbuff;
        Particle pbuff1;
        Particle pbuff2;
        Particle pbuff3;
        Buff thisBuff;

        public void OnActivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {
            var owner = ownerSpell.CastInfo.Owner as Champion;
            pbuff = AddParticleTarget(owner, owner, "Riven_Base_E_Shield.troy", owner, buff.Duration);
            pbuff1 = AddParticleTarget(owner, owner, "Riven_Base_E_Mis.troy", owner, buff.Duration);
            pbuff2 = AddParticleTarget(owner, owner, "Riven_Base_E_Interupt.troy", owner, buff.Duration);
        }

        public void OnDeactivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {
            RemoveParticle(pbuff);
            RemoveParticle(pbuff1);
            RemoveParticle(pbuff2);
        }
        public void OnLaunchAttack(Spell spell)
        {
        }

        public void OnUpdate(float diff)
        {

        }
    }
}