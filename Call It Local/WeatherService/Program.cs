using Messages;

using NServiceBus;

using System;
using System.Threading.Tasks;

namespace WeatherService
{
    class Program
    {
        //The following is of the same structure as EchoService and AuthenticationService
        //See EchoService/Program.cs or AuthenticationService/Program.cs for fully documentation
        static void Main(string[] args)
        {
            AsyncMain().GetAwaiter().GetResult();
        }

        static async Task AsyncMain()
        {
            Console.Title = "Weather";

            //Create a new Endpoint configuration with the name "Chat"
            EndpointConfiguration endpointConfiguration = new EndpointConfiguration("Weather");

            //These two lines prevemt the endpoint configuration from scanning the MySql dll. 
            //This is done because it speeds up the startup time, and it prevents a rare but 
            //very confusing error sometimes caused by NServiceBus scanning the file. If you 
            //wish to know morw about this, google it, then ask your TA(since they will probably
            //just google it anyway)
            var scanner = endpointConfiguration.AssemblyScanner();
            scanner.ExcludeAssemblies("MySql.Data.dll");

            //Allows the endpoint to run installers upon startup. This includes things such as the creation of message queues.
            endpointConfiguration.EnableInstallers();
            //Instructs the queue to serialize messages with Json, should it need to serialize them
            endpointConfiguration.UseSerialization<JsonSerializer>();
            //Instructs the endpoint to use local RAM to store queues.Good during development, not during deployment (According to the NServiceBus tutorial)
            endpointConfiguration.UsePersistence<InMemoryPersistence>();
            //Instructs the endpoint to send messages it cannot process to a queue named "error"
            endpointConfiguration.SendFailedMessagesTo("error");

            //Instructs the endpoint to use Microsoft Message Queuing 
            var transport = endpointConfiguration.UseTransport<MsmqTransport>();
            //This variable is used to configure how messages are routed. Using this, you may set the default reciever of a particular command, and/or subscribe to any number of events
            var routing = transport.Routing();

            //Start the endpoint with the configuration defined above. It should be noted that any changes made to the endpointConfiguration after an endpoint is instantiated will not apply to any endpoints that have already been instantiated
            var endpointInstance = await Endpoint.Start(endpointConfiguration).ConfigureAwait(false);

            Debug.consoleMsg("Press Enter to exit.");
            string entry;

            do
            {
                entry = Console.ReadLine();

                switch (entry)
                {
                    default:
                        Debug.consoleMsg("Command not understood");
                        break;
                }
            } while (!entry.Equals(""));

            await endpointInstance.Stop().ConfigureAwait(false);
        }
    }
}

