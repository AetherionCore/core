using GameServerCore.Enums;
using LeagueSandbox.GameServer.GameObjects.StatsNS;
using LeagueSandbox.GameServer.Scripting.CSharp;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.GameObjects;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.GameObjects.SpellNS;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using LeagueSandbox.GameServer.API;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;

namespace CharScripts
{
     
    public class  CharScriptMasterYi : ICharScript

    {
        Spell Spell;
        public void OnActivate(ObjAIBase owner, Spell spell = null)

        {

            Spell = spell;
            {
                ApiEventManager.OnLaunchAttack.AddListener(this, owner, OnLaunchAttack, false);
            }
			var ownerSkinID = owner.SkinID;
            if (ownerSkinID == 2)
            {			
            AddParticleTarget(owner, owner, "MasterYi_Skin02_Glow_Sword_Blue.troy", owner, 25000f, 1, "BUFFBONE_Cstm_Sword1_loc");
			}
        }
        public void OnLaunchAttack(Spell spell)      
        {
            var owner = Spell.CastInfo.Owner;
            AddBuff("MasterYiPassive", 3f, 1, Spell, owner, owner);
        }     
        public void OnDeactivate(ObjAIBase owner, Spell spell = null)
        {
            ApiEventManager.OnHitUnit.RemoveListener(this);
        }
        public void OnUpdate(float diff)
        {
        }
    }
}