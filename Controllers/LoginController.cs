using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TCallingTask.Models;

namespace TCallingTask.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Authorize(UserMaster userModel)
        {
            using (SqlConnection sqlCon = new SqlConnection("Data Source = DESKTOP-BV0OTOG\\SQLEXPRESS ; Initial Catalog = TCalling ; Integrated Security = true ; multipleactiveresultsets = true ; timeout = 1000; Connection Timeout = 1000;")) // Replace with your actual connection string
            {
                sqlCon.Open();
                string query = "SELECT * FROM UserMaster WHERE UserName=@UserName AND Password=@Password";
                SqlCommand sqlCmd = new SqlCommand(query, sqlCon);
                sqlCmd.Parameters.AddWithValue("@UserName", userModel.UserName);
                sqlCmd.Parameters.AddWithValue("@Password", userModel.Password);
                SqlDataReader sdr = sqlCmd.ExecuteReader();
                if (sdr.Read())
                {
                    Session["UserID"] = sdr["UserID"].ToString();
                    Session["UserName"] = sdr["UserName"].ToString();
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ViewBag.LoginErrorMessage = "Invalid Username or Password.";
                    return View("Index", userModel);
                }
            }
        }

        public ActionResult Logout()
        {
            Session.Abandon();
            return RedirectToAction("Index", "Login");
        }
    }
}