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
using LeagueSandbox.GameServer.GameObjects.SpellNS.Missile;
using LeagueSandbox.GameServer.GameObjects.SpellNS.Sector;
using LeagueSandbox.GameServer.API;

namespace Spells
{
    public class SpellFlux : ISpellScript
    {

        ObjAIBase _owner;
        AttackableUnit _targ;

        public SpellScriptMetadata ScriptMetadata => new SpellScriptMetadata()
        {
            TriggersSpellCasts = true,
            IsDamagingSpell = true,
            MissileParameters = new MissileParameters
            {
                Type = MissileType.Arc
            },
            CastTime = 0.25f
        };

        public void OnActivate(ObjAIBase owner, Spell spell)
        {
            _owner = owner;
            ApiEventManager.OnSpellCast.AddListener(this, spell, TargetExecute);
        }
        float ap;
        float dmg;
        public void TargetExecute(Spell spell)
        {

            ap = spell.CastInfo.Owner.Stats.AbilityPower.Total;
            dmg = 30 + (float)(ap / 3) + spell.CastInfo.SpellLevel * 20;

            _targ = spell.CastInfo.Targets[0].Unit;
            var x = _owner.GetSpell("SpellFlux").CreateSpellMissile(ScriptMetadata.MissileParameters);
            ApiEventManager.OnSpellMissileEnd.AddListener(this, x, OnSpellEnd, true);
        }
        public void OnSpellEnd(SpellMissile mis)
        {

            _owner.CancelAutoAttack(false);

            _targ.TakeDamage(_owner, dmg, DamageType.DAMAGE_TYPE_MAGICAL, DamageSource.DAMAGE_SOURCE_SPELL, false);

            if (_owner.HasBuff("DesperatePower"))
            {
                AddParticle(_owner, _targ, "DesperatePower_aoe.troy", _targ.Position);
                var u = GetUnitsInRange(_targ.Position, 300, true);
                foreach (var unit in u)
                {
                    if (unit.Team != _owner.Team)
                    {
                        unit.TakeDamage(_owner, dmg / 2, DamageType.DAMAGE_TYPE_MAGICAL, DamageSource.DAMAGE_SOURCE_SPELL, false);
                    }
                }
            }
        }

        public void OnSpellPreCast(ObjAIBase owner, Spell spell, AttackableUnit target, Vector2 start, Vector2 end)
        {
            owner.Spells[0].LowerCooldown((float)1.5);   //Q
            owner.Spells[1].LowerCooldown((float)1.5);   //W
            //owner.Spells[2].LowerCooldown((float)1.5);   //E
            owner.Spells[3].LowerCooldown((float)1.5);   //R
        }

       
    }
}