using MTV.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using Newtonsoft.Json;
using System.Text.RegularExpressions;
using System.Text;
using System.Configuration;
using NLog;
using MTV.Models;

namespace MTV.Controllers
{
    public class HomeController : Controller
    {
        private Logger logger = LogManager.GetCurrentClassLogger();
        string LINK_API = ConfigurationManager.AppSettings["LINK_API"];
        // GET: Default
        public ActionResult Index()
        {
            return View();
        }
        public JsonResult CallApi(string username, string pass, string commandcode, string maqua, string address)
        {
            logger.Info(String.Format("[Call API]: username={0} pass={1} commandcode={2} maqua={3} address={4}", username, pass, commandcode, maqua, address));
            string msg = "Vui lòng nhập đủ thông tin.";
            username = username.Replace(" ", string.Empty);
            int point = 0;

            try
            {
                if (username.Length == 0)
                {
                    msg = "Vui lòng nhập đủ thông tin.";
                }
                else
                {//truong hop tich diem pass duoc dung de thay code, pass de trong
                    username = username.FormatPhonenumberStartWith84();//start phone with 84
                    if (username.Length > 11)//chan sdt 11 so
                    {
                        msg = "Số điện thoại đã được chuyển về 10 số, vui lòng kiểm tra lại.";
                    }
                    else if (commandcode.Equals("tichdiem"))
                    {
                        RootObject jsonResult = CallRequest(username, commandcode, pass, "", "");
                        msg = jsonResult.Result.message;
                    }
                    else if (commandcode.Equals("kiemtra"))//truong hop tra cuu code se duoc de trong
                    {
                        RootObject jsonResult = CallRequest(username, commandcode, "", "", "");
                        msg = jsonResult.Result.message;
                    }
                    else if (commandcode.Equals("doiqua"))//truong hop tra cuu code se duoc de trong
                    {
                        //kiemtra
                        //RootObject jsonResult = CallRequest(username, "kiemtra", "", "", "");
                         //point = Int32.Parse(jsonResult.Result.point);
                        //check qua
                        address = Common.ConvertTiengviet(address);
                        //RootObject jsonResultend = CallRequest(username, commandcode, "", maqua, address);
                        //msg = jsonResultend.Result.message;
                        if (maqua.Equals("H1"))
                        {
                            if (point < 6)
                            {
                                msg = "Bạn không đủ điểm.";
                            }
                            else
                            {
                                RootObject jsonResultend = CallRequest(username, commandcode, "", maqua, address);
                                msg = jsonResultend.Result.message;
                            }
                        }
                        else if (maqua.Equals("H2"))
                        {
                            if (point < 18)
                            {
                                msg = "Bạn không đủ điểm.";
                            }
                            else
                            {
                                RootObject jsonResultend = CallRequest(username, commandcode, "", maqua, address);
                                msg = jsonResultend.Result.message;
                            }
                        }
                        else if (maqua.Equals("H3"))
                        {
                            if (point < 18)
                            {
                                msg = "Bạn không đủ điểm.";
                            }
                            else
                            {
                                RootObject jsonResultend = CallRequest(username, commandcode, "", maqua, address);
                                msg = jsonResultend.Result.message;
                            }
                        }
                        else if (maqua.Equals("H4"))
                        {
                            if (point < 24)
                            {
                                msg = "Bạn không đủ điểm.";
                            }
                            else
                            {
                                RootObject jsonResultend = CallRequest(username, commandcode, "", maqua, address);
                                msg = jsonResultend.Result.message;
                            }
                        }
                        else
                        {
                            if (point < 36)
                            {
                                msg = "Bạn không đủ điểm.";
                            }
                            else
                            {
                                RootObject jsonResultend = CallRequest(username, commandcode, "", maqua, address);
                                msg = jsonResultend.Result.message;
                            }
                        }    

                    }
                }

            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                msg = "Hệ thống đang bận, xin vui lòng thử lại sau!";
                //return Json(new
                //{
                //    success = false,
                //    message = "Hệ thống xảy ra lỗi.",
                //}, JsonRequestBehavior.AllowGet);
            }
            return Json(new
            {
                success = true,
                message = msg
            }, JsonRequestBehavior.AllowGet);

        }
        public RootObject CallRequest(string username, string commandcode, string code, string maqua, string address)
        {
            // Nhãn hàng Minh Thông Vương
            string category = "MTV";
            // LINK_API = "http://sms.bluesea.vn:8000/SmsPortal/vbapi.jsp?";
            string linkrequest = LINK_API + "commandcode=" + commandcode + "&msisdn=" + username + "&code=" + code + "&category=" + category + "&MaQua=" + maqua + "&address=" + address;
            logger.Info("[Request]: " + linkrequest);
            WebRequest request = WebRequest.Create(linkrequest);
            request.Credentials = CredentialCache.DefaultCredentials;
            WebResponse response = request.GetResponse();
            Stream dataStream = response.GetResponseStream();
            StreamReader reader = new StreamReader(dataStream);
            string responseFromServer = reader.ReadToEnd();
            //xữ lý dữ liệu ở đây
            JavaScriptSerializer jss = new JavaScriptSerializer();
            // convert json string
            RootObject result = JsonConvert.DeserializeObject<RootObject>(responseFromServer);
            reader.Close();
            response.Close();
            logger.Info("[Response]: " + responseFromServer);
            //Utils.Common.WriteLogError(String.Format("request: {0}", linkrequest));
            //Utils.Common.WriteLogError(String.Format("response: {0}", responseFromServer));
            return result;
        }

    }
    //public class Result
    //{
    //    public string message { get; set; }
    //    public string point { get; set; }
    //    public string status { get; set; }
    //    public string msisdn { get; set; }
    //    public string code { get; set; }
    //}

    public class RootObject
    {
        public Result Result { get; set; }
    }


}