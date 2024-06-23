using GameServerCore.Enums;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using LeagueSandbox.GameServer.Scripting.CSharp;
using System.Numerics;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.API;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using LeagueSandbox.GameServer.GameObjects.SpellNS;
using LeagueSandbox.GameServer.GameObjects.SpellNS.Missile;
using LeagueSandbox.GameServer.GameObjects.SpellNS.Sector;

namespace Spells
{
    public class JaxBasicAttack : ISpellScript
    {
        bool Has;
        string Animation;
        public SpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
        {
            TriggersSpellCasts = true,
            IsDamagingSpell = true
        };

        public void OnSpellPreCast(ObjAIBase owner, Spell spell, AttackableUnit target, Vector2 start, Vector2 end)
        {
            if (owner.HasBuff("JaxEmpowerTwo") || (owner.HasBuff("JaxRelentlessAttack") && owner.GetBuffWithName("JaxRelentlessAttack").StackCount == 2))
            {
                Has = true;
                Animation = "Spell2";
            }
            if (owner.HasBuff("JaxEmpowerTwo")) { Has = true; Animation = "Spell2"; }
            if (owner.HasBuff("JaxRelentlessAttack") && owner.GetBuffWithName("JaxRelentlessAttack").StackCount == 2) { Has = true; Animation = "Spell4"; }
            if (Has == true) { OverrideAnimation(owner, Animation, "Attack1"); Has = false; } else { OverrideAnimation(owner, "Attack1", Animation); }
        }
    }
    public class JaxBasicAttack2 : ISpellScript
    {
        bool Has;
        string Animation;
        public SpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
        {
            TriggersSpellCasts = true,
            IsDamagingSpell = true
        };

        public void OnSpellPreCast(ObjAIBase owner, Spell spell, AttackableUnit target, Vector2 start, Vector2 end)
        {
            if (owner.HasBuff("JaxEmpowerTwo") || (owner.HasBuff("JaxRelentlessAttack") && owner.GetBuffWithName("JaxRelentlessAttack").StackCount == 2))
            {
                Has = true;
                Animation = "Spell2";
            }
            if (owner.HasBuff("JaxEmpowerTwo")) { Has = true; Animation = "Spell2"; }
            if (owner.HasBuff("JaxRelentlessAttack") && owner.GetBuffWithName("JaxRelentlessAttack").StackCount == 2) { Has = true; Animation = "Spell4"; }
            if (Has == true) { OverrideAnimation(owner, Animation, "Attack2"); Has = false; } else { OverrideAnimation(owner, "Attack2", Animation); }
        }
    }
    public class JaxCritAttack : ISpellScript
    {
        bool Has;
        string Animation;
        public SpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
        {
            TriggersSpellCasts = true,
            IsDamagingSpell = true
        };

        public void OnSpellPreCast(ObjAIBase owner, Spell spell, AttackableUnit target, Vector2 start, Vector2 end)
        {
            if (owner.HasBuff("JaxEmpowerTwo") || (owner.HasBuff("JaxRelentlessAttack") && owner.GetBuffWithName("JaxRelentlessAttack").StackCount == 2))
            {
                Has = true;
                Animation = "Spell2";
            }
            if (owner.HasBuff("JaxEmpowerTwo")) { Has = true; Animation = "Spell2"; }
            if (owner.HasBuff("JaxRelentlessAttack") && owner.GetBuffWithName("JaxRelentlessAttack").StackCount == 2) { Has = true; Animation = "Spell4"; }
            if (Has == true) { OverrideAnimation(owner, Animation, "Crit"); Has = false; } else { OverrideAnimation(owner, "Crit", Animation); }
        }
    }
    public class JaxCounterStrikeAttack : ISpellScript
    {
        bool Has;
        string Animation;
        public SpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
        {
            TriggersSpellCasts = true,
            IsDamagingSpell = true
        };

        public void OnSpellPreCast(ObjAIBase owner, Spell spell, AttackableUnit target, Vector2 start, Vector2 end)
        {
            if (owner.HasBuff("JaxEmpowerTwo") || (owner.HasBuff("JaxRelentlessAttack") && owner.GetBuffWithName("JaxRelentlessAttack").StackCount == 2))
            {
                Has = true;
                Animation = "Spell2";
            }
            if (owner.HasBuff("JaxEmpowerTwo")) { Has = true; Animation = "Spell2"; }
            if (owner.HasBuff("JaxRelentlessAttack") && owner.GetBuffWithName("JaxRelentlessAttack").StackCount == 2) { Has = true; Animation = "Spell4"; }
            if (Has == true) { OverrideAnimation(owner, Animation, "CounterStrikeAttack"); Has = false; } else { OverrideAnimation(owner, "CounterStrikeAttack", Animation); }
        }
    }
}