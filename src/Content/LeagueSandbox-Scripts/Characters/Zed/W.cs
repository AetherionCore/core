using GameServerCore.Enums;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using LeagueSandbox.GameServer.Scripting.CSharp;
using System.Numerics;
using LeagueSandbox.GameServer.API;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using LeagueSandbox.GameServer.GameObjects.SpellNS;
using LeagueSandbox.GameServer.GameObjects.SpellNS.Missile;
using LeagueSandbox.GameServer.GameObjects;

namespace Spells
{
    public class ZedShadowDash : ISpellScript
    {
        byte Slot;
        float Dist;
        Vector2 Pos;
        ObjAIBase Zed;
        public SpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
        {
            AutoFaceDirection = false,
            TriggersSpellCasts = true
            // TODO
        };

        public void OnActivate(ObjAIBase owner, Spell spell)
        {
            Zed = owner = spell.CastInfo.Owner as Champion;
            ApiEventManager.OnLevelUpSpell.AddListener(this, spell, OnLevelUp, false);
        }
        public void OnLevelUp(Spell spell)
        {
            AddBuff("ZedWPassiveBuff", 25000.0f, 1, spell, Zed, Zed, true);
        }

        public void OnSpellCast(Spell spell)
        {
            Pos = new Vector2(spell.CastInfo.TargetPosition.X, spell.CastInfo.TargetPosition.Z);
            Dist = System.Math.Abs(Vector2.Distance(Pos, Zed.Position));
            Slot = Dist <= 400 ? (byte)4 : (byte)9;
            SpellCast(Zed, Slot, SpellSlotType.ExtraSlots, Pos, Pos, true, Vector2.Zero);
            PlayAnimation(Zed, "Spell2_Cast", timeScale: 0.6f);
        }
    }

    public class ZedShadowDashMissile : ISpellScript
    {
        public SpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
        {
            // TODO
        };
        Spell S;
        float Damage;
        ObjAIBase Zed;
        Buff HandlerBuff;
        Minion Shadow;

        public void OnSpellPreCast(ObjAIBase owner, Spell spell, AttackableUnit target, Vector2 start, Vector2 end)
        {
            S = spell;
            Zed = owner = spell.CastInfo.Owner as Champion;
            HandlerBuff = AddBuff("ZedWHandler", 4.0f, 1, spell, Zed, Zed);
            AddBuff("ZedW2", 4.0f, 1, spell, Zed, Zed);

            if (Shadow != null)
            {
                var buff = Shadow.GetBuffWithName("ZedWShadowBuff");

                if (buff != null)
                {
                    buff.DeactivateBuff();
                }
            }

            var missile = spell.CreateSpellMissile(new MissileParameters
            {
                Type = MissileType.Circle,
                OverrideEndPosition = end
            });
            ApiEventManager.OnSpellMissileEnd.AddListener(this, missile, OnMissileEnd, true);
        }

        public void OnMissileEnd(SpellMissile missile)
        {
            if (HandlerBuff != null)
            {
                Shadow = (HandlerBuff.BuffScript as Buffs.ZedWHandler).ShadowSpawn();
            }
        }
    }

    public class ZedW2 : ISpellScript
    {
        public SpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
        {
            TriggersSpellCasts = true
        };
    }
    public class ZedUltMissile : ZedShadowDashMissile
    {
    }
}