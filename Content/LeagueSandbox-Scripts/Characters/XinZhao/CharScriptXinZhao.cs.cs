using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using LeagueSandbox.GameServer.Scripting.CSharp;
using System.Numerics;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.GameObjects.SpellNS;
using LeagueSandbox.GameServer.API;
using GameServerCore.Enums;
using GameServerLib.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.Buildings;


namespace CharScripts
{
    public class CharScriptXinZhao : ICharScript
    {
        Spell Passive;
        ObjAIBase XinZhao;
        AttackableUnit Target;
        public void OnActivate(ObjAIBase owner, Spell spell = null)
        {
            Passive = spell;
            XinZhao = owner as Champion;
            ApiEventManager.OnHitUnit.AddListener(this, XinZhao, OnHitUnit, false);
        }
        public void OnHitUnit(DamageData damageData)
        {
            Target = damageData.Target;
            if (!(Target is ObjBuilding || Target is BaseTurret) && !XinZhao.HasBuff("XinZhaoPassive"))
            {
                AddBuff("XenZhaoPuncture", 3.0f, 1, Passive, Target, XinZhao);
            }
            if (!(Target is ObjBuilding || Target is BaseTurret) && Target.HasBuff("XenZhaoPuncture"))
            {
                AddBuff("XenZhaoPuncture", 3.0f, 1, Passive, Target, XinZhao);
            }
        }
    }
}