using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using LeagueSandbox.GameServer.Scripting.CSharp;
using GameServerCore.Enums;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using LeagueSandbox.GameServer.GameObjects.SpellNS;
using System.Numerics;


namespace Spells
{
    public class Crystallize : ISpellScript
    {
        public SpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
        {
            TriggersSpellCasts = true
            // TODO
        };

        public void OnActivate(ObjAIBase owner, Spell spell)
        {
        }

        public void ExecuteSpell(Spell spell)
        {
        }

        public void OnDeactivate(ObjAIBase owner, Spell spell)
        {
        }

        public void OnSpellPreCast(ObjAIBase owner, Spell spell, AttackableUnit target, Vector2 start, Vector2 end)
        {
        }

        public void OnSpellCast(Spell spell)
        {
        }

        // THIS IS THE WORK OF THE DEVIL
        // SORRY ABOUT THAT

      
        public void OnSpellPostCast(Spell spell)
        {
             var spellLVL = spell.CastInfo.SpellLevel;
            var owner = spell.CastInfo.Owner;
            var spellPos = new Vector2(spell.CastInfo.TargetPosition.X, spell.CastInfo.TargetPosition.Z);
            var facingPos = GetPointFromUnit(spell.CastInfo.Owner, 950);

            Minion m = AddMinion((Champion)owner, "AniviaIceBlock", "AniviaIceBlock", spellPos, targetable: false);
            FaceDirection(facingPos, m);

            var LPos = GetPointFromUnit(m, spellLVL * 80, -90);
            var RPos = GetPointFromUnit(m, spellLVL * 80, 90);

            Minion L = AddMinion((Champion)owner, "AniviaIceBlock", "AniviaIceBlock", LPos, targetable: false);
            Minion R = AddMinion((Champion)owner, "AniviaIceBlock", "AniviaIceBlock", RPos, targetable: false);

            var L1Pos = GetPointFromUnit(m, spellLVL * 20, -90);
            var R1Pos = GetPointFromUnit(m, spellLVL * 20, 90);

            Minion L1 = AddMinion((Champion)owner, "AniviaIceBlock", "AniviaIceBlock", L1Pos, targetable: false);
            Minion R1 = AddMinion((Champion)owner, "AniviaIceBlock", "AniviaIceBlock", R1Pos, targetable: false);

            var L2Pos = GetPointFromUnit(m, spellLVL * 40, -90);
            var R2Pos = GetPointFromUnit(m, spellLVL * 40, 90);

            Minion L2 = AddMinion((Champion)owner, "AniviaIceBlock", "AniviaIceBlock", L2Pos, targetable: false);
            Minion R2 = AddMinion((Champion)owner, "AniviaIceBlock", "AniviaIceBlock", R2Pos, targetable: false);

            var L3Pos = GetPointFromUnit(m, spellLVL * 60, -90);
            var R3Pos = GetPointFromUnit(m, spellLVL * 60, 90);

            Minion L3 = AddMinion((Champion)owner, "AniviaIceBlock", "AniviaIceBlock", L3Pos, targetable: false);
            Minion R3 = AddMinion((Champion)owner, "AniviaIceBlock", "AniviaIceBlock", R3Pos, targetable: false);

            m.SetCollisionRadius(1.0f);
            L.SetCollisionRadius(1.0f);
            R.SetCollisionRadius(1.0f);
            L1.SetCollisionRadius(1.0f);
            R1.SetCollisionRadius(1.0f);
            L2.SetCollisionRadius(1.0f);
            R2.SetCollisionRadius(1.0f);
            L3.SetCollisionRadius(1.0f);
            R3.SetCollisionRadius(1.0f);
            m.Stats.Size.BaseValue = 10;
            L.Stats.Size.BaseValue = 10;
            R.Stats.Size.BaseValue = 10;
            L1.Stats.Size.BaseValue = 10;
            R1.Stats.Size.BaseValue = 10;
            L2.Stats.Size.BaseValue = 10;
            R2.Stats.Size.BaseValue = 10;
            L3.Stats.Size.BaseValue = 10;
            R3.Stats.Size.BaseValue = 10;
            CreateTimer(5.0f, () =>
            {
                m.TakeDamage(owner, 50000, DamageType.DAMAGE_TYPE_TRUE, DamageSource.DAMAGE_SOURCE_INTERNALRAW, false);
                L.TakeDamage(owner, 50000, DamageType.DAMAGE_TYPE_TRUE, DamageSource.DAMAGE_SOURCE_INTERNALRAW, false);
                R.TakeDamage(owner, 50000, DamageType.DAMAGE_TYPE_TRUE, DamageSource.DAMAGE_SOURCE_INTERNALRAW, false);
                L1.TakeDamage(owner, 50000, DamageType.DAMAGE_TYPE_TRUE, DamageSource.DAMAGE_SOURCE_INTERNALRAW, false);
                R1.TakeDamage(owner, 50000, DamageType.DAMAGE_TYPE_TRUE, DamageSource.DAMAGE_SOURCE_INTERNALRAW, false);
                L2.TakeDamage(owner, 50000, DamageType.DAMAGE_TYPE_TRUE, DamageSource.DAMAGE_SOURCE_INTERNALRAW, false);
                R2.TakeDamage(owner, 50000, DamageType.DAMAGE_TYPE_TRUE, DamageSource.DAMAGE_SOURCE_INTERNALRAW, false);
                L3.TakeDamage(owner, 50000, DamageType.DAMAGE_TYPE_TRUE, DamageSource.DAMAGE_SOURCE_INTERNALRAW, false);
                R3.TakeDamage(owner, 50000, DamageType.DAMAGE_TYPE_TRUE, DamageSource.DAMAGE_SOURCE_INTERNALRAW, false);
            });
        }

        public void OnSpellChannel(Spell spell)
        {
        }

        public void OnSpellChannelCancel(Spell spell, ChannelingStopSource source)
        {
        }

        public void OnSpellPostChannel(Spell spell)
        {
        }



        public void OnUpdate(float diff)
        {
        }
    }
}