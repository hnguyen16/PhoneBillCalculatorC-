using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PhoneBillCalculator
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            double equipment = Convert.ToDouble(txtEquipment.Text);
            double tax = Convert.ToDouble(txtTax.Text);
            List<phoneBill> pList = new List<phoneBill>();
            pList.Add(new phoneBill("Ba Hanh", Convert.ToDouble(txtBaH.Text), 2));
            pList.Add(new phoneBill("Hien", Convert.ToDouble(txtHien.Text), 1));
            pList.Add(new phoneBill("DanFam", Convert.ToDouble(txtDanFam.Text), 5));

            pList.ForEach(x => x.calculate(equipment, tax));

            var dt = ConvertToDataTable(pList);
            DataRow row = dt.NewRow();
            DataRow row2 = dt.NewRow();
            row2["name"] = "Total";
            row2["tax"] = Math.Round(pList.Sum(x => x.total), 2);
            dt.Rows.Add(row);
            dt.Rows.Add(row2);
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView1.DataSource = dt;

        }

        public DataTable ConvertToDataTable<T>(IList<T> data)
        {
            PropertyDescriptorCollection properties =
               TypeDescriptor.GetProperties(typeof(T));
            DataTable table = new DataTable();
            foreach (PropertyDescriptor prop in properties)
                table.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);
            foreach (T item in data)
            {
                DataRow row = table.NewRow();
                foreach (PropertyDescriptor prop in properties)
                    row[prop.Name] = prop.GetValue(item) ?? DBNull.Value;
                table.Rows.Add(row);
            }
            return table;

        }

    }

    public class phoneBill
    {
        public string name { get; set; }
        public double equipment { get; set; }
        public double tax { get; set; }
        private double billPerc { get; set; }
        public double total { get; set; }

        public phoneBill(string name, double equipment, int multiplier)
        {
            this.name = name;
            this.equipment = equipment + (90 / 8 * multiplier);
        }

        public void calculate(double equipmentTotal, double taxTotal)
        {
            this.billPerc = this.equipment / equipmentTotal;
            this.equipment = Math.Round(equipmentTotal * this.billPerc, 2, MidpointRounding.AwayFromZero);
            this.tax = Math.Round(taxTotal * this.billPerc, 2, MidpointRounding.AwayFromZero);
            this.total = Math.Round(this.equipment + this.tax, 2, MidpointRounding.AwayFromZero);
        }            
    }
}
