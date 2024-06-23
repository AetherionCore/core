using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using LeagueSandbox.GameServer.Scripting.CSharp;
using System.Numerics;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.GameObjects.SpellNS;
using LeagueSandbox.GameServer.API;

namespace Spells
{
    public class XenZhaoBattleCry : ISpellScript
    {
        Spell ComboTarget;
        ObjAIBase XinZhao;
        public SpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
        {
            TriggersSpellCasts = true,
            IsDamagingSpell = true
        };

        public void OnActivate(ObjAIBase owner, Spell spell)
        {
            ComboTarget = spell;
            XinZhao = owner = spell.CastInfo.Owner as Champion;
            ApiEventManager.OnLevelUpSpell.AddListener(this, spell, OnLevelUp, true);
        }
        public void OnLevelUp(Spell spell)
        {
            ApiEventManager.OnLaunchAttack.AddListener(this, XinZhao, OnLaunchAttack, false);
        }
        public void OnLaunchAttack(Spell spell)
        {
            AddBuff("XenZhaoBattleCryPassive", 3f, 1, ComboTarget, XinZhao, XinZhao);
        }
        public void OnSpellPreCast(ObjAIBase owner, Spell spell, AttackableUnit target, Vector2 start, Vector2 end)
        {
            AddBuff("XenZhaoBattleCry", 5f, 1, ComboTarget, XinZhao, XinZhao, false);
        }
    }
}