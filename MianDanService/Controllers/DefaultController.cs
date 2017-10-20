using Pac.Api;
using Pac.Api.Common;
using Models.Request.TMS_WAYBILL_GET;
using Pac.Api.Dataobject.Request.TMS_WAYBILL_SUBSCRIPTION_QUERY;
using Pac.Api.Dataobject.Response.TMS_WAYBILL_GET;
using Pac.Api.Dataobject.Response.TMS_WAYBILL_SUBSCRIPTION_QUERY;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CaiNiaoMiandan;
using System.Collections;
using PacClient;
using Models.Request.TMS_WAYBILL_GET_TEMP;

namespace MianDanService.Controllers
{
    public class DefaultController : Controller
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
        private static String dailyToken = "R1ZRODUzVG9jZDVSV3VYUkJ5UURkMXlvd0IrSzBJM0ZQN1Zid042emVuMmZuZ2FhUHliWjM1Sk5ISVJvSHd3cQ==";
        IPacClient pacClient = new PacClient.DefaultPacClient(dailyUrl, dailyAppKey, dailySecretKey);
        // GET: Default
        public ActionResult Index()
        {


            //PacResponse rep = SubscriptionQueryResponse();

            return View();
        }

        public ActionResult Print()
        {
            return View();
        }

        /// <summary>
        /// 获取地址
        /// </summary>
        /// <returns></returns>
        PacResponse SubscriptionQueryResponse()
        {
            var req = new TmsWaybillSubscriptionQueryRequest();
            req.cpCode = "ZTO";
            TmsWaybillSubscriptionQueryResponse rep = pacClient.Send(req, dailyToken);

            if (rep.Success == false)
            {
                LogHelper.WriteLog(typeof(TmsWaybillSubscriptionQueryResponse), rep.ErrorMsg);
            }
            else
            {
                GetResponse(rep);
            }
            return rep;
        }
        /// <summary>
        /// 获取面单号
        /// </summary>
        /// <returns></returns>
        PacResponse GetResponse(TmsWaybillSubscriptionQueryResponse repSub)
        {
            var req = new TmsWaybillGetRequest();

            req.cpCode = "ZTO";
            var userInfo = new Models.Request.TMS_WAYBILL_GET_TEMP.UserInfoDto();

            userInfo.name = "王帅伟";
            userInfo.phone = "0371-68145658";
            userInfo.mobile = "18317169319";
            userInfo.address = new Models.Request.TMS_WAYBILL_GET.AddressDto
            {
                province = repSub.waybillApplySubscriptionCols[0].branchAccountCols[0].shippAddressCols[0].province,
                detail = repSub.waybillApplySubscriptionCols[0].branchAccountCols[0].shippAddressCols[0].detail,
                city = repSub.waybillApplySubscriptionCols[0].branchAccountCols[0].shippAddressCols[0].city,
                district = repSub.waybillApplySubscriptionCols[0].branchAccountCols[0].shippAddressCols[0].district,
                town = repSub.waybillApplySubscriptionCols[0].branchAccountCols[0].shippAddressCols[0].town
            };
            req.sender = userInfo;


            List<string> tradeOrders = new List<string> { "123456" };
            req.tradeOrderInfoDtos = new List<TradeOrderInfoDto>{ new TradeOrderInfoDto
            {
                objectId = new Random().Next(1, 1000).ToString(),
                orderInfo = new OrderInfoDto { orderChannelsType = "OTHERS", tradeOrderList=tradeOrders},
                packageInfo = new PackageInfoDto { id = "123-A", items = new List<Item> { new Item { count = 1, name = "衣服" } }, volume = null, weight = null },
                recipient = new Models.Request.TMS_WAYBILL_GET.UserInfoDto { address = new Models.Request.TMS_WAYBILL_GET.AddressDto { detail = "东风南路与金水东路绿地新都会c座", province = "河南" }, phone = "0371-68145658", name = "王帅伟", mobile = "18317169319" },
                templateUrl = "http://cloudprint.cainiao.com/template/standard/301/176",
                userId = null,
                logisticsServices = null

            }};

            req.dmsSorting = false;
            string jsonse = SerializeHelp.SerializeByRef(req);
            TmsWaybillGetResponse rep = pacClient.Send(req, dailyToken);




            return rep;


        }
    }
}