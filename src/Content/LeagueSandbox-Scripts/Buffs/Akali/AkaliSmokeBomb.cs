using GameServerCore.Enums;
using LeagueSandbox.GameServer.GameObjects.StatsNS;
using LeagueSandbox.GameServer.Scripting.CSharp;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.GameObjects;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.GameObjects.SpellNS;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using LeagueSandbox.GameServer.API;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.Buildings;

namespace Buffs
{
    class AkaliSmokeBomb : IBuffGameScript
    {
        Buff Bomb;
        Spell SmokeB;
        Particle ARC;
        Particle Smoke;
        ObjAIBase Akali;
        public BuffScriptMetaData BuffMetaData { get; set; } = new BuffScriptMetaData
        {
            BuffAddType = BuffAddType.REPLACE_EXISTING
        };

        public StatsModifier StatsModifier { get; private set; } = new StatsModifier();

        public void OnActivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {
            Bomb = buff;
            SmokeB = ownerSpell;
            Akali = ownerSpell.CastInfo.Owner as Champion;
            Smoke = AddParticle(Akali, null, "akali_smoke_bomb_tar.troy", unit.Position, buff.Duration);
            ARC = AddParticle(Akali, null, "akali_smoke_bomb_tar_team_green.troy", unit.Position, buff.Duration);
            ApiEventManager.OnLaunchAttack.AddListener(this, Akali, HoldStealth, false);
            for (byte i = 0; i < 4; i++)
            {
                ApiEventManager.OnSpellCast.AddListener(this, Akali.Spells[i], HoldStealth);
            }
        }
        public void HoldStealth(Spell spell)
        {
            AddBuff("AkaliHoldStealth", 0.5f, 1, SmokeB, Akali, Akali, false);
            if (Akali.HasBuff("AkaliWBuff"))
            {
                Akali.RemoveBuffsWithName("AkaliWBuff");
            }
        }
        public void OnDeactivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {
            if (Akali.HasBuff("AkaliWBuff"))
            {
                Akali.RemoveBuffsWithName("AkaliWBuff");
            }
            LogInfo("8 second timer finished, removing smoke bomb");
            RemoveParticle(Smoke);
            RemoveParticle(ARC);
        }
        public void OnUpdate(float diff)
        {
            if (GameServerCore.Extensions.IsVectorWithinRange(Akali.Position, Smoke.Position, 400f) && Bomb != null && Bomb.StackCount != 0 && !Bomb.Elapsed())
            {
                if (!Akali.HasBuff("AkaliWBuff") && !Akali.HasBuff("AkaliHoldStealth"))
                {
                    AddBuff("AkaliWBuff", 8f, 1, SmokeB, Akali, Akali, false);
                }
            }
            else
            {
                Akali.RemoveBuffsWithName("AkaliWBuff");
            }
            var units = GetUnitsInRange(Smoke.Position, 350f, true);
            for (int i = 0; i < units.Count; i++)
            {
                if (units[i].Team != Akali.Team && !(units[i] is ObjBuilding || units[i] is BaseTurret))
                {
                    if (!units[i].HasBuff("AkaliWBuff"))
                    {
                        AddBuff("AkaliWDebuff", 1f, 1, SmokeB, units[i], Akali, false);
                    }
                }
            }
        }
    }
}