using System;
using System.Configuration;
using ChildFund.FunctionApp.Models;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Newtonsoft.Json;
namespace ChildFund.FunctionApp
{
    public static class InfimationHubEmail
    {
        [FunctionName("InfimationHubEmail")]
        public static void Run([QueueTrigger("mailservicequeue", Connection = "StorageConnectionString")]string myQueueItem, TraceWriter log)
        {
            MailModel mailModel = JsonConvert.DeserializeObject<MailModel>(myQueueItem);
            if (mailModel != null)
            {
                string emailSend = ConfigurationManager.AppSettings["MailSend"];
                string passSend = ConfigurationManager.AppSettings["MailPass"];
                try
                {
                    EmailProcess.SendMail(emailSend, passSend, mailModel.MailInbox, mailModel.Title, mailModel.Content);
                }
                catch (Exception ex)
                {
                    throw new Exception("failed");
                }
                
            }
        }
    }
}
