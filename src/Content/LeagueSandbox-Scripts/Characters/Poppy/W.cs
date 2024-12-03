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
using GameServerLib.GameObjects.AttackableUnits;

namespace Spells
{
    public class PoppyParagonOfDemacia : ISpellScript
    {
        public SpellScriptMetadata ScriptMetadata => new SpellScriptMetadata()
        {
            TriggersSpellCasts = true,
            CastingBreaksStealth = true,
            DoesntBreakShields = true,
            IsDamagingSpell = true,
            NotSingleTargetSpell = true,
        };
        
        ObjAIBase daowner;
        Spell daspell;
        public SpellSector DamageSector;

        public void OnSpellPreCast(ObjAIBase owner, Spell spell, AttackableUnit target, Vector2 start, Vector2 end)
        {
            daowner = owner;
            daspell = spell;
            ApiEventManager.OnHitUnit.AddListener(this, owner, GivePoppySomething, false);
        }
        public void GivePoppySomething(DamageData damage)
        {
            AddBuff("PoppyParagonManager", 5f, 1, daspell, daowner, daowner, false);
        }
        public void AddPoppyWPassive(Spell spell)
        {
            AddBuff("PoppyParagonManager", 5f, 1, spell, daowner, daowner, false);
        }
        public void OnSpellCast(Spell spell)
        {
            ApiEventManager.OnHitUnit.RemoveListener(this, daowner);
            var owner = spell.CastInfo.Owner;
            AddBuff("PoppyParagonSpeed", 5f, 1, spell, owner, owner);
            for (int i = 1; i <= 10; i++)
            {
                AddBuff("PoppyParagonManager", 5f, 1, spell, daowner, daowner, false);
            }
        }
        public void OnSpellPostCast(Spell spell)
        {
            ApiEventManager.OnHitUnit.AddListener(this, daowner, GivePoppySomething, false);

        }
    }
}