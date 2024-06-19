using GameServerCore.Enums;
using LeagueSandbox.GameServer.Scripting.CSharp;
using LeagueSandbox.GameServer.API;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using LeagueSandbox.GameServer.GameObjects.SpellNS;
using System.Numerics;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using GameServerLib.GameObjects.AttackableUnits;
using GameServerCore;

namespace Spells
{
    public class DravenSpinning : ISpellScript
    {
        public SpellScriptMetadata ScriptMetadata => new SpellScriptMetadata()
        {
            TriggersSpellCasts = true
        };

        public void OnActivate(ObjAIBase owner, Spell spell)
        {
            ApiEventManager.OnKill.AddListener(this, owner, AddStackOnKill, false);
        }

        public void AddStackOnKill(DeathData dat)
        {
            if (dat.Unit is Champion)
            {
                if (_owner.GetBuffWithName("DravenPassiveStacks") != null)
                {
                    var DoubleGold = _owner.GetBuffWithName("DravenPassiveStacks").StackCount * 2;
                    var BaseGold = 50;
                    _owner.Stats.Gold += DoubleGold + BaseGold;
                    _owner.GetBuffWithName("DravenPassiveStacks").DeactivateBuff();
                }
            }
        }

        public void OnDeactivate(ObjAIBase owner, Spell spell)
        {
        }

        ObjAIBase _owner;
        Spell _spell;

        bool fixCrash = false;

        public void OnSpellPreCast(ObjAIBase owner, Spell spell, AttackableUnit target, Vector2 start, Vector2 end)
        {
            if (fixCrash == false)
            {
                ApiEventManager.OnLaunchAttack.AddListener(this, owner, OnLaunchAttack, false);
                fixCrash = true;
            }

            _owner = owner;
            _spell = spell;
            AddBuff("DravenSpinning", 5.8f, 1, spell, owner, owner);
        }

        public void RemoveStacks(int var)
        {
            int x = 0;
            foreach (var swag in _owner.GetBuffsWithName("DravenSpinning"))
            {
                if (x < var)
                {
                    x++;
                    swag.DeactivateBuff();
                }
            }
        }

        bool add1buff = false;
        bool add1buffPassive = false;

        public void ReturnAttack(AttackableUnit attacker, AttackableUnit target)
        {
            var pos = GetPointFromUnit(_owner, _owner.Stats.MoveSpeed.Total / 1.5f);
            AddParticle(_owner, null, "Draven_Base_Q_catch_indicator.troy", pos, lifetime: 1.0f);
            AddParticle(_owner, null, "Draven_Base_Q_reticle_self.troy", pos, lifetime: 1.0f);
            AddParticle(_owner, null, "Draven_Base_Q_reticle.troy", pos, lifetime: 1.0f);
            CreateTimer(0.5f, () => { SpellCast(_owner, 2, SpellSlotType.ExtraSlots, pos, pos, false, target.Position); });
            CreateTimer(1.0f, () =>
            {
                if (Extensions.IsVectorWithinRange(_owner.Position, pos, 200))
                {
                    add1buff = true;
                    add1buffPassive = true;
                }
            });
        }

        public void OnLaunchAttack(Spell spell)
        {
            float damage = spell.CastInfo.Owner.Stats.AttackDamage.Total;
            if (_owner.HasBuff("DravenSpinning"))
            {
                if (_owner.TargetUnit is BaseTurret)
                {
                    SpellCast(_owner, 2, SpellSlotType.ExtraSlots, spell.CastInfo.Targets[0].Unit.Position, spell.CastInfo.Targets[0].Unit.Position, false, Vector2.Zero);
                    RemoveStacks(1);
                    add1buff = true;
                }
                else
                {
                    SpellCast(_owner, 2, SpellSlotType.ExtraSlots, spell.CastInfo.Targets[0].Unit.Position, spell.CastInfo.Targets[0].Unit.Position, false, Vector2.Zero);
                    int i = 0;
                    while (i < _owner.GetBuffWithName("DravenSpinning").StackCount)
                    {
                        i++;
                        if (i == _owner.GetBuffWithName("DravenSpinning").StackCount)
                        {
                            RemoveStacks(1);
                        }
                    }
                    ApiEventManager.OnHitUnitByAnother.AddListener(this, _owner, ReturnAttack, true);
                }
            }
            else
            {
                SpellCast(spell.CastInfo.Owner, 2, SpellSlotType.ExtraSlots, spell.CastInfo.Targets[0].Unit.Position, spell.CastInfo.Targets[0].Unit.Position, false, Vector2.Zero);
            }
        }

        public void OnSpellCast(Spell spell)
        {
        }

        public void OnSpellPostCast(Spell spell)
        {
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
            if (add1buff == true)
            {
                AddBuff("DravenSpinning", 5.8f, 1, _spell, _owner, _owner);
                add1buff = false;
            }
            if (add1buffPassive == true)
            {
                AddBuff("DravenPassiveStacks", float.MaxValue, 1, _spell, _owner, _owner, true);
                add1buffPassive = false;
            }
        }
    }
}
