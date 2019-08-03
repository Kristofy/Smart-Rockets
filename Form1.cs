using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Vector;

namespace SmartRockets
{
    public partial class Form1 : Form
    {
        public static Vector2 Spawner { get; private set; }
        public static int Gen = 0;
        public static float Best = float.PositiveInfinity;
        public static Size WH;
        public static List<IObstacle> obstacles;
        public static int optionSelectIndex = 0;
        public static int objectSelectIndex = 0;
        private static float newLifetime = DNA.lifetime,
            newMaxforce = DNA.maxforce, newPopulation = Population.PopulationSize;

        private List<Function> functions;
        private Population population;
        private Vector2 from;
        public Bitmap arrow;
        public Bitmap bigArrow;
        public Bitmap deleteButton;
        public bool timerOn = true; 

        public Form1()
        {
            functions = new List<Function>();
            InitializeComponent();
            Spawner = new Vector2(550, 550);
            Population.target = new Vector.Vector2(0, 0);
            population = new Population();
            DoubleBuffered = true;
            obstacles = new List<IObstacle>();
            deleteButton = new Bitmap(new Bitmap("delete.png"), new Size(50, 50));
            bigArrow = new Bitmap("arrow.png");
            arrow = new Bitmap(bigArrow, new Size(10, 8));

            loadFunctions();
        }

        private void loadFunctions()
        {
            //Spawner
            functions.Add(new Function
            {
                action = (e) =>
                {
                    Spawner = new Vector2(x: e.X, y: e.Y);
                    Setup();
                },
                render = (g) => g.FillEllipse(Brushes.Blue, WH.Width - 130, 11, 50, 50)
            });

            //Target
            functions.Add(new Function
            {
                action = (e) =>
                {
                    Population.target = new Vector2(e.X, e.Y);
                    Setup();
                },
                render = (g) => g.FillEllipse(Brushes.Green, WH.Width - 130, 11, 50, 50)
            });

            //Rectangle
            functions.Add(new Function
            {
                action = (e) =>
                {
                    Vector2 curr = new Vector2(e.X, e.Y);
                    Rectangle rectangle;
                    Vector2 diff = curr - from;
                    if (diff.Magnitude() > 7)
                    {
                        Size size = new Size((int)Math.Abs(diff.X), (int)Math.Abs(diff.Y));

                        if (from.X < curr.X && from.Y < curr.Y)
                        {
                            rectangle = new Rectangle(from, size);
                        }
                        else if (from.X > curr.X && from.Y > curr.Y)
                        {
                            rectangle = new Rectangle(curr, size);
                        }
                        else if (from.X < curr.X && from.Y > curr.Y)
                        {
                            rectangle = new Rectangle(new Vector2(from.X, curr.Y), size);
                        }
                        else
                        {
                            rectangle = new Rectangle(new Vector2(curr.X, from.Y), size);
                        }
                        obstacles.Add(rectangle);
                        Setup();
                    }

                },
                render = (g) => g.FillRectangle(Brushes.White, WH.Width - 130, 11, 50, 50)
            });

            //Circle
            functions.Add(new Function
            {
                action = (e) =>
                {
                    Vector2 curr = new Vector2(e.X, e.Y);
                    Vector2 diff = from - curr;
                    float m = diff.Magnitude();
                    if (m > 5)
                    {

                        Circle circle = new Circle(curr, (int)(m * 2));
                        obstacles.Add(circle);
                        Setup();
                    }
                },
                render = (g) => g.FillEllipse(Brushes.White, WH.Width - 130, 11, 50, 50)
            });


            //Delete
            functions.Add(new Function
            {
                action = (e) =>
                {
                    Vector2 curr = new Vector2(e.X, e.Y);
                    obstacles.RemoveAll((o) => o.Distance(curr) <= 0);
                    Setup();
                },
                render = (g) => g.DrawImageUnscaled(deleteButton, WH.Width - 130, 11, 50, 50)
            });
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            ClientSize = new Size(600, 600);
            WH = ClientSize;

            obstacles.Add(new Circle(new Vector2(100, 100), 100));
            obstacles.Add(new Circle(new Vector2(250, 200), 50));
            obstacles.Add(new Rectangle(new Vector2(300, 500), new Size(50, 100)));
            obstacles.Add(new Rectangle(new Vector2(200, 400), new Size(100, 200)));

            tick_timer.Interval = 1;
            tick_timer.Start();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            Render(g);

            base.OnPaint(e);
        }

