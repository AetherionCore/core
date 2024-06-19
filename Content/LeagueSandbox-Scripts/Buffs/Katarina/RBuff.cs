using GameServerCore.Enums;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.GameObjects;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using LeagueSandbox.GameServer.GameObjects.SpellNS;
using LeagueSandbox.GameServer.GameObjects.StatsNS;
using LeagueSandbox.GameServer.Scripting.CSharp;
using System.Linq;
using System.Numerics;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using GameServerCore;

namespace Buffs
{
    internal class KatarinaR : IBuffGameScript
    {
        public BuffScriptMetaData BuffMetaData { get; set; } = new BuffScriptMetaData
        {
            BuffType = BuffType.COMBAT_DEHANCER,
            BuffAddType = BuffAddType.RENEW_EXISTING,
            MaxStacks = 1
        };

        public StatsModifier StatsModifier { get; private set; } = new StatsModifier();

        private ObjAIBase Owner;
        float somerandomTick;
        AttackableUnit Target1;
        AttackableUnit Target2;
        AttackableUnit Target3;
        Particle p;

        Spell spell;
        public void OnActivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {
            Champion champion = unit as Champion;
            Owner = ownerSpell.CastInfo.Owner;
            spell = ownerSpell;
            var owner = ownerSpell.CastInfo.Owner;
            p = AddParticleTarget(owner, owner, "Katarina_deathLotus_cas.troy", owner, lifetime: 2.5f, bone: "C_BUFFBONE_GLB_CHEST_LOC");


            var champs = GetChampionsInRange(owner.Position, 500f, true).OrderBy(enemy => Vector2.Distance(enemy.Position, owner.Position)).ToList();
            if (champs.Count > 3)
            {
                foreach (var enemy in champs.GetRange(0, 4)
                     .Where(x => x.Team == CustomConvert.GetEnemyTeam(owner.Team)))
                {
                    SpellCast(owner, 0, SpellSlotType.ExtraSlots, true, enemy, Vector2.Zero);
                    if (Target1 == null) Target1 = enemy;
                    else if (Target2 == null) Target2 = enemy;
                    else if (Target3 == null) Target3 = enemy;
                }
            }
            else
            {
                foreach (var enemy in champs.GetRange(0, champs.Count)
                    .Where(x => x.Team == CustomConvert.GetEnemyTeam(owner.Team)))
                {
                    SpellCast(owner, 0, SpellSlotType.ExtraSlots, true, enemy, Vector2.Zero);
                    if (Target1 == null) Target1 = enemy;
                    else if (Target2 == null) Target2 = enemy;
                    else if (Target3 == null) Target3 = enemy;
                }
            }
        }
        public void OnDeactivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {
            RemoveParticle(p);
            StopAnimation(Owner, "Spell4");
        }

        public void OnPreAttack(Spell spell)
        {
        }

        public void OnUpdate(float diff)
        {
            somerandomTick += diff;
            if (somerandomTick >= 250f)
            {
                if (Target1 != null) SpellCast(Owner, 0, SpellSlotType.ExtraSlots, true, Target1, Vector2.Zero);
                if (Target2 != null) SpellCast(Owner, 0, SpellSlotType.ExtraSlots, true, Target2, Vector2.Zero);
                if (Target3 != null) SpellCast(Owner, 0, SpellSlotType.ExtraSlots, true, Target3, Vector2.Zero);
                somerandomTick = 0;
            }

        }
    }
}
