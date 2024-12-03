using LeagueSandbox.GameServer.Scripting.CSharp;
using System.Numerics;
using LeagueSandbox.GameServer.API;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using LeagueSandbox.GameServer.GameObjects.SpellNS;
using GameServerCore.Enums;
using LeagueSandbox.GameServer.GameObjects.SpellNS.Missile;
using LeagueSandbox.GameServer.GameObjects.SpellNS.Sector;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.Buildings;

namespace Spells
{
    public class NasusE : ISpellScript
    {
        public SpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
        {
            TriggersSpellCasts = true,
            IsDamagingSpell = false
        };

        private ObjAIBase Owner;
        private Spell Spell;


        public void OnActivate(ObjAIBase owner, Spell spell)
        {
            Owner = owner;
            Spell = spell;
        }

        public void TargetExecute(Spell spell, AttackableUnit target, SpellMissile missile, SpellSector sector)
        {
        }

        public void OnDeactivate(ObjAIBase owner, Spell spell)
        {
        }

        public void OnSpellPreCast(ObjAIBase owner, Spell spell, AttackableUnit target, Vector2 start, Vector2 end)
        {
            AddParticle(owner, null, "Nasus_Base_E_Warning.troy", end, 5f, 1);
            AddParticle(owner, null, "Nasus_Base_E_Staff_Swirl.troy", end, 5f, 1);
            AddParticle(owner, null, "Nasus_Base_E_Clynder_Scroll.troy", end, 5f, 1);
        }

        public void OnSpellCast(Spell spell)
        {
            var owner = spell.CastInfo.Owner;
        }

        public void OnSpellPostCast(Spell spell)
        {
            var owner = spell.CastInfo.Owner;
            if (owner is ObjAIBase c)
            {
                var ownerSkinID = c.SkinID;
                var targetPos = new Vector2(spell.CastInfo.TargetPosition.X, spell.CastInfo.TargetPosition.Z);
                var ownerPos = c.Position;
                var distance = Vector2.Distance(ownerPos, targetPos);
                FaceDirection(targetPos, c);
                if (distance > 1200.0)
                {
                    targetPos = GetPointFromUnit(c, 1150.0f);
                }
                var units = GetUnitsInRange(targetPos, 270f, true);
                for (int i = 0; i < units.Count; i++)
                {
                    if (units[i].Team != c.Team && !(units[i] is ObjBuilding || units[i] is BaseTurret))
                    {
                        var ELevel = c.GetSpell("NasusE").CastInfo.SpellLevel;
                        var damage = 55 + (40 * (ELevel - 1)) + (c.Stats.AbilityPower.Total * 0.6f);
                        units[i].TakeDamage(c, damage, DamageType.DAMAGE_TYPE_MAGICAL, DamageSource.DAMAGE_SOURCE_SPELL, false);
                    }
                }
                Minion NA = AddMinion(c, "TestCube", "TestCube", targetPos, c.Team, c.SkinID, true, false);
                AddBuff("NasusE", 5f, 1, spell, NA, NA, false);
            }
        }

        public void OnSpellChannel(Spell spell)
        {
        }

        public void OnSpellChannelCancel(Spell spell, ChannelingStopSource reason)
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