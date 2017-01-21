using System;
using System.Messaging;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Threading;
using System.Data.SqlClient;
using Solishine.CommonLib;
using Solishine.CommonLib.Daemon;
using Aci.X.ClientLib.ProfileTypes;
using Aci.X.IwsLib.DB;
using Aci.X.Database;
using Newtonsoft.Json;

namespace Aci.X.IwsLib
{
  public class IwsPreviewConsumerQueue : SolishineMessageQueue
  {
    private const string QUEUE_NAME = @"IwsPreviewConsumer";
    public static IwsPreviewConsumerQueue Singleton = new IwsPreviewConsumerQueue();

    public IwsPreviewConsumerQueue()
      : base(strQueueName: QUEUE_NAME, intMaxConcurrentOps: IwsConfig.PreviewConsumerMaxConcurrentOps)
    {
    }

    public void Queue(int intQueryID, bool boolAsynch = true)
    {
      if (QueueEnabled)
      {
        Send(intQueryID.ToString());
      }
      else
      {
        if (!boolAsynch)
          Process(intQueryID);
        else
        {
          Interlocked.Increment(ref _longOpsPending);
          ThreadPool.QueueUserWorkItem(new WaitCallback(DoProcess), new Message { Body = intQueryID.ToString() });
        }
      }
    }

    protected override void Process(Message msg)
    {
      int intQueryID = Convert.ToInt32(msg.Body);
      Process(intQueryID);
    }

    public int Process(int intQueryID)
    {
      try
      {
        using (var dbProfile = new ProfileDB(new SqlConnection(IwsConfig.ProfileConnectionString)))
        {
          string strJson;
          string strConvertedJson;
          ProfileResponse response;
          var results = dbProfile.spSearchResultsGet(
            intQueryID: intQueryID, 
            boolIgnoreDateCachedBefore1111: false)
            .FirstOrDefault();
          if (results==null)
          {
            _logger.Warn("QueryID={0} has no results", intQueryID);
            return 0;
          }

          if (results.CompressedResults == null)
          {
            _logger.Warn("QueryID={0} has NULL CompressedResults; deleting.", intQueryID);
            dbProfile.spSearchResultsDelete(intQueryID);
            return 0;
          }

          try 
          {
            strJson = CompressionHelper.UncompressString(results.CompressedResults);
            response = JsonConvert.DeserializeObject<ProfileResponse>(strJson);
          }
          catch (Exception ex)
          {
            _logger.Warn("QueryID={0} cannot deserialize unconverted JSON: {1}; deleting.", intQueryID, ex.Message);
            dbProfile.spSearchResultsDelete(intQueryID);
            return 0;
          }

          try
          {
            strConvertedJson = ProfileHelper.ConvertTextJson(strJson);
            // Try to deserialize it again, just to make sure the conversion didn't mess things up.
            response = JsonConvert.DeserializeObject<ProfileResponse>(strConvertedJson);
          }
          catch (Exception ex)
          {
            _logger.Warn("QueryID={0} cannot deserialize converted JSON: {1}; deleting.", intQueryID, ex.Message);
            dbProfile.spSearchResultsDelete(intQueryID);
            return 0;
          }

          if (!results.Minimized)
          {
            try
            {
              // Eliminate "default" values, and other stuff we don't want (Upsell Links)
              strConvertedJson = JsonConvert.SerializeObject(response);
              if (strConvertedJson.Length != strJson.Length)
              {
                byte[] tCompressedJson = CompressionHelper.CompressString(strConvertedJson);
                dbProfile.spSearchResultsSaveMinimized(intQueryID, tCompressedJson);
              }
            }
            catch (Exception ex)
            {
              _logger.Warn("QueryID={0} error minimizing: {1}; deleting.", intQueryID, ex.Message);
              dbProfile.spSearchResultsDelete(intQueryID);
              return 0;
            }
          }


          List<int> listPersonIDs = new List<int>();
          foreach (Profile profile in response.Profiles.Profile)
          {
            try
            {
              int intPersonID = ServerProfile.UpdateDB(profile);
              listPersonIDs.Add(intPersonID);
            }
            catch (Exception exProf)
            {
              _logger.Error("QueryID {0,10}, Profile: {1,11}: {2} at {3}", intQueryID, profile.ProfileID, exProf.Message, exProf.StackTrace);
            }
          }
          dbProfile.spSearchResultsSetPersonIDs(intQueryID: intQueryID, strListPersonIDs: String.Join(",", listPersonIDs.ToArray()));
        }
      }
      catch (Exception ex)
      {
        _logger.Error("QueryID {0,10}: {1} at {2}", intQueryID, ex.Message, ex.StackTrace);
      }
      return 0;
    }
  }
}
