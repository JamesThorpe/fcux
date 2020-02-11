(function (window, ko, $) {
    const cbus = {};
    window.cbus = cbus;
    cbus.status = {
        socketConnected: ko.observable(false),
        cbusConnected: ko.observable(false),
        receivedMessagesBuffer: ko.observableArray(),
        showMessages: ko.observable(false),
        toggleDebug: () => cbus.status.showMessages(!cbus.status.showMessages())
    };

    cbus.api = {
        sendApiRequest: (controller, action, data) => {
            $.ajax({
                url: `/api/${controller}/${action}`,
                data: data,
                contentType: 'application/json',
                method: 'POST'
            });
        },
        sendQNN: () => {
            cbus.api.sendApiRequest("Manager", "Communications", '"enumerate"');
        }
    };

    const messageHandlers = [];
    cbus.comms = {
        connect: () => {
            cbus.api.sendApiRequest("Manager", "Communications",'"open"');
        },
        disconnect: () => {
            cbus.api.sendApiRequest("Manager", "Communications",'"close"');
        },
        addHandler: (opCode, handler) => {
            messageHandlers.push({ opCode: opCode, handler: handler });
        }
    };


    const socket = new WebSocket("ws://" + window.location.host + "/ws");
    socket.onmessage = (d) => {
        var msg = JSON.parse(d.data);
        switch (msg.Type) {
            case "cbus":
                cbus.status.receivedMessagesBuffer.push(msg);
                while (cbus.status.receivedMessagesBuffer().length > 20) {
                    cbus.status.receivedMessagesBuffer.shift();
                }
                if (msg.Direction === "received") {
                    var mh = messageHandlers.filter(handler => handler.opCode === msg.Message.OpCode);
                    for (var x = 0; x < mh.length; x++) {
                        mh[x].handler(msg.Message);
                    }
                }

                break;
            case "connection-status":
                cbus.status.cbusConnected(msg.IsConnected);
                break;
            default:
                console.warn("Unknown websocket message received: ", d.data);
        }
    };
    socket.onopen = () => {
        cbus.status.socketConnected(true);
    };
    socket.onclose = () => {
        cbus.status.socketConnected(false);
    };

    //TODO: reconnect socket, handle errors


})(window, ko, jQuery);