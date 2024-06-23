using GameServerCore.Enums;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.Scripting.CSharp;
using System.Numerics;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using LeagueSandbox.GameServer.GameObjects.SpellNS;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.Buildings.AnimatedBuildings;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.Buildings;
using LeagueSandbox.GameServer.GameObjects.SpellNS;
using LeagueSandbox.GameServer.GameObjects.SpellNS.Missile;
using LeagueSandbox.GameServer.GameObjects.SpellNS.Sector;
using LeagueSandbox.GameServer.API;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Runtime;
using System.Numerics;

namespace Spells
{
    public class DariusCleave : ISpellScript
    {
        float Damage;
        private Spell AOE;
        private ObjAIBase Darius;
        public SpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
        {
            TriggersSpellCasts = true,
            IsDamagingSpell = true
        };
        public void OnActivate(ObjAIBase owner, Spell spell)
        {
            AOE = spell;
            Darius = owner = spell.CastInfo.Owner as Champion;
            ApiEventManager.OnSpellHit.AddListener(this, AOE, TargetExecute, false);
        }
        public void OnSpellCast(Spell spell)
        {
            PlayAnimation(Darius, "Spell1", 0.5f);
            AOE.CreateSpellSector(new SectorParameters { Length = 400f, SingleTick = true, Type = SectorType.Area });
            AddParticleTarget(Darius, Darius, "darius_Base_Q_aoe_cast.troy", Darius, bone: "C_BuffBone_Glb_Center_Loc");
            AddParticleTarget(Darius, Darius, "darius_Base_Q_aoe_cast_mist.troy", Darius, bone: "C_BuffBone_Glb_Center_Loc");
        }
        public void TargetExecute(Spell spell, AttackableUnit target, SpellMissile missile, SpellSector sector)
        {
            AddParticleTarget(Darius, target, "darius_Base_Q_tar.troy", target, 1f);
            AddParticleTarget(Darius, target, "darius_Base_Q_tar_inner.troy", target, 1f);
            AddParticleTarget(Darius, target, "darius_Base_Q_impact_spray.troy", target, 1f);
            Damage = 35 + (35f * Darius.Spells[0].CastInfo.SpellLevel) + (Darius.Stats.AttackDamage.FlatBonus * 0.7f);
            if (target.Team != Darius.Team && !(target is ObjBuilding || target is BaseTurret))
            {
                if (Math.Abs(Vector2.Distance(target.Position, Darius.Position)) > 200)
                {
                    target.TakeDamage(Darius, Damage * 1.5f, DamageType.DAMAGE_TYPE_PHYSICAL, DamageSource.DAMAGE_SOURCE_ATTACK, false);
                    AddBuff("DariusHemo", 5.0f, 1, spell, target, Darius);
                    AddBuff("DariusHemo", 5.0f, 1, spell, target, Darius);
                }
                else
                {
                    target.TakeDamage(Darius, Damage, DamageType.DAMAGE_TYPE_PHYSICAL, DamageSource.DAMAGE_SOURCE_ATTACK, false);
                    AddBuff("DariusHemo", 5.0f, 1, spell, target, Darius);
                }
            }
        }
    }
}