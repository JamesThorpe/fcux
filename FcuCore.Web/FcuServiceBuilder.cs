using System;
using System.Collections.Generic;
using System.Text;
using FcuCore.Communications;
using Microsoft.Extensions.DependencyInjection;

namespace FcuCore
{
    public static class FcuServiceBuilder
    {
        public static void AddFcu(this IServiceCollection services)
        {
            services.AddSingleton<CbusManager, CbusManager>();
        }
    }
}
