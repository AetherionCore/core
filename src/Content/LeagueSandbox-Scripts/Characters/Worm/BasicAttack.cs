using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.GameObjects.SpellNS;
using LeagueSandbox.GameServer.GameObjects;
using LeagueSandbox.GameServer.Scripting.CSharp;
using GameServerCore.Enums;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using System.Numerics;
using LeagueSandbox.GameServer.Handlers;

namespace Spells
{
    public class WormBasicAttack : ISpellScript
    {
        public SpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
        {
            TriggersSpellCasts = true,
            IsDamagingSpell = true,
            MissileParameters = new MissileParameters { Type = MissileType.Target }
        };
        public void OnActivate(AttackableUnit unit, Buff buff, Spell ownerSpell) { }
    }

    public class WormAttack : ISpellScript
    {
        public SpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
        {
            TriggersSpellCasts = true,
            IsDamagingSpell = true,
            MissileParameters = new MissileParameters { Type = MissileType.Target }
        };
        public void OnActivate(AttackableUnit unit, Buff buff, Spell ownerSpell) { }
    }
}