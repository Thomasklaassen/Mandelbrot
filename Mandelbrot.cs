using System;
using System.Diagnostics.Eventing.Reader;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;


Form scherm = new Form();
scherm.Text = "Mandelbrot";
scherm.ClientSize = new Size(500, 500);
Bitmap plaatje = new Bitmap(500, 500);
Label schaaltekst = new Label();
Label MaxIterationtekst = new Label();
Label xtekst = new Label();
Label ytekst = new Label();
TextBox invoerx = new TextBox();
TextBox invoery = new TextBox();
TextBox schaalinvoer = new TextBox();
TextBox MaxIterationInvoer = new TextBox();
Button Goknop = new Button();
scherm.Controls.Add(xtekst);
scherm.Controls.Add(ytekst);
scherm.Controls.Add(schaaltekst);
scherm.Controls.Add(MaxIterationtekst);
scherm.Controls.Add(invoerx);
scherm.Controls.Add(invoery);
scherm.Controls.Add(schaalinvoer);
scherm.Controls.Add(MaxIterationInvoer);
scherm.Controls.Add(Goknop);



xtekst.AutoSize = true;
ytekst.AutoSize = true;
schaaltekst.AutoSize = true;
MaxIterationtekst.AutoSize = true;


int xInvoerLocatie = 110;
int yInvoerLocatie = 20;
int xTekstLocatie = 30;
int yTekstLocatie = 15;
invoerx.Location = new Point(xInvoerLocatie, yInvoerLocatie - 5);
xtekst.Location = new Point(xTekstLocatie, yTekstLocatie - 5);


invoery.Location = new Point(xInvoerLocatie, yInvoerLocatie + 25);
ytekst.Location = new Point(xTekstLocatie, yTekstLocatie + 25);

schaaltekst.Location = new Point(xTekstLocatie, yTekstLocatie + 55);
schaalinvoer.Location = new Point(xInvoerLocatie, yInvoerLocatie + 55);




xtekst.Text = "Midden x:";
ytekst.Text = "Midden y:";
schaaltekst.Text = "Schaal:";


Graphics gr = Graphics.FromImage(plaatje);

/*double Mandelgetal(double a, double b)
{ 
    int iterations = 0;
    // Formules:
    // x = a- a^2 + b ^2
    // y = b - 2ab
    double x = a - (a * a) + (b * b);
    double y = b - (2*a*b);
    a = a * a - b * b + x;
}*/

/*double Afstand(double a, double b)
{
    return Math.Sqrt(a * a + b * b);
}
*/




Application.Run(scherm);