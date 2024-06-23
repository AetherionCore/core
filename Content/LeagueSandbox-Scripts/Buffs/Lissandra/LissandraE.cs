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
    public class LissandraE : IBuffGameScript
    {
        Buff EBuff;
        Minion Ice;
        Particle End;
        Vector2 Departure;
        Vector2 TargetPos;
        ObjAIBase Lissandra;
        public BuffScriptMetaData BuffMetaData { get; set; } = new BuffScriptMetaData
        {
            BuffType = BuffType.AURA,
            BuffAddType = BuffAddType.REPLACE_EXISTING
        };

        public StatsModifier StatsModifier { get; private set; } = new StatsModifier();

        public void OnActivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {
            EBuff = buff;
            Lissandra = ownerSpell.CastInfo.Owner as Champion;
            Lissandra.Spells[2].SetCooldown(0.5f, true);
            TargetPos = GetPointFromUnit(Lissandra, 1050f);
            End = AddParticle(Lissandra, null, "Lissandra_Base_E_End", TargetPos, 2f);
            FaceDirection(TargetPos, End, true);
            ApiEventManager.OnSpellCast.AddListener(this, Lissandra.Spells[2], Teleport);
            SpellCast(Lissandra, 2, SpellSlotType.ExtraSlots, TargetPos, TargetPos, false, Vector2.Zero);
            Ice = AddMinion(Lissandra, "TestCubeRender", "TestCubeRender", Lissandra.Position, Lissandra.Team, Lissandra.SkinID, true, false);
            ForceMovement(Ice, null, TargetPos, 850, 0, 0, 0);
            SetStatus(Ice, StatusFlags.NoRender, true);
        }
        public void Teleport(Spell spell)
        {
            EBuff.DeactivateBuff();
            Departure = Lissandra.Position;
            TeleportTo(Lissandra, Ice.Position.X, Ice.Position.Y);
            AddParticle(Lissandra, null, "Lissandra_Base_E_Departure", Departure, 10f);
            AddParticleTarget(Lissandra, Lissandra, "Lissandra_Base_E_Arrival", Lissandra, 10f);
        }
        public void OnDeactivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {
            RemoveParticle(End);
            Ice.Die(CreateDeathData(false, 0, Ice, Ice, DamageType.DAMAGE_TYPE_TRUE, DamageSource.DAMAGE_SOURCE_INTERNALRAW, 0.0f));
        }
    }
}