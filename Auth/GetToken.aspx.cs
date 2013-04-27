using System;
using System.Collections.Generic;

using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Auth
{
    public partial class GetToken : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.QueryString["BackURL"] != null)
            {
                //获取URL参数（回调地址）
                string backURL = Server.UrlDecode(Request.QueryString["BackURL"]);
                //获取Cookie（令牌）---- 登录认证时保存的Cookie
                HttpCookie tokenCookie = Request.Cookies["Token"];
                if (tokenCookie != null)
                {
                    //令牌存在则替换回调地址中的$Token$
                    backURL = backURL.Replace("$Token$", tokenCookie.Values["Value"].ToString());
                }

                Response.Redirect(backURL);
            }
        }
    }
}