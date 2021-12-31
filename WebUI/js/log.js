let animationInterval = setInterval(AnimateLoading, 75);
LoadData();

function LoadData() {
    let id = getUrlParam("id", null);
    if (id == null) {
        DisplayError("Missing id.")
        return;
    }

    $.ajax({
        url: window.location.toString().replace("log.html", "GameLogs/" + id), success: function (data) {
            data = data.split("\n");
            let output = "";

            data.forEach((l, i) => {
                if (i < 4) {
                    if (l !== "")
                        output += '<b>' + l.replace(": ", "</b>: ") + '<br>';
                    else output += '<br>';
                    return;
                }

                output += l.replace("[", "<b>[").replace("]", "]</b>") + '<br>';
            });

            $("#logs").html(output);

            $("#loading-container").css("visibility", "hidden");
            $("#container").css("visibility", "visible");

            clearInterval(animationInterval);
        }, error: function (xhr, status, error) {
            DisplayError(xhr.status + ': ' + xhr.statusText);
    }});
}

const LoadingText = "Loading...";
const AnimationCharacters = "!#$%()-_";

let ltLen = LoadingText.length;
let acLen = AnimationCharacters.length;

let i = 0;

function DisplayError(error) {
    $("#logs").html(error);
    $("#loading-container").css("visibility", "hidden");
    $("#container").css("visibility", "visible");

    clearInterval(animationInterval);
}

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

function getUrlVars() {
    var vars = {};
    var parts = window.location.href.replace(/[?&]+([^=&]+)=([^&]*)/gi, function(m,key,value) {
        vars[key] = value;
    });
    return vars;
}

function getUrlParam(parameter, defaultvalue){
    var urlparameter = defaultvalue;
    if(window.location.href.indexOf(parameter) > -1){
        urlparameter = getUrlVars()[parameter];
    }
    return urlparameter;
}
