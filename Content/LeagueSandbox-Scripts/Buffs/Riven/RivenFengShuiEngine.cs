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
    internal class RivenFengShuiEngine : IBuffGameScript
    {
        public BuffScriptMetaData BuffMetaData { get; set; } = new BuffScriptMetaData
        {
            BuffType = BuffType.COMBAT_ENCHANCER,
            BuffAddType = BuffAddType.REPLACE_EXISTING
        };

        public StatsModifier StatsModifier { get; private set; } = new StatsModifier();

        string pmodelname;
        Particle pmodel;
        Particle pbuff;
        Particle pbuff1;
        Buff thisBuff;

        public void OnActivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {
            var owner = ownerSpell.CastInfo.Owner as Champion;
            pbuff = AddParticleTarget(ownerSpell.CastInfo.Owner, unit, "Riven_Base_R_Buff.troy", unit, buff.Duration, bone: "L_HAND");
            pbuff1 = AddParticleTarget(ownerSpell.CastInfo.Owner, unit, "Riven_Base_R_Buff.troy", unit, buff.Duration, bone: "R_HAND");
            if (unit is Champion c)
            {
                // TODO: Implement Animation Overrides for spells like these
                if (c.SkinID == 0)
                {
                    pmodelname = "Riven_Base_R_Sword";
                }
                else if (c.SkinID == 1)
                {
                    pmodelname = "Riven_Base_R_Sword";
                }
                else if (c.SkinID == 2)
                {
                    pmodelname = "Riven_Base_R_Sword";
                }
                else if (c.SkinID == 3)
                {
                    pmodelname = "Riven_Base_R_Sword";
                }
                else if (c.SkinID == 4)
                {
                    pmodelname = "Riven_Base_R_Sword";
                }
                else if (c.SkinID == 5)
                {
                    pmodelname = "Riven_Base_R_Sword";
                }
                pmodel = AddParticleTarget(c, c, pmodelname, c, buff.Duration, bone: "BUFFBONE_GLB_WEAPON_2");
                StatsModifier.AttackDamage.PercentBonus = (0.4f + (0.1f * (ownerSpell.CastInfo.SpellLevel - 1))) * buff.StackCount; // StackCount included here as an example
                StatsModifier.Range.FlatBonus = 75f * buff.StackCount;
                unit.AddStatModifier(StatsModifier);
                //ownerSpell.CastInfo.Owner.SetAutoAttackSpell("AatroxBasicAttack4", false);
            }
        }

        public void OnDeactivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {
            RemoveParticle(pmodel);
        }
        public void OnLaunchAttack(Spell spell)
        {
        }

        public void OnUpdate(float diff)
        {

        }
    }
}
