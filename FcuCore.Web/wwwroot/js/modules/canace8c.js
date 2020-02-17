
function generateGeneralInput(index) {
    return {
        group: "Input " + index,
        items: [
            {
                name: "On/Off or On selection",
                type: "flag",
                nv: 1,
                flagValue: (1 << (8-index))
            }, {
                name: "Invert Input",
                type: "flag",
                nv: 2,
                flagValue: (1 << (8 - index))
            }, {
                name: "Push button toggle",
                type: "flag",
                nv: 6,
                flagValue: (1 << (8 - index))
            }
        ]
    }
}

function generateAdvancedInput(index) {
    return {
        group: "Input " + index,
        items: [
            {
                name: "Delayed Input",
                type: "flag",
                nv: 3,
                flagValue: (1 << (8 - index))
            }, {
                name: "Disable SOD",
                type: "flag",
                nv: 8,
                flagValue: (1 << (8 - index))
            }
        ]
    };
}

cbus.modules.definitions["CANACE8C"] = {
    manufacturerId: 165,
    moduleId: 5,
    name: "CANACE8C",
    configTabs: [
        {
            name: "General",
            items: [
                {
                    //Example of creating an input group based on specifying all values
                    group: "Input 1",
                    items: [
                        {
                            name: "On/Off or On selection",
                            type: "flag",
                            nv: 1,
                            flagValue: 128
                        }, {
                            name: "Invert Input",
                            type: "flag",
                            nv: 2,
                            flagValue: 128
                        }, {
                            name: "Push button toggle",
                            type: "flag",
                            nv: 6,
                            flagValue: 128
                        }
                    ]

                },
                //The rest of the inputs are generated using a function returning a template item
                //It saves a lot of repetition, though you could repeat the above for all 8
                generateGeneralInput(2),
                generateGeneralInput(3),
                generateGeneralInput(4),
                generateGeneralInput(5),
                generateGeneralInput(6),
                generateGeneralInput(7),
                generateGeneralInput(8)
            ]
        }, {
            name: "Advanced",
            items: [
                {
                    //Again, first input is created by specifying all values
                    group: "Input 1",
                    items: [
                        {
                            name: "Delayed Input",
                            type: "flag",
                            nv: 3,
                            flagValue: 128
                        }, {
                            name: "Disable SOD",
                            type: "flag",
                            nv: 8,
                            flagValue: 128
                        }
                    ]
                },
                //And the others are done using a template function
                generateAdvancedInput(2),
                generateAdvancedInput(3),
                generateAdvancedInput(4),
                generateAdvancedInput(5),
                generateAdvancedInput(6),
                generateAdvancedInput(7),
                generateAdvancedInput(8),
                {
                    name: "Input delay, ON time",
                    type: "numeric",
                    nv: 4
                }, {
                    name: "Input delay, OFF time",
                    type: "numeric",
                    nv: 5
                }
            ]
        }
    ]
};