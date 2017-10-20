using System.Collections.Generic;
using System.Xml.Serialization;
using System;
using Pac.Api.Common;
/*订单信息*/
namespace Models.Request.TMS_WAYBILL_GET
{
/// <summary>
///订单信息
/// <summary>
[XmlRoot("orderInfo")]
public class OrderInfoDto : PacObject
{
/// <summary>
///订单渠道平台编码：淘宝(TB)、天猫(TM)、京东(JD)、当当(DD)、拍拍(PP)、易讯(YX)、ebay(EBAY)、QQ网购(QQ) 、亚马逊(AMAZON)、苏宁(SN)、国美(GM)、唯品会(WPH)、聚美(JM)、乐蜂(LF)、蘑菇街(MGJ) 、聚尚(JS)、拍鞋(PX)、银泰(YT)、1号店(YHD)、凡客(VANCL)、邮乐(YL)、优购(YG)、阿里 巴巴(1688)、其他(OTHERS)
/// <summary>
public string orderChannelsType{ get; set; }
/// <summary>
///订单号,数量限制100
/// <summary>
[XmlArrayItem("tradeOrder")]
public List<string> tradeOrderList{ get; set; }

}
}