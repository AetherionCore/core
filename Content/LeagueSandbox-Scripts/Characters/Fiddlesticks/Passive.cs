using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using LeagueSandbox.GameServer.GameObjects.SpellNS;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;

namespace Passives
{
    public class Paranoia : ICharScript
    {
        private Spell originspell;
        private ObjAIBase yi;
        float stanceTime = 500;
        float stillTime = 0;
        bool beginStance = false;
        bool stance = false;

        public void OnActivate(ObjAIBase owner, Spell spell)
        {
            yi = owner;
            originspell = spell;
        }

        public void OnDeactivate(ObjAIBase owner, Spell spell)
        {
        }
        static internal bool isinvis = false;

        static internal float timer = 0;

        public void OnUpdate(float diff)
        {


            if (yi != null)
            {
                if (isinvis == true)
                {
                    AddBuff("DreadMS", 1.5f, 1, originspell, yi, yi);
                    //PlayAnimation(yi, "IDLE1", 100f);
                }
                else
                {
                    //StopAnimation(yi, "IDLE1");
                }

                // not moving
                if (yi.CurrentWaypoint == yi.Position)
                {
                    if (isinvis == false)
                    {
                        if (timer < 4000)
                        {
                            timer += diff;
                        }
                        if (timer >= 4000)
                        {
                            isinvis = true;
                        }
                    }
                }
                else
                {
                    if (isinvis == true)
                    {
                        isinvis = false;
                        timer = 0;
                    }
                }
            }

        }
    }
}