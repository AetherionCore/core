using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.API;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.GameObjects.SpellNS;

namespace CharScripts
{
    public class CharScriptEkko : ICharScript
    {
        Spell Spell;
        AttackableUnit Target;

        public void OnActivate(ObjAIBase owner, Spell spell = null)

        {
            Spell = spell;
            {
                ApiEventManager.OnLaunchAttack.AddListener(this, owner, OnLaunchAttack, false);
            }
        }

        public void OnLaunchAttack(Spell spell)
        {
            var owner = spell.CastInfo.Owner;
            Target = spell.CastInfo.Targets[0].Unit;
            if (!Target.HasBuff("EkkoPassiveSlow"))
            {
                AddBuff("EkkoPassive", 4f, 1, spell, Target, owner);
            }
            else
            {
            }
        }
    }
}