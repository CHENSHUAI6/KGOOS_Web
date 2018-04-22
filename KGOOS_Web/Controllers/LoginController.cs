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
                password = BaseClass1.md5(password, 32);
                sql = "select t1.id_user, t1.id_name, phone_phone, email_user,pwd_user from t_user as t1 " +
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
                    Session["phone"] = phone;
                    Session["email"] = email;
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

    }
}
