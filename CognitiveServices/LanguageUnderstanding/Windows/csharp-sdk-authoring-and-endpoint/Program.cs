using System;
using System.Threading.Tasks;

// v3.0.0 authoring
using Microsoft.Azure.CognitiveServices.Language.LUIS.Authoring;
using Microsoft.Azure.CognitiveServices.Language.LUIS.Authoring.Models;

// v2.8.0-preview prediction runtime
using Microsoft.Azure.CognitiveServices.Language.LUIS.Runtime;
using Microsoft.Azure.CognitiveServices.Language.LUIS.Runtime.Models;

namespace LUIS_Prediction_CS
{
    struct ApplicationInfo
    {
        public Guid ID;
        public string Version;
    }

    //private static readonly string runtime_key = Environment.GetEnvironmentVariable(runtime_key_var);

    class Program
    {
        private const string runtime_key_var = "LUIS_RUNTIME_KEY";
        private const string runtime_endpoint_var = "LUIS_RUNTIME_ENDPOINT";

        private static readonly string runtime_key = Environment.GetEnvironmentVariable(runtime_key_var);
        private static readonly string runtime_endpoint = Environment.GetEnvironmentVariable(runtime_endpoint_var); 


        private const string auth_key_var = "LUIS_AUTHORING_KEY";
        private const string auth_endpoint_var = "LUIS_AUTHORING_ENDPOINT";

        private static readonly string authoring_key = Environment.GetEnvironmentVariable(auth_key_var);
        private static readonly string authoring_endpoint = Environment.GetEnvironmentVariable(auth_endpoint_var);

        static Program()
        {
            if (null == runtime_key)
            {
                throw new Exception("Please set/export the environment variable: " + runtime_key);
            }
            if (null == runtime_endpoint)
            {
                throw new Exception("Please set/export the environment variable: " + runtime_endpoint);
            }
            if (null == authoring_key)
            {
                throw new Exception("Please set/export the environment variable: " + authoring_key);
            }
            if (null == authoring_endpoint)
            {
                throw new Exception("Please set/export the environment variable: " + authoring_endpoint);
            }
        }

        // Create a new LUIS application. Return the application ID and version.
        async static Task<ApplicationInfo> CreateApplication(LUISAuthoringClient client)
        {
            string app_version = "0.1";
            var app_info = new ApplicationCreateObject()
            {
                Name = String.Format("Contoso {0}", DateTime.Now),
                InitialVersionId = app_version,
                Description = "Flight booking app built with LUIS .NET SDK.",
                Culture = "en-us"
            };
            var app_id = await client.Apps.AddAsync(app_info);
            Console.WriteLine("Created new LUIS application {0}\n with ID {1}.", app_info.Name, app_id);
            return new ApplicationInfo() { ID = app_id, Version = app_version };
        }

        // Train a LUIS application.
        async static Task Train_App(LUISAuthoringClient client, ApplicationInfo app)
        {
            var response = await client.Train.TrainVersionAsync(app.ID, app.Version);
            Console.WriteLine("Training status: " + response.Status);
        }

        // Publish a LUIS application and show the endpoint URL for the published application.
        async static Task Publish_App(LUISAuthoringClient client, ApplicationInfo app)
        {
            ApplicationPublishObject obj = new ApplicationPublishObject
            {
                VersionId = app.Version,
                IsStaging = false // publish to production
                
            };
            var info = await client.Apps.PublishAsync(app.ID, obj);
            Console.WriteLine("Endpoint URL: " + info.EndpointUrl);
        }

        // Send a query to a LUIS application.
        async static Task Query_App(LUISRuntimeClient client, ApplicationInfo app, string query)
        {
            PredictionRequest obj = new PredictionRequest
            {
                Query = query
            };
            try
            {
                var info = await client.Prediction.GetSlotPredictionAsync(app.ID, "production", obj);
                Console.WriteLine(info.ToString());
            }
            catch (ErrorException e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine(e.Response.Content);
            }
        }

        // Delete a LUIS application.
        async static Task Delete_App(LUISAuthoringClient client, ApplicationInfo app)
        {
            await client.Apps.DeleteAsync(app.ID);
            Console.WriteLine("Deleted application with ID {0}.", app.ID);
        }

        async static Task RunQuickstart()
        {
            // Generate the credentials and create the authoring client.
            var authoring_credentials = new Microsoft.Azure.CognitiveServices.Language.LUIS.Authoring.ApiKeyServiceClientCredentials(authoring_key);
            var authoring_client = new LUISAuthoringClient(authoring_credentials, new System.Net.Http.DelegatingHandler[] { })
            {
                Endpoint = authoring_endpoint
            };

            // Generate the credentials and create the runtime client.
            var runtime_credentials = new Microsoft.Azure.CognitiveServices.Language.LUIS.Runtime.ApiKeyServiceClientCredentials(runtime_key);
            var runtime_client = new LUISRuntimeClient(runtime_credentials, new System.Net.Http.DelegatingHandler[] { })
            {
                Endpoint = runtime_endpoint
            };

            Console.WriteLine("Creating application...");
            var app = await CreateApplication(authoring_client);
            Console.WriteLine();

            /* We skip adding entities, intents, and utterances because the
             * predict method will not find the app anyway. */

            Console.WriteLine("Training application...");
            await Train_App(authoring_client, app);
            Console.WriteLine("Waiting 30 seconds for training to complete...");
            System.Threading.Thread.Sleep(30000);
            Console.WriteLine();

            Console.WriteLine("Publishing application...");
            await Publish_App(authoring_client, app);
            Console.WriteLine();

            Console.WriteLine("Querying application...");
            /* It doesn't matter what query we send because the predict method
             * will not find the app anyway. */
            await Query_App(runtime_client, app, "test");
            Console.WriteLine();

            Console.WriteLine("Deleting application...");
            await Delete_App(authoring_client, app);
        }

        static void Main(string[] args)
        {
            Task.WaitAll(RunQuickstart());
            Console.WriteLine("Press any key to exit.");
            Console.Read();
        }
    }
}