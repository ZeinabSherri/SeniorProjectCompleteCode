using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;

namespace OnlineJobPortalProject.Admin
{
    public partial class ViewResume : System.Web.UI.Page
    {
        SqlConnection con;
        SqlCommand cmd;
        DataTable dt;
        string str = ConfigurationManager.ConnectionStrings["cs"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session[" Admin "] == null)
            {
                Response.Redirect("../User/Login.aspx");
            }
            //check if request is not a postback, which means it's the initial page
            //to ensure loading jobs on initial page 
            if (!IsPostBack)
            {
                ShowAppliedJob();
            }
        }

        private void ShowAppliedJob()
        {
            string query = string.Empty;
            con = new SqlConnection(str);
            query = @"Select Row_Number() over(order by (Select 1)) as [Sr.No], JobId, Title, NoOfPost, Qualification, Experience, LastDateToApply, CompanyName, Country, State, CreateDate from Jobs";
            cmd = new SqlCommand(query, con);
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            dt = new DataTable();
            sda.Fill(dt);
            GridView1.DataSource = dt;
            GridView1.DataBind();
        }

        protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GridView1.PageIndex = e.NewPageIndex;
            ShowAppliedJob() ;
        }

        protected void GridView1_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                GridViewRow row = GridView1.Rows[e.RowIndex];
                int AppliedJobId = Convert.ToInt32(GridView1.DataKeys[e.RowIndex].Values[0]);
                con = new SqlConnection(str);
                cmd = new SqlCommand("Delete from AppliedJobs where AppliedJobId=@id", con);
                cmd.Parameters.AddWithValue("@id", AppliedJobId);
                con.Open();
                int r = cmd.ExecuteNonQuery();
                if (r > 0)
                {
                    lblMsg.Text = "Resume deleted successfully";
                    lblMsg.CssClass = "alert alert-success";

                }
                else
                {
                    lblMsg.Text = "sorry, couldn't delete Record";
                    lblMsg.CssClass = "alert alert-danger";
                }
             
                GridView1.EditIndex = -1;
                ShowAppliedJob();
            }
            catch (Exception ex)
            {
              
                Response.Write("<script>alert('" + ex.Message + "');</script>");
            }
            finally
            {
                con.Close() ;
            }
        }

        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            e.Row.Attributes["onclick"] = Page.ClientScript.GetPostBackClientHyperlink(GridView1, "Select$" + e.Row.RowIndex);
            e.Row.ToolTip = "Click to view Job details";
        }

        protected void GridView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            foreach(GridView row in GridView1.Rows)
            {
                if(row.RowIndex == GridView1.SelectedIndex)
                {
                    HiddenField jobId = (HiddenField)row.FindControl("hdnJobId");
                    Response.Redirect("JobList.aspx?id=" + jobId.Value ) ;
                }
                else
                {
                    row.BackColor = ColorTranslator.FromHtml("#FFFFFF");
                    row.ToolTip = "Click to select this row";
                }
            }


        }
    }
}