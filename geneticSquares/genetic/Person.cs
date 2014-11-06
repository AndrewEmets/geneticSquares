using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace geneticSquares.genetic
{
    

    class SortByFit : IComparer<Person>
    {
        public int Compare(Person x, Person y)
        {
            return x.FitnessFunction().CompareTo(y.FitnessFunction());
        }
    }

    class Person
    {
        public Point[] vertex;
        public Int32 generation;        

        public Person(Point[] Vertex, Int32 generation) 
        {
            vertex = new Point[4];
            this.generation = generation;

            for (int i = 0; i < 4; i++)
            {
                vertex[i] = Vertex[i];
            }
        }

        public Person(Rectangle bounds, Random rand, Int32 generation) 
        {
            vertex = new Point[4];
            this.generation = generation;
            
            vertex[0] = new Point(
                rand.Next(0, bounds.Width / 2),
                rand.Next(0, bounds.Height / 2)
                );

            vertex[1] = new Point(
                rand.Next(bounds.Width / 2, bounds.Width),
                rand.Next(0, bounds.Height / 2)
                );

            vertex[2] = new Point(
                rand.Next(bounds.Width / 2, bounds.Width),
                rand.Next(bounds.Height / 2, bounds.Height)
                );

            vertex[3] = new Point(
                rand.Next(0, bounds.Width / 2),
                rand.Next(bounds.Height / 2, bounds.Height)
                );
        }

        public Person(String seed, Int32 generation)
        {
            vertex = new Point[4];
            this.generation = generation;

            SetVertexes(seed);
        }

        public IEnumerable<Person> CrossOver(Person parent)
        {
            Random rand = new Random();
                String fatherGenome = this.GetGenome();
                String motherGenome = parent.GetGenome();
                String childGenome1 = "";
                String childGenome2 = "";

                int splitPoint = rand.Next(0, fatherGenome.Length);

                childGenome1 = fatherGenome.Remove(splitPoint, fatherGenome.Length - splitPoint) + motherGenome.Remove(0, splitPoint);
                childGenome2 = motherGenome.Remove(splitPoint, motherGenome.Length - splitPoint) + fatherGenome.Remove(0, splitPoint);

                yield return new Person(childGenome1, generation + 1);
                yield return new Person(childGenome2, generation + 1);
        }

        public override string ToString()
        {
            String str = "";

            str += generation.ToString() + ") ";

            for (int i = 0; i < 4; i++)
            {
                str += vertex[i].X.ToString() + " " + vertex[i].Y.ToString() + " ";
            }

            str += ". Fit: " + Math.Round(FitnessFunction(), 3);

            return str;
        }

        private Double GetAngle(Point point1, Point point2, Point point3)
        {
            Double alpha;
            Vector2
                a = new Vector2(point2, point1),
                b = new Vector2(point2, point3);

            alpha = Math.Abs(Math.Acos((a.X * b.X + a.Y * b.Y) / (a.Length * b.Length)));

            return alpha;
        }

        private string Reverse(string s)
        {
            char[] charArray = s.ToCharArray();
            Array.Reverse(charArray);
            return new string(charArray);
        }

        private void SetVertexes(String seed)
        {
            String gen = "";
            int i = 0;

            while (seed != "")
            {
                gen = seed;

                if (gen.Length > 9)
                    gen = gen.Remove(9);

                vertex[i].X = GetInt16(gen);
                
                seed = seed.Remove(0, 9);

                gen = seed;
                if (gen.Length > 9)
                    gen = gen.Remove(9);

                vertex[i].Y = GetInt16(gen);

                i++;

                seed = seed.Remove(0, 9);
            }

        }

        private String GetBinary(UInt16 a)
        {
            String str = "";
            UInt16 t = 0;

            while (a != 0)
            {
                t = a;
                a = (UInt16)(a >> 1);

                t = (UInt16)(t & 0x01);
                str += t.ToString();
            }

            str = Reverse(str);
            str = str.PadLeft(9, '0');

            return str;
        }

        private UInt16 GetInt16(String binary)
        {
            UInt16 a = 0;
            binary = binary.TrimStart('0');

            for (int i = 0; i < binary.Length; i++)
            {
                a = (UInt16)(a << 1);
                if (binary[i] == '1') a = (UInt16)(a | 0x1);
                
            }

            return a;
        }

        public String GetGenome()
        {
            String genome = "";

            for (int i = 0; i < 4; i++)
            {
                genome += GetBinary((UInt16)vertex[i].X);
                genome += GetBinary((UInt16)vertex[i].Y);
            }

            return genome;
        }

        public Double FitnessFunction()
        {
            Double score = 0;

            for (int i = 0; i <= 3; i++)
            {
                int a = i;
                int b = (i + 1) % 4;
                int c = (i + 2) % 4;

                Double angle = GetAngle(vertex[a], vertex[b], vertex[c]);

                score += Math.Abs(angle - Math.PI/2);
            }

            /*Double[] lengths = new Double[4];
            for (int i = 0; i <= 3; i++)
            {
                int a = i;
                int b = (i + 1) % 4;


            }*/

            return score;
        }

        public Int32 Mutation(Double chance, Random rand)
        {
            Int32 mutations = 0;
            String selfGenome = this.GetGenome();
            String newGenome = "";
            Char newGen = ' ';
                        
            for (int i = 0; i < selfGenome.Length; i++)
            {
                newGen = selfGenome[i];
                if (rand.NextDouble() < chance)
                {
                    mutations++;
                    newGen = (newGen == '0' ? '1' : '0');
                }
                newGenome += newGen;
            }

            SetVertexes(newGenome);

            return mutations;
        }
        
        public void AlignCenter(Point center)
        {
            Int32
                minX = vertex[0].X, minY = vertex[0].Y,
                maxX = vertex[0].X, maxY = vertex[0].Y;

            for (int i = 1; i < 4; i++)
            {
                if (vertex[i].X < minX) minX = vertex[i].X;
                if (vertex[i].Y < minY) minY = vertex[i].Y;
                if (vertex[i].X > maxX) maxX = vertex[i].X;
                if (vertex[i].Y > maxY) maxY = vertex[i].Y;
            }

            Rectangle bounds = new Rectangle(minX, minY, maxX - minX, maxY - minY);

            Point offsetVector = new Point(
                center.X - (bounds.X + bounds.Width / 2),
                center.Y - (bounds.Y + bounds.Height / 2)
                );

            for (int i = 0; i < 4; i++)
            {
                vertex[i].X += offsetVector.X;
                vertex[i].Y += offsetVector.Y;
            }
        }
    }
}
