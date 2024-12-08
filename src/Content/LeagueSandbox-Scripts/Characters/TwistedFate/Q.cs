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
using GameServerLib.GameObjects.AttackableUnits;
using LeaguePackets.Game.Common;
using LeagueSandbox.GameServer.GameObjects.StatsNS;

namespace Spells
{
    public class WildCards : ISpellScript
    {
        public SpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
        {
            TriggersSpellCasts = true,
            IsDamagingSpell = true
        };


        public void OnSpellPreCast(ObjAIBase owner, Spell spell, AttackableUnit target, Vector2 start, Vector2 end)
        {
            //PlayAnimation(owner, "Spell1");
        }

        public void OnSpellCast(Spell spell)
        {
        }

        public void OnSpellPostCast(Spell spell)
        {
            var owner = spell.CastInfo.Owner as Champion;
            for (int bladeCount = 0; bladeCount <= 2; bladeCount++)
            {
                var end = GetPointFromUnit(owner, 1450f, (-22.5f + (bladeCount * 22.5f)));
                SpellCast(owner, 6, SpellSlotType.ExtraSlots, end, end, true, Vector2.Zero);
            }
        }
    }
}

public class SealFateMissile : ISpellScript
{
    public SpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
    {
        MissileParameters = new MissileParameters
        {
            Type = MissileType.Circle
        },
        IsDamagingSpell = true,
        TriggersSpellCasts = true

        // TODO
    };
    public List<AttackableUnit> UnitsHit = new List<AttackableUnit>();

    public void OnActivate(ObjAIBase owner, Spell spell)
    {
        ApiEventManager.OnSpellHit.AddListener(this, spell, TargetExecute, false);
    }

    public void OnSpellPreCast(ObjAIBase owner, Spell spell, AttackableUnit target, Vector2 start, Vector2 end)
    {
        UnitsHit.Clear();
    }

    public void TargetExecute(Spell spell, AttackableUnit target, SpellMissile missile, SpellSector sector)
    {
        var owner = spell.CastInfo.Owner;
        var spellLevel = owner.GetSpell("WildCards").CastInfo.SpellLevel;
        var APratio = (owner.Stats.AttackDamage.Total - owner.Stats.AttackDamage.BaseValue) * 0.65f;
        var damage = 10 + (50f * spellLevel) + APratio;
        if (!UnitsHit.Contains(target))
        {
            UnitsHit.Add(target);
            target.TakeDamage(owner, damage, DamageType.DAMAGE_TYPE_MAGICAL, DamageSource.DAMAGE_SOURCE_ATTACK, false);
            AddParticleTarget(owner, target, "Roulette_hit.troy", target, 1f);
        }
    }
}