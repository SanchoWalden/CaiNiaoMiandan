using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
//using Jayrock.Json.Conversion;
using Pac.Api.Common;
using Pac.Api.Log;
using Pac.Api.Util;
//using Top.Api.Parser;
//using Top.Api.Request;
using System.Text;
using Pac.Api;
using CaiNiaoMiandan;

namespace PacClient
{
    /// <summary>
    /// 基于REST的PAC客户端。
    /// </summary>
    public class DefaultPacClient : IPacClient
    {
        public const string APP_KEY = "app_key";
        public const string FORMAT = "format";
        public const string METHOD = "method";
        public const string TIMESTAMP = "timestamp";
        public const string VERSION = "v";
        public const string SIGN = "sign";
        public const string PARTNER_ID = "partner_id";
        public const string SESSION = "session";
        public const string FORMAT_XML = "xml";
        public const string SDK_VERSION = "pac-sdk-net-20150604"; // SDK自动生成会替换成真实的版本

        private string serverUrl;
        private string appKey;
        private string appSecret;
        private string format = FORMAT_XML;

        private WebUtils webUtils;
        private IPacLogger _pacLogger;
        private bool disableParser = false; // 禁用响应结果解释
        private bool disableTrace = false; // 禁用日志调试功能
        private IDictionary<string, string> systemParameters; // 设置所有请求共享的系统级参数

        #region DefaultTopClient Constructors

        public DefaultPacClient(string serverUrl, string appKey, string appSecret)
        {
            this.appKey = appKey;
            this.appSecret = appSecret;
            this.serverUrl = serverUrl;
            this.webUtils = new WebUtils();
            this._pacLogger = new DefaultPacLogger();
        }

        public DefaultPacClient(string serverUrl, string appKey, string appSecret, string format)
            : this(serverUrl, appKey, appSecret)
        {
            this.format = format;
        }

        #endregion


        public void SetTopLogger(IPacLogger topLogger)
        {
            this._pacLogger = topLogger;
        }

        public void SetTimeout(int timeout)
        {
            this.webUtils.Timeout = timeout;
        }

        public void SetDisableParser(bool disableParser)
        {
            this.disableParser = disableParser;
        }

        public void SetDisableTrace(bool disableTrace)
        {
            this.disableTrace = disableTrace;
        }

        public void SetSystemParameters(IDictionary<string, string> systemParameters)
        {
            this.systemParameters = systemParameters;
        }

        #region ITopClient Members

        public T Send<T>(IPacRequest<T> request, string fromCode) where T : PacResponse
        {
            return Send(request, fromCode, string.Empty);
        }

        public T Send<T>(IPacRequest<T> request, string fromCode, string toCode) where T : PacResponse
        {
            return DoSend<T>(request, fromCode, toCode);
        }
       
        #endregion
        
        private T DoSend<T>(IPacRequest<T> request, string fromCode, string toCode) where T : PacResponse
        {
            // 传入参数验证
            try
            {
                Validate(request,fromCode,toCode);
            }
            catch (PacException e)
            {
                return CreateErrorResponse<T>(e.ErrorCode, e.ErrorMsg);
            }

            // 序列化报文
            var content = SerializeHelp.SerializeByRef(request);
            //// 反序列化报文
            //var model = SerializeHelper.DesrializeByRef(content, typeof(T)) as T;

            // 添加协议级请求参数
            var txtParams = new PacDictionary();
            txtParams.Add("msg_type", request.GetApi());
            txtParams.Add("logistics_interface", content);
            txtParams.Add("data_digest", PacUtils.SignSdkRequest(content, request, appSecret)); // 添加签名参数
            txtParams.Add("logistic_provider_id", fromCode);
            if (!string.IsNullOrEmpty(toCode))
            {
                txtParams.Add("to_code", toCode);
            }

            //发送请求
            string responseText = string.Empty;
            var rspModel = Activator.CreateInstance<T>();

            try
            {
                responseText = webUtils.DoPost(serverUrl, txtParams);
                rspModel.Success = true;
                rspModel = SerializeHelper.DesrializeByRef(responseText, typeof(T)) as T;

            }
            catch (Exception exception)
            {
                rspModel.Success = false;
                rspModel.ErrorCode = "S99";
                rspModel.ErrorMsg = "错误:"+exception.Message;
            }

            return rspModel;


            //try
            //{
            //    string body;
            //    //if (request is ITopUploadRequest<T>) // 是否需要上传文件
            //    //{
            //    //    ITopUploadRequest<T> uRequest = (ITopUploadRequest<T>)request;
            //    //    IDictionary<string, FileItem> fileParams = TopUtils.CleanupDictionary(uRequest.GetFileParameters());
            //    //    body = webUtils.DoPost(this.serverUrl, txtParams, fileParams);
            //    //}
            //    //else
            //    //{
            //        body = webUtils.DoPost(serverUrl, txtParams);
            //    //}

            //   // 解释响应结果
            //   var rsp = Activator.CreateInstance<T>();
            //    rsp.
            //    //if (disableParser)
            //    //{
            //    //    rsp = Activator.CreateInstance<T>();
            //    //    rsp.Body = body;
            //    //}
            //    //else
            //    //{
            //    //    if (FORMAT_XML.Equals(format))
            //    //    {
            //    //        ITopParser tp = new TopXmlParser();
            //    //        rsp = tp.Parse<T>(body);
            //    //    }
            //    //    else
            //    //    {
            //    //        ITopParser tp = new TopJsonParser();
            //    //        rsp = tp.Parse<T>(body);
            //    //    }
            //    //}

            //    //// 追踪错误的请求
            //    //if (!disableTrace && rsp.IsError)
            //    //{
            //    //    StringBuilder sb = new StringBuilder(reqUrl).Append(" response error!\r\n").Append(rsp.Body);
            //    //    _pacLogger.Warn(sb.ToString());
            //    //}
            //    return rsp;
            //}
            //catch (Exception e)
            //{
            //    if (!disableTrace)
            //    {
            //        //StringBuilder sb = new StringBuilder(reqUrl).Append(" request error!\r\n").Append(e.StackTrace);
            //        //_pacLogger.Error(sb.ToString());
            //    }
            //    throw e;
            //}
        }

        private void Validate<T>(IPacRequest<T> request, string fromCode, string toCode) where T : PacResponse
        {
            if (request==null)
            {
                throw new PacException("请求数据对象request不能为空");
            }
            if (string.IsNullOrEmpty(fromCode))
            {
                throw new PacException("fromCode为空");
            }
        }

       

        private T CreateErrorResponse<T>(string errCode, string errMsg) where T : PacResponse
        {
            var rsp = Activator.CreateInstance<T>();
            rsp.ErrorCode = errCode;
            rsp.ErrorMsg = errMsg;
            rsp.Success = false;
            return rsp;
        }

        
    }
}
