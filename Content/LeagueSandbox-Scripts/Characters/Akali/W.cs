using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using LeagueSandbox.GameServer.Scripting.CSharp;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.GameObjects.SpellNS;
using System.Numerics;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;

namespace Spells
{
    public class AkaliSmokeBomb : ISpellScript
    {
        Vector2 Pos;
        Minion Smoke;
        ObjAIBase Akali;
        public SpellScriptMetadata ScriptMetadata => new SpellScriptMetadata() { TriggersSpellCasts = true };
        public void OnSpellPostCast(Spell spell)
        {
            Akali = spell.CastInfo.Owner as Champion;
            Pos = new Vector2(spell.CastInfo.TargetPosition.X, spell.CastInfo.TargetPosition.Z);
            Smoke = AddMinion(Akali, "TestCubeRender", "TestCubeRender", Pos, Akali.Team, Akali.SkinID, true, false);
            AddBuff("AkaliSmokeBomb", 8f, 1, spell, Smoke, Akali);
        }
    }
}