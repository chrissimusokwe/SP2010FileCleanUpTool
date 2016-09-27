
namespace SPFileCleanUp.Layouts.SPFileCleanUp
{
    using System;
    using System.Data;
    using System.Configuration;
    using System.Collections;
    using System.Web;
    using System.Web.Security;
    using System.Web.UI;
    using System.Web.UI.WebControls;
    using System.Web.UI.WebControls.WebParts;
    using System.Web.UI.HtmlControls;
    using Microsoft.SharePoint;
    using Microsoft.SharePoint.WebControls;
    using Microsoft.SharePoint.Utilities;
    using System.Xml;

        public partial class PropertyBagsSettings : LayoutsPageBase
        {
            private DataTable _dtEntries;
            private DataView _dvEntriesView;
            protected SPGridView grdEntries = new SPGridView();
            protected Label lblMessage = new Label();
            protected InputFormTextBox txtKey = new InputFormTextBox();
            protected InputFormTextBox txtValue = new InputFormTextBox();

            //In this event will we add two data columns and two command cokumns to our grid.
            protected void Page_Init(object sender, EventArgs e)
            {
                BoundField objKeyField = new BoundField();
                objKeyField.ItemStyle.Width = Unit.Percentage(30);
                objKeyField.DataField = "Key";
                objKeyField.HeaderText = "Key";
                grdEntries.Columns.Add(objKeyField);

                BoundField objValueField = new BoundField();
                objValueField.DataField = "Value";
                objValueField.HeaderText = "Value";
                grdEntries.Columns.Add(objValueField);

                CommandField objEditField = new CommandField();
                objEditField.ShowEditButton = true;
                objEditField.HeaderText = "Edit";
                objEditField.ItemStyle.Width = Unit.Pixel(20);
                objEditField.ButtonType = ButtonType.Image;
                objEditField.EditImageUrl = "/_layouts/images/edit.gif";
                grdEntries.Columns.Add(objEditField);

                CommandField objDeleteField = new CommandField();
                objDeleteField.ShowDeleteButton = true;
                objDeleteField.HeaderText = "Delete";
                objDeleteField.ItemStyle.Width = Unit.Pixel(20);
                objDeleteField.ButtonType = ButtonType.Image;
                objDeleteField.DeleteImageUrl = "/_layouts/images/delete.gif";
                grdEntries.Columns.Add(objDeleteField);
            }

            //In this event we set allowunsafeupdates=true and we bind the grid to data.
            protected void Page_Load(object sender, EventArgs e)
            {
                SPContext.Current.Web.AllowUnsafeUpdates = true;
                BindGrid();
            }

            //LoadData function to help load the data into the datatable.
            private void LoadData(string strSortOrder)
            {
                _dtEntries = new DataTable();
                DataColumn objKeyColumn = new DataColumn();
                objKeyColumn.DataType = Type.GetType("System.String");
                objKeyColumn.ColumnName = "Key";
                objKeyColumn.ReadOnly = true;
                _dtEntries.Columns.Add(objKeyColumn);

                DataColumn objValueColumn = new DataColumn();
                objValueColumn.DataType = Type.GetType("System.String");
                objValueColumn.ColumnName = "Value";
                objValueColumn.ReadOnly = true;
                _dtEntries.Columns.Add(objValueColumn);

                foreach (string strKey in SPContext.Current.Site.RootWeb.Properties.Keys)
                {
                    if (strKey.StartsWith("vti_")) continue;
                    if (strKey.StartsWith("_reporting")) continue;
                    if (SPContext.Current.Site.RootWeb.Properties[strKey] == null) continue;
                    if (SPContext.Current.Site.RootWeb.Properties[strKey].ToString() == String.Empty) continue;

                    DataRow objRow = _dtEntries.NewRow();
                    try
                    {
                        objRow["Key"] = strKey;
                    }
                    catch
                    {
                        objRow["Key"] = "Error";
                    }
                    try
                    {
                        objRow["Value"] = SPContext.Current.Site.RootWeb.Properties[strKey];
                    }
                    catch
                    {
                        objRow["Value"] = "Error";
                    }
                    _dtEntries.Rows.Add(objRow);
                }
                _dvEntriesView = _dtEntries.DefaultView;
                _dvEntriesView.Sort = "[Key] asc";
            }

