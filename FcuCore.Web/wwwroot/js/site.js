// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

var socket = new WebSocket("ws://" + window.location.host + "/ws");
socket.onmessage = (d) => {
    var msg = JSON.parse(d.data);
    console.log(msg);
};

$('#open-comms').click(function() {
    $.ajax({
        url: '/api/Manager/Communications',
        data: '"open"',
        contentType: 'application/json',
        method: 'POST'
    });
});

$('#close-comms').click(function() {
    $.ajax({
        url: '/api/Manager/Communications',
        data: '"close"',
        contentType: 'application/json',
        method: 'POST'
    });
});

$('#enumerate').click(function() {
    $.ajax({
        url: '/api/Manager/Communications',
        data: '"enumerate"',
        contentType: 'application/json',
        method: 'POST'
    });
});