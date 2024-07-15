using CSharpApp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Doviz_Ofisi
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            ssHider.BringToFront();
            tcMain.SelectedTab = tpGiris;
            Sifirla();
        }

        /*
        
            TODO : Kullnaıcı alış biriminde satış yapamasın ✅
            TODO : Buton text - name uyumu sağla ✅
            TODO : Döviz butonlarını tek bir metoda indirge ✅
            TODO : İşlem geçmişini sakla ✅
            TODO : Kasa bilgilerini tek satırda tut ve bilgileri sakla ✅
            TODO : Sql komutlarını başka bir sınıfta çalıştır  ✅
            ⭐TODO⭐
                listbox veya başka bir nesne içersinde birden fazla döviz işlemini kolay bir şekilde çekmeye çalış
            ⭐TODO⭐
                bütün çevrim ve atama işlemlerini başka bir sınıfta yap 

         */
        DataAccessClass dac = new DataAccessClass();
        XmlAccessClass xac = new XmlAccessClass();
        RegexAccessClass rac = new RegexAccessClass();
        MathAccessClass mac;
        EmailSenderClass esc = new EmailSenderClass();
        TimeSpan TodayTime;

        private string currentUsername = "";
        enum Dovizler
        {
            Dollar, Euro, Null
        }
        
        Dovizler doviz = Dovizler.Null;
        // TableNames
        // tblCase
        // tblHistory
  
        private void Form1_Load(object sender, EventArgs e)
        {
            mac = new MathAccessClass(tbKur, tbMiktar, tbTutar, tbKalan);
        }

       

        private void tcMain_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tcMain.SelectedTab == tpGiris)
            {
                Sifirla();
            }else if (tcMain.SelectedTab == tpYeniKullanici)
            {
                
            }else if (tcMain.SelectedTab == tpSifreYenile)
            {
                
            }else if (tcMain.SelectedTab == tpDovizBuro)
            {
                lblDolarAlis.Text = xac.BanknoteBuying("USD");
                lblDolarSatis.Text = xac.BanknoteSelling("USD");
                lblEuroAlis.Text = xac.BanknoteBuying("EUR");
                lblEuroSatis.Text = xac.BanknoteSelling("EUR");

                dac.PrintDgv("tblCase", dgvCase);
                dac.PrintDgv("tblHistory", dgvHistory);
                dgvCase.Columns[0].Width = 35;
                dgvHistory.Columns[0].Width = 35;
                dgvHistory.Columns[3].Width = 150;

                lblKullaniciAdi.Text = $"Operatör : {currentUsername}";
            }
            else if (tcMain.SelectedTab == tpSuperAdmin)
            {
                dac.PrintDgv("tblCase", dgvSuperCase);
                dac.PrintDgv("tblStaff", dgvSuperStaff, "ID", "KullaniciAdi", "Eposta");
                currentUserId = string.Empty;
            }
        }

        private void Sifirla()
        {
            currentUsername = "";
            currentUserId = string.Empty;
            gbYKEpostaKontrol.Visible = false;
            gbSYPinKontrol.Visible = false;
            gbSYYeniParolaBelirle.Visible = false;
            cbSuperCode.SelectedIndex = 0;
            foreach (TextBox textBox in GetAllControls(this).OfType<TextBox>())
            {
                textBox.Text = string.Empty;
                textBox.Enabled = true;
            }
            Console.WriteLine("sıfırla metudu çalıştı");
            foreach (DataGridView item in GetAllControls(this).OfType<DataGridView>())
            {
                item.DataSource = null;
            }

            foreach (CheckBox item in GetAllControls(this).OfType<CheckBox>())
            {
                item.Checked = true;
                item.Checked = false;
            }
        }

        

        private IEnumerable<Control> GetAllControls(Control parent)
        {
            var controls = parent.Controls.Cast<Control>();

            return controls.SelectMany(ctrl => GetAllControls(ctrl)).Concat(controls);
        }

        private void btnArayüzKontrol(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            if (btn.Name == btnGParolaSifirla.Name)
            {
                tcMain.SelectedTab = tpSifreYenile;
            }
            else if (btn.Name == btnGYeniKullanici.Name)
            {
                tcMain.SelectedTab = tpYeniKullanici;
            }
            else if (btn.Name == btnDBOturumuKapat.Name)
            {
                tcMain.SelectedTab = tpGiris;
                
            }
            else if (btn.Name == btnSAOturumuKapat.Name)
            {
                tcMain.SelectedTab = tpGiris;
            }
            else if (btn.Name == btnYKGeriDon.Name)
            {
                tcMain.SelectedTab = tpGiris;
                
            }
            else if (btn.Name == btnSYGeriDon.Name)
            {
                tcMain.SelectedTab = tpGiris;
            }
        }

        private void cbPass_CheckStateChanged(object sender, EventArgs e)
        {
            CheckBox cb = (CheckBox)sender;
            if (cb.Name == cbGPass.Name)
            {
                if (cb.CheckState == CheckState.Checked)
                {
                    cb.ImageIndex = 0;
                    tbGParola.UseSystemPasswordChar = false;
                }
                else
                {
                    cb.ImageIndex = 1;
                    tbGParola.UseSystemPasswordChar = true;
                }
            }else if (cb.Name == cbYKPass.Name)
            {
                if (cb.CheckState == CheckState.Checked)
                {
                    cb.ImageIndex = 0;
                    tbYKParola.UseSystemPasswordChar = false;
                    tbYKParolaTekrar.UseSystemPasswordChar = false;
                }
                else
                {
                    cb.ImageIndex = 1;
                    tbYKParola.UseSystemPasswordChar = true;
                    tbYKParolaTekrar.UseSystemPasswordChar = true;
                }
            }else if (cb.Name == cbSYPass.Name)
            {
                if (cb.CheckState == CheckState.Checked)
                {
                    cb.ImageIndex = 0;
                    tbSYParola.UseSystemPasswordChar = false;
                    tbSYParolaTekrar.UseSystemPasswordChar = false;
                }
                else
                {
                    cb.ImageIndex = 1;
                    tbSYParola.UseSystemPasswordChar = true;
                    tbSYParolaTekrar.UseSystemPasswordChar = true;
                }
            }
        }

        /*

            Dolar alış butonuna basıldığında
                Kur --> düşük olan sayı gelmeli
                [YeniDOLAR * KUR] durumu Kasadaki TRY'ı karşılıyor mu onu kontrol et
                Kasa TRY = KasaTRY - Miktar;
                Kasa DOLAR = KasaDOLAR + (Miktar / KUR);

            Dolar satış butonuna basıldığında
                kur --> yüksek olan sayı gelmeli
                Satılmak istenen dolar Miktarı  Kasadaki USD'y ikarşılıyor mu kontrol et
                Kasa TRY = KasaTRY + (YeniDolar * Kur)
                kasa Dolar = KasaDolar - YeniDolar;

         */


        private void btnSatisVeAlis(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            
            if (btn.Name == btnDolarAlis.Name)
            {
                tbKur.Text = lblDolarAlis.Text;
                doviz = Dovizler.Dollar;
                cbAlis.CheckState = CheckState.Unchecked;
                cbAlis.CheckState = CheckState.Checked;
                
            }
            else if(btn.Name == btnDolarSatis.Name)
            {
                tbKur.Text = lblDolarSatis.Text;
                doviz = Dovizler.Dollar;
                cbAlis.CheckState = CheckState.Checked;
                cbAlis.CheckState = CheckState.Unchecked;
                
            }
            else if (btn.Name == btnEuroAlis.Name)
            {
                tbKur.Text = lblEuroAlis.Text;
                doviz = Dovizler.Euro;
                cbAlis.CheckState = CheckState.Unchecked;
                cbAlis.CheckState = CheckState.Checked;
                
            }
            else if (btn.Name == btnEuroSatis.Name)
            {
                tbKur.Text = lblEuroSatis.Text;
                doviz = Dovizler.Euro;
                cbAlis.CheckState = CheckState.Checked;
                cbAlis.CheckState = CheckState.Unchecked;
                
            }

        }

        private void cbAlis_CheckStateChanged(object sender, EventArgs e)
        {
            if (cbAlis.CheckState != CheckState.Indeterminate)
            {
                if (cbAlis.CheckState == CheckState.Checked)
                {
                    btnAlis.Enabled = true;
                    btnAlis.ForeColor = Color.LightGreen;
                    
                    btnSatis.Enabled = false;
                    btnSatis.ForeColor = Color.Black;
                    
                    btnHesapla.Text = "Hesapla (Alış)";

                    lblTutar.Text = "Tutar (TRY): ";

                    if (doviz == Dovizler.Dollar)
                        lblMiktar.Text = "Miktar (USD): ";
                    else if (doviz == Dovizler.Euro)
                        lblMiktar.Text = "Miktar (EUR): ";


                    
                }
                else
                {
                    btnAlis.Enabled = false;
                    btnAlis.ForeColor = Color.Black;
                    
                    btnSatis.Enabled = true;
                    btnSatis.ForeColor = Color.LightGreen;
                    
                    btnHesapla.Text = "Hesapla (Satış)";

                    lblMiktar.Text = "Miktar (TRY): ";

                    if (doviz == Dovizler.Dollar)
                        lblTutar.Text = "Tutar (USD): ";
                    else if (doviz == Dovizler.Euro)
                        lblTutar.Text = "Tutar (EUR): ";
                }
            }
        }
        /*
            
            Dolar alış butonuna basıldığında Aslında kullanıcı elindeki USD birimini TRY birimine çevirmek istiyor. Bu durumda kasada yeterli TRY olup olmadığını kontrol etmemiz gerekiyor
                Kur --> düşük olan sayı gelmeli ✅
                [Miktar * KUR] durumu Kasadaki TRY birimini karşılıyor mu onu kontrol et
                Kasa TRY = KasaTRY - (Miktar * Kur);
                Kasa DOLAR = KasaDOLAR + Mitkar ;
         
         */
        private void btnIslem1_Click(object sender, EventArgs e) // btnAlis
        {
            if (rac.CheckTb(tbKur) && rac.CheckTb(tbMiktar))
            {
                double kur, miktar, tutar;
                kur = Convert.ToDouble(tbKur.Text.Replace('.', ','));
                miktar = Convert.ToDouble(tbMiktar.Text);
                tutar = kur * miktar;
                tutar = Math.Round(tutar, 2);
                tbTutar.Text = tutar.ToString();
                
                if (doviz == Dovizler.Dollar)
                {
                    if (mac.CaseOut("TRY", miktar * kur))
                    {
                        dac.UpdateValue("TRY", dac.GetValue("TRY") - tutar);
                        dac.UpdateValue("USD", dac.GetValue("USD") + miktar);
                        lblKasaDurum.Text = "Kasa Durum : Döviz bürosu bu alışı gerçekleştirmiştir";
                        dac.newAcitonLog(currentUsername, "USD - Alış", $"{DateTime.Now}");
                        dac.PrintDgv("tblHistory", dgvHistory);
                        lblKasaDurum.ForeColor = Color.LightGreen;
                    }
                    else
                    {
                        lblKasaDurum.Text = "Kasa Durum : Döviz bürosu, bu alışı gerçekleştiremedi";
                        lblKasaDurum.ForeColor = Color.Coral;
                    }
                }
                else if(doviz == Dovizler.Euro)
                {
                    if (mac.CaseOut("TRY", miktar * kur))
                    {
                        dac.UpdateValue("TRY", dac.GetValue("TRY") - tutar);
                        dac.UpdateValue("EUR", dac.GetValue("EUR") + miktar);
                        lblKasaDurum.Text = "Kasa Durum : Döviz bürosu bu alışı gerçekleştirmiştir";
                        dac.newAcitonLog(currentUsername, "EUR - Alış", $"{DateTime.Now}");
                        dac.PrintDgv("tblHistory", dgvHistory);
                        lblKasaDurum.ForeColor = Color.LightGreen;
                    }
                    else
                    {
                        lblKasaDurum.Text = "Kasa Durum : Döviz bürosu bu alışı gerçekleşemedi";
                        lblKasaDurum.ForeColor = Color.Coral;
                    }
                }
                dac.PrintDgv("tblCase", dgvCase);
            }
            else
            {
                MessageBox.Show("Kur türnüzü ve Miktar değerini giriniz");
            }

        }

        private void tbKur_TextChanged(object sender, EventArgs e)
        {
            tbKur.Text = tbKur.Text.Replace('.', ',');
        }

        private void tbMiktar_TextChanged(object sender, EventArgs e)
        {
            tbMiktar.Text = tbMiktar.Text.Replace('.', ',');
        }


        /*
        
                    Dolar satış butonuna basıldığında Aslında kullanıcı elindeki TRY birimini USD birimine çevirmek istiyor. Bu durumda kasada yeterli USD olup olmadığını kontrol etmemiz gerekiyor
                        kur --> yüksek olan sayı gelmeli 
                        [Miktar / kur ] ile Kasada yeterli USD olup olmadığını kontrol et
                        Kasa TRY = KasaTRY + Miktar;
                        kasa Dolar = KasaDolar - Miktar / kur;

                 */

        private void bntIslem2_Click(object sender, EventArgs e) // btnSatis
        {
            if (rac.CheckTb(tbKur) && rac.CheckTb(tbMiktar))
            {
                double kur = Convert.ToDouble(tbKur.Text.Replace('.', ','));
                int miktar = Convert.ToInt32(tbMiktar.Text);
                int tutar = Convert.ToInt32(Math.Floor(Convert.ToDouble(miktar / kur)));
                tbTutar.Text = tutar.ToString();
                double kalan = miktar - tutar * kur;
                tbKalan.Text = $"{Math.Floor(kalan)}";
                MessageBox.Show($"{tutar}");
                double mik_kur = Math.Round(miktar / kur, 2); 
                if (doviz == Dovizler.Dollar)
                {
                    if (mac.CaseOut("USD", miktar / kur))
                    {
                        dac.UpdateValue("TRY", dac.GetValue("TRY") + miktar);
                        dac.UpdateValue("USD", dac.GetValue("USD") - tutar);
                        lblKasaDurum.Text = "Kasa Durum : Döviz bürosu bu satışı gerçekleştirmiştir";
                        dac.newAcitonLog(currentUsername, "USD - Satış", $"{DateTime.Now}");
                        dac.PrintDgv("tblHistory", dgvHistory);
                        lblKasaDurum.ForeColor = Color.LightGreen;
                    }
                    else
                    {
                        lblKasaDurum.Text = "Kasa Durum : Döviz bürosu, bu satışı gerçekleştiremedi";
                        lblKasaDurum.ForeColor = Color.Coral;
                    }
                }
                else if (doviz == Dovizler.Euro)
                {
                    if (mac.CaseOut("EUR", miktar / kur))
                    {
                        dac.UpdateValue("TRY", dac.GetValue("TRY") + miktar);
                        dac.UpdateValue("EUR", dac.GetValue("EUR") - mik_kur);
                        lblKasaDurum.Text = "Kasa Durum : Döviz bürosu bu satışı gerçekleştirmiştir";
                        dac.newAcitonLog(currentUsername, "EUR - Satış", $"{DateTime.Now}");
                        dac.PrintDgv("tblHistory", dgvHistory);
                        lblKasaDurum.ForeColor = Color.LightGreen;
                    }
                    else
                    {
                        lblKasaDurum.Text = "Kasa Durum : Döviz bürosu bu satışı gerçekleşemedi";
                        lblKasaDurum.ForeColor = Color.Coral;
                    }
                }
                dac.PrintDgv("tblCase", dgvCase);
            }
            else
            {
                MessageBox.Show("Kur türnüzü ve Miktar değerini giriniz");
            }
            //writeData(doviz);
        }


        /*
            
            Dolar alış butonuna basıldığında Aslında kullanıcı elindeki USD birimini TRY birimine çevirmek istiyor. Bu durumda kasada yeterli TRY olup olmadığını kontrol etmemiz gerekiyor
                Kur --> düşük olan sayı gelmeli ✅
                [Miktar * KUR] durumu Kasadaki TRY birimini karşılıyor mu onu kontrol et
                Kasa TRY = KasaTRY - (Miktar * Kur);
                Kasa DOLAR = KasaDOLAR + Mitkar ;
         
         */

        private void btnHesapla_Click(object sender, EventArgs e)
        {
            if (cbAlis.CheckState == CheckState.Checked)
            {// Program alış durumunda, program alırsa türk lirası geri verecek, bu durumda kasanın yeterli türk lirasına sahip olması gerekiyor
                if (rac.CheckTb(tbKur) && rac.CheckTb(tbMiktar))
                {
                    double kur = Convert.ToDouble(tbKur.Text.Replace('.', ','));
                    int miktar = Convert.ToInt32(tbMiktar.Text);
                    int tutar = Convert.ToInt32(miktar * kur);
                    tbTutar.Text = tutar.ToString();
                    tbKalan.Text = "";
                    //double kalan = miktar - tutar * kur;
                    //tbKalan.Text = $"{Math.Floor(kalan)}";

                    if (mac.CaseOut("TRY", miktar * kur))
                    {
                        //dac.UpdateValue("TRY", dac.GetValue("TRY") - miktar * kur);
                        //dac.UpdateValue("EUR", dac.GetValue("EUR") + miktar );
                        lblKasaDurum.Text = "Kasa Durum : Döviz bürosu Döviz --> TRY işlemini gerçekleştirebilir";
                        lblKasaDurum.ForeColor = Color.LightGreen;
                    }
                    else
                    {
                        lblKasaDurum.Text = "Kasa Durum : Döviz bürosu Döviz --> TRY işlemini gerçekleştiremez";
                        lblKasaDurum.ForeColor = Color.Coral;
                    }
                }
            }
            else
            {// Program satış durumunda, program satarsa Dolar veya Euro satacak, kasada yeterince döviz olması gerekiyor

                /*
        
                    Dolar satış butonuna basıldığında Aslında kullanıcı elindeki TRY birimini USD birimine çevirmek istiyor. Bu durumda kasada yeterli USD olup olmadığını kontrol etmemiz gerekiyor
                        kur --> yüksek olan sayı gelmeli 
                        [Miktar / kur ] ile Kasada yeterli USD olup olmadığını kontrol et
                        Kasa TRY = KasaTRY + Miktar;
                        kasa Dolar = KasaDolar - Miktar / kur;

                 */
                if (rac.CheckTb(tbKur) && rac.CheckTb(tbMiktar))
                {
                    double kur, miktar, tutar, kalan;
                    kur = Convert.ToDouble(tbKur.Text.Replace('.',','));
                    miktar = Convert.ToDouble(tbMiktar.Text);
                    MessageBox.Show($"{kur}");
                    tutar = Convert.ToInt32(Math.Floor(Convert.ToDouble(miktar / kur)));
                    tbTutar.Text = tutar.ToString();
                    kalan = miktar - (tutar * kur);
                    tbKalan.Text = kalan.ToString();

                    if (doviz == Dovizler.Dollar)
                    {
                        if (mac.CaseOut("USD", miktar / kur))
                        {
                            lblKasaDurum.Text = "Kasa Durum : Döviz bürosu TRY --> Döviz işlemini gerçekleştirebilir";
                            lblKasaDurum.ForeColor = Color.LightGreen;
                        }
                        else
                        {
                            lblKasaDurum.Text = "Kasa Durum : Döviz bürosu, TRY --> Döviz işlemini gerçekleştiremez";
                            lblKasaDurum.ForeColor = Color.Coral;
                        }
                    }
                    else if (doviz == Dovizler.Euro)
                    {
                        if (mac.CaseOut("EUR", miktar / kur))
                        {
                            lblKasaDurum.Text = "Kasa Durum : Döviz bürosu TRY --> Döviz işlemini gerçekleştirebilir";
                            lblKasaDurum.ForeColor = Color.LightGreen;
                        }
                        else
                        {
                            lblKasaDurum.Text = "Kasa Durum : Döviz bürosu TRY --> Döviz işlemini gerçekleştiremez";
                            lblKasaDurum.ForeColor = Color.Coral;
                        }
                    }                
                }
                else
                {
                    MessageBox.Show("Kur türnüzü ve Miktar değerini giriniz");
                }
            }
        }


        private void tcMain_KeyDown(object sender, KeyEventArgs e)
        {
            // Eğer odaklanılan kontrol bir TextBox veya benzeri bir kontrolse
            if (this.ActiveControl is TextBox || this.ActiveControl is DataGridView)
            {
                // Eğer ok tuşlarından birine basılmışsa
                if (e.KeyCode == Keys.Left || e.KeyCode == Keys.Right)
                {
                    // Olayı işlenmiş olarak işaretlemeyin
                    e.Handled = false;
                    return;
                }
            }

            // Diğer tüm durumlarda olayın işlenmesini engelle
            e.Handled = true;
        }

        private void btnGGiris_Click(object sender, EventArgs e)
        {
            if (tbGKullaniciAdi.Text == "SuperAdmin" && tbGParola.Text == "1234")
            {
                tcMain.SelectedTab = tpSuperAdmin;
                return;
            }
            if (rac.usernameCheck(tbGKullaniciAdi) && rac.usernameCheck(tbGParola))  
            {
                if (dac.CheckLoginUser(tbGKullaniciAdi, tbGParola))
                {
                    currentUsername = tbGKullaniciAdi.Text;
                    tcMain.SelectedTab = tpDovizBuro;
                }
                else
                {
                    MessageBox.Show("Geçerli kullanıcı bulunamadı", "Giriş Başarısız");
                }
            }
            else
            {
                MessageBox.Show("Kullanıcı adınız veya parolanız'da geçersiz karakterler var", "Giriş Başarısız");
            } 
        }

        private void btnYKYeniKullaniciOlustur_Click(object sender, EventArgs e)
        {
            if (rac.emailCheck(tbYKEposta) && rac.usernameCheck(tbYKKullaniciAdi) && rac.usernameCheck(tbYKParola) && tbYKParola.Text == tbYKParolaTekrar.Text)
            {
                if (dac.CheckUsername(tbYKKullaniciAdi.Text) && dac.CheckEmail(tbYKEposta.Text))
                {
                    tbYKEposta.Enabled = false;
                    tbYKKullaniciAdi.Enabled = false;
                    tbYKParola.Enabled = false;
                    tbYKParolaTekrar.Enabled = false;

                    gbYKEpostaKontrol.Visible = true;
                    
                    string selectedPin = mac.rndPin(), selectedRef = mac.rndRef();
                    lblYKRefText.Text = selectedRef;
                    dac.NewUserAuth(tbYKEposta.Text, selectedRef, selectedPin);
                    esc.NewUser(tbYKEposta.Text, selectedPin, selectedRef);
                }
                else
                {
                    if (!dac.CheckUsername(tbYKKullaniciAdi.Text))
                    {
                        tbYKKullaniciAdi.BackColor = Color.Coral;
                        MessageBox.Show("Bu kullanıcı adı kullanılmaktadır", "Yeni kullanıcı oluşturulamadı");
                        tbYKKullaniciAdi.BackColor = Color.White;
                    }
                    else if (!dac.CheckEmail(tbYKEposta.Text))
                    {
                        tbYKEposta.BackColor = Color.Coral;
                        MessageBox.Show("Bu eposta kullanılmaktadır", "Yeni kullanıcı oluşturulamadı");
                        tbYKEposta.BackColor = Color.White;
                    }
                } 
            }
            else
            {
                if (!rac.emailCheck(tbYKEposta))
                {
                    MessageBox.Show("Epostanızı kontrol ediniz, hatalı eposta.", "Yeni Kullanıcı Oluşturmada Hata");
                }
                else if (!rac.usernameCheck(tbYKKullaniciAdi))
                {
                    MessageBox.Show("Kullanıcı adınızı kontrol ediniz, hatalı kullanıcı adı.", "Yeni Kullanıcı Oluşturmada Hata");
                }
                else if (!rac.usernameCheck(tbYKParola))
                {
                    MessageBox.Show("Parolanızı kontrol ediniz, hatalı parola girdisi.", "Yeni Kullanıcı Oluşturmada Hata");
                }
                else if (tbYKParola.Text != tbYKParolaTekrar.Text)
                {
                    MessageBox.Show("Parolanız uyuşmuyor tekrar deneyin, parola uyuşmazlığı.", "Yeni Kullanıcı Oluşturmada Hata");
                }
            }
        }

        private void btnYKEpostaOnay_Click(object sender, EventArgs e)
        {
            if(dac.NewUserAuth(lblYKRefText.Text, tbYKPin.Text))
            {
                dac.CreateUser(tbYKKullaniciAdi.Text, tbYKEposta.Text, tbYKParola.Text);
                MessageBox.Show("Yeni kullanıcı oluşturuldu", "İşlem başarılı");
                tcMain.SelectedTab = tpGiris;
            }
            else
            {
                MessageBox.Show("Pininiz doğrulanamadı, pininizi tekrar girin", "Pin Onaylama Başarısız");
            }
        }

        private void btnSYPinGonder_Click(object sender, EventArgs e)
        {
            if (dac.UserExist(tbSYKullaniciAdi.Text))
            {
                tbSYKullaniciAdi.Enabled = false;
                currentUsername = tbSYKullaniciAdi.Text;
                string _useremail = dac.GetEmailWithUsername(tbSYKullaniciAdi);
                string selectedPin = mac.rndPin(), selectedRef = mac.rndRef();
                dac.SetRefPin(currentUsername, _useremail, selectedPin, selectedRef);
                lblSYRef.Text = selectedRef;
                gbSYPinKontrol.Visible = true;
                esc.ResetPassword(_useremail, selectedPin, selectedRef);
                lblSYEposta.Text = $"Eposta : {_useremail}";
            }
            else
            {
                MessageBox.Show("Kullanıcı Bulunamadı", "Hatalı Giriş");
            }
        }

        private void btnSYPinOnayla_Click(object sender, EventArgs e)
        {
            if (dac.CheckUsernameRefPin(currentUsername, lblSYRef.Text, tbSYPin.Text))
            {
                MessageBox.Show("Yeni parolanızı belirleyebilirsiniz", "Eposta kontrolü başarılı");
                gbSYYeniParolaBelirle.Visible = true;
            }
            else
            {
                MessageBox.Show("Pini tekrar kontrol edin", "Eposta kontrolü başarısız");
            }
        }

        private void btnSYYeniParolaBelirle_Click(object sender, EventArgs e)
        {
            if (rac.usernameCheck(tbSYParola) && tbSYParola.Text == tbSYParolaTekrar.Text)
            {
                dac.SetPassword(currentUsername, tbSYParola.Text);
                lblSYEposta.Text = "Eposta : ";
                tcMain.SelectedTab = tpGiris;
            }
            else
            {
                if (!rac.usernameCheck(tbSYParola))
                {
                    MessageBox.Show("Yeni parolanızda istenmeyen karakterler bulunuyor, \nyeni parolayı özel karakterler kullanmadan ve boşluk bırakmadan oluşturun","Geçersiz Parola Oluşturma");
                }
                else if (!(tbSYParola.Text == tbSYParolaTekrar.Text))
                {
                    MessageBox.Show("Parolanın tekrarında uyuşmazlık var, tekrar deneyin.","Geçersiz Parola Oluşturma");
                }
                
            }
        }


        string currentUserId = string.Empty;
        private void dgvSuperStaff_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int selectedIndex = e.RowIndex;
            if (selectedIndex >= 0)
            {
                currentUserId = dgvSuperStaff.Rows[selectedIndex].Cells[0].Value.ToString();
                tbSuperUsername.Text = dgvSuperStaff.Rows[selectedIndex].Cells[1].Value.ToString();
                tbSuperEmail.Text = dgvSuperStaff.Rows[selectedIndex].Cells[2].Value.ToString();
            }
        }

        private void btnSuperUpdate_Click(object sender, EventArgs e)
        {
            if (currentUserId != string.Empty)
            {
                dac.UpdateUsernameEmail(tbSuperUsername.Text, tbSuperEmail.Text, currentUserId);
                dac.PrintDgv("tblStaff", dgvSuperStaff, "ID", "KullaniciAdi", "Eposta");
            }
            else
            {
                if (currentUserId == string.Empty)
                {
                    MessageBox.Show("CurrentuserId şuanda empty konumunda");
                }
            }
        }

        private void btnSuperDelete_Click(object sender, EventArgs e)
        {
            if (currentUserId != string.Empty)
            {
                dac.DeleteUser(currentUserId);
                dac.PrintDgv("tblStaff", dgvSuperStaff, "ID", "KullaniciAdi", "Eposta");
            }
            else
            {
                if (currentUserId == string.Empty)
                {
                    MessageBox.Show("CurrentuserId şuanda empty konumunda");
                }
            }
        }

        private void btnSuperAdd_Click(object sender, EventArgs e)
        {
            if (rac.CheckTb(tbSuperValue))
            {
                string _code = cbSuperCode.Text;
                dac.UpdateValue(_code, dac.GetValue(_code) + Convert.ToDouble(tbSuperValue.Text.Replace('.',',')));
                dac.PrintDgv("tblCase", dgvSuperCase);
            }
            else
            {
                MessageBox.Show("Geçerli bir değer girin");
            }
        }

        private void btnSuperMinus_Click(object sender, EventArgs e)
        {
            if (rac.CheckTb(tbSuperValue))
            {
                string _code = cbSuperCode.Text;
                if ((dac.GetValue(_code) - Convert.ToDouble(tbSuperValue.Text.Replace('.', ','))) >= 0)
                {
                    dac.UpdateValue(_code, dac.GetValue(_code) - Convert.ToDouble(tbSuperValue.Text.Replace('.', ',')));
                    dac.PrintDgv("tblCase", dgvSuperCase);
                }
                else
                {
                    MessageBox.Show("Kasa negatife düşemez", "Çıkarma İşlemi başarısız");
                }

            }
            else
            {
                MessageBox.Show("Geçerli bir değer girin");
            }
        }
    }
}
