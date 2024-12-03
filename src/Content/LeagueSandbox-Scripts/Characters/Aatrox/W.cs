using GameServerCore.Enums;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using LeagueSandbox.GameServer.Scripting.CSharp;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using LeagueSandbox.GameServer.GameObjects.SpellNS;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.GameObjects.SpellNS.Missile;
using LeagueSandbox.GameServer.GameObjects.SpellNS.Sector;
using System.Numerics;
using LeagueSandbox.GameServer.API;

namespace Spells
{
    public class AatroxW : ISpellScript
    {
        public SpellScriptMetadata ScriptMetadata => new SpellScriptMetadata()
        {
            TriggersSpellCasts = true,
        };
        Spell Spell;
        ObjAIBase Owner;
        int counter;

        public void OnActivate(ObjAIBase owner, Spell spell)
        {
            Spell = spell;
            Owner = spell.CastInfo.Owner;
            ApiEventManager.OnLevelUpSpell.AddListener(this, spell, OnLevelUp, true);
            ApiEventManager.OnLevelUpSpell.AddListener(this, spell, OnWLevelUp, true);
        }
        public void OnLevelUp(Spell spell)
        {
            ApiEventManager.OnLaunchAttack.AddListener(this, spell.CastInfo.Owner, OnLaunchAttack, false);
        }
        public void OnWLevelUp(Spell spell)
        {
            AddBuff("AatroxWLife", 25000f, 1, Spell, Owner, Owner);
            CreateTimer(0.1f, () =>
            {
                ApiEventManager.OnLevelUpSpell.RemoveListener(this);
            });
        }
        public void OnLaunchAttack(Spell spell)
        {
            if (!Owner.HasBuff("AatroxWPower"))
            {
                counter++;
                if (counter == 2)
                {
                    AddBuff("AatroxWONHLifeBuff", 25000f, 1, Spell, Owner, Owner);
                }
                if (counter == 3)
                {
                    counter = 0;
                }
            }
            else
            {
                counter++;
                if (counter == 2)
                {
                    AddBuff("AatroxWONHPowerBuff", 25000f, 1, Spell, Owner, Owner);
                }
                if (counter == 3)
                {
                    counter = 0;
                }
            }
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

        public void OnSpellPostCast(Spell spell)
        {
        }

        public void OnSpellChannel(Spell spell)
        {
        }

        public void OnSpellChannelCancel(Spell spell, ChannelingStopSource reason)
        {
        }

        public void OnSpellPostChannel(Spell spell)
        {
        }

        public void OnUpdate(float diff)
        {
        }
    }
    public class AatroxW2 : ISpellScript
    {
        Spell Spell;
        ObjAIBase Owner;
        public SpellScriptMetadata ScriptMetadata => new SpellScriptMetadata()
        {
            TriggersSpellCasts = true,
        };

        public void OnActivate(ObjAIBase owner, Spell spell)
        {
        }
        public void OnLevelUp(Spell spell)
        {
        }
        public void OnHitUnit(AttackableUnit target, bool isCrit)
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

        public void OnSpellPostCast(Spell spell)
        {
        }

        public void OnSpellChannel(Spell spell)
        {
        }

        public void OnSpellChannelCancel(Spell spell, ChannelingStopSource reason)
        {
        }

        public void OnSpellPostChannel(Spell spell)
        {
        }
        public void OnUpdate(float diff)
        {
        }
    }
    public class AatroxWONHAttackLife : ISpellScript
    {
        AttackableUnit Target;
        public SpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
        {
            TriggersSpellCasts = true,
            NotSingleTargetSpell = true,
            IsDamagingSpell = true,
            // TODO
        };
        public void OnActivate(ObjAIBase owner, Spell spell)
        {
        }
        public void TargetExecute(Spell spell, AttackableUnit target, SpellMissile missile, SpellSector sector)
        {
        }
        public void OnDeactivate(ObjAIBase owner, Spell spell)
        {
        }
        public void OnSpellPreCast(ObjAIBase owner, Spell spell, AttackableUnit target, Vector2 start, Vector2 end)
        {
            Target = target;
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

        public void OnSpellChannelCancel(Spell spell, ChannelingStopSource reason)
        {
        }

        public void OnSpellPostChannel(Spell spell)
        {
        }
        public void OnUpdate(float diff)
        {
        }
    }
    public class AatroxWONHAttackPower : ISpellScript
    {
        AttackableUnit Target;
        public SpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
        {
            TriggersSpellCasts = true,
            NotSingleTargetSpell = true,
            IsDamagingSpell = true,
            // TODO
        };
        public void OnActivate(ObjAIBase owner, Spell spell)
        {
        }
        public void TargetExecute(Spell spell, AttackableUnit target, SpellMissile missile, SpellSector sector)
        {
        }
        public void OnDeactivate(ObjAIBase owner, Spell spell)
        {
        }
        public void OnSpellPreCast(ObjAIBase owner, Spell spell, AttackableUnit target, Vector2 start, Vector2 end)
        {
            Target = target;
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

        public void OnSpellChannelCancel(Spell spell, ChannelingStopSource reason)
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