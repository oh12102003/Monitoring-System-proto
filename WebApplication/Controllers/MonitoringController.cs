﻿using DataStreamType;
using MySql.Data.MySqlClient;
using System.Collections.Generic;
using System.Data;
using System.Web.Mvc;

namespace WebApplication.Controllers
{
    public class MonitoringController : Controller
    {
        // GET: Monitoring
        public ActionResult Index()
        {
            string userId = Session["userId"] as string;

            if (userId == null)
            {
                Session["messageDisplay"] = "true";
                Session["message"] = "로그인 이후에 사용할 수 있습니다.";
                Session["messageType"] = "info";
                Session["redirect"] = Url.Content("~/Home");
                return RedirectToAction("Index", "Home");
            }

            DataBaseModuleFactory dbFactory = DataBaseModuleFactory.getInstance();
            IDataBase db = dbFactory.getDataBaseModule(dataBaseType.MySql);

            dbFactory = DataBaseModuleFactory.getInstance();

            try
            {
                db.connect();

                DataTable dt = new DataTable();
                string checkQuery = "select * from userInfo where userId=@userId";

                List<MySqlParameter> queryData = new List<MySqlParameter>();
                queryData.Add(new MySqlParameter("userId", userId));

                int count = db.inquire(ref dt, checkQuery, queryData);

                if (count == 1)
                {
                    if (Session["monitoring"].Equals("true"))
                    {
                        return View();
                    }

                    else
                    {
                        Session["messageDisplay"] = "true";
                        Session["message"] = "권한이 없습니다.";
                        Session["messageType"] = "warning";
                        Session["redirect"] = Url.Content("~/Home");
                        return RedirectToAction("Messaging", "Shared");
                    }
                }

                else
                {
                    Session["messageDisplay"] = "true";
                    Session["message"] = "잘못된 권한 요청입니다.";
                    Session["messageType"] = "danger";
                    Session["redirect"] = Url.Content("~/Home");
                    return RedirectToAction("Messaging", "Shared");
                }
            }

            catch
            {
                Session["messageDisplay"] = "true";
                Session["message"] = "에러가 발생하였습니다.";
                Session["messageType"] = "danger";
                Session["redirect"] = Url.Content("~/Home");
                return RedirectToAction("Messaging", "Shared");
            }
        }
    }
}