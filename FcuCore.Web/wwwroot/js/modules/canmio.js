const ioTypes = [
    {
        name: "Input",
        value: 0
    }, {
        name: "Output",
        value: 1
    }, {
        name: "Servo",
        value: 2
    }, {
        name: "Bounce",
        value: 3
    }, {
        name: "Multi",
        value: 4
    }, {
        name: "Analogue",
        value: 5
    }, {
        name: "Magnet",
        value: 6
    }
];

function generateIO(index, baseNv) {
    return {
        group: "I/O " + index,
            items: [
                {
                    name: "Type",
                    type: "select",
                    nv: baseNv,
                    selectValues: ioTypes
                }, {
                    group: "Flags",
                    items: [
                        {
                            name: "Trigger Inverted",
                            type: "flag",
                            nv: baseNv + 1,
                            flagValue: 1
                        }, {
                            name: "Cutoff",
                            visible: (n) => {
                                const t = n.getNodeVariable(baseNv);
                                return t === 2 || t === 3 || t === 4;
                            },
                            type: "flag",
                            nv: baseNv + 1,
                            flagValue: 2
                        }, {
                            name: "Startup",
                            visible: (n) => {
                                const t = n.getNodeVariable(baseNv);
                                return t === 1 || t === 2 || t === 3 || t === 4;
                            },
                            type: "flag",
                            nv: baseNv + 1,
                            flagValue: 4
                        }, {
                            name: "Disable Off",
                            visible: (n) => {
                                const t = n.getNodeVariable(baseNv);
                                return t !== 4;
                            },
                            type: "flag",
                            nv: baseNv + 1,
                            flagValue: 8
                        }, {
                            name: "Toggle",
                            visible: (n) => {
                                const t = n.getNodeVariable(baseNv);
                                return t === 0;
                            },
                            type: "flag",
                            nv: baseNv + 1,
                            flagValue: 16
                        }, {
                            name: "Action Inverted",
                            visible: (n) => {
                                const t = n.getNodeVariable(baseNv);
                                return t === 1 || t === 2 || t === 3 || t === 4;
                            },
                            type: "flag",
                            nv: baseNv + 1,
                            flagValue: 32
                        }, {
                            name: "Event Inverted",
                            type: "flag",
                            nv: baseNv + 1,
                            flagValue: 64
                        }, {
                            name: "Action Expedited",
                            visible: (n) => {
                                const t = n.getNodeVariable(baseNv);
                                return t === 1;
                            },
                            type: "flag",
                            nv: baseNv + 1,
                            flagValue: 128
                        }
                    ]
                }, {
                    group: "Input options",
                    visible: (n) => n.getNodeVariable(baseNv) === 0,
                    items: [
                        {
                            name: "ON delay",
                            type: "numeric",
                            nv: baseNv + 2
                        }, {
                            name: "OFF delay",
                            type: "numeric",
                            nv: baseNv + 3
                        }
                    ]
                }, {
                    group: "Output options",
                    visible: (n) => n.getNodeVariable(baseNv) === 1,
                    items: [
                        {
                            name: "Pulse duration",
                            type: "numeric",
                            nv: baseNv + 2
                        }, {
                            name: "Flash duration",
                            type: "numeric",
                            nv: baseNv + 3
                        }
                    ]
                }, {
                    group: "Server options",
                    visible: (n) => n.getNodeVariable(baseNv) === 2,
                    items: [
                        {
                            name: "OFF Pos",
                            type: "numeric",
                            nv: baseNv + 2
                        }, {
                            name: "ON Pos",
                            type: "numeric",
                            nv: baseNv + 3
                        }, {
                            name: "OFF to ON speed",
                            type: "numeric",
                            nv: baseNv + 4
                        }, {
                            name: "ON to OFF speed",
                            type: "numeric",
                            nv: baseNv + 5
                        }
                    ]
                }, {
                    group: "Bounce options",
                    visible: (n) => n.getNodeVariable(baseNv) === 3,
                    items: [
                        {
                            name: "Upper Pos",
                            type: "numeric",
                            nv: baseNv + 2
                        }, {
                            name: "Lower Pos",
                            type: "numeric",
                            nv: baseNv + 3
                        }, {
                            name: "Bounce coefficient",
                            type: "numeric",
                            nv: baseNv + 4
                        }, {
                            name: "Pull speed",
                            type: "numeric",
                            nv: baseNv + 5
                        }, {
                            name: "Pull pause",
                            type: "numeric",
                            nv: baseNv + 6
                        }
                    ]
                }, {
                    group: "Multi options",
                    visible: (n) => n.getNodeVariable(baseNv) === 4,
                    items: [
                        {
                            name: "Num Pos",
                            type: "numeric",
                            nv: baseNv + 2
                        }, {
                            name: "Pos 1",
                            type: "numeric",
                            nv: baseNv + 3
                        }, {
                            name: "Pos 2",
                            type: "numeric",
                            nv: baseNv + 4
                        }, {
                            name: "Pos 3",
                            type: "numeric",
                            nv: baseNv + 5
                        }, {
                            name: "Pos 4",
                            type: "numeric",
                            nv: baseNv + 6
                        }
                    ]
                }, {
                    group: "Analogue options",
                    //TODO: check why not IO 12? mistake on the wiki?
                    visible: (n) => n.getNodeVariable(baseNv) === 5 && (index === 9 || index === 10 || index === 11 || index === 13 || index === 14 || index === 15 || index === 16),
                    items: [
                        {
                            name: "Threshold",
                            type: "numeric",
                            nv: baseNv + 3
                        }, {
                            name: "Hysteresis",
                            type: "numeric",
                            nv: baseNv + 4
                        }
                    ]
                }, {
                    group: "Magnet options",
                    //TODO: check why not IO 12? mistake on the wiki?
                    visible: (n) => n.getNodeVariable(baseNv) === 6 && (index === 9 || index === 10 || index === 11 || index === 13 || index === 14 || index === 15 || index === 16),
                    items: [
                        {
                            name: "Do Setup",
                            type: "numeric",
                            nv: baseNv + 2
                        }, {
                            name: "Threshold",
                            type: "numeric",
                            nv: baseNv + 3
                        }, {
                            name: "Hysteresis",
                            type: "numeric",
                            nv: baseNv + 4
                        }, {
                            name: "Offset H",
                            type: "numeric",
                            nv: baseNv + 4
                        }, {
                            name: "Offset L",
                            type: "numeric",
                            nv: baseNv + 5
                        }
                    ]
                }
            ]
    }
}

