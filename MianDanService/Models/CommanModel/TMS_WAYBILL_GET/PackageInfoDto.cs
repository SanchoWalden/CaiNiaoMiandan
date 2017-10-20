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
[XmlRoot("packageInfo")]
public class PackageInfoDto : PacObject
{
/// <summary>
///����id,��ϵ�ʹ�ã�ʹ�÷�ʽ ע�����ֶα����Сд�����ظ������磺123-A,123-a ���ɵ�����ͬID���������һ������ȡ��ʧ��
/// <summary>
public string id{ get; set; }
/// <summary>
///��Ʒ��Ϣ,��������Ϊ100
/// <summary>
public List<Item> items{ get; set; }
/// <summary>
///���, ��λ ml
/// <summary>
public long? volume{ get; set; }
/// <summary>
///����,��λ g
/// <summary>
public long? weight{ get; set; }

}
}