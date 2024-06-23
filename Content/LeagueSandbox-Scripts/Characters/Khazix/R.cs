using System.Numerics;
using GameServerCore.Enums;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using LeagueSandbox.GameServer.Scripting.CSharp;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using LeagueSandbox.GameServer.GameObjects.SpellNS;
using LeagueSandbox.GameServer.GameObjects.SpellNS.Missile;

namespace Spells
{
    public class KhazixR : ISpellScript
    {
        ObjAIBase Khazix;
        public SpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
        {
            TriggersSpellCasts = true
        };
        public void OnSpellPreCast(ObjAIBase owner, Spell spell, AttackableUnit target, Vector2 start, Vector2 end)
        {
            Khazix = spell.CastInfo.Owner as Champion;
            AddParticleTarget(Khazix, Khazix, "khazix_base_r_cas", Khazix);
        }
        public void OnSpellPostCast(Spell spell)
        {
            if (!Khazix.HasBuff("KhazixR"))
            {
                Khazix.Spells[3].SetCooldown(0.5f, true);
                AddBuff("KhazixR", 15.0f, 1, spell, Khazix, Khazix);
            }
            else
            {
                Khazix.RemoveBuffsWithName("KhazixR");
            }
            AddBuff("KhazixRStealth", 1.0f, 1, spell, Khazix, Khazix);
        }
    }
}