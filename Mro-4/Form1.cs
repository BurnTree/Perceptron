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


namespace Mro_4
{
    public partial class Form1 : Form
    {
        const int T = 1;
        ObjectImg myPic;
        List<Lymbda> lym;

        public Form1()
        {
            InitializeComponent();
            pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
            lym = new List<Lymbda>();
            Image img = new Bitmap(@"..\bmp\1\1.bmp");
            ObjectImg myPic = new ObjectImg(img);
            lym.Add(new Lymbda(myPic.lineY.Length));
            lym.Add(new Lymbda(myPic.lineY.Length));
            lym.Add(new Lymbda(myPic.lineY.Length));
        }
        
        private void button1_Click(object sender, EventArgs e)
        {
            
            pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = ("Image Files(*.BMP)|*.BMP");
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    pictureBox1.Image = new Bitmap(ofd.FileName);
                    myPic = new ObjectImg(pictureBox1.Image);
                    myPic.MassImg = myPic.ImgToMat(myPic.bmp);
                    myPic.bmp = new Bitmap(pictureBox1.Image);
                    myPic.griding(dataGridView1, myPic.MassImg);
                    fuckYoo(dataGridView2, myPic.MassImg);
                    findImg();
                }
                catch
                {
                    MessageBox.Show("Невозможно открыть файл", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }                
            }     
        }


        private void fuckYoo(DataGridView d, int[,] mas)
        {
            int ost = 0;
            for (int i = 1; i < (mas.GetLength(0) * mas.GetLength(0)); i++)
            {
                d.Rows[i].Cells[0].Value = "x" + i + " =" + mas[Math.DivRem(i, 20, out ost), ost];
            }

        }


        private void button2_Click(object sender, EventArgs e)
        {
            ObjectImg Img = new ObjectImg(new Bitmap(@"..\bmp\1\1.bmp"));
            recognize(Img);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            ObjectImg Img = new ObjectImg(new Bitmap(@"..\bmp\1\1.bmp"));
            readFile();
            recognize(Img);
        }

        private void recognize(ObjectImg Img)
        {
            int time = Convert.ToInt32(textBox1.Text);
            string str;
            int len;
            Random rand = new Random();
            Img.griding(dataGridView2, Img.MassPer);
            for (int j = 0; j < time; j++)
            {
                //  выбираем картинку     //
                Img.kl = rand.Next(0, 3) + 1;
                Text = new DirectoryInfo(@"..\bmp\" + Img.kl).GetFiles().Length.ToString();
                len = Convert.ToInt32(Text);
                pictureBox1.Image = new Bitmap(@"..\bmp\" + Img.kl + @"\" + (rand.Next(0, len) + 1) + ".bmp");
                Img.bmp = new Bitmap(pictureBox1.Image);
                //  находим составляющие  //
                Img.MassImg = Img.ImgToMat(Img.bmp);
                Img.sumAndY();
                Img.addLine(dataGridView2, Img.sum, "sum");
                Img.addLine(dataGridView2, Img.lineY, "Y");
                Img.defineKod();
                //  вычисляем лямбды     //
                int ourkl = 0;
                int maxS = -1000;
                for (int i = 0; i < 3; i++)
                {
                    lym[i].s = findS(lym[i].mass, Img.lineY);
                    if (lym[i].s > maxS)
                    {
                        maxS = lym[i].s;
                        ourkl = i+1;
                    }
                }
                if(Img.kl != ourkl)
                {

                    for (int i = 0; i < Img.lineY.Length; i++)
                    {
                        lym[Img.kl - 1].mass[i] += Img.lineY[i];
                        lym[ourkl - 1].mass[i] -= Img.lineY[i];
                    }
                }
                Img.addLine(dataGridView3, lym[0].mass, "lym");
                Img.addLine(dataGridView4, lym[1].mass, "lym");
                Img.addLine(dataGridView5, lym[2].mass, "lym");
            }

            pictureBox1.Image = new Bitmap(@"..\bmp\OK.bmp");

            writeFile();
        }

        private int findSigma(int[] ly, int[] y)
        {
            int s = 0;
            for(int i = 0; i< ly.Length;i++)
            {
                s += ly[i] * y[i];
            }

            if (s >= 0)
                return 1;
            else
                return 0; 
        }

        private int findS(int[] ly, int[] y)
        {
            int s = 0;
            for (int i = 0; i < ly.Length; i++)
            {
                s += ly[i] * y[i];
            }
            return s;
        }

        private int[] findLym(int[] ly)
        {
            for(int i = 0; i < ly.Length; i++)
            {
                ly[i] = 1;
            }
            return ly;
        }        

        private void writeFile()
        {
            string writeTxt;
            string text;
            DataGridView d = new DataGridView();
            for (int k = 0; k < 3; k++)
            {
                writeTxt = @"..\bmp\lym"+ (k+1) +".txt";
                text = "";
                if (k == 0) d = dataGridView3;
                if (k == 1) d = dataGridView4;
                if (k == 2) d = dataGridView5;
                for (int i = 1; i < d.ColumnCount; i++)
                    text += d[i, d.Rows.Count - 2].Value.ToString() + "\n";
                try
                {
                    using (StreamWriter sw = new StreamWriter(writeTxt, false, System.Text.Encoding.Default))
                    {
                        sw.WriteLine(text);
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
        }

        private void findImg()
        {
            readFile();
            myPic.sumAndY();
            for (int i = 0; i < 3; i++)
                lym[i].s = findS(lym[i].mass, myPic.lineY);
            myPic.kl = 0;
            int maxS = -1000;
            for (int i = 0; i < 3; i++)
            {
                lym[i].s = findS(lym[i].mass, myPic.lineY);
                if (lym[i].s > maxS)
                {
                    maxS = lym[i].s;
                    myPic.kl = i + 1;
                }
            }
            pictureBox2.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox3.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox2.Image = new Bitmap(myPic.bmp);
            pictureBox3.Image = new Bitmap(@"..\bmp\" + myPic.kl + @"\1.bmp");
            for (int i = 0; i < 3; i++)
                label3.Text += " sum" + (i + 1) + " = " + lym[i].s + " ";
            label3.Text += "\nklass = " + myPic.kl;         
        }

        private void readFile()
        {
            for (int k = 0; k < 3; k++)
            {
                string path = @"..\bmp\lym" + (k + 1) + ".txt";
                try
                {
                    using (StreamReader sr = new StreamReader(path, System.Text.Encoding.Default))
                    {
                        string line;
                        for (int i = 0; i < lym[k].mass.Length; i++)
                        {
                            lym[k].mass[i] = Convert.ToInt32(sr.ReadLine());
                        }
                    }

                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
            /*myPic.addLine(dataGridView3, lym[0].mass, "lym");
            myPic.addLine(dataGridView4, lym[1].mass, "lym");
            myPic.addLine(dataGridView5, lym[2].mass, "lym");*/
        }
    }
}
