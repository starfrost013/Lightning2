using NuCore.Utilities;
using System;
using System.Collections.Generic;
using System.Diagnostics; 
using System.Linq; 
using System.Text;
using System.Timers; 

namespace Lightning.Core.API
{
    /// <summary>
    /// ServiceControlManager
    /// 
    /// March 10, 2021 (modified April 29, 2021: RunningServices is dead, now uses its native children property)
    /// 
    /// Controls, manages, and updates services. Contains the main loop.
    /// </summary>
    public class ServiceControlManager : Instance
    {
        internal override string ClassName => "ServiceControlManager";
        internal override InstanceTags Attributes =>  InstanceTags.Instantiable | InstanceTags.ParentLocked | InstanceTags.ParentCanBeNull; // non-serialisable or archivable as it is automatically created

        /// <summary>
        /// A timer used to update each service. 
        /// </summary>
        private ServiceGlobalData SvcGlobalData { get; set; }

        public ServiceControlManager()
        {
            SvcGlobalData = new ServiceGlobalData(); 
        }

        public void InitStartupServices(ServiceStartupCommandCollection StartupServices)
        {
            Logging.Log("Initialising startup services...", ClassName);

            List<ServiceStartupCommand> SSCList = StartupServices.Commands;
            
            // Sort by the StartOrder. 
            SSCList = SSCList.OrderBy(SSCList => SSCList.StartOrder).ToList(); 

            foreach (ServiceStartupCommand SSC in StartupServices)
            {
                Logging.Log($"Initialising startup service with name {SSC.ServiceName}, startup priority {SSC.StartOrder}...", ClassName);
                ServiceStartResult SSR = StartService(SSC.ServiceName);

                if (!SSR.Successful) // June 30, 2021
                {

                    Logging.Log($"Service {SSC.ServiceName} failed to start, trying to recover...", ClassName);
                    HandleCrashedService(SSC.ServiceName);

                }
            } 
        }

        /// <summary>
        /// Initialises the main timer used for updating services.
        /// 
        /// April 9, 2021
        /// </summary>
        public void InitServiceUpdates()
        {

            
            Logging.Log("Entering main loop...", ClassName);

            Workspace WS = DataModel.GetWorkspace();

            GetInstanceResult GIR = WS.GetFirstChildOfType("GameSettings");

            if (!GIR.Successful || GIR.Instance == null)
            {
                ErrorManager.ThrowError(ClassName, "GameSettingsFailedToLoadException", $"Failed to load GameSettings or it was somehow unloaded: {GIR.FailureReason}");
                return; 
            }
            else
            {
                GameSettings GS = (GameSettings)GIR.Instance;

                string SettingName = "MaxFPS";

                
                GetGameSettingResult MaxFPS_Result = GS.GetSetting(SettingName);
                
                if (!MaxFPS_Result.Successful)
                {
                    ErrorManager.ThrowError(ClassName, "FailedToObtainCriticalGameSettingException", $"Failed to load a setting that is required for the game to start: {SettingName}.");
                    return; 
                }
                else
                {
                    GameSetting MaxFPS_Setting = MaxFPS_Result.Setting;

                    SvcGlobalData.ServiceUpdateTimer = new Stopwatch();

                    int MaxFPS = (int)MaxFPS_Setting.SettingValue;
                    
                    SvcGlobalData.ServiceUpdateTimer.Start();
                    UpdateGame(MaxFPS); // REFACTOR THIS

                }

            }

        }

        [Obsolete("Incorrect delta-time implementation")]
        private void UpdateGame(int MaxFPS)
        {
#if DEBUG
            SvcGlobalData.StopwatchMsAtLastFPSCheck = SvcGlobalData.ServiceUpdateTimer.ElapsedMilliseconds;
#endif
            // slightly less temporary code
            while (true)
            {
                long ElapsedMillisecondsSinceStart = SvcGlobalData.ServiceUpdateTimer.ElapsedMilliseconds;

                int TargetFrameTimeMS = 1000 / MaxFPS;

#if DEBUG
                // Check FPS
                if (ElapsedMillisecondsSinceStart - SvcGlobalData.StopwatchMsAtLastFPSCheck >= 1000)
                {
                    SvcGlobalData.FPS = SvcGlobalData.FrameCount;
                    SvcGlobalData.StopwatchMsAtLastFPSCheck = ElapsedMillisecondsSinceStart;
                    SvcGlobalData.FrameCount = 0;

                    Logging.Log($"FPS: {SvcGlobalData.FPS} Target FPS: {MaxFPS}", ClassName);
                }
#endif

                if (ElapsedMillisecondsSinceStart % TargetFrameTimeMS == 0)
                {
#if DEBUG
                    SvcGlobalData.FrameCount++;
#endif
                    UpdateServices();
                }
            }
        }

