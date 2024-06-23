using GameServerCore.Enums;
using System.Collections.Generic;
using System.Numerics;
using static LeagueSandbox.GameServer.API.ApiMapFunctionManager;
using GameServerLib.GameObjects;
using LeagueSandbox.GameServer.GameObjects;

namespace MapScripts.Map4
{
    public class NeutralMinionSpawn
    {
        private static bool forceSpawn;

        public static Dictionary<MonsterCamp, List<Monster>> MonsterCamps = new Dictionary<MonsterCamp, List<Monster>>();

        public static void InitializeCamps()
        {
            //Blue Side Wraiths
            var blue_Wraiths = CreateJungleCamp(new Vector3(5514.874f, 60.0f, 9323.166f), groupNumber: 1, teamSideOfTheMap: TeamId.TEAM_BLUE, campTypeIcon: "LesserCamp", 100.0f * 1000);
            MonsterCamps.Add(blue_Wraiths, new List<Monster>
            {
                CreateJungleMonster("TT_NWraith1.1.1", "TT_NWraith", new Vector2(5514.874f, 9323.166f), new Vector3(5514.874f, -109.177f, 9323.166f), blue_Wraiths, aiScript: "BasicJungleMonsterAi"),
                CreateJungleMonster("TT_NWraith21.1.2", "TT_NWraith2", new Vector2(5514.874f, 9423.166f), new Vector3(5514.874f, -109.177f, 9423.166f), blue_Wraiths, aiScript: "BasicJungleMonsterAi"),
                CreateJungleMonster("TT_NWraith21.1.3", "TT_NWraith2", new Vector2(5414.874f, 9423.166f), new Vector3(5414.874f, -109.177f, 9423.166f), blue_Wraiths, aiScript: "BasicJungleMonsterAi")
            });

            //Blue Side Golems
            var blue_Golems = CreateJungleCamp(new Vector3(4650.874f, 60.0f, 4800.0f), groupNumber: 2, teamSideOfTheMap: TeamId.TEAM_BLUE, campTypeIcon: "LesserCamp", 100.0f * 1000);
            MonsterCamps.Add(blue_Golems, new List<Monster>
            {
                CreateJungleMonster("TT_NGolem2.1.1", "TT_NGolem", new Vector2(4650.874f, 4800.0f), new Vector3(4650.874f, -109.332f, 4800.0f), blue_Golems, aiScript: "BasicJungleMonsterAi"),
                CreateJungleMonster("TT_NGolem22.1.2", "TT_NGolem2", new Vector2(4750.874f, 4800.0f), new Vector3(4750.874f, -109.332f, 4800.0f), blue_Golems, aiScript: "BasicJungleMonsterAi")
            });

            //Blue Side Wolves
            var blue_Wolves = CreateJungleCamp(new Vector3(4350.874f, 60.0f, 9423.166f), groupNumber: 3, teamSideOfTheMap: TeamId.TEAM_BLUE, campTypeIcon: "LesserCamp", 100.0f * 1000);
            MonsterCamps.Add(blue_Wolves, new List<Monster>
            {
                CreateJungleMonster("TT_NWolf3.1.1", "TT_NWolf", new Vector2(4350.874f, 9423.166f), new Vector3(4350.874f, -109.744f, 9423.166f), blue_Wolves, aiScript: "BasicJungleMonsterAi"),
                CreateJungleMonster("TT_NWolf23.1.2", "TT_NWolf2", new Vector2(4250.874f, 9423.166f), new Vector3(4250.874f, -109.744f, 9423.166f), blue_Wolves, aiScript: "BasicJungleMonsterAi"),
            });

            //Red Side Wraiths
            var red_Wraiths = CreateJungleCamp(new Vector3(7940.874f, 60.0f, 9323.166f), groupNumber: 4, teamSideOfTheMap: TeamId.TEAM_PURPLE, campTypeIcon: "LesserCamp", 100.0f * 1000);
            MonsterCamps.Add(red_Wraiths, new List<Monster>
            {
                CreateJungleMonster("TT_NWraith4.1.1", "TT_NWraith", new Vector2(7940.874f, 9323.166f), new Vector3(7940.874f, -109.202f, 9323.166f), red_Wraiths, aiScript: "BasicJungleMonsterAi"),
                CreateJungleMonster("TT_NWraith24.1.2f", "TT_NWraith2", new Vector2(7940.874f, 9423.166f), new Vector3(7940.874f, -109.202f,  9423.166f), red_Wraiths, aiScript: "BasicJungleMonsterAi"),
                CreateJungleMonster("TT_NWraith24.1.3", "TT_NWraith2", new Vector2(7840.874f, 9423.166f), new Vector3(7840.874f, -109.202f,  9423.166f), red_Wraiths, aiScript: "BasicJungleMonsterAi")
            });

            //Red Side Golems
            var red_Golems = CreateJungleCamp(new Vector3(8650.874f, 60.0f, 4800.0f), groupNumber: 5, teamSideOfTheMap: TeamId.TEAM_PURPLE, campTypeIcon: "LesserCamp", 100.0f * 1000);
            MonsterCamps.Add(red_Golems, new List<Monster>
            {
                CreateJungleMonster("TT_NGolem5.1.1f", "TT_NGolem", new Vector2(8650.874f, 4800.0f), new Vector3(8650.874f, -109.466f, 4800.0f), red_Golems, aiScript: "BasicJungleMonsterAi"),
                CreateJungleMonster("TT_NGolem25.1.2", "TT_NGolem2", new Vector2(8750.874f, 4800.0f), new Vector3(8750.874f, -109.466f, 4800.0f), red_Golems, aiScript: "BasicJungleMonsterAi")
            });

            //Red Side Wolves
            var red_Wolves = CreateJungleCamp(new Vector3(9200.874f, 60.0f, 9423.166f), groupNumber: 6, teamSideOfTheMap: TeamId.TEAM_PURPLE, campTypeIcon: "LesserCamp", 100.0f * 1000);
            MonsterCamps.Add(red_Wolves, new List<Monster>
            {
                CreateJungleMonster("TT_NWolf6.1.1", "TT_NWolf", new Vector2(9200.874f, 9423.166f), new Vector3(9200.874f, -109.837f, 9423.166f), red_Wolves, aiScript: "BasicJungleMonsterAi"),
                CreateJungleMonster("TT_NWolf26.1.2", "TT_NWolf2", new Vector2(9250.874f, 9423.166f), new Vector3(9250.874f, -109.837f, 9423.166f), red_Wolves, aiScript: "BasicJungleMonsterAi"),
            });

            //Center of the Map Health Pack
            var healthPack = CreateJungleCamp(new Vector3(6748.594f, 60.0f, 5000.0f), groupNumber: 7, teamSideOfTheMap: TeamId.TEAM_UNKNOWN, campTypeIcon: "Camp", 115.0f * 1000);
            MonsterCamps.Add(healthPack, new List<Monster>
            {
                CreateJungleMonster("LizardElder1.1.1", "LizardElder", new Vector2(6748.594f, 5000.0f), new Vector3(6748.594f, -112.716f, 5000.0f), healthPack, aiScript: "BasicJungleMonsterAi")
            });

            //Vilemaw
            //TODO: VIle maw needs it's own Special A.I Script, for now it'll be just a dummy.
            var spiderBoss = CreateJungleCamp(new Vector3(6748.594f, 60.0f, 7878f), groupNumber: 8, teamSideOfTheMap: TeamId.TEAM_UNKNOWN, campTypeIcon: "Epic", 600.0f * 1000);
            MonsterCamps.Add(spiderBoss, new List<Monster>
            {
                CreateJungleMonster("Dragon8.1.1", "Dragon", new Vector2(6748.594f, 7878f), new Vector3(6748.594f, -108.603f, 7878f), spiderBoss, aiScript: "BasicJungleMonsterAi")
            });
        }

