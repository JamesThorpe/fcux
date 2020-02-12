(function (cbus) {

    function nodeVariable(group, nv, node) {
        this.group = group;
        this.nv = nv;
        this.definition = ko.computed(() => {
            if (nv.definition) {
                return nv.definition;
            }

            let targetDefinition;
            nv.definitions.forEach((d) => {
                if (typeof (d.condition) === "function") {
                    if (d.condition(node)) {
                        targetDefinition = d.definition;
                    }
                }
            });
            return targetDefinition;
        });
        this.value = ko.observable(this.definition().default);
    }

    function node(config, type) {
        this.nodeType = type;
        this.nodeNumber = config.NodeNumber;
        this.canId = config.CanId;
        this.isConsumerNode = config.IsConsumerNode;
        this.isProducerNode = config.IsProducerNode;
        this.inFlimMode = config.InFlimMode;
        this.supportsBootloader = config.SupportsBootloader;

        this.nodeVariables = ko.observableArray();
        type.configGroups.forEach(cg => {
            cg.nodeVariables.forEach(nv => {
                this.nodeVariables.push(new nodeVariable(cg, nv, this));
            });
        });
    }

    node.prototype.getValuesInGroup = function (group) {
        return this.nodeVariables().filter((nv) => nv.group === group);
    };

    node.prototype.getNodeVariable = function (index) {
        const nv = this.nodeVariables().find((nv) => nv.nv.index === index);
        if (nv != null) {
            return nv.value();
        }
        return null;
    };

    node.prototype.editNodeVariables = function() {
        cbus.modules.currentNode(this);
        $("#dialog-edit-node-variables").modal("show");
    };
    node.prototype.closeEditNodeVariables = function() {
        $("#dialog-edit-node-variables").modal("hide");
        cbus.modules.currentNode(null);
    };

    cbus.modules = {
        definitions: {},
        list: ko.observableArray(),
        currentNode: ko.observable(null)
    };

    cbus.comms.addHandler(0xB6, (msg) => {
        for (let m in cbus.modules.definitions) {
            const md = cbus.modules.definitions[m];
            if (md.manufacturerId === msg.ManufacturerId && md.moduleId === msg.ModuleId) {
                const n = new node(msg, md);
                cbus.modules.list.remove(m => m.canId === n.canId);
                cbus.modules.list.push(n);
                break;
            }
        }
    });

})(window.cbus);