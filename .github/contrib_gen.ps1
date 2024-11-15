function Get-Contributors {
    param (
        [string]$RepoName,
        [int]$Page = 1
    )
    $url = "https://api.github.com/repos/$RepoName/contributors?per_page=100&page=$Page"
    $response = Invoke-RestMethod -Uri $url -Method Get -Headers @{
        'Content-Type' = 'application/json'
    }
    return $response
}

$repos = @(
    "LeagueSandbox/GameServer",
    "LeagueSandbox/LeagueSandbox-Default",
    "LeagueSandbox/LeaguePackets",
    "LeagueSandbox/ContentSerializer",
    "LeagueSandboxAutoSetup",
    "LeagueSandbox/Nexus",
    "LeagueSandbox/LENet",
    "LeagueSandbox/ENetSharpLeague",
    "LeagueSandbox/LobbyServer",
    "LeagueSandbox/LoLReplayUnpacker",
    "LeagueSandbox/Testbox",
    "LeagueSandbox/ReplayInspector",
    "LeagueSandbox/lspm",
    "LeagueSandbox/leaguesandbox.github.io",
    "LeagueSandbox/LobbyClient"#,
    #"brian8544/LeagueServer
)

$uniqueUsernames = @{}

foreach ($repo in $repos) {
    $page = 1
    $moreData = $true
    while ($moreData) {
        $contributors = Get-Contributors -RepoName $repo -Page $page
        if ($contributors.Count -eq 0) {
            $moreData = $false
        }
        else {
            foreach ($contributor in $contributors) {
                $uniqueUsernames[$contributor.login] = $true
            }
        }
        $page++
    }
}

$contributorsFile = "CONTRIBUTORS.MD"

@"
# List of people who contributed over time to the LeagueServer project

## History of development
The development of LeagueSandbox project dates back to its roots in IntWars, which has undergone various stages of development under different contributors and organizations over time:

IntWars/HeroWars, 2012-2012
LeagueSandbox, 2016-2022
LeagueServer, 2024

## Exceptions with third party libraries
The third party libraries have their own way of addressing authorship, and the authorship of commits importing/ updating
a third party library reflects who did the importing instead of who wrote the code within the commit.

The Authors of third party libraries are not explicitly mentioned, and usually are possible to obtain from the files belonging to the third party libraries.

## Contributors

*Please inform us if you find somebody who is missing!*

"@ | Out-File -FilePath $contributorsFile

$uniqueUsernames.Keys | Sort-Object | ForEach-Object {
    "- $_" | Out-File -FilePath $contributorsFile -Append
}