

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
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SendMailProject
{
    public partial class FrmSend : Form
    {
        public object[] infos;
        public string[] isim;
        List<EkDosya> dosyalar;
        List<string> dosyaYolu;
        List<string> mail;
        SqliteService sqliteService;
        SqliteDataReader dataReader;
       public string mail1, konu, icerik;
       

        List<string> mailler;
        FrmLogin frmLogin;
        string filePath;

        public FrmSend()
        {
            InitializeComponent();
            dosyalar = new List<EkDosya>();
            dosyaYolu = new List<string>();
            mail = new List<string>();
            sqliteService = SqliteService.GetInstance();
            mailler = new List<string>();
            frmLogin = new FrmLogin();

        }

        private void FrmSend_Load(object sender, EventArgs e)
        {
            
            ComboLoad();
            txtMail.Text = mail2;
            txtKonu.Text = konu1;
            rchIcerik.Text = icerik1;
            isim = infos[1].ToString().Split('@');


        }
        public void MailSend(string subject, string body, List<string> receivers, List<string> attachments = null)
        {
            try
            {
                SmtpClient client = new SmtpClient();
                client.Port = 587;
                client.Host = "smtp.gmail.com";
                client.EnableSsl = true;
                client.Timeout = 20000;
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.UseDefaultCredentials = false;
                client.Credentials = new System.Net.NetworkCredential(infos[1].ToString(), infos[2].ToString());
                MailMessage mailMessage = new MailMessage();
                mailMessage.From = new MailAddress(infos[1].ToString(), isim[0]);
                mailMessage.SubjectEncoding = Encoding.UTF8;
                mailMessage.Subject = subject; //E-posta Konu Kısmı
                mailMessage.BodyEncoding = Encoding.UTF8;
                mailMessage.Body = body; // E-posta'nın Gövde Metni
                foreach (string item in receivers)
                {
                    mailMessage.To.Add(item);
                }
                mailMessage.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;
                if (attachments != null)
                {
                    if (attachments.Count > 0)
                    {
                        foreach (string filePath in attachments)
                        {
                            if (File.Exists(filePath))
                            {
                                Attachment attachment = new Attachment(filePath);
                                mailMessage.Attachments.Add(attachment);
                            }
                        }
                    }
                }
                client.Send(mailMessage);
                MessageBox.Show("Mail Gönderildi !","BİLGİ",MessageBoxButtons.OK,MessageBoxIcon.Information);
                this.Hide();
                FrmLogin frmLogin = new FrmLogin();
                frmLogin.Test(infos,"","","");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnGonder_Click(object sender, EventArgs e)
        {
         
            DialogResult result = MessageBox.Show("Göndermek İstediğinize Emin Misiniz ?","SORU",MessageBoxButtons.YesNo,MessageBoxIcon.Question);

            if (result==DialogResult.Yes)
            {

           
            
            
            try
            {

                if (string.IsNullOrEmpty(txtKonu.Text) || string.IsNullOrEmpty(rchIcerik.Text) || string.IsNullOrEmpty(txtMail.Text))
                {
                    MessageBox.Show("Lütfen Bilgileri Eksiksiz Doldurunuz !", "HATA", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                    if (!txtMail.Text.Contains("@") || !txtMail.Text.Contains(".com"))
                    {
                        MessageBox.Show("Lütfen Girdiğiniz Maili Kontrol Ediniz !", "HATA", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    foreach (EkDosya item in dosyalar)
                {
                    dosyaYolu.Add(item.FilePath);
                }

                mail.Add(txtMail.Text);

                MailSend(txtKonu.Text, rchIcerik.Text, mail, dosyaYolu);

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
            }
        }

        private void btnDosyaEkle_Click(object sender, EventArgs e)
        {
            
            DialogResult dialog = openFileDialog1.ShowDialog();
            if (dialog==DialogResult.OK)
            {
                filePath = openFileDialog1.FileName;
                EkDosya dosya = new EkDosya(Path.GetFileName(filePath), filePath);
               dosyalar.Add(dosya);
               
                dtgDosyalar.DataSource = dosyalar;

            }
        
            
        
        }

        private void FrmSend_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }

    public void ComboLoad()
        {

            cmbKisiler.DataSource = ComboData();
            cmbKisiler.SelectedIndex= -1;




        }
        public List<string> ComboData()
        {
            try
            {
                
                dataReader = sqliteService.Reader("select Mail from MAILLER where Kullanici_Id=@id ",new SqliteParameter("@id",infos[0]));
                while (dataReader.Read())
                {
                    mailler.Add(dataReader["Mail"].ToString());

                }
                dataReader.Close();
                return mailler;


            }
            catch (Exception)
            {

                return new List<string>();
            }




        }

        private void cmbKisiler_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtMail.Text = cmbKisiler.Text;
        }

        private void kişiEkleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmAddPerson addPerson = new FrmAddPerson();
            addPerson.infos = infos;
            this.Hide();
            addPerson.ShowDialog();
        }

        private void yenileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            txtKonu.Text = "";
            txtMail.Text = "";
            rchIcerik.Text = "";
            cmbKisiler.SelectedIndex = -1;
            
       
        }

        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        void Clear()
        {

            txtKonu.Text = "";
            txtMail.Text = "";
            rchIcerik.Text = "";
           
        }

        private void çıkışYapToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmLogin frmLogin = new FrmLogin();
            this.Hide();
            frmLogin.ShowDialog();
        }

        private void ayarlarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmSettings frmSettings = new FrmSettings();
            frmSettings.infos = infos;
            this.Hide();
            frmSettings.ShowDialog();
        }

        public string mail2, konu1, icerik1;
        private void button1_Click(object sender, EventArgs e)
        {
            
        mail1 = txtMail.Text;
            konu = txtKonu.Text;
            icerik = rchIcerik.Text;
            
            dtgDosyalar.DataSource = "";
            FrmLogin login = new FrmLogin();
            this.Hide();
           
            login.Test(infos,mail1,konu,icerik);

        }
    }
    class EkDosya
    {
        string name, filePath;

        public string Name { get => name; set => name = value; }
        public string FilePath { get => filePath; set => filePath = value; }

        public EkDosya(string name, string filePath)
        {
            this.name = name;
            this.filePath = filePath;
        }
   
    
    }

}
