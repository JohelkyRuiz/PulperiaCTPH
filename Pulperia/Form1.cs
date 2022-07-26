using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace Pulperia
{
    public partial class Form1 : Form
    {
        SqlConnection Conec = new SqlConnection("Data Source=DESKTOP-VTK955H;Initial Catalog=Articulos;Integrated Security=True");

        double PrecioProdu = 0;
        double Subtotal = 0;


        public Form1()
        {
            InitializeComponent();

        }

        private void PestanaVenta_Click(object sender, EventArgs e)
        {
            //Sin función. (Ignorar)
        }

        private void btnSalir_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            txtCantidad.Clear();
            txtImpuesto.Clear();
            txtSubtotal.Clear();
            txtTotal.Clear();
        }

        private void txtCantidad_TextChanged(object sender, EventArgs e)
        {
            //Sin función. (Ignorar)
        }

        private void txtCantidad_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((e.KeyChar >= 32 && e.KeyChar <= 47) || (e.KeyChar >= 58 && e.KeyChar <= 255))
            {
                MessageBox.Show("Sólo números.", "Alerta!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                e.Handled = true;
                return;
            }
        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            /* Conec.Open();
             DataGridViewRow fila = new DataGridViewRow();
             fila.CreateCells(DataGVArticulos);
             fila.Cells[0].Value = ComboArticulos.Text;
            */
            Conec.Open();
            SqlCommand SelectPrecio = new SqlCommand($"Select Precio_articulo From TArticulo where Nombre_articulo = '{ComboArticulos.Text}'",Conec);
            PrecioProdu = Convert.ToDouble(SelectPrecio.ExecuteScalar());
     
            Conec.Close();

             Subtotal = PrecioProdu * Convert.ToDouble(txtCantidad.Text);

            DataGVArticulos.Rows.Add(ComboArticulos.Text, txtCantidad.Text, Subtotal.ToString());


        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //Seleccionar nombres para combobox
            Conec.Open();
            SqlCommand SelectNombre = new SqlCommand("Select Nombre_articulo From TArticulo", Conec);
            SqlDataReader Nombres = SelectNombre.ExecuteReader();

            

            while (Nombres.Read())
            {
                ComboArticulos.Items.Add(Nombres["Nombre_articulo"].ToString());
                
            }

            Conec.Close();

            Conec.Open();
            
            string consulta = "Select * From TArticulo";
            SqlDataAdapter adap = new SqlDataAdapter(consulta, Conec);
            DataTable dt = new DataTable();

            adap.Fill(dt);
            dtgInventario.DataSource = dt;

            Conec.Close();
        }

        private void btnCalcular_Click(object sender, EventArgs e)
        {
            double impuesto = Subtotal * 0.13;
            double Total = Subtotal + impuesto;

            txtSubtotal.Text = Subtotal.ToString();
            txtImpuesto.Text = impuesto.ToString();
            txtTotal.Text = Total.ToString();
            
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void PestanaInventario_Click(object sender, EventArgs e)
        {

        }

        private void btnAgregarInventario_Click(object sender, EventArgs e)
        {
            string Articulo = txtArticulo.Text;
            double Precio = Convert.ToDouble(txtPrecio.Text);
            string Unidad = txtUnidad.Text;

            Conec.Open();

            SqlCommand insertar = new SqlCommand($"Insert into TArticulo(Nombre_articulo,Precio_articulo,Unidad_articulo) values({Articulo},{Precio},{Unidad})",Conec);
            dtgInventario.Rows.Add(Articulo, Precio, Unidad);

            Conec.Close();
        }
    }
}
