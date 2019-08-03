using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vector;

namespace SmartRockets
{
    public enum State { Alive, Dead, Finished};

    class Rocket
    {
        private Vector2 Velocity;
        private Vector2 Acceleration;
        
        public DNA dna;
        public int genesCounter;
        public float record;
        public State state;
        public float fitness = 0;

        public Vector2 Pos { get; set; }

        public void calculateFitness()
        {
            // fitness = (float)Math.Pow(1 / (record+0.0001f), 2);
            fitness = 1 / (record+0.0001f);
            if (state == State.Dead)
            {
                fitness *= 0.005f;
            }else if(state == State.Finished)
            {
                fitness *= 10;
            }
        }

        public Rocket(Vector2 pos)
        {
            record = 99999;
            state = State.Alive;
            Velocity = new Vector2();
            Acceleration = new Vector2();
            Pos = pos;
            genesCounter = 0;
            dna = new DNA();
        }

        public void Render(Graphics g)
        {
            Color col = Color.Magenta;

            switch (state)
            {
                case State.Alive:
                col = Color.FromArgb(100, Color.White);
                    break;
                case State.Dead:
                col = Color.FromArgb(100, Color.Red);
                    break;
                case State.Finished:
                col = Color.FromArgb(100, Color.Magenta);
                    break;
            }
            g.FillEllipse(new SolidBrush(col), Pos.X-5, Pos.Y-5, 10, 10);
        }

        public void ApplyForce(Vector2 force)
        {
            Acceleration.Add(force);
        }

        public void Run()
        {
            ApplyForce(dna.genes[genesCounter]);
            genesCounter++;
            Update();
        }

        public void Update()
        {
            Velocity.Add(Acceleration);
            Pos.Add(Velocity);
            Acceleration.Multiply(0);
            float time = ((float)genesCounter / DNA.lifetime) * 100;
            float temp = Vector2.Distance(Pos, Population.target);
            record = time + temp;
            //if (temp + time < record)
            //{
            //    record = temp + time;
            //} 
            if(temp <= 5)
            {
                Pos = new Vector2(Population.target.X, Population.target.Y);
                record = time;
                state = State.Finished;
            }
            if (isOutOfMap())
            {
                state = State.Dead;
            }else if(Form1.obstacles.Any(o => o.Distance(Pos) <= 25))
            {
                state = State.Dead;
            }
        }

        private bool isOutOfMap()
        {
            return (Pos.Y < - 5) || (Pos.X > Form1.WH.Width + 5)
                || (Pos.Y > Form1.WH.Height + 5) || (Pos.X < - 5);
        }

    }


    public class DoneException : Exception
    {
        public DoneException(string message) : base(message)
        {
        }
    }

}
