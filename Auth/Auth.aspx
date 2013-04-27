<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Auth.aspx.cs" Inherits="Auth.Auth_aspx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>身份验证登录页</title>
    <style type="text/css">
        html,body{ margin:0px; padding:0px;font-family:微软雅黑;}
        #head{height:40px; font-size:22px; line-height:40px; border:1px solid #D8DFEA; margin:5px 10px;  padding:0px 24px; color:red; }
        #login{ margin:5px 10px;  padding:0px 24px;}
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div id="head">
        <b>SSO测试：主站：www.auth.com</b>
    </div>
    <div id="login">
        <div>           
            <br />
            帐号：<asp:TextBox ID="txtAccount" runat="server" Width="180px"></asp:TextBox><br />
            <br />
            密码：<asp:TextBox ID="txtPassport" runat="server" TextMode="Password" Width="180px"></asp:TextBox><br />
            <br />
            <asp:Button ID="btnSubmit" runat="server" OnClick="btnSubmit_Click" Text="提 交" Width="89px" />
        </div>
    </div>
    </form>
</body>
</html>
