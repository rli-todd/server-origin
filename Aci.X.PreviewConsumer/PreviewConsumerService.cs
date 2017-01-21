using System;
using System.ServiceProcess;
using System.Threading;
using NLog;
using Aci.X.IwsLib;
using Solishine.CommonLib.Daemon;

namespace Aci.X.PreviewConsumer
{
  public partial class PreviewConsumerService: ServiceBase
  {
    private static Logger _logger = LogManager.GetCurrentClassLogger();
    private IwsPreviewConsumerQueue _queueProcessor = IwsPreviewConsumerQueue.Singleton;

    public PreviewConsumerService()
    {
      try
      {
        ServiceName = GetType().FullName;
        _logger.Info("{0} instantiating", ServiceName);
        CanPauseAndContinue = false;
        CanHandleSessionChangeEvent = true;
        CanShutdown = true;
        CanStop = true;
        AutoLog = true;
        _logger.Info("{0}: Instantiated", ServiceName);
      }
      catch (Exception ex)
      {
        _logger.ErrorException(String.Format("{0}: {1}", ex.Message, ex.StackTrace), ex);
      }
    }

    Thread _thread;
    protected override void OnStart(string[] args)
    {
      _thread = new Thread(new ThreadStart(Start));
      _thread.Start();
      base.OnStart(args);
    }

    public void Start()
    {
      _logger.Info("{0}: Starting", ServiceName);
      _queueProcessor.Start();
      _logger.Info("{0}: Started", ServiceName);
    }


    protected override void OnStop()
    {
      _logger.Info("{0}: Stopping", ServiceName);
      _queueProcessor.Stop();
      while (!_queueProcessor.IsStopped)
      {
        _logger.Info("{0}: not yet stopped; waiting", _queueProcessor.Name);
        Thread.Sleep(500);
      }
      base.OnStop();
      _logger.Info("{0}: Stopped", ServiceName);
    }

  }
}
