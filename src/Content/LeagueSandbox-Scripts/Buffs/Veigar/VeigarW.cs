using System.Numerics;
using GameServerCore.Enums;
using GameServerCore.Scripting.CSharp;
using GameServerLib.GameObjects.AttackableUnits;
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
    class VeigarW : IBuffGameScript
    {
        public BuffScriptMetaData BuffMetaData { get; set; } = new BuffScriptMetaData
        {
            BuffAddType = BuffAddType.STACKS_AND_OVERLAPS,
            MaxStacks = byte.MaxValue
        };

        public StatsModifier StatsModifier { get; private set; } = new StatsModifier();
        SpellSector DamageSector;
        string particles2;

        public void OnActivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {
            ApiEventManager.OnSpellHit.AddListener(this, ownerSpell, TargetExecute, true);

            DamageSector = ownerSpell.CreateSpellSector(new SectorParameters
            {
                Length = 225f,
                Lifetime = -1.0f,
                Tickrate = 0,
                SingleTick = true,
                OverrideFlags = SpellDataFlags.AffectMinions | SpellDataFlags.AffectEnemies | SpellDataFlags.AffectFriends | SpellDataFlags.AffectBarracksOnly,
                Type = SectorType.Area
            });

            switch ((unit as ObjAIBase).SkinID)
            {
                case 8:
                    particles2 = "Veigar_Skin08_W_aoe_explosion.troy";
                    break;

                case 4:
                    particles2 = "Veigar_Skin04_W_aoe_explosion.troy";
                    break;

                default:
                    particles2 = "Veigar_Base_W_aoe_explosion.troy";
                    break;
            }
        }

        public void OnDeactivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {
            DamageSector.ExecuteTick();
            AddParticle(ownerSpell.CastInfo.Owner, null, particles2, DamageSector.Position);
        }

        public void TargetExecute(Spell spell, AttackableUnit target, SpellMissile missile, SpellSector sector)
        {
            var owner = spell.CastInfo.Owner;
            if (!(target is BaseTurret || target is LaneTurret || target.Team == owner.Team || target == owner))
            {
                var ownerSkinID = owner.SkinID;
                var APratio = owner.Stats.AbilityPower.Total;
                var damage = 120f + ((spell.CastInfo.SpellLevel - 1) * 50) + APratio;
                var StacksPerLevel = spell.CastInfo.SpellLevel;

                target.TakeDamage(spell.CastInfo.Owner, damage, DamageType.DAMAGE_TYPE_MAGICAL, DamageSource.DAMAGE_SOURCE_SPELL, false);
            }
        }
    }
}