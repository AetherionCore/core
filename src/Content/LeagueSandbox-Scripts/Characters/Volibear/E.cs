using GameServerCore;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.Scripting.CSharp;
using System.Linq;
using System.Numerics;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using GameServerCore.Enums;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using LeagueSandbox.GameServer.GameObjects.SpellNS;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;
using static LeaguePackets.Game.Common.CastInfo;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.Buildings.AnimatedBuildings;
using Buffs;

namespace Spells
{
    public class VolibearE : ISpellScript
    {
        public SpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
        {
            TriggersSpellCasts = true
            // TODO
        };

        public void OnActivate(ObjAIBase owner, Spell spell)
        {
        }

        public void OnDeactivate(ObjAIBase owner, Spell spell)
        {
        }

        public void OnSpellPreCast(ObjAIBase owner, Spell spell, AttackableUnit target, Vector2 start, Vector2 end)
        {
        }

        public void OnSpellCast(Spell spell)
        {
        }

        public void OnSpellPostCast(Spell spell)
        {
            spell.CastInfo.Owner.PlayAnimation("Spell3", 0.5f);

            AddParticle(spell.CastInfo.Owner, spell.CastInfo.Owner, "Volibear_E_cas_blast.troy", spell.CastInfo.Owner.Position);

            float damage = new float[] { 60, 105, 150, 195, 240 }[spell.CastInfo.SpellLevel - 1];
            float slow = new float[] { 0.30f, 0.35f, 0.40f, 0.45f, 0.50f }[spell.CastInfo.SpellLevel - 1];

            var units = GetUnitsInRange(spell.CastInfo.Owner.Position, 300, true).Where(x => x.Team == CustomConvert.GetEnemyTeam(spell.CastInfo.Owner.Team));

            foreach (var target in units)
            {
                if (target is AttackableUnit && (target is not BaseTurret or Inhibitor or Nexus) && spell.CastInfo.Owner != target)
                {
                    var buff = (IBuffGameScript)AddBuff("Slow", 3.0f, 1, spell, target, spell.CastInfo.Owner) as Slow;
                    buff.SetSlowMod(slow);
                    target.TakeDamage(spell.CastInfo.Owner, damage, DamageType.DAMAGE_TYPE_MAGICAL, DamageSource.DAMAGE_SOURCE_SPELL, false);

                    //CreateTimer(3.0f, () => { target.Stats.MoveSpeed.PercentBonus = 0f; });
                }
            }
        }

        public void OnSpellChannel(Spell spell)
        {
        }

        public void OnSpellChannelCancel(Spell spell, ChannelingStopSource source)
        {
        }

        public void OnSpellPostChannel(Spell spell)
        {
        }

        public void OnUpdate(float diff)
        {
        }
    }
}