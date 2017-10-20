using System.Collections.Generic;
using System.Xml.Serialization;
using System;
using Pac.Api.Common;
/*�����浥��Ϣ*/
namespace Models.Request.TMS_WAYBILL_GET
{
/// <summary>
///�����浥��Ϣ
/// <summary>
[XmlRoot("tradeOrderInfoDto")]
public class TradeOrderInfoDto : PacObject
{
/// <summary>
///�粻��Ҫ������񣬸�ֵΪ��  ����ֵ
/// <summary>
public string logisticsServices{ get; set; }
/// <summary>
///����ID
/// <summary>
public string objectId{ get; set; }
/// <summary>
///������Ϣ
/// <summary>
public OrderInfoDto orderInfo{ get; set; }
/// <summary>
///������Ϣ
/// <summary>
public PackageInfoDto packageInfo{ get; set; }
/// <summary>
///�ռ�����Ϣ
/// <summary>
public UserInfoDto recipient{ get; set; }
/// <summary>
///ģ��URL
/// <summary>
public string templateUrl{ get; set; }
/// <summary>
///ʹ����ID
/// <summary>
public long? userId{ get; set; }

}
}