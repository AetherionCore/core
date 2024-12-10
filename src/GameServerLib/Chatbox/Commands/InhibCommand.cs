using LeagueSandbox.GameServer.Players;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using LeagueSandbox.GameServer.GameObjects;
using LeagueSandbox.GameServer.API;
using GameServerLib.GameObjects;
using GameServerCore.Enums;
using System.Numerics;

namespace LeagueSandbox.GameServer.Chatbox.Commands
{
    public class InhibCommand : ChatCommandBase
    {
        private readonly PlayerManager _playerManager;

        public override string Command => "inhib";
        public override string Syntax => $"{Command}";

        public InhibCommand(ChatCommandManager chatCommandManager, Game game)
            : base(chatCommandManager, game)
        {
            _playerManager = game.PlayerManager;
        }

        public override void Execute(int userId, bool hasReceivedArguments, string arguments = "")
        {
            var sender = _playerManager.GetPeerInfo(userId);
            var baron = ApiMapFunctionManager.CreateJungleCamp(sender.Champion.GetPosition3D(), 12, TeamId.TEAM_UNKNOWN, "Baron", 900.0f * 1000);
            var min = ApiMapFunctionManager.CreateJungleMonster("Worm", "Worm", sender.Champion.Position,
                sender.Champion.Direction, baron, aiScript: "BasicJungleMonsterAI");
            Game.ObjectManager.AddObject(min);
        }
    }
}
