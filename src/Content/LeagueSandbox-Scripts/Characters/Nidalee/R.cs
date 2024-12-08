using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using LeagueSandbox.GameServer.GameObjects.SpellNS;
using LeagueSandbox.GameServer.API;
using GameServerCore.Enums;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.Buildings;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.Scripting.CSharp;
using System.Numerics;

namespace Spells
{
    public class AspectOfTheCougar : ISpellScript
    {
        public SpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
        {
            TriggersSpellCasts = true
            // TODO
        };

        public void OnSpellCast(Spell spell)
        {
            var owner = spell.CastInfo.Owner;
            if (owner.Model == "Nidalee")
            {
                owner.ChangeModel("Nidalee_Cougar");
                owner.SetAutoAttackSpell("Nidalee_CougarBasicAttack2", false);
                (owner as ObjAIBase).SetSpell("Takedown", 0, true);
                (owner as ObjAIBase).SetSpell("Pounce", 1, true);
                (owner as ObjAIBase).SetSpell("Swipe", 2, true);
                (owner as ObjAIBase).SetSpell("AspectOfTheCougar", 3, true);
            }
            else
            {
                owner.ChangeModel("Nidalee");
                owner.SetAutoAttackSpell("NidaleeBasicAttack2", false);
                (owner as ObjAIBase).SetSpell("JavelinToss", 0, true);
                (owner as ObjAIBase).SetSpell("Bushwhack", 1, true);
                (owner as ObjAIBase).SetSpell("PrimalSurge", 2, true);
                (owner as ObjAIBase).SetSpell("AspectOfTheCougar", 3, true);
            }
        }
    }
}