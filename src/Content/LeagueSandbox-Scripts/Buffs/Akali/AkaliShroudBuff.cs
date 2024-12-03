using GameServerCore.Enums;
using LeagueSandbox.GameServer.GameObjects.StatsNS;
using LeagueSandbox.GameServer.Scripting.CSharp;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.GameObjects;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.GameObjects.SpellNS;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using System.Numerics;

namespace Buffs
{
    class AkaliShroudBuff : IBuffGameScript
    {
        public BuffScriptMetaData BuffMetaData { get; set; } = new BuffScriptMetaData
        {
            BuffType = BuffType.COMBAT_DEHANCER,
            BuffAddType = BuffAddType.REPLACE_EXISTING,
            MaxStacks = 1,
            IsHidden = false
        };

        public StatsModifier StatsModifier { get; private set; } = new StatsModifier();

        float ticks;
        Spell originSpell;
        AttackableUnit Owner;

        public void OnActivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {
            var owner = ownerSpell.CastInfo.Owner;
            var Champs = GetChampionsInRange(unit.Position, 50000, true);
            Owner = owner;
            originSpell = ownerSpell;
            foreach (Champion player in Champs)
            {
                if (player.Team.Equals(owner.Team))
                {
                    //owner.SetInvisible((int)player.GetPlayerId(), owner, 0.5f, 0.1f);
                }
                if (!(player.Team.Equals(owner.Team)))
                {
                    if (player.IsAttacking)
                    {
                        player.CancelAutoAttack(false);
                    }
                    //owner.SetInvisible((int)player.GetPlayerId(), owner, 0f, 0.1f);
                }
            }
        }

        public void OnDeactivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {
            var owner = ownerSpell.CastInfo.Owner;
            var Champs = GetChampionsInRange(unit.Position, 50000, true);
            foreach (Champion player in Champs)
            {
                //owner.SetInvisible((int)player.GetPlayerId(), owner, 1f, 0.1f);
                owner.SetStatus(StatusFlags.Ghosted, false);
            }
            unit.SetStatus(StatusFlags.Targetable, true);
        }

        public void OnPreAttack(Spell spell)
        {
        }

        public void OnUpdate(float diff)
        {
            ticks += diff;
            if (ticks >= 100f)
            {
                var spellPos = new Vector2(originSpell.CastInfo.TargetPositionEnd.X, originSpell.CastInfo.TargetPositionEnd.Z);
                if (GameServerCore.Extensions.IsVectorWithinRange(Owner.Position, spellPos, 400f))
                {
                    Owner.SetStatus(StatusFlags.Targetable, false);
                }
                else Owner.SetStatus(StatusFlags.Targetable, true);
                ticks = 0f;
            }
        }
    }
}