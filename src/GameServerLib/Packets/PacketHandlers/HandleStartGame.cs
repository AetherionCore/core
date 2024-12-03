using GameServerCore.Packets.PacketDefinitions.Requests;
using GameServerCore.Packets.Handlers;
using LeaguePackets.Game.Events;
using GameServerCore.NetInfo;
using System.Linq;
using LeagueSandbox.GameServer.Players;
using System.Text;
using System.Numerics;

namespace LeagueSandbox.GameServer.Packets.PacketHandlers
{
    public class HandleStartGame : PacketHandlerBase<StartGameRequest>
    {
        private readonly Game _game;
        private readonly PlayerManager _playerManager;
        private bool _shouldStartAsSoonAsPossible = false;

        public HandleStartGame(Game game)
        {
            _game = game;
            _playerManager = game.PlayerManager;
        }

        public override bool HandlePacket(int userId, StartGameRequest req)
        {
            var peerInfo = _playerManager.GetPeerInfo(userId);
            peerInfo.IsDisconnected = false;

            if (_game.IsRunning)
            {
                StartFor(peerInfo);
                return true;
            }
            else
            {
                TryStart();
            }
            return true;
        }

        public void ForceStart()
        {
            _shouldStartAsSoonAsPossible = true;
            TryStart();
        }

        private void TryStart()
        {
            var players = _playerManager.GetPlayers(false);

            bool isPossibleToStart;
            if (_shouldStartAsSoonAsPossible)
            {
                isPossibleToStart = players.Any(p => !p.IsDisconnected);
            }
            else
            {
                isPossibleToStart = players.All(p => !p.IsDisconnected);
            }

            if (!isPossibleToStart)
            {
                return;
            }

            foreach (var player in players)
            {
                if (!player.IsDisconnected)
                {
                    StartFor(player);
                }
            }
            _game.Start();
        }

        private void StartFor(ClientInfo player)
        {
            if (_game.IsPaused)
            {
                _game.PacketNotifier.NotifyPausePacket(player, (int)_game.PauseTimeLeft, true);
            }

            _game.PacketNotifier.NotifyGameStart(player.ClientId);

            if (_game.IsRunning)
            {
                var announcement = new OnReconnect { OtherNetID = player.Champion.NetId };
                _game.PacketNotifier.NotifyS2C_OnEventWorld(announcement, player.Champion);
            }

            if (!player.IsMatchingVersion)
            {
                _game.PacketNotifier.NotifyS2C_SystemMessage(
                    player.ClientId,
                    "Your client version does not match the server. " +
                    "Check the server log for more information."
                );
            }


            NotifySystemMessagesServerMOTD();

            _game.PacketNotifier.NotifyS2C_HandleTipUpdate(player.ClientId,
                "[SERVER INFO] Welcome to LeagueServer", "https://github.com/brian8544/LeagueServer",
                "", 0, player.Champion.NetId, _game.NetworkIdManager.GetNewNetId());
            /*
            _game.PacketNotifier.NotifyS2C_HandleTipUpdate(player.ClientId,
                "[DEBUG INFO] Server Build Date", ServerContext.BuildDateString,
                "", 0, player.Champion.NetId, _game.NetworkIdManager.GetNewNetId());
            
            _game.PacketNotifier.NotifyS2C_HandleTipUpdate(player.ClientId,
                "[DEBUG INFO] Your Champion:", player.Champion.Model,
                "", 0, player.Champion.NetId, _game.NetworkIdManager.GetNewNetId());
            */

            SyncTime(player.ClientId);
        }

        void SyncTime(int userId)
        {
            // Send the initial game time sync packets, then let the map send another
            var gameTime = _game.GameTime;
            _game.PacketNotifier.NotifySynchSimTimeS2C(userId, gameTime);
            _game.PacketNotifier.NotifySyncMissionStartTimeS2C(userId, gameTime);
        }

        void NotifySystemMessagesServerMOTD()
        {
            var formattedText = new StringBuilder();
            var fontSize = 20;

            formattedText.Append("<font size=\"" + fontSize + "\" color =\"#2E2E2E\"><b><font color=\"#FFD700\">[SERVER INFO]</font></b><font color =\"#00FF7F\">: Welcome to LeagueServer</font>\n");
            formattedText.Append("<font size=\"" + fontSize + "\" color =\"#2E2E2E\"><b><font color=\"#FFD700\">[SERVER INFO]</font></b><font color =\"#00FF7F\">: Found a bug? Visit: <font color =\"#1E90FF\">https://github.com/brian8544/LeagueServer</font></font>\n");
            formattedText.Append("<font size=\"" + fontSize + "\" color =\"#2E2E2E\"><b><font color=\"#FFD700\">[SERVER INFO]</font></b><font color =\"#00FF7F\">: Server Build Date: " + ServerContext.BuildDateString + "</font>");

            _game.PacketNotifier.NotifyS2C_SystemMessage(formattedText.ToString());
        }
    }
}