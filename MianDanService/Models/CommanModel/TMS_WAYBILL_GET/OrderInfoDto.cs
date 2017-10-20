using System.Collections.Generic;
using System.Xml.Serialization;
using System;
using Pac.Api.Common;
/*������Ϣ*/
namespace Models.Request.TMS_WAYBILL_GET
{
/// <summary>
///������Ϣ
/// <summary>
[XmlRoot("orderInfo")]
public class OrderInfoDto : PacObject
{
/// <summary>
///��������ƽ̨���룺�Ա�(TB)����è(TM)������(JD)������(DD)������(PP)����Ѷ(YX)��ebay(EBAY)��QQ����(QQ) ������ѷ(AMAZON)������(SN)������(GM)��ΨƷ��(WPH)������(JM)���ַ�(LF)��Ģ����(MGJ) ������(JS)����Ь(PX)����̩(YT)��1�ŵ�(YHD)������(VANCL)������(YL)���Ź�(YG)������ �Ͱ�(1688)������(OTHERS)
/// <summary>
public string orderChannelsType{ get; set; }
/// <summary>
///������,��������100
/// <summary>
[XmlArrayItem("tradeOrder")]
public List<string> tradeOrderList{ get; set; }

}
}