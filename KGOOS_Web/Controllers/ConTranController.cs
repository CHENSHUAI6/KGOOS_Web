using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KGOOS_Web.Controllers
{
    public class ConTranController : Controller
    {
        //
        // GET: /ConTran/

        public ActionResult MySelf()
        {
            return View();
        }
        public ActionResult InputGoods()
        {
            return View();
        }
        public ActionResult ConTran()
        {
            return View();
        }
        public ActionResult ConAdress()
        {
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
        
        
        
    }
}