        public static void OnUpdate(float diff)
        {
            foreach (var camp in MonsterCamps.Keys)
            {
                if (!camp.IsAlive)
                {
                    camp.RespawnTimer -= diff;
                    if (camp.RespawnTimer <= 0 || forceSpawn)
                    {
                        SpawnCamp(camp);
                        camp.RespawnTimer = GetRespawnTimer(camp);
                    }
                }
            }

            if (forceSpawn)
            {
                forceSpawn = false;
            }
        }

        public static void SpawnCamp(MonsterCamp monsterCamp)
        {
            var averageLevel = GetPlayerAverageLevel();

            foreach (var monster in MonsterCamps[monsterCamp])
            {
                monster.UpdateInitialLevel(averageLevel);
                monster.Stats.Level = (byte)averageLevel;
                Monster campMonster = monsterCamp.AddMonster(monster);
                MonsterDataTable.UpdateStats(campMonster);
            }
        }

        public static void ForceCampSpawn()
        {
            forceSpawn = true;
        }

        public static float GetRespawnTimer(MonsterCamp monsterCamp)
        {
            switch (monsterCamp.CampIndex)
            {
                case 7:
                    return 90.0f * 1000;
                case 8:
                    return 300.0f * 1000;
                default:
                    return 50.0f * 1000;
            }
        }
    }
}
