using System.Collections.Generic;
using System.Xml.Serialization;
using System;
using Pac.Api.Common;
using Models.Request.TMS_WAYBILL_GET;
/*��������Ϣ*/
namespace Models.Request.TMS_WAYBILL_GET_TEMP
{
    /// <summary>
    ///��������Ϣ
    /// <summary>
    [XmlRoot("sender")]
    public class UserInfoDto : PacObject
    {
        /// <summary>
        ///������ַ
        /// <summary>
        public AddressDto address { get; set; }
        /// <summary>
        ///�ֻ�����
        /// <summary>
        public string mobile { get; set; }
        /// <summary>
        ///����
        /// <summary>
        public string name { get; set; }
        /// <summary>
        ///�̶��绰
        /// <summary>
        public string phone { get; set; }

    }
}