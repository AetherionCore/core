using GameServerCore.Enums;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using LeagueSandbox.GameServer.GameObjects;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.GameObjects.SpellNS;
using LeagueSandbox.GameServer.GameObjects.StatsNS;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.Scripting.CSharp;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using LeagueSandbox.GameServer.API;
using System.Numerics;

namespace Buffs
{
    internal class TalonShadowAssaultBuff : IBuffGameScript
    {
        public BuffScriptMetaData BuffMetaData { get; set; } = new BuffScriptMetaData
        {
            BuffType = BuffType.COMBAT_ENCHANCER,
            BuffAddType = BuffAddType.REPLACE_EXISTING
        };

        public StatsModifier StatsModifier { get; private set; } = new StatsModifier();

        Buff ThisBuff;
        Particle p;
        Particle p2;

        public void OnActivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {
            //ApiEventManager.OnSpellCast.AddListener(this, ownerSpell.CastInfo.Owner.GetSpell("TalonRake"), WOnSpellCast);
            //ApiEventManager.OnSpellCast.AddListener(this, ownerSpell.CastInfo.Owner.GetSpell("TalonCutthroat"), EOnSpellPostCast);
            //ApiEventManager.OnSpellCast.AddListener(this, ownerSpell.CastInfo.Owner.GetSpell("TalonBasicAttack"), AOnSpellCast);
            ApiEventManager.OnSpellCast.AddListener(this, ownerSpell.CastInfo.Owner.GetSpell("TalonShadowAssaultToggle"), R2OnSpellCast);
            StatsModifier.MoveSpeed.PercentBonus += 0.4f;
            unit.AddStatModifier(StatsModifier);
            p = AddParticleTarget(ownerSpell.CastInfo.Owner, unit, "talon_invis_cas.troy", unit, 2.5f);
            p2 = AddParticleTarget(ownerSpell.CastInfo.Owner, ownerSpell.CastInfo.Owner, "talon_ult_sound.troy", ownerSpell.CastInfo.Owner, 10f);
            if (unit is ObjAIBase owner)
            {

                var r2Spell = owner.SetSpell("TalonShadowAssaultToggle", 3, true);
                //CreateTimer((float) 0.5f , () =>
                //{
                //ownerSpell.CastInfo.Owner.GetSpell("TalonShadowAssaultToggle").SetCooldown(0f);
                //});
            }
        }

        public void R2OnSpellCast(Spell spell)
        {
            ThisBuff.DeactivateBuff();
        }

        public void OnDeactivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {
            RemoveParticle(p2);
            (unit as ObjAIBase).SetSpell("TalonShadowAssault", 3, true);
        }

        public void OnUpdate(float diff)
        {
        }
    }
}