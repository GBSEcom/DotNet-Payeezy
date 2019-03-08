<%@ Page Language="C#" Debug="true" AutoEventWireup="true" EnableViewState="false"  EnableEventValidation="false" CodeFile="Default.aspx.cs" Inherits="PayeezyCard" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Payeezy ASP.NET</title>
    <style type="text/css">
body{font-family: Arial, Helvetica, sans-serif;font-size: 14px; margin:20px;;}

    </style>
</head>
<body>
    <h1>Payeezy ASP.NET</h1>
    <form id="form1" runat="server">
    <div>
        <label>API Key<br />
        </label><input type="text" name="apikey" size="60"  /><br /><br />
        <label>Token<br />
        </label><input type="text" name="token" size="60"/><br /><br />
        <label>API Secret<br />
        </label><input type="text" name="apisecret" size="60" /><br /><br />
       
       
        <asp:Button ID="Button1" runat="server" Text="Process" onclick="Button1_Click" />
        <pre><asp:Label ID="error" runat="server" ForeColor="red" Text =""></asp:Label></pre><br />
    </div>
    </form>

    <pre><asp:Label ID="request_label" runat="server"></asp:Label></pre><br />
   
	<pre><asp:Label ID="response_label" runat="server" Text=""></asp:Label></pre>
    <br />

    
</body>
</html>
