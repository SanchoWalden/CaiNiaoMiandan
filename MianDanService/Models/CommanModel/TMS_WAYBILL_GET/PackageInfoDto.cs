using System.Collections.Generic;
using System.Xml.Serialization;
using System;
using Pac.Api.Common;
/*包裹信息*/
namespace Models.Request.TMS_WAYBILL_GET
{
/// <summary>
///包裹信息
/// <summary>
[XmlRoot("packageInfo")]
public class PackageInfoDto : PacObject
{
/// <summary>
///包裹id,拆合单使用，使用方式 注：该字段必须大小写不可重复，例如：123-A,123-a 不可当做不同ID，否则存在一定可能取号失败
/// <summary>
public string id{ get; set; }
/// <summary>
///商品信息,数量限制为100
/// <summary>
public List<Item> items{ get; set; }
/// <summary>
///体积, 单位 ml
/// <summary>
public long? volume{ get; set; }
/// <summary>
///重量,单位 g
/// <summary>
public long? weight{ get; set; }

}
}