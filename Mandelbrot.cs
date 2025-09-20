using System;
using System.Diagnostics.Eventing.Reader;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;






//form init
Form scherm = new Form();
scherm.Text = "Mandelbrot";
scherm.ClientSize = new Size(500, 600);

//Bitmaps and picturebox
Bitmap plaatje = new Bitmap(500, 500);
PictureBox canvas = new PictureBox();
canvas.Image = plaatje;
canvas.SizeMode = PictureBoxSizeMode.Normal; 
canvas.Dock = DockStyle.Top;  
canvas.Height = 400;
using (Graphics gr = Graphics.FromImage(plaatje))
{
    gr.Clear(Color.Blue);
}


// Control panel
Panel controlPanel = new Panel();
controlPanel.Dock = DockStyle.Fill; // takes remaining space (bottom part)
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

Button goKnop = new Button { Text = "Go!", Location = new Point(10, 140) };

//Create control list and add all controls to the canvas. 
Control[] allControls = new Control[] { xtekst, invoerx, ytekst, invoery, schaaltekst, schaalinvoer, MaxIterationtekst, MaxIterationInvoer, goKnop };
controlPanel.Controls.AddRange(allControls);
scherm.Controls.Add(controlPanel);
scherm.Controls.Add(canvas);



//double Mandelgetal(double x, double y, double a, double b)
//{
//    double afstand = 0;
//    int iterations = 0;
//    while (afstand <= 2)
//    {
//        double mandelA = a * a - b * b + x;
//        double mandelB = 2 * a * b + y;
//        afstand = Afstand(mandelA, mandelB);
//        iterations++;
//    }

//}

double Afstand(double a, double b)
{
    return Math.Sqrt(a * a + b * b);
}





Application.Run(scherm);
