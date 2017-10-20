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
        ///������˾Code
        /// <summary>
        public string cpCode { get; set; }
        /// <summary>
        ///��������Ϣ
        /// <summary>
        public Models.Request.TMS_WAYBILL_GET_TEMP.UserInfoDto sender { get; set; }
        /// <summary>
        ///�����浥��Ϣ����������Ϊ10
        /// <summary>
        public List<TradeOrderInfoDto> tradeOrderInfoDtos { get; set; }
        /// <summary>
        ///��code�� �ֿ�WMSϵͳ�Խ������ҵ�����������벻Ҫʹ��
        /// <summary>
        public string storeCode { get; set; }
        /// <summary>
        ///������Դcode�� �ֿ�WMSϵͳ�Խ������ҵ�����������벻Ҫʹ��
        /// <summary>
        public string resourceCode { get; set; }
        /// <summary>
        ///�Ƿ�ʹ���Ƿֱ�Ԥ�ּ� �ֿ�WMSϵͳ�Խ������ҵ�����������벻Ҫʹ��
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