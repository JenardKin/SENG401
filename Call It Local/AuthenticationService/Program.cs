using AuthenticationService.Database;

using NServiceBus;

using System;
using System.Threading;
using System.Threading.Tasks;

namespace AuthenticationService
{
    /// <summary>
    /// This class is the starting point for the process, responsible for configuring and initializing everything
    /// </summary>
    public class Program
    {
        /// <summary>
        /// Start point for the Authentication Service
        /// </summary>
        public static void Main()
        {
            AsyncMain().GetAwaiter().GetResult();
        }

        /// <summary>
        /// This method is responsible for initializing the endpoint used to publish events from the Authentications Service
        /// </summary>
        /// <returns>Nothing.</returns>
        static async Task AsyncMain()
        {
//#if DEBUG
            Console.Title = "Authentication";// Give the console a title so it is easier to tell them apart
//#endif
            //Create a new Endpoint configuration with the name "Authentication"
            EndpointConfiguration endpointConfiguration = new EndpointConfiguration("Authentication");

            //These two lines prevemt the endpoint configuration from scanning the MySql dll. 
            //This is donw because it speeds up the startup time, and it prevents a rare but 
            //very confusing error sometimes caused by NServiceBus scanning the file. If you 
            //wish to know morw about this, google it, then ask your TA (since they will probably
            //just google it anyway)
            var scanner = endpointConfiguration.AssemblyScanner();
            scanner.ExcludeAssemblies("MySql.Data.dll");

            //Allows the endpoint to run installers upon startup. This includes things such as the creation of message queues.
            endpointConfiguration.EnableInstallers();
            //Instructs the queue to serialize messages with Json, should it need to serialize them
            endpointConfiguration.UseSerialization<JsonSerializer>();
            //Instructs the endpoint to use local RAM to store queues. TODO: Good during development, not during deployment (According to the NServiceBus tutorial)
            endpointConfiguration.UsePersistence<InMemoryPersistence>();
            //Instructs the endpoint to send messages it cannot process to a queue named "error"
            endpointConfiguration.SendFailedMessagesTo("error");
            //Allows the endpoint to make requests to other endpoints and await a response.
            endpointConfiguration.EnableCallbacks();

            //Instructs the endpoint to use Microsoft Message Queuing
            var transport = endpointConfiguration.UseTransport<MsmqTransport>();
            //This variable is used to configure how messages are routed. Using this, you may set the default reciever of a particular command, and/or subscribe to any number of events
            var routing = transport.Routing();

            endpointConfiguration.MakeInstanceUniquelyAddressable("1");

            //Start the endpoint with the configuration defined above. It should be noted that any changes made to the endpointConfiguration after an endpoint is instantiated
            //will not apply to any endpoints that have already been instantiated
            var eventPublishingEndpoint = await Endpoint.Start(endpointConfiguration).ConfigureAwait(false);

            //Create the server object
            Server serverConnection = new Server(eventPublishingEndpoint);

            //Start the server running in its own thread
            Thread serverThread = new Thread(new ThreadStart(serverConnection.StartListening));

            serverThread.Start();//Start the server
            
            Messages.Debug.consoleMsg("Press Enter to exit.");
            string entry;

            do
            {
                entry = Console.ReadLine();

                switch (entry)
                {
                    case ("DELETEDB"):
                        AuthenticationDatabase.getInstance().deleteDatabase();
                        Messages.Debug.consoleMsg("Delete database attempt complete");
                        break;
                    case ("CREATEDB"):
                        AuthenticationDatabase.getInstance().createDB();
                        Messages.Debug.consoleMsg("Completed Database Creation Attempt.");
                        break;
                    default:
                        Messages.Debug.consoleMsg("Command not understood");
                        break;
                }
            } while (!entry.Equals(""));
        }


    }
}
