(function (cbus, ko, $) {

    function node(config, type) {
        this.nodeType = type;
        this.nodeNumber = config.NodeNumber;
        this.name = ko.observable('Unnamed Node');
        this.canId = config.CanId;
        this.isConsumerNode = config.IsConsumerNode;
        this.isProducerNode = config.IsProducerNode;
        this.inFlimMode = config.InFlimMode;
        this.supportsBootloader = config.SupportsBootloader;

        this.params = ko.observableArray([]);

        this.manufacturerId = ko.computed(() => {
            if (this.params()[1] !== undefined) {
                return this.params()[1];
            }
            return -1;
        });

        this.moduleId = ko.computed(() => {
            if (this.params()[3] !== undefined) {
                return this.params()[3];
            }
            return -1;
        });

        this.minorVersion = ko.computed(() => {
            if (this.params()[2] !== undefined) {
                return String.fromCharCode(this.params()[2]);
            } else {
                return "?";
            }
        });

        this.majorVersion = ko.computed(() => {
            if (this.params()[7] !== undefined) {
                return this.params()[7];
            } else {
                return "";
            }
        });

        this.version = ko.computed(() => {
            return `${this.majorVersion()}${this.minorVersion()}`;
        });

        this.supportedNodeVariables = ko.computed(() => {
            if (this.params()[6] !== undefined) {
                return this.params()[6];
            }
            return -1;
        });


        this.nodeVariables = ko.observableArray([]);
        this.params.subscribe(() => {
            if (this.supportedNodeVariables() !== -1) {
                if (this.nodeVariables().length < this.supportedNodeVariables()) {
                    for (let x = 1; x <= this.supportedNodeVariables(); x++) {
                        if (!this.nodeVariables().find((nv) => nv.index === x)) {
                            this.nodeVariables.push({
                                index: x,
                                value: ko.observable(0)
                            });
                        }
                    }
                }
            }
        });

        this._rawNVs = {};

        this.producedEvents = ko.observableArray([]);
    }

    node.prototype.editName = function() {
        cbus.modules.currentNode(this);
        $("#dialog-edit-node-name").modal("show");
        $("#dialog-edit-node-name-input").select();
    };
    node.prototype.editNodeVariables = function () {
        if (this.supportedNodeVariables() !== -1) {
            cbus.modules.currentNode(this);
            $("#dialog-edit-node-variables").modal("show");
        } else {
            console.warn('Unknown node variables count - unable to display NV editor');
        }
    };
    node.prototype.closeEditNodeVariables = function() {
        $("#dialog-edit-node-variables").modal("hide");
        cbus.modules.currentNode(null);
    };
    node.prototype.readNodeVariables = function() {
        cbus.api.sendApiRequest("Manager", "ReadNodeVariables", {
            NodeNumber: this.nodeNumber,
            VariableCount: this.supportedNodeVariables()
        });
    };
    node.prototype.getNodeVariable = function (nvIndex) {
        const nv = this.getNV(nvIndex);
        if (nv != null) {
            return nv.value();
        }
        return -1;
    };

    node.prototype.getNV = function(nvIndex) {
        if (this._rawNVs[nvIndex]) {
            return this._rawNVs[nvIndex];
        }
        this._rawNVs[nvIndex] = this.nodeVariables().find((n) => n.index === nvIndex);
        return this._rawNVs[nvIndex];
    };
    node.prototype.addEvent = function(eventNumber) {
        if (!this.producedEvents().find((pe) => pe.eventNumber === eventNumber)) {
            this.producedEvents.push({
                eventNumber: eventNumber,
                name: `Event ${this.nodeNumber}:${eventNumber}`
            });
        }
    }


    var id = 0;

    cbus.modules = {
        definitions: {},
        list: ko.observableArray(),
        listFilter: ko.observable(""),
        nodeSelectorFilter: ko.observable(""),
        shortEvents: ko.observableArray([]),
        currentNode: ko.observable(null),
        selectedNode: ko.observable(null),
        eventsNode: ko.observable(null),
        assignedEventsNode: ko.observable(null),
        getData: function() {
            return JSON.parse(ko.toJSON(cbus.modules.list,
                function(k, v) {
                    if (k === "nodeType")
                        return;

                    return v;
                }));
        },
        loadData: function(d) {
            cbus.modules.list([]);
            for (let i in d) {
                const n = d[i];
                let md = null;
                for (let m in cbus.modules.definitions) {
                    let check = cbus.modules.definitions[m];
                    if (check.manufacturerId === n.manufacturerId && check.moduleId === n.moduleId) {
                        md = check;
                        break;
                    }
                }
                if (md == null) {
                    md = cbus.modules.definitions["UNKNOWN"];
                }
                const ni = new node({
                    NodeNumber: n.nodeNumber,
                    CanId: n.canId,
                    IsConsumerNode: n.isConsumerNode,
                    IsProducerNode: n.isProducerNode,
                    InFlimMode: n.inFlimMode,
                    SupportsBootloader: n.supportsBootloader
                }, md);
                ni.params(n.params);
                ni.producedEvents(n.producedEvents);
                cbus.modules.list.push(ni);
            }
        },
        getByNodeNumber: (n) => {
            var f = cbus.modules.list().filter(m => m.nodeNumber === n);
            if (f.length) {
                return f[0];
            }
            return null;
        },
        getId: (d) => {
            if (d.__id == undefined) {
                d.__id = ++id;
            }
            return "module-nv-" + d.__id;
        },
        selectNode: (n) => {
            cbus.modules.selectedNode(n);
        }
    };

    function filterNodes(f) {
        return cbus.modules.list().filter((m) => {
            const filter = f.toUpperCase();
            return m.nodeNumber.toString().indexOf(filter) > -1 ||
                m.nodeType.name.toUpperCase().indexOf(filter) > -1 ||
                m.name().toUpperCase().indexOf(filter) > -1;
        });
    }

    cbus.modules.filteredList = ko.computed(() => {
        if (cbus.modules.listFilter() === "")
            return cbus.modules.list();

        return filterNodes(cbus.modules.listFilter());
    });

    cbus.modules.nodeSelectorList = ko.computed(() => {
        if (cbus.modules.nodeSelectorFilter() === "")
            return cbus.modules.list();

        return filterNodes(cbus.modules.nodeSelectorFilter().toUpperCase());

    });

    cbus.modules.nodeSelector = (a, b, c, d) => {
        $(".node-selector").hide();
        const node = $(b.target).closest(".node");
        
        const selector = $(node).nextAll(".node-selector");
        selector.show();
        selector.click(function(e) {
            e.stopPropagation();
        });

        b.stopPropagation();
    };
    $(document).click(function() {
        $(".node-selector").hide();
        cbus.modules.nodeSelectorFilter("");
    });

    cbus.comms.addHandler("PNN", (msg) => {
        let md = null;
        for (let m in cbus.modules.definitions) {
            let check = cbus.modules.definitions[m];
            if (check.manufacturerId === msg.ManufacturerId && check.moduleId === msg.ModuleId) {
                md = check;
                break;
            }
        }
        if (md == null) {
            md = cbus.modules.definitions["UNKNOWN"];
        }
        const n = new node(msg, md);
        cbus.modules.list.remove(m => m.canId === n.canId);
        cbus.modules.list.push(n);
    });

    cbus.comms.addHandler(0x9B, (msg) => {
        var n = cbus.modules.getByNodeNumber(msg.NodeNumber);
        n.params()[msg.ParameterIndex] = msg.ParameterValue;
        n.params.notifySubscribers();
    });

    cbus.comms.addHandler(0x97,
        (msg) => {
            const n = cbus.modules.getByNodeNumber(msg.NodeNumber);
            
            const nv = n.nodeVariables().find(x => x.index === msg.VariableIndex);
            if (nv != null) {
                nv.value(msg.VariableValue);
            }
        });

    cbus.comms.addHandler("ACON",
        (msg) => {
            const n = cbus.modules.getByNodeNumber(msg.NodeNumber);
            n.addEvent(msg.EventNumber);
        });

    cbus.comms.addHandler("ACOF",
        (msg) => {
            const n = cbus.modules.getByNodeNumber(msg.NodeNumber);
            n.addEvent(msg.EventNumber);
        });


})(window.cbus, ko, jQuery);