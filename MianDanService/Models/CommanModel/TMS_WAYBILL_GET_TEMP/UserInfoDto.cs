using System.Collections.Generic;
using System.Xml.Serialization;
using System;
using Pac.Api.Common;
using Models.Request.TMS_WAYBILL_GET;
/*发货人信息*/
namespace Models.Request.TMS_WAYBILL_GET_TEMP
{
    /// <summary>
    ///发货人信息
    /// <summary>
    [XmlRoot("sender")]
    public class UserInfoDto : PacObject
    {
        /// <summary>
        ///发货地址
        /// <summary>
        public AddressDto address { get; set; }
        /// <summary>
        ///手机号码
        /// <summary>
        public string mobile { get; set; }
        /// <summary>
        ///姓名
        /// <summary>
        public string name { get; set; }
        /// <summary>
        ///固定电话
        /// <summary>
        public string phone { get; set; }

    }
}