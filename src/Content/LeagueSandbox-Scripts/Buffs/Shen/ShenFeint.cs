using GameServerCore.Enums;
using LeagueSandbox.GameServer.GameObjects.StatsNS;
using LeagueSandbox.GameServer.Scripting.CSharp;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.GameObjects;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.GameObjects.SpellNS;
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
using System.Numerics;

namespace Buffs
{
    internal class ShenFeint : IBuffGameScript
    {
        float Time;
        float Health;
        Buff FeintBuff;
        Particle Feint;
        ObjAIBase Shen;
        public BuffScriptMetaData BuffMetaData { get; set; } = new BuffScriptMetaData
        {
            BuffType = BuffType.HEAL,
            BuffAddType = BuffAddType.REPLACE_EXISTING
        };
        public StatsModifier StatsModifier { get; private set; } = new StatsModifier();
        public void OnActivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {
            FeintBuff = buff;
            Shen = ownerSpell.CastInfo.Owner as Champion;
            Feint = AddParticleTarget(Shen, Shen, "shen_feint_block", Shen, buff.Duration);
            ApiEventManager.OnLaunchAttack.AddListener(this, Shen, OnLaunchAttack, false);
            Health = (2 + (4 * Shen.Spells[2].CastInfo.SpellLevel) + (Shen.Stats.HealthPoints.Total * 0.015f)) / 9;
        }
        public void OnDeactivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {
            RemoveParticle(Feint);
            ApiEventManager.OnLaunchAttack.RemoveListener(this);
            AddParticleTarget(Shen, Shen, "shen_feint_self_deactivate", Shen, buff.Duration);
        }
        public void OnLaunchAttack(Spell spell)
        {
            if (FeintBuff != null && FeintBuff.StackCount != 0 && !FeintBuff.Elapsed())
            {
                Shen.Spells[0].LowerCooldown(1);
            }
        }
    }
}