using GameServerCore.Enums;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using LeagueSandbox.GameServer.Scripting.CSharp;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using LeagueSandbox.GameServer.GameObjects.SpellNS;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;
using System.Linq;
using System.Numerics;

namespace Spells
{
    public class Pulverize : ISpellScript
    {
        public SpellScriptMetadata ScriptMetadata => new SpellScriptMetadata()
        {
            TriggersSpellCasts = true
            // TODO
        };

        // w=>q missing

        public void OnActivate(ObjAIBase owner, Spell spell)
        {
            _owner = owner;
        }

        public void OnDeactivate(ObjAIBase owner, Spell spell)
        {
        }
        AttackableUnit _owner;
        public void OnSpellPreCast(ObjAIBase owner, Spell spell, AttackableUnit target, Vector2 start, Vector2 end)
        {
            AddBuff("Trample", 4.0f, 1, spell, owner, owner);
        }

        public void OnSpellCast(Spell spell)
        {
            var owner = spell.CastInfo.Owner as Champion;
            PlayAnimation(owner, "SPELL1");
        }

        public void OnSpellPostCast(Spell spell)
        {
            var owner = spell.CastInfo.Owner as Champion;
            AddParticle(owner, null, "Pulverize_cas.troy", owner.Position, lifetime: 0.5f);

            var spellLevel = owner.GetSpell("Pulverize").CastInfo.SpellLevel;

            var ap = spell.CastInfo.Owner.Stats.AbilityPower.Total * 0.5f;
            var damage = 20 + spellLevel * 40 + ap;
            foreach (var enemy in GetUnitsInRange(owner.Position, 375, true)
                .Where(x => x.Team != owner.Team))
            {
                if (enemy is ObjAIBase)
                {
                    if (!(enemy is BaseTurret))
                    {
                        enemy.TakeDamage(owner, damage, DamageType.DAMAGE_TYPE_MAGICAL, DamageSource.DAMAGE_SOURCE_SPELL, false);
                        AddBuff("Pulverize", 1.0f, 1, spell, enemy, owner);
                    }
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
        bool cd = false;
        public void OnUpdate(float diff)
        {
            //ONCE AGAIN i am going to do spell logic in a separate spell
            // what a bummer
            var dmg = 6 + _owner.Stats.Level;
            var ap = _owner.Stats.AbilityPower.Total * 0.1f;
            //LogDebug(Buffs.Trample.trample.ToString());
            if (Trample.trample == true)
            {
                if (cd == false)
                {
                    var units = GetUnitsInRange(_owner.Position, 300, true);
                    foreach (var unit in units)
                    {
                        if (unit.Team != _owner.Team)
                        {
                            unit.TakeDamage(_owner, dmg + ap, DamageType.DAMAGE_TYPE_MAGICAL, DamageSource.DAMAGE_SOURCE_SPELLAOE, false);
                            cd = true;
                            CreateTimer(0.9f, () => { cd = false; });
                        }
                    }
                }
            }

        }
    }
}