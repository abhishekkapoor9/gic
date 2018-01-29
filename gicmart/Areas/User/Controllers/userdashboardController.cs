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

namespace gicmart.Areas.User.Controllers
{
    [SessionExpire]
    public class userdashboardController : Controller
    {
        public class images
        {
            public string name { get; set; }
            public string id { get; set; }
            public string joindate { get; set; }

            public string referedname { get; set; }
        }
        //
        // GET: /User/userdashboard/
        [SessionExpire]
        public ActionResult Index()
        {
            int sponsorCount = 0;
            int sum = 180;
            int level = 1;
            try
            {
                List<images> imagelst = new List<images>();
                SqlDataReader rdr = null;
                TempData["userId"] = System.Web.HttpContext.Current.Session["userId"];
                TempData["userName"] = System.Web.HttpContext.Current.Session["userName"];
                ViewBag.sectionName = "Dashboard";
                string cs = ConfigurationManager.ConnectionStrings["cs"].ConnectionString;
                SqlConnection con = new SqlConnection(cs);
                con.Open();
                string usersp1 = "sp_getDirectSponsorsCount";
                SqlCommand cmd3 = new SqlCommand(usersp1, con);
                cmd3.CommandType = CommandType.StoredProcedure;
                cmd3.Parameters.AddWithValue("@empid", System.Web.HttpContext.Current.Session["userId"]);
                rdr = cmd3.ExecuteReader();
                while (rdr.Read())
                {
                    sponsorCount = Convert.ToInt32(rdr["SponsorCount"].ToString());
                    sum = 180 + (sponsorCount * 180);
                    if (sponsorCount >= 0 && sponsorCount <= 7)
                        level = 1;
                    if (sponsorCount > 7 && sponsorCount <= 49)
                        level = 2;
                    if (sponsorCount > 49 && sponsorCount < 343)
                        level = 3;
                    if (sponsorCount >= 343 && sponsorCount < 2401)
                        level = 4;
                    if (sponsorCount >= 2401 && sponsorCount < 16807)
                        level = 5;
                    if (sponsorCount >= 16807 && sponsorCount < 117649)
                        level = 6;
                    if (sponsorCount >= 117649 && sponsorCount < 823543)
                        level = 7;
                    if (sponsorCount >= 823543 && sponsorCount < 5764801)
                        level = 8;
                }
                ViewBag.SponsorCount = sponsorCount;
                ViewBag.sum = sum;
                ViewBag.level = level;
                rdr.Close();
                string usersp2 = "sp_getclientList";
                SqlCommand cmd4 = new SqlCommand(usersp2, con);
                cmd4.CommandType = CommandType.StoredProcedure;
                cmd4.Parameters.AddWithValue("@empid", System.Web.HttpContext.Current.Session["userId"]);
                cmd4.Parameters.AddWithValue("@assigneddatetime", Convert.ToDateTime(System.Web.HttpContext.Current.Session["joindate"]));
                //cmd4.ExecuteNonQuery(); // MISSING
                //getting reference_user_Id
                rdr = cmd4.ExecuteReader();

                // iterate through results, printing each to console
                while (rdr.Read())
                {
                    imagelst.Add(new images
                    {
                        id = rdr["user_id"].ToString(),
                        name = rdr["name"].ToString(),
                        joindate = rdr["assignedDate"].ToString(),
                        referedname= rdr["Refered_Name"].ToString()

                    });
                }
                ViewBag.clientlist = imagelst;
                rdr.Close();
            }
            catch (Exception e1)
            {
                //reference_user_id = null;
            }

            return View();
        }
    }
}

