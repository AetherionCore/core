using System.Numerics;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using LeagueSandbox.GameServer.Scripting.CSharp;
using GameServerCore.Scripting.CSharp;
using GameServerCore.Enums;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.GameObjects.SpellNS;
using LeagueSandbox.GameServer.GameObjects.SpellNS.Missile;
using LeagueSandbox.GameServer.GameObjects.SpellNS.Sector;
using LeagueSandbox.GameServer.API;
using System.Collections.Generic;
using LeagueSandbox.GameServer.GameObjects;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.Buildings;

namespace Spells
{
    public class BloodBoil : ISpellScript
    {
        AttackableUnit Target;

        public SpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
        {
            TriggersSpellCasts = true
            // TODO
        };

        public void OnSpellPreCast(ObjAIBase owner, Spell spell, AttackableUnit target, Vector2 start, Vector2 end)
        {
            Target = target;
        }

        public void OnSpellPostCast(Spell spell)
        {
            var owner = spell.CastInfo.Owner;
            var APratio = owner.Stats.AbilityPower.Total * 0.3f;
            var HPratio = (owner.Stats.HealthPoints.Total - owner.Stats.HealthPoints.BaseValue) * 0.05f;
            float Heal = 20f + spell.CastInfo.SpellLevel * 40f + APratio + HPratio;

            if (Target != owner)
            {
                AddBuff("BloodBoil", 12f, 1, spell, Target, owner);
            }
            AddBuff("BloodBoil", 12f, 1, spell, owner, owner);


        }
    }
}