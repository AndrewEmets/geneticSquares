using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using geneticSquares.genetic;

namespace geneticSquares
{
    public partial class Form1 : Form
    {
        Population population;
        Random rand;
        Boolean populationCreated;

        public Form1()
        {
            InitializeComponent();

            population = new Population();
            rand = new Random();

            populationCreated = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            population.CreateNewPopulation(Int32.Parse(textBox2.Text), pictureBox1.ClientRectangle);
            populationCreated = true;

            DrawPopulation(population.persons);
            AddToListBox();
        }

        private void DrawPopulation(List<Person> drawlist)
        {
            Bitmap DrawArea = new Bitmap(pictureBox1.Size.Width, pictureBox1.Size.Height);
            pictureBox1.Image = DrawArea;
            Pen redPen = new Pen(Brushes.Red);
            Graphics g = Graphics.FromImage(pictureBox1.Image);

            g.FillRectangle(Brushes.White, pictureBox1.ClientRectangle);
            g.DrawLine(
                Pens.LightGray,
                0,
                pictureBox1.ClientRectangle.Height / 2,
                pictureBox1.ClientRectangle.Right,
                pictureBox1.ClientRectangle.Height / 2);

            g.DrawLine(
                Pens.LightGray,
                pictureBox1.ClientRectangle.Width / 2,
                0,
                pictureBox1.ClientRectangle.Width / 2,
                pictureBox1.ClientRectangle.Height);


            foreach (Person person in drawlist)
            {

                for (int i = 0; i < 3; i++)
                {
                    g.DrawLine(redPen, person.vertex[i], person.vertex[i + 1]);
                }
                g.DrawLine(redPen, person.vertex[3], person.vertex[0]);

            }
            g.Dispose();
        }

        private void AddToListBox()
        {
            listBox1.Items.Clear();
            foreach (Person person in population.persons)
            {
                listBox1.Items.Add(person.ToString());
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            int lasts = Int32.Parse(textBox3.Text);
            Random rand = new Random();

            population.persons.Sort(new SortByFit());
            population.persons.RemoveRange(lasts, population.persons.Count - lasts);

            DrawPopulation(population.persons);
            AddToListBox();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (populationCreated)
            {
                Int32 cycles = Int32.Parse(textBox4.Text);
                int lasts = Int32.Parse(textBox3.Text);
                Random rand = new Random();

                for (int i = 0; i < cycles; i++)
                {
                    population.persons.Sort(new SortByFit());
                    population.persons.RemoveRange(lasts, population.persons.Count - lasts);

                    population.AppendPopulation(Int32.Parse(textBox2.Text));
                    population.Mutation(Int32.Parse(textBox3.Text), 0.02, rand);
                }               

                population.AllignAll(new Point(
                        pictureBox1.ClientRectangle.Width / 2,
                        pictureBox1.ClientRectangle.Height / 2)
                    );

                population.persons.Sort(new SortByFit());
                DrawPopulation(population.persons);
                AddToListBox();
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {

            population.AppendPopulation(Int32.Parse(textBox2.Text));
            population.Mutation(Int32.Parse(textBox3.Text), 0.02, rand);

            DrawPopulation(population.persons);
            AddToListBox();
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            List<Person> selectedPersons = new List<Person>();

            ListBox.SelectedIndexCollection indecies = listBox1.SelectedIndices;

            this.Text = "";
            foreach (Int32 item in indecies)
            {
                selectedPersons.Add(population.persons[item]);
            }

            DrawPopulation(selectedPersons);
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}