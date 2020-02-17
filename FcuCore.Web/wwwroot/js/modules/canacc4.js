
cbus.modules.definitions["CANACC4"] = {
 
    manufacturerId: 165,
    moduleId: 8,
    name: "CANACC4",
    nvDefaultValues: [5, 5, 5, 5, 5, 5, 5, 20, 20],
    configGroups: [
        {
            name: "General",
            nodeVariables: [{
                index: 9,
                definition: {
                    name: "Recharge Time",
                    type: "numeric"
                }
            }, {
                index: 10,
                definition: {
                    name: "Fire Delay",
                    type: "numeric"
                }
            }]
        }, {
            name: "Output Timings",
            nodeVariables: [
                {
                    index: 1,
                    definition: {
                        name: "Pulse Duration Output 1 A",
                        type: "numeric"
                    }
                }, {
                    index: 2,
                    definition: {
                        name: "Pulse Duration Output 1 B",
                        type: "numeric"
                    }
                }, {
                    index: 3,
                    definition: {
                        name: "Pulse Duration Output 2 A",
                        type: "numeric"
                    }
                }, {
                    index: 4,
                    definition: {
                        name: "Pulse Duration Output 2 B",
                        type: "numeric"
                    }
                }, {
                    index: 5,
                    definition: {
                        name: "Pulse Duration Output 3 A",
                        type: "numeric"
                    }
                }, {
                    index: 6,
                    definition: {
                        name: "Pulse Duration Output 3 B",
                        type: "numeric"
                    }
                }, {
                    index: 7,
                    definition: {
                        name: "Pulse Duration Output 4 A",
                        type: "numeric"
                    }
                }, {
                    index: 8,
                    definition: {
                        name: "Pulse Duration Output 4 B",
                        type: "numeric"
                    }
                }
            ]
        }
    ]
};
