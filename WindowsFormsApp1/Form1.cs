using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Security.Cryptography;
using System.IO;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace PassAuth
{
    public partial class Form1 : Form
    {


        public Form1()

        {
            InitializeComponent();
            FileInfo fileInf = new FileInfo("pass.txt");
            if (fileInf.Exists)
                groupBox1.Visible = true;
        }

        private int i = 3;

        private void button1_Click(object sender, EventArgs e)
        {

            string pas = textBox1.Text;
            string pass;
            FileInfo fileInf = new FileInfo("pass.txt");
            if (fileInf.Exists)
            {
                FileStream file = new FileStream("pass.txt", FileMode.Open);
                StreamReader reader = new StreamReader(file);
                pass = reader.ReadLine();
                reader.Close();
                key Key = new key();
                //int[] kl = textBox2.Text.Split(' ').Where(x => !string.IsNullOrWhiteSpace(x)).Select(x => int.Parse(x)).ToArray();
                int[] kl = new int[textBox2.Text.Length];
                char[] chArray = textBox2.Text.ToCharArray();
                for (int i = 0; i < textBox2.Text.Length; ++i)
                {
                    kl[i] = char.Parse("" + textBox2.Text[i]);
                }
                Key.Val = kl;
                pas = Encrypt(pas,kl);
                if (pas == pass)
                {
                    Form ifrm = new Form2(Key);
                    ifrm.Show();
                    this.Hide();
                }
                else
                {
                    i--;
                    MessageBox.Show("Wrong password or key! You have " + i + " chances!");
                }
            }
            else
            {
                FileStream file = new FileStream("pass.txt", FileMode.Create);
                StreamWriter writer = new StreamWriter(file);
                Random rnd = new Random();
               
                int i = 1 + rnd.Next(100);
                int[] kl= GenerateKeyWord(i);
                string str = "";
                for (i = 0; i < kl.Length; i++)
                {
                    str += kl[i].ToString() + " ";
                }
                key Key = new key();
                Key.Val = kl;
                pas = Encrypt(pas, kl);
                writer.WriteLine(pas);
                MessageBox.Show("Your key: " + str);
                writer.Close();
                Form ifrm = new Form2(Key);
                ifrm.Show();
                this.Hide();
            }
            if (i == 0)
            {
                this.Close();
            }
        }
        public string Encrypt(string input, int[] key)
        {
            for (int i = 0; i < input.Length % 4; i++)
                input += input[i];

            string result = "";

            for (int i = 0; i < input.Length; i += 4)
            {
                char[] transposition = new char[4];

                for (int j = 0; j < 4; j++)
                    transposition[key[j] - 1] = input[i + j];

                for (int j = 0; j < 4; j++)
                    result += transposition[j];
            }

            return result;
        }

        public int[] GenerateKeyWord(int startSeed)
        {
            int[] key = new int[4];
            Random rand = new Random(startSeed);
            for (int i = 0; i < 4; i++)
                key[i] = rand.Next(1, 4);
            return key;
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
    public class key
    {
        private int[] val;
        public int[] Val
        { 
            get { return val; }
          set { val = value; }
        }
    }
 }