        /// <summary>
        /// Starts the service of type Type. The type must inherit from <see cref="Service"/> in the DataModel. change to classname?
        /// </summary>
        /// <param name="TypeOfService"></param>
        /// <returns>A <see cref="ServiceStartResult"/> containing the success code and - if it has failed - the failure reason; if it succeeds the Service object will be added to <see cref="RunningServices"/>.</returns>
        public ServiceStartResult StartService(string ClassName)
        {
            ServiceStartResult SSR = new ServiceStartResult();

            //todo: check for duplicate 
            try
            {
                Logging.Log($"Starting Service {ClassName}", ClassName);
                object ObjX = DataModel.CreateInstance(ClassName);

                Service Svc = (Service)ObjX;

                // Check if another instance of this service is already running.
                if (!StartService_CheckForDuplicateServiceRunning(ClassName))
                {
                    Children.Add(Svc, this);
                    Svc.RunningNow = true;
                    return Svc.OnStart();
                    
                }
                else
                {
                    SSR.FailureReason = $"Attempted to initiate {ClassName} when it is already running!";
                    return SSR;
                }

                
            }
            catch (ArgumentException err)
            {
                ServiceStartResult SvcSR = new ServiceStartResult();
#if DEBUG
                SvcSR.FailureReason = $"Attempted to instantiate an invalid service\n\n{err}";
#else
                SvcSR.FailureReason = "Attempted to instantiate an invalid service";
#endif

                return SvcSR; 
            }

        }

        // not bool?
        private bool StartService_CheckForDuplicateServiceRunning(string ClassName)
        {
            foreach (Service Svc in Children)
            {
                if (Svc.RunningNow
                    && Svc.ClassName == ClassName)
                {
                    return true;
                }
                
            }

            return false;
        }

        /// <summary>
        /// Get the service with name <paramref name="ServiceName"/>
        /// </summary>
        /// <param name="ServiceName">The name of the service you wish to acquire. It must be running - this is signified by a reference being within the <see cref="ServiceControlManager.RunningServices"/> list. </param>
        /// <returns>The service object, or null if it does not exist [TEMP]</returns>
        public Service GetService(string ServiceName)
        {
            foreach (Service Svc in Children)
            {
                if (Svc.ClassName == ServiceName)
                {
                    return Svc;
                }
            }

            return null; // TEMP 
        }

        /// <summary>
        /// Kills the service with class name <paramref name="ServiceName"/>.
        /// </summary>
        /// <param name="ServiceName">The class name of the service to kill. Must inherit from <see cref="Service"/>.</param>
        /// <returns>A <see cref="ServiceShutdownResult"/> object. Success is determined by the <see cref="ServiceShutdownResult.Successful"/> property. For further information, see the documentation for <see cref="ServiceShutdownResult"/>.</returns>
        private ServiceShutdownResult KillService(string ServiceName, bool ForceShutdown = false)
        {
            Logging.Log($"Attempting to kill the service with type {ServiceName}", ClassName);

            ServiceShutdownResult SSR = new ServiceShutdownResult(); 

            foreach (Service Svc in Children)
            {
                if (Svc.ClassName == ServiceName)
                {
                    switch (ForceShutdown)
                    {
                        case false:
                            ServiceShutdownResult SSR_Svc = Svc.OnShutdown();

                            if (!SSR_Svc.Successful)
                            {
                                SSR.FailureReason = $"Service shutdown failure: {SSR_Svc.FailureReason}";
                                return SSR;
                            }
                            else
                            {
                                Svc.RunningNow = false;
                                Children.Remove(Svc);
                                SSR.Successful = true;
                                return SSR;
                            }
                        case true:
                            Svc.RunningNow = false; 
                            Children.Remove(Svc);
                            SSR.Successful = true;
                            return SSR;
                    }   
                }
            }

            SSR.FailureReason = "Attempted to kill a service that does not exist or is not running!";
            return SSR; 
        }

        /// <summary>
        /// Shutdown all services. Used at the killing of the SCM itself during engine shutdown.
        /// </summary>
        /// <returns></returns>
        private ServiceShutdownResult KillAllServices(bool ForceKillAll = false)
        {
            Logging.Log("Shutting down all services...", ClassName);

            ServiceShutdownResult SSR = new ServiceShutdownResult();

            // April 10, 2021: Change to a for loop to prevent pesky "collection was modified" errors.
            for (int i = 0; i < Children.Count; i++)
            {
                // temporary hack
                Service Svc = (Service)Children[i];

                string XClassName = Svc.ClassName;

                ServiceShutdownResult SSR_KillSvc = KillService(XClassName);

                if (!ForceKillAll)
                {
                    if (!SSR_KillSvc.Successful)
                    {
                        SSR.FailureReason = $"SCM: Service shutdown failure: The service {XClassName} failed to shut down: {SSR.FailureReason}";
                    }
                    else
                    {

                        // no error occurred, continue.
                        continue;
                    }
                }
                else
                {
                    continue; 
                }
            }

            // No errors have occurred

            SSR.Successful = true;
            return SSR; 

        }

        /// <summary>
        /// Main loop: Polls all services
        /// </summary>
        private void UpdateServices()
        {
            foreach (Service Svc in Children)
            {
                Svc.Poll();
            }

        }

        /// <summary>
        /// Checks if the service <see cref="Service"/>
        /// </summary>
        /// <param name="ClassName"></param>
        /// <returns></returns>
        public bool IsServiceRunning(string ClassName)
        {
            foreach (Service Svc in Children)
            {
                if (Svc.ClassName == ClassName)
                {
                    return true;
                }
            }

            return false; 
        }

        /// <summary>
        /// Handles a service notification. 
        /// </summary>
        /// <param name="SvcNotification"></param>
        public void OnNotifyServiceEvent(ServiceNotification SvcNotification)
        {
            if (SvcNotification == null)
            {
                ErrorManager.ThrowError(ClassName, "AttemptedToPassInvalidServiceNotificationException");
                return;
            }
            else
            {

                // If the ServiceNotification exists, check it
                if (SvcNotification.ServiceClassName == null)
                {
                    ErrorManager.ThrowError(ClassName, "AttemptedToPassInvalidServiceNotificationException");
                    return; 
                }
                else
                {
                    if (SvcNotification.ServiceClassName.Length == 0)
                    {
                        ErrorManager.ThrowError(ClassName, "AttemptedToPassInvalidServiceNotificationException");
                        return; 
                    }
                }

                try
                {
                    Service Service = GetService(SvcNotification.ServiceClassName);

                    if (Service == null)
                    {
                        ErrorManager.ThrowError(ClassName, "AttemptedToHandleServiceNotificationAboutANonServiceException", $"Attempted to handle a ServiceNotification about {ClassName}, which is not a Service!");
                        return; 
                    }
                    else // If we have a valid notification, handle it 
                    {
                        switch (SvcNotification.NotificationType)
                        {
                            case ServiceNotificationType.Shutdown:
                                Logging.Log($"The {SvcNotification.ServiceClassName} has notified the Service Control Manager that it wishes to shut down. Killing it...", ClassName);
                                
                                if (SvcNotification.Reason != null)
                                {
                                    Logging.Log($"The service provided the following reason: {SvcNotification.Reason}", ClassName);
                                }

                                KillService(SvcNotification.ServiceClassName);
                                return;
                            case ServiceNotificationType.Shutdown_ShutDownEngine:
                                Logging.Log($"The {SvcNotification.ServiceClassName} has notified the Service Control Manager that it is shutting down as the user has requested an engine shutdown. Shutting down...", ClassName);

                                if (SvcNotification.Reason != null)
                                {
                                    Logging.Log($"The service provided the following reason: {SvcNotification.Reason}", ClassName);
                                }

                                ShutdownEngine();
                                return;
                            case ServiceNotificationType.Crash:
                                Logging.Log($"The {SvcNotification.ServiceClassName} has notified the Service Control Manager that it has crashed. Attempting to recover the service...", ClassName);

                                if (SvcNotification.Reason != null)
                                {
                                    Logging.Log($"The service provided the following reason: {SvcNotification.Reason}", ClassName);
                                }

                                HandleCrashedService(SvcNotification.ServiceClassName);
                                return;
                            case ServiceNotificationType.MessageSend:

                                if (SvcNotification.Data.Name != null)
                                {
                                    Logging.Log($"Service message received for the Service {SvcNotification.ServiceClassName} - name {SvcNotification.Data.Name}", ClassName);

                                    Service.OnDataSent(SvcNotification.Data);
                                }
                                else
                                {
                                    ErrorManager.ThrowError(ClassName, "ServiceMessageMustHaveNameException");
                                    return;
                                }


                                return; 
                        }
                    }
                }
                catch (ArgumentException err)
                {
#if DEBUG
                    ErrorManager.ThrowError(ClassName, "AttemptedToHandleServiceNotificationAboutANonServiceException", $"Attempted to handle a ServiceNotification about {ClassName}, which is not a Service!\n\n{err}");
#else
                    ErrorManager.ThrowError(ClassName, "AttemptedToHandleServiceNotificationAboutANonServiceException", $"Attempted to handle a ServiceNotification about {ClassName}, which is not a Service!");
#endif
                }
            }

            
        }

        /// <summary>
        /// Shuts down the engine. 
        /// </summary>
        internal void ShutdownEngine()
        {
            Logging.Log("The engine is shutting down...", ClassName);
            // Shuts down the engine by first killing all services, then clearing the DataModel
            // and finally exiting the process.
           
            DataModel.Shutdown();
            KillAllServices();

            Logging.Log("The engine has shut down. Exiting.");
            Environment.Exit(0);
        }

        private void HandleCrashedService(string ClassName)
        {
            // Get the service before we kill it
            Service Svc = GetService(ClassName);

            // Check the importance of the service.
            switch (Svc.Importance)
            {
                case ServiceImportance.Low:
                    Logging.Log($"Trying to reboot the service {ClassName}...", ClassName);
                    // Forcibly kill the service and reboot it
                    KillService(ClassName, true);
                    StartService(ClassName);
                    return; 
                case ServiceImportance.High:
                    ErrorManager.ThrowError(ClassName, "CriticalServiceFailureException", $"The critical service {ClassName} has failed - the engine cannot continue to run.");
                    return; 
            }
        }

        private void SendDataToService(string ClassName, ServiceMessage Data)
        {
            Service Svc = GetService(ClassName);

            Svc.OnDataSent(Data);
        }
    }
}
