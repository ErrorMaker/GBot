using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;
using Sulakore.Communication;
using giopib.Properties;
using Tangine;
using Sulakore.Modules;
using Tangine.Habbo;

namespace giopib
{
    [Module("GBot", "Grijpers/grabbers owned!")]
    [Author("Mika", DonationsUrl = "http://mika.host", HabboName = "ushort", Hotel = Sulakore.Habbo.HHotel.Nl)]
    [Author("Mika", DonationsUrl = "http://mika.host", HabboName = "BlameHabbo", Hotel = Sulakore.Habbo.HHotel.Com)]
    [Author("Wes", HabboName = "Weszzz", Hotel = Sulakore.Habbo.HHotel.Com)]
    [Author("Wes", HabboName = "Nubkoek", Hotel = Sulakore.Habbo.HHotel.Nl)]


    public partial class Form1 : ExtensionForm
    {
        #region Variables
        private List<GGame> GGames = new List<GGame>();
        private GGame CurrentGame { get; set; }
        private List<Panel> LeftPanels = new List<Panel>();
        private List<Panel> RightPanels = new List<Panel>();
        private List<Panel> HitPanels = new List<Panel>();
        private int LastBlueLeftPanel = 0;
        private int LastBlueRightPanel = 0;

        private int LeftID = 0;
        private int RightID = 0;
        private int LeverID = 0;
        private int XDeviation = 0;
        private int YDeviation = 0;

        private int YCoord = 0;
        private int XCoord = 0;

        private bool CustomHit = false;
        private int CustomX = 0;
        private int CustomY = 0;
        private Panel CustomPanel { get; set; }

        private bool Running = false;

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern bool RegisterHotKey(IntPtr hWnd, int id, int fsModifiers, int vk);
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern bool UnregisterHotKey(IntPtr hWnd, int id);

        public enum KeyModifier
        {
            Control = 2
        }

        public override void ModifyGame(HGame game)
        {
            //Hi there, this method will only be called if your extension is running while connecting!
            game.GenerateMessageHashes();
            var move = game.GetMessages("814e9490db3f636bf970d72e072d7ef1")[0];
            var click = game.GetMessages("1d783bdbfb54f51403c1f40d931d3043")[0];
            Settings.Default.FurniMove = game.GetMessageHeader(move);
            Settings.Default.UseFurni = game.GetMessageHeader(click);
            Settings.Default.Save();
            Settings.Default.Reload();
            HeaderBx.Enabled = false; //You can still manually update as this is only false when headers were extracted from HGame
            MoveTxt.Text = Settings.Default.FurniMove.ToString();
            UseTxt.Text = Settings.Default.UseFurni.ToString();
            base.ModifyGame(game);
        }
        #endregion
        #region Hotkeys Method
        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);

