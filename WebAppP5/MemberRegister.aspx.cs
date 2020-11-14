using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;

namespace WebAppP5
{
    public partial class MemberRegister : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            HttpCookie myCookies = Request.Cookies["myCookieID"];
            if ((myCookies == null) || (myCookies["Name"] == ""))
            {
                lblRegisterOutput.Text = "Welcome, new user";
            }
            else
            {
                lblRegisterOutput.Text = "Welcome, " + myCookies["Name"];
            }
        }

        protected void btnRegister_Click(object sender, EventArgs e)
        {
            // image verification situation

            // validate username and password
            string filepath = HttpRuntime.AppDomainAppPath + @"\App_Data\Member.xml";
            string user = txtRegisterUser.Text;
            string password = txtRegisterPassword.Text;

            if (user == "")
            {
                lblRegisterOutput.Text = "Username cannot be empty";
                //lblRegisterOutput.Visible = true;
                return;
            }

            if (password == "")
            {
                lblRegisterOutput.Text = "Password cannot be empty";
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
                    lblRegisterOutput.Text = "Username already exists";
                    //lblRegisterOutput.Visible = true;
                    return;
                }
            }

            // add new member to XML file
            XmlElement newMember = doc.CreateElement("Member", root.NamespaceURI);
            root.AppendChild(newMember);
            XmlElement newUser = doc.CreateElement("username", root.NamespaceURI);
            newMember.AppendChild(newUser);
            newUser.InnerText = user;
            XmlElement newPassword = doc.CreateElement("password", root.NamespaceURI);
            newMember.AppendChild(newPassword);
            newPassword.InnerText = password;       // need to change to encrypted password

            doc.Save(filepath);
            lblRegisterOutput.Text =  String.Format("You are now registered, {0}. " +
                "Please return to home page and log in", user);

        }

        protected void btnHome_Click(object sender, EventArgs e)
        {
            Response.Redirect("Default.aspx");
        }
    }
}