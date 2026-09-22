using System;
using System.Collections.Generic;
using System.Data;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Text;

namespace Backend.Formas.Utilities
{
    public static class ConvertTypes
    {
        public static DataTable ToDataTable<T>(IEnumerable<T> items)
        {
            var table = CreateDataTableForPropertiesOfType<T>();
            var piT = typeof(T).GetProperties();
            foreach (var item in items)
            {
                var dr = table.NewRow();
                for (var property = 0; property < table.Columns.Count; property++)
                {
                    if (piT[property].CanRead)
                    {
                        var value = piT[property].GetValue(item, null);
                        if (piT[property].PropertyType.IsGenericType)
                        {
                            if (value == null)
                            {
                                dr[property] = DBNull.Value;
                            }
                            else
                            {
                                dr[property] = piT[property].GetValue(item, null);
                            }
                        }
                        else
                        {
                            dr[property] = piT[property].GetValue(item, null);
                        }
                    }
                }

                table.Rows.Add(dr);
            }

            return table;
        }

        public static DataTable CreateDataTableForPropertiesOfType<T>()
        {
            var dt = new DataTable();
            var piT = typeof(T).GetProperties();
            foreach (var pi in piT)
            {
                Type propertyType;
                if (pi.PropertyType.IsGenericType)
                {
                    propertyType = pi.PropertyType.GetGenericArguments()[0];
                }
                else
                {
                    propertyType = pi.PropertyType;
                }

                var dc = new DataColumn(pi.Name, propertyType);
                if (pi.CanRead)
                {
                    dt.Columns.Add(dc);
                }
            }

            return dt;
        }

        public static List<dynamic> ToDynamic(this DataTable dt)
        {
            var dynamicDt = new List<dynamic>();
            foreach (DataRow row in dt.Rows)
            {
                dynamic dyn = new ExpandoObject();
                dynamicDt.Add(dyn);
                foreach (DataColumn column in dt.Columns)
                {
                    var dic = (IDictionary<string, object>)dyn;
                    dic[column.ColumnName] = row[column];
                }
            }

            return dynamicDt;
        }

        /// <summary>
        ///     Converts to dynamic.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dt">The dt.</param>
        /// <returns></returns>
        public static List<T> ToDynamic<T>(this DataTable dt)
        {
            var dynamicDt = new List<T>();
            foreach (DataRow row in dt.Rows)
            {
                dynamic dyn = new ExpandoObject();
                dynamicDt.Add(dyn);
                foreach (DataColumn column in dt.Columns)
                {
                    var dic = (IDictionary<string, object>)dyn;
                    dic[column.ColumnName] = row[column];
                }
            }

            return dynamicDt;
        }

        /// <summary>
        ///     Converts to stringbase64.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static string ToStringBase64(this string value)
        {
            if (value.IsBase64())
            {
                var data = Convert.FromBase64String(value);
                return Encoding.ASCII.GetString(data);
            }

            return value;
        }

        /// <summary>
        ///     Determines whether this instance is base64.
        /// </summary>
        /// <param name="base64String">The base64 string.</param>
        /// <returns>
        ///     <c>true</c> if the specified base64 string is base64; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsBase64(this string base64String)
        {
            if (string.IsNullOrEmpty(base64String) || base64String.Length % 4 != 0
                                                   || base64String.Contains(" ") || base64String.Contains("\t") ||
                                                   base64String.Contains("\r") || base64String.Contains("\n"))
            {
                return false;
            }

            try
            {
                Convert.FromBase64String(base64String);
                return true;
            }
            catch
            {
            }

            return false;
        }

        /// <summary>
        ///     Converts to guid.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static Guid? ToGuid(this string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return null;
            }

            try
            {
                return Guid.Parse(value);
            }
            catch
            {
                return null;
            }
        }

        public static string ConverDataDynamic(this List<dynamic> _data, string _key)
        {
            var daa = _data.FirstOrDefault();
            string _responseData = null;
            try
            {
                string dataSerialize = Newtonsoft.Json.JsonConvert.SerializeObject(daa);
                Dictionary<string, string> data = dataSerialize.Deserialize<Dictionary<string, string>>();
                _responseData = data.GetValueOrDefault(_key);
            }
            catch (Exception)
            {
                return "";
            }
            return _responseData;
        }

        public static string ConvertToBase64(this Stream stream)
        {
            var bytes = new byte[(int)stream.Length];

            stream.Seek(0, SeekOrigin.Begin);
            stream.Read(bytes, 0, (int)stream.Length);

            return Convert.ToBase64String(bytes);
        }

        public static DateTime FechaOperativa()
        {
            DateTime now = DateTime.Now;
            DateTime mesOperativo = new DateTime();
            if (now.Month == 1)
                mesOperativo = new DateTime(now.Year - 1, 12, 1);
            else
                mesOperativo = new DateTime(now.Year, now.Month - 1, 1);
            return mesOperativo;
                
        }
    }
}