using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Net.NetworkInformation;

namespace OnlineJobPortalProject.User
{
    public partial class ResumBuild : System.Web.UI.Page
    {
        SqlConnection con;
        SqlCommand cmd;
        SqlDataReader sdr;
        string str = ConfigurationManager.ConnectionStrings["cs"].ConnectionString;
        string query;

        protected void Page_Load(object sender, EventArgs e)
        {

            if (Session[" User "] == null)
            {
                Response.Redirect("Login.aspx");

            }


            if (IsPostBack)
            {

                if (Request.QueryString["id"] != null)
                {
                    ShowUserInfo();
                }
                else
                {
                    Response.Redirect("Login.aspx");
                }
            }
        }

        private void ShowUserInfo()
        {
            try
            {
                con = new SqlConnection(str);
                string query = "Select * from [User] where UserId=@UserId";
                cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@UserId", Request.QueryString["id"]);
                con.Open();
                sdr = cmd.ExecuteReader();
                if (sdr.HasRows)
                {
                    if (sdr.Read())
                    {
                        txtUserName.Text = sdr["UserName"].ToString();
                        txtFullName.Text = sdr["FullName"].ToString();
                        txtEmail.Text = sdr["Email"].ToString();
                        txtUniversity.Text = sdr["University"].ToString();
                        txtMajor.Text = sdr["Major"].ToString();
                        txtMasters.Text = sdr["Masters"].ToString();
                        txtGraduation.Text = sdr["GradGPA"].ToString();
                        txtWork.Text = sdr["WorksOn"].ToString();
                        txtExperience.Text = sdr["Experience"].ToString();
                        txtAddress.Text = sdr["Address"].ToString();
                        ddlCountry.SelectedValue = sdr["Country"].ToString();
                    }
                }
                else
                {
                    LblMsg.Visible = true;
                    LblMsg.Text = "User not found!";
                    LblMsg.CssClass = "alert alert-danger";
                }
            }
            catch (Exception ex)
            {
                Response.Write("<script>alert('" + ex.Message + "');</script>");
            }
            finally
            {
                con.Close();
            }
        }

        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                if (Request.QueryString["id"] != null)
                {
                    string concatQuery = string.Empty;
                    string    filePath = string.Empty;
                    //bool isValidToExcute = false;
                    bool isValid = false;
                    con = new SqlConnection(str);
                    if (fuResume.HasFile)
                    {
                        if(Utils.isValidExtension4Resume(fuResume.FileName)) { 
                        
                        concatQuery = "Resume=@resume";
                            isValid = true;
                        }
                        else
                        {
                            concatQuery = string.Empty;
                           
                        }
                    }
                    else
                    {
                        concatQuery = string.Empty;
                    }
                    query = @"Update [User] set Username=@Username,Name=@Name, Email=@Email, Mobile=@Mobile, GradGPA=@GradGPA, 
                             WorksOn=@WorksOn,Experience=@Experience, University=@University, Major=@Major, Masters=@Masters" + concatQuery + 
                             "Address=@Address,Country=@Country where UserId=@UserId";
                    cmd = new SqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@Username", txtUserName.Text.Trim());
                    cmd.Parameters.AddWithValue("@Name", txtFullName.Text.Trim());
                    cmd.Parameters.AddWithValue("@Email", txtEmail.Text.Trim());
                    cmd.Parameters.AddWithValue("@Mobile", txtMobileNumber.Text.Trim());
                    cmd.Parameters.AddWithValue("@GradGPA", txtGraduation.Text.Trim());
                    cmd.Parameters.AddWithValue("@WorksOn", txtWork.Text.Trim());
                    cmd.Parameters.AddWithValue("@Experience", txtExperience.Text.Trim());
                    cmd.Parameters.AddWithValue("@University", txtUniversity.Text.Trim());
                    cmd.Parameters.AddWithValue("@Major", txtMajor.Text.Trim());
                    cmd.Parameters.AddWithValue("@Masters", txtMasters.Text.Trim());
                    cmd.Parameters.AddWithValue("@Address", txtAddress.Text.Trim());
                    cmd.Parameters.AddWithValue("@Country",ddlCountry.SelectedValue);
                    cmd.Parameters.AddWithValue("@UserId", Request.QueryString["id"]);
                    if (fuResume.HasFile)
                    {
                        if (Utils.isValidExtension4Resume(fuResume.FileName))
                        {
                            Guid obj = Guid.NewGuid();
                            filePath = "Resumes/" + obj.ToString() + fuResume.FileName;
                            fuResume.PostedFile.SaveAs(Server.MapPath("~/Resumes/") + obj.ToString() + fuResume.FileName);

                            cmd.Parameters.AddWithValue("resume", filePath);
                            isValid = true;  
                        }
                        else
                        {
                            concatQuery = string.Empty;
                            LblMsg.Visible = true;
                            LblMsg.Text = "Please selected .doc, .docx, .pdf file for resume!";
                            LblMsg.CssClass = "alert alert-danger";
                        }
                    }
                    else
                    {
                        isValid = true;
                    }
                    if (isValid)
                    {
                        con.Open();
                        int r = cmd.ExecuteNonQuery();
                        if(r > 0)
                        {
                            LblMsg.Visible = true;
                            LblMsg.Text = "Resume details updated successfully";
                            LblMsg.CssClass = "alert alert-success";

                        }
                        else
                        {
                            LblMsg.Visible = true;
                            LblMsg.Text = "Cannot update the records, pleasse try after sometime!";
                            LblMsg.CssClass = "alert alert-danger";
                        }
                    }
                }
                else
                {
                    LblMsg.Visible = true;
                    LblMsg.Text = "Cannot update the records, please try <b>Relogin</b>";
                    LblMsg.CssClass = "alert alert-danger";
                }
            }
            catch (SqlException ex)
            {
                if (ex.Message.Contains("Violation of unique KEY constraint"))
                {
                    LblMsg.Visible = true;
                    LblMsg.Text = "Sorry, the user " + "<b>" + txtUserName.Text.Trim() + "</b> already exists, try another one.";
                    LblMsg.CssClass = "alert alert-warning";
                }
                else
                {
                    Response.Write("<script>alert('" + ex.Message + "');</script>");
                }
            }
            catch(Exception ex)
            {
                Response.Write("<script>alert('" + ex.Message + "');</script>");

            }
            finally
            {
                con.Close();
            }
        }
    }
}