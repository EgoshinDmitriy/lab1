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
    public partial class Form2 : Form
    {
        public static key kl;
        public Form2(key K)
        {
            InitializeComponent();
            kl = K;
        }

        private int i = 3;

        private void button1_Click(object sender, EventArgs e)
        {
            string oldpas = textBox1.Text;
            string newpas = textBox2.Text;
            string pass;
            FileStream file = new FileStream("pass.txt", FileMode.Open);
            StreamReader reader = new StreamReader(file);
            pass = reader.ReadLine();
            reader.Close();
           /* int[] key = textBox3.Text.Split(' ').Where(x => !string.IsNullOrWhiteSpace(x)).Select(x => int.Parse(x)).ToArray();*/
            oldpas = Encrypt(oldpas, kl.Val);
            if (pass == oldpas)
            {
                FileStream wfile = new FileStream("pass.txt", FileMode.Truncate);
                StreamWriter writer = new StreamWriter(wfile);
                Random rnd = new Random();
                int i = 1 + rnd.Next(100);
                kl.Val = GenerateKeyWord(i);
                string str = "";
                for (i = 0; i < kl.Val.Length; i++)
                {
                    str += kl.Val[i].ToString() + " ";
                }
                newpas = Encrypt(newpas, kl.Val);
                writer.WriteLine(newpas);
                writer.Close();
                MessageBox.Show("Your new key: " + str);
            }
            else
            {
                i--;
                MessageBox.Show("Wrong password or key! You have " + i + " chances!");
            }
        }

        public static char[] characters = new char[] {'a','b','c','d','e','f','g','h','i','j','k','l','m','n','o','p','q','r','s','t','u','v','w','x','y','z',
           'A','B','C','D','E','F','G','H','I','J','K','L','M','N','O','P','Q','R','S','T','U','V','W','X','X','X','0','1','2','3','4','5','6','7','8','9','_'};

        public int N = characters.Length;

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


        private void Form2_FormClosed(object sender, FormClosedEventArgs e)
        {
            Form ifrm = Application.OpenForms[0];
            ifrm.Show();
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }
            private void Form2_FormClosing(object sender, FormClosingEventArgs e)
            {
                Application.Exit();
            }
    }
}
