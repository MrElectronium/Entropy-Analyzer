using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EntropyAnalyzer
{
    public partial class Form1 : Form
    {
        byte[] arr;
        double[] ftbl = new double[256];
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofn = new OpenFileDialog();
            ofn.Filter = "All Files(*.*)|*.*";
            if (ofn.ShowDialog() == DialogResult.OK)
            {
                arr=File.ReadAllBytes(ofn.FileName);
                                
                for (int i = 0; i < 256; i++)
                    ftbl[i] = 0;
                for (int i = 0; i < arr.Length; i++)
                    ftbl[arr[i]] += 1;
                chart1.Series[0].Points.Clear();
                double len = arr.Length;
                double num = 0;
                double Entropy = 0;
                for (int i = 0; i < 256; i++)
                {
                    chart1.Series[0].Points.AddXY(i,ftbl[i]);
                    num = ftbl[i]/len;
                    if (num == 0)
                        num = 1 / (1 << 64);
                    Entropy += -num * Math.Log(num, 2);
                }
                Entropy = Math.Round(Entropy, 7);
                label2.Text = Entropy.ToString();
                label4.Text = Convert.ToString(arr.Length);
            }

        }

        private void button2_Click(object sender, EventArgs e)
        {
          
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            if (arr == null) return;
            int coeff = 0;
            coeff = Convert.ToInt32(textBox1.Text);
            for (int i = 0; i < arr.Length; i++)
            {
                arr[i] = (byte)((int)arr[i] - coeff);
            }
            
            ftbl = new double[256];
            for (int i = 0; i < 256; i++)
                ftbl[i] = 0;
            for (int i = 0; i < arr.Length; i++)
                ftbl[arr[i]] += 1;
            chart1.Series[0].Points.Clear();
            double len = arr.Length;
            double num = 0;
            double Entropy = 0;
            listBox1.Items.Clear();
            for (int i = 0; i < 256; i++)
            {
                chart1.Series[0].Points.AddXY(i, ftbl[i]);
                 num = ftbl[i] / len;                
                if (num == 0)
                    num = 1 / (1 << 64);
                Entropy += -num * Math.Log(num, 2);
            }
            
            Entropy = Math.Round(Entropy, 7);
            label2.Text = Entropy.ToString();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (arr == null)
            {
                MessageBox.Show("Önce bir dosya açıp analiz yaptırmalısınız.","Hata");
                return;
            }
            double max;
            int maxi;
            int cum_freq=0;
            listBox1.Items.Clear();
            string s="";
            int maxfreq = 4;
            maxfreq = Convert.ToInt32(textBox1.Text);
            double[] ftbl2 = new double[256];
            Array.Copy(ftbl, ftbl2, 256);
            int clen;
            for (int j = 0; j < maxfreq; j++)
            {
                max = 0;
                maxi = 0;
                s = "";
                for (int i = 0; i < 256; i++)
                {
                 if(ftbl2[i]>max)
                    {
                        max = ftbl2[i];
                        maxi = i;
                    }    
                }
                s = String.Format("Sym:{0} Freq:{1}", maxi, max);
                cum_freq += (int)max;
                listBox1.Items.Add(s);
                ftbl2[maxi] =0;
            }
            clen =(int)((double)cum_freq * (Math.Log((double)maxfreq, 2)+1) / 8 + (double)(arr.Length - cum_freq) * 9 / 8 + 256 + 20);
            label5.Text = String.Format("Kümülatif Frekans:{0}"
               , cum_freq);
        }
    }
}