            if (m.Msg == 0x0312)
            {
                Keys key = (Keys)(((int)m.LParam >> 16) & 0xFFFF);
                KeyModifier modifier = (KeyModifier)((int)m.LParam & 0xFFFF);
                int id = m.WParam.ToInt32();
                switch (key)
                {
                    case Keys.D1: StartBot(); break;
                    case Keys.D2: StopBot("Action aborted"); break;
                    case Keys.D3: WinBx.Checked = true; break;
                    case Keys.D4: LoseBx.Checked = true; break;
                    case Keys.D5: CustomHitBx.Checked = true; break;
                }
            }
        }
        #endregion

        public Form1()
        {
            InitializeComponent();

            MoveTxt.Text = Settings.Default.FurniMove.ToString();
            UseTxt.Text = Settings.Default.UseFurni.ToString();

            int id = 0;
            RegisterHotKey(Handle, id, (int)KeyModifier.Control, Keys.D1.GetHashCode());
            RegisterHotKey(Handle, id, (int)KeyModifier.Control, Keys.D2.GetHashCode());
            RegisterHotKey(Handle, id, (int)KeyModifier.Control, Keys.D3.GetHashCode());
            RegisterHotKey(Handle, id, (int)KeyModifier.Control, Keys.D4.GetHashCode());
            RegisterHotKey(Handle, id, (int)KeyModifier.Control, Keys.D5.GetHashCode());

            #region Adding GGames
            GGame gg1 = new GGame("Sirjonasxx-VII Testgrijper");
            gg1.LeftID = 225318692;
            gg1.RightID = 225318691;
            gg1.LeverID = 152028365;
            gg1.XDeviation = 4;
            gg1.YDeviation = 4;

            GGames.Add(gg1);

            GGame gg2 = new GGame("Kleinmeisje Left");
            gg2.LeftID = 164962711;
            gg2.RightID = 164962709;
            gg2.LeverID = 241971137;
            gg2.XDeviation = 3;
            gg2.YDeviation = 20;

            GGames.Add(gg2);

            GGame maw = new GGame("SUPER HOT CASINO");
            maw.LeftID = 134958734;
            maw.RightID = 132887009;
            maw.LeverID = 178079445;
            maw.XDeviation = 4;
            maw.YDeviation = 20;

            GGames.Add(maw);

            GGame gg3 = new GGame("Kleinmeisje Right");
            gg3.LeftID = 230673924;
            gg3.RightID = 230673923;
            gg3.LeverID = 217796790;
            gg3.XDeviation = 10;
            gg3.YDeviation = 3;

            GGames.Add(gg3);

            foreach (GGame g in GGames)
            {
                RoomsBx.Items.Add(g.ToString());
            }
            #endregion
            #region Panels 2 Lists
            LeftPanels.Add(panel2);
            LeftPanels.Add(panel3);
            LeftPanels.Add(panel6);
            LeftPanels.Add(panel5);
            LeftPanels.Add(panel4);
            LeftPanels.Add(panel12);

            RightPanels.Add(panel7);
            RightPanels.Add(panel9);
            RightPanels.Add(panel8);
            RightPanels.Add(panel11);
            RightPanels.Add(panel10);
            RightPanels.Add(panel13);

            HitPanels.Add(panel14);
            HitPanels.Add(panel17);
            HitPanels.Add(panel15);
            HitPanels.Add(panel24);
            HitPanels.Add(panel18);
            HitPanels.Add(panel20);
            HitPanels.Add(panel25);
            HitPanels.Add(panel16);
            HitPanels.Add(panel19);
            HitPanels.Add(panel21);
            HitPanels.Add(panel22);
            HitPanels.Add(panel26);
            HitPanels.Add(panel23);
            HitPanels.Add(panel27);
            #endregion
        }

        private bool IsHit(int x, int y)
        {
            if (!CustomHit)
            {
                if ((x == 2 && y == 1) || (x == 1 && y == 2) || (x == 1 && y == 4) || (x == 4 && y == 1) || (x == 2 && y == 5) || (x == 5 && y == 2) || (x == 3 && y == 3) || (x == 4 && y == 3) || (x == 3 && y == 4) || (x == 3 && y == 6) || (x == 6 && y == 3) || (x == 4 && y == 4) || (x == 5 && y == 6) || (x == 6 && y == 5))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                if ((x == CustomX && y == CustomY))
                    return true;
                else return false;
            }
        }

        private async void WiredMoveFurniture(DataInterceptedEventArgs e)
        {
            var args = new WiredMoveFurniture(e.Packet);
            int furniRealPosX = args.xPos2;
            int furniRealPosY = args.yPos2;
            int furniID = args.FurniID;
            int furniDeviatedPosX = 0;
            int furniDeviatedPosY = 0;
            if (furniID == LeftID)
            {
                furniDeviatedPosY = furniRealPosY - YDeviation;
                YCoord = furniDeviatedPosY;
                MainYLbl.Text = YCoord.ToString();
                UpdateColors("left", YCoord);
            }

            if (furniID == RightID)
            {
                furniDeviatedPosX = furniRealPosX - XDeviation;
                XCoord = furniDeviatedPosX;
                MainXLbl.Text = XCoord.ToString();
                UpdateColors("right", XCoord);
            }
            if (WinBx.Checked && IsHit(YCoord, XCoord))
            {
                await Connection.SendToServerAsync(Settings.Default.UseFurni, LeverID, 0);

                XCoord = 0;
                YCoord = 0;
                StopBot("Won game");
            }
            else if (LoseBx.Checked && !IsHit(YCoord, XCoord))
            {
                await Connection.SendToServerAsync(Settings.Default.UseFurni, LeverID, 0);

                XCoord = 0;
                YCoord = 0;

                StopBot("Lost game");
            }

            await Task.Delay(300);
            XCoord = 0;
            YCoord = 0;
        }

        private void StartBtn_Click(object sender, EventArgs e)
        {
            StartBot();
        }

        private void StopBtn_Click(object sender, EventArgs e)
        {
            StopBot("Action aborted");
        }

        private void StartBot()
        {
            if (StartBtn.Enabled)
            {
                Triggers.InAttach(Settings.Default.FurniMove, WiredMoveFurniture);
                if (WinBx.Checked)
                    StatusBx.Text = "Running: Trying to win";
                else if (LoseBx.Checked)
                    StatusBx.Text = "Running: Trying to lose";
                else StatusBx.Text = "Running: Trying to hit custom spot";
                Running = true;
            }
        }

        private void StopBot(string reason)
        {
            Triggers.InDetach(Settings.Default.FurniMove);
            StatusBx.Text = $"Stopped: {reason}";
            Running = false;
        }

        private void RoomsBx_SelectedIndexChanged(object sender, EventArgs e)
        {
            CurrentGame = GGames.Find(GrijperGame => GrijperGame.ToString() == RoomsBx.Text);

            LeftID = CurrentGame.LeftID;
            RightID = CurrentGame.RightID;
            LeverID = CurrentGame.LeverID;
            XDeviation = CurrentGame.XDeviation;
            YDeviation = CurrentGame.YDeviation;
        }

        private void UpdateColors(string side, int number)
        {
            if (VisBx.Checked)
            {
                if (side == "left")
                {
                    if ((number - 1) < (LeftPanels.Count))
                    {
                        if (LastBlueLeftPanel != 0)
                        {
                            LeftPanels[(LastBlueLeftPanel - 1)].BackColor = Color.DarkSeaGreen;
                        }

                        LeftPanels[(number - 1)].BackColor = Color.DarkOrange;
                        LeftLine.Location = new Point(91, (55 + ((number - 1) * 36)));
                        LastBlueLeftPanel = number;
                    }
                }

                if (side == "right")
                {
                    if ((number - 1) < (RightPanels.Count))
                    {
                        if (LastBlueRightPanel != 0)
                        {
                            RightPanels[(LastBlueRightPanel - 1)].BackColor = Color.DarkSeaGreen;
                        }

                        RightPanels[(number - 1)].BackColor = Color.DarkOrange;
                        RightLine.Location = new Point((110 + ((number - 1) * 36)), 35);
                        LastBlueRightPanel = number;
                    }
                }
            }
        }

        private void SetCustomHit(Panel p, int x, int y)
        {
            if (CustomHit)
            {
                foreach (Panel op in HitPanels)
                    if (op.BackColor == Color.Khaki)
                        op.BackColor = Color.RoyalBlue;

                p.BackColor = Color.Khaki;
                CustomPanel = p;
                CustomX = x;
                CustomY = y;
                StatusBx.Text = "Idle: Ready to hit custom spot";
                StartBtn.Enabled = true;
            }
        }

        #region Panel Click Events
        //Hi there, yeah, this should have been dynamic code. It isn't though, I must have been lazy. Fix it yourself if you really want to.
        private void panel14_Click(object sender, EventArgs e)
        {
            Panel p = sender as Panel;
            SetCustomHit(p, 1, 2);
        }

        private void panel17_Click(object sender, EventArgs e)
        {
            Panel p = sender as Panel;
            SetCustomHit(p, 1, 4);
        }

        private void panel15_Click(object sender, EventArgs e)
        {
            Panel p = sender as Panel;
            SetCustomHit(p, 2, 1);
        }

        private void panel24_Click(object sender, EventArgs e)
        {
            Panel p = sender as Panel;
            SetCustomHit(p, 2, 5);
        }

        private void panel18_Click(object sender, EventArgs e)
        {
            Panel p = sender as Panel;
            SetCustomHit(p, 3, 3);
        }

        private void panel20_Click(object sender, EventArgs e)
        {
            Panel p = sender as Panel;
            SetCustomHit(p, 3, 4);
        }

        private void panel25_Click(object sender, EventArgs e)
        {
            Panel p = sender as Panel;
            SetCustomHit(p, 3, 6);
        }

        private void panel16_Click(object sender, EventArgs e)
        {
            Panel p = sender as Panel;
            SetCustomHit(p, 4, 1);
        }

        private void panel19_Click(object sender, EventArgs e)
        {
            Panel p = sender as Panel;
            SetCustomHit(p, 4, 3);
        }

        private void panel21_Click(object sender, EventArgs e)
        {
            Panel p = sender as Panel;
            SetCustomHit(p, 4, 4);
        }

        private void panel22_Click(object sender, EventArgs e)
        {
            Panel p = sender as Panel;
            SetCustomHit(p, 5, 2);
        }

        private void panel26_Click(object sender, EventArgs e)
        {
            Panel p = sender as Panel;
            SetCustomHit(p, 5, 6);
        }

        private void panel23_Click(object sender, EventArgs e)
        {
            Panel p = sender as Panel;
            SetCustomHit(p, 6, 3);
        }

        private void panel27_Click(object sender, EventArgs e)
        {
            Panel p = sender as Panel;
            SetCustomHit(p, 6, 5);
        }
        #endregion

        private async void CustomHitBx_CheckedChanged(object sender, EventArgs e)
        {
            CustomHit = CustomHitBx.Checked;
            if (CustomHit)
            {
                if (CustomX == 0)
                {
                    if (!Running)
                    {
                        StatusBx.Text = "Waiting: Click a blue tile..";
                        StartBtn.Enabled = false;
                    }

                    else
                    {
                        StopBot("No custom tile chosen");
                        StartBtn.Enabled = false;
                        await Task.Delay(5000);
                        if (CustomHit)
                            StatusBx.Text = "Waiting: Click a blue tile..";
                    }
                }
                else
                {
                    if (!Running)
                        StatusBx.Text = "Idle: Ready to hit custom spot";
                    else StatusBx.Text = "Running: Trying to hit custom spot";
                }
                if (CustomHit)
                {
                    foreach (Panel p in HitPanels)
                    {
                        try
                        {
                            if (p != CustomPanel)
                                p.BackColor = Color.RoyalBlue;
                            else p.BackColor = Color.Khaki;
                        }
                        catch { }
                    }
                }
            }
            else
            {
                foreach (Panel p in HitPanels)
                    p.BackColor = Color.Plum;
                StartBtn.Enabled = true;
            }
        }

        private void WinBx_CheckedChanged(object sender, EventArgs e)
        {
            if (WinBx.Checked)
            {
                if (!Running)
                    StatusBx.Text = "Idle: Ready to win";
                else StatusBx.Text = "Running: Trying to win";
            }
        }

        private void LoseBx_CheckedChanged(object sender, EventArgs e)
        {
            if (!Running)
                StatusBx.Text = "Idle: Ready to lose";
            else StatusBx.Text = "Running: Trying to lose";
        }

        private void SaveBtn_Click(object sender, EventArgs e)
        {
            Settings.Default.FurniMove = ushort.Parse(MoveTxt.Text);
            Settings.Default.UseFurni = ushort.Parse(UseTxt.Text);

            Settings.Default.Save();
            Settings.Default.Reload();
        }

        private void CreditsBtn_Click(object sender, EventArgs e)
        {
            //The credits might be outdated, what about you just don't touch this form?!
            CreditsForm f = new CreditsForm();
            f.Show();
        }
    }
}
