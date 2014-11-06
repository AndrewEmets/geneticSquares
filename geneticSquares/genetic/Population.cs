using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace geneticSquares.genetic
{
    class Population
    {
        public List<Person> persons;
        public Int32 currentGeneration;
        public Population()
        {
            persons = new List<Person>();
            currentGeneration = 0;
        }

        public void CreateNewPopulation(int PopulationSize, Rectangle rect) 
        {
            Random rand = new Random();

            currentGeneration = 1;

            persons.Clear();

            for (int i = 0; i < PopulationSize; i++)
            {
                persons.Add(new Person(rect, rand, currentGeneration));
            }
        }

        public void AppendPopulation(Int32 maxPerons)
        {
            Random rand = new Random();
            
            currentGeneration++;

            List<Person> newGeneration = new List<Person>();

            while (persons.Count + newGeneration.Count < maxPerons)
            {
                int a = rand.Next(0, persons.Count - 1);
                int b = rand.Next(0, persons.Count - 1);
                while (a == b) b = rand.Next(0, persons.Count - 1);

                newGeneration.AddRange(persons[a].CrossOver(persons[b]));
            }

            persons.AddRange(newGeneration);
        }

        public void Mutation(Int32 startIndex, Double Chance, Random rand)
        {
            for (int i = startIndex; i < persons.Count; i++)
            {
                persons[i].Mutation(0.02, rand);
            }
            
        }

        public void AllignAll(Point center)
        {
            foreach (Person person in persons)
            {
                person.AlignCenter(center);
            }
        }
    }
}
