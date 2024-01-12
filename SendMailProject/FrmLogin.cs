
using Connectionsss;
using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SendMailProject
{
    public partial class FrmLogin : Form
    {
        SqliteService sqlService;
        SqliteDataReader dataReader1;
        string folderPath = System.Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @"\Talimatlar.pdf";
        string folderPath2 = Environment.CurrentDirectory + @"\Talimatlar.pdf";
        public string[] isim;
        object[] infos = null;
        List<string> hesaplar;
        bool start;
        bool start1;


        



        public FrmLogin()
        {
            InitializeComponent();
            sqlService = SqliteService.GetInstance();
            hesaplar = new List<string>();
            
        }

        private void btnGiris_Click(object sender, EventArgs e)
        {

            try
            {
                if (txtMail.Text.Trim() == "" || txtSifre.Text.Trim() == "")
                {
                    MessageBox.Show("Lütfen Bilgileri Eksiksiz Doldurunuz !", "HATA", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
               
                if (!txtMail.Text.Contains("@") || !txtMail.Text.Contains(".com"))
                {
                    MessageBox.Show("Lütfen Girdiğiniz Maili Kontrol Ediniz !", "HATA", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                if (checkBox2.Checked)
                {
                    sqlService.Execute("update KULLANICILAR set REMEMBER=1  where MAIL=@mail and SIFRE=@sifre   ", new SqliteParameter("@mail", txtMail.Text), new SqliteParameter("@sifre", txtSifre.Text));
                } 
                if (!checkBox2.Checked)
                {
                    sqlService.Execute("update KULLANICILAR set REMEMBER=0  where MAIL=@mail and SIFRE=@sifre   ", new SqliteParameter("@mail", txtMail.Text), new SqliteParameter("@sifre", txtSifre.Text));
                }
            
                dataReader1=  sqlService.Reader("select * from KULLANICILAR where MAIL=@mail and SIFRE=@sifre", new SqliteParameter("@mail", txtMail.Text), new SqliteParameter("@sifre", txtSifre.Text));
                if (dataReader1.Read())
                {
                    string mail,sifre;int id;

                    id = Convert.ToInt16(dataReader1["ID"]);
                    mail = dataReader1["MAIL"].ToString();
                    sifre = dataReader1["SIFRE"].ToString();
                   


                    infos = new object[] { id, mail,sifre };
                isim = txtMail.Text.Split('@');
                MessageBox.Show("Giriş Başarılı '"+isim[0]+"'","BİLGİ", MessageBoxButtons.OK, MessageBoxIcon.Information);
                FrmSend frmSend = new FrmSend();
                frmSend.infos = infos;
                    this.Hide();
                    frmSend.ShowDialog();

                }
                dataReader1.Close();
                if (infos == null)
                {
                    MessageBox.Show("Bilgileriniz Hatalı","HATA",MessageBoxButtons.OK,MessageBoxIcon.Error);
                    return;
                }

            }
            catch (Exception)
            {
              
                throw;
            }


        }

      

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                txtSifre.UseSystemPasswordChar = false;
            }
            else { txtSifre.UseSystemPasswordChar = true; }
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {

            
            
           DialogResult dialog= MessageBox.Show("Uygulamamızı Kullanabilmeniz İçin Önce Masaüstünüze Yüklenen (Talimatlar.pdf) Adlı Dosyanın İçinde Yazanları Uygulamanız Gerekmektedir ! \n\n(Aksi Halde Uygulama Hata Verecektir !)", "BİLGİ", MessageBoxButtons.OK, MessageBoxIcon.Information);                           
            if (!File.Exists(folderPath))
            {
                File.Copy(folderPath2, folderPath);

            }
           
            
            
            FrmSignUp frmSignUp = new FrmSignUp();
            this.Hide();
            frmSignUp.ShowDialog();

        }

        private void FrmLogin_Load(object sender, EventArgs e)
        {
            start = true;
            start1 = true;
            CmbData();
            
            start = false;
            
           
        
        }
  public void Login(string mail)
        {
            txtMail.Text = mail;

        }
    public void Test(object[] infos1,string mail,string konu,string icerik)
        {
            FrmSend frmSend = new FrmSend();
           
            infos = infos1;
            frmSend.mail2 = mail;
            frmSend.konu1 = konu;
            frmSend.icerik1 = icerik;
            frmSend.infos = infos;
            this.Hide();
            frmSend.ShowDialog();


        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox2.Checked)
            {

                if (string.IsNullOrEmpty(txtMail.Text) || string.IsNullOrEmpty(txtSifre.Text))
                {
                    MessageBox.Show("Lütfen Bilgileri Eksiksiz Doldurunuz !","HATA",MessageBoxButtons.OK,MessageBoxIcon.Error);
                    checkBox2.Checked = false;
                    return;
                }

                if (start1)
                {

                DialogResult dialog=    MessageBox.Show("Hesabınızı Kaydetmek İstediğinize Emin Misiniz ? ","SORU",MessageBoxButtons.YesNo,MessageBoxIcon.Question);

                try
                {
                    if (dialog == DialogResult.Yes)
                    {
                        dataReader1 = sqlService.Reader("Select *from KULLANICILAR where  MAIL=@mail and SIFRE=@sifre", new SqliteParameter("@mail", txtMail.Text), new SqliteParameter("@sifre", txtSifre.Text));
                        if (!dataReader1.Read())
                        {
                            MessageBox.Show("Hesabınız Sistemde Kayıtlı Değildir !", "HATA", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                checkBox2.Checked = false;
                                return;
                        }

                        dataReader1.Close();

                    }

                }
                catch (Exception ex)
                {

                    MessageBox.Show(ex.Message);
                } 
                
                }  
            
            }
        }
   
    


      
        
        
        public void CmbData()
        {

                    
            try
            {
               
                dataReader1 = sqlService.Reader("select MAIL from KULLANICILAR where REMEMBER=1");
                
                while (dataReader1.Read())
                {

                    hesaplar.Add(dataReader1["MAIL"].ToString());
                    
                    
                }
             
                dataReader1.Close();
                cmbHesaplar.DataSource = hesaplar;
                cmbHesaplar.SelectedIndex = -1;



            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }

        }

       

        private void cmbHesaplar_SelectedIndexChanged(object sender, EventArgs e)
        {
         

            
            if (start)
            {
                return ;
            }

            txtMail.Text = cmbHesaplar.Text;          
            dataReader1 = sqlService.Reader("Select SIFRE from KULLANICILAR where MAIL=@mail",new SqliteParameter("@mail",txtMail.Text));

            if (dataReader1.Read())
            {
               
                txtSifre.Text = dataReader1["SIFRE"].ToString();
                start1 = false;
                checkBox2.Checked = true;
            }
            dataReader1.Close();
            
        }

       
    }



}
