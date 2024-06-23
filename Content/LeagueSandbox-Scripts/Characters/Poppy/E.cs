using GameServerCore.Enums;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using LeagueSandbox.GameServer.GameObjects;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.GameObjects.SpellNS;
using LeagueSandbox.GameServer.GameObjects.StatsNS;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.Scripting.CSharp;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using System.Numerics;
using System.Linq;
using LeagueSandbox.GameServer.GameObjects.SpellNS.Missile;
using LeagueSandbox.GameServer.GameObjects.SpellNS.Sector;
using LeagueSandbox.GameServer.API;

namespace Spells
{
    public class PoppyHeroicCharge : ISpellScript
    {
        public SpellScriptMetadata ScriptMetadata => new SpellScriptMetadata()
        {
            TriggersSpellCasts = true,
            CastingBreaksStealth = true,
            DoesntBreakShields = true,
            IsDamagingSpell = true,
            NotSingleTargetSpell = true,
        };
        AttackableUnit Target;

        public void OnActivate(ObjAIBase owner, Spell spell)
        {
            ApiEventManager.OnSpellHit.AddListener(this, spell, TargetExecute, true);
        }
        public SpellSector DamageSector;

        public void OnSpellPreCast(ObjAIBase owner, Spell spell, AttackableUnit target, Vector2 start, Vector2 end)
        {
            Target = target;
        }
        public void OnSpellCast(Spell spell)
        {
            var owner = spell.CastInfo.Owner;
            var to = Vector2.Normalize(Target.Position - owner.Position);
            owner.SetTargetUnit(null);
            ForceMovement(owner, "Spell1", new Vector2(Target.Position.X - to.X * 100f, Target.Position.Y - to.Y * 100f), 2000f, 500f, 0f, 0f);
            SpellCast(owner, 2, SpellSlotType.SpellSlots, true, Target, Target.Position);
        }
        public void TargetExecute(Spell spell, AttackableUnit target, SpellMissile missile, SpellSector sector)
        {
            var owner = spell.CastInfo.Owner;
            var to = Vector2.Normalize(Target.Position - owner.Position);
            ForceMovement(target, "Spell1", new Vector2(Target.Position.X - to.X * 100f, Target.Position.Y - to.Y * 100f), 2000f, 500f, 0f, 0f);
        }
    }
}