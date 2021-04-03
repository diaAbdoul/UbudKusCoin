using System;
using System.Net;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.Hosting;
using UbudKusCoin.Helpers;
using UbudKusCoin.Services;
using UbudKusCoin.Services.DB;
using UbudKusCoin.Services.P2P;

namespace Main
{
    public class Program
    {
        public static void Main(string[] args)
        {

            // TCP server port
            int port = 3000; //default port
            if (args.Length > 0)
            {
                try
                {
                    port = int.Parse(args[0]);
                }
                catch
                {
                    Console.WriteLine("First argument should be port number, ex 3001\n use this commant 'dotnet run 3001'");
                    return;
                }
            }

            var dbName = Utils.CreateDbName(port.ToString());
            AllEvents evt = new AllEvents();
            ServiceManager.Add(
                new DbService(dbName),
                new ChainService(),
                new ForgerService(evt),
                new P2PService(IPAddress.Any, port, evt)
                );
            ServiceManager.Start();


            // grpc
            IHost host = CreateHostBuilder(port).Build();
            host.Run();

        }

        public static IHostBuilder CreateHostBuilder(int port) =>
           Host.CreateDefaultBuilder()
          .ConfigureWebHostDefaults(webBuilder =>
          {
              int portGrpc = port + 100;
              int portWebApi = portGrpc + 10;
              Console.WriteLine("== portGrpc: {0}, portWebApi {1}", portGrpc, portWebApi);
              // if macos
              //   if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
              //   {

              //       webBuilder.ConfigureKestrel(options =>
              //       {
              //           // Setup a HTTP/2 endpoint without TLS.
              //           options.ListenLocalhost(5002, o => o.Protocols =
              //               HttpProtocols.Http2);
              //       });
              //   }

              //webBuilder.ConfigureKestrel(options =>
              //{
              //    options.ListenLocalhost(5002, o => o.Protocols =
              //    HttpProtocols.Http2);
              //});

              webBuilder.ConfigureKestrel(options =>
              {
                  options.ListenAnyIP(portWebApi, listenOptions => listenOptions.Protocols = HttpProtocols.Http1); //webapi
                  options.ListenAnyIP(portGrpc, listenOptions => listenOptions.Protocols = HttpProtocols.Http2); //grpc
              });

              // start
              webBuilder.UseStartup<Startup>();

          });
    }
}
