using System.Collections.Generic;
using System.Xml.Serialization;
using System;
using Pac.Api.Common;
/*������ַ*/
namespace Models.Request.TMS_WAYBILL_GET
{
/// <summary>
///������ַ
/// <summary>
[XmlRoot("address")]
public class AddressDto : PacObject
{
/// <summary>
///����
/// <summary>
public string city{ get; set; }
/// <summary>
///��ϸ��ַ
/// <summary>
public string detail{ get; set; }
/// <summary>
///��
/// <summary>
public string district{ get; set; }
/// <summary>
///ʡ
/// <summary>
public string province{ get; set; }
/// <summary>
///�ֵ�
/// <summary>
public string town{ get; set; }

}
}