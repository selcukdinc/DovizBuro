using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Doviz_Ofisi
{
    internal class MathAccessClass
    {
        TextBox _tbKur, _tbMiktar, _tbTutar, _tbKalan;
        public MathAccessClass()
        {

        }

        public MathAccessClass(TextBox tbKur, TextBox tbMiktar, TextBox tbTutar, TextBox tbKalan)
        {
            _tbKur = tbKur;
            _tbMiktar = tbMiktar;
            _tbTutar = tbTutar;
            _tbKalan = tbKalan;
        }

        DataAccessClass dac = new DataAccessClass();
        RegexAccessClass rac = new RegexAccessClass();
        Random R = new Random();
        // Kullanıcı Tl - Dolar veya Dolar - Tl dönüşümleri yapacak
        // Kasadan Çıkacak değer, 

        public bool CaseOut(string Code, double CaseOutValue) // Kasa Kullanıcıdan döviz satın alacağı zaman, kasasında yeterli miktarda ödeme yapabileceği hacim olmalı
        {// kasadan hangi kodlu, ne kadar birim çıkacak, 
            if (dac.GetValue(Code) >= CaseOutValue) // kasadaki değer çıkacak değerden büyük veya eşitse
                return true;
            else
                return false;
        }

        enum alpha
        {
            A, B, C, D, E, F, G, H, I, J, K, L, M, N, O, R, S, T, U, V, Y, Z
        }

        public string rndPin()
        {// 0000
            return $"{R.Next(1000, 10000)}";
        }

        public string rndRef()
        {//AAA000AAA000
            return $"{(alpha)R.Next(0,22)}{(alpha)R.Next(0, 22)}{(alpha)R.Next(0, 22)}{R.Next(100,1000)}{(alpha)R.Next(0, 22)}{(alpha)R.Next(0, 22)}{(alpha)R.Next(0, 22)}{R.Next(100, 1000)}";
        }

        public double AlisHesapla()
        {// Yalnızca miktar döndür
            double kur = -1, miktar = 1, tutar;
            if (rac.CheckTb(_tbKur) && rac.CheckTb(_tbMiktar))
            {

                kur = Convert.ToDouble(_tbKur.Text);
                miktar = Convert.ToDouble(_tbMiktar.Text);
                tutar = kur * miktar;
                _tbTutar.Text = tutar.ToString();
            }
            else
            {
                MessageBox.Show("Kur türnüzü ve Miktar değerini giriniz");

            }
            return kur * miktar;
        }

        public bool AlisHesaplaState()
        {
            double kur = -1, miktar = 1, tutar;
            if (rac.CheckTb(_tbKur) && rac.CheckTb(_tbMiktar))
            {

                kur = Convert.ToDouble(_tbKur.Text);
                miktar = Convert.ToDouble(_tbMiktar.Text);
                tutar = kur * miktar;
                _tbTutar.Text = tutar.ToString();
                return true;
            }
            else
            {
                MessageBox.Show("Kur türnüzü ve Miktar değerini giriniz");
                return false;
            }

        }

        public double SatisHesapla()
        {
            double result = -1;
            if (rac.CheckTb(_tbKur) && rac.CheckTb(_tbMiktar))
            {
                double kur = Convert.ToDouble(_tbKur.Text);
                int miktar = Convert.ToInt32(_tbMiktar.Text);
                int tutar = Convert.ToInt32(Math.Floor(Convert.ToDouble(miktar / kur)));
                _tbTutar.Text = tutar.ToString();
                double kalan = miktar - tutar * kur;
                _tbKalan.Text = $"{Math.Floor(kalan)}";
                result = miktar;
            }
            else
            {
                MessageBox.Show("Kur türnüzü ve Miktar değerini giriniz");
            }
            return result;
        }

        public bool SatisHesaplaState()
        {
            double result = -1;
            if (rac.CheckTb(_tbKur) && rac.CheckTb(_tbMiktar))
            {
                double kur = Convert.ToDouble(_tbKur.Text);
                int miktar = Convert.ToInt32(_tbMiktar.Text);
                int tutar = Convert.ToInt32(Math.Floor(Convert.ToDouble(miktar / kur)));
                _tbTutar.Text = tutar.ToString();
                double kalan = miktar - tutar * kur;
                result = tutar;
                _tbKalan.Text = $"{Math.Floor(kalan)}";
                return true;
            }
            else
            {
                MessageBox.Show("Kur türnüzü ve Miktar değerini giriniz");
                return false;
            }
        }

        public double TutarHesapla()
        {
            double result = -1;
            if (rac.CheckTb(_tbKur) && rac.CheckTb(_tbMiktar))
            {
                double kur = Convert.ToDouble(_tbKur.Text);
                int miktar = Convert.ToInt32(_tbMiktar.Text);
                int tutar = Convert.ToInt32(Math.Floor(Convert.ToDouble(miktar / kur)));
                _tbTutar.Text = tutar.ToString();
                double kalan = miktar - tutar * kur;
                _tbKalan.Text = $"{Math.Floor(kalan)}";
                result = tutar;
            }
            else
            {
                MessageBox.Show("Kur türnüzü ve Miktar değerini giriniz");
            }
            return result;
        }

        public double TRYMiktar()
        {
            double result = -1;
            if (rac.CheckTb(_tbKur) && rac.CheckTb(_tbMiktar))
            {
                result = Convert.ToInt32(_tbMiktar.Text);
            }
            else
            {
                MessageBox.Show("Kur türnüzü ve Miktar değerini giriniz");
            }
            return result;
        }
    }
}
