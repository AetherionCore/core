using GameServerCore.Enums;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using LeagueSandbox.GameServer.Scripting.CSharp;
using System.Numerics;
using LeagueSandbox.GameServer.API;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using LeagueSandbox.GameServer.GameObjects.SpellNS;
using LeagueSandbox.GameServer.GameObjects.SpellNS.Missile;
using LeagueSandbox.GameServer.GameObjects.SpellNS.Sector;

namespace Spells
{
    public class LissandraR : ISpellScript
    {
        ObjAIBase Lissandra;
        AttackableUnit Target;
        public SpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
        {
            TriggersSpellCasts = true,
            IsDamagingSpell = true
        };
        public void OnSpellPostCast(Spell spell)
        {
            Lissandra = spell.CastInfo.Owner as Champion;
            Target = Lissandra.Spells[3].CastInfo.Targets[0].Unit;
            if (Lissandra == Target)
            {
                AddBuff("LissandraRSelf", 2.5f, 1, spell, Lissandra, Lissandra);
            }
            else
            {
                SpellCast(Lissandra, 3, SpellSlotType.ExtraSlots, false, Target, Vector2.Zero);
            }
        }
    }
    public class LissandraREnemy : ISpellScript
    {
        ObjAIBase Lissandra;
        AttackableUnit Target;
        public SpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
        {
            TriggersSpellCasts = true,
            IsDamagingSpell = true
        };
        public void OnSpellPostCast(Spell spell)
        {
            Lissandra = spell.CastInfo.Owner as Champion;
            Target = Lissandra.Spells[3].CastInfo.Targets[0].Unit;
            AddBuff("LissandraREnemy2", 1.5f, 1, spell, Target, Lissandra);
        }
    }
}