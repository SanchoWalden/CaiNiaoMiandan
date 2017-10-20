using System.Collections.Generic;
using System.Xml.Serialization;
using System;
using Pac.Api.Common;
/*请求面单信息*/
namespace Models.Request.TMS_WAYBILL_GET
{
/// <summary>
///请求面单信息
/// <summary>
[XmlRoot("tradeOrderInfoDto")]
public class TradeOrderInfoDto : PacObject
{
/// <summary>
///如不需要特殊服务，该值为空  服务值
/// <summary>
public string logisticsServices{ get; set; }
/// <summary>
///请求ID
/// <summary>
public string objectId{ get; set; }
/// <summary>
///订单信息
/// <summary>
public OrderInfoDto orderInfo{ get; set; }
/// <summary>
///包裹信息
/// <summary>
public PackageInfoDto packageInfo{ get; set; }
/// <summary>
///收件人信息
/// <summary>
public UserInfoDto recipient{ get; set; }
/// <summary>
///模板URL
/// <summary>
public string templateUrl{ get; set; }
/// <summary>
///使用者ID
/// <summary>
public long? userId{ get; set; }

}
}