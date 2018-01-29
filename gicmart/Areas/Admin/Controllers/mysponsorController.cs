using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using gicmart.Models;
using gicmart.Areas.Admin.Filters;
using gicmart.Areas.Admin.Models;

namespace gicmart.Areas.Admin.Controllers
{
    [SessionExpire]
    public class mysponsorController : Controller
    {
        //
        // GET: /Admin/mysponsor/

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
                        address = rdr["address"].ToString()+','+ rdr["city"].ToString()+','+ rdr["state"].ToString(),
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
