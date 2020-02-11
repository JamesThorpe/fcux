using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FcuCore.Communications.Opcodes;
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
        public void Communications([FromBody]string action)
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
                    _manager.Messenger.SendMessage(msg);
                    break;
            }
        }
    }

    
}