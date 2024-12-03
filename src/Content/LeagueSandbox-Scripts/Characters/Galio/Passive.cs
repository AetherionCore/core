using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using LeagueSandbox.GameServer.GameObjects.SpellNS;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.GameObjects.StatsNS;

namespace Passives
{
    public class GalioRunicSkin : ICharScript
    {
        private Spell originspell;
        private ObjAIBase ownermain;

        public void OnActivate(ObjAIBase owner, Spell spell)
        {
            originspell = spell;
            ownermain = owner;
        }

        public void OnDeactivate(ObjAIBase owner, Spell spell)
        {
        }

        float baseap = 0;
        float bonusap = 0;

        public void OnUpdate(float diff)
        {
            if (!ownermain.HasBuff("GalioPassiveBuff"))
            {
                AddBuff("GalioPassiveBuff", 0.5f, 1, originspell, ownermain, ownermain);
            }
        }
    }
}