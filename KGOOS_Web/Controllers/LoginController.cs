using KGOOS_Web.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KGOOS_Web.Controllers
{
    public class LoginController : Controller
    {
        //
        // GET: /Login/

        public ActionResult Login()
        {
            return View();
        }

        public ActionResult Register()
        {
            return View();
        }

        #region 登录判断
        /// <summary>
        /// 登录判断
        /// </summary>
        /// <param name="name"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult inputLogin(string name, string password)
        {
            try
            {
                string sql = "";
                string user_bp = "";
                password = BaseClass1.md5(password, 32);
                sql = "select t1.id_user, t1.id_name, phone_phone, email_user,tb_user,t1.user_bp from t_user as t1 " +
                    "where t1.id_name = '" + name + "' " +
                    //"where (t1.id_name = '" + name + "' " +
                    //"or t1.phone_phone = '" + name + "' " +
                    //"or t1.email_user = '" + name + "') " +
                    "and t1.pwd_user = '" + password + "'";
                DataSet ds = new DataSet();
                ds = DBClass.execQuery(sql);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    Session["id"] = ds.Tables[0].Rows[0][0];
                    Session["name"] = ds.Tables[0].Rows[0][1];
                    Session["phone"] = ds.Tables[0].Rows[0][2];
                    Session["email"] = ds.Tables[0].Rows[0][3];
                    Session["tb_User"] = ds.Tables[0].Rows[0][4];
                    user_bp = ds.Tables[0].Rows[0][5].ToString();
                    if (user_bp.Length > 0)
                    {
                        Session["user_bp"] = user_bp;
                    }
                    else
                    {
                        Session["user_bp"] = "0";
                    }
                    
                    getCouponNum(ds.Tables[0].Rows[0][0].ToString());             

                    return Json(new
                    {
                        code = 0,
                        msg = "登錄成功！"
                    });
                }

                getLoginData();
                return Json(new
                {
                    code = 1,
                    msg = "會員名/郵箱/電話 于 密碼不匹配，請重試！"
                });
            }
            catch (Exception e)
            {
                return Json(new
                {
                    code = -1,
                    msg = "系統繁忙，請稍後再試，" + e.Message
                });
            }
        }
        #endregion

        #region 注册
        /// <summary>
        /// 注册
        /// </summary>
        /// <param name="name"></param>
        /// <param name="password"></param>
        /// <param name="email"></param>
        /// <param name="id"></param>
        /// <param name="phone"></param>
        /// <param name="friendId"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult inputRegister(string name, string password, string email, string id, string phone, string friendId)
        {
            try
            {
                string sql = "";
                string id_user = "";
                string time = "";

                password = BaseClass1.md5(password, 32);
                sql = "select * from T_User as t1 where t1.id_name = '" + name + "' ";
                DataSet ds = new DataSet();
                ds = DBClass.execQuery(sql);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    return Json(new
                    {
                        code = 1,
                        msg = "會員名已存在，請重試。"
                    });
                }

                sql = "";
                sql = "select * from T_User as t1 where t1.email_user = '" + email + "' ";
                ds = DBClass.execQuery(sql);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    return Json(new
                    {
                        code = 2,
                        msg = "郵箱地址重複，是否直接登錄。"
                    });
                }

                sql = "";
                sql = "select * from T_User as t1 where t1.phone_phone = '" + phone + "' ";
                ds = DBClass.execQuery(sql);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    return Json(new
                    {
                        code = 3,
                        msg = "聯繫方式重複，是否直接登錄。"
                    });
                }

                id_user = BaseClass1.getLoginMaxId("T_User", "id_user", "10001");
                time = DateTime.Now.ToString("yyyyMMddHHmmss");
                
                sql = "insert into T_User (id_user, id_name, pwd_user, tb_User, email_user, phone_phone, " + 
                    "register_time, other_info) values ('{0}', '{1}', '{2}', '{3}', '{4}', " + 
                    "'{5}', '{6}', '{7}')";
                sql = string.Format(sql, id_user, name, password, id, email, phone, time, "网页注册");
                int n = 0;
                n = DBClass.execUpdate(sql);
                if (n > 0)
                {
                    Session["id"] = id_user;
                    Session["name"] = name;
                    Session["tb_User"] = id;
                    Session["phone"] = phone;
                    Session["email"] = email;
                    Session["tb_User"] = "0";
                    getCouponNum(id_user);
                    return Json(new
                    {
                        code = 0,
                        msg = "登錄成功！"
                    });
                }
                return Json(new
                {
                    code = 1,
                    msg = "會員名/郵箱/電話 于 密碼不匹配，請重試！"
                });
            }
            catch (Exception e)
            {
                return Json(new
                {
                    code = -1,
                    msg = "系統繁忙，請稍後再試，" + e.Message
                });
            }
        }

        #endregion

        public void getCouponNum(string user_id)
        {
            string num = "";
            string sql = "";
            DataSet ds = new DataSet();
            sql = "select count(*) from T_User_Coupon as t1 " +
                "where t1.start_time < Datename(year,GetDate())+Datename(month,GetDate())+Datename(day,GetDate()) " +
                "and t1.end_time > Datename(year,GetDate())+Datename(month,GetDate())+Datename(day,GetDate()) " +
                "and t1.user_id = '" + user_id + "' ";
            ds = DBClass.execQuery(sql);
            try
            {
                num = ds.Tables[0].Rows[0][0].ToString();
            }
            catch(Exception e)
            {
                num = "0";
            }
            Session["coupon"] = num;

            
        }

        /// <summary>
        /// 获取登录信息
        /// </summary>
        /// <param name="userid"></param>
        [HttpPost]
        public JsonResult getLoginData()
        {
            try
            {
                string sql = "";
                string id_user = "", user_bp = "";

                try
                {
                    id_user = Session["id"].ToString();
                }
                catch(Exception e1)
                {
                    return Json(new
                    {
                        code = 1,
                        msg = "登录信息过期，请重新登录"
                    });
                }

                sql = "select t1.id_user, t1.id_name, phone_phone, email_user,tb_user,t1.user_bp from t_user as t1 " +
                    "where t1.id_user = '" + id_user + "' ";
                    
                DataSet ds = new DataSet();
                ds = DBClass.execQuery(sql);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    Session["id"] = ds.Tables[0].Rows[0][0];
                    Session["name"] = ds.Tables[0].Rows[0][1];
                    Session["phone"] = ds.Tables[0].Rows[0][2];
                    Session["email"] = ds.Tables[0].Rows[0][3];
                    Session["tb_User"] = ds.Tables[0].Rows[0][4];
                    user_bp = ds.Tables[0].Rows[0][5].ToString();
                    if (user_bp.Length > 0)
                    {
                        Session["user_bp"] = user_bp;
                    }
                    else
                    {
                        Session["user_bp"] = "0";
                    }
                    getCouponNum(ds.Tables[0].Rows[0][0].ToString());

                    return Json(new
                    {
                        code = 0,
                        msg = ""
                    });
                }
                else
                {
                    return Json(new
                    {
                        code = 1,
                        msg = "登录信息过期，请重新登录"
                    });
                }
            }
            catch(Exception e)
            {
                return Json(new
                {
                    code = -1,
                    msg = "系统繁忙，请重新登录"
                });
            }
        }
    }
}
