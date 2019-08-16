using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;



namespace Webbangame.Models
{
    public class Giohang
    {
        //tạo đối tượng chứa toàn bộ dữ liệu database
        dbQLBangameDataContext data = new dbQLBangameDataContext();
        public int iMaGame { set; get; }
        public string sTenGame { set; get; }
        public string sAnhbia { set; get; }
        public Double dDongia { set; get; }
        public int iSoluong { set; get; }
        public Double dThanhtien 
        {
            get { return iSoluong * dDongia; }
        }
        //khởi tạo giỏ hàng theo magame được truyền vào với so lượng mặc định là 1
        public Giohang(int MaGame)
        {
            iMaGame = MaGame;
            GAME game = data.GAMEs.Single(n => n.MaGame == iMaGame);
            sTenGame = game.TenGame;
            sAnhbia = game.Anhbia;
            dDongia = double.Parse(game.GiaBan.ToString());
            iSoluong = 1;
        }
    }
}