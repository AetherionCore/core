using System;
using GameServerCore.Enums;
using LeagueSandbox.GameServer.Scripting.CSharp;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using LeagueSandbox.GameServer.GameObjects.SpellNS;
using LeagueSandbox.GameServer.GameObjects.SpellNS.Missile;
using LeagueSandbox.GameServer.GameObjects.SpellNS.Sector;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using LeagueSandbox.GameServer.API;

namespace Spells
{
    public class Parley : ISpellScript
    {
        float Damage;
        ObjAIBase owner;
        SpellMissile Missile;
        public SpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
        {
            TriggersSpellCasts = true,
            IsDamagingSpell = true,
        };
        public void OnActivate(ObjAIBase owner, Spell spell)
        {
            owner = spell.CastInfo.Owner as Champion;
            ApiEventManager.OnSpellHit.AddListener(this, spell, TargetExecute, false);
        }
        public void OnSpellPostCast(Spell spell)
        {
            Missile = spell.CreateSpellMissile(new MissileParameters { Type = MissileType.Target });
        }

        public void TargetExecute(Spell spell, AttackableUnit target, SpellMissile missile, SpellSector sector)
        {
            owner = spell.CastInfo.Owner as Champion;
            var isCrit = new Random().Next(0, 100) < owner.Stats.CriticalChance.Total;
            var baseDamage = new[] { 20, 45, 70, 95, 120 }[spell.CastInfo.SpellLevel - 1] + owner.Stats.AttackDamage.Total;
            var damage = isCrit ? baseDamage * owner.Stats.CriticalDamage.Total / 100 : baseDamage;
            var goldIncome = new[] { 4, 5, 6, 7, 8 }[spell.CastInfo.SpellLevel - 1];
            if (target != null && !target.IsDead)
            {
                target.TakeDamage(owner, damage, DamageType.DAMAGE_TYPE_PHYSICAL, DamageSource.DAMAGE_SOURCE_ATTACK,
                    false);
                
                AddBuff("Scurvy", 3f, 1, spell, target, owner, false);

                if (target.IsDead)
                {
                    owner.Stats.Gold += goldIncome;
                    var manaCost = new float[] { 50, 55, 60, 65, 70 }[spell.CastInfo.SpellLevel - 1];
                    var newMana = owner.Stats.CurrentMana + manaCost / 2;
                    var maxMana = owner.Stats.ManaPoints.Total;
                    if (newMana >= maxMana)
                    {
                        owner.Stats.CurrentMana = maxMana;
                    }
                    else
                    {
                        owner.Stats.CurrentMana = newMana;
                    }
                }

                missile.SetToRemove();
            }
        }
    }
}