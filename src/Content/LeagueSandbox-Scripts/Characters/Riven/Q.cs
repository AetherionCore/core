using GameServerCore.Enums;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using LeagueSandbox.GameServer.GameObjects;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.GameObjects.SpellNS;
using LeagueSandbox.GameServer.GameObjects.StatsNS;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.Scripting.CSharp;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using System.Numerics;
using LeagueSandbox.GameServer.GameObjects.SpellNS.Missile;
using LeagueSandbox.GameServer.GameObjects.SpellNS.Sector;
using LeagueSandbox.GameServer.API;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.Buildings;

namespace Spells
{
    public class RivenTriCleave : ISpellScript
    {
        int Dash = 0;
        float Damage;
        Spell TriCleave;
        string particles;
        string particles2;
        string particles3;
        string particles4;
        string particles5;
        string particles6;
        string particles7;
        string particles8;
        string particles9;
        string particles10;
        string particles11;
        ObjAIBase Riven;
        Buff TriCleaveBuff;
        public SpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
        {
            TriggersSpellCasts = true,
            IsDamagingSpell = true
        };

        public void OnSpellPreCast(ObjAIBase owner, Spell spell, AttackableUnit target, Vector2 start, Vector2 end)
        {
            TriCleave = spell;
            Riven = owner = spell.CastInfo.Owner as Champion;
            Riven.CancelAutoAttack(true);
            //Riven.SetTargetUnit(null, true);
            var x = GetChampionsInRange(end, 200, true);
            foreach (var champ in x)
            {
                if (champ.Team != owner.Team)
                {
                    FaceDirection(champ.Position, Riven);
                }
            }
            if (Riven.CurrentWaypoint != null)
            {
                FaceDirection(Riven.CurrentWaypoint, Riven);
            }
            switch (Riven.SkinID)
            {
                case 3:
                    particles = "exile_bunny_Q_01_detonate.troy";
                    particles2 = "exile_bunny_Q_02_detonate.troy";
                    particles3 = "exile_bunny_Q_03_detonate.troy";
                    particles4 = "RivenQ_tar.troy";
                    particles5 = "exile_Q_tar_01.troy";
                    particles6 = "exile_Q_tar_02.troy";
                    particles7 = "exile_Q_tar_03.troy";
                    particles8 = "exile_Q_tar_04.troy";
                    particles9 = "exile_bunny_Q_01_detonate_ult.troy";
                    particles10 = "exile_bunny_Q_02_detonate.troy";
                    particles11 = "exile_bunny_Q_03_detonate.troy";
                    break;

                case 4:
                    particles = "Riven_S2_Q_01_detonate.troy";
                    particles2 = "Riven_S2_Q_02_detonate.troy";
                    particles3 = "Riven_S2_Q_03_detonate.troy";
                    particles4 = "RivenQ_tar.troy";
                    particles5 = "Riven_S2_Q_tar_01.troy";
                    particles6 = "Riven_S2_Q_tar_02.troy";
                    particles7 = "Riven_S2_Q_tar_03.troy";
                    particles8 = "Riven_S2_Q_tar_04.troy";
                    particles9 = "Riven_S2_Q_01_detonate_ult.troy";
                    particles10 = "Riven_S2_Q_02_detonate_ult.troy";
                    particles11 = "Riven_S2_Q_03_detonate_ult.troy";
                    break;
                case 5:
                    particles = "Riven_Skin05_Q_01_Detonate.troy";
                    particles2 = "Riven_Skin05_Q_02_Detonate.troy";
                    particles3 = "Riven_Skin05_Q_03_Detonate.troy";
                    particles4 = "RivenQ_tar.troy";
                    particles5 = "Riven_Skin05_Q_Tar_01.troy";
                    particles6 = "Riven_Skin05_Q_Tar_01.troy";
                    particles7 = "Riven_Skin05_Q_Tar_01.troy";
                    particles8 = "Riven_Skin05_Q_Tar_01.troy";
                    particles9 = "Riven_Skin05_Q_01_Detonate_Ult.troy";
                    particles10 = "Riven_Skin05_Q_02_Detonate_Ult.troy";
                    particles11 = "Riven_Skin05_Q_03_Detonate_Ult.troy";
                    break;
                case 6:
                    particles = "Riven_Skin06_Q_01_Detonate.troy";
                    particles2 = "Riven_Skin06_Q_02_Detonate.troy";
                    particles3 = "Riven_Skin06_Q_03_Detonate.troy";
                    particles4 = "RivenQ_tar.troy";
                    particles5 = "Riven_Skin06_Q_Tar_01.troy";
                    particles6 = "Riven_Skin06_Q_Tar_02.troy";
                    particles7 = "Riven_Skin05_Q_Tar_03.troy";
                    particles8 = "Riven_Skin05_Q_Tar_04.troy";
                    particles9 = "Riven_Skin06_Q_01_Detonate.troy";
                    particles10 = "Riven_Skin06_Q_02_detonate_ult.troy";
                    particles11 = "Riven_Skin06_Q_03_detonate_ult.troy";
                    break;
                case 16:
                    particles = "Riven_Skin16_Q_01_Detonate.troy";
                    particles2 = "Riven_Skin16_Q_02_Detonate.troy";
                    particles3 = "Riven_Skin16_Q_03_Detonate.troy";
                    particles4 = "RivenQ_tar.troy";
                    particles5 = "Riven_Skin16_Q_Tar_01.troy";
                    particles6 = "Riven_Skin16_Q_Tar_02.troy";
                    particles7 = "Riven_Skin16_Q_Tar_03.troy";
                    particles8 = "Riven_Skin16_Q_Tar_04.troy";
                    particles9 = "Riven_Skin16_Q_01_Detonate.troy";
                    particles10 = "Riven_Skin16_Q_02_detonate_ult.troy";
                    particles11 = "Riven_Skin16_Q_03_detonate_ult.troy";
                    break;
                default:
                    particles = "exile_Q_01_detonate.troy";
                    particles2 = "exile_Q_02_detonate.troy";
                    particles3 = "exile_Q_03_detonate.troy";
                    particles4 = "RivenQ_tar.troy";
                    particles5 = "exile_Q_tar_01.troy";
                    particles6 = "exile_Q_tar_02.troy";
                    particles7 = "exile_Q_tar_03.troy";
                    particles8 = "exile_Q_tar_04.troy";
                    particles9 = "exile_Q_01_detonate_ult.troy";
                    particles10 = "exile_Q_02_detonate_ult.troy";
                    particles11 = "exile_Q_03_detonate_ult.troy";
                    break;
            }
        }
        public void OnSpellPostCast(Spell spell)
        {
            AddBuff("RivenTriCleave", 4.0f, 1, spell, Riven, Riven);
            ApiEventManager.OnMoveEnd.AddListener(this, Riven, OnMoveEnd, true);
            TriCleaveBuff = Riven.GetBuffWithName("RivenTriCleave");
            Damage = -10 + (20 * Riven.Spells[0].CastInfo.SpellLevel) + (Riven.Stats.AttackDamage.FlatBonus * 0.6f);
            switch (TriCleaveBuff.StackCount)
            {
                case 1:
                    Dash = 1;
                    PlayAnimation(Riven, "Spell1A", 0.75f);
                    ForceMovement(Riven, "Spell1A", GetPointFromUnit(Riven, 225), 700, 0, 0, 0);
                    if (Riven.HasBuff("RivenFengShuiEngine"))
                    {
                        AddParticle(Riven, Riven, "Riven_Base_Q_01_Wpn_Trail_Ult.troy", Riven.Position, bone: "chest");
                    }
                    else
                    {
                        AddParticle(Riven, Riven, "Riven_Base_Q_01_Wpn_Trail.troy", Riven.Position, bone: "chest");
                    }
                    return;
                case 2:
                    Dash = 2;
                    PlayAnimation(Riven, "Spell1B", 0.75f);
                    ForceMovement(Riven, "Spell1B", GetPointFromUnit(Riven, 225), 700, 0, 0, 0);
                    if (Riven.HasBuff("RivenFengShuiEngine"))
                    {
                        AddParticle(Riven, Riven, "Riven_Base_Q_02_Wpn_Trail_Ult.troy", Riven.Position, bone: "chest");
                    }
                    else
                    {
                        AddParticle(Riven, Riven, "Riven_Base_Q_02_Wpn_Trail.troy", Riven.Position, bone: "chest");
                    }
                    return;
                case 3:
                    Dash = 3;
                    PlayAnimation(Riven, "Spell1C", 0.75f);
                    ForceMovement(Riven, "Spell1C", GetPointFromUnit(Riven, 250), 700, 0, 50, 0);
                    if (Riven.HasBuff("RivenFengShuiEngine"))
                    {
                        AddParticle(Riven, Riven, "Riven_Base_Q_03_Wpn_Trail_Ult.troy", Riven.Position, size: -1);
                    }
                    else
                    {
                        AddParticle(Riven, Riven, "Riven_Base_Q_03_Wpn_Trail.troy", Riven.Position, size: -1);
                    }
                    TriCleaveBuff.DeactivateBuff();
                    return;
            }
        }
        public void AoeOneTwo(Spell spell)
        {
            var units = GetUnitsInRange(GetPointFromUnit(Riven, 80f), 260f, true);
            for (int i = 0; i < units.Count; i++)
            {
                if (units[i].Team != Riven.Team && !(units[i] is ObjBuilding || units[i] is BaseTurret))
                {
                    AddParticleTarget(Riven, units[i], particles4, units[i], 10f, 1, "");
                    AddParticleTarget(Riven, units[i], particles8, units[i], 10f, 1, "");
                    units[i].TakeDamage(Riven, Damage, DamageType.DAMAGE_TYPE_PHYSICAL, DamageSource.DAMAGE_SOURCE_SPELL, false);
                    if (Dash == 1)
                    {
                        AddParticleTarget(Riven, units[i], particles5, units[i], 10f, 1, "");
                    }
                    else
                    {
                        AddParticleTarget(Riven, units[i], particles6, units[i], 10f, 1, "");
                    }
                }
            }
        }
        public void AoeThree(Spell spell)
        {
            var Units = GetUnitsInRange(GetPointFromUnit(Riven, 80f), 300f, true);
            for (int i = 0; i < Units.Count; i++)
            {
                if (Units[i].Team != Riven.Team && !(Units[i] is ObjBuilding || Units[i] is BaseTurret))
                {
                    Units[i].TakeDamage(Riven, Damage, DamageType.DAMAGE_TYPE_PHYSICAL, DamageSource.DAMAGE_SOURCE_SPELL, false);
                    ForceMovement(Units[i], "RUN", new Vector2(Units[i].Position.X + 10f, Units[i].Position.Y + 10f), 20f, 0, 5.5f, 0);
                    AddBuff("Pulverize", 0.75f, 1, TriCleave, Units[i], Riven);
                    AddParticleTarget(Riven, Units[i], particles4, Units[i], 10f, 1, "");
                    AddParticleTarget(Riven, Units[i], particles7, Units[i], 10f, 1, "");
                    AddParticleTarget(Riven, Units[i], particles8, Units[i], 10f, 1, "");
                }
            }
        }
        public void OnMoveEnd(AttackableUnit unit)
        {
            switch (Dash)
            {
                case 1:
                    Riven.SkipNextAutoAttack();
                    if (Riven.HasBuff("RivenFengShuiEngine"))
                    {
                        AddParticle(Riven, null, particles9, GetPointFromUnit(Riven, 125f));
                    }
                    else
                    {
                        AddParticle(Riven, null, particles, GetPointFromUnit(Riven, 125f));
                    }
                    AoeOneTwo(TriCleave);
                    return;
                case 2:
                    Riven.SkipNextAutoAttack();
                    if (Riven.HasBuff("RivenFengShuiEngine"))
                    {
                        AddParticle(Riven, null, particles10, GetPointFromUnit(Riven, 125f));
                    }
                    else
                    {
                        AddParticle(Riven, null, particles2, GetPointFromUnit(Riven, 125f));
                    }
                    AoeOneTwo(TriCleave);
                    return;
                case 3:
                    Riven.SkipNextAutoAttack();
                    if (Riven.HasBuff("RivenFengShuiEngine"))
                    {
                        AddParticle(Riven, null, particles11, GetPointFromUnit(Riven, 125f));
                    }
                    else
                    {
                        AddParticle(Riven, null, particles3, GetPointFromUnit(Riven, 125f));
                    }
                    AoeThree(TriCleave);
                    return;
            }
        }
    }
}