using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

public class MandelBrotViewer
{
    public class Form1 : Form
    {
        // Mandelbrot instellingen
        double middenX = -0.5;
        double middenY = 0;
        double schaal = 0.01;
        int maxIteraties = 100;
        const int canvasGrootte = 500;

        Bitmap plaatje;
        PictureBox canvas;
        TextBox invoerx, invoery, schaalinvoer, MaxIterationInvoer;
        Label Errormsg;
        ComboBox kleurMenu, figuurMenu;


        Dictionary<string, (double x, double y, double schaal, int iter)> standaardFiguur = new()
        {
            { "Zeepaardvallei", (-0.7962441, -0.245452, 0.0007296, 200) },
            { "Web", (0.013516845703125013, -0.6556672668457033, 9.765625E-06, 1000) },
            { "Spiraal", (-0.761574, -0.0847596, 0.0005, 300) },
            { "Vlinder", (-1.747, 0.00005, 0.00001, 500) },
            { "Dubbelkern", (-0.1015, 0.633, 0.00005, 400) }
        };

        public Form1()
        {
           
            UI();
        }

        private void UI()
        {
            this.Text = "Mandelbrot Viewer";
            this.ClientSize = new Size(500, 700);

            plaatje = new Bitmap(canvasGrootte, canvasGrootte);
            canvas = new PictureBox
            {
                Image = plaatje,
                SizeMode = PictureBoxSizeMode.Normal,
                Dock = DockStyle.Top,
                Height = canvasGrootte
            };
            this.Controls.Add(canvas);
            canvas.MouseDown += canvas_MouseDown;

            Panel controlPanel = new Panel
            {
                Padding = new Padding(10),
                Height = 350,
                Width = 500,
                Top = canvas.Height
            };

            // labels enz
            int labelX = 20, inputX = 120, rowHeight = 25;
            int yOffset = 10;

            Label xtekst = new Label { Text = "Midden x:", AutoSize = true, Location = new Point(labelX, yOffset) };
            invoerx = new TextBox { Location = new Point(inputX, yOffset), Width = 100 };

            yOffset += rowHeight;
            Label ytekst = new Label { Text = "Midden y:", AutoSize = true, Location = new Point(labelX, yOffset) };
            invoery = new TextBox { Location = new Point(inputX, yOffset), Width = 100 };

            yOffset += rowHeight;
            Label schaaltekst = new Label { Text = "Schaal:", AutoSize = true, Location = new Point(labelX, yOffset) };
            schaalinvoer = new TextBox { Location = new Point(inputX, yOffset), Width = 100 };

            yOffset += rowHeight;
            Label MaxIterationtekst = new Label { Text = "Max herhaling:", AutoSize = true, Location = new Point(labelX, yOffset) };
            MaxIterationInvoer = new TextBox { Location = new Point(inputX, yOffset), Width = 100 };

            // Kleurenschema dropdown
            yOffset += rowHeight + 10;
            Label kleurLabel = new Label { Text = "Kleurenschema:", AutoSize = true, Location = new Point(labelX, yOffset) };
            kleurMenu = new ComboBox
            {
                Location = new Point(inputX, yOffset),
                Size = new Size(150, 25),
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            kleurMenu.Items.AddRange(new object[] {
            "Zwart-wit", "Blauw", "Vuur", "Groen", "Paars",
            "Pastel Regenboog", "Koel Spectrum", "Vulkanisch"
            });
            kleurMenu.SelectedItem = "Zwart-wit";

            // figuur dropdown
            yOffset += rowHeight;
            Label figuurLabel = new Label { Text = "Standaardfiguur:", AutoSize = true, Location = new Point(labelX, yOffset) };
            figuurMenu = new ComboBox
            {
                Location = new Point(inputX, yOffset),
                Size = new Size(150, 25),
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            figuurMenu.Items.AddRange(standaardFiguur.Keys.ToArray());
            figuurMenu.SelectedItem = "Zeepaardvallei";

            // Go-knop en errors
            yOffset += rowHeight ;
            Button goKnop = new Button
            {
                Text = "Go!",
                Location = new Point(labelX, yOffset),
                Size = new Size(80, 30)
            };

            goKnop.Click += GoKnop_Click;
            this.AcceptButton = goKnop;

            yOffset += rowHeight;
            Errormsg = new Label { Text = "", ForeColor = Color.Red, AutoSize = true, Location = new Point(labelX, yOffset) };

            //  alles toevoegen aan het panel
            controlPanel.Controls.AddRange(new Control[] {
            xtekst, invoerx, ytekst, invoery, schaaltekst, schaalinvoer,
            MaxIterationtekst, MaxIterationInvoer,
            kleurLabel, kleurMenu,
            figuurLabel, figuurMenu,
            goKnop, Errormsg
            });

            this.Controls.Add(controlPanel);

            // teken het figuur bij opstart
            TekenMandelbrot(middenX, middenY, schaal, maxIteraties);
        }


        private void GoKnop_Click(object sender, EventArgs e)
        {
            if (figuurMenu.SelectedItem != null && standaardFiguur.ContainsKey(figuurMenu.SelectedItem.ToString()))
            {
                var figuur = standaardFiguur[figuurMenu.SelectedItem.ToString()];
                TekenMandelbrot(figuur.x, figuur.y, figuur.schaal, figuur.iter);
                return;
            }

            if (string.IsNullOrWhiteSpace(invoerx.Text) || string.IsNullOrWhiteSpace(invoery.Text) ||
                string.IsNullOrWhiteSpace(schaalinvoer.Text) || string.IsNullOrWhiteSpace(MaxIterationInvoer.Text))
            {
                Errormsg.Text = "Vul alle velden in!";
                return;
            }

            try
            {
                middenX = Convert.ToDouble(invoerx.Text.Replace(".", ","));
                middenY = Convert.ToDouble(invoery.Text.Replace(".", ","));
                schaal = Convert.ToDouble(schaalinvoer.Text.Replace(".", ","));
                maxIteraties = Convert.ToInt32(MaxIterationInvoer.Text);
            }
            catch
            {
                Errormsg.Text = "Vul geldige getallen in!";
                return;
            }

            TekenMandelbrot(middenX, middenY, schaal, maxIteraties);

        }

        private void canvas_MouseDown(object sender, MouseEventArgs e)
        {
            middenX += (e.X - canvas.Width / 2.0) * schaal;
            middenY += (e.Y - canvas.Height / 2.0) * schaal;

            if (e.Button == MouseButtons.Left)
                schaal /= 2.0;
            else if (e.Button == MouseButtons.Right)
                schaal *= 2.0;

            invoerx.Text = middenX.ToString();
            invoery.Text = middenY.ToString();
            schaalinvoer.Text = schaal.ToString();

            TekenMandelbrot(middenX, middenY, schaal, maxIteraties);
        }

        private int BerekenMandelgetal(double x, double y, int maxIteraties)
        {
            double a = 0.0, b = 0.0;
            int iteraties = 0;
            while (a * a + b * b <= 4 && iteraties < maxIteraties)
            {
                double tempA = a * a - b * b + x;
                b = 2 * a * b + y;
                a = tempA;
                iteraties++;
            }
            return iteraties == maxIteraties ? -1 : iteraties;
        }

        private Color BepaalKleur(int mandelgetal)
        {
            if (mandelgetal == -1) return Color.Black;
            double ratio = (double)mandelgetal / maxIteraties;

            switch (kleurMenu.SelectedItem.ToString())
            {
                case "Blauw": return Color.FromArgb(0, 0, Math.Clamp((int)(255 * Math.Sqrt(ratio)), 30, 255));
                case "Zwart-wit": return mandelgetal % 2 == 0 ? Color.Black : Color.White;
                case "Vuur":
                    int r = Math.Clamp(mandelgetal * 5, 0, 255);
                    int g = Math.Clamp(mandelgetal * 2, 0, 255);
                    return Color.FromArgb(r, g, 0);
                case "Groen":
                    int groen = Math.Clamp((int)(255 * Math.Sqrt(ratio)), 0, 255);
                    return Color.FromArgb(0, groen, 0);
                case "Paars":
                    int p = Math.Clamp((int)(255 * Math.Pow(ratio, 0.5)), 0, 255);
                    return Color.FromArgb(p / 2, 0, p);
                case "Pastel Regenboog":
                    double hue1 = 360.0 * ratio;
                    return HSV(hue1, 0.4, 1.0);
                case "Koel Spectrum":
                    double hue2 = 240.0 * ratio; // blueish tones
                    double sat2 = 0.8;
                    double val2 = 0.6 + 0.4 * ratio;
                    return HSV(hue2, sat2, val2);
                case "Vulkanisch":
                    double hue3 = 30 + 30 * ratio; // orange-red
                    double sat3 = 1.0;
                    double val3 = 0.5 + 0.5 * ratio;
                    return HSV(hue3, sat3, val3);

                default: return Color.White;
            }
        }

         Color HSV(double hue, double saturation, double value)
        {
            saturation = Math.Clamp(saturation, 0.0, 1.0);
            value = Math.Clamp(value, 0.0, 1.0);
            int hi = Convert.ToInt32(Math.Floor(hue / 60)) % 6;
            double f = hue / 60 - Math.Floor(hue / 60);

            value *= 255;
            int v = Convert.ToInt32(value);
            int p = Convert.ToInt32(value * (1 - saturation));
            int q = Convert.ToInt32(value * (1 - f * saturation));
            int t = Convert.ToInt32(value * (1 - (1 - f) * saturation));

            return hi switch
            {
                0 => Color.FromArgb(v, t, p),
                1 => Color.FromArgb(q, v, p),
                2 => Color.FromArgb(p, v, t),
                3 => Color.FromArgb(p, q, v),
                4 => Color.FromArgb(t, p, v),
                _ => Color.FromArgb(v, p, q),
            };
        }


        private void TekenMandelbrot(double mx, double my, double s, int maxIter)
        {
            using Graphics gr = Graphics.FromImage(plaatje);
            gr.Clear(Color.Blue);

            for (int px = 0; px < canvasGrootte; px++)
            {
                for (int py = 0; py < canvasGrootte; py++)
                {
                    double x = mx + (px - canvasGrootte / 2.0) * s;
                    double y = my + (py - canvasGrootte / 2.0) * s;
                    int mandelgetal = BerekenMandelgetal(x, y, maxIter);
                    plaatje.SetPixel(px, py, BepaalKleur(mandelgetal));
                }
            }

            canvas.Invalidate();
        }
    }
    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }

}

