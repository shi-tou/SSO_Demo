using System;
using System.Collections.Generic;
using System.Web;
using System.Text.RegularExpressions;

namespace SiteA.Class
{
    public class AuthBase : System.Web.UI.Page
    {
        /// <summary>
        /// 重写OnLoad的事件
        /// </summary>
        /// <param name="e"></param>
        protected override void OnLoad(EventArgs e)
        {
            if (Session["Token"] != null)
            {
                //分站凭证存在
                Response.Write("恭喜，分站凭证存在，您被授权访问该页面！");
            }
            else
            {
                //令牌验证结果
                if (Request.QueryString["Token"] != null)
                {
                    if (Request.QueryString["Token"] != "$Token$")
                    {
                        //持有令牌
                        string tokenValue = Request.QueryString["Token"];
                        //调用WebService获取主站凭证
                        SiteB.TokenService.TokenService tokenService = new SiteB.TokenService.TokenService();
                        SiteB.TokenService.MySoapHeader header = new SiteB.TokenService.MySoapHeader();
                        header.UserID = "admin";
                        header.PassWord = "admin";
                        tokenService.MySoapHeaderValue = header;
                        object o = tokenService.TokenGetCredence(tokenValue);
                        if (o != null)
                        {
                            //令牌正确
                            Session["Token"] = o;                            
                            Response.Write("恭喜，令牌存在，您被授权访问该页面！");
                        }
                        else
                        {
                            //令牌错误
                            Response.Redirect(this.ReplaceToken());
                        }
                    }
                    else
                    {
                        //未持有令牌
                        Response.Redirect(this.ReplaceToken());
                    }
                }
                //未进行令牌验证，去主站验证
                else
                {
                    Response.Redirect(this.getTokenURL());
                }
            }

            base.OnLoad(e);
        }

        /// <summary>
        /// 获取带令牌请求的URL
        /// </summary>
        /// <returns></returns>
        private string getTokenURL()
        {
            string url = Request.Url.AbsoluteUri;
            Regex reg = new Regex(@"^.*\?.+=.+$");
            if (reg.IsMatch(url))
                url += "&Token=$Token$";
            else
                url += "?Token=$Token$";

            return "http://www.auth.com/gettoken.aspx?BackURL=" + Server.UrlEncode(url);
        }

        /// <summary>
        /// 去掉URL中的令牌
        /// </summary>
        /// <returns></returns>
        private string ReplaceToken()
        {
            string url = Request.Url.AbsoluteUri;
            url = Regex.Replace(url, @"(\?|&)Token=.*", "", RegexOptions.IgnoreCase);
            return "http://www.auth.com/auth.aspx?BackURL=" + Server.UrlEncode(url);
        }

    }
}