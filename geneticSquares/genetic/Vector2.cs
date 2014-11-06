using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace geneticSquares.genetic
{
    class Vector2
    {
        private Point
            start,
            end;

        public Double X 
        {
            get
            {
                return (end.X - start.X);
            }
        }
        public Double Y 
        {
            get
            {
                return (end.Y - start.Y);
            }
        }

        public Double Length 
        {
            get
            {
                return Math.Sqrt(X*X + Y*Y);
            }
        }

        public Vector2(Point start, Point end)
        {
            this.start = start;
            this.end = end;
        }


    }
}
