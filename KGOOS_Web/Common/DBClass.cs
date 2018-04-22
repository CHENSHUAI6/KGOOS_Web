using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Xml;

namespace KGOOS_Web.Common
{
    public class DBClass
    {
        //private static SqlConnection conn;
        //private static SqlCommand cmd;

        /// <summary>
        /// 获取数据库链接
        /// </summary>
        /// <returns></returns>
        public static SqlConnection getConnection()
        {
            SqlConnection conn = null;
            XmlDocument xmlDoc = new XmlDocument();
            XmlReaderSettings settings = new XmlReaderSettings();
            settings.IgnoreComments = true;//忽略文档里面的注释
            String file = AppDomain.CurrentDomain.BaseDirectory + "\\Common\\dbSetting.xml";
            XmlReader reader = XmlReader.Create(file, settings);
            xmlDoc.Load(reader);

            // 得到根节点dbconfig
            XmlNode xn = xmlDoc.SelectSingleNode("dbconfig");
            // 得到根节点的所有子节点
            XmlNodeList xnl = xn.ChildNodes;
            foreach (XmlNode xn1 in xnl)
            {
                // 将节点转换为元素，便于得到节点的属性值
                XmlElement xe = (XmlElement)xn1;

                if (xe.GetAttribute("default").ToString().ToUpper().Equals("TRUE"))
                {
                    conn = getConnection(xe.GetAttribute("database").ToString());
                    break;
                }
            }
            reader.Close();
            return conn;
        }
        /// <summary>
        /// 获取数据库链接
        /// </summary>
        /// <returns></returns>
        public static SqlConnection getConnection(String dbname)
        {
            String server = "";
            String uid = "";
            String pwd = "";
            String Timeout = "";

            XmlDocument xmlDoc = new XmlDocument();
            XmlReaderSettings settings = new XmlReaderSettings();
            settings.IgnoreComments = true;//忽略文档里面的注释

            String file = AppDomain.CurrentDomain.BaseDirectory + "\\Common\\dbSetting.xml";
            XmlReader reader = XmlReader.Create(file, settings);
            xmlDoc.Load(reader);

            // 得到根节点dbconfig
            XmlNode xn = xmlDoc.SelectSingleNode("dbconfig");
            // 得到根节点的所有子节点
            XmlNodeList xnl = xn.ChildNodes;
            foreach (XmlNode xn1 in xnl)
            {
                // 将节点转换为元素，便于得到节点的属性值
                XmlElement xe = (XmlElement)xn1;
                if (dbname.Equals(xe.GetAttribute("database").ToString()))
                {
                    // 得到db节点的所有子节点
                    XmlNodeList xnl0 = xe.ChildNodes;
                    server = xnl0.Item(0).InnerText;
                    uid = xnl0.Item(1).InnerText;
                    pwd = xnl0.Item(2).InnerText;
                    Timeout = xnl0.Item(3).InnerText;
                }
            }
            reader.Close();

            if (server.Equals("") || uid.Equals("") || pwd.Equals("")) throw new Exception("数据库没有正确配置");
            String connStr = "server=" + server + ";DataBase =" + dbname + ";uid=" + uid + ";pwd=" + pwd + ";Connection Timeout=" + Timeout;
            SqlConnection conn = new SqlConnection(connStr);

            if (conn == null)
            {
                conn.Open();
            }
            else if (conn.State == System.Data.ConnectionState.Closed)
            {
                conn.Open();
            }
            else if (conn.State == System.Data.ConnectionState.Broken)
            {
                conn.Close();
                conn.Open();
            }
            return conn;
        }
        /// <summary>
        /// 关闭数据库链接
        /// </summary>
        /// <returns></returns>
        public static void closeConn(SqlConnection conn)
        {
            if (conn != null)
            {
                if (conn.State == ConnectionState.Open)
                {
                    conn.Close();
                }
            }
        }
        /// <summary>
        /// 执单行增删改操作
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public static int execUpdate(string sql)
        {
            SqlConnection conn = getConnection();
            try
            {
                SqlCommand cmd = new SqlCommand(sql, conn);
                return cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                conn.Close();
            }
        }
        /// <summary>
        /// 执单行增删改操作,传链接
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public static int execUpdate(SqlConnection conn, string sql)
        {
            int i = -1;
            try
            {
                if (conn.State == ConnectionState.Closed) conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.CommandTimeout = 600000;
                i = cmd.ExecuteNonQuery();
                return i;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 执单行增删改操作,传链接，受事务控制
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public static int execUpdate(SqlConnection conn, SqlTransaction sqlTran, string sql)
        {
            int i = -1;
            try
            {
                if (conn.State == ConnectionState.Closed) conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Transaction = sqlTran;
                i = cmd.ExecuteNonQuery();
                return i;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 执行查询语句，得到表数据
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public static DataSet execQuery(string sql)
        {
            SqlConnection conn = getConnection();
            try
            {
                SqlDataAdapter da = new SqlDataAdapter(sql, conn);
                DataSet ds = new DataSet();
                da.Fill(ds);
                return ds;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                conn.Close();
            }
        }
        /// <summary>
        /// 执行查询语句，得到表数据,传链接
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public static DataSet execQuery(SqlConnection conn, string sql)
        {
            try
            {
                if (conn.State == ConnectionState.Closed) conn.Open();
                SqlDataAdapter da = new SqlDataAdapter(sql, conn);
                DataSet ds = new DataSet();
                da.Fill(ds);
                //return ds.Tables[0];
                return ds;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// 执行查询语句，得到表数据,传链接，传事务
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public static DataSet execQuery(SqlConnection conn, SqlTransaction sqlTran, string sql)
        {
            try
            {
                if (conn.State == ConnectionState.Closed) conn.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = sql;
                cmd.Connection = conn;
                cmd.Transaction = sqlTran;
                cmd.CommandTimeout = 600000;

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                da.Fill(ds);
                return ds;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// 获取表主键
        /// </summary>
        /// 参数：不传连接
        /// <returns>主键</returns>
        public static String getKeyId(String tableName, String fieldName)
        {
            SqlConnection conn = getConnection();
            SqlTransaction sqlTrans = conn.BeginTransaction();
            try
            {
                String keyid = getKeyId(conn, sqlTrans, tableName, fieldName);
                sqlTrans.Commit();
                return keyid;
            }
            catch (Exception ex)
            {
                sqlTrans.Rollback();
                throw ex;
            }
            finally
            {
                closeConn(conn);
            }
        }

        /// <summary>
        /// 获取表主键
        /// </summary>
        /// 参数：传连接
        /// <returns>主键</returns>
        public static String getKeyId(SqlConnection conn, SqlTransaction sqlTran, String tableName, String fieldName)
        {
            tableName = tableName.ToUpper();
            fieldName = fieldName.ToUpper();
            try
            {
                String prefix = "";
                String sql1 = "select * from SYS_KEY_ID_INFO where TABLE_NAME = '"
                            + tableName + "' and FIELD_NAME = '"
                            + fieldName + "'";
                DataTable dt = execQuery(conn, sqlTran, sql1).Tables[0];
                int rows = dt.Rows.Count;
                if (rows != 1)
                {
                    throw new Exception("SYS_KEY_ID_INFO表定义错误，可能是主键未定义(rows:" + rows.ToString() + ")");
                }

                if (dt.Rows[0]["PREFIX"] != null) prefix = dt.Rows[0]["PREFIX"].ToString().Trim();
                int seqidLength = int.Parse(dt.Rows[0]["SEQID_LENGTH"].ToString().Trim());
                String resetType = dt.Rows[0]["RESET_TYPE"].ToString().Trim();
                int nextValue = int.Parse(dt.Rows[0]["LAST_SEQID"].ToString().Trim());
                String lastModified = dt.Rows[0]["LAST_MODIFIED_DATETIME"].ToString().Trim();
                String dateString = "";
                //不重置序号
                String sql2 = "update SYS_KEY_ID_INFO SET LAST_MODIFIED_DATETIME = '"
                            + DateTime.Now.ToString("yyyyMMddHHmmss")
                            + "' , LAST_SEQID = LAST_SEQID + 1 where TABLE_NAME = '"
                            + tableName
                            + "' and FIELD_NAME = '"
                            + fieldName + "'";
                //重置序号
                String sql3 = "update SYS_KEY_ID_INFO SET LAST_MODIFIED_DATETIME = '"
                            + DateTime.Now.ToString("yyyyMMddHHmmss")
                            + "' , LAST_SEQID = 1 where TABLE_NAME = '"
                            + tableName
                            + "' and FIELD_NAME = '"
                            + fieldName + "'";
                if (resetType.ToLower().Equals("no_reset"))
                {
                    dateString = "";
                    sql1 = sql2;
                }
                if (resetType.ToLower().Equals("reset_per_year"))
                {
                    dateString = DateTime.Now.ToString("yyyyMMddHHmmss").Substring(0, 4);
                    if (lastModified != null && !lastModified.Equals("") && !dateString.Equals(lastModified.Substring(0, 4)))
                    {
                        nextValue = 1;
                        sql1 = sql3;
                    }
                    else
                    {
                        nextValue++;
                        sql1 = sql2;
                    }
                }
                if (resetType.ToLower().Equals("reset_per_month"))
                {
                    dateString = DateTime.Now.ToString("yyyyMMddHHmmss").Substring(0, 6); ;
                    if (lastModified != null && !lastModified.Equals("") && !dateString.Equals(lastModified.Substring(0, 6)))
                    {
                        nextValue = 1;
                        sql1 = sql3;
                    }
                    else
                    {
                        nextValue++;
                        sql1 = sql2;
                    }
                }
                if (resetType.ToLower().Equals("reset_per_day"))
                {
                    dateString = DateTime.Now.ToString("yyyyMMddHHmmss").Substring(0, 8); ;
                    if (lastModified != null && !lastModified.Equals("") && !dateString.Equals(lastModified.Substring(0, 8)))
                    {
                        nextValue = 1;
                        sql1 = sql3;
                    }
                    else
                    {
                        nextValue++;
                        sql1 = sql2;
                    }
                }
                execUpdate(conn, sqlTran, sql1);
                String seqidStr = nextValue.ToString();
                while (seqidStr.Length < seqidLength)
                {
                    seqidStr = '0' + seqidStr;
                }

                return prefix + dateString + seqidStr;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}