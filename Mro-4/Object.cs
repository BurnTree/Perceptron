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
    class ObjectImg
    {

        const int T = 0;
        //const int Len = 50;
        const int Per = 10;
        public int[,] MassImg;
        public int[,] MassSoed;
        public int[,] MassPer;
        public int[] lineY;
        public int[] soed;
        public int[] sum;
        public int kl;
        public int[] kod;
        public Bitmap bmp;

        public ObjectImg(Image img)
        {
            bmp = new Bitmap(img);
            Random rand = new Random();

            MassImg = new int[bmp.Width, bmp.Height];
            MassSoed = new int[bmp.Width, bmp.Height];
            soed = new int[(MassImg.GetLength(0) * MassImg.GetLength(1)) / Per];
            sum = new int[soed.Length];
            lineY = new int[soed.Length];
            MassPer = new int[MassImg.GetLength(0) * MassImg.GetLength(1), soed.Length];
            for (int i = 0; i < MassSoed.GetLength(0); i++)
                for (int j = 0; j < MassSoed.GetLength(1); j++)
                {
                    MassSoed[i, j] = (int)(((i * MassSoed.GetLength(1)) + j) / Per);
                }
            int ost = 0;
            for (int i = 0; i < MassPer.GetLength(0); i++)
                for (int j = 0; j < MassPer.GetLength(1); j++)
                {
                    if (MassSoed[Math.DivRem(i, 50, out ost), ost] == j)
                    {
                        if (rand.Next(0, 2) == 0)
                            MassPer[i, j] = 1;
                        else
                            MassPer[i, j] = -1;
                    }
                    else
                        MassPer[i, j] = 0;
                }
        }

        public int[,] ImgToMat(Bitmap img)
        {
            int[,] mat = new int[img.Width, img.Height];
            for (int ik = 0; ik < img.Height; ik++)
                for (int jk = 0; jk < img.Width; jk++)
                {
                    Color c1 = img.GetPixel(jk, ik);
                    if (c1.R > 0)
                        mat[ik, jk] = 0;
                    else
                        mat[ik, jk] = 1;
                }
            return mat;
        }

        public void defineKod()
        {
            kod = new int[3];    
            switch (kl)
            {
                case 1:
                    kod[0] = 1;
                    kod[1] = 1;
                    kod[2] = 1;
                    break;
                case 2:
                    kod[0] = 0;
                    kod[1] = 1;
                    kod[2] = 1;
                    break;
                case 3:
                    kod[0] = 1;
                    kod[1] = 1;
                    kod[2] = 0;
                    break;
                case 4:
                    kod[0] = 1;
                    kod[1] = 0;
                    kod[2] = 0;
                    break;
                case 5:
                    kod[0] = 0;
                    kod[1] = 0;
                    kod[2] = 0;
                    break;
                default:
                    //нам хана
                    break;
            }
        }

        public void findKod(int[] k)
        {
            kl = 0;
            if (k[0] == 1 && k[1] == 1 && k[2] == 1)
                kl = 1;
            if (k[0] == 0 && k[1] == 1 && k[2] == 1)
                kl = 2;
            if (k[0] == 1 && k[1] == 1 && k[2] == 0)
                kl = 3;
            if (k[0] == 1 && k[1] == 0 && k[2] == 0)
                kl = 4;
            if (k[0] == 0 && k[1] == 0 && k[2] == 0)
                kl = 5;

        }

        public void sumAndY()
        {
            int ost = 0;
            for (int j = 0; j < sum.Length; j++)
            {
                sum[j] = 0;
                for (int i = 0; i < MassPer.GetLength(0); i++)
                    sum[j] += MassPer[i, j] * MassImg[Math.DivRem(i, 50, out ost), ost];
                if (sum[j] >= T)
                    lineY[j] = 1;
                else
                    lineY[j] = 0;
            }

        }

        public void griding(DataGridView d, int[,] mat)
        {
            d.ColumnCount = mat.GetLength(1) + 1;
            for (int i = 0; i < mat.GetLength(1); i++)
                d.Columns[i].Width = 20;
            for (int i = 0; i < mat.GetLength(0); i++)
            {
                d.Rows.Add("x" + i);

                //d.Rows[i].Cells[0].Value = i;
            }
            for (int i = 0; i < mat.GetLength(0); i++)
                for (int j = 0; j < mat.GetLength(1); j++)
                {
                    d.Rows[i].Cells[j + 1].Value = mat[i, j];
                }
        }

        public void addLine(DataGridView d, int[] mas, string name)
        {

            d.ColumnCount = mas.Length + 1;
            for (int i = 0; i < mas.Length; i++)
                d.Columns[i].Width = 20;
            d.Rows.Add(name);
            for (int i = 0; i < mas.Length; i++)
                d.Rows[d.Rows.Count - 2].Cells[i + 1].Value = mas[i];
        }
    }   
}
