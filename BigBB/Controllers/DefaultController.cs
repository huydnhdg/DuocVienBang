using BigBB.Utils;
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

namespace BigBB.Controllers
{
    public class DefaultController : Controller
    {
        string LINK_API = ConfigurationManager.AppSettings["LINK_API"];
        // GET: Default
        public ActionResult Index()
        {
            return View();
        }
        public JsonResult CallApi(string username, string pass, string commandcode, string maqua, string address)
        {
            Utils.StringExtension.WriteLogError(String.Format("call api: {0} {1} {2} {3} {4}", username, pass, commandcode, maqua, address));
            //address = ConvertTiengviet(address);
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
                        return Json(new
                        {
                            success = true,
                            message = msg
                        }, JsonRequestBehavior.AllowGet);
                    }
                    if (commandcode.Equals("tichdiem"))
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
                        RootObject jsonResult = CallRequest(username, "kiemtra", "", "", "");
                        point = Int32.Parse(jsonResult.Result.point);
                        //check qua
                        address = ConvertTiengviet(address);
                        if (maqua.Equals("F1") || maqua.Equals("F2"))
                        {
                            if (point < 3)
                            {
                                msg = "Bạn không đủ điểm.";
                            }
                            else
                            {
                                RootObject jsonResultend = CallRequest(username, commandcode, "", maqua, address);
                                msg = jsonResultend.Result.message;
                            }
                        }
                        else if (maqua.Equals("F3") || maqua.Equals("F4"))
                        {
                            if (point < 4)
                            {
                                msg = "Bạn không đủ điểm.";
                            }
                            else
                            {
                                RootObject jsonResultend = CallRequest(username, commandcode, "", maqua, address);
                                msg = jsonResultend.Result.message;
                            }
                        }
                        else if (maqua.Equals("F5") || maqua.Equals("F6") || maqua.Equals("F7")|| maqua.Equals("F8"))
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
                        else if (maqua.Equals("F9") || maqua.Equals("F10"))
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
                    }
                }
                return Json(new
                {
                    success = true,
                    message = msg
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    success = false,
                    message = "Hệ thống xảy ra lỗi.",
                }, JsonRequestBehavior.AllowGet);
            }

        }
        public RootObject CallRequest(string username, string commandcode, string code, string maqua, string address)
        {

            string category = "";//category khac GOLD "&category!=" + category

            string linkrequest = LINK_API + "&commandcode=" + commandcode + "&msisdn=" + username + "&code=" + code + "&category=" + category + "&MaQua=" + maqua + "&address=" + address;
            WebRequest request = WebRequest.Create(linkrequest);
            //WebRequest request = WebRequest.Create(
            // "http://sms.bluesea.vn:8080/SmsPortal/vbapi.jsp?username=" + uname + "&pass=" + "" + "&commandcode=" + commandcode + "&msisdn=" + username + "&code=" + code);
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
            Utils.StringExtension.WriteLogError(String.Format("request: {0}", linkrequest));
            Utils.StringExtension.WriteLogError(String.Format("response: {0}", responseFromServer));
            return result;
        }
        public string ConvertTiengviet(string s)
        {
            Regex regex = new Regex("\\p{IsCombiningDiacriticalMarks}+");
            string temp = s.Normalize(NormalizationForm.FormD);
            return regex.Replace(temp, String.Empty).Replace('\u0111', 'd').Replace('\u0110', 'D');
        }
    }
    public class Result
    {
        public string message { get; set; }
        public string point { get; set; }
        public string status { get; set; }
        public string msisdn { get; set; }
        public string code { get; set; }
    }

    public class RootObject
    {
        public Result Result { get; set; }
    }


}