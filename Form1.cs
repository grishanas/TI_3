using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Numerics;
using System.IO;

namespace TI_3
{
    public partial class Form1 : Form
    {
        RSA rsa;
        Form1 form;
        public Form1()
        {
            InitializeComponent();
            form = this;
        }

        private void button1_Click(object sender, EventArgs e)
        {

            rsa = new RSA();
            rsa.CreateKey();
            form.textBox1.Text = rsa.PrivateKey[0].ToString();
            form.textBox2.Text = rsa.PrivateKey[1].ToString();
            form.textBox3.Text = rsa.Publickey[0].ToString();
            form.textBox4.Text = rsa.Publickey[1].ToString();
            
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (rsa == null)
                rsa = new RSA();
            if (form.textBox1.Text != "" && form.textBox2.Text != "")
            {
                rsa.PrivateKey[0] = BigInteger.Parse(form.textBox1.Text);
                rsa.PrivateKey[1] = BigInteger.Parse(form.textBox2.Text);
            }
            var Dial = new OpenFileDialog();
            Dial.ShowDialog();
            string Message;
            using (StreamReader stream = new StreamReader(Dial.FileName))
            {
                Message = stream.ReadToEnd();

            }
            var temp = rsa.CreateSig(Message);
            MessageBox.Show("Подпись = " + temp );
            form.textBox6.Text = temp.ToString();
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (rsa == null)
                rsa = new RSA();
            if (form.textBox1.Text != "" && form.textBox2.Text != "")
            {
                rsa.Publickey[0] = BigInteger.Parse(form.textBox3.Text);
                rsa.Publickey[1] = BigInteger.Parse(form.textBox4.Text);
            }
            var Dial = new OpenFileDialog();
            Dial.ShowDialog();
            string Message;
            using (StreamReader stream = new StreamReader(Dial.FileName))
            {
                Message = stream.ReadToEnd();

            }
            var Code = BigInteger.Parse(form.textBox6.Text);
            var tmp = rsa.ChekSig(Code, Message);
            if(tmp)
            {
                MessageBox.Show("Все ок");
            }
            else
            {
                MessageBox.Show("Подпись не подходит");
            }
        }
    }
}
