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
    public class TwitchHideInShadows : ISpellScript
    {
        ObjAIBase Twitch;
        public SpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
        {
            TriggersSpellCasts = true,
            IsDamagingSpell = true
        };
        public void OnSpellPostCast(Spell spell)
        {
            Twitch = spell.CastInfo.Owner as Champion;
            OverrideAnimation(Twitch, "Run_STEALTH", "Run");
            spell.SetCooldown(0, true);
            CreateTimer(1, () =>
            {
                Twitch.SetTargetUnit(null, true);
                AddBuff("TwitchHideInShadows", Twitch.Spells[0].CastInfo.SpellLevel + 9f, 1, spell, Twitch, Twitch);
            });
        }
    }
}