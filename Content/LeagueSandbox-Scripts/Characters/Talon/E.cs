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
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.Buildings;

namespace Spells
{
    public class TalonCutthroat : ISpellScript
    {
        string P1;
        string P2;
        string P3;
        private Spell Cutthroat;
        private ObjAIBase Talon;
        private AttackableUnit Target;
        public SpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
        {
            TriggersSpellCasts = true,
            IsDamagingSpell = true
        };

        public void OnSpellPreCast(ObjAIBase owner, Spell spell, AttackableUnit target, Vector2 start, Vector2 end)
        {
            Target = target;
            Cutthroat = spell;
            Talon = owner = spell.CastInfo.Owner as Champion;
            switch (Talon.SkinID)
            {
                case 5:
                    P1 = "Talon_Skin05_E_Cas.troy";
                    P2 = "Talon_Skin05_E_cas_trail.troy";
                    P3 = "Talon_Skin05_E_cas_trail_long.troy";
                    break;
                default:
                    P1 = "Talon_E_cast.troy";
                    P2 = ".troy";
                    P3 = ".troy";
                    break;
            }
            AddParticle(Talon, null, P1, Talon.Position, lifetime: 10f, 1f);
            AddParticle(Talon, null, P2, Talon.Position, lifetime: 1f);
            AddParticle(Talon, null, P3, Talon.Position, lifetime: 1f);
            if (Talon.HasBuff("TalonShadowAssaultBuff")) { Talon.RemoveBuffsWithName("TalonShadowAssaultBuff"); }
        }
        public void OnSpellCast(Spell spell)
        {
            var targetPos = GetPointFromUnit(Talon, System.Math.Abs(Vector2.Distance(Target.Position, Talon.Position)) + 125);
            TeleportTo(Talon, targetPos.X, targetPos.Y);
            AddBuff("TalonESlow", 0.25f, 1, spell, Target, Talon);
            AddBuff("TalonDamageAmp", 3f, 1, spell, Target, Talon);
            AddParticleTarget(Talon, Target, "talon_E_tar.troy", Target, 10f);
        }
    }
}