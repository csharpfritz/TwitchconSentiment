using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.ML.Data;

namespace TwitchCon.Sentiment.DataModels
{
  public class SentimentPrediction : SentimentData
  {

    [ColumnName("PredictedLabel")]
    public bool Prediction { get; set; }

    public float Probability { get; set; }

    public float Score { get; set; }
  }

}
