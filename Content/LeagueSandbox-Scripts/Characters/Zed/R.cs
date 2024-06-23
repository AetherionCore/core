using GameServerCore.Enums;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using LeagueSandbox.GameServer.Scripting.CSharp;
using System.Numerics;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using LeagueSandbox.GameServer.GameObjects.SpellNS;
using LeagueSandbox.GameServer.GameObjects.SpellNS.Missile;
using LeagueSandbox.GameServer.GameObjects.SpellNS.Sector;
using LeagueSandbox.GameServer.GameObjects;

namespace Spells
{
    public class ZedUlt : ISpellScript
    {
        private Spell Ult;
        private ObjAIBase Zed;
        public static AttackableUnit Target;
        public SpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
        {
            TriggersSpellCasts = true,
            IsDamagingSpell = true
        };
        public void OnSpellPreCast(ObjAIBase owner, Spell spell, AttackableUnit target, Vector2 start, Vector2 end)
        {
            Ult = spell;
            Target = target;
            Zed = owner = Ult.CastInfo.Owner as Champion;
            PlayAnimation(Zed, "Spell4");
            Zed.SetTargetUnit(null, true);
            Zed.CancelAutoAttack(false, true);
        }
        public void OnSpellCast(Spell spell)
        {
            AddBuff("ZedUltBuff", 1.5f, 1, Ult, Zed, Zed);
            AddBuff("ZedR2", 5.9f, 1, Ult, Zed, Zed);
            AddBuff("ZedRHandler", 6.0f, 1, Ult, Zed, Zed, false);
            AddBuff("ZedUlt", 0.7f, 1, Ult, Zed, Zed);
            AddParticleTarget(Zed, Target, "Zed_Base_R_tar_TargetMarker.troy", Target, 10f);
        }
        public void OnSpellPostCast(Spell spell) { Ult.SetCooldown(0.5f, true); }
    }

    public class ZedR2 : ISpellScript
    {
        public SpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
        {
            TriggersSpellCasts = true
        };
    }
}