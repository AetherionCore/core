using GameServerCore.Enums;
using LeagueSandbox.GameServer.GameObjects.StatsNS;
using LeagueSandbox.GameServer.Scripting.CSharp;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.GameObjects;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.GameObjects.SpellNS;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.Buildings;
using LeagueSandbox.GameServer.API;
using System.Numerics;
using System.Linq;
using GameServerCore;

namespace Buffs
{
    internal class TwitchExpunge : IBuffGameScript
    {
        Buff Venom;
        float Damage;
        Spell Expunge;
        ObjAIBase Twitch;
        float Time = 900f;
        AttackableUnit Unit;
        public BuffScriptMetaData BuffMetaData { get; set; } = new BuffScriptMetaData
        {
            BuffAddType = BuffAddType.REPLACE_EXISTING
        };


        public StatsModifier StatsModifier { get; private set; } = new StatsModifier();

        public void OnActivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {
            Unit = unit;
            Venom = buff;
            Expunge = ownerSpell;
            Twitch = ownerSpell.CastInfo.Owner as Champion;

        }
        public void OnUpdate(float diff)
        {
            var units = GetUnitsInRange(Twitch.Position, 1150f, true);
            for (int i = 0; i < units.Count; i++)
            {
                if (units[i].Team != Twitch.Team && !(units[i] is ObjBuilding || units[i] is BaseTurret))
                {
                    if (units[i].HasBuff("TwitchDeadlyVenom"))
                    {
                        AddBuff("TwitchEParticle", 0.1f, 1, Expunge, Twitch, units[i] as ObjAIBase, false);
                    }
                }
            }
            if (!Twitch.HasBuff("TwitchEParticle"))
            {
                SealSpellSlot(Twitch, SpellSlotType.SpellSlots, 2, SpellbookType.SPELLBOOK_CHAMPION, true);
            }
            else
            {
                SealSpellSlot(Twitch, SpellSlotType.SpellSlots, 2, SpellbookType.SPELLBOOK_CHAMPION, false);
            }
        }
    }
}