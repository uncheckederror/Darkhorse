// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

function showContact(contactObjectHash) {
    var contacts = document.getElementsByClassName("contactDetail");
    var i;
    for (i = 0; i < contacts.length; i++) {
        contacts[i].classList.toggle("d-none");
    }
    document.getElementById(contactObjectHash).classList.toggle("d-none");
}