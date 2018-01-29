using gicmart.Areas.Admin.Models;
using gicmart.Areas.User.Filters;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace gicmart.Areas.User.Controllers
{
    [SessionExpire]
    public class usermysponsorController : Controller
    {
        //
        // GET: /User/usermysponsor/
        [SessionExpire]
        public ActionResult Index()
        {
            List<profile> imagelst = new List<profile>();
            TempData["userId"] = System.Web.HttpContext.Current.Session["userId"];
            TempData["userName"] = System.Web.HttpContext.Current.Session["userName"];
            ViewBag.sectionName = "My Sponsors";
            SqlDataReader rdr = null;
            var profileinfo = new profile();
            string cs = ConfigurationManager.ConnectionStrings["cs"].ConnectionString;
            SqlConnection con = new SqlConnection(cs);
            con.Open();
            string usersp2 = "sp_getMySponsers";
            SqlCommand cmd4 = new SqlCommand(usersp2, con);
            cmd4.CommandType = CommandType.StoredProcedure;

            cmd4.Parameters.AddWithValue("@empid", System.Web.HttpContext.Current.Session["userId"]);
            //cmd4.ExecuteNonQuery(); // MISSING
            //getting reference_user_Id
            try
            {
                rdr = cmd4.ExecuteReader();
                // iterate through results, printing each to console
                while (rdr.Read())
                {
                    imagelst.Add(new profile
                    {
                        sponsorid = rdr["sponsor_id"].ToString(),
                        userid = rdr["user_id"].ToString(),
                        name = rdr["name"].ToString(),
                        address = rdr["address"].ToString() + ',' + rdr["city"].ToString() + ',' + rdr["state"].ToString(),
                        mobileno = rdr["mobileno"].ToString(),
                    });
                }
                rdr.Close();
                ViewBag.clientlist = imagelst;
                return View();
            }
            catch (Exception e1)
            {
                ViewBag.clientlist = null;
                return View();
            }
        }

    }
}
