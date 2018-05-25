using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace KGOOS_Web.Common
{
    public class BaseClass1
    {
        /// <summary>
        /// 注册获取ID
        /// </summary>
        /// <param name="Table">表名</param>
        /// <param name="Id_Name">序号ID字段名</param>
        /// <param name="Num">自然序号：001/0001/00001</param>
        /// <returns></returns>
        public static String getLoginMaxId(String Table, String Id_Name, String Num)
        {
            String sql = "";
            string KID = "";
            KID = "'K" + Num + "'";
            sql = "select " + Id_Name + " from " + Table + " where " + Id_Name + " >= " + KID + " order by " + Id_Name + " desc ";

            DataSet ds = DBClass.execQuery(sql);
            var count = ds.Tables[0].Rows.Count;
            if (count > 0)
            {
                String max_id = ds.Tables[0].Rows[0][0].ToString();
                max_id = max_id.Substring(1);
                var i = Int64.Parse(max_id);
                var ii = i + 1;
                string id = "K" + ii;
                return id;
            }
            else
            {
                return ("K" + Num);
            }
        }

        /// <summary>
        /// 获取最大ID
        /// </summary>
        /// <param name="Table">表名</param>
        /// <param name="Id_Name">序号ID字段名</param>
        /// <param name="Num">自然序号：001/0001/00001</param>
        /// <returns></returns>
        public static String getInsertMaxId(String Table, String Id_Name, String Num)
        {
            String sql = "";
            string yyyyMM = "";

            yyyyMM = DateTime.Now.ToString("yyyyMM");
            yyyyMM = yyyyMM + Num;
            sql = "select " + Id_Name + " from " + Table + " where " + Id_Name + " >= " + yyyyMM + " order by " + Id_Name + " desc ";

            DataSet ds = DBClass.execQuery(sql);
            var count = ds.Tables[0].Rows.Count;
            if (count > 0)
            {
                String max_id = ds.Tables[0].Rows[0][0].ToString();
                var i = Int64.Parse(max_id);
                var ii = i + 1;
                return ii.ToString();
            }
            else
            {
                return yyyyMM;
            }
        }

        /// <summary>
        /// 加密用户密码
        /// </summary>
        /// <param name="password">密码</param>
        /// <param name="codeLength">加密位数</param>
        /// <returns>加密密码</returns>
        public static string md5(string password, int codeLength)
        {
            if (!string.IsNullOrEmpty(password))
            {
                // 16位MD5加密（取32位加密的9~25字符）  
                if (codeLength == 16)
                {
                    return System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(password, "MD5").ToLower().Substring(8, 16);
                }

                // 32位加密
                if (codeLength == 32)
                {
                    return System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(password, "MD5").ToLower();
                }
            }
            return string.Empty;
        }      

        /// <summary>
        /// 根据区间锁定公式
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="W"></param>
        /// <returns></returns>
        public static string getWeight(DataTable dt, double W)
        {
            string weight = "";
            double min_num = 0.0, max_num = 0.0;
            string char1 = "", char2 = "";
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    min_num = 0.0;
                    max_num = 0.0;
                    char1 = "";
                    char2 = "";

                    min_num = double.Parse(dt.Rows[i]["min_num"].ToString());
                    max_num = double.Parse(dt.Rows[i]["max_num"].ToString());
                    char1 = dt.Rows[i]["char1"].ToString();
                    char2 = dt.Rows[i]["char2"].ToString();

                    if (char1.Equals("＜"))
                    {
                        if (W > min_num)
                        {
                            if (char2.Equals("＜"))
                            {
                                if (W < max_num)
                                {
                                    weight = "";
                                    weight = dt.Rows[i]["formula"].ToString();

                                    return weight;
                                }
                            }
                            else if (char2.Equals("≤"))
                            {
                                if (W <= max_num)
                                {
                                    weight = "";
                                    weight = dt.Rows[i]["formula"].ToString();
                                    return weight;
                                }
                            }
                        }
                    }
                    else if (char1.Equals("≤"))
                    {
                        if (W >= min_num)
                        {
                            if (char2.Equals("＜"))
                            {
                                if (W < max_num)
                                {
                                    weight = "";
                                    weight = dt.Rows[i]["formula"].ToString();
                                    return weight;
                                }
                            }
                            else if (char2.Equals("≤"))
                            {
                                if (W <= max_num)
                                {
                                    weight = "";
                                    weight = dt.Rows[i]["formula"].ToString();
                                    return weight;
                                }
                            }
                        }
                    }

                }
            }
            return "";
        }
    }
}