<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="RepotView.aspx.cs" Inherits="TopUp17Web.Report.RepotView" %>

<%@ Register Assembly="CrystalDecisions.Web, Version=13.0.2000.0, Culture=neutral, PublicKeyToken=692fbea5521e1304" Namespace="CrystalDecisions.Web" TagPrefix="CR" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">

<head runat="server">
    <title></title>
</head>

<body>
    <form id="form1" runat="server">
        <div id="dvReport">
            <div>
                <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
                <CR:CrystalReportViewer ID="CrystalReportViewer" runat="server" HasPrintButton="True" AutoDataBind="true" EnableParameterPrompt="False" EnableDatabaseLogonPrompt="False" EnableTheming="False" HasCrystalLogo="False" HasRefreshButton="True" HasToggleParameterPanelButton="False" Height="50px" ShowAllPageIds="True" ToolPanelView="None" Width="350px" OnUnload="CrystalReportViewer_Unload" HasDrillUpButton="true" HyperlinkTarget="_blank" EnableDrillDown="False" HasDrilldownTabs="False" PrintMode="ActiveX" SeparatePages="true"/>
                
                    <%--<CR:CrystalReportViewer ID="CrystalReportViewer" runat="server" HasPrintButton="True" AutoDataBind="true" EnableParameterPrompt="False" EnableDatabaseLogonPrompt="False" EnableTheming="False" HasCrystalLogo="False" HasRefreshButton="True" HasToggleParameterPanelButton="False" Height="50px" ShowAllPageIds="True" ToolPanelView="None" Width="350px" OnUnload="CrystalReportViewer_Unload" HasDrillUpButton="true" HyperlinkTarget="_blank" EnableDrillDown="False" HasDrilldownTabs="False" PrintMode="ActiveX" SeparatePages="true"/>--%>
            </div>
        </div>
        <asp:TextBox ID="hiddenFieldTextBox"  style="display:none" runat="server"></asp:TextBox>
        <asp:TextBox ID="databaseFileTextBox"  style="display:none" runat="server"></asp:TextBox>
    </form>
</body>

</html>
