using System.Numerics;
using GameServerCore.Enums;
using GameServerCore.Scripting.CSharp;
using LeaguePackets.Game.Events;
using LeagueSandbox.GameServer.API;
using LeagueSandbox.GameServer.GameObjects;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.Buildings;
using LeagueSandbox.GameServer.GameObjects.SpellNS;
using LeagueSandbox.GameServer.GameObjects.SpellNS.Missile;
using LeagueSandbox.GameServer.GameObjects.SpellNS.Sector;
using LeagueSandbox.GameServer.GameObjects.StatsNS;
using LeagueSandbox.GameServer.Scripting.CSharp;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;

namespace Buffs
{
    class MonkeyKingSpinToWin : IBuffGameScript
    {
        public BuffScriptMetaData BuffMetaData { get; set; } = new BuffScriptMetaData
        {
            BuffAddType = BuffAddType.RENEW_EXISTING
        };

        public StatsModifier StatsModifier { get; private set; } = new StatsModifier();

        ObjAIBase Owner;
        Particle p;
        Particle p2;
        Buff thisBuff;
        public SpellSector AOE;
        public void OnActivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {
            thisBuff = buff;
            Owner = ownerSpell.CastInfo.Owner;
            Owner.SetSpell("MonkeyKingSpinToWinLeave", 3, true);
            PlayAnimation(Owner, "spell4", 0.3f);
            ApiEventManager.OnSpellHit.AddListener(this, ownerSpell, TargetExecute, false);
            ApiEventManager.OnSpellCast.AddListener(this, ownerSpell.CastInfo.Owner.GetSpell("MonkeyKingSpinToWinLeave"), R2OnSpellCast);
            p = AddParticleTarget(Owner, unit, "MonkeyKing_Base_R_Cas.troy", unit, buff.Duration, 1);
            p2 = AddParticleTarget(Owner, unit, "MonkeyKing_Base_R_Cas_Glow.troy", unit, buff.Duration, 1);
            AOE = ownerSpell.CreateSpellSector(new SectorParameters
            {
                BindObject = Owner,
                Length = 270f,
                Tickrate = 1,
                CanHitSameTargetConsecutively = true,
                OverrideFlags = SpellDataFlags.AffectEnemies | SpellDataFlags.AffectNeutral | SpellDataFlags.AffectMinions | SpellDataFlags.AffectHeroes,
                Type = SectorType.Area
            });
        }

        public void TargetExecute(Spell spell, AttackableUnit target, SpellMissile missile, SpellSector sector)
        {
            var AP = Owner.Stats.AbilityPower.Total * 0.12f;
            var damage = 11f + (8f * Owner.GetSpell("MonkeyKingNimbus").CastInfo.SpellLevel - 1) + AP;
            target.TakeDamage(Owner, damage, DamageType.DAMAGE_TYPE_MAGICAL, DamageSource.DAMAGE_SOURCE_SPELLAOE, false);
            AddParticleTarget(Owner, target, "MonkeyKing_Base_R_Tar.troy", target);
            AddParticleTarget(Owner, target, "MonkeyKing_Base_R_Tar_Audio.troy", target);
        }

        public void R2OnSpellCast(Spell spell)
        {
            if (thisBuff != null && thisBuff.StackCount != 0 && !thisBuff.Elapsed())
            {
                thisBuff.DeactivateBuff();
            }
        }

        public void OnDeactivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {
            Owner.SetSpell("MonkeyKingSpinToWin", 3, true);
            StopAnimation(unit, "spell4", true, true, true);
            ApiEventManager.OnSpellHit.RemoveListener(this);
            RemoveParticle(p);
            RemoveBuff(thisBuff);
            RemoveParticle(p2);
            AOE.SetToRemove();
        }
    }
}