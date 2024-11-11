using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.GameObjects.SpellNS;
using LeagueSandbox.GameServer.GameObjects;
using LeagueSandbox.GameServer.Scripting.CSharp;

namespace Spells
{
    public class LesserWraithBasicAttack : ISpellScript { public SpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata(); public void OnActivate(AttackableUnit unit, Buff buff, Spell ownerSpell) { } }
    public class LesserWraithBasicAttack2 : ISpellScript { public SpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata(); public void OnActivate(AttackableUnit unit, Buff buff, Spell ownerSpell) { } }
    public class LesserWraithBasicAttack3 : ISpellScript { public SpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata(); public void OnActivate(AttackableUnit unit, Buff buff, Spell ownerSpell) { } }
}