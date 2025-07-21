using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Vigenere
{
    public partial class Form1 : Form
    {
        private const int AlphabetLength = 26;
        private const string Alphabet = "abcdefghijklmnopqrstuvwxyz";
        private readonly char[] Trb = Alphabet.ToUpper().ToCharArray();
        private readonly char[] Trs = Alphabet.ToCharArray();
        private readonly double[] Frs = new double[]
        {
            0.15361, 0.01323, 0.06035, 0.03734, 0.10494, 0.01005,
            0.00989, 0.00551, 0.12995, 0.00228, 0.00001, 0.04252,
            0.03888, 0.06278, 0.03771, 0.03104, 0.00000, 0.05257,
            0.06181, 0.06283, 0.06342, 0.01212, 0.00000, 0.00006,
            0.00000, 0.00783
        };
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
        

      

        private string VigenereEncryptLines(string text, string key)
        {
            key = key.ToLower();
            var lines = text.Split('\n');
            var result = new StringBuilder();

            foreach (var line in lines)
            {
                var encryptedLine = new StringBuilder();
                int keyIndex = 0;

                foreach (char ch in line)
                {
                    if (char.IsLetter(ch))
                    {
                        int textCharIndex = ch >= 'a' && ch <= 'z' ? ch - 'a' : ch - 'A';
                        int keyCharIndex = key[keyIndex % key.Length] - 'a';
                        int encryptedCharIndex = (textCharIndex + keyCharIndex) % 26;

                        char encryptedChar = char.IsUpper(ch) ? (char)(encryptedCharIndex + 'A') : (char)(encryptedCharIndex + 'a');
                        encryptedLine.Append(encryptedChar);
                        keyIndex++;
                    }
                    else
                    {
                        encryptedLine.Append(ch);
                    }
                }

                result.AppendLine(encryptedLine.ToString());
            }

            return result.ToString().TrimEnd();
        }



        private string VigenereDecryptLines(string text, string key)
        {
            key = key.ToLower();
            var lines = text.Split('\n');
            var result = new StringBuilder();

            foreach (var line in lines)
            {
                var decryptedLine = new StringBuilder();
                int keyIndex = 0;

                foreach (char ch in line)
                {
                    if (char.IsLetter(ch))
                    {
                        int textCharIndex = ch >= 'a' && ch <= 'z' ? ch - 'a' : ch - 'A';
                        int keyCharIndex = key[keyIndex % key.Length] - 'a';
                        int decryptedCharIndex = (textCharIndex - keyCharIndex + 26) % 26;

                        char decryptedChar = char.IsUpper(ch) ? (char)(decryptedCharIndex + 'A') : (char)(decryptedCharIndex + 'a');
                        decryptedLine.Append(decryptedChar);
                        keyIndex++;
                    }
                    else
                    {
                        decryptedLine.Append(ch);
                    }
                }

                result.AppendLine(decryptedLine.ToString());
            }

            return result.ToString().TrimEnd();
        }


        string NormalizeText(string text)
        {
            return text.Replace("\r", "").Replace("\n", " ").Trim();
        }



        private void ButtonEncrypt_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(richTextBox1.Text))
            {
                MessageBox.Show("Introduceți textul pentru criptare.");
                return;
            }

            if (string.IsNullOrWhiteSpace(textBox1.Text))
            {
                MessageBox.Show("Introduceți cheia.");
                return;
            }

            string inputText = richTextBox1.Text;
            string key = textBox1.Text;

            string encryptedText = VigenereEncryptLines(inputText, key);

            richTextBox2.Text = encryptedText;
        }

        private void ButtonDecrypt_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(richTextBox1.Text))
            {
                MessageBox.Show("Introduceți textul pentru decriptare.");
                return;
            }

            if (string.IsNullOrWhiteSpace(textBox1.Text))
            {
                MessageBox.Show("Introduceți cheia.");
                return;
            }

            string inputText = richTextBox1.Text;
            string key = textBox1.Text;

            string decryptedText = VigenereDecryptLines(inputText, key);

            richTextBox2.Text = decryptedText;
        }

        private void ButtonClear_Click(object sender, EventArgs e)
        {
            textBox1.Clear();
            richTextBox1.Clear();
            richTextBox2.Clear();
        }

        private string InsertSpaces(string input, int interval)
        {
            var builder = new StringBuilder(input);
            int i = interval;

            while (i < builder.Length)
            {
                builder.Insert(i, ' ');
                i += interval + 1;
            }

            return builder.ToString();
        }

        private void spargereaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form2 form2 = new Form2();
            form2.Show();
        }
    }
}
