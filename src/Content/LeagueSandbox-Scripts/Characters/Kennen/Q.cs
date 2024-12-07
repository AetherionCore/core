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

namespace Spells
{
    public class KennenShurikenHurlMissile1 : ISpellScript
    {
        ObjAIBase Owner;
        public SpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
        {
            MissileParameters = new MissileParameters
            {
                Type = MissileType.Circle
            },
            IsDamagingSpell = true
        };

        public void OnActivate(ObjAIBase owner, Spell spell)
        {
            ApiEventManager.OnSpellHit.AddListener(this, spell, TargetExecute, false);
        }

        public void OnSpellPreCast(ObjAIBase owner, Spell spell, AttackableUnit target, Vector2 start, Vector2 end)
        {
            FaceDirection(end, owner);
        }

        public void TargetExecute(Spell spell, AttackableUnit target, SpellMissile missile, SpellSector sector)
        {
            var owner = spell.CastInfo.Owner;
            var APratio = owner.Stats.AbilityPower.Total * 0.75f;
            var damage = 75 + (spell.CastInfo.SpellLevel - 1) * 40 + APratio; //kennen q damage = 75 + spell level * 40 + 75% ap
            target.TakeDamage(owner, damage, DamageType.DAMAGE_TYPE_MAGICAL, DamageSource.DAMAGE_SOURCE_SPELLAOE, false);

            AddParticleTarget(owner, target, "Kennen_ts_tar.troy", target, bone: "C_BuffBone_Glb_Center_Loc");
            AddBuff("KennenMarkOfStorm", 6f, 1, spell, target, owner);

            if (target.GetBuffWithName("KennenMarkOfStorm").StackCount == 3) //remove mos if stacks reach 3
            {
                // AddBuff("Stun", 1f, 1, spell, target, owner); //applies stun buff correctly
                target.RemoveBuffsWithName("KennenMarkOfStorm");
            }
            missile.SetToRemove();
        }
    }
}