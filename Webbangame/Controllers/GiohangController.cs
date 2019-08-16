using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Webbangame.Models;

namespace Webbangame.Controllers
{
    public class GiohangController : Controller
    {
        dbQLBangameDataContext data = new dbQLBangameDataContext();
        // GET: Giohang
        public List<Giohang>Laygiohang()
        {
            List<Giohang> lstGiohang = Session["Giohang"] as List<Giohang>;
            if (lstGiohang==null)
            {
                //Nếu giỏ hàng chưa tồn tại thì khởi tạo listGiohang
                lstGiohang = new List<Giohang>();
                Session["Giohang"] = lstGiohang;
            }
            return lstGiohang;
        }
        // thêm hàng vào giỏ
        public ActionResult ThemGiohang(int iMaGame,string strURL)
        {
            //Lay ra Session gio hang
            List<Giohang> lstGiohang = Laygiohang();
            //kiểm tra sách này tồn tại trong Sesion["Giohang"] chưa?
            Giohang sanpham = lstGiohang.Find(n => n.iMaGame == iMaGame);
                if(sanpham == null)
                {
                    sanpham = new Giohang(iMaGame);
                    lstGiohang.Add(sanpham);
                    return Redirect(strURL);
                }
                else
                {
                    sanpham.iSoluong++;
                    return Redirect(strURL);
                }
        }
        //Tổng số lượng
        private int TongSoLuong()
        {
            int iTongSoLuong = 0;
            List<Giohang> lstGiohang=Session["GioHang"] as List<Giohang>;
            if(lstGiohang!=null)
            {
                iTongSoLuong = lstGiohang.Sum(n => n.iSoluong);
            }
            return iTongSoLuong;
        }
        //Tổng tiền
        private double TongTien()
        {
            double iTongTien = 0;
            List<Giohang> lstGiohang = Session["GioHang"] as List<Giohang>;
            if (lstGiohang != null)
            {
                iTongTien = lstGiohang.Sum(n => n.dThanhtien);
            }
            return iTongTien;
        }
        //Xây dựng trang giỏ hàng
        public ActionResult GioHang()
        {
            List<Giohang> lstGiohang = Laygiohang();
            if(lstGiohang.Count == 0)
            {
                return RedirectToAction("Index", "GameStore");
            }
            ViewBag.Tongsoluong = TongSoLuong();
            ViewBag.Tongtien = TongTien();
            return View(lstGiohang);
        }
        public ActionResult GiohangPartial()
        {
            ViewBag.Tongsoluong = TongSoLuong();
            ViewBag.Tongtien = TongTien();
            return PartialView();
        }
        public ActionResult XoaGiohang(int iMaSP)
        {
            //Lấy giỏ hàng từ session
            List<Giohang> lstGiohang = Laygiohang();
            //Kiểm tra game đã có trong session Giohang
            Giohang sanpham = lstGiohang.SingleOrDefault(n => n.iMaGame == iMaSP);
            //Nếu tồn tại thif cho sửa số lượng
            if(sanpham!=null)
            {
                lstGiohang.RemoveAll(n => n.iMaGame == iMaSP);
                return RedirectToAction("GioHang");

            }
            if(lstGiohang.Count == 0)
            {
                return RedirectToAction("Index", "GameStore");
            }
            return RedirectToAction("GioHang");
        }
        //cap nhat gio hang
        public ActionResult CapnhatGiohang(int iMaSP,FormCollection f)
        {
            //Lay gio hang session
            List<Giohang> lstGiohang = Laygiohang();
            //Kiểm tra game đã có trong session Giohang
            Giohang sanpham = lstGiohang.SingleOrDefault(n => n.iMaGame == iMaSP);
            //nếu tồn tại thì sửa số lượng
            if(sanpham !=null)
            {
                sanpham.iSoluong = int.Parse(f["txtSoluong"].ToString());
            }
            return RedirectToAction("GioHang");
        }
        public ActionResult XoaTatcaGiohang()
        {
            //Lay gio hang session
            List<Giohang> lstGiohang = Laygiohang();
            lstGiohang.Clear();
            return RedirectToAction("Index", "GameStore");

        }
        //Hien thi View DatHang de cap nhat  cac thong tin  cho Don hang
        [HttpGet]
        public ActionResult DatHang()
        {
            //kiem tra dang nhap
            if(Session["Taikhoan"] == null || Session["Taikhoan"].ToString() == "")
            {
                return RedirectToAction("Dangnhap", "Nguoidung");
            }
            if(Session["Giohang"]== null)
            {
                return RedirectToAction("Index", "GameStore");
            }
            //Lay gio hang tu Session
            List<Giohang> lstGiohang = Laygiohang();
            ViewBag.Tongsoluong = TongSoLuong();
            ViewBag.Tongtien = TongTien();

            return View(lstGiohang); 
        }

        [HttpPost]
        public ActionResult DatHang(FormCollection collection)
        {
            //them don hang
            DON_HANG ddh = new DON_HANG();
            KHACHHANG kh = (KHACHHANG)Session["Taikhoan"];
            List<Giohang> gh = Laygiohang();
            ddh.MaKH = kh.MaKH;
            ddh.NgayDat = DateTime.Now;
            var ngaygiao = String.Format("{0:MM/dd/yyyy}", collection["Ngaygiao"]);
            ddh.Ngaygiao = DateTime.Parse(ngaygiao);
            ddh.Tinhtranggiaohang = false;
            ddh.Dathanhtoan = false;
            data.DON_HANGs.InsertOnSubmit(ddh);
            data.SubmitChanges();
            
            //Them chi tiet don hang
            foreach(var item in gh)
            {
                CT_DONHANG ctdh = new CT_DONHANG();
                ctdh.MaDH = ddh.MaDH;
                ctdh.MaGame = ddh.MaGame;
                ctdh.SoLuong = item.iSoluong;
                ctdh.Dongia = (decimal)item.dDongia;
                data.CT_DONHANGs.InsertOnSubmit(ctdh);

            }
            data.SubmitChanges();
            Session["Giohang"] = null;
            return RedirectToAction("Xacnhandonhang", "Giohang");
        }
        public ActionResult Xacnhandonhang()
        {
            return View();
        }

        public ActionResult Index()
        {
            return View();
        }

    }
}