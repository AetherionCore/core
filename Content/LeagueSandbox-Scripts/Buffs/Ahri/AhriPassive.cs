using GameServerCore.Enums;
using LeagueSandbox.GameServer.GameObjects.StatsNS;
using LeagueSandbox.GameServer.Scripting.CSharp;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.GameObjects;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.GameObjects.SpellNS;
namespace Buffs
{
    internal class AhriSoulCrusherCounter : IBuffGameScript
    {
        public BuffType BuffType => BuffType.COMBAT_ENCHANCER;
        public BuffAddType BuffAddType => BuffAddType.STACKS_AND_OVERLAPS;
        public int MaxStacks => 9;
        public bool IsHidden => false;

        public BuffScriptMetaData BuffMetaData { get; set; } = new BuffScriptMetaData
        {
            BuffType = BuffType.COMBAT_ENCHANCER,
            BuffAddType = BuffAddType.STACKS_AND_OVERLAPS,
            MaxStacks = 9,
            IsHidden = false
        };

        public StatsModifier StatsModifier { get; private set; } = new StatsModifier();


        public void OnActivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {
            //AddBuff("ShenKiAttack", float.MaxValue, 1, ownerSpell, unit, unit as ObjAIBase, true);
        }

        public void OnDeactivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {
            //unit.RemoveBuffsWithName("ShenKiAttack");
            //AddBuff("ShenWayOfTheNinjaMarker", float.MaxValue, 1, ownerSpell, unit, unit as ObjAIBase, true);
        }

        public void OnUpdate(float diff)
        {
        }
    }
}