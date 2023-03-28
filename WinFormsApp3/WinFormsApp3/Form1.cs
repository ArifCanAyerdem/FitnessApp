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

namespace WinFormsApp3
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        SqlConnection cnn;
        SqlCommand command;
        SqlDataReader dataReader;
        String sql;

        string guncellenecekUyeId = "", guncellenecekEgitmenId ="", guncellenecekUrunId="";

        int secilenUrunFiyat = 0, toplamFiyat=0;

        int siparisVerecekUrunId=0, siparisVerecekKullaniciId = 0;


        private void uyeGetir(ComboBox cmb)
        {
            cmb.Items.Clear();
            sql = "Select uye_id,uye_adi,uye_soyadi from Uyeler";
            command = new SqlCommand(sql, cnn);
            dataReader = command.ExecuteReader();
            while (dataReader.Read())
            {
                String uye = dataReader.GetValue(0) + "-" + dataReader.GetValue(1) + "-" + dataReader.GetValue(2);

                cmb.Items.Add(uye);
            }

            dataReader.Close();

        }
        private void egitmenGetir(ComboBox cmb)
        {
            cmb.Items.Clear();
            sql = "Select egitmen_id,egitmen_adi,egitmen_soyadi from Egitmenler";
            command = new SqlCommand(sql, cnn);
            dataReader = command.ExecuteReader();
            while (dataReader.Read())
            {
                String uye = dataReader.GetValue(0) + "-" + dataReader.GetValue(1) + "-" + dataReader.GetValue(2);

                cmb.Items.Add(uye);
            }

            dataReader.Close();

        }
        void urunlistele()
        {


            //SqlCommand command;

            sql = " select * from Urunler ORDER BY urun_id desc";
            DataTable tbl = new DataTable();
            SqlDataAdapter adaptr = new SqlDataAdapter(sql, cnn);
            adaptr.Fill(tbl);

            dataGridViewUrun.DataSource = tbl;


        }
        private void urunGetir(ComboBox cmb)
        {
            cmb.Items.Clear();
            sql = "Select urun_id,urun_adi from Urunler";
            command = new SqlCommand(sql, cnn);
            dataReader = command.ExecuteReader();
            while (dataReader.Read())
            {
                String uye = dataReader.GetValue(0) + "-" + dataReader.GetValue(1);

                cmb.Items.Add(uye);
            }

            dataReader.Close();

        }
        void egitmenlistele()
        {


            //SqlCommand command;

            sql = "select e.egitmen_id,e.egitmen_adi,e.egitmen_soyadi,d.egtmen_id,d.egitmen_telno,d.egitmen_detayid,d.egitmen_brans,d.egitmen_ucret from Egitmenler as e left Join Egitmen_Detay as d On e.egitmen_id=d.egtmen_id  ORDER BY e.egitmen_id desc";
            DataTable tbl = new DataTable();
            SqlDataAdapter adaptr = new SqlDataAdapter(sql, cnn);
            adaptr.Fill(tbl);

            dataGridViewEgitmen.DataSource = tbl;


        }
        void siparislistele()
        {


            //SqlCommand command;

            sql = " select * from Siparis_Detay ORDER BY siparis_id desc";
            DataTable tbl = new DataTable();
            SqlDataAdapter adaptr = new SqlDataAdapter(sql, cnn);
            adaptr.Fill(tbl);

            dataGridViewSiparis.DataSource = tbl;


        }
        void uyelistele()
        {


            //SqlCommand command;

            sql = "select u.uye_id,u.uye_adi,u.uye_soyadi,u.egitmen_id,u.uye_ogr_no,d.uye_detayid,d.uye_telno,d.uye_giristarih,d.uye_cikistarih,d.fakulte_id from Uyeler as u left Join Uye_Detay as d On u.uye_id=d.uye_id  ORDER BY u.uye_id desc";
            DataTable tbl = new DataTable();
            SqlDataAdapter adaptr = new SqlDataAdapter(sql, cnn);
            adaptr.Fill(tbl);

            dataGridView1.DataSource = tbl;


        }
        private void button1_Click(object sender, EventArgs e)
        {


            uyelistele();

         /*sql = "select * from Uyeler as u Inner Join Uye_Detay as d    On u.uye_id=d.uye_id  ORDER BY u.uye_id desc";
            DataTable tbl = new DataTable();
            SqlDataAdapter adaptr = new SqlDataAdapter(sql, cnn);
            adaptr.Fill(tbl);

            dataGridView1.DataSource = tbl;*/


        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            cnn.Close();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            string connetionString = @"Server= LAPTOP-9D6JCF80;Database=fitness; Trusted_Connection=True";
            cnn = new SqlConnection(connetionString);
            cnn.Open();

            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.AllowUserToDeleteRows = false;
            dataGridView1.ReadOnly = true;

            dataGridViewEgitmen.AllowUserToAddRows = false;
            dataGridViewEgitmen.AllowUserToDeleteRows = false;
            dataGridViewEgitmen.ReadOnly = true;

            dataGridViewUrun.AllowUserToAddRows = false;
            dataGridViewUrun.AllowUserToDeleteRows = false;
            dataGridViewUrun.ReadOnly = true;

            dataGridViewSiparis.AllowUserToAddRows = false;
            dataGridViewSiparis.AllowUserToDeleteRows = false;
            dataGridViewSiparis.ReadOnly = true;
        }


        private void button2_Click(object sender, EventArgs e)
        {
            string secilen = comboBox1.Text.ToString();
            if (secilen != "")
            {
                int s = secilen.IndexOf('-');
                string secId = "";
                for (int i = 0; i < s; i++)
                {
                    secId += secilen.ToCharArray()[i].ToString();
                }
                int secilenId = Convert.ToInt32(secId);

               
                SqlCommand command;
                SqlDataAdapter adapter = new SqlDataAdapter();
                String sql = "";
                sql = "Delete Uyeler  where uye_id = " + secilenId;
                command = new SqlCommand(sql, cnn);
                adapter.DeleteCommand = command;
                adapter.DeleteCommand.ExecuteNonQuery();
                command.Dispose();

                uyeGetir(comboBox1);
                uyelistele();
            }
            else
                MessageBox.Show("Kullanıcı seç!");

        }

        private void button3_Click(object sender, EventArgs e)
        {
            int egitmenId=0;
            int fakulteId = 0;

            string secilen = comboBoxUyeEgitmen.Text.ToString();
            string secilen1 = comboBoxUyeFakulte.Text.ToString();

            if (secilen != "")
            {
                string[] secilenSplit = secilen.Split('-');
                egitmenId = Convert.ToInt32(secilenSplit[0]);
                string[] secilen1Split = secilen1.Split('-');
                fakulteId = Convert.ToInt32(secilen1Split[0]);



                SqlCommand command;
                SqlDataAdapter adapter = new SqlDataAdapter();
                String sql = "";
                sql = "Insert into Uyeler (uye_adi,uye_soyadi,egitmen_id,uye_ogr_no) values ('" + textBoxUyeAd.Text + "','" + textBoxUyeSoyad.Text + "'," + egitmenId + "," + Convert.ToInt32(textBoxUyeNo.Text) + ")SELECT SCOPE_IDENTITY()";

                command = new SqlCommand(sql, cnn);
                adapter.InsertCommand = command;
                int lastId = Convert.ToInt32(adapter.InsertCommand.ExecuteScalar());
                


                string giris = dateTimePickerUyeGiris.Value.Year.ToString() + "." + dateTimePickerUyeGiris.Value.Month.ToString() + "." + dateTimePickerUyeGiris.Value.Day.ToString();
                int sozlesme = Convert.ToInt32(numericUpDownUyeSozlesme.Value.ToString());

                DateTime girisZaman = dateTimePickerUyeGiris.Value;
                DateTime cikisZaman = girisZaman.AddMonths(sozlesme);

                string cikis = cikisZaman.Year.ToString() + "." + cikisZaman.Month.ToString() + "." + cikisZaman.Day.ToString();

                sql = "Insert into Uye_Detay(uye_cikistarih,uye_giristarih,uye_id,uye_telno,fakulte_id) values ('"+ cikis + "','"+ giris + "',"+lastId+","+Convert.ToInt32(textBoxUyeTel.Text)+"," + fakulteId +");";

                command = new SqlCommand(sql, cnn);
                adapter.InsertCommand = command;
                adapter.InsertCommand.ExecuteNonQuery();
                command.Dispose();


                uyelistele();
            }
            else
                MessageBox.Show("Kullanıcıya egitmen seç!");
        }




        private void button4_Click(object sender, EventArgs e)
        {

            int egitmenId = 0;
            int fakulteId = 0;
            string secilen = comboBox3.Text.ToString();
            string secilen1 = comboBox2.Text.ToString();


            if (secilen != "")
            {
                string[] secilenSplit = secilen.Split('-');
                egitmenId = Convert.ToInt32(secilenSplit[0]);
                string[] secilen1Split = secilen1.Split('-');
                fakulteId = Convert.ToInt32(secilen1Split[0]);




                SqlCommand command;
                SqlDataAdapter adapter = new SqlDataAdapter();
                String sql = "";
                sql = "Update Uyeler set uye_adi='" + textBox4.Text + "',uye_soyadi='" + textBox3.Text + "',egitmen_id=" + egitmenId + ",uye_ogr_no=" + Convert.ToInt32(textBox1.Text)+" where uye_id = "+guncellenecekUyeId;


                command = new SqlCommand(sql, cnn);
                adapter.UpdateCommand = command;
                adapter.UpdateCommand.ExecuteNonQuery();



                string giris = dateTimePicker1.Value.Year.ToString() + "." + dateTimePicker1.Value.Month.ToString() + "." + dateTimePicker1.Value.Day.ToString();
                int sozlesme = Convert.ToInt32(numericUpDown1.Value.ToString());

                DateTime girisZaman = dateTimePicker1.Value;
                DateTime cikisZaman = girisZaman.AddMonths(sozlesme);

                string cikis = cikisZaman.Year.ToString() + "." + cikisZaman.Month.ToString() + "." + cikisZaman.Day.ToString();

                sql = "Update Uye_Detay set uye_cikistarih='" + cikis + "',uye_giristarih='" + giris  + "',uye_telno=" + Convert.ToInt32(textBox2.Text)+" ,fakulte_id= " + fakulteId+ "where uye_id = " + guncellenecekUyeId;

                command = new SqlCommand(sql, cnn);
                adapter.UpdateCommand = command;
                adapter.UpdateCommand.ExecuteNonQuery();

                command.Dispose();


                uyelistele();
            }
            else
                MessageBox.Show("Kullanıcıya egitmen seç!");

        }

        private void comboBoxUyeEgitmen_Click(object sender, EventArgs e)
        {

            egitmenGetir(comboBoxUyeEgitmen);
        }

        void egitmenGetir(ComboBox cmb, String egitmenId="")
        {

            cmb.Items.Clear();
            sql = "Select * from Egitmenler";
            command = new SqlCommand(sql, cnn);
            dataReader = command.ExecuteReader();
            int sira = 0;
            while (dataReader.Read())
            {
                String uye = dataReader.GetValue(0) + "--" + dataReader.GetValue(1) + " " + dataReader.GetValue(2);
            
                cmb.Items.Add(uye);

                if (egitmenId == dataReader.GetValue(0).ToString())
                    cmb.SelectedIndex = sira;

                sira++;
            }

            dataReader.Close();
        }

        private void comboBoxUyeFakulte_Click(object sender, EventArgs e)
        {

            fakulteGetir(comboBoxUyeFakulte);
        }

        void fakulteGetir(ComboBox cmb, string fakulteId="")
        {
            cmb.Items.Clear();
            sql = "Select * from Fakulte";
            command = new SqlCommand(sql, cnn);
            dataReader = command.ExecuteReader();
            int sira = 0;
            while (dataReader.Read())
            {
                String uye = dataReader.GetValue(0) + "--" + dataReader.GetValue(1) + " " + dataReader.GetValue(2);

                cmb.Items.Add(uye);

                if (fakulteId == dataReader.GetValue(0).ToString())
                    cmb.SelectedIndex = sira;

                sira++;
            }

            dataReader.Close();
        }

        private void comboBox1_Click(object sender, EventArgs e)
        {
            uyeGetir(comboBox1);
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
          
            guncellenecekUyeId = dataGridView1.CurrentRow.Cells["uye_id"].Value.ToString();
            textBox4.Text = dataGridView1.CurrentRow.Cells["uye_adi"].Value.ToString();
           textBox3.Text = dataGridView1.CurrentRow.Cells["uye_soyadi"].Value.ToString();
           textBox2.Text = dataGridView1.CurrentRow.Cells["uye_telno"].Value.ToString();
           textBox1.Text = dataGridView1.CurrentRow.Cells["uye_ogr_no"].Value.ToString();

            string t = dataGridView1.CurrentRow.Cells["uye_giristarih"].Value.ToString();
            if(t != "")
            {
                DateTime dt = DateTime.Parse(t);
                dateTimePicker1.Value = dt;
            }
            

            string egtmnn = dataGridView1.CurrentRow.Cells["egitmen_id"].Value.ToString();
            string fakulte = dataGridView1.CurrentRow.Cells["fakulte_id"].Value.ToString();

            fakulteGetir(comboBox2, fakulte);
            egitmenGetir(comboBox3, egtmnn);
        }

        private void dataGridViewEgitmen_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {

            guncellenecekEgitmenId = dataGridViewEgitmen.CurrentRow.Cells["egitmen_id"].Value.ToString();
            textBox8.Text = dataGridViewEgitmen.CurrentRow.Cells["egitmen_adi"].Value.ToString();
            textBox7.Text = dataGridViewEgitmen.CurrentRow.Cells["egitmen_soyadi"].Value.ToString();
            textBox6.Text = dataGridViewEgitmen.CurrentRow.Cells["egitmen_telno"].Value.ToString();
            textBox5.Text = dataGridViewEgitmen.CurrentRow.Cells["egitmen_ucret"].Value.ToString();
            textBox14.Text = dataGridViewEgitmen.CurrentRow.Cells["egitmen_brans"].Value.ToString();

        }

        private void button8_Click(object sender, EventArgs e)
        {
            egitmenlistele();
        }

        private void button7_Click(object sender, EventArgs e)
        {

                SqlCommand command;
                SqlDataAdapter adapter = new SqlDataAdapter();
                String sql = "";
                sql = "Insert into Egitmenler (egitmen_adi,egitmen_soyadi) values ('" + textBoxEAd.Text + "','" + textBoxESoy.Text + "')SELECT SCOPE_IDENTITY()";

                command = new SqlCommand(sql, cnn);
                adapter.InsertCommand = command;
                int lastId = Convert.ToInt32(adapter.InsertCommand.ExecuteScalar());
                

                sql = "Insert into Egitmen_Detay(egitmen_telno,egitmen_brans,egitmen_ucret,egtmen_id) values (" + Convert.ToInt32(textBoxETel.Text) + ",'" + textBoxEBrans.Text + "'," + Convert.ToInt32(textBoxEUcret.Text) + "," + lastId + ");";

                command = new SqlCommand(sql, cnn);
                adapter.InsertCommand = command;
                adapter.InsertCommand.ExecuteNonQuery();
                command.Dispose();


                egitmenlistele();
            
        }

        private void comboBox4_Click(object sender, EventArgs e)
        {
            egitmenGetir(comboBox4);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            string secilen = comboBox4.Text.ToString();
            if (secilen != "")
            {
                int s = secilen.IndexOf('-');
                string secId = "";
                for (int i = 0; i < s; i++)
                {
                    secId += secilen.ToCharArray()[i].ToString();
                }
                int secilenId = Convert.ToInt32(secId);


                SqlCommand command;
                SqlDataAdapter adapter = new SqlDataAdapter();
                String sql = "";
                sql = "Delete Egitmenler  where egitmen_id = " + secilenId;
                command = new SqlCommand(sql, cnn);
                adapter.DeleteCommand = command;
                adapter.DeleteCommand.ExecuteNonQuery();
                command.Dispose();

                egitmenGetir(comboBox4);
                egitmenlistele();
            }
            else
                MessageBox.Show("Egitmen seç!");
        }

        private void button9_Click(object sender, EventArgs e)
        {
            urunlistele();
        }

        private void button12_Click(object sender, EventArgs e)
        {

            SqlCommand command;
            SqlDataAdapter adapter = new SqlDataAdapter();
            String sql = "";
            sql = "Insert into Urunler (urun_adi,urun_fiyati,urun_adeti,urun_kategorisi) values ('" + textBox23.Text + "'," + Convert.ToInt32(textBox22.Text) + ","+ Convert.ToInt32(textBox21.Text)+",'"+textBox20.Text+"')SELECT SCOPE_IDENTITY()";

            command = new SqlCommand(sql, cnn);
            adapter.InsertCommand = command;
            int lastId = Convert.ToInt32(adapter.InsertCommand.ExecuteScalar());



           

            urunlistele();
        }

        private void button11_Click(object sender, EventArgs e)
        {

            SqlCommand command;
            SqlDataAdapter adapter = new SqlDataAdapter();
            String sql = "";
            sql = "Update Urunler set urun_adi='" + textBox18.Text + "',urun_fiyati=" + Convert.ToInt32(textBox17.Text) + ",urun_adeti=" + Convert.ToInt32(textBox16.Text) + ",urun_kategorisi='" + textBox15.Text + "' where urun_id = " + guncellenecekUrunId;

            command = new SqlCommand(sql, cnn);
            adapter.UpdateCommand = command;
            adapter.UpdateCommand.ExecuteNonQuery();

            urunlistele();
        }

        private void comboBox5_Click(object sender, EventArgs e)
        {
            urunGetir(comboBox5);
        }

        private void button10_Click(object sender, EventArgs e)
        {
            string secilen = comboBox5.Text.ToString();
            if (secilen != "")
            {
                int s = secilen.IndexOf('-');
                string secId = "";
                for (int i = 0; i < s; i++)
                {
                    secId += secilen.ToCharArray()[i].ToString();
                }
                int secilenId = Convert.ToInt32(secId);


                SqlCommand command;
                SqlDataAdapter adapter = new SqlDataAdapter();
                String sql = "";
                sql = "Delete Urunler  where urun_id = " + secilenId;
                command = new SqlCommand(sql, cnn);
                adapter.DeleteCommand = command;
                adapter.DeleteCommand.ExecuteNonQuery();
                command.Dispose();

                urunGetir(comboBox5);
                urunlistele();
            }
            else
                MessageBox.Show("Urun seç!");
        }

        private void button13_Click(object sender, EventArgs e)
        {
            siparislistele();
        }

        private void comboBox6_Click(object sender, EventArgs e)
        {
            comboBox6.Items.Clear();
            sql = "Select * from Urunler";
            command = new SqlCommand(sql, cnn);
            dataReader = command.ExecuteReader();
            while (dataReader.Read())
            {
                String uye = dataReader.GetValue(0) + "-" + dataReader.GetValue(1) + "-" + dataReader.GetValue(2) + "-" + dataReader.GetValue(3) + "-" + dataReader.GetValue(4);

                comboBox6.Items.Add(uye);
            }

            dataReader.Close();
        }

        private void comboBox6_SelectedValueChanged(object sender, EventArgs e)
        {
            string secilen = comboBox6.Text.ToString();
            string[] secilenSplit = secilen.Split('-');
            int fiyat = Convert.ToInt32(secilenSplit[2]);
            int adet = Convert.ToInt32(secilenSplit[3]);
            int id = Convert.ToInt32(secilenSplit[0]);
            string ad = secilenSplit[1];
            string ktgr = secilenSplit[4];

            labelUrunBilgi.Text = "Urun Ad:"+ad+"\nFiyat:" + fiyat + "\nKategori:" + ktgr + "\nStok:" + adet;

            siparisVerecekUrunId = id;

            secilenUrunFiyat = fiyat;

            toplamFiyat = secilenUrunFiyat * Convert.ToInt32(numericUpDown2.Value.ToString());
            labelToplamFiyat.Text = toplamFiyat + " TL";
        
        
        }

        private void comboBox7_Click(object sender, EventArgs e)
        {
            uyeGetir(comboBox7);
        }

        private void comboBox7_SelectedValueChanged(object sender, EventArgs e)
        {
            string secilen = comboBox7.Text.ToString();
            string[] secilenSplit = secilen.Split('-');
            int id = Convert.ToInt32(secilenSplit[0]);


            siparisVerecekKullaniciId = id;

   

        }

        private void button15_Click(object sender, EventArgs e)
        {

            string secilen = comboBox8.Text.ToString();
            if (secilen != "")
            {
                int s = secilen.IndexOf('-');
                string secId = "";
                for (int i = 0; i < s; i++)
                {
                    secId += secilen.ToCharArray()[i].ToString();
                }
                int secilenId = Convert.ToInt32(secId);


                SqlCommand command;
                SqlDataAdapter adapter = new SqlDataAdapter();
                String sql = "";
                sql = "Delete Siparis_Detay  where siparis_id = " + secilenId;
                command = new SqlCommand(sql, cnn);
                adapter.DeleteCommand = command;
                adapter.DeleteCommand.ExecuteNonQuery();
                command.Dispose();

                siparisGetir(comboBox8);
                siparislistele();
            }
            else
                MessageBox.Show("Sipariş seç!");
        }
        private void siparisGetir(ComboBox cmb)
        {
            cmb.Items.Clear();
            sql = "Select siparis_id,siparis_ucreti,urun_id,uye_id from Siparis_Detay";
            command = new SqlCommand(sql, cnn);
            dataReader = command.ExecuteReader();
            while (dataReader.Read())
            {
                String uye = dataReader.GetValue(0) + "-" + dataReader.GetValue(1) + "-" + dataReader.GetValue(2) + "-" + dataReader.GetValue(3);

                cmb.Items.Add(uye);
            }

            dataReader.Close();

        }

        private void comboBox8_Click(object sender, EventArgs e)
        {
            siparisGetir(comboBox8);
        }

        private void dataGridViewEgitmen_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void comboBox4_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button14_Click(object sender, EventArgs e)
        {
            string secilenUrun = comboBox6.Text.ToString();
            if (secilenUrun != "")
            {
                string secilenMus = comboBox7.Text.ToString();
                if (secilenMus != "")
                {



                    SqlCommand command;
                    SqlDataAdapter adapter = new SqlDataAdapter();
                    String sql = "";
                    sql = "Insert into Siparis_Detay (siparis_ucreti,urun_id,uye_id) values (" + toplamFiyat + "," + siparisVerecekUrunId + "," + siparisVerecekKullaniciId+")SELECT SCOPE_IDENTITY()";
                    command = new SqlCommand(sql, cnn);
                    adapter.InsertCommand = command;
                    int lastId = Convert.ToInt32(adapter.InsertCommand.ExecuteScalar());

                    siparislistele();
                    

                    sql = "Update Urunler set urun_adeti= urun_adeti - "+numericUpDown2.Value+" where urun_id="+siparisVerecekUrunId;

                    command = new SqlCommand(sql, cnn);
                    adapter.UpdateCommand = command;
                    adapter.UpdateCommand.ExecuteNonQuery();


                    comboBox6.Items.Clear();
                    labelUrunBilgi.Text = "Urun Ad:" +"" + "\nFiyat:" + "" + "\nKategori:" + "" + "\nStok:" + "";
                    numericUpDown2.Value = 1;

                }
                else
                    MessageBox.Show("Musteri seç!");
            }
            else
                MessageBox.Show("Urun seç!");
        }

        private void numericUpDown2_ValueChanged(object sender, EventArgs e)
        {

            toplamFiyat = secilenUrunFiyat * Convert.ToInt32(numericUpDown2.Value.ToString());
            
            labelToplamFiyat.Text = toplamFiyat + " TL";

        }

        private void dataGridViewUrun_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            guncellenecekUrunId = dataGridViewUrun.CurrentRow.Cells["urun_id"].Value.ToString();
            textBox18.Text = dataGridViewUrun.CurrentRow.Cells["urun_adi"].Value.ToString();
            textBox17.Text = dataGridViewUrun.CurrentRow.Cells["urun_fiyati"].Value.ToString();
            textBox16.Text = dataGridViewUrun.CurrentRow.Cells["urun_adeti"].Value.ToString();
            textBox15.Text = dataGridViewUrun.CurrentRow.Cells["urun_kategorisi"].Value.ToString();
        }

        private void button6_Click(object sender, EventArgs e)
        {

                SqlCommand command;
                SqlDataAdapter adapter = new SqlDataAdapter();
                String sql = "";
                sql = "Update Egitmenler set egitmen_adi='" + textBox8.Text + "',egitmen_soyadi='" + textBox7.Text+ "' where egitmen_id = " + guncellenecekEgitmenId;


                command = new SqlCommand(sql, cnn);
                adapter.UpdateCommand = command;
                adapter.UpdateCommand.ExecuteNonQuery();


             

                sql = "Update Egitmen_Detay set egitmen_telno=" + Convert.ToInt32(textBox6.Text) + ",egitmen_ucret=" + Convert.ToInt32(textBox5.Text) + ",egitmen_brans='" + textBox14.Text + "' where egtmen_id = " + guncellenecekEgitmenId;

                command = new SqlCommand(sql, cnn);
                adapter.UpdateCommand = command;
                adapter.UpdateCommand.ExecuteNonQuery();

                command.Dispose();


                egitmenlistele();
        }
    }
}
