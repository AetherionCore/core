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
    internal class LeblancMIFull : IBuffGameScript
    {
        Buff Passive;
        ObjAIBase Leblanc;
        AttackableUnit Unit;
        public BuffScriptMetaData BuffMetaData { get; set; } = new BuffScriptMetaData
        {
            BuffType = BuffType.COMBAT_ENCHANCER,
            BuffAddType = BuffAddType.REPLACE_EXISTING
        };


        public StatsModifier StatsModifier { get; private set; } = new StatsModifier();

        public void OnActivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {
            Unit = unit;
            Passive = buff;
            Leblanc = ownerSpell.CastInfo.Owner as Champion;
            if (buff.SourceUnit is Champion ch)
            {
                var Clone = CreateClonePet(
                        owner: ch,
                        spell: ownerSpell,
                        cloned: Leblanc,
                        position: Vector2.Zero,
                        buffName: "LeblancMIApplicator",
                        lifeTime: 8.0f,
                        cloneInventory: true,
                        showMinimapIfClone: true,
                        disallowPlayerControl: false,
                        doFade: false
                        );
                //LeblancClone.SetTargetUnit(null, true);
                AddParticleTarget(Leblanc, Leblanc, "LeBlanc_Base_P_poof", Leblanc, 10f);
            }
        }
    }
}