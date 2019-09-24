using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Microsoft.Extensions.ML;
using TwitchCon.Sentiment.DataModels;

namespace TwitchCon.Sentiment
{
  public class AnalyzeSentiment
  {

    private readonly PredictionEnginePool<SentimentData, SentimentPrediction> _predictionEnginePool;

    // AnalyzeSentiment class constructor
    public AnalyzeSentiment(PredictionEnginePool<SentimentData, SentimentPrediction> predictionEnginePool)
    {
      _predictionEnginePool = predictionEnginePool;
    }


    [FunctionName("AnalyzeSentiment")]
    public async Task<IActionResult> Run(
[HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] HttpRequest req,
ILogger log)
    {
      log.LogInformation("C# HTTP trigger function processed a request.");

      //Parse HTTP Request Body
      string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
      SentimentData data = JsonConvert.DeserializeObject<SentimentData>(requestBody);

      //Make Prediction
      SentimentPrediction prediction = _predictionEnginePool.Predict(data);

      //Convert prediction to string
      int sentiment = Convert.ToBoolean(prediction.Prediction) ? 1 : 0;

      //Return Prediction
      return (ActionResult)new OkObjectResult(sentiment);
    }

  }
}
