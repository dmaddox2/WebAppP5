using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;

namespace WebAppP5
{
    public partial class Staff : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            HttpCookie myCookies = Request.Cookies["myCookieID"];
            if ((myCookies == null) || (myCookies["Name"] == ""))
            {
                lblOutput.Text = "Welcome, new user";
            }
            else
            {
                lblOutput.Text = "Welcome, " + myCookies["Name"];
            }
        }

        protected void btnHome_Click(object sender, EventArgs e)
        {
            Response.Redirect("Default.aspx");
        }

        protected void btnNewStaff_Click(object sender, EventArgs e)
        {
            // validate username and password
            string filepath = HttpRuntime.AppDomainAppPath + @"\App_Data\Staff.xml";
            string user = txtUser.Text;
            string password = txtPassword.Text;

            if (user == "")
            {
                lblOutput.Text = "Username cannot be empty";
                return;
            }

            if (password == "")
            {
                lblOutput.Text = "Password cannot be empty";
                //lblRegisterOutput.Visible = true;
                return;
            }

            // use hashing service to encrypt password


            // load and check XML document
            XmlDocument doc = new XmlDocument();
            doc.Load(filepath);
            XmlElement root = doc.DocumentElement;

            foreach (XmlNode node in root.ChildNodes)
            {
                if (node["username"].InnerText == user)
                {
                    lblOutput.Text = "Username already exists";
             
                    return;
                }
            }

            // add new member to XML file
            XmlElement newMember = doc.CreateElement("User", root.NamespaceURI);
            root.AppendChild(newMember);
            XmlElement newUser = doc.CreateElement("username", root.NamespaceURI);
            newMember.AppendChild(newUser);
            newUser.InnerText = user;
            XmlElement newPassword = doc.CreateElement("password", root.NamespaceURI);
            newMember.AppendChild(newPassword);
            newPassword.InnerText = password;       // need to change to encrypted password

            doc.Save(filepath);
            lblOutput.Text = String.Format("New Staff Member added: {0}.", user);
        }
    }
}