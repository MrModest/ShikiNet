using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using Newtonsoft.Json;

namespace ShikiNet.Utils.Extensions
{
    public static class LoggerExtensions
    {
        public static void InfoStartRequest(this NLog.Logger logger, HttpRequestMessage requestMessage)
        {
            logger.Info($"Start request | {requestMessage.Method.Method}: {requestMessage.RequestUri.OriginalString}");
        }

        public static void InfoDoneRequest(this NLog.Logger logger, HttpRequestMessage requestMessage)
        {
            logger.Info($"Done request | {requestMessage.Method.Method}: {requestMessage.RequestUri.OriginalString}");
        }


        public static void WarnNotOkResponse(this NLog.Logger logger, HttpRequestMessage requestMessage, HttpResponseMessage response)
        {
            logger.Warn($"Request | url: [{requestMessage.RequestUri.OriginalString}] | code: [{response.StatusCode}] | message: [{response.ReasonPhrase}]");
        }

        public static void WarnDeserializationFail(this NLog.Logger logger, JsonSerializationException ex, string response, string method = "<unknown>", string url = "<unknown>", string args = null)
        {
            logger.Warn(ex, $"{method} | url: [{url}] | args: [{args}] | response: [{response}] | exMessage: [{ex.Message}]");
        }


        public static void InfoExecutionStart(this NLog.Logger logger, string description)
        {
            logger.Info(description + " | Start");
        }

        //ToDo: Find right naming
        public static void InfoOrWarnExecutionStatus(this NLog.Logger logger, string description, bool isSuccess)
        {
            if (isSuccess)
            {
                logger.Info(description + " | Success");
            }
            else
            {
                logger.Warn(description + " | Fail");
            }
        }
    }
}
