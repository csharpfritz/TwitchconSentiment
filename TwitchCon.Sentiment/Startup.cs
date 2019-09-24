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
      builder.Services.AddPredictionEnginePool<SentimentData, SentimentPrediction>()
          .FromFile("MLModels/sentiment_model.zip");
    }
  }

}

