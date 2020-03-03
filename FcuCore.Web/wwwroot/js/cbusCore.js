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
        readApi: (controller, action) => {
            return $.ajax({
                url: `/api/${controller}/${action}`,
                method: "GET",
                dataType: "JSON"
            });
        },
        sendApiRequest: (controller, action, data) => {
            return $.ajax({
                url: `/api/${controller}/${action}`,
                data: JSON.stringify(data),
                contentType: 'application/json',
                method: "POST"
            });
        },
        sendQNN: () => {
            cbus.api.sendApiRequest("Manager", "Communications", "enumerate");
        }
    };

    const messageHandlers = [];
    cbus.comms = {
        connect: () => {
            cbus.api.sendApiRequest("Manager", "Communications","open");
        },
        disconnect: () => {
            cbus.api.sendApiRequest("Manager", "Communications","close");
        },
        addHandler: (opCode, handler) => {
            messageHandlers.push({ opCode: opCode, handler: handler });
        },
        configure: () => {
            cbus.api.readApi("Manager", "ConfigureComms").done((d) => {
                cbus.comms.transport(d.transport);
                cbus.comms.serialPort(d.serialPort);
                cbus.comms.availableSerialPorts(d.availableSerialPorts);
                $("#dialog-configure-comms").modal("show");
            });
        },
        transport: ko.observable('Serial'),
        serialPort: ko.observable(''),
        availableSerialPorts: ko.observableArray([]),
        closeConfigure: () => {
            $("#dialog-configure-comms").modal("hide");
        },
        saveConfigure: () => {
            cbus.api.sendApiRequest("Manager", "ConfigureComms", {
                transport: cbus.comms.transport(),
                serialPort: cbus.comms.serialPort()
                });
            $("#dialog-configure-comms").modal("hide");
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
                    var mh = messageHandlers.filter(handler => handler.opCode === msg.Message.OpCode || handler.opCode === msg.Message.OpCodeString);
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


    cbus.loadData = function () {
        /*
        cbus.api.readApi("Manager", "LoadData").done((d) => {
            const config = JSON.parse(d);
            cbus.modules.loadData(config.modules);
        });
        */
        $("#dialog-load-config").modal("show");
    };
    var lastFile;
    cbus.loadData.file = function(a, b, c) {
        b.target.files[0].text().then((f) => {
            lastFile = f;
        });
    }

    cbus.loadData.load = function() {
        const config = JSON.parse(lastFile);
        cbus.modules.loadData(config.modules);
    };

    cbus.saveData = function () {
        const d = {};
        d.modules = cbus.modules.getData();

        const e = document.createElement("a");
        e.setAttribute("href", "data:text/plain;charset=utf-8," + encodeURIComponent(JSON.stringify(d, null, 4)));
        e.setAttribute("download", "fcu.json");
        e.style.display = "none";
        document.body.appendChild(e);
        e.click();
        document.body.removeChild(e);
    };

})(window, ko, jQuery);