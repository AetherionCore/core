using GameServerCore.Enums;
using LeagueSandbox.GameServer.GameObjects.StatsNS;
using LeagueSandbox.GameServer.Scripting.CSharp;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.GameObjects;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.GameObjects.SpellNS;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using LeagueSandbox.GameServer.API;
using System.Numerics;
using System.Linq;
using GameServerCore;

namespace Buffs
{
    internal class AhriTumble : IBuffGameScript
    {

        public BuffScriptMetaData BuffMetaData { get; set; } = new BuffScriptMetaData
        {
            BuffType = BuffType.COMBAT_ENCHANCER,
            BuffAddType = BuffAddType.STACKS_AND_CONTINUE,
            MaxStacks = 3,
            IsHidden = false
        };

        Spell Spell;
        AttackableUnit datarget;
        float ticks;
        bool cancastmissile = false;
        ObjAIBase Owner;
        Buff thisbuff;


        public StatsModifier StatsModifier { get; private set; } = new StatsModifier();


        public void OnActivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {
            var owner = ownerSpell.CastInfo.Owner;
            Owner = owner;
            thisbuff = buff;
            ApiEventManager.OnSpellPostCast.AddListener(owner, owner.GetSpell("AhriE"), DoTheThing);
            DoTheThing(ownerSpell);
        }

        public void DoTheThing(Spell spell)
        {
            Spell = spell;
            var target = spell.CastInfo.Targets[0].Unit;
            datarget = target;
            var owner = spell.CastInfo.Owner;
            var current2 = new Vector2(owner.Position.X, owner.Position.Y);
            var spellPos = new Vector2(spell.CastInfo.TargetPosition.X, spell.CastInfo.TargetPosition.Z);
            var dist = Vector2.Distance(current2, spellPos);

            if (dist > 425.0f)
            {
                dist = 425.0f;
            }

            FaceDirection(spellPos, owner, true);
            var trueCoords2 = GetPointFromUnit(owner, dist);
            spell.CastInfo.Owner.SetTargetUnit(null);
            ForceMovement(owner, "Spell4", trueCoords2, 2200, 0, 0, 0);
            cancastmissile = true;

        }
    

        public void OnDeactivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {
        }

        public void OnUpdate(float diff)
        {
            ticks += diff;

            if (ticks >=350f && ticks <= 400f)
            {
                if (cancastmissile == true)
                {
                    var spell = Spell;
                    var current = new Vector2(spell.CastInfo.Owner.Position.X, spell.CastInfo.Owner.Position.Y);
                    var trueCoords = GetPointFromUnit(spell.CastInfo.Owner, spell.SpellData.CastRangeDisplayOverride);
                    var units = GetUnitsInRange(spell.CastInfo.Owner.Position, 425f, true).Where(x => x.Team == CustomConvert.GetEnemyTeam(spell.CastInfo.Owner.Team));
                    units.OrderBy(allyTarget => Vector2.Distance(Owner.Position, allyTarget.Position));
                    var i = 0;
                    foreach (var allyTarget in units)
                    {
                        if (allyTarget is AttackableUnit && spell.CastInfo.Owner != allyTarget)
                        {
                            if (i < 1)
                            {
                                SpellCast(spell.CastInfo.Owner, 5, SpellSlotType.ExtraSlots, true, allyTarget, Vector2.Zero);
                                i++;
                            }
                        }
                    }
                    cancastmissile = false;
                }
                Owner.GetBuffWithName("AhriTumble").DecrementStackCount();
                ticks = 0;
            }
        }
    }
}