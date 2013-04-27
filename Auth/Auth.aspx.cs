using System;
using System.Collections.Generic;

using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using Auth.Class;

namespace Auth
{
    public partial class Auth_aspx : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            //摸拟用户登录验证(帐号、密码于web.config中)
            //真实环境此处应通过数据库进行验证
            if (this.txtAccount.Text == ConfigurationManager.AppSettings["username"] && this.txtPassport.Text == ConfigurationManager.AppSettings["password"])
            {
                //产生令牌
                string tokenValue = this.GetGuidString();
                HttpCookie tokenCookie = new HttpCookie("Token");
                tokenCookie.Values.Add("Value", tokenValue);
                tokenCookie.Domain = "auth.com";
                Response.AppendCookie(tokenCookie);

                //产生主站凭证
                object info = ConfigurationManager.AppSettings["username"].ToString() + "$" + ConfigurationManager.AppSettings["password"].ToString();
                TokenCache.TokenInsert(tokenValue, info, DateTime.Now.AddMinutes(double.Parse(System.Configuration.ConfigurationManager.AppSettings["timeout"])));

                //跳转回分站
                if (Request.QueryString["BackURL"] != null)
                {
                    Response.Redirect(Server.UrlDecode(Request.QueryString["BackURL"]));
                }
            }
            else
            {
                Response.Write("抱歉，帐号或密码有误！请在Web.config中配置帐号密码！");
            }
        }
        /// <summary>
        /// 产生绝对唯一字符串，用于令牌
        /// </summary>
        /// <returns></returns>
        public string GetGuidString()
        {
            return Guid.NewGuid().ToString();
        }
    }
}