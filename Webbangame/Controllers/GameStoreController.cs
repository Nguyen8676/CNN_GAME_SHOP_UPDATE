using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Webbangame.Models;

namespace Webbangame.Controllers
{
    public class GameStoreController : Controller
    {
        // tạo 1 đối tượng chứa toàn bộ dữ liệu database
           dbQLBangameDataContext data = new dbQLBangameDataContext();
        private List<GAME> laygamemoi(int count)
        {
            //sắp xếp 
            return data.GAMEs.OrderByDescending(a => a.NgayCapNhat).Take(count).ToList();
        }
        // GET: GameStore
        public ActionResult Index()
        {
            //lấy 5 game mới 
            var game_moi = laygamemoi(5);
            return View(game_moi);
        }
        public ActionResult TheLoai()
        {
            var theloai = from tl in data.THE_LOAIs select tl;
            return PartialView(theloai);
        }
        public ActionResult NhaPhatTrien()
        {
            var nhaphattrien = from npt in data.NHA_PHAT_TRIENs select npt;
            return PartialView(nhaphattrien);

        }
        public ActionResult NhaPhatHanh()
        {
            var nhaphathanh = from nph in data.NHA_PHAT_HANHs select nph;
            return PartialView(nhaphathanh);
        }
        public ActionResult SPTheotheloai(string a)
        {
            var game = from g in data.GAMEs where g.MaTheLoai==a select g;
            return View(game);
        }
        public ActionResult SPTheonhaphathanh(int id)
        {
            
            var game = from s in data.GAMEs where s.MaNPH==id select s;
            return View(game);
        }
        public ActionResult SPTheonhaphattrien(int id)
        {
            var game = from g in data.GAMEs where g.MaNPT == id select g;
            return View(game);
        }
        public ActionResult Details(int id)
        {
            var game = from d in data.GAMEs
                       where d.MaGame == id
                       select d;
            return View(game.Single());
        }
        public ActionResult LienHe()
        {
            var lienhe = from lh in data.LIEN_HEs select lh;
            return PartialView(lienhe);
        }
        public ActionResult LoGo()
        {
            var logo = from lg in data.AVARTAs select lg;
            return PartialView(logo);
        }

    }

    
}