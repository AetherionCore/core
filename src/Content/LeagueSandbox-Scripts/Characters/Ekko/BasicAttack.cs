using LeagueSandbox.GameServer.Scripting.CSharp;
using System.Numerics;
using GameServerCore.Scripting.CSharp;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.GameObjects.SpellNS;

namespace Spells
{
    public class EkkoBasicAttack : ISpellScript
    {
        AttackableUnit Target;

        public SpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
        {
            // TODO
        };

        public void OnSpellPreCast(ObjAIBase owner, Spell spell, AttackableUnit target, Vector2 start, Vector2 end)
        {
            if (target.HasBuff("EkkoPassiveSpellShieldCheck"))
            {
                OverrideAnimation(owner, "Attack_P_1", "Attack1");
            }
            else
            {
                OverrideAnimation(owner, "Attack1", "Attack_P_1");
            }
        }

        public void OnLaunchAttack(Spell spell)
        {
            //spell.CastInfo.Owner.SetAutoAttackSpell("NasusBasicAttack2", false);
        }
    }

    public class EkkoBasicAttack2 : ISpellScript
    {
        AttackableUnit Target;

        public SpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
        {
            // TODO
        };

        public void OnSpellPreCast(ObjAIBase owner, Spell spell, AttackableUnit target, Vector2 start, Vector2 end)
        {
            if (target.HasBuff("EkkoPassiveSpellShieldCheck"))
            {
                OverrideAnimation(owner, "Attack_P_2", "Attack2");
            }
            else
            {
                OverrideAnimation(owner, "Attack2", "Attack_P_2");
            }
        }

        public void OnLaunchAttack(Spell spell)
        {
            //spell.CastInfo.Owner.SetAutoAttackSpell("NasusBasicAttack", false);
        }
    }

    public class EkkoBasicAttack3 : ISpellScript
    {
        AttackableUnit Target;
        public SpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
        {
            // TODO
        };

        public void OnSpellPreCast(ObjAIBase owner, Spell spell, AttackableUnit target, Vector2 start, Vector2 end)
        {
            if (target.HasBuff("EkkoPassiveSpellShieldCheck"))
            {
                OverrideAnimation(owner, "Attack_P_3", "Attack3");
            }
            else
            {
                OverrideAnimation(owner, "Attack3", "Attack_P_3");
            }
        }

        public void OnLaunchAttack(Spell spell)
        {
            //spell.CastInfo.Owner.SetAutoAttackSpell("NasusBasicAttack", false);
        }
    }
}
