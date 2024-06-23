using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using LeagueSandbox.GameServer.Scripting.CSharp;
using System.Numerics;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.GameObjects.SpellNS;
using GameServerCore.Enums;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.Buildings;

namespace Spells
{
    public class XenZhaoParry : ISpellScript
    {
        float Damage;
        ObjAIBase XinZhao;
        AttackableUnit Target;
        public SpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
        {
            TriggersSpellCasts = true,
            IsDamagingSpell = true
        };
        public void OnSpellPostCast(Spell spell)
        {
            XinZhao = spell.CastInfo.Owner as Champion;
            Damage = (75 * spell.CastInfo.SpellLevel) + (XinZhao.Stats.AttackDamage.FlatBonus * 2f);
            var units = GetUnitsInRange(XinZhao.Position, 450f, true);
            for (int i = 0; i < units.Count; i++)
            {
                if (!(units[i].Team == XinZhao.Team || units[i] is BaseTurret || units[i] is ObjBuilding))
                {
                    FaceDirection(XinZhao.Position, units[i]);
                    AddParticleTarget(XinZhao, units[i], "xenZiou_utl_tar.troy", units[i]);
                    AddParticleTarget(XinZhao, units[i], "xenZiou_utl_tar_02.troy", units[i]);
                    AddParticleTarget(XinZhao, units[i], "xenZiou_utl_tar_03.troy", units[i]);
                    units[i].TakeDamage(XinZhao, Damage, DamageType.DAMAGE_TYPE_PHYSICAL, DamageSource.DAMAGE_SOURCE_SPELLAOE, false);
                    if (!units[i].HasBuff("XenZhaoPuncture")) { ForceMovement(units[i], null, GetPointFromUnit(units[i], -450.0f), 1800, 0, 80, 0); }
                }
            }
        }
    }
}