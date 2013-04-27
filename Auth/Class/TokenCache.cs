using System;
using System.Collections.Generic;
using System.Web;
using System.Data;

namespace Auth.Class
{
    public class TokenCache
    {
        /// <summary>
        /// 获取缓存中的DataTable
        /// </summary>
        /// <returns></returns>
        public static DataTable GetCacheTable()
        {
            DataTable dt = null;
            if (HttpContext.Current.Cache["TokenList"] != null)
            {
                dt = (DataTable)HttpContext.Current.Cache["TokenList"];
            }
            return dt;
        }

        /// <summary>
        /// 初始化数据结构
        /// </summary>
        /// <remarks>
        /// ----------------------------------------------------
        /// | token(令牌) | info(用户凭证) | timeout(过期时间) |
        /// |--------------------------------------------------|
        /// </remarks>
        private static void cacheInit()
        {
            if (HttpContext.Current.Cache["TokenList"] == null)
            {
                DataTable dt = new DataTable();

                dt.Columns.Add("token", Type.GetType("System.String"));
                dt.Columns["token"].Unique = true;

                dt.Columns.Add("info", Type.GetType("System.Object"));
                dt.Columns["info"].DefaultValue = null;

                dt.Columns.Add("timeout", Type.GetType("System.DateTime"));
                dt.Columns["timeout"].DefaultValue = DateTime.Now.AddMinutes(double.Parse(System.Configuration.ConfigurationManager.AppSettings["timeout"]));

                DataColumn[] keys = new DataColumn[1];
                keys[0] = dt.Columns["token"];
                dt.PrimaryKey = keys;

                //Cache的过期时间为 令牌过期时间*2
                HttpContext.Current.Cache.Insert("TokenList", dt, null, DateTime.MaxValue, TimeSpan.FromMinutes(double.Parse(System.Configuration.ConfigurationManager.AppSettings["timeout"]) * 2));
            }
        }

        /// <summary>
        /// 判断令牌是否存在
        /// </summary>
        /// <param name="token">令牌</param>
        /// <returns></returns>
        public static bool TokenIsExist(string token)
        {
            cacheInit();

            DataTable dt = (DataTable)HttpContext.Current.Cache["TokenList"];
            if (dt.Select("token = '" + token + "'").Length == 0)
                return false;
            else
                return true;
        }

        /// <summary>
        /// 更新令牌过期时间
        /// </summary>
        /// <param name="token">令牌</param>
        /// <param name="time">过期时间</param>
        public static void TokenTimeUpdate(string token, DateTime time)
        {
            cacheInit();

            DataTable dt = (DataTable)HttpContext.Current.Cache["TokenList"];
            DataRow[] dr = dt.Select("token = '" + token + "'");
            if (dr.Length > 0)
            {
                dr[0]["timeout"] = time;
            }
        }

        /// <summary>
        /// 添加令牌
        /// </summary>
        /// <param name="token">令牌</param>
        /// <param name="info">凭证</param>
        /// <param name="timeout">过期时间</param>
        public static void TokenInsert(string token, object info, DateTime timeout)
        {
            cacheInit();

            if (!TokenIsExist(token))
            {
                DataTable dt = (DataTable)HttpContext.Current.Cache["TokenList"];
                DataRow dr = dt.NewRow();
                dr["token"] = token;
                dr["info"] = info;
                dr["timeout"] = timeout;
                dt.Rows.Add(dr);
                HttpContext.Current.Cache["TokenList"] = dt;
            }
            else
            {
                TokenTimeUpdate(token, timeout);
            }
        }
    }
}