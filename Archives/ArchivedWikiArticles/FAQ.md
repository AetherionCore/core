# Intro

### Q: What is the goal of this project?
 A: The goal of the GameServer project is to act as a server emulator for League of Legends version 4.20 (sln 0.0.1.67, project 0.0.1.7), or Preseason 5.

### Q: Why choose v4.20?
 A: v4.20 was the last version of League before Riot began rigorous work to encrypt their packet structure, specifically in game replays, which hindered this project's predecessor, [IntWars](https://github.com/Elyotna/IntWars) in its efforts to provide a sandbox for the most current version of League. Thus, the version before major packet encryption was chosen as a base for LeagueSandbox, the (unofficial) successor to IntWars.

### Q: What makes packet encryption so important?
 A: When developing the GameServer, it was decided that the official League v4.20 GameClient (what users run to play the game) would be used to connect to the server. Because of this, it was required that the packets the GameClient was built for be emulated as well. To do so, extensive research was done on League Replay Files (LRF), which housed all the information sent from server to client during a match. When encryption was introduced, no only did it put a strain on the replay tools, as they were unable to handle the encryption, but it also meant all replays after v4.20 were severely corrupted, and research was halted as a result.

# Setup

### Q: I get the error “A project with an Output type of Class Library cannot be started directly” after trying to run the project using visual studio.
 A: Choose GameServerConsole as the startup project in Visual Studio.
***

### Q: Minions don't spawn.
 A: Type '!spawnstate 1' in the game. For permanent solution the settings files need to be edited.
***

### Q: I changed the settings but nothing changed in the game.
 A: Be sure that you edited the settings file in the 'bin' folder (where the exe file is).
***