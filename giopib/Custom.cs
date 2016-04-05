using giopib.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace giopib
{
    public partial class Custom : Form
    {
        public static List<Point> Coords = new List<Point>();
        public static List<int> Pillows = new List<int> { 14279223, 14414759, 14235012, 14231082, 14636593, 14220749, 14318416, 14573136, 14304517, 14230816, 14279016, 14232584 };
        public Main MainForm { get; set; }
        public Custom(Main m)
        {
            MainForm = m;
            InitializeComponent();
            Size = new Size((6 * 30) + 16, (6 * 30) + 100);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            int x = 0;
            int y = 0;

            for (int i = 0; i < 36; i++)
            {
                PictureBox pb = new PictureBox();
                pb.Size = new Size(30, 30);
                pb.SizeMode = PictureBoxSizeMode.CenterImage;
                pb.BackColor = Color.DarkGray;
                pb.Location = new Point(x, y);
                pb.BorderStyle = BorderStyle.FixedSingle;
                Controls.Add(pb);
                x += 30;

                if (x == (30 * 6))
                {
                    y += 30;
                    x = 0;
                }
            }
        }

        public async void UpdatePercentage(int p)
        {
            PBar.Value = p;

            if (p == 12)
            {
                MainForm.Triggers.InDetach(Settings.Default.Rotate);
                MainForm.CustomHitBx.Checked = true;
                MainForm.CustomHit = true;
                await MainForm.Connection.SendToClientAsync(Settings.Default.Rank, 0, 0, false);
                PBar.Hide();
            }
        }

        public void UpdatePositions(int id, Point oldp, Point newp)
        {
            bool oldFound = false;
            bool newFound = false;
            foreach (Control c in Controls)
            {
                if (c is PictureBox)
                {
                    PictureBox p = c as PictureBox;
                    if (p.Location == new Point((oldp.X - 1) * 30, (oldp.Y - 1) * 30))
                    {
                        if (Main.Coords.Contains(oldp)) Main.Coords.Remove(oldp);
                        p.Image = null;
                        oldFound = true;
                    }

                    if (p.Location == new Point((newp.X - 1) * 30, (newp.Y - 1) * 30))
                    {
                        if (!Main.Coords.Contains(newp)) Main.Coords.Add(newp);
                        p.Image = Resources.pillow;
                        p.Refresh();
                        newFound = true;
                    }

                    if (oldFound && newFound) return;
                }
            }
        }
    }
}
