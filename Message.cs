using Org.BouncyCastle.Asn1.Crmf;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PoliceDepartment
{
    public partial class Message : Form
    {
        private static Message instance = null;  
        public Message()
        {
            InitializeComponent();
            this.CenterToScreen();
        }
       
        public static Message GetInstance()
        {
            if(instance == null)
            {
                instance = new Message();
                instance.Show();
                instance.BringToFront();
                instance.FormClosed += delegate { instance = null; };
            }
            else
            {
                instance.BringToFront();
            }
            return instance;
        }

        public void SetText(string text)
        {
            msgText.Text = text;
        }
    }
}
