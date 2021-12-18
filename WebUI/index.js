let animationInterval = setInterval(AnimateLoading, 75);
LoadList();

function LoadList() {
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

            let scoreboard = '<table><tr><td>Username</td>';
            rounds.forEach(r => {
                scoreboard += (data.ServerConfig.ActiveRound === r) ? ('<td class="active-round">' + roundFullNames[r] + '</td>') : ('<td>' + roundFullNames[r] + '</td>');
            });
            scoreboard += '</tr>';

            data.Scores.forEach(player => {
                scoreboard += '<tr><td>' + player.Username + '</td>';

                rounds.forEach(r => {
                    if (r in player.Score)
                        scoreboard += '<td>' + player.Score[r] + '</td>';
                    else scoreboard += '<td>-</td>';
                });

                scoreboard += '</tr>';
            });

            scoreboard += '</table>';

            scoreboardContainer.html(scoreboard);

            $("#version").text(data.ServerVersion);
            $("#timestamp").text(data.Timestamp);
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
