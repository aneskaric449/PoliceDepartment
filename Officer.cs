using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using System.Windows.Forms;

namespace PoliceDepartment
{
    public partial class Officer : Form
    {
        public Officer()
        {
            InitializeComponent();
            this.CenterToScreen();
            ShowMembers();
            LoadRanksIntoCb();
        }

        static string connstring = "Server=localhost; Port=3306;Database=dbpd;Uid=root;Pwd=Sutlija1312-";
        MySqlConnection conn = new MySqlConnection(connstring);

        int selectedID;

        private void button10_Click(object sender, EventArgs e)
        {
            if(NameTb.Text == "" || RankCb.SelectedIndex < 0 || EmailTb.Text == "" || UserTb.Text == "" || PassTb.Text == "")
            {
                Message.GetInstance().SetText("Missing information!");
            }
            else
            {
                try
                {
                    conn.Open();
                    string query = @"INSERT INTO members (ID, FullName, Birth, Email, Ranking, Username, Secret) 
                                                        VALUES (@param0, @param1, @param2, @param3, @param4, @param5, @param6)";
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@param0", GetLastID() + 1);
                    cmd.Parameters.AddWithValue("@param1", NameTb.Text);
                    cmd.Parameters.AddWithValue("@param2", dateTimePicker1.Value.Date);
                    cmd.Parameters.AddWithValue("@param3", EmailTb.Text);
                    cmd.Parameters.AddWithValue("@param4", RankCb.SelectedIndex + 1);
                    cmd.Parameters.AddWithValue("@param5", UserTb.Text);
                    cmd.Parameters.AddWithValue("@param6", PassTb.Text);

                    cmd.ExecuteNonQuery();
                    conn.Close();

                    Message.GetInstance().SetText("Member recorded!");
                    ShowMembers();
                }
                catch(MySqlException ex) 
                {
                    MessageBox.Show(ex.ToString());  
                }
                finally
                {
                    if (conn.State == System.Data.ConnectionState.Open)
                    {
                        conn.Close();
                    }
                }
            }
        }

        private void ShowMembers()
        {
            conn.Open();
            string query = "SELECT * FROM members";
            MySqlDataAdapter sda = new MySqlDataAdapter(query, conn);
            MySqlCommandBuilder builder = new MySqlCommandBuilder(sda);
            DataSet ds = new DataSet();
            sda.Fill(ds);
            MembersDGV.DataSource = ds.Tables[0];
            conn.Close();
        }

        private int GetLastID()
        {
            try
            {
                string query = "SELECT MAX(ID) FROM members";
                MySqlCommand cmd = new MySqlCommand(query, conn);

                object result = cmd.ExecuteScalar();

                if (result == DBNull.Value) return 0;
             
                return Convert.ToInt32(result);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return 0;
            }
        }

        private void LoadRanksIntoCb()
        {
            try
            {
                conn.Open();
                string query = "SELECT rank_name from ranks ORDER BY rank_id";

                MySqlCommand cmd = new MySqlCommand(query, conn);
                MySqlDataReader reader = cmd.ExecuteReader();   
                while (reader.Read())
                {
                    RankCb.Items.Add(reader["rank_name"].ToString());
                }
            }
            catch(Exception ex) 
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                conn.Close();
            }
        }

        private void MembersDGV_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow selectedRow = MembersDGV.Rows[e.RowIndex];

                NameTb.Text = selectedRow.Cells[1].Value.ToString();
                RankCb.SelectedIndex = Convert.ToInt32(selectedRow.Cells[2].Value) - 1;
                dateTimePicker1.Text = selectedRow.Cells[3].Value.ToString();
                EmailTb.Text = selectedRow.Cells[4].Value.ToString();
                UserTb.Text = selectedRow.Cells[5].Value.ToString();
                PassTb.Text = selectedRow.Cells[6].Value.ToString();

                selectedID = Convert.ToInt32(selectedRow.Cells[0].Value);
            }
        }

        private void button9_Click(object sender, EventArgs e)
        {
            if (NameTb.Text == "" || RankCb.SelectedIndex < 0 || EmailTb.Text == "" || UserTb.Text == "" || PassTb.Text == "")
            {
                Message.GetInstance().SetText("Missing information!");
            }
            else
            {
                try
                {
                    conn.Open();
                    string query = @"UPDATE members 
                                    SET FullName = @param0, Birth = @param1, Email = @param2, Ranking = @param3, Username = @param4, Secret = @param5
                                    WHERE ID = @selectedid";
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@param0", NameTb.Text);
                    cmd.Parameters.AddWithValue("@param1", dateTimePicker1.Value.Date);
                    cmd.Parameters.AddWithValue("@param2", EmailTb.Text);
                    cmd.Parameters.AddWithValue("@param3", RankCb.SelectedIndex + 1);
                    cmd.Parameters.AddWithValue("@param4", UserTb.Text);
                    cmd.Parameters.AddWithValue("@param5", PassTb.Text);

                    cmd.Parameters.AddWithValue("@selectedid", selectedID);

                    cmd.ExecuteNonQuery();
                    conn.Close();

                    Message.GetInstance().SetText("Member updated!");
                    ShowMembers();
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show(ex.ToString());
                }
                finally
                {
                    if (conn.State == System.Data.ConnectionState.Open)
                    {
                        conn.Close();
                    }
                }
            }
        }
    }
}
