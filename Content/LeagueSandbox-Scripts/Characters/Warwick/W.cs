using System.Linq;
using GameServerCore;
using GameServerCore.Enums;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using LeagueSandbox.GameServer.Scripting.CSharp;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.GameObjects.SpellNS;
using System.Numerics;

namespace Spells
{
    public class HuntersCall : ISpellScript
    {
        public SpellScriptMetadata ScriptMetadata => new SpellScriptMetadata()
        {
            // TODO
        };

        public void OnSpellPreCast(ObjAIBase owner, Spell spell, AttackableUnit target, Vector2 start, Vector2 end)
        {
            AddBuff("Hunterscall", 5.0f, 1, spell, owner, owner);

            foreach (var allyTarget in GetUnitsInRange(owner.Position, 1100, true)
                     .Where(x => x.Team != CustomConvert.GetEnemyTeam(owner.Team) && x is ObjAIBase && x != owner))
            {
                AddBuff("WarwickWAura", 5.0f, 1, spell, allyTarget, owner);
            }
        }
    }
}