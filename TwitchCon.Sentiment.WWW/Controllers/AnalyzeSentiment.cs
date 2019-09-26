using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.ML;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using TwitchCon.Sentiment.WWW.DataModels;

namespace TwitchCon.Sentiment.WWW.Controllers
{

  [ApiController()]
  [Route("[controller]")]
  public class SentimentController : ControllerBase
  {

    private readonly PredictionEnginePool<SentimentData, SentimentPrediction> _predictionEnginePool;
    private readonly ILogger logger;

    private static readonly Dictionary<string, List<(string User, DateTime time, decimal Sentiment)>> _Sentiment = new Dictionary<string, List<(string User, DateTime time, decimal Sentiment)>>();

    // AnalyzeSentiment class constructor
    public SentimentController(PredictionEnginePool<SentimentData, SentimentPrediction> predictionEnginePool, ILoggerFactory logger)
    {
      _predictionEnginePool = predictionEnginePool;
      this.logger = logger.CreateLogger("Sentiment");
    }


    [HttpPost]
    public ActionResult<decimal> Post([FromBody]SentimentData data)
    {

      if (!ModelState.IsValid)
      {
        return BadRequest();
      }

      logger.LogInformation("Sentiment text received: " + data.SentimentText);

      SentimentPrediction prediction = _predictionEnginePool.Predict(data);

      if (!_Sentiment.ContainsKey(data.Channel)) _Sentiment.Add(data.Channel, new List<(string User, DateTime time, decimal Sentiment)>());

      _Sentiment[data.Channel].Add((data.UserName, DateTime.UtcNow, (decimal)prediction.Probability));

      return Ok(prediction.Probability);

    }

    [HttpGet("top/{channel}")]
    public ActionResult<TopSentiment[]> GetTopSentiment(string channel)
    {

      var now = DateTime.UtcNow;

      return _Sentiment[channel].Where(s => s.time.AddHours(3) > now)
        .GroupBy(s => s.User)
        .Select(s => new TopSentiment { UserName = s.Key, Sentiment = s.Average(a => a.Sentiment) })
        .OrderByDescending(s => s.Sentiment)
        .Take(5)
        .ToArray();

    }

    [HttpGet("current/{channel}")]
    public ActionResult<decimal> GetCurrentSentiment(string channel)
    {

      var now = DateTime.UtcNow;
      return _Sentiment[channel].Where(s => s.time.AddMinutes(1) > now).Average(s => s.Sentiment);

    }


  }
}
