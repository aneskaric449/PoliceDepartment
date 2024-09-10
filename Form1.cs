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

namespace PoliceDepartment
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        string connstring = "Server=localhost; Port=3306;Database=dbpd;Uid=root;Pwd=Sutlija1312-";

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
                                            SELECT * FROM dbpd.members WHERE username = @param1
                                        ) AND EXISTS (
                                            SELECT * FROM dbpd.members WHERE username = @param1 AND pass = @param2
                                        ) THEN 'Logged in!'
                                        WHEN EXISTS (
                                            SELECT * FROM dbpd.members WHERE username = @param1
                                        ) THEN 'Incorrect password'
                                        ELSE 'User does not exist'
                                    END AS result
                                ";

                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@param1", user);
                cmd.Parameters.AddWithValue("@param2", pass);

                MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    string result = reader["result"].ToString();
                    //MessageBox.Show(result);
                    
                    if(result == "Logged in!" && comboBox1.SelectedIndex == 0)
                    {
                        string query_ = "SELECT * FROM dbpd.members WHERE username = @param1 AND pass = @param2 AND rank_ref > 5";
                        MySqlCommand cmd_ = new MySqlCommand(query_, conn);
                        cmd_.Parameters.AddWithValue("@param1", user);
                        cmd_.Parameters.AddWithValue("@param2", pass);

                        MySqlDataReader reader_ = cmd_.ExecuteReader();

                        if(!reader_.HasRows)
                        {
                            MessageBox.Show("Insufficient permissions!");
                        }
                    }
                }
            }
            catch (MySqlException ex)
            {
                MessageBox.Show(ex.ToString());
            }
           
        }
    }
}
