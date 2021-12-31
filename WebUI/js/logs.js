let animationInterval = setInterval(AnimateLoading, 75);
LoadData();

function LoadData() {
    $.ajax({url: "GameLogs/", success: function (data) {
            let dta = $("#data");
            dta.html(data);

            let games = {};
            let max = 0;
            let output =  '&bull; <a href="../">Go back</a><br>';

            dta.find('a').toArray().forEach(i => {
                let sp = i.href.split('/').slice(-1)[0].split('-');
                if (sp.length !== 7)
                    return;

                let iid = parseInt(sp[0]);
                games[iid] = sp;
                if (iid > max)
                    max = iid;
            });

            max = max.toString().length;

            Object.keys(games).sort(function(a, b) {
                return a - b;
            }).forEach(k => output += "&bull; " + parseEntry(games[k], max) + "<br>");

            dta.remove();
            $("#logs").html(output);

            $("#loading-container").css("visibility", "hidden");
            $("#container").css("visibility", "visible");

            clearInterval(animationInterval);
    }});
}

function parseEntry(sp, len)
{
    let id = sp[0];
    while (id.length < len)
        id = "0" + id;

    return '<a href="log.html?id=' + sp.join('-') + '" target="_blank">Game ' + id + ' played at ' + sp[1] + '-' + sp[2] + '-' + sp[3] + ' ' + sp[4] + ':' + sp[5] + ':' + sp[6].substr(0, 2) + 'Z</a>';
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
