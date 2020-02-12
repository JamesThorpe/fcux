using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using FcuCore.Communications;
using FcuCore.Communications.Opcodes;
using FcuCore.Web.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FcuCore.Web.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class ManagerController : ControllerBase
    {
        private readonly CbusManager _manager;

        public ManagerController(CbusManager manager)
        {
            _manager = manager;
        }

        [HttpPost("Communications")]
        public async Task Communications([FromBody]string action)
        {
            
            switch (action) {
                case "open":
                    _manager.OpenComms();
                    break;
                case "close":
                    _manager.CloseComms();
                    break;
                case "enumerate":
                    var msg = new MessageQueryAllNodes();

                    _manager.Messenger.MessageReceived += MessengerOnMessageReceivedEnumerateNodes;
                    await _manager.Messenger.SendMessage(msg);
                    await Task.Delay(3000);
                    _manager.Messenger.MessageReceived -= MessengerOnMessageReceivedEnumerateNodes;

                    break;
            }
        }

        [HttpPost("ReadNodeVariables")]
        public async Task ReadNodeVariables(ReadNodeVariablesRequest request)
        {
            for (byte x = 0; x < request.VariableCount; x++) {
                var m = new MessageReadNodeVariable {
                    NodeNumber = request.NodeNumber,
                    VariableIndex = x
                };
                await _manager.Messenger.SendMessage(m);
            }
        }

        private async void MessengerOnMessageReceivedEnumerateNodes(object sender, CbusMessageEventArgs e)
        {
            try {
                switch (e.Message) {
                    case MessageResponseToQueryNode msg:
                        var m = new MessageRequestReadOfNodeParameterByIndex {
                            ParameterIndex = 0,
                            NodeNumber = msg.NodeNumber
                        };
                        await _manager.Messenger.SendMessage(m);
                        break;
                    case MessageNodeParameterResponse msg:
                        if (msg.ParameterIndex == 0) {
                            for (byte x = 1; x < msg.ParameterValue; x++) {
                                var m2 = new MessageRequestReadOfNodeParameterByIndex {
                                    ParameterIndex = x,
                                    NodeNumber = msg.NodeNumber
                                };
                                await _manager.Messenger.SendMessage(m2);
                            }
                        }
                        break;
                }
            }
            catch (Exception ex) {

            }

        }

        [HttpGet("ConfigureComms")]
        public IActionResult ConfigureComms()
        {
            
            return new JsonResult(_manager.CommsSettings);
        }

        [HttpPost("ConfigureComms")]
        public IActionResult ConfigureComms(CommunicationSettings settings)
        {
            _manager.CommsSettings = settings;
            return Ok();
        }
    }

    
}