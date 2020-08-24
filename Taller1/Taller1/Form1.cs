using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Taller1
{
    public partial class Form1 : Form
    {
        private List<string[]> values;
        public Form1()
        {
            InitializeComponent();
        }
        
        private void button1_Click(object sender, EventArgs e)
        {
            values = new List<string[]>();
            if (openFileDialog1.ShowDialog() == DialogResult.OK) {
                String rutan = openFileDialog1.FileName;
                string[] lines = System.IO.File.ReadAllLines(openFileDialog1.FileName);
               
                Dictionary<string, bool> regiones = new Dictionary<string, bool>();
                foreach (string line in lines)
                {
                    string[] rowLine = line.Split(',');

                    if (!rowLine[0].Equals("REGION"))
                    {
                        values.Add(rowLine);
                        grid.Rows.Add(rowLine);
                    }

                    if (!regiones.ContainsKey(rowLine[0]))
                    {
                        regiones.Add(rowLine[0], true);
                        cb.Items.Add(rowLine[0]);
                    }
                }

            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string dep = "Departamentos por region";
            grafico.Series.Clear();
            grafico.Series.Add(dep);

            Dictionary<string, Dictionary<string, int>> regiones = new Dictionary<string, Dictionary<string, int>>();
            foreach (string[] row in values)
            {
                string region = row[0];
                string dept = row[2];
                if (!regiones.ContainsKey(region))
                {
                    regiones.Add(region, new Dictionary<string, int>());
                    regiones[region].Add(dept, 1);
                }
                else
                {
                    if (!regiones[region].ContainsKey(dept))
                    {
                        regiones[region].Add(dept, 1);
                    }
                    else
                    {
                        regiones[region][dept] += 1;
                    }
                }
            }

            foreach (KeyValuePair<string, Dictionary<string, int>> kvp in regiones)
            {
                grafico.Series[dep].Points.AddXY(kvp.Key, kvp.Value.Count);
            }

            grafico.Series[dep].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Bar;
        }
    

        public void clearGrid() {
            grid.Rows.Clear();
        }

        private void cb_SelectedIndexChanged(object sender, EventArgs e)
        {
            clearGrid();
            foreach (string[] row in values)
            {
                if (cb.SelectedItem.Equals(row[0]) || cb.SelectedItem.Equals("REGION"))
                {
                    grid.Rows.Add(row);
                }
            }
        }

    }
}
