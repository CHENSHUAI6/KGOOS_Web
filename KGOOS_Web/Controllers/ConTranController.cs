using KGOOS_Web.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KGOOS_Web.Controllers
{
    public class ConTranController : Controller
    {
        string tbId, userId;

        // GET: /ConTran/

        public ActionResult MySelf()
        {
            return View();
        }
        public ActionResult InputGoods()
        {
            return View();
        }
        public ActionResult ConTran(string weightIdList)
        {
            ViewBag.weightIdList = weightIdList;
            return View();
        }
        public ActionResult ConAdress(string weightIdList, string conCarrierId)
        {
            ViewBag.weightIdList = weightIdList;
            ViewBag.conCarrierId = conCarrierId;
            return View();
        }

        public ActionResult PayFeight()
        {
            return View();
        }

        public ActionResult Order()
        {
            return View();
        }

        public ActionResult editGoods(string ConID)
        {
            ViewBag.ConID = ConID;
            return View();
        }

        public ActionResult UpPay()
        {
            return View();
        }

        #region 获取用户基本信息
        /// <summary>
        /// 获取用户基本信息
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult getUserData()
        {
            string sql = "";
            DataSet ds = new DataSet();
            string userId, tbId;
            try
            {
                string id_name, integral, coupon, k_con;
                
                try
                {
                    tbId = Session["tb_User"].ToString();
                    userId = Session["id"].ToString();
                }
                catch (Exception e0)
                {
                    tbId = "test";
                    userId = "test";
                }
                sql = "select * from T_User as t1 where t1.id_user = '" + userId + "'";
                ds = DBClass.execQuery(sql);

                DataTable dt = ds.Tables[0];
                if (dt.Rows.Count > 0)
                {
                    //integral = dt.Rows[0]["id_name"].ToString() + " 分";
                    //coupon = dt.Rows[0]["id_name"].ToString() + " 張";
                    //k_con = dt.Rows[0]["id_name"].ToString();
                    integral = "研发中，请耐心等待";
                    coupon = "研发中，请耐心等待";
                    k_con = "研发中，请耐心等待";
                }
                else
                {
                    id_name = "";
                    integral = "";
                    coupon = "";
                    k_con = "";
                }

                return Json(new
                {
                    code = 0,
                    integral = integral,
                    coupon = coupon,
                    k_con = k_con,
                    msg = "成功！"
                });
            }
            catch(Exception e)
            {
                return Json(new
                {
                    code = -1,
                    msg = "系統繁忙，請稍後再試"
                });
            }
            
        }
        #endregion

        #region 貨物登記
        /// <summary>
        /// 貨物登記
        /// </summary>
        /// <param name="conId"></param>
        /// <param name="name"></param>
        /// <param name="kgoos_herf"></param>
        /// <param name="note"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult inputGoods(string conId, string name, string kgoos_herf, string note)
        {
            string sql = "";
            string id, tbId, userId;
            DataSet ds = new DataSet();

            try 
            {
                id = BaseClass1.getInsertMaxId("T_Weight", "Id", "000001");
                try
                {
                    tbId = Session["tb_User"].ToString();
                    userId = Session["id"].ToString();
                }
                catch(Exception e0)
                {
                    tbId = "test";
                    userId = "test";
                }

                sql = "select Web_UserId from T_Weight as t1 where t1.Weight_ConID = '" + conId + "'";
                ds = DBClass.execQuery(sql);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    if (ds.Tables[0].Rows[0][0].ToString().Length > 0)
                    {
                        return Json(new
                        {
                            code = 1,
                            msg = "該物品已登記，請勿重複登記，謝謝！"
                        });
                    }
                    else
                    {
                        sql = "update T_Weight set Web_Goods_Name = '{1}', Web_UserId = '{2}', " +
                        "Web_TBId = '{3}', Web_Href = '{4}', Web_Note = '{5}', Weight_Type = '{6}' " +
                        "where Weight_ConID = '{0}' ";
                        sql = string.Format(sql, conId, name, userId, tbId, kgoos_herf, note, "Y");
                    }
                    
                }
                else
                {
                    sql = "insert into T_Weight " +
                    "(Weight_ConID, Web_Goods_Name, Web_UserId, Web_TBId, Web_Href, Web_Note, Id) " +
                    "values ('{0}','{1}','{2}','{3}','{4}','{5}','{6}')";
                    sql = string.Format(sql, conId, name, userId, tbId, kgoos_herf, note, id);
                }
                int n = DBClass.execUpdate(sql);

                if (n > 0)
                {
                    return Json(new
                    {
                        code = 0,
                        msg = "操作成功！"
                    });
                }
                else
                {
                    return Json(new
                    {
                        code = -1,
                        msg = "系統繁忙，請稍後再試"
                    });
                }

                
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

        #region 貨物登記修改
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="conId"></param>
        /// <param name="name"></param>
        /// <param name="kgoos_herf"></param>
        /// <param name="note"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult updateGoods(string id, string conId, string name, string kgoos_herf, string note)
        {
            string sql = "";
            DataSet ds = new DataSet();

            try
            {
                sql = "update T_Weight set Web_Goods_Name = '{0}', Weight_ConID = '{1}', " +
                        " Web_Href = '{2}', Web_Note = '{3}' " +
                        "where Id = '{4}' ";
                sql = string.Format(sql, name, conId, kgoos_herf, note, id);
                
                int n = DBClass.execUpdate(sql);

                if (n > 0)
                {
                    return Json(new
                    {
                        code = 0,
                        msg = "操作成功！"
                    });
                }
                else
                {
                    return Json(new
                    {
                        code = -1,
                        msg = "系統繁忙，請稍後再試"
                    });
                }


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

        #region 获得未到件列表
        /// <summary>
        /// 获得未到件列表
        /// </summary>
        /// <returns></returns>
        public string getNoArrive()
        {
            string _html = "";
            string sql = "";

            try
            {
                userId = Session["id"].ToString();
            }
            catch (Exception e0)
            {
                userId = "test";
            }

            DataSet ds = new DataSet();

            try
            {
                sql = "select * from T_Weight as t1 " +
                    " where t1.Web_UserId = '" + userId + "' " + 
                    " and t1.Weight_Type is null";
                ds = DBClass.execQuery(sql);
                DataTable dt = ds.Tables[0];
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        _html += "<tr> " +
                                    "<td>" + (i + 1).ToString() + "</td> " +
                                    "<td>" + dt.Rows[i]["Weight_ConID"].ToString() + "</td> " +
                                    "<td>" + dt.Rows[i]["Web_Goods_Name"].ToString() + "</td> " +
                                    "<td>" + dt.Rows[i]["Web_Note"].ToString() + "</td> " +
                                    "<td><a class='c_red' href='#' onclick=openwin('修改登记物品信息','../ConTran/editGoods?ConID=" + dt.Rows[i]["Weight_ConID"].ToString() + "');>修改</a></td> " +
                                    "<td><a class='c_red' href='#' onclick=delNoArrive('" + dt.Rows[i]["Weight_ConID"].ToString() + "');>刪除</a></td> " +
                                 "</tr> ";
                    }
                }
                else 
                {
                    _html = "";
                }                           
            }
            catch(Exception e)
            {
                _html = e.Message;
            }

            return _html;
        }
        #endregion

        #region 获得已到件列表
        /// <summary>
        /// 获得已到件列表
        /// </summary>
        /// <returns></returns>
        public string getArrived(string region)
        {
            string _html = "";
            string sql = "";

            try
            {
                userId = Session["id"].ToString();
            }
            catch (Exception e0)
            {
                userId = "test";
            }

            DataSet ds = new DataSet();

            try
            {
                sql = "select * from T_Weight as t1 " +
                    " join T_Region as t2 on t2.Region_Id = t1.Weight_Region " +
                    " where t1.Web_UserId = '" + userId + "' " +
                    " and t1.Weight_Type = 'Y' ";

                if (region == "香港")
                {
                    sql += " and t1.Weight_Region = '2'";
                }
                else if (region == "台灣")
                {
                    sql += " and t1.Weight_Region = '1'";
                }
                else if (region == "Other")
                {
                    sql += " and t1.Weight_Region = '3'";
                }

                ds = DBClass.execQuery(sql);
                DataTable dt = ds.Tables[0];
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        string cs_Note = "";
                        string size = "";
                        string shortName = "";
                        cs_Note = dt.Rows[i]["Weight_note"].ToString();
                        size = dt.Rows[i]["Weight_Size"].ToString();
                        if (dt.Rows[i]["Web_Goods_Name"].ToString().Length > 6)
                        {
                            shortName = dt.Rows[i]["Web_Goods_Name"].ToString().Substring(0, 6) + "..."; 
                        }
                        else
                        {
                            shortName = dt.Rows[i]["Web_Goods_Name"].ToString();
                        }

                        _html += "<tr> " +
                                    "<td>" + (i + 1).ToString() + "</td> " +
                                    "<td><input type='checkbox' name='cb_Arrived' value='"+dt.Rows[i]["Id"].ToString()+"'></td> " +
                                    "<td>" + dt.Rows[i]["Region_Name"].ToString() + "</td> " +                                    
                                    "<td>" + dt.Rows[i]["Weight_ConID"].ToString() + "</td> " +

                                    "<td>" + "<a title='" + dt.Rows[i]["Web_Goods_Name"].ToString() + "'>" + shortName + "</td> " +
                                    "<td>" + dt.Rows[i]["Weight_Helf"].ToString() + "</td> " +
                                    "<td><a title='" + cs_Note + "'>" + size + "</a></td> " +
                                    "<td>是</td> " +
                                    "<td>" + dt.Rows[i]["Weight_Time"].ToString().Substring(0, 10) + "</td> " +
                                    "<td><a title='"+dt.Rows[i]["Web_Note"].ToString()+"'>...</a></td> " +
                                   // "<td><a class='c_red' href='#' onclick=openwin('修改登记物品信息','../ConTran/editGoods?ConID=" + dt.Rows[i]["Weight_ConID"].ToString() + "');>改</a></td> " +
                                   // "<td><a class='c_red' href='#' onclick=delNoArrive('" + dt.Rows[i]["Weight_ConID"].ToString() + "');>刪</a></td> " +
                                 "</tr> ";                                                                                                                                                          
                    }
                }
                else
                {
                    _html = "";
                }
            }
            catch (Exception e)
            {
                _html = e.Message;
            }

            return _html;
        }
        #endregion

        #region 根据快递单号获取货物信息
        [HttpPost]
        public JsonResult getGoodsById(string conId)
        {
            string sql = "";
            DataSet ds = new DataSet();

            try 
            {
                sql = "select * from t_weight as t1 where t1.Weight_ConID = '" + conId + "'";
                ds = DBClass.execQuery(sql);
                DataTable dt = ds.Tables[0];
                if (dt.Rows.Count > 0)
                {
                    string name = "";
                    string href = "";
                    string note = "";
                    string id = "";
                    name = dt.Rows[0]["Web_Goods_Name"].ToString();
                    href = dt.Rows[0]["Web_Href"].ToString();
                    note = dt.Rows[0]["Web_Note"].ToString();
                    id = dt.Rows[0]["Id"].ToString();
                    
                    return Json(new
                    {
                        code = 0,
                        id = id,
                        name = name,
                        href = href,
                        note = note,
                        msg = "成功！"
                    });
                }
                else
                {
                    return Json(new
                    {
                        code = -1,
                        msg = "系統繁忙，請稍後再試"
                    }); 
                }
                
            }
            catch(Exception e)
            {
                return Json(new
                {
                    code = -1,
                    msg = "系統繁忙，請稍後再試"
                });
            }           

        }
        #endregion

        #region 未到件删除
        /// <summary>
        /// 未到件删除
        /// </summary>
        /// <param name="conId"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult delNoArrive(string conId)
        {
            string sql = "";
            sql = "";

            try
            {
                userId = Session["id"].ToString();
            }
            catch (Exception e0)
            {
                userId = "test";
            }

            try
            {
                sql = "delete T_Weight where Weight_ConID = '" + conId + "'";
                int n = 0;
                n = DBClass.execUpdate(sql);

                if (n > 0)
                {
                    return Json(new
                    {
                        code = 0,
                        msg = "删除成功！"
                    });
                }
                else
                {
                    return Json(new
                    {
                        code = -1,
                        msg = "系統繁忙，請稍後再試"
                    });
                }
                
            }
            catch (Exception e)
            {
                return Json(new
                {
                    code = -1,
                    msg = "系統繁忙，請稍後再試"
                });
            }


        }
        #endregion

        #region 付费弹窗
        /// <summary>
        /// 付费弹窗
        /// </summary>
        /// <returns></returns>
        public string getPopups()
        {
            string html = "";
            html = 
                "<div style='padding: 50px; line-height: 22px; background-color: #393D49; color: #fff; font-weight: 300;'> " +

                    "<div style='text-align:center;'>您本次集運還需支付 " +
                        "<span style='color: red; font-weight: bold;'>XX</span> " +
                        "元。 " +
                    "</div>  " +
                    "<br><br> " +

                    "<div style='text-align:center;'>謝謝您的支持！！！您本次交易獲得 " +
                        "<span style='color: red; font-weight: bold;'>XX</span> " +
                        "積分！（積分可在下次集運時抵扣運費） " +
                    "</div>  " +  
                    "<br><br> " +

                    "<div style='text-align:center;'> " +
                        "您的集運訂單號為： " +
                        "<span style='color: red; font-weight: bold;'>XXXXXXXX </span> " +
                        "<span style='padding-left: 30px; color: pink;'>請將此編號複製粘貼到淘寶付款留言里，以證明閣下已付款！</span> " + 
                    "</div> " +
                    "<br><br> " +

                    "<div style='text-align:center;'> " +
                        "我們將核對您的訂單無誤后，會盡快幫您將貨物發出！請保持電話暢通耐心等待！ " +
                    "</div> " +
                    "<br><br> " +

                    "<div style='text-align:center;'> " +
                        "香港：貨物到港后，正常派送时效為3個工作日內，偏遠地區加1-2个工作日。 " +
                    "</div> " +
                    "<br><br> " +

                    "<div style='text-align:center;'> " +
                        "台灣：空運正常时效為3-5個工作日。 " +
                    "</div> " +
        
                "</div>";
            return html;
        }
        #endregion

        #region 獲取地址區域
        /// <summary>
        /// 獲取地址區域
        /// </summary>
        /// <returns></returns>
        public string getAdressRegion()
        {
            string _html = "";
            string sql = "";
            sql = "select t1.Adress_Region from T_Adress as t1 group by t1.Adress_Region";
            DataSet ds = new DataSet();
            ds = DBClass.execQuery(sql);
            DataTable dt = new DataTable();
            dt = ds.Tables[0];
            _html = "<select class=form-control id=region onchange=Adress_Region_Change()> " +
                        "<option>請選擇區域</option> ";
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++ )
                {
                    _html += "<option>" + dt.Rows[i][0].ToString() + "</option> ";
                }                  
            }
             _html += "</select>";
            return _html;
        }
        #endregion

        #region 獲取地址城市
        /// <summary>
        /// 獲取地址城市
        /// </summary>
        /// <returns></returns>
        public string getAdressCity(string adress_region)
        {
            string _html = "";
            string sql = "";
            sql = "select t1.Adress_City from T_Adress as t1 " +
                "where t1.Adress_Region = '" + adress_region + "' " +
                "group by t1.Adress_City";
            DataSet ds = new DataSet();
            ds = DBClass.execQuery(sql);
            DataTable dt = new DataTable();
            dt = ds.Tables[0];
            _html = "<select class=form-control id=city onchange=Adress_City_Change()> " +
                       "<option>請選擇城市</option> ";
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    _html += "<option>" + dt.Rows[i][0].ToString() + "</option> ";
                }
            }
            _html += "</select>";
            return _html;
        }
        #endregion

        #region 獲取地址其他
        /// <summary>
        /// 獲取地址其他
        /// </summary>
        /// <returns></returns>
        public string getAdressOther(string adress_region, string adress_city)
        {
            string _html = "";
            string sql = "";
            sql = "select t1.Adress_Other from T_Adress as t1 " +
                "where t1.Adress_Region = '" + adress_region + "' " +
                "and t1.Adress_City = '" + adress_city + "'" +
                "group by t1.Adress_Other";
            DataSet ds = new DataSet();
            ds = DBClass.execQuery(sql);
            DataTable dt = new DataTable();
            dt = ds.Tables[0];
            _html = "<select class=form-control id=other> " +
                       "<option>請選擇其他</option> ";
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    _html += "<option>" + dt.Rows[i][0].ToString() + "</option> ";
                }
            }
            _html += "</select>";
            return _html;
        }
        #endregion

        #region 新增收货地址
        [HttpPost]
        public JsonResult inputUserAdress(string adress_name, string adress_phone, string adress_tel,
            string adress_region, string adress_city, string adress_other, string adress_datail )
        {
            string sql = "";
            DataSet ds = new DataSet();
            string id = "";
            id = BaseClass1.getInsertMaxId("T_User_Adress", "id", "000001");
            try
            {
                try
                {
                    tbId = Session["tb_User"].ToString();
                    userId = Session["id"].ToString();
                }
                catch (Exception e0)
                {
                    tbId = "test";
                    userId = "test";
                }

                sql = "insert into T_User_Adress " +
                    "(id, user_id, adress_name, adress_phone, adress_tel, adress_region,  " +
                    "adress_city, adress_other, adress_datail, adress_note) " +
                    "values " +
                    "('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}')";

                sql = string.Format(sql, id, userId, adress_name, adress_phone, adress_tel, adress_region,
                    adress_city, adress_other, adress_datail, "网页添加地址");
                int n = 0;
                n = DBClass.execUpdate(sql);
                if (n > 0)
                {
                    return Json(new
                    {
                        code = 0,
                        msg = "操作成功！"
                    });
                }
                else
                {
                    return Json(new
                    {
                        code = -1,
                        msg = "系統繁忙，請稍後再試"
                    });
                }

                
            }catch(Exception e)
            {
                return Json(new
                {
                    code = -1,
                    msg = "系統繁忙，請稍後再試" + e.Message
                });
            }
            
        }
        #endregion

        #region 獲取用戶收貨地址
        /// <summary>
        /// 獲取用戶收貨地址
        /// </summary>
        /// <returns></returns>
        public string getUserAdress()
        {
            string _html = "";
            string sql = "";
            
            try
            {
                try
                {
                    tbId = Session["tb_User"].ToString();
                    userId = Session["id"].ToString();
                }
                catch (Exception e0)
                {
                    tbId = "test";
                    userId = "test";
                }

                sql = "select * from T_User_Adress as t1 where t1.user_id = '" + userId + "'";
                DataSet ds = new DataSet();
                ds = DBClass.execQuery(sql);
                DataTable dt = new DataTable();
                dt = ds.Tables[0];
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        string userData = "";
                        userData += dt.Rows[i]["adress_name"].ToString();
                        if (dt.Rows[i]["adress_phone"].ToString().Length > 0)
                        {
                            userData += " [ " + dt.Rows[i]["adress_phone"].ToString() + " ] ";
                        }
                        if (dt.Rows[i]["adress_tel"].ToString().Length > 0)
                        {
                            userData += " [ " + dt.Rows[i]["adress_tel"].ToString() + " ] ";
                        }
                        _html += "<div class=radio> " +
                            "<label> " +
                                "<input type=radio name=optionsRadios value=" + dt.Rows[i]["id"].ToString() +" checked> " +
                                userData + "<br /> " +
                                dt.Rows[i]["adress_region"].ToString() + " - " + dt.Rows[i]["adress_city"].ToString() + " - " + 
                                dt.Rows[i]["adress_other"].ToString() + " - " + dt.Rows[i]["adress_datail"].ToString() +
                            "</label> " +
                        "</div>";
                    }
                }
                return _html;
            }catch(Exception e)
            {
                return e.Message;
            }
            
        }
        #endregion

        #region 獲取发往区域
        /// <summary>
        /// 獲取发往区域
        /// </summary>
        /// <returns></returns>
        public string getToRegion()
        {
            string _html = "";
            string sql = "";
            sql = "select t1.Region_Id, t1.Region_Name from T_Region as t1";
            DataSet ds = new DataSet();
            ds = DBClass.execQuery(sql);
            DataTable dt = new DataTable();
            dt = ds.Tables[0];
            _html = "<select class=form-control id=region onchange=Region_Change()> " +
                        "<option value=0>請選擇發往區域</option> ";
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    _html += "<option value=" + dt.Rows[i][0].ToString() + ">" + dt.Rows[i][1].ToString() + "</option> ";
                }
            }
            _html += "</select>";
            return _html;
        }
        #endregion

        #region 獲取目的地
        /// <summary>
        /// 獲取目的地
        /// </summary>
        /// <returns></returns>
        public string getDestination(string region)
        {
            string _html = "";
            string sql = "";
            sql = "select t1.destination from T_con_carrier as t1 " +
                "where t1.region_id = '" + region + "' group by t1.destination";
            DataSet ds = new DataSet();
            ds = DBClass.execQuery(sql);
            DataTable dt = new DataTable();
            dt = ds.Tables[0];
            _html = "<select class=form-control id=destination onchange=Destination_Change()> " +
                         "<option>請選擇目的地</option> ";
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    _html += "<option>" + dt.Rows[i][0].ToString() + "</option> ";
                }
            }
            _html += "</select>";
            return _html;
        }
        #endregion

        #region 获得承運商列表
        /// <summary>
        /// 获得承運商列表
        /// </summary>
        /// <returns></returns>
        public string getConList(string region, string destination, string weightIdList)
        {
            string _html = "";
            string sql = "";

            DataSet ds = new DataSet();
            DataSet ds1 = new DataSet();

            try
            {
                sql = "select * from T_con_carrier as t1 " +
                    " where t1.destination = '" + destination + "' " +
                    " and t1.region_id = '" + region + "' ; " +
                    "select * from t_weight as t2 where t2.id in (" + weightIdList + ") ";

                ds = DBClass.execQuery(sql);

                DataTable dt = ds.Tables[0];
                DataTable dt1 = ds.Tables[1];
                float helf = 0;
                for (int i = 0; i < dt1.Rows.Count; i++ )
                {
                    helf += float.Parse(dt1.Rows[i]["Weight_Helf"].ToString());
                }



                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        float freight = 0;
                        string con_id = "";
                        con_id = dt.Rows[i]["id"].ToString();
                        freight = getFeight(con_id, helf);

                        _html += "<tr> " +
                                    "<td style='text-align:center;vertical-align:middle;'> " +
                                        "<input type=radio name=optionsRadios id=optionsRadios1 value=" + con_id + " > " +
                                        //"<input type=radio name=optionsRadios id=optionsRadios1 value=" + con_id + " checked> " +
                                    "</td> " +
                                    "<td style='text-align:center;vertical-align:middle;'> " + (i + 1).ToString() + "</td> " +
                                    "<td style='text-align:center;vertical-align:middle;'>" + dt.Rows[i]["name"].ToString() + "</td> " +
                                    "<td style='text-align:center;vertical-align:middle;'>" + helf + "</td> " +
                                    "<td style='text-align:center;vertical-align:middle;'>" + freight + "</td> " +
                                    "<td style='text-align:center;vertical-align:middle;'>" + dt.Rows[i]["quote_note"].ToString() + "</td> " +
                                 "</tr> ";
                    }
                }
                else
                {
                    _html = "";
                }
            }
            catch (Exception e)
            {
                _html = e.Message;
            }

            return _html;
        }
        #endregion

        #region 根据区间获取运费
        /// <summary>
        /// 根据区间获取运费
        /// </summary>
        /// <param name="carrier_id"></param>
        /// <param name="W"></param>
        /// <returns></returns>
        public float getFeight(string carrier_id, float W)
        {
            float freight = 0;
            string sql = "";
            string weight = "";
            DataSet ds = new DataSet();

            sql = "select * from T_Freight where carrier_id = '" + carrier_id + "' ";
            ds = DBClass.execQuery(sql);

            weight = getWeight(ds.Tables[0], W);

            if (weight == "")
            {
                freight = -100;
            }
            else
            {
                try 
                {
                    freight = Freight.Count_Freight(W, weight);
                }
                catch(Exception e1)
                {
                    freight = -200;
                }
                
            }

            return freight;
        }
        #endregion

        #region 根据区间锁定公式
        /// <summary>
        /// 根据区间锁定公式
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="W"></param>
        /// <returns></returns>
        public string getWeight(DataTable dt, double W)
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
        #endregion
    }
}