        private void Render(Graphics g)
        {
            g.Clear(Color.DarkGray);

            obstacles.ForEach(o => o.Render(g));

            g.FillEllipse(Brushes.Green, Population.target.X - 5, Population.target.Y - 5, 10, 10);

            g.FillEllipse(Brushes.Blue, Spawner.X, Spawner.Y, 10, 10);

            population.Render(g);

            g.DrawString($"Lifetime: {population.LeftLifetime}{Environment.NewLine}Best: {((float.IsNaN(Best)) ? "Goal" : Convert.ToString(Best))}{Environment.NewLine}Generation: {Gen}"
                , new Font(FontFamily.GenericSansSerif, 15)
                , Brushes.Red
                , new Point(20, WH.Height - 100));

            g.DrawString($"Whole LifeTime: {(int)newLifetime}{Environment.NewLine}Maxforce: {newMaxforce}{Environment.NewLine}Population: {(int)newPopulation}{Environment.NewLine}Speed: {tick_timer.Interval}"
                , new Font(FontFamily.GenericMonospace, 10)
                , Brushes.Blue
                , new Point(WH.Width - 180, 70));

            if (optionSelectIndex == 4)
            {
                g.DrawImage(bigArrow, WH.Width - 220, 30, 30, 24);
            }
            else
            {
                g.DrawImageUnscaled(arrow, WH.Width - 200, 73 + optionSelectIndex * 15, 10, 8);
            }

            functions[objectSelectIndex].render(g);


        }

        private void UpdateMain(object sender, EventArgs e)
        {
            try
            {
                for (int i = 0; i < 10; i++)
                {
                    population.Update();
                }
            }
            catch (DoneException) { }
            finally
            {
                Refresh();
            }
        }

        private void Form1_MouseUp(object sender, MouseEventArgs e)
        {
            functions[objectSelectIndex].action(e);
        }

        private void Setup()
        {
            Gen = 0;
            Best = float.PositiveInfinity;
            population = new Population();
            Refresh();
        }

        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            from = new Vector2(x: e.X, y: e.Y);
        }

        private void Form1_SizeChanged(object sender, EventArgs e)
        {
            WH = ClientSize;
        }

        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F)
            {
                for (int k = 0; k < 25; k++)
                {
                    try
                    {
                        for (int i = 0; i <= DNA.lifetime; i++)
                        {
                            population.Update();
                        }
                    }
                    catch (DoneException) { }
                }
                return;
            }

            if(e.KeyCode == Keys.P)
            {
                if (timerOn)
                {
                    tick_timer.Stop();
                }
                else
                {
                    tick_timer.Start();
                }
                timerOn = !timerOn;
            }

            switch (e.KeyCode)
            {
                case Keys.Space:
                    Population.PopulationSize = (int)newPopulation;
                    DNA.lifetime = (int)newLifetime;
                    DNA.maxforce = newMaxforce;
                    Setup();
                    break;
                case Keys.W:
                case Keys.Up:
                    if (optionSelectIndex > 0)
                    {
                        optionSelectIndex--;
                    }
                    else
                    {
                        optionSelectIndex = 4;
                    }
                    break;
                case Keys.S:
                case Keys.Down:
                    if (optionSelectIndex < 4)
                    {
                        optionSelectIndex++;
                    }
                    else
                    {
                        optionSelectIndex = 0;
                    }
                    break;
                case Keys.D:
                case Keys.Right:

                    switch (optionSelectIndex)
                    {
                        case 0:
                            newLifetime += 50;
                            break;
                        case 1:
                            newMaxforce += 0.01f;
                            newMaxforce = (float)decimal.Round((decimal)newMaxforce, 2);
                            break;
                        case 2:
                            newPopulation += 10;
                            break;
                        case 3:
                            if (tick_timer.Interval < 30)
                                tick_timer.Interval++;
                            break;
                        case 4:
                            objectSelectIndex++;
                            objectSelectIndex %= functions.Count;
                            break;
                    }

                    break;
                case Keys.A:
                case Keys.Left:

                    switch (optionSelectIndex)
                    {
                        case 0:
                            if (newLifetime > 100)
                                newLifetime -= 50;
                            break;
                        case 1:
                            if (newMaxforce > 0.01)
                                newMaxforce -= 0.01f;
                            newMaxforce = (float)decimal.Round((decimal)newMaxforce, 2);
                            break;
                        case 2:
                            if (newPopulation > 10)
                                newPopulation -= 10;
                            break;
                        case 3:
                            if (tick_timer.Interval > 1)
                                tick_timer.Interval--;
                            break;
                        case 4:
                            objectSelectIndex--;
                            if (objectSelectIndex < 0)
                            {
                                objectSelectIndex = functions.Count - 1;
                            }
                            break;
                    }
                    break;
            }
            Refresh();
        }
    }
}
