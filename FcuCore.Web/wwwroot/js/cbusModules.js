﻿(function (cbus, ko, $) {

    /*
    function nodeVariable(group, nv, node) {
        this.group = group;
        this.index = nv.index;
        this.nv = nv;
        this.definition = ko.computed(() => {
            if (nv.definition) {
                return nv.definition;
            }

            let targetDefinition;
            if (nv.definitions != undefined) {
                nv.definitions.forEach((d) => {
                    if (typeof (d.condition) === "function") {
                        if (d.condition(node)) {
                            targetDefinition = d.definition;
                        }
                    }
                });
            }
            return targetDefinition;
        });
        let defaultValue = 0;
        if (this.definition() != undefined) {
            defaultValue = this.definition().default;
        }
        this.value = ko.observable(defaultValue);
    }
    */

    function node(config, type) {
        this.nodeType = type;
        this.nodeNumber = config.NodeNumber;
        this.canId = config.CanId;
        this.isConsumerNode = config.IsConsumerNode;
        this.isProducerNode = config.IsProducerNode;
        this.inFlimMode = config.InFlimMode;
        this.supportsBootloader = config.SupportsBootloader;

        this.params = ko.observableArray([]);

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

        /*
        this.nodeVariables = ko.observableArray();
        type.configGroups.forEach(cg => {
            cg.nodeVariables.forEach(nv => {
                this.nodeVariables.push(new nodeVariable(cg, nv, this));
            });
        });
        */
    }

    /*
    node.prototype.getValuesInGroup = function (group) {
        return this.nodeVariables().filter((nv) => nv.group === group);
    };

    node.prototype.getNodeVariableValue = function (index) {
        const nv = this.nodeVariables().find((nv) => nv.nv.index === index);
        if (nv != null) {
            return nv.value();
        }
        return null;
    };
    */

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
    /*
    node.prototype.getNodeVariableByIndex = function (i) {
        const nv = this.nodeVariables().find((n) => n.index === i);
        return nv ? nv : null;
    };
    */

    cbus.modules = {
        definitions: {},
        list: ko.observableArray(),
        listFilter: ko.observable(''),
        currentNode: ko.observable(null),
        getByNodeNumber: (n) => {
            var f = cbus.modules.list().filter(m => m.nodeNumber === n);
            if (f.length) {
                return f[0];
            }
            return null;
        }
    };

    cbus.modules.filteredList = ko.computed(() => {
        if (cbus.modules.listFilter() === "")
            return cbus.modules.list();

        return cbus.modules.list().filter((m) => {
            return m.nodeNumber.toString().indexOf(cbus.modules.listFilter()) > -1 ||
                m.nodeType.name.toUpperCase().indexOf(cbus.modules.listFilter().toUpperCase()) > -1;
            
        });
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
            var n = cbus.modules.getByNodeNumber(msg.NodeNumber);
            var nv = n.getNodeVariableByIndex(msg.VariableIndex);
            if (nv != null) {
                nv.value(msg.VariableValue);
            }
        });

})(window.cbus, ko, jQuery);