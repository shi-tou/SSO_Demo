using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using Auth.Class;
using System.Data;

namespace Auth
{
    /// <summary>
    /// 主站WEB服务
    /// 对接口添加安全验证
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]

    public class TokenService : System.Web.Services.WebService
    {
        public string msg = "";
        //声明重写的SOAP标头
        public MySoapHeader mySoapHeader = new MySoapHeader();

        /// <summary>
        /// 根据令牌获取用户凭证
        /// </summary>
        /// <param name="tokenValue">令牌</param>
        /// <returns></returns>
        [WebMethod]
        [SoapHeader("mySoapHeader")]
        public object TokenGetCredence(string tokenValue)
        {
            if (!mySoapHeader.IsValid())
            {
                return null;
            }
            object obj = null;
            DataTable dt = TokenCache.GetCacheTable();
            if (dt != null)
            {
                DataRow[] dr = dt.Select("token = '" + tokenValue + "'");
                if (dr.Length > 0)
                {
                    obj = dr[0]["info"];
                }
            }
            return obj;
        }

        /// <summary>
        /// 清除令牌
        /// </summary>
        /// <param name="tokenValue">令牌</param>
        [WebMethod]
        [SoapHeader("mySoapHeader")]
        public object ClearToken(string tokenValue)
        {
            if (!mySoapHeader.IsValid())
            {
                return msg;
            }
            DataTable dt = TokenCache.GetCacheTable();
            if (dt != null)
            {
                DataRow[] dr = dt.Select("token = '" + tokenValue + "'");
                if (dr.Length > 0)
                {
                    dt.Rows.Remove(dr[0]);
                }
            }
            return null;
        }
    }

    #region MySoapHeader
    /// <summary>
    /// 定义SOAP头（重写SoapHeader，表示 SOAP 标头的内容。）
    /// </summary>
    public class MySoapHeader : SoapHeader
    {
        private string _userID = string.Empty;
        private string _passWord = string.Empty;

        ///
        /// 构造函数
        ///
        public MySoapHeader()
        {
        }
        ///
        /// 构造函数
        ///
        /// 用户ID
        /// 加密后的密码
        public MySoapHeader(string nUserID, string nPassWord)
        {
            Initial(nUserID, nPassWord);
        }
        #region 属性
        ///
        /// 用户名
        ///
        public string UserID
        {
            get { return _userID; }
            set { _userID = value; }
        }
        ///
        /// 加密后的密码
        ///
        public string PassWord
        {
            get { return _passWord; }
            set { _passWord = value; }
        }
        #endregion
        #region 方法
        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="nUserID">用户ID</param>
        /// <param name="nPassWord">加密后的密码</param>
        public void Initial(string nUserID, string nPassWord)
        {
            UserID = nUserID;
            PassWord = nPassWord;
        }
        /// <summary>
        /// 用户名密码是否正确
        /// </summary>
        /// <param name="nUserID">用户ID</param>
        /// <param name="nPassWord">加密后的密码</param>
        /// <param name="nMsg">返回的错误信息</param>
        /// <returns>用户名密码是否正确</returns> 
        public bool IsValid(string nUserID, string nPassWord)
        {;
            try
            {
                //判断用户名密码是否正确 (注：这个密码没有经过加密处理)
                if (nUserID == "admin" && nPassWord == "admin")
                {
                    return true;
                }
                else
                {                   
                    return false;
                }
            }
            catch
            {
                return false;
            }
        }
        ///
        /// 用户名密码是否正确
        ///
        ///
        ///
        public bool IsValid()
        {
            return IsValid(UserID, PassWord);
        }
        #endregion
    }
    #endregion
}
