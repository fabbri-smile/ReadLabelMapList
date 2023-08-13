using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using Sprache;

namespace ReadLabelMapList
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
#if false
            textBox1.Text = "item {\r\n"
                          + "   name: \"/m/01g317\"\r\n"
                          + "   id: 1\r\n"
                          + "   display_name: \"person\"\r\n"
                          + "}\r\n"
                          + "item {\r\n"
                          + "   name: \"/m/0199g\"\r\n"
                          + "   id: 2\r\n"
                          + "   display_name: \"bicycle\"\r\n"
                          + "}\r\n";
#endif
        }

        private void button1_Click(object sender, EventArgs e)
        {
#if true
            clsLabelMapList list = new clsLabelMapList();

            if (true != list.ReadLabelMapTextFile(@".\label_map.txt")) return;

            textBox1.Text = list.GetDisplayName(52);
#else
            string input = textBox1.Text;

            textBox2.Text = string.Empty;

            clsLabelMapList list = clsLabelMapList.LabelMapListParser.Parse(input);
#endif

            foreach (clsLabelMapItem item in list.LabelMapList)
            {
                textBox2.Text += $"{item.id} : {item.display_name}\r\n";
            }
        }
    }
}