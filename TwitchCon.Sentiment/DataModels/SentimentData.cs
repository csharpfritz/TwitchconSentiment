using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.ML.Data;  

namespace TwitchCon.Sentiment.DataModels
{
  public class SentimentData
  {
    [LoadColumn(0)]
    public string SentimentText;

    [LoadColumn(1)]
    [ColumnName("Label")]
    public bool Sentiment;
  }

}
