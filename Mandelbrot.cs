using System;
using System.Diagnostics.Eventing.Reader;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;





double middenX = -0.5;
double middenY = 0;
double schaal = 0.01;
int maxIteraties = 100;
const int canvasGrootte = 500;
//form init
Form scherm = new Form();
scherm.Text = "Mandelbrot";
scherm.ClientSize = new Size(500, 600);

//Bitmaps and picturebox
Bitmap plaatje = new Bitmap(canvasGrootte, canvasGrootte);
PictureBox canvas = new PictureBox();
canvas.Image = plaatje;
canvas.SizeMode = PictureBoxSizeMode.Normal; 
canvas.Dock = DockStyle.Top;  
canvas.Height = canvasGrootte;
using (Graphics gr = Graphics.FromImage(plaatje))
{
    gr.Clear(Color.Blue);
}



// Control panel
Panel controlPanel = new Panel();
controlPanel.Padding = new Padding(10);

//All labels, textboxes and buttons init
Label xtekst = new Label { Text = "Midden x:", AutoSize = true, Location = new Point(10, 10)};
TextBox invoerx = new TextBox { Location = new Point(100, 10)};


Label ytekst = new Label { Text = "Midden y:", AutoSize = true, Location = new Point(10, 40) };
TextBox invoery = new TextBox {Location = new Point(100, 40) };

Label schaaltekst = new Label { Text = "Schaal:", AutoSize = true, Location = new Point(10, 70)};
TextBox schaalinvoer = new TextBox { Location = new Point(100, 70)};

Label MaxIterationtekst = new Label { Text = "Max herhaling", AutoSize = true, Location = new Point(10, 100)};
TextBox MaxIterationInvoer = new TextBox { Location = new Point(100, 100)};

Button goKnop = new Button { Text = "Go!", Location = new Point(200, 50)};



//Create control list and add all controls to the canvas. 
Control[] allControls = new Control[] { xtekst, invoerx, ytekst, invoery, schaaltekst, schaalinvoer, MaxIterationtekst, MaxIterationInvoer, goKnop };
controlPanel.Controls.AddRange(allControls);
scherm.Controls.Add(canvas);
scherm.Controls.Add(controlPanel);

controlPanel.Height = 250;

controlPanel.Top = canvas.Height;

int BerekenMandelgetal(double x, double y, int maxIteraties)
{
    double a = 0.0;
    double b = 0.0;
    int iteraties = 0;


    while (a * a + b * b <= 4 && iteraties < maxIteraties)
    {
        double tempA = a * a - b * b + x;
        b = 2 * a * b + y;
        a = tempA;
        iteraties++;
    }


    if (iteraties == maxIteraties)
    {
        return -1; // dit is zegmaar om oneindig te representeren, omdat als die -1 returned dan stopt die met runnnen, want hij kan niet oneindig itereren.(dus als die max iteraties berijkt dan stopt DIE)
    }

    return iteraties;
}

Color BepaalKleur(int mandelgetal)
{
    if (mandelgetal == -1)
    {
        return Color.Black;
    }
    else if (mandelgetal % 2 == 0)
    {
        return Color.Black;
    }
    else
    {
        return Color.White;
    }
}
void TekenMandelbrot(double mx, double my, double s, int maxIter)
{
    using (Graphics gr = Graphics.FromImage(plaatje))
    {
        gr.Clear(Color.Blue); // maakt de achtergrond blauw

        for (int p_x = 0; p_x < canvasGrootte; p_x++)
        {
            for (int p_y = 0; p_y < canvasGrootte; p_y++)
            {
                
                double x = mx + (p_x - canvasGrootte / 2.0) * s;
                double y = my + (p_y - canvasGrootte / 2.0) * s;

                // berekent het mandelgetal en bepaal de kleur
                int mandelgetal = BerekenMandelgetal(x, y, maxIter);
                Color kleur = BepaalKleur(mandelgetal);

                plaatje.SetPixel(p_x, p_y, kleur);
            }
        }
    }
    canvas.Invalidate();
}
void GoKnop_Click(object sender, EventArgs e)
{
    // lees de waarden uit de tekstvakken en update de variabelen
    if (double.TryParse(invoerx.Text, out double newX)) middenX = newX;
    if (double.TryParse(invoery.Text, out double newY)) middenY = newY;
    if (double.TryParse(schaalinvoer.Text, out double newSchaal)) schaal = newSchaal;
    if (int.TryParse(MaxIterationInvoer.Text, out int newMaxIteraties)) maxIteraties = newMaxIteraties;

    TekenMandelbrot(middenX, middenY, schaal, maxIteraties);
}
void canvas_MouseDown(object sender, MouseEventArgs e)
{
    // bepaalt het nieuwe middelpunt van de muisklik
    middenX += (e.X - canvas.Width / 2.0) * schaal;
    middenY += (e.Y - canvas.Height / 2.0) * schaal;

    // past de schaal aan op basis van de muisknop
    if (e.Button == MouseButtons.Left)
    {
        schaal /= 2.0; // Inzoomen
    }
    else if (e.Button == MouseButtons.Right)
    {
        schaal *= 2.0; // Uitzoomen
    }

    // update de tekstvakken en teken opnieuw
    invoerx.Text = middenX.ToString();
    invoery.Text = middenY.ToString();
    schaalinvoer.Text = schaal.ToString();

    TekenMandelbrot(middenX, middenY, schaal, maxIteraties);
}


goKnop.Click += GoKnop_Click;
canvas.MouseDown += canvas_MouseDown;
TekenMandelbrot(middenX, middenY, schaal, maxIteraties);






Application.Run(scherm);
