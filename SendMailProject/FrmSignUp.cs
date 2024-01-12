
using Connectionsss;
using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SendMailProject
{
    public partial class FrmSignUp : Form
    {
        SqliteService sqlService;
        SqliteDataReader dataReader;
        bool test;
        object[] infos;
        public FrmSignUp()
        {
            InitializeComponent();
            sqlService = SqliteService.GetInstance();
        }

        private void btnKayit_Click(object sender, EventArgs e)
        {
            try
            {


                if (string.IsNullOrEmpty(txtMail.Text) || string.IsNullOrEmpty(txtPass.Text) ||string.IsNullOrEmpty(txtPass2.Text))
                {
                    MessageBox.Show("Lütfen Bilgileri Eksiksiz Doldurunuz !","Hata",MessageBoxButtons.OK,MessageBoxIcon.Error);
                    return;
                
                
                }
                if (!txtMail.Text.Contains("@") || !txtMail.Text.Contains(".com"))
                {
                    MessageBox.Show("Lütfen Girdiğiniz Maili Kontrol Ediniz !", "HATA", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                if (txtPass.Text!=txtPass2.Text)
                {
                    MessageBox.Show("Şifreler Eşleşmiyor","HATA",MessageBoxButtons.OK,MessageBoxIcon.Error);
                    return;
                }
                    DialogResult dialog = MessageBox.Show("'"+txtMail.Text+"' İle Kayıt Olmak İstediğinize Emin Misiniz ?","SORU",MessageBoxButtons.YesNo,MessageBoxIcon.Question);;
                if (dialog == DialogResult.Yes)
                {
                   dataReader= sqlService.Reader("select Mail from KULLANICILAR where Mail=@mail", new SqliteParameter("@mail", txtMail.Text));
                   if (dataReader.Read())
                    {
                        MessageBox.Show("'" + txtMail.Text + "' Sistemde Kayıtlı !", "HATA", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    dataReader.Close();
                        return;
                    }
                  else
                    {
                        dataReader.Close();
                        sqlService.Execute("insert into  KULLANICILAR  (Mail,Sifre) values (@mail,@sifre)", new SqliteParameter("@mail", txtMail.Text), new SqliteParameter("@sifre", txtPass.Text));
                        object[] isim = txtMail.Text.Split('@');
                    MessageBox.Show("'" +isim[0]+ "' Kaydınız Başarıyla Yapılmıştır", "BİLGİ", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        FrmLogin frmLogin = new FrmLogin();
                        
                        frmLogin.Login(txtMail.Text);
                       this.Hide();
                        frmLogin.ShowDialog();
                    }






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
                txtPass.UseSystemPasswordChar = false;
                txtPass2.UseSystemPasswordChar = false;
            }
            else
            {
                txtPass.UseSystemPasswordChar =  true  ;
                txtPass2.UseSystemPasswordChar = true;
            }
        }

        private void FrmSignUp_Load(object sender, EventArgs e)
        {
            
        }
    }
}
