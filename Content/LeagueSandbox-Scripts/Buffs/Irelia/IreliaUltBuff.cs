using LeagueSandbox.GameServer.GameObjects.StatsNS;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.Scripting.CSharp;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using LeagueSandbox.GameServer.GameObjects;
using LeagueSandbox.GameServer.GameObjects.SpellNS;
using GameServerCore.Enums;
using LeagueSandbox.GameServer.API;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using System.Numerics;

namespace Buffs
{
    class IreliaTranscendentBlades : IBuffGameScript
    {
        public BuffScriptMetaData BuffMetaData { get; set; } = new BuffScriptMetaData
        {
            BuffType = BuffType.DAMAGE,
            BuffAddType = BuffAddType.STACKS_AND_RENEWS,
            MaxStacks = 5
        };


        public StatsModifier StatsModifier { get; private set; } = new StatsModifier();

        Particle p;
        Particle p2;
        Particle p3;
        Particle p4;
        Particle p5;
        Particle p6;

        public void OnActivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {
            var owner = ownerSpell.CastInfo.Owner;
            var ownerSkinID = owner.SkinID;
            var trueCoords = GetPointFromUnit(owner, 1000f);
            switch (buff.StackCount)
            {
                case 1:
                    PlayAnimation(unit, "Spell4");
                    p = AddParticleTarget(ownerSpell.CastInfo.Owner, unit, ".troy", unit, buff.Duration, 1, "BUFFBONE_Cstm_Sword1_loc");
                    p2 = AddParticleTarget(ownerSpell.CastInfo.Owner, unit, ".troy", unit, buff.Duration, 1, "BUFFBONE_Cstm_Sword1_loc");
                    p3 = AddParticleTarget(ownerSpell.CastInfo.Owner, unit, ".troy", unit, buff.Duration, 1, "BUFFBONE_Cstm_Sword1_loc");
                    p4 = AddParticleTarget(ownerSpell.CastInfo.Owner, unit, ".troy", unit, buff.Duration, 1, "BUFFBONE_Cstm_Sword1_loc");
                    p5 = AddParticleTarget(ownerSpell.CastInfo.Owner, unit, ".troy", unit, buff.Duration, 1, "BUFFBONE_Cstm_Sword1_loc");
                    p6 = AddParticleTarget(ownerSpell.CastInfo.Owner, unit, "irelia_ult_magic_resist.troy", unit, buff.Duration);
                    break;
                case 2:
                    FaceDirection(trueCoords, owner);
                    RemoveParticle(p);
                    SpellCast(owner, 0, SpellSlotType.ExtraSlots, trueCoords, trueCoords, false, Vector2.Zero);
                    break;
                case 3:
                    FaceDirection(trueCoords, owner);
                    RemoveParticle(p);
                    SpellCast(owner, 0, SpellSlotType.ExtraSlots, trueCoords, trueCoords, false, Vector2.Zero);
                    break;
                case 4:
                    FaceDirection(trueCoords, owner);
                    RemoveParticle(p);
                    SpellCast(owner, 0, SpellSlotType.ExtraSlots, trueCoords, trueCoords, false, Vector2.Zero);
                    break;
                case 5:
                    FaceDirection(trueCoords, owner);
                    RemoveParticle(p);
                    SpellCast(owner, 0, SpellSlotType.ExtraSlots, trueCoords, trueCoords, false, Vector2.Zero);
                    buff.DeactivateBuff();
                    break;
            }

        }
        public void OnDeactivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {
            RemoveParticle(p);
            RemoveParticle(p2);
            RemoveParticle(p3);
            RemoveParticle(p4);
            RemoveParticle(p5);
            RemoveParticle(p6);
        }

        public void OnUpdate(float diff)
        {
        }
    }
}