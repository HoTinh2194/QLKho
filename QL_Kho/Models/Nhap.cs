using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using QL_Kho.Models;

namespace QL_Kho.Models
{
    public class Nhap
    {
        KhohangDataContext kho = new KhohangDataContext();
        public int iMaMH { get; set; }
        public string sTenMH { get; set; }
        
        public Double dDonggia { get; set; }
        public int iSoluong { get; set; }
        public Double dThanhtien { get { return iSoluong * dDonggia; } }

        public Nhap(int MaMH)
        {
            iMaMH = MaMH;
            Mathang MH = kho.Mathangs.Single(n => n.MaMH == iMaMH);
            sTenMH = MH.TenMH;
            dDonggia = double.Parse(MH.Dongia.ToString());
            iSoluong = 1;

        }
    }
}