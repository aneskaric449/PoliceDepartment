using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql;
using MySqlX.XDevAPI.Common;

namespace PoliceDepartment
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            this.CenterToScreen();
        }

        string connstring = "Server=localhost; Port=3306;Database=dbpd;Uid=root;Pwd=Sutlija1312-";
        Message currentMsgBox;
        bool isPassVisible = false;

        private void button1_Click(object sender, EventArgs e)
        {
            string user = textBox1.Text;
            string pass = textBox2.Text;

            try
            {
                MySqlConnection conn = new MySqlConnection(connstring);
                conn.Open();

                string query = @"SELECT
                                    CASE
                                        WHEN EXISTS (
                                            SELECT * FROM dbpd.members WHERE Username = @param1
                                        ) AND EXISTS (
                                            SELECT * FROM dbpd.members WHERE Username = @param1 AND Secret = @param2
                                        ) THEN 'Logged in!'
                                        WHEN EXISTS (
                                            SELECT * FROM dbpd.members WHERE Username = @param1
                                        ) THEN 'Incorrect password'
                                        ELSE 'User does not exist'
                                    END AS result
                                ";

                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@param1", user);
                cmd.Parameters.AddWithValue("@param2", pass);

                MySqlDataReader reader = cmd.ExecuteReader();

                string result = string.Empty;

                while (reader.Read())
                {
                    result = reader["result"].ToString();
                    Message.GetInstance().SetText(result);          
                }
                reader.Close();
                if (result == "Logged in!" && comboBox1.SelectedIndex == 0)
                {
                    string query_ = "SELECT * FROM dbpd.members WHERE Username = @param0 AND Secret = @param1 AND Ranking > 5";
                    MySqlCommand cmd_ = new MySqlCommand(query_, conn);
                    cmd_.Parameters.AddWithValue("@param0", user);
                    cmd_.Parameters.AddWithValue("@param1", pass);
                    reader = cmd_.ExecuteReader();
                    if (!reader.HasRows)
                    {
                        Message.GetInstance().SetText("Insufficient permissions!");
                    }
                }
            }
            catch (MySqlException ex)
            {
                MessageBox.Show(ex.ToString());
            }
           
        }

        private void pictureBoxPass_Click(object sender, EventArgs e)
        {
            pictureBoxPass.Image = isPassVisible ? Properties.Resources.icons8_invisible_60 : Properties.Resources.icons8_visible_60;
            isPassVisible = !isPassVisible;
            textBox2.UseSystemPasswordChar = !textBox2.UseSystemPasswordChar;
        }
    }
}
