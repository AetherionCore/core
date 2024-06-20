using GameServerCore.Enums;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.Scripting.CSharp;
using System;
using LeagueSandbox.GameServer.GameObjects;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using LeagueSandbox.GameServer.GameObjects.SpellNS;
using LeagueSandbox.GameServer.GameObjects.StatsNS;
using LeagueSandbox.GameServer.API;
using System.Numerics;

namespace Buffs
{
    internal class LeblancSlide : IBuffGameScript
    {
        public BuffScriptMetaData BuffMetaData { get; set; } = new BuffScriptMetaData
        {
            BuffType = BuffType.COMBAT_ENCHANCER,
            BuffAddType = BuffAddType.REPLACE_EXISTING
        };

        public StatsModifier StatsModifier { get; private set; }

        Buff ThisBuff;
        Particle p;
        Particle p2;

        public void OnActivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {
            //buff.SetStatusEffect(StatusFlags.Targetable, false);
            //buff.SetStatusEffect(StatusFlags.Ghosted, true);
            ApiEventManager.OnSpellCast.AddListener(this, ownerSpell.CastInfo.Owner.GetSpell("LeblancSlideReturn"), W2OnSpellCast);
            if (unit is ObjAIBase owner)
            {

                var r2Spell = owner.SetSpell("LeblancSlideReturn", 1, true);
            }
        }

        public void W2OnSpellCast(Spell spell)
        {
            ThisBuff.DeactivateBuff();
        }

        public void OnDeactivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {
            RemoveParticle(p2);
            //buff.SetStatusEffect(StatusFlags.Ghosted, false);
            //buff.SetStatusEffect(StatusFlags.Targetable, true);
            (unit as ObjAIBase).SetSpell("LeblancSlide", 1, true);
        }

        public void OnUpdate(float diff)
        {
        }
    }
}