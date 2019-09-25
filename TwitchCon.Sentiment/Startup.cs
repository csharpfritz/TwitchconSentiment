using System;
using System.IO;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.ML;
using TwitchCon.Sentiment;
using TwitchCon.Sentiment.DataModels;

[assembly: FunctionsStartup(typeof(Startup))]
namespace TwitchCon.Sentiment
{

    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            string modelPath = Path.Combine(GetFunctionRootPath(), "MLModels/sentiment_model.zip");
            
            builder.Services.AddPredictionEnginePool<SentimentData, SentimentPrediction>()
                .FromFile(modelPath);
        }

        public string GetFunctionRootPath()
        {
            var home = Environment.GetEnvironmentVariable("HOME");

            if (home != null)
            {
                return Path.Combine(home, "site", "wwwroot");
            }
            else
            {
                return Environment.GetEnvironmentVariable("AzureWebJobsScriptRoot");
            }
        }
    }
}

