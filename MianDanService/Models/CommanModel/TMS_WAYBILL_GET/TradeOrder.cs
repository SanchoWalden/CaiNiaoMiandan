using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

namespace Models.Request.TMS_WAYBILL_GET
{
    [XmlRoot("tradeOrder")]
    public class TradeOrder
    {
        public string Name { set; get; }
    }
}