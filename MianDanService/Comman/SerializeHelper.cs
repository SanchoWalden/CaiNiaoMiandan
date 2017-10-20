using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
using System.Security;
using Pac.Api.Common;
using Pac.Api.Log;
using Pac.Api;

namespace CaiNiaoMiandan
{
    /// <summary>
    /// 泛型、实体的序列化与反序列化、转化XML与存储 XML
    /// </summary>
    public class SerializeHelp
    {
        /// <summary>
        /// 通过反射序列化
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string SerializeByRef(object obj)
        {
            #region 类别名

            var nodeName = obj.GetType().Name;
            try
            {
                var nodeAtt = obj.GetType().GetCustomAttributes(false);
                if (nodeAtt.Length > 0)
                {
                    nodeName = ((XmlRootAttribute)obj.GetType().GetCustomAttributes(false)[0]).ElementName;
                }
            }
            catch (Exception ex)
            {
                IPacLogger pacLogger = new DefaultPacLogger();
                pacLogger.Error("SerializeHelper-SerializeByRef:" + obj.GetType().Name + "\r\n" + ex.Message + "\r\n" +
                                ex.StackTrace);
            }

            #endregion

            var sb = new StringBuilder();
            sb.Append(string.Format("<{0}>\r\n", nodeName));
            var propertys = obj.GetType().GetProperties();
            foreach (var property in propertys)
            {
                #region 属性别名

                var fieldname = property.Name;
                try
                {
                    var nodeAtt = property.GetCustomAttributes(false);
                    if (nodeAtt.Length > 0)
                    {
                        fieldname = ((XmlElementAttribute)property.GetCustomAttributes(false)[0]).ElementName;
                    }
                }
                catch (Exception ex)
                {
                    IPacLogger pacLogger = new DefaultPacLogger();
                    pacLogger.Error("SerializeHelper-SerializeByRef:" + obj.GetType().Name + "\r\n" + ex.Message +
                                    "\r\n" + ex.StackTrace);
                }

                #endregion

                var fieldVal = property.GetValue(obj, null);

                if (fieldVal == null)
                {
                    continue;
                }
                if (fieldVal is string && string.IsNullOrEmpty(fieldVal + ""))
                {
                    continue;
                }
             

                if (fieldVal is IList)
                {
                    sb.Append(string.Format("<{0}>\r\n", fieldname));
                    if (fieldname == "tradeOrderList")
                    {
                        foreach (var item in fieldVal as IList)
                        {
                            sb.Append(string.Format("<{0}>{1}</{0}>", "tradeOrder", item));
                        }

                    }
                    else
                    {
                        foreach (var oneList in fieldVal as IList)
                        {
                            sb.Append(SerializeByRef(oneList));
                        }
                    }

                    sb.Append(string.Format("</{0}>\r\n", fieldname));
                }
                else if (fieldVal is Dictionary<string, string>)
                {
                    /*
                        <extendFields>
                            <dickey1>dicvalue1</dickey1>
                        </extendFields>
                     */

                    #region 产生以上格式代码

                    //sb.Append(string.Format("<{0}>\r\n", fieldname));
                    //foreach (var keyVal in fieldVal as Dictionary<string,string>)
                    //{
                    //    if (string.IsNullOrEmpty(keyVal.Key) || string.IsNullOrEmpty(keyVal.Value))
                    //    {
                    //        continue;
                    //    }
                    //    sb.Append(string.Format("<{0}>{1}</{0}>\r\n", keyVal.Key, keyVal.Value));
                    //}
                    //sb.Append(string.Format("</{0}>\r\n", fieldname));

                    #endregion

                    /*
                        <extendFields>dickey1:dicvalue1;dickey2:a;</extendFields>
                     */
                    var allStr = new StringBuilder();
                    foreach (var keyVal in fieldVal as Dictionary<string, string>)
                    {
                        if (string.IsNullOrEmpty(keyVal.Key) || string.IsNullOrEmpty(keyVal.Value))
                        {
                            continue;
                        }
                        allStr.Append(string.Format("{0}:{1};", SecurityElement.Escape(keyVal.Key),
                            SecurityElement.Escape(keyVal.Value)));
                    }
                    if (string.IsNullOrEmpty(allStr.ToString()))
                    {
                        continue;
                    }
                    sb.Append(string.Format("<{0}>{1}</{0}>\r\n", fieldname, allStr.ToString()));
                }
                else if (fieldVal is DateTime)
                {
                    sb.Append(string.Format("<{0}>{1}</{0}>\r\n", fieldname,
                        CommonHelper.ToStandardDataTimeStr(fieldVal)));
                }
                else if (IsBasicType(fieldVal))
                {
                    sb.Append(string.Format("<{0}>{1}</{0}>\r\n", fieldname, SecurityElement.Escape(fieldVal.ToString())));
                }
                else
                {
                    sb.Append(SerializeByRef(fieldVal));
                }
            }
            sb.Append(string.Format("</{0}>\r\n", nodeName));

            return sb.ToString();
        }

        public static object DesrializeByRef(string xml, Type type)
        {
            if (string.IsNullOrEmpty(xml))
            {
                return Activator.CreateInstance(type);
            }
            var xmlDocument = new XmlDocument();
            xmlDocument.LoadXml(xml);
            var rootElement = xmlDocument.DocumentElement;
            return DesrializeByRef(rootElement, type);
        }

        /// <summary>
        /// 通过反射序列化
        /// </summary>
        /// <param name="xml"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        private static object DesrializeByRef(XmlNode xmlNode, Type type)
        {
            var model = Activator.CreateInstance(type);

            #region 类别名

            var nodeName = model.GetType().Name;
            try
            {
                var nodeAtt = model.GetType().GetCustomAttributes(false);
                if (nodeAtt.Length > 0)
                {
                    nodeName = ((XmlRootAttribute)model.GetType().GetCustomAttributes(false)[0]).ElementName;
                }
            }
            catch (Exception ex)
            {
                IPacLogger pacLogger = new DefaultPacLogger();
                pacLogger.Error("SerializeHelper-SerializeByRef:" + model.GetType().Name + "\r\n" + ex.Message +
                                "\r\n" +
                                ex.StackTrace);
            }

            #endregion

            if (!nodeName.Equals(xmlNode.Name))
            {
                return null;
            }

            try
            {
                var propertys = model.GetType().GetProperties();
                foreach (var property in propertys)
                {
                    #region 属性别名
                    var fieldname = property.Name;
                    try
                    {
                        var nodeAtt = property.GetCustomAttributes(false);
                        if (nodeAtt.Length > 0)
                        {
                            fieldname = ((XmlElementAttribute)property.GetCustomAttributes(false)[0]).ElementName;
                        }

                    }
                    catch (Exception ex)
                    {
                        IPacLogger pacLogger = new DefaultPacLogger();
                        pacLogger.Error("SerializeHelper-DesrializeByRef:" + model.GetType().Name + "\r\n" + ex.Message + "\r\n" + ex.StackTrace);
                    }
                    #endregion
                    var elements = xmlNode.SelectNodes(fieldname);
                    if (elements == null || elements.Count <= 0)
                    {
                        continue;
                    }
                    var element = elements[0];

                    var fieldType = property.PropertyType;

                    var xmlVal = element.InnerText;

                    //对比类型

                    if (fieldType == typeof(DateTime?))
                    {
                        property.SetValue(model, Convert.ToDateTime(xmlVal), null);
                    }
                    else if (fieldType == typeof(int?))
                    {
                        property.SetValue(model, Convert.ToInt32(xmlVal), null);
                    }
                    else if (fieldType == typeof(double?))
                    {
                        property.SetValue(model, Convert.ToDouble(xmlVal), null);
                    }
                    else if (fieldType == typeof(long?))
                    {
                        property.SetValue(model, Convert.ToInt64(xmlVal), null);
                    }
                    else if (fieldType == typeof(bool?))
                    {
                        property.SetValue(model, Convert.ToBoolean(xmlVal), null);
                    }
                    else if (fieldType == typeof(string))
                    {
                        property.SetValue(model, Convert.ToString(xmlVal), null);
                    }
                    else if (fieldType == typeof(IDictionary<string, string>))
                    {
                        if (string.IsNullOrEmpty(xmlVal) || xmlVal.Split(';').Length == 0)
                        {
                            continue;
                        }
                        IDictionary<string, string> id = new Dictionary<string, string>();
                        var xmlValDic = xmlVal.Split(';');
                        for (int i = 0; i < xmlValDic.Length; i++)
                        {
                            if (string.IsNullOrEmpty(xmlValDic[i]) || xmlValDic[i].Split(':').Length != 2)
                            {
                                continue;
                            }
                            id.Add(xmlValDic[i].Split(':')[0], xmlValDic[i].Split(':')[1]);
                        }
                        property.SetValue(model, id, null);
                    }
                    else if (fieldType.IsGenericType) //List
                    {
                        var sonObj = Activator.CreateInstance(fieldType) as IList;
                        if (sonObj == null) continue;
                        var sonModelType = sonObj.GetType().GetProperty("Item").PropertyType;
                        var sonNodeName = sonModelType.Name;
                        try
                        {
                            var nodeAtt = sonModelType.GetCustomAttributes(false);
                            if (nodeAtt.Length > 0)
                            {
                                sonNodeName = ((XmlRootAttribute)sonModelType.GetCustomAttributes(false)[0]).ElementName;
                            }
                        }
                        catch (Exception ex)
                        {
                            IPacLogger pacLogger = new DefaultPacLogger();
                            pacLogger.Error("SerializeHelper-SerializeByRef:" + sonModelType.Name + "\r\n" + ex.Message + "\r\n" +
                                            ex.StackTrace);
                        }
                        var listNodes = element.SelectNodes(sonNodeName);

                        for (int i = 0; i < listNodes.Count; ++i)
                        {
                            sonObj.Add(DesrializeByRef(listNodes.Item(i), sonModelType));
                        }
                        property.SetValue(model, sonObj, null);
                    }
                    else
                    {
                        property.SetValue(model, DesrializeByRef(element, fieldType), null);
                    }
                }
            }
            catch (Exception ex)
            {
                IPacLogger pacLogger = new DefaultPacLogger();
                pacLogger.Error("SerializeHelper-DesrializeByRef:" + xmlNode.Name + "\r\n" + ex.Message + "\r\n" +
                                ex.StackTrace);
            }


            return model;
        }

        /// <summary>
        /// 序列化成xml字符串。用法：List《MyObject》 lMy = new List《MyObject》();  lMy.Add(ceshi);  lMy.Add(ceshi2);  string s = Serialize(lMy);
        /// </summary>
        /// <param name="obj"></param>
        /// <returns>序列化后的字符串</returns>
        public static string SerializeCustom(object obj)
        {
            //Assembly assembly = typeof()

            XmlSerializer xs = CreateOverrider(obj.GetType());
            using (MemoryStream ms = new MemoryStream())
            {
                System.Xml.XmlTextWriter xtw = new System.Xml.XmlTextWriter(ms, System.Text.Encoding.UTF8);
                xtw.Formatting = System.Xml.Formatting.Indented;
                xs.Serialize(xtw, obj);
                ms.Seek(0, SeekOrigin.Begin);
                using (StreamReader sr = new StreamReader(ms))
                {
                    string str = sr.ReadToEnd();
                    xtw.Close();
                    ms.Close();
                    return str;
                }
            }
        }

        public static XmlSerializer CreateOverrider(Type theType)
        {
            var xmlAttOr = new XmlAttributeOverrides();

            var xmlAtt = new XmlAttributes();
            xmlAtt.XmlIgnore = true;
            xmlAttOr.Add(theType, "orderCode", xmlAtt);


            var serilizer = new XmlSerializer(theType, xmlAttOr);
            return serilizer;
        }

        /// <summary>
        /// 序列化成xml字符串。用法：List《MyObject》 lMy = new List《MyObject》();  lMy.Add(ceshi);  lMy.Add(ceshi2);  string s = Serialize(lMy);
        /// </summary>
        /// <param name="obj"></param>
        /// <returns>序列化后的字符串</returns>
        public static string Serialize(object obj)
        {
            XmlSerializer xs = new XmlSerializer(obj.GetType());
            using (MemoryStream ms = new MemoryStream())
            {
                System.Xml.XmlTextWriter xtw = new System.Xml.XmlTextWriter(ms, System.Text.Encoding.UTF8);
                xtw.Formatting = System.Xml.Formatting.Indented;
                xs.Serialize(xtw, obj);
                ms.Seek(0, SeekOrigin.Begin);
                using (StreamReader sr = new StreamReader(ms))
                {
                    string str = sr.ReadToEnd();
                    xtw.Close();
                    ms.Close();
                    return str;
                }
            }
        }

        /// <summary>
        /// 序列化成xml字符串。用法：List《MyObject》 lMy = new List《MyObject》();  lMy.Add(ceshi);  lMy.Add(ceshi2);  string s = Serialize(lMy);
        /// </summary>
        /// <param name="obj"></param>
        /// <returns>序列化后的字符串</returns>
        public static string Serialize(object obj, Type type)
        {
            XmlSerializer xs = new XmlSerializer(type);
            using (MemoryStream ms = new MemoryStream())
            {
                System.Xml.XmlTextWriter xtw = new System.Xml.XmlTextWriter(ms, System.Text.Encoding.UTF8);
                xtw.Formatting = System.Xml.Formatting.Indented;
                xs.Serialize(xtw, obj);
                ms.Seek(0, SeekOrigin.Begin);
                using (StreamReader sr = new StreamReader(ms))
                {
                    string str = sr.ReadToEnd();
                    xtw.Close();
                    ms.Close();
                    return str;
                }
            }
        }


        /// <summary>
        /// 反序列化方法。用法：List<MyObject> list = Desrialize(xmlStr, typeof(List<MyObject>)) as List<MyObject>;
        /// </summary>
        /// <param name="xml">xml字符串</param>
        /// <param name="type">反序列化对象的类型</param>
        /// <returns>反序列化后的对象</returns>
        public static object Desrialize(string xml, Type type)
        {
            object obj = null;
            XmlSerializer xs = new XmlSerializer(type);
            TextReader tr;
            if (!File.Exists(xml))
            {
                tr = new StringReader(xml);
            }
            else
            {
                tr = new StreamReader(xml);
            }
            using (tr)
            {
                obj = xs.Deserialize(tr);
            }
            return obj;
        }

        private static bool IsBasicType(object obj)
        {
            return obj.GetType().IsPrimitive
                   || obj is string
                   || obj is DateTime;
        }
    }
}