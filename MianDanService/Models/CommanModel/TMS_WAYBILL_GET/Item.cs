using System.Collections.Generic;
using System.Xml.Serialization;
using System;
using Pac.Api.Common;
/*��Ʒ��Ϣ*/
namespace Models.Request.TMS_WAYBILL_GET
{
/// <summary>
///��Ʒ��Ϣ
/// <summary>
[XmlRoot("item")]
public class Item : PacObject
{
/// <summary>
///����
/// <summary>
public int? count{ get; set; }
/// <summary>
///����
/// <summary>
public string name{ get; set; }

}
}