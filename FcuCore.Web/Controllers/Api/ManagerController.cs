using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using FcuCore.Communications;
using FcuCore.Communications.Opcodes;
using FcuCore.Web.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace FcuCore.Web.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class ManagerController : ControllerBase
    {
        private readonly CbusManager _manager;
        private readonly IConfiguration _configuration;

        public ManagerController(CbusManager manager, IConfiguration configuration)
        {
            _manager = manager;
            _configuration = configuration;
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
                    var msg = new QueryAllNodesMessage();

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
            for (byte x = 1; x <= request.VariableCount; x++) {
                var m = new ReadNodeVariableMessage {
                    NodeNumber = request.NodeNumber,
                    VariableIndex = x
                };
                await _manager.Messenger.SendMessage(m);
            }
        }

        [HttpPost("SaveData")]
        public async Task SaveData([FromBody]string data)
        {
            using (var sw = new StreamWriter(_configuration.GetSection("Fcu")["FilePath"])) {
                await sw.WriteAsync(data);
            }
        }

        [HttpGet("LoadData")]
        public async Task<IActionResult> LoadData()
        {
            using (var sr = new StreamReader(_configuration.GetSection("Fcu")["FilePath"])) {
                return new JsonResult(await sr.ReadToEndAsync());
            }
        }

        private async void MessengerOnMessageReceivedEnumerateNodes(object sender, CbusMessageEventArgs e)
        {
            try {
                switch (e.Message) {
                    case QueryNodesResponseMessage msg:
                        var m = new ReadNodeParameterByIndexMessage {
                            ParameterIndex = 0,
                            NodeNumber = msg.NodeNumber
                        };
                        await _manager.Messenger.SendMessage(m);
                        break;
                    case ReadNodeParameterByIndexResponseMessage msg:
                        if (msg.ParameterIndex == 0) {
                            for (byte x = 1; x < msg.ParameterValue; x++) {
                                var m2 = new ReadNodeParameterByIndexMessage {
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