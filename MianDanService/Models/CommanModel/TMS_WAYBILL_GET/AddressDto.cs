using System.Collections.Generic;
using System.Xml.Serialization;
using System;
using Pac.Api.Common;
/*发货地址*/
namespace Models.Request.TMS_WAYBILL_GET
{
/// <summary>
///发货地址
/// <summary>
[XmlRoot("address")]
public class AddressDto : PacObject
{
/// <summary>
///城市
/// <summary>
public string city{ get; set; }
/// <summary>
///详细地址
/// <summary>
public string detail{ get; set; }
/// <summary>
///区
/// <summary>
public string district{ get; set; }
/// <summary>
///省
/// <summary>
public string province{ get; set; }
/// <summary>
///街道
/// <summary>
public string town{ get; set; }

}
}