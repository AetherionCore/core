using GameServerCore.Enums;
using LeagueSandbox.GameServer.Scripting.CSharp;
using LeagueSandbox.GameServer.API;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using LeagueSandbox.GameServer.GameObjects.SpellNS;
using System.Numerics;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using LeagueSandbox.GameServer.GameObjects.SpellNS.Missile;
using LeagueSandbox.GameServer.GameObjects.SpellNS.Sector;
using System.Collections.Generic;
using GameServerCore;

namespace Spells
{
    public class GalioIdolOfDurand : ISpellScript
    {
        public SpellScriptMetadata ScriptMetadata => new SpellScriptMetadata()
        {
            TriggersSpellCasts = true
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

            var x = GetChampionsInRange(spell.CastInfo.Owner.Position, 800, true);
            foreach (var units in x)
            {
                if (units.Team != spell.CastInfo.Owner.Team)
                {
                    var target = units;
                    var owner = spell.CastInfo.Owner;
                    //target.TakeDamage(owner, 50, DamageType.DAMAGE_TYPE_PHYSICAL, DamageSource.DAMAGE_SOURCE_RAW, true);
                    var ap = owner.Stats.AbilityPower.Total * 0.60;
                    float damage = (float)(ap + 200 + (owner.Spells[3].CastInfo.SpellLevel * 100));
                    target.TakeDamage(owner, damage, DamageType.DAMAGE_TYPE_MAGICAL, DamageSource.DAMAGE_SOURCE_SPELL, false);
                    FaceDirection(owner.Position, target);
                    var xy = GetPointFromUnit(target, 300);

                    var xy1 = target as ObjAIBase;
                    xy1.SetTargetUnit(null);

                    ForceMovement(target, "run", xy, 10, 0, 0, 0);
                    AddBuff("VeigarEventHorizon", 2.0f, 1, spell, target, owner);
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