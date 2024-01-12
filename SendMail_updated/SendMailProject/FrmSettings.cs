using Connectionsss;
using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SendMailProject
{
    public partial class FrmSettings : Form
    {
        public object[] infos;
        SqliteDataReader dataReader;
        SqliteService sqliteService;
        public FrmSettings()
        {
            InitializeComponent();
        }

        private void FrmSettings_Load(object sender, EventArgs e)
        {
            txtMail.Text = infos[1].ToString();
            txtMailDel.Text = infos[1].ToString();

            sqliteService = SqliteService.GetInstance();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                txtOldPass.UseSystemPasswordChar = false;
                txtNewPass.UseSystemPasswordChar = false;
                txtNewPass2.UseSystemPasswordChar = false;
            }
            else
            {
                txtOldPass.UseSystemPasswordChar = true;
                txtNewPass.UseSystemPasswordChar = true;
                txtNewPass2.UseSystemPasswordChar = true;

            }
        }

        private void btnKaydet_Click(object sender, EventArgs e)
        {
            DialogResult dialog = MessageBox.Show("Şifrenizi Güncellemek İstediğinize Emin Misiniz ?", "SORU", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dialog == DialogResult.Yes)
            {

                if (string.IsNullOrEmpty(txtNewPass.Text) || string.IsNullOrEmpty(txtNewPass2.Text) || string.IsNullOrEmpty(txtOldPass.Text))
                {
                    MessageBox.Show("Lütfen Bilgileri Eksiksiz Doldurunuz !", "HATA", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (txtOldPass.Text != infos[2].ToString())
                {
                    MessageBox.Show("Mevcut Şifre Yanlış !", "HATA", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                if (txtNewPass.Text != txtNewPass2.Text)
                {
                    MessageBox.Show("Şifreler Uyuşmuyor !", "HATA", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                try
                {

                    sqliteService.Execute("update KULLANICILAR set SIFRE=@yeniSifre where MAIL=@mail and SIFRE=@eskiSifre", new SqliteParameter("@yeniSifre", txtNewPass.Text), new SqliteParameter("@mail",txtMail.Text), new SqliteParameter("@eskiSifre", txtOldPass.Text));
                    dataReader = sqliteService.Reader("select *from KULLANICILAR where MAIL=@mail and SIFRE=@yeniSifre", new SqliteParameter("@yeniSifre", txtNewPass.Text), new SqliteParameter("@mail", txtMail.Text));
                    if (!dataReader.Read())
                    {
                        dataReader.Close();
                        MessageBox.Show("Bir Hata Oluştu !","HATA",MessageBoxButtons.OK,MessageBoxIcon.Error);
                        return;
                    }
                    dataReader.Close();
                    MessageBox.Show("Şifreniz Başarıyla Güncellendi !", "BİLGİ", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    infos[0] = txtMail.Text;
                    infos[1] = txtNewPass.Text;
                    txtOldPass.Text = "";
                    txtNewPass.Text = "";
                    txtNewPass2.Text = "";
                }
                catch (Exception ex)
                {

                    MessageBox.Show("Bir Hata Oluştu : " + ex.Message);
                }

            }
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            FrmSend frmSend = new FrmSend();
            frmSend.infos = infos;
            this.Hide();
            frmSend.ShowDialog();
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox2.Checked)
            {
                txtPassDel.UseSystemPasswordChar = false;

            }
            else
            {
                txtPassDel.UseSystemPasswordChar = true;


            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            DialogResult dialog = MessageBox.Show("Hesabınızı Silmek İstediğinize Emin Misiniz ?", "SORU", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dialog == DialogResult.Yes)
            {

                if (string.IsNullOrEmpty(txtPassDel.Text))
                {
                    MessageBox.Show("Lütfen Bilgileri Eksiksiz Doldurunuz !", "HATA", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                dataReader = sqliteService.Reader("select *from KULLANICILAR where  MAIL=@mail and SIFRE=@sifre", new SqliteParameter("@mail", txtMailDel.Text), new SqliteParameter("@sifre", txtPassDel.Text));
                
                if (!dataReader.Read())
                {
                    MessageBox.Show("Şifre Yanlış !", "HATA", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    dataReader.Close();
                    return;
                }
                    dataReader.Close();
                try
                {
                    sqliteService.Execute("delete from KULLANICILAR where MAIL=@mail and SIFRE=@sifre ", new SqliteParameter("@mail", txtMailDel.Text), new SqliteParameter("@sifre", txtPassDel.Text));
                    MessageBox.Show("Hesabınız Başarıyla Silindi !","BİLGİ",MessageBoxButtons.OK,MessageBoxIcon.Information);
                    FrmLogin frmLogin = new FrmLogin();
                    this.Hide();
                    frmLogin.ShowDialog();
                }
                catch (Exception ex)
                {

                    MessageBox.Show("HATA : " + ex.Message);
                }

            }

        }

    }
}

