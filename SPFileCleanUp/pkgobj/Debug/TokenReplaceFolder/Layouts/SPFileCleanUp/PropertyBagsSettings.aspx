<%@ Assembly Name="SPFileCleanUp, Version=1.0.0.0, Culture=neutral, PublicKeyToken=41e7e8d806d0a869" %>
<%@ Import Namespace="Microsoft.SharePoint.ApplicationPages" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PropertyBagsSettings.aspx.cs" Inherits="SPFileCleanUp.Layouts.SPFileCleanUp.PropertyBagsSettings" DynamicMasterPageFile="~masterurl/default.master" %>

<%@ Register TagPrefix="SharePointWebControls" Namespace="Microsoft.SharePoint.WebControls"
    Assembly="Microsoft.SharePoint, Version=12.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages"
    Assembly="Microsoft.SharePoint, Version=12.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="PublishingWebControls" Namespace="Microsoft.SharePoint.Publishing.WebControls"
    Assembly="Microsoft.SharePoint.Publishing, Version=12.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="PublishingNavigation" Namespace="Microsoft.SharePoint.Publishing.Navigation"
    Assembly="Microsoft.SharePoint.Publishing, Version=12.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>

<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Import Namespace="Microsoft.SharePoint.ApplicationPages" %>
<%@ Import Namespace="System.Data" %>
<%@ Register TagPrefix="wssuc" TagName="ToolBar" Src="~/_controltemplates/ToolBar.ascx" %>
<%@ Register TagPrefix="wssuc" TagName="ToolBarButton" Src="~/_controltemplates/ToolBarButton.ascx" %>
<%@ Register TagPrefix="wssuc" TagName="InputFormSection" src="~/_controltemplates/InputFormSection.ascx" %>
<%@ Register TagPrefix="wssuc" TagName="InputFormControl" src="~/_controltemplates/InputFormControl.ascx" %>
<%@ Register TagPrefix="wssuc" TagName="ButtonSection" src="~/_controltemplates/ButtonSection.ascx" %>
<%@ Register TagPrefix="wssawc" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=12.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>

<asp:Content ID="Content2" ContentPlaceHolderID="PlaceHolderPageTitle" runat="server">
Custom list config settings page
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="PlaceHolderPageTitleInTitleArea" runat="server">
Custom list config settings
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="PlaceHolderMain" runat="server">
<table width="50%" class="ms-propertysheet" cellspacing="0" cellpadding="0" border="0">
<tr>
<td class="ms-error">
<asp:Label ID="lblMessage" runat="server" EnableViewState="False" />
</td>
</tr>
</table>
<table width="50%" class="ms-propertysheet" cellspacing="0" cellpadding="0" border="0">
<tr><td class="ms-descriptionText"><asp:Label ID="LabelMessage" Runat="server" EnableViewState="False" class="ms-descriptionText"/></td></tr>
<tr><td class="ms-error"><asp:Label ID="LabelErrorMessage" Runat="server" EnableViewState="False" /></td></tr>
</table>
<table width="50%" border="0" cellspacing="0" cellpadding="0" class="ms-propertysheet">
<!-- Key -->
<wssuc:InputFormSection Title="Key" runat="server">
<Template_Description>
<SharePoint:EncodedLiteral ID="EncodedLiteral1" runat="server" text="Specify a key for the entry." EncodeMethod='HtmlEncodeAllowSimpleTextFormatting'/>
</Template_Description>
<Template_InputFormControls>
<wssuc:InputFormControl LabelText="" runat="server">
<Template_control>
<wssawc:InputFormTextBox Title="Key" class="ms-input" ID="txtKey" Columns="35" Runat="server" MaxLength="255" />
</Template_control>
</wssuc:InputFormControl>
</Template_InputFormControls>
</wssuc:InputFormSection>
<!-- Value -->
<wssuc:InputFormSection Title="Value" runat="server">
<Template_Description>
<SharePoint:EncodedLiteral ID="EncodedLiteral2" runat="server" text="Specify the value of the entry" EncodeMethod='HtmlEncodeAllowSimpleTextFormatting'/>
</Template_Description>
<Template_InputFormControls>
<wssuc:InputFormControl LabelText="" runat="server">
<Template_control>
<wssawc:InputFormTextBox Title="Value" class="ms-input" ID="txtValue" Columns="35" Runat="server" MaxLength="255" />
</Template_control>
</wssuc:InputFormControl>
</Template_InputFormControls>
</wssuc:InputFormSection>
<wssuc:ButtonSection runat="server" ShowStandardCancelButton="false">
<template_buttons>
<asp:PlaceHolder ID="PlaceHolderInsertUpdate" runat="server">
<asp:Button UseSubmitBehavior="false" runat="server" class="ms-ButtonHeightWidth" Text="Insert" id="cmdInsertUpdate" Onclick="cmdInsertUpdate_OnClick"/>  
<asp:Button UseSubmitBehavior="false" runat="server" class="ms-ButtonHeightWidth" Text="Cancel" id="cmdInsertUpdateCancel" Onclick="cmdInsertUpdateCancel_OnClick"/>  
</asp:PlaceHolder>
</template_buttons>
</wssuc:ButtonSection>
<br />
<br />
</table>
<SharePoint:SPGridView ID="grdEntries" runat="server" AutoGenerateColumns="false" Width="50%" AllowSorting="True" OnRowDeleting="grdEntries_RowDeleting" OnRowEditing="grdEntries_RowEditing" EnableViewState="false">
<AlternatingRowStyle CssClass="ms-alternating" />
</SharePoint:SPGridView>
<wssuc:ButtonSection runat="server" ShowStandardCancelButton="false">
<template_buttons>
<asp:PlaceHolder ID="PlaceHolder1" runat="server">
<asp:Button UseSubmitBehavior="false" runat="server" class="ms-ButtonHeightWidth" OnClick="cmdOK_OnClick" Text="OK" id="cmdOK" causesvalidation=false />
</asp:PlaceHolder>
</template_buttons>
</wssuc:ButtonSection>
<br />
<br />
<SharePoint:FormDigest ID="FormDigest1" runat="server" />
</asp:Content>
