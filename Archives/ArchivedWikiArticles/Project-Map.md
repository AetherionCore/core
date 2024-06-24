## The different projects in this repo
* PacketsDefinition420

Used or all networking including the definition of some packets. The packets should be used from the most accurate repo which is LeaguePackets repo.
* GameServerCore

All enums/interfaces that used to describe what GameServer should implement with some documentation. Introduced to decouple the scripts & packet system to be completly independent on the exact implementation and to help testing (which currently we don't exactly have).
* GameServerLib

The implementation of all the interfaces and systems. This contains all the logic of the server, and properly implement the interfaces.
* GameServerConsole

Used to run the project, including all the run settings. Has some GUI to show the log of the server.
