using System.Collections.Generic;
using System.Xml.Serialization;
using System;
using Pac.Api.Common;
using Pac.Api.Dataobject.Response.TMS_WAYBILL_GET;
/**/
namespace Models.Request.TMS_WAYBILL_GET
{
    /// <summary>
    ///null
    /// <summary>
    [XmlRoot("request")]
    public class TmsWaybillGetRequest : IPacRequest<TmsWaybillGetResponse>
    {
        /// <summary>
        ///物流公司Code
        /// <summary>
        public string cpCode { get; set; }
        /// <summary>
        ///发货人信息
        /// <summary>
        public Models.Request.TMS_WAYBILL_GET_TEMP.UserInfoDto sender { get; set; }
        /// <summary>
        ///请求面单信息，数量限制为10
        /// <summary>
        public List<TradeOrderInfoDto> tradeOrderInfoDtos { get; set; }
        /// <summary>
        ///仓code， 仓库WMS系统对接落地配业务，其它场景请不要使用
        /// <summary>
        public string storeCode { get; set; }
        /// <summary>
        ///配送资源code， 仓库WMS系统对接落地配业务，其它场景请不要使用
        /// <summary>
        public string resourceCode { get; set; }
        /// <summary>
        ///是否使用智分宝预分拣， 仓库WMS系统对接落地配业务，其它场景请不要使用
        /// <summary>
        public bool? dmsSorting { get; set; }

        public string GetApi()
        {
            return "TMS_WAYBILL_GET";
        }
        public string getDataObjectId()
        {
            return null;
        }

    }
}