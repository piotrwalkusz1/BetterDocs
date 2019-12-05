"use strict";

// TODO: Usunąć
// OLD VERSION

// var documentContentElement = document.getElementById("documentContent");
// var oldText = "";
// var dmp = new diff_match_patch();
//
// documentContentElement.value = oldText;
//
// connection.on("ChangeText", function (patch) {
//     var result = dmp.patch_apply(dmp.patch_fromText(patch), documentContentElement.value);
//     documentContentElement.value = result[0];
//     oldText = result[0];
//     if (result.includes(false)) {
//         console.error("Error while patch applying");
//     }
// });
//
// documentContentElement.oninput = function () {
//     var text = documentContentElement.value;
//     var patch = dmp.patch_toText(dmp.patch_make(oldText, text));
//     oldText = text;
//     connection.invoke("ChangeText", patch, "1").catch(function (err) {
//         return console.error(err.toString());
//     });
// };


var documentContentElement = document.getElementById("documentContent");
documentContentElement.hidden = true;
var documentId = new URLSearchParams(window.location.search).get("id");

document.getElementById("saveDocument").onclick = function () {
    connection.invoke("SaveText", documentId).catch(function (err) {
        return console.error(err.toString());
    });
};

document.getElementById("addContributor").onclick = () => $("#addContributorModal").modal('show');
document.getElementById("manageContributors").onclick = () => $("#showContributorsModal").modal('show');

var connection = new signalR.HubConnectionBuilder().withUrl("/editDocumentHub").build();

connection.start().then(function () {
    connection.invoke("StartEditingDocument", documentId).then(function (text) {
        documentContentElement.value = text;
        documentContentElement.hidden = false;
    }).catch(function (err) {
        return console.error(err.toString());
    });
}).catch(function (err) {
    return console.error(err.toString());
});

connection.on("ChangeText", function (text) {
    documentContentElement.value = text;
});

documentContentElement.oninput = function () {
    var text = documentContentElement.value;
    connection.invoke("ChangeText", text, documentId).catch(function (err) {
        return console.error(err.toString());
    });
};