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
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }
        private void button1_Click(object sender, EventArgs e) { 
        string ciphertext = richTextBox1.Text.ToUpper().Replace(" ", "");
        int keyLength = FindKeyLength(ciphertext);
        label3.Text = $"Lungimea estimată a cheii: {keyLength}";
        string key = CrackKey(ciphertext, keyLength);
        textBox1.Text = key;
            string plaintext = Decrypt(ciphertext, key);
            richTextBox2.Text = plaintext;
        }
        static int FindKeyLength(string ciphertext)
        {
            var distances = FindRepeatedSequences(ciphertext);
            if (!distances.Any()) return 3;
            var possibleLengths = new Dictionary<int, int>();
            foreach (int distance in distances)
            {
                for (int i = 2; i <= Math.Min(20, distance); i++)
                {
                    if (distance % i == 0)
                    {
                        if (!possibleLengths.ContainsKey(i))
                            possibleLengths[i] = 0;
                        possibleLengths[i]++;
                    }
                }
            }
            var candidateLength = possibleLengths
            .OrderByDescending(x => x.Value)
            .Take(3)
            .Select(x => x.Key)
            .ToList();
            foreach (int len in candidateLength)
            {
                double avgIC = 0;
                for (int i = 0; i < len; i++)
                {
                    string segment = string.Concat(ciphertext.Where((c, index) => index % len == i));
                    avgIC += CalculateIndexOfCoincidence(segment);
                }
                avgIC /= len;
                if (avgIC > 0.06)
                    return len;
            }

            return candidateLength.FirstOrDefault();
        }
        static List<int> FindRepeatedSequences(string text, int minLength = 3, int maxLength = 5)
        {
            var distances = new List<int>();
            for (int length = minLength; length <= maxLength; length++)
            {
                var sequences = new Dictionary<string, List<int>>();

                for (int i = 0; i <= text.Length - length; i++)
                {
                    string seq = text.Substring(i, length);
                    if (!sequences.ContainsKey(seq))
                        sequences[seq] = new List<int>();
                    sequences[seq].Add(i);
                }
                foreach (var sequence in sequences)
                {
                    if (sequence.Value.Count < 2) continue;
                    for (int i = 1; i < sequence.Value.Count; i++)
                    {
                        int distance = sequence.Value[i] - sequence.Value[i - 1];
                        if (distance > 0)
                            distances.Add(distance);
                    }
                }
            }
            return distances;
        }
        static double CalculateIndexOfCoincidence(string text)
        {
            int[] frequencies = new int[26];
            foreach (char c in text)
                frequencies[c - 'A']++;
            int n = text.Length;
            double ic = 0;
            foreach (int freq in frequencies)
                ic += freq * (freq - 1);

            return ic / (n * (n - 1));
        }
        static string CrackKey(string ciphertext, int keyLength)
        {
            StringBuilder key = new StringBuilder();
            for (int i = 0; i < keyLength; i++)
            {
                string segment = string.Concat(ciphertext.Where((c, index) => index % keyLength == i));
                char keyChar = CrackKeyForSegment(segment);
                key.Append(keyChar);
            }
            return key.ToString();
        }
        static char CrackKeyForSegment(string segment)
        {
            int[] frequencies = new int[26];
            foreach (char c in segment)
                frequencies[c - 'A']++;

            int total = segment.Length;
            double[] englishFrequencies = {
            0.15361, 0.01323, 0.06035, 0.03734, 0.10494, 0.01005,
            0.00989, 0.00551, 0.12995, 0.00228, 0.00001, 0.04252,
            0.03888, 0.06278, 0.03771, 0.03104, 0.00000, 0.05257,
            0.06181, 0.06283, 0.06342, 0.01212, 0.00000, 0.00006,
            0.00000, 0.00783
        }; // am utilizat frecventele literelor in limba romana dar e scris englishFrequencies
            double maxScore = double.MinValue;
            char bestChar = 'A';
            for (int shift = 0; shift < 26; shift++)
            {
                double score = 0;
                for (int i = 0; i < 26; i++)
                {
                    int index = (i + shift) % 26;
                    score += englishFrequencies[i] * (frequencies[index] / (double)total);
                }

                if (score > maxScore)
                {
                    maxScore = score;
                    bestChar = (char)('A' + shift);
                }
            }
            return bestChar;
        }
        static string Decrypt(string ciphertext, string key)
        {
            StringBuilder plaintext = new StringBuilder();
            for (int i = 0; i < ciphertext.Length; i++)
            {
                int keyIndex = i % key.Length;
                char decryptedChar = (char)((ciphertext[i] - key[keyIndex] + 26) % 26 + 'A');
                plaintext.Append(decryptedChar);
            }
            return plaintext.ToString();
        }
    }
}
