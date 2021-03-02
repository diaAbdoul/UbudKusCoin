using System;
using System.Net;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.Hosting;
using UbudKusCoin;
using UbudKusCoin.P2P;

namespace Main
{
    public class Program
    {
        public static void Main(string[] args)
        {

            

            // TCP server port
            int port = 80;

            if (args.Length > 0)
                port = int.Parse(args[0]);


            // db
            DbAccess.Initialize(port);

            // blockchain
            _ = new Blockchain();


            Console.WriteLine($"TCP server port: {port}");

            Console.WriteLine();



            AllEvents evt = new AllEvents();
        
            // run all service 
            ServiceManager.Add(
                new P2PService(IPAddress.Any, port, evt),
                new BlockForger(evt)
            );
            ServiceManager.Forger.Start();
            ServiceManager.P2pService.StartServer();

            if ( port!= 3000) {
                ServiceManager.P2pService.Connect("127.0.0.1", 3000);
            }    

            // grpc
            IHost host = CreateHostBuilder(args).Build();
            //host.Services.UseScheduler(scheduler =>
            //{
            //    scheduler.Schedule<BlockJob>()
            //        .EveryFifteenSeconds();
            //});
            host.Run();

        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
           Host.CreateDefaultBuilder(args)
          .ConfigureWebHostDefaults(webBuilder =>
          {
              int port = int.Parse(args[0]);
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
