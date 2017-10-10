using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Pac.Api;
using Pac.Api.Dataobject.Request.TMS_WAYBILL_GET;
using Pac.Api.Dataobject.Response.TMS_WAYBILL_GET;
using Pac.Api.Dataobject.Request.TMS_WAYBILL_SUBSCRIPTION_QUERY;
using Pac.Api.Dataobject.Response.TMS_WAYBILL_SUBSCRIPTION_QUERY;
using Pac.Api.Common;

namespace CaiNiaoMiandan
{
    public partial class TestMianDan : System.Web.UI.Page
    {
        /**
        * 测试环境请求url
        */
        //private static String dailyUrl = "http://link.cainiao.com/gateway/link.do";
        private static String dailyUrl = "http://106.11.61.125/gateway/link.do";
        //private  static String dailyUrl = "http://linkdaily.tbsandbox.com/gateway/link.do";

        /**
         * 测试环境appKey
         */
        private static String dailyAppKey = "769678";
        /**
         * 测试环境secretKey
         */
        private static String dailySecretKey = "Z40m28n6j83Ci6na4i9C735z9c60pk2h";
        /**
         * 测试环境token，token是商家在物流云平台授权ISV后生成的授权码，目前需要商家配置在ISV的软件中
         */
        private static String dailyToken = "TDFwc3B1bVhrc096bkNoYndFbS9KVDUzOGNsK3UrZlpOUkpsT3VMcWhiWDQxWWV6L24yUm1iZm1QTmJtak40dA==";
        IPacClient pacClient = new DefaultPacClient(dailyUrl, dailyAppKey, dailySecretKey);
        protected void Page_Load(object sender, EventArgs e)
        {
            PacResponse rep = SubscriptionQueryResponse();
            Response.Write(rep);
        }

        /// <summary>
        /// 获取地址
        /// </summary>
        /// <returns></returns>
        PacResponse SubscriptionQueryResponse()
        {
            var req = new TmsWaybillSubscriptionQueryRequest();
            req.cpCode = "STO";
            TmsWaybillSubscriptionQueryResponse rep = pacClient.Send(req, dailyToken);
            if (rep.Success == false)
            {
                LogHelper.WriteLog(typeof(TmsWaybillSubscriptionQueryResponse), rep.ErrorMsg);
            }
            return rep;
        }
        /// <summary>
        /// 获取面单号
        /// </summary>
        /// <returns></returns>
        PacResponse GetResponse()
        {
            IPacClient pacClient = new DefaultPacClient(dailyAppKey, dailySecretKey, dailyUrl);
            var req = new TmsWaybillGetRequest();

            TmsWaybillGetResponse rep = pacClient.Send(req, dailyToken);

            return rep;


        }

    }
    /// <summary>
    /// 暂时无用
    /// </summary>
    public class TmsWaybillGetRequestHandle : PacRequestHandle<TmsWaybillGetRequest, TmsWaybillGetResponse>
    {

        public override TmsWaybillGetResponse Execute(ReceiveSysParams receiveSysParams, TmsWaybillGetRequest request)
        {
            //接收到receiveSysParams 、request对象；业务处理
            var pacRequest = request;

            var res = new TmsWaybillGetResponse { };
            return res;
        }

        public override void Init()
        {
            //注册接口名称
            IfObjMappingBags.Add("TMS_WAYBILL_GET", this);
        }
    }
}