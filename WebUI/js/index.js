let animationInterval = setInterval(AnimateLoading, 75);
LoadData();

function LoadData() {
    $.ajax({url: "scoreboard.json", dataType: "json", success: function (data) {
            let scoreboardContainer = $("#scoreboard");
            scoreboardContainer.html();

            let rounds = [];
            let roundPriorities = {};
            let roundFullNames = {};

            data.Rounds.forEach(r => {
                if (r.DisplayOrder < 0)
                    return;

                roundFullNames[r.ShortName] = r.Name;
                if (r.DisplayOrder in roundPriorities)
                    roundPriorities[r.DisplayOrder].push(r.ShortName);
                else roundPriorities[r.DisplayOrder] = [r.ShortName];
            });

            Object.keys(roundPriorities).sort().forEach(k => {
                roundPriorities[k].forEach(p => {
                    rounds.push(p);
                });
            });

            let scoreboard = '<table><tr><td><b>Username</b></td>';
            rounds.forEach(r => {
                scoreboard += (data.ServerConfig.ActiveRound === r) ? ('<td class="active-round"><b>' + roundFullNames[r] + '</b></td>') : ('<td><b>' + roundFullNames[r] + '</b></td>');
            });
            scoreboard += '</tr>';

            data.Scores.forEach(player => {
                scoreboard += '<tr><td>' + player.Username + '</td>';

                rounds.forEach(r => {
                    if (r in player.Score)
                        scoreboard += '<td>' + player.Score[r].Score + ' in ' + player.Score[r].Games + (player.Score[r].Games === 1 ? ' game' : ' games') + ', avg. ' + (player.Score[r].Games == 0 ? 'N/A' : (player.Score[r].Score / player.Score[r].Games).toFixed(4)) + '</td>';
                    else scoreboard += '<td>-</td>';
                });

                scoreboard += '</tr>';
            });

            scoreboard += '</table>';

            scoreboardContainer.html(scoreboard);
            scoreboard = '';

            let pendingContainer = $("#pending-games");
            pendingContainer.html();

            let pending = '';

            data.GamesInProgress.forEach(r => {
                pending += '<b>&bull; Game #' + r.InternalId + '</b><br>';
                pending += '<b>Status: </b>';

                if (r.TimeElapsed === -2)
                    pending += "Game in progress";
                else if (r.TimeElapsed === -1)
                    pending += "Waiting for players";
                else if (r.TimeElapsed === -3)
                    pending += "Administrative hold";
                else pending += "Waiting for players, starting in " + (data.ServerConfig.GameDelay - r.TimeElapsed);
                pending += '<br>';

                if (r.Players.length > 0) {
                    pending += "<b>Players (" + r.Players.length + "):</b><br>";
                    r.Players.forEach(p => {
                        pending += "- " + p + "<br>";
                    });
                }
            });

            if (pending === "")
                pending = "<i>No pending games.</i>"

            pendingContainer.html(pending);

            $("#version").text(data.ServerVersion);
            $("#timestamp").text(data.Timestamp);
            $("#cfg-minimum").text(data.ServerConfig.MinimumPlayersAmount);
            $("#cfg-limit").text(data.ServerConfig.PlayersLimit);
            $("#cfg-delay").text(data.ServerConfig.GameDelay);
            $("#cfg-active").text(roundFullNames[data.ServerConfig.ActiveRound]);

            $("#loading-container").css("visibility", "hidden");
            $("#container").css("visibility", "visible");

            clearInterval(animationInterval);
        }});
}

const LoadingText = "Loading...";
const AnimationCharacters = "!#$%()-_";

let ltLen = LoadingText.length;
let acLen = AnimationCharacters.length;

let i = 0;

function AnimateLoading() {
    $("#loading-text").text(LoadingText.replaceAt(i, AnimationCharacters.charAt(Math.floor(Math.random() * acLen))));
    i++;
    if (i >= ltLen) i = 0;
}

String.prototype.replaceAt=function(index, replacement) {
    return this.substr(0, index) + replacement+ this.substr(index + replacement.length);
};

String.prototype.replaceAll = function(search, replacement) {
    return this.replace(new RegExp(search, 'g'), replacement);
};
