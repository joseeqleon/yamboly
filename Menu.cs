using CapaDatos;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;



namespace Tienda_Yamboly
{
    public partial class Menu : Form
    {
        // Instanciar la conexión
        Conexion con = new Conexion();

        public Menu()
        {
            InitializeComponent();
        }

        // Evento Load: carga la información en el DataGridView
        private void Menu_Load(object sender, EventArgs e)
        {
            MostrarDatos();
        }

        // Método para mostrar datos en el DataGridView
        private void MostrarDatos()
        {
            try
            {
                using (SqlConnection cn = con.Conectar())
                {
                    string query = "SELECT * FROM Producto";
                    SqlDataAdapter da = new SqlDataAdapter(query, cn);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    dataGridView1.DataSource = dt;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar datos: " + ex.Message);
            }
        }

        // Evento Click del botón Agregar
        private void btnAgregar_Click(object sender, EventArgs e)
        {
            try
            {
                using (SqlConnection cn = con.Conectar())
                {
                    string query = "INSERT INTO Producto (ProductoID) VALUES (@ProductoID)";
                    SqlCommand cmd = new SqlCommand(query, cn);
                    cmd.Parameters.AddWithValue("@ProductoID", txtProductoID.Text.Trim());

                    int filas = cmd.ExecuteNonQuery();
                    if (filas > 0)
                        MessageBox.Show("Registro agregado exitosamente");
                    else
                        MessageBox.Show("No se pudo agregar el registro");

                    MostrarDatos();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al agregar dato: " + ex.Message);
            }
        }

        // Evento Click del botón Actualizar
        private void btnActualizar_Click(object sender, EventArgs e)
        {
            try
            {
                if (dataGridView1.CurrentRow != null)
                {
                    // Se toma el identificador original del registro seleccionado
                    string idAntiguo = dataGridView1.CurrentRow.Cells["ProductoID"].Value.ToString();

                    using (SqlConnection cn = con.Conectar())
                    {
                        string query = "UPDATE Producto SET ProductoID = @ProductoID WHERE ProductoID = @ProductoIDOld";
                        SqlCommand cmd = new SqlCommand(query, cn);
                        cmd.Parameters.AddWithValue("@ProductoID", txtProductoID.Text.Trim());
                        cmd.Parameters.AddWithValue("@ProductoIDOld", idAntiguo);

                        int filas = cmd.ExecuteNonQuery();
                        if (filas > 0)
                            MessageBox.Show("Registro actualizado exitosamente");
                        else
                            MessageBox.Show("No se pudo actualizar el registro");

                        MostrarDatos();
                    }
                }
                else
                {
                    MessageBox.Show("Seleccione un registro para actualizar");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al actualizar: " + ex.Message);
            }
        }

        // Evento Click del botón Eliminar
        private void btnEliminar_Click(object sender, EventArgs e)
        {
            try
            {
                if (dataGridView1.CurrentRow != null)
                {
                    string idEliminar = dataGridView1.CurrentRow.Cells["ProductoID"].Value.ToString();

                    using (SqlConnection cn = con.Conectar())
                    {
                        string query = "DELETE FROM Producto WHERE ProductoID = @ProductoID";
                        SqlCommand cmd = new SqlCommand(query, cn);
                        cmd.Parameters.AddWithValue("@ProductoID", idEliminar);

                        int filas = cmd.ExecuteNonQuery();
                        if (filas > 0)
                            MessageBox.Show("Registro eliminado exitosamente");
                        else
                            MessageBox.Show("No se pudo eliminar el registro");

                        MostrarDatos();
                    }
                }
                else
                {
                    MessageBox.Show("Seleccione un registro para eliminar");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al eliminar: " + ex.Message);
            }
        }

        // Evento para seleccionar un registro del DataGridView y mostrar sus datos en el TextBox
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            // Evitar errores si se hace clic en la cabecera
            if (e.RowIndex >= 0)
            {
                DataGridViewRow fila = dataGridView1.Rows[e.RowIndex];
                txtProductoID.Text = fila.Cells["ProductoID"].Value.ToString();
            }
        }
    }
}
