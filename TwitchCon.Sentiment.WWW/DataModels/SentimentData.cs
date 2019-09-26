using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.ML.Data;  

namespace TwitchCon.Sentiment.WWW.DataModels
{
  public class SentimentData
  {

    [LoadColumn(0)]
    public string SentimentText;

    [LoadColumn(1)]
    [ColumnName("Label")]
    public bool Sentiment;

    [ColumnName("User")]
    public string UserName;

    [ColumnName("Channel")]
    public string Channel;


  }



}