cbus.modules.definitions["CANMIO"] = {
    manufacturerId: 165,
    moduleId: 32,
    name: "CANMIO",
    configTabs: [
        {
            name: "General",
            items: [
                {
                    name: "Produced Startup Delay",
                    type: "numeric",
                    nv: 1
                }, {
                    name: "HB Delay",
                    type: "numeric",
                    nv: 2
                }, {
                    name: "Servo Speed",
                    type: "numeric",
                    nv: 3
                }, {
                    name: "PORTB Pull-ups enable",
                    type: "bitfield",
                    nv: 4
                }
            ]
        }, {
            name: "IO 1-8",
            items: [
                generateIO(1, 16),
                generateIO(2, 23),
                generateIO(3, 30),
                generateIO(4, 37),
                generateIO(5, 44),
                generateIO(6, 51),
                generateIO(7, 58),
                generateIO(8, 65)
            ]
        }, {
            name: "IO 9-16",
            items: [
                generateIO(9, 72),
                generateIO(10, 79),
                generateIO(11, 86),
                generateIO(12, 93),
                generateIO(13, 100),
                generateIO(14, 107),
                generateIO(15, 114),
                generateIO(16, 121)
            ]
        }

        /*,
        generateIO(1, 16),
        generateIO(2, 23),
        generateIO(3, 30),
        generateIO(4, 37),
        generateIO(5, 44),
        generateIO(6, 51),
        generateIO(7, 58),
        generateIO(8, 65),
        generateIO(9, 72),
        generateIO(10, 79),
        generateIO(11, 86),
        generateIO(12, 93),
        generateIO(13, 100),
        generateIO(14, 107),
        generateIO(15, 114),
        generateIO(16, 121)*/
    ]
};