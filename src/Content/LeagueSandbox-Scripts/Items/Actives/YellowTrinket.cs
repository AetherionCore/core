using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using LeagueSandbox.GameServer.Scripting.CSharp;
using GameServerCore.Enums;
using System.Numerics;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.GameObjects.SpellNS;

namespace ItemSpells
{
    public class TrinketTotemLvl1 : ISpellScript
    {
        Minion ward;
        public SpellScriptMetadata ScriptMetadata => new SpellScriptMetadata()
        {
            TriggersSpellCasts = true,
        };
        public void OnSpellPreCast(ObjAIBase owner, Spell spell, AttackableUnit target, Vector2 start, Vector2 end)
        {
            var Cursor = new Vector2(spell.CastInfo.TargetPosition.X, spell.CastInfo.TargetPosition.Z);
            var current = new Vector2(owner.Position.X, owner.Position.Y);
            var distance = Cursor - current;
            Vector2 truecoords;
            if (distance.Length() > 500f)
            {
                distance = Vector2.Normalize(distance);
                var range = distance * 500f;
                truecoords = current + range;
            }
            else
            {
                truecoords = Cursor;
            }

            ward = AddMinion(owner, "YellowTrinket", "YellowTrinket", truecoords, owner.Team, owner.SkinID, false, true);
            ward.SetCollisionRadius(0.0f);
            ward.SetStatus(StatusFlags.Ghosted, true);
            AddParticle(owner, null, "Global_Trinket_Yellow.troy", truecoords);
            AddBuff("YellowTrinket", 65f, 1, spell, ward, ward);
        }
    }
}