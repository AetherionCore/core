using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using LeagueSandbox.GameServer.Scripting.CSharp;
using System.Numerics;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.GameObjects.SpellNS;

namespace Spells
{
    public class XinZhaoBasicAttack : ISpellScript
    {
        AttackableUnit Target;
        public SpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
        {
            TriggersSpellCasts = true,
            IsDamagingSpell = true
        };

        public void OnSpellPreCast(ObjAIBase owner, Spell spell, AttackableUnit target, Vector2 start, Vector2 end)
        {
            Target = target;
            if (owner.HasBuff("XenZhaoComboAutoFinish") && owner.GetBuffWithName("XenZhaoComboAutoFinish").StackCount == 3)
            {
                OverrideAnimation(owner, "Spell3", "Attack1");
            }
            else
            {
                OverrideAnimation(owner, "Attack1", "Spell3");
            }
        }
    }
    public class XinZhaoBasicAttack2 : ISpellScript
    {
        AttackableUnit Target;
        public SpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
        {
            TriggersSpellCasts = true,
            IsDamagingSpell = true
        };

        public void OnSpellPreCast(ObjAIBase owner, Spell spell, AttackableUnit target, Vector2 start, Vector2 end)
        {
            Target = target;
            if (owner.HasBuff("XenZhaoComboAutoFinish") && owner.GetBuffWithName("XenZhaoComboAutoFinish").StackCount == 3)
            {
                OverrideAnimation(owner, "Spell3", "Attack2");
            }
            else
            {
                OverrideAnimation(owner, "Attack2", "Spell3");
            }
        }
    }
    public class XinZhaoCritAttack : ISpellScript
    {
        AttackableUnit Target;
        public SpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
        {
            TriggersSpellCasts = true,
            IsDamagingSpell = true
        };

        public void OnSpellPreCast(ObjAIBase owner, Spell spell, AttackableUnit target, Vector2 start, Vector2 end)
        {
            Target = target;
            if (owner.HasBuff("XenZhaoComboAutoFinish") && owner.GetBuffWithName("XenZhaoComboAutoFinish").StackCount == 3)
            {
                OverrideAnimation(owner, "Spell3", "Crit");
            }
            else
            {
                OverrideAnimation(owner, "Crit", "Spell3");
            }
        }
    }
}