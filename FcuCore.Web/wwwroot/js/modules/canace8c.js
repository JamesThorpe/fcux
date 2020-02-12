cbus.modules.definitions["CANACE8C"] = {
    manufacturerId: 165,
    moduleId: 5,
    name: "CANACE8C",
    configGroups: [
        {
            name: "General",
            nodeVariables: [
                {
                    index: 1,
                    definition: {
                        name: "On/Off or On selection",
                        default: 0,
                        type: "bitfield"
                    }
                }, {
                    index: 2,
                    definition: {
                        name: "Invert input",
                        default: 0,
                        type: "bitfield"
                    }
                }, {
                    index: 3,
                    definition: {
                        name: "Enable input delay",
                        default: 0,
                        type: "bitfield"
                    }
                }, {
                    index: 4,
                    definition: {
                        name: "Input delay, ON time",
                        default: 10,
                        type: "numeric"
                    }
                }, {
                    index: 5,
                    definition: {
                        name: "Input delay, OFF time",
                        default: 10,
                        type: "numeric"
                    }
                }, {
                    index: 6,
                    definition: {
                        name: "Push button toggle",
                        default: 0,
                        type: "bitfield"
                    }
                }, {
                    index: 7,
                    definition: {
                        name: "Route options",
                        default: 0,
                        type: "numeric"
                    }
                }, {
                    index: 8,
                    definition: {
                        name: "Disable SOD",
                        default: 0,
                        type: "bitfield"
                    }
                }
            ]
        }
    ]
};