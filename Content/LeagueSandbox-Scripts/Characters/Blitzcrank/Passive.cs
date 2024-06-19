using System.Numerics;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using LeagueSandbox.GameServer.API;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using LeagueSandbox.GameServer.GameObjects.SpellNS;

namespace Passives
{
    public class ManaBarrierIcon : ICharScript
    {
        private Spell originspell;
        private ObjAIBase _owner;

        public void OnActivate(ObjAIBase owner, Spell spell)
        {
            originspell = spell;
            _owner = owner;
            ApiEventManager.OnTakeDamageByAnother.AddListener(this, owner, TargetExecute, false);
        }
        bool haspassive = false;
        bool timer = false;
        bool oldpos = false;
        Vector2 oldposV;
        private void TargetExecute(AttackableUnit unit1, AttackableUnit unit2)
        {
                if (unit1.Stats.CurrentHealth < unit1.Stats.HealthPoints.Total * 0.2f)
                {
                    var unit1champ = unit1 as Champion;
                    if (timer != true)
                    {
                        timer = true;
                        var shieldamt = unit1.Stats.ManaPoints.Total * 0.5f;
                        /* Shielding not implemented, yet.
                        unit1champ.ApplyShield(unit1, shieldamt, true, true, false);
                        CreateTimer(10.0f, () => { unit1champ.ApplyShield(unit1, -shieldamt, true, true, false); });
                        CreateTimer(90f, () => { timer = false; });
                        */
                    }
                }
            }

        public void OnDeactivate(ObjAIBase owner, Spell spell)
        {
        }

        public void OnUpdate(float diff)
        {
        }
    }
}