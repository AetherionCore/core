using GameServerCore.Enums;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.API;
using LeagueSandbox.GameServer.GameObjects;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.Buildings;
using LeagueSandbox.GameServer.GameObjects.SpellNS;
using LeagueSandbox.GameServer.GameObjects.SpellNS.Missile;
using LeagueSandbox.GameServer.GameObjects.SpellNS.Sector;
using LeagueSandbox.GameServer.GameObjects.StatsNS;
using LeagueSandbox.GameServer.Scripting.CSharp;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;

namespace Buffs
{
    class FizzJump : IBuffGameScript
    {
        public BuffScriptMetaData BuffMetaData { get; set; } = new BuffScriptMetaData
        {
            BuffAddType = BuffAddType.REPLACE_EXISTING
        };

        public StatsModifier StatsModifier { get; private set; } = new StatsModifier();

        AttackableUnit Unit;
        ObjAIBase owner;
        Particle p;
        Buff thisBuff;
        Particle p2;
        public void OnActivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {
            thisBuff = buff;
            owner = ownerSpell.CastInfo.Owner as Champion;
            Unit = unit;
            owner.SetSpell("FizzJumpTwo", 2, true);
            owner.Stats.SetActionState(ActionState.CAN_MOVE, false);
            p = AddParticleTarget(owner, unit, "", unit, buff.Duration, 1f);
            p2 = AddParticleTarget(owner, unit, "", unit, buff.Duration, 1f);
            ApiEventManager.OnSpellCast.AddListener(this, owner.GetSpell("FizzJumpTwo"), E2OnSpellCast);
            if (unit.IsDead)
            {
                RemoveParticle(p);
                RemoveBuff(thisBuff);
                RemoveParticle(p2);
            }
        }
        public void E2OnSpellCast(Spell spell)
        {
            RemoveBuff(thisBuff);
        }

        public void OnDeactivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {
            PlayAnimation(owner, "spell3c");
            AddBuff("FizzTrickSlamSoundDummy", 1f, 1, ownerSpell, owner, owner);
            owner.Stats.SetActionState(ActionState.CAN_MOVE, true);
            AddParticleTarget(owner, owner, ".troy", owner, 0.5f);
            AddParticle(owner, null, ".troy", owner.Position);
            CreateTimer((float)0.5f, () =>
            {
                var AP = owner.Stats.AbilityPower.Total * 0.75f;
                var RLevel = owner.GetSpell("FizzSeastonePassive").CastInfo.SpellLevel;
                var damage = 70 + (50 * (RLevel - 1)) + AP;
                if (owner.HasBuff("FizzJumpTwo"))
                {
                    var units = GetUnitsInRange(owner.Position, 450f, true);
                    for (int i = 0; i < units.Count; i++)
                    {
                        if (units[i].Team != owner.Team && !(units[i] is ObjBuilding || units[i] is BaseTurret))
                        {
                            units[i].TakeDamage(owner, damage, DamageType.DAMAGE_TYPE_MAGICAL, DamageSource.DAMAGE_SOURCE_SPELL, false);
                            AddParticleTarget(owner, units[i], ".troy", units[i], 1f);
                            AddParticleTarget(owner, units[i], "Fizz_TrickSlam_tar.troy", units[i], 1f);
                        }
                    }
                    AddParticleTarget(owner, owner, "Fizz_TrickSlam.troy", owner);
                }
                else
                {
                    var units = GetUnitsInRange(owner.Position, 250f, true);
                    for (int i = 0; i < units.Count; i++)
                    {
                        if (units[i].Team != owner.Team && !(units[i] is ObjBuilding || units[i] is BaseTurret))
                        {
                            units[i].TakeDamage(owner, damage, DamageType.DAMAGE_TYPE_MAGICAL, DamageSource.DAMAGE_SOURCE_SPELL, false);
                            AddParticleTarget(owner, units[i], ".troy", units[i], 1f);
                            AddParticleTarget(owner, units[i], "Fizz_TrickSlam_tar.troy", units[i], 1f);
                        }
                    }
                    AddParticleTarget(owner, owner, "Fizz_TrickSlamTwo.troy", owner);
                }
            });
            owner.SetSpell("FizzJump", 2, true);
            RemoveParticle(p);
            RemoveBuff(thisBuff);
            RemoveParticle(p2);
        }
        public void OnUpdate(float diff)
        {
        }
    }
}