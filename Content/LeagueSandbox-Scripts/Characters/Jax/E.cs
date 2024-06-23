using System.Numerics;
using GameServerCore.Enums;
using LeagueSandbox.GameServer.Scripting.CSharp;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using LeagueSandbox.GameServer.GameObjects.SpellNS;
using GameServerCore.Enums;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using LeagueSandbox.GameServer.GameObjects;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.GameObjects.SpellNS;
using LeagueSandbox.GameServer.GameObjects.StatsNS;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.Scripting.CSharp;


namespace Spells
{
    public class JaxCounterStrike : ISpellScript
    {
        ObjAIBase Jax;
        public SpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
        {
            TriggersSpellCasts = true,
            IsDamagingSpell = true
        };
        public void OnSpellPostCast(Spell spell)
        {
            Jax = spell.CastInfo.Owner as Champion;
            if (!Jax.HasBuff("JaxCounterStrike"))
            {
                spell.SetCooldown(1f, true);
                AddBuff("JaxCounterStrike", 2f, 1, spell, Jax, Jax, false);
            }
            else
            {
                RemoveBuff(Jax, "JaxCounterStrike");
            }
        }
    }
}