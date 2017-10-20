using System.Collections.Generic;
using System.Xml.Serialization;
using System;
using Pac.Api.Common;
/*商品信息*/
namespace Models.Request.TMS_WAYBILL_GET
{
/// <summary>
///商品信息
/// <summary>
[XmlRoot("item")]
public class Item : PacObject
{
/// <summary>
///数量
/// <summary>
public int? count{ get; set; }
/// <summary>
///名称
/// <summary>
public string name{ get; set; }

}
}