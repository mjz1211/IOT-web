<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="GetParkData.aspx.cs" Inherits="MyWeb.GetParkData" Async="true"%>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>取得停車位即時資訊</title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <asp:Button ID="btnGetParking" runat="server" OnClick="btnGetParking_Click" Text="取得Park01停車狀態" />
        </div>
        <p>
            <asp:TextBox ID="txtStatus" runat="server" Width="737px"></asp:TextBox>
        </p>
    </form>
</body>
</html>
