
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
    public partial class FrmAddPerson : Form
    {
        SqliteService sqlService;
        SqliteDataReader dataReader;
        SqlDataAdapter adapter;
        DataTable dataTable;
        public object[] infos;
        bool result;
        bool test;



        public FrmAddPerson()
        {
            InitializeComponent();
            sqlService = SqliteService.GetInstance();


        }

        private void btnKaydet_Click(object sender, EventArgs e)
        {
            try
            {
                DialogResult dialog = MessageBox.Show("Kaydetmek İstediğinize Emin Misiniz ?", "SORU", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (dialog == DialogResult.Yes)
                {

                    if (string.IsNullOrEmpty(txtMail.Text))
                    {

                        MessageBox.Show("Lütfen Bilgileri Eksiksiz Doldurunuz !", "HATA", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;






                    }
                    else
                    {
                        if (!txtMail.Text.Contains("@") || !txtMail.Text.Contains(".com"))
                        {
                            MessageBox.Show("Lütfen Girdiğiniz Maili Kontrol Ediniz !","HATA",MessageBoxButtons.OK,MessageBoxIcon.Error);
                            return;
                        }
                        dataReader = sqlService.Reader("select Mail from MAILLER where Mail=@mail and Kullanici_Id=@id", new SqliteParameter("@mail", txtMail.Text),new SqliteParameter("@id", infos[0]));
                        if (dataReader.Read())
                        {
                            MessageBox.Show(txtMail.Text + " Daha Önce Eklenmiş");
                            return;
                        }
                        dataReader.Close();
                        sqlService.Execute("insert into MAILLER (Mail,Kullanici_Id) values (@mail,@id)", new SqliteParameter("@mail", txtMail.Text), new SqliteParameter("@id", infos[0]));
                       
                        MessageBox.Show(txtMail.Text + " Başarıyla Kaydedildi");
                        txtMail.Text = "";
                        DtgData();
                        return;
                    }

                }


            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message); ;
            }
        }

        public void DtgData()
        {
            List<string> mailler = new List<string>();
            try
            {
                dtgKisiler.Rows.Clear();
               dataReader = sqlService.Reader("select Mail from MAILLER where Kullanici_Id=@id ", new SqliteParameter("@id", infos[0]));
                while (dataReader.Read())
                {
                    dtgKisiler.Rows.Add(dataReader["Mail"]);
                }

                dataReader.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);

            }


        }


        private void FrmAddPerson_Load(object sender, EventArgs e)
        {
            DtgData();
        }

        private void btnSil_Click(object sender, EventArgs e)
        {
            try
            {
                if (dtgKisiler.CurrentRow == null)
                {
                    MessageBox.Show("Lütfen Mail Seçiniz !", "HATA", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;

                }
                object mail = dtgKisiler.CurrentRow.Cells[0].Value;
                DialogResult dialog = new DialogResult();
                dialog = MessageBox.Show("' " + mail + " '" + " Mailini Silmek İstediğinize Emin Misiniz ?", "SORU", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dialog == DialogResult.Yes)
                {
                   dataReader = sqlService.Reader("select Mail from MAILLER where Mail=@mail and Kullanici_Id=@id", new SqliteParameter("@mail", mail), new SqliteParameter("@id", infos[0]));
                    if (dataReader.Read())
                    {
                        sqlService.Execute("delete from MAILLER where Mail=@mail and Kullanici_Id=@id", new SqliteParameter("@mail", mail), new SqliteParameter("@id", infos[0]));
                        MessageBox.Show("' " + mail + " '" + " Maili Başarıyla Silindi !", "BİLGİ", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        DtgData();
                        dataReader.Close();
                        return;

                    }              
                    else
                    {
                        dataReader.Close();
                        MessageBox.Show("' " + mail + " '" + " Maili Kayıtlı Değil Veya Silinmiş !", "HATA", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        
                        return;
                    }
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            FrmSend frmSend = new FrmSend();
            frmSend.infos = infos;
            this.Hide();
            frmSend.ShowDialog();
        }
    }
}
