using KGOOS_Web.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KGOOS_Web.Controllers
{
    public class OtherController : Controller
    {
        //
        // GET: /Other/

        public ActionResult Coupon()
        {
            return View();
        }

        public ActionResult MyData()
        {
            return View();
        }

        [HttpPost]
        public string getCoupon()
        {
            string _html = "";
            string sql = "";
            DataSet ds = new DataSet();
            string userId, userIdtbId;
            try
            {
                try
                {
                    userId = Session["id"].ToString();
                }
                catch (Exception e0)
                {
                    userId = "test";
                }

                sql = "select * from T_User_Coupon as t1 " +
                    "left join T_Coupon as t2 on t2.coupon_id = t1.coupon_id " +
                    "where t2.coupon_type = 'Y' " +
                    "and (t1.coupon_state = 'Y' or t1.coupon_state is null) " +
                    "and t1.start_time < Datename(year,GetDate())+Datename(month,GetDate())+Datename(day,GetDate()) " +
                    "and t1.end_time > Datename(year,GetDate())+Datename(month,GetDate())+Datename(day,GetDate()) " +
                    "and t1.user_id = '" + userId + "' " +
                    "order by t1.end_time, t2.coupon_num desc";

                ds = DBClass.execQuery(sql);

                DataTable dt = ds.Tables[0];
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        _html += "<tr> " +
                                    "<td>" + (i + 1).ToString() + "</td> " +
                                    "<td>" + dt.Rows[i]["coupon_name"].ToString() + "</td> " +
                                    "<td>" + dt.Rows[i]["coupon_num"].ToString() + "</td> " +
                                    "<td>" + dt.Rows[i]["start_time"].ToString() + "</td> " +
                                    "<td>" + dt.Rows[i]["end_time"].ToString() + "</td> " +
                                    "<td>" + dt.Rows[i]["coupon_note"].ToString() + "</td> " +
                                 "</tr> ";
                    }
                }
                return _html;
            }
            catch(Exception e)
            {
                _html = "系統錯誤：" + e.Message;
                return _html;
            }
            
        }


    }
}