            //Helper function to populate the grid.
            private void BindGrid()
            {
                try
                {
                    LoadData(String.Empty);
                    grdEntries.DataSource = _dvEntriesView;
                    grdEntries.DataBind();
                }
                catch (Exception err)
                {
                    lblMessage.Text = lblMessage.Text + "Failed to bind data to page. Please try to reload the page.";
                }
            }

            //Event handlers for the grid behaviour.
            public void grdEntries_RowDeleting(object sender, GridViewDeleteEventArgs e)
            {
                try
                {
                    string strKey = grdEntries.Rows[e.RowIndex].Cells[0].Text;
                    strKey = SPHttpUtility.HtmlDecode(strKey);
                    if (SPContext.Current.Site.RootWeb.Properties.ContainsKey(strKey))
                    {
                        SPContext.Current.Site.RootWeb.Properties[strKey] = null;
                        SPContext.Current.Site.RootWeb.Properties.Update();
                    }
                    BindGrid();
                }
                catch (Exception err)
                {
                    lblMessage.Text = lblMessage.Text + err.Message + "\n";
                }
            }

            public void grdEntries_RowEditing(object sender, GridViewEditEventArgs e)
            {
                string strKey = grdEntries.Rows[e.NewEditIndex].Cells[0].Text;
                txtKey.Text = strKey;
                txtKey.Enabled = false;
                txtValue.Text = SPContext.Current.Site.RootWeb.Properties[strKey].ToString();
            }

            public void grdEntries_Sorting(object sender, GridViewEditEventArgs e)
            {
                grdEntries.DataSource = _dvEntriesView;
                grdEntries.DataBind();
            }

            //Add a helper function to build a redirect url (which will be used when the form close):
            //private string GetRedirectUrl()
            //{
            //    //return SPContext.Current.Web.Url + String.Format("/_layouts/listedit.aspx?List={0}", Request.QueryString["List"]);
            //}

            //Add the Button Click Events which corresponds to the definition in the aspx file:
            public void cmdOK_OnClick(object sender, EventArgs e)
            {
                SPUtility.Redirect(SPContext.Current.Web.Url, SPRedirectFlags.UseSource, HttpContext.Current);
            }

            public void cmdInsertUpdate_OnClick(object sender, EventArgs e)
            {
                try
                {
                    string strKey = txtKey.Text;
                    if (strKey.Length == 0)
                    {
                        throw new Exception("You must provide a key.");
                    }
                    if (strKey.Contains("&"))
                    {
                        throw new Exception("& is not allowed. You must provide an alternative name.");
                    }
                    string strValue = txtValue.Text;
                    if (strValue.Length == 0)
                    {
                        throw new Exception("You must provide a value.");
                    }
                    if (SPContext.Current.Site.RootWeb.Properties.ContainsKey(strKey))
                    {
                        SPContext.Current.Site.RootWeb.Properties[strKey] = strValue;
                    }
                    else
                    {
                        SPContext.Current.Site.RootWeb.Properties.Add(strKey, strValue);
                    }
                    SPContext.Current.Site.RootWeb.Properties.Update();
                    strKey = "";
                    strValue = "";
                    txtKey.Text = "";
                    txtKey.Enabled = true;
                    txtValue.Text = "";
                    BindGrid();
                }
                catch (Exception err)
                {
                    lblMessage.Text = err.Message;
                }
            }

            public void cmdInsertUpdateCancel_OnClick(object sender, EventArgs e)
            {
                txtKey.Text = "";
                txtKey.Enabled = true;
                txtValue.Text = "";
            }

        }
    }
