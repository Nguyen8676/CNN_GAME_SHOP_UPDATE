using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Webbangame.Models;


namespace Webbangame.Controllers
{
 
    public class NguoidungController : Controller
    {
        dbQLBangameDataContext data = new dbQLBangameDataContext();
        // GET: Nguoidung
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public ActionResult Dangky() //get đăng ký
        {
            return View();
        }
        //POST: hàm  đk  nhận dữ liệu  từ trang đăng kí và thực hiện việc tạo mới dữ liệu
        [HttpPost]
        public ActionResult Dangky(FormCollection collection,KHACHHANG kh)
        {
            var hoten = collection["HoTenKH"];
            var tendn = collection["TenDN"];
            var matkhau = collection["Matkhau"];
            var matkhaunhaplai = collection["Matkhaunhaplai"];
            var diachi = collection["Diachi"];
            var email = collection["Email"];
            var dienthoai = collection["Dienthoai"];
            var ngaysinh = String.Format("{0:MM/dd/yyyy}", collection["Ngaysinh"]);
            if(String.IsNullOrEmpty(hoten))
            {
                ViewData["Loi1"] = "Họ tên khách hàng không được để trống";

            }
            else if(String.IsNullOrEmpty(tendn))
            {
                ViewData["Loi2"] = "phải nhập tên đăng nhập";
            }
            else if (String.IsNullOrEmpty(matkhau))
            {
                ViewData["Loi3"] = "phải nhập mật khẩu";
            }
            else if (String.IsNullOrEmpty(matkhaunhaplai))
            {
                ViewData["Loi4"] = "phải nhập lại mật khẩu";
            }
             if (String.IsNullOrEmpty(email))
            {
                ViewData["Loi5"] = "Email không được bỏ trống";
            }
            if (String.IsNullOrEmpty(dienthoai))
            {
                ViewData["Loi6"] = "phải nhập điện thoại";
            }
            else
            {
                //gán giá trị cho đối tượng được tạo mới(kh)

                kh.HoTen = hoten;
                kh.TaiKhoan = tendn;
                kh.MatKhau = matkhau;
                kh.EMAIL = email;
                kh.DiaChi = diachi;
                kh.DT = dienthoai;
                kh.NgaySinh = DateTime.Parse(ngaysinh);
                data.KHACHHANGs.InsertOnSubmit(kh);
                data.SubmitChanges();
                return RedirectToAction("DangNhap");
               
            }
            return this.Dangky();

        }
        [HttpGet]
        //Get đăng nhập
        public ActionResult Dangnhap()
        {
            return View();
        }
        //post đăng nhập
        [HttpPost]
        public ActionResult Dangnhap(FormCollection collection)
        {
            // gán giá trị người dùng nhập dữ liệu cho các biến
            var tendn = collection["TenDN"];
            var matkhau = collection["Matkhau"];
            if(String.IsNullOrEmpty(tendn))
            {
                ViewData["Loi1"] = "phải nhập tên đn";
            }
            else if(String.IsNullOrEmpty(matkhau))
            {
                ViewData["Loi2"] = "Phải nhập mật khẩu";
            }
            else
            {
                // gán giá trị cho đối tượng được tạo mới (kh)
                KHACHHANG kh = data.KHACHHANGs.SingleOrDefault(n => n.TaiKhoan == tendn && n.MatKhau == matkhau);
                if(kh != null)
                {
                    ViewBag.Thongbao = " Chúc mừng đăng nhập thành công";
                    Session["Taikhoan"] = kh;
                    return RedirectToAction("Index", "GameStore");

                }
                else
                {
                    ViewBag.Thongbao = "tên đăng nhập hoặc mật khẩu không đúng";
                }

            }
            return View();
        }
    }
}