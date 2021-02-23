using DatabaseWebService.Common;
using DatabaseWebService.DomainOTP.Abstract;
using DatabaseWebService.DomainOTP.Concrete;
using DatabaseWebService.DomainPDO.Abstract;
using DatabaseWebService.DomainPDO.Concrete;
using DatabaseWebService.ModelsOTP.Order;
using Ninject;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace UtilityServiceOTP
{
    public partial class UtilityServiceOTP : ServiceBase
    {
        private IKernel kernel;
        private Timer timerSchedular;
        private IUtilityServiceRepository utilityserviceRepo;                

        public UtilityServiceOTP()
        {
            try
            {
                InitializeComponent();

                kernel = new StandardKernel();

                kernel.Bind<ISystemMessageEventsRepository_OTP>().To<SystemMessageEventsRepository_OTP>();
                kernel.Bind<IUtilityServiceRepository>().To<UtilityServiceRepository>();
                kernel.Bind<IRecallRepository>().To<RecallRepository>();
                kernel.Bind<IClientOTPRepository>().To<ClientOTPRepository>();
                kernel.Bind<IMSSQLFunctionsRepository>().To<MSSQLFunctionsRepository>();
                //kernel.Bind<IOrderPDORepository>().To<OrderPDORepository>();
                //kernel.Bind<ISystemEmailMessageRepository_PDO>().To<SystemEmailMessageRepository_PDO>();
                kernel.Bind<IInquiryRepository>().To<InquiryRepository>();
                //kernel.Bind<IMSSQLPDOFunctionRepository>().To<MSSQLPDOFunctionRepository>();

                utilityserviceRepo = kernel.Get<IUtilityServiceRepository>();
                //orderRepoPDO = kernel.Get<IOrderPDORepository>();
            }
            catch (Exception ex)
            {
                DataTypesHelper.LogThis(ex.Message);
            }
        }

        protected override void OnStart(string[] args)
        {
            try
            {
                this.ScheduleService();
            }
            catch (Exception ex)
            {
                string error = "", errorToThrow = "";
                DataTypesHelper.getError(ex, ref error);
                errorToThrow = DataTypesHelper.ConcatenateErrorIN_DB("", error, DataTypesHelper.GetCurrentMethodName());
                DataTypesHelper.LogThis(errorToThrow);
            }
        }

        protected override void OnStop()
        {
        }

        private void ScheduleService()
        {
            try
            {
                timerSchedular = new Timer(new TimerCallback(TimerScheduleCallback));
                //Set the Default Time.
                DateTime scheduledTime = DateTime.MinValue;
                DataTypesHelper.LogThis("Schedule mode: "+ ConfigurationManager.AppSettings["ScheduleMode"].ToString());
                string scheduleMode = ConfigurationManager.AppSettings["ScheduleMode"].ToString();

                if (scheduleMode == "Dnevno")
                {
                    scheduledTime = DateTime.Parse(ConfigurationManager.AppSettings["ScheduledTime"]);
                    if (DateTime.Now > scheduledTime)
                    {
                        //If Scheduled Time is passed set Schedule for the next day.
                        scheduledTime = scheduledTime.AddDays(1);
                    }
                }
                else if (scheduleMode == "Interval")
                {
                    int intervalMinutes = Convert.ToInt32(ConfigurationManager.AppSettings["IntervalMin"]);
                    scheduledTime = DateTime.Now.AddMinutes(intervalMinutes);

                    if (DateTime.Now > scheduledTime)
                    {
                        //If Scheduled Time is passed set Schedule for the next Interval.
                        scheduledTime = scheduledTime.AddMinutes(intervalMinutes);
                    }
                }

                TimeSpan timeSpan = scheduledTime.Subtract(DateTime.Now);
                //Get the difference in Minutes between the Scheduled and Current Time.
                int dueTime = Convert.ToInt32(timeSpan.TotalMilliseconds);

                //Change the Timer's Due Time.
                timerSchedular.Change(dueTime, Timeout.Infinite);
            }
            catch (Exception ex)
            {
                string error = "", errorToThrow = "";
                DataTypesHelper.getError(ex, ref error);
                errorToThrow = DataTypesHelper.ConcatenateErrorIN_DB("", error, DataTypesHelper.GetCurrentMethodName());
                DataTypesHelper.LogThis(errorToThrow);
            }
        }

        private void TimerScheduleCallback(object e)
        {
            try
            {
                DataTypesHelper.LogThis("Start Utility service!");
                utilityserviceRepo = kernel.Get<IUtilityServiceRepository>();
                DataTypesHelper.LogThis("Init Kernel!");
                //orderRepoPDO = kernel.Get<IOrderPDORepository>();
                //utilityserviceRepo.CreateAndSendOrdersMultiple();
                // izdela PDF in pošlje izbranemu dobavitelju - 0250 naročilnica                
                //orderRepoPDO.CreatePDFAndSendPDOOrdersMultiple();

                DataTypesHelper.LogThis("Preverimo ali so kateri odpoklici že prevzeti");
                utilityserviceRepo.CheckForOrderTakeOver2();
                DataTypesHelper.LogThis("After take over!");
                utilityserviceRepo.CheckRecallsForCarriersSubmittingPrices();
                DataTypesHelper.LogThis("After CheckRecallsForCarriersSubmittingPrices!");
                utilityserviceRepo.CheckForRecallsWithNoSubmitedPrices();
                DataTypesHelper.LogThis("After CheckForRecallsWithNoSubmitedPrices!");
                DataTypesHelper.LogThis("END");
            }
            catch (Exception ex)
            {
                string error = "", errorToThrow = "";
                DataTypesHelper.getError(ex, ref error);
                errorToThrow = DataTypesHelper.ConcatenateErrorIN_DB("", error, DataTypesHelper.GetCurrentMethodName());
                DataTypesHelper.LogThis(errorToThrow);
            }

            this.ScheduleService();
        }
    }
}

