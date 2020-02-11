class CbusModule {
    constructor(canId, definition) {
        this.CanId = canId;
        this.Definition = definition;
        this.IsConsumerNode = false;
        this.IsProducerNode = false;
        this.InFlimMode = false;
        this.SupportsBootloader = false;
    }
}

(function (cbus) {



    cbus.modules = {
        definitions: {},
        list: ko.observableArray()
    };

    cbus.comms.addHandler(0xB6, (msg) => {
        for (let m in cbus.modules.definitions) {
            const md = cbus.modules.definitions[m];
            if (md.ManufacturerId === msg.ManufacturerId && md.ModuleId === msg.ModuleId) {
                const module = new CbusModule(msg.CanId, md);
                module.IsConsumerNode = msg.IsConsumerNode;
                module.IsProducerNode = msg.IsProducerNode;
                module.InFlimMode = msg.InFlimMode;
                module.SupportsBootloader = msg.SupportsBootloader;
                cbus.modules.list.remove(m => m.CanId === module.CanId);
                cbus.modules.list.push(module);
                break;
            }
        }
    });

})(window.cbus);