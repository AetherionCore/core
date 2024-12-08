using GameServerCore.Enums;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using LeagueSandbox.GameServer.Scripting.CSharp;
using LeagueSandbox.GameServer.API;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using LeagueSandbox.GameServer.GameObjects.SpellNS;
using LeagueSandbox.GameServer.GameObjects.SpellNS.Missile;
using LeagueSandbox.GameServer.GameObjects.SpellNS.Sector;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.Buildings;
using GameServerLib.GameObjects.AttackableUnits;
using System.Numerics;
using LeagueSandbox.GameServer.GameObjects.StatsNS;

namespace CharScripts
{
    public class CharScriptKhazix : ICharScript
    {
        Spell Spell;
        public StatsModifier StatsModifier { get; private set; } = new StatsModifier();

        public void OnActivate(ObjAIBase owner, Spell spell = null)
        {
            Spell = spell;
            {
                ApiEventManager.OnLevelUp.AddListener(this, owner, OnLevelUp, true);
            }
        }

        public void OnLevelUp(AttackableUnit owner)
        {
            var Owner = Spell.CastInfo.Owner;
            var ownerSkinID = Owner.SkinID;
            AddParticleTarget(Owner, Owner, "Khazix_Base_P_Buf_Left.troy", Owner, 25000f, 1, "L_HAND");
            AddParticleTarget(Owner, Owner, "Khazix_Base_P_Buf_Right.troy", Owner, 25000f, 1, "R_HAND");
            CreateTimer(0.1f, () =>
            {
                ApiEventManager.OnLevelUp.RemoveListener(this);
            });
        }
    }
}