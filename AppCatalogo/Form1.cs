using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Forms;
using dominio;
using negocio;


namespace AppCatalogo
{
    public partial class FmrCatalogo : Form
    {
        private List<Articulo> listaArticulo;
   
        public FmrCatalogo()
        {
            InitializeComponent();
        }

        private void FmrCatalogo_Load(object sender, EventArgs e)
        {
            cargar();
            cboCampo.Items.Add("Nombre");
            cboCampo.Items.Add("Artículo");
            cboCampo.Items.Add("Precio");
        }

        private void dgvCatalogo_SelectionChanged(object sender, EventArgs e)
        {

            if(dgvCatalogo.CurrentRow != null)
            {
                Articulo seleccionado = (Articulo)dgvCatalogo.CurrentRow.DataBoundItem;
                cargarImagen(seleccionado.ImagenUrl);
            }
            
        }

        private void cargar()
        {
            ArticuloNegocio negocio = new ArticuloNegocio();
            try
            {
                listaArticulo = negocio.listar();
                dgvCatalogo.DataSource = listaArticulo;
                ocultarColumnas();
                cargarImagen(listaArticulo[0].ImagenUrl);
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }
        }

        private void ocultarColumnas()
        {
            dgvCatalogo.Columns["ImagenUrl"].Visible = false;
            dgvCatalogo.Columns["Id"].Visible = false;
        }
        private void cargarImagen(string imagen)
        {
            try
            {
                pxbCatalogo.Load(imagen);
            }
            catch (Exception)
            {

                pxbCatalogo.Load("https://i0.wp.com/thefoodmanager.com/wp-content/uploads/2021/03/placeholder.png?ssl=1");
            }
         }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            fmrAltaCatalogo alta = new fmrAltaCatalogo();
            alta.ShowDialog();
            cargar();
        }

        private void btnElimar_Click(object sender, EventArgs e)
        {
            ArticuloNegocio negocio = new ArticuloNegocio();
            Articulo seleccionado;
            try
            {
                if(dgvCatalogo.CurrentRow != null && txtFiltroRapido.Text != " ")
                {
                    DialogResult respuesta = MessageBox.Show("¿Desea eliminarlo definitivamente?", "Eliminando", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                    if (respuesta == DialogResult.Yes)
                    {
                        seleccionado = (Articulo)dgvCatalogo.CurrentRow.DataBoundItem;
                        negocio.eliminar(seleccionado.Id);
                        cargar();
                    }
                }
                else
                {
                    MessageBox.Show("Debe seleccionar un árticulo");
                }

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        private bool validarFiltro()
        {
            if (cboCampo.SelectedIndex < 0)
            {
                MessageBox.Show("Por favor, seleccione el campo para fitrar.");
                return true;
            }
            if(cboCriterio.SelectedIndex < 0)
            {
                MessageBox.Show("Por favor, seleccione el criterio para fitrar.");
                return true;
            }
            if(cboCampo.SelectedItem.ToString() == "Precio")
            {
            if (string.IsNullOrEmpty(txtFiltroAvanzado.Text))
            {
                MessageBox.Show("Debes cargar el filtro para numéricos");
                return true;
            }
            if (!(soloNúmeros(txtFiltroAvanzado.Text)))
            {
                MessageBox.Show("Solo números para filtrar un campo numérico");
                return true;
            }

            }
            return false;
        }
        private bool soloNúmeros (string cadena)
        {
            foreach (char caracter in cadena)
            {
                if(!(char.IsNumber(caracter)))
                    return false;
            }
            return true;
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            ArticuloNegocio negocio = new ArticuloNegocio();
            try
            {
                if (validarFiltro())
                    return;
                string campo = cboCampo.SelectedItem.ToString();
                string criterio = cboCriterio.SelectedItem.ToString();
                string filtro = txtFiltroAvanzado.Text;
                dgvCatalogo.DataSource = negocio.filtrar(campo, criterio, filtro);
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }
           
            
        }
        private void txtFiltroRapido_TextChanged(object sender, EventArgs e)
        {
            List<Articulo> listaFiltrada;
            string filtroRapido = txtFiltroRapido.Text;

            if (filtroRapido.Length >= 3)
            {
                listaFiltrada = listaArticulo.FindAll(x => x.Nombre.ToUpper().Contains(filtroRapido.ToUpper()) || x.Marcas.Descripcion.ToUpper().Contains(filtroRapido.ToUpper()) || x.Articulos.Descripcion.ToUpper().Contains(filtroRapido.ToUpper()));

            }
            else
            {
                listaFiltrada = listaArticulo;
            }
            dgvCatalogo.DataSource = null;
            dgvCatalogo.DataSource = listaFiltrada;
            ocultarColumnas();

        }

        private void cboCampo_SelectedIndexChanged(object sender, EventArgs e)
        {
            string opcion = cboCampo.SelectedItem.ToString();
            if(opcion == "Precio")
            {
                cboCriterio.Items.Clear();
                cboCriterio.Items.Add("Mayor a");
                cboCriterio.Items.Add("Menor a");
                cboCriterio.Items.Add("Igual a");
            }
            else
            {
                cboCriterio.Items.Clear();
                cboCriterio.Items.Add("Comienza con");
                cboCriterio.Items.Add("Termina con");
                cboCriterio.Items.Add("Contiene");
            }
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            cargar();
        }

        private void btnModificar_Click_1(object sender, EventArgs e)
        {
            
            try
            {
                

                if (dgvCatalogo.CurrentRow != null && txtFiltroRapido.Text != " ")
                {
                    Articulo seleccionado;
                    seleccionado = (Articulo)dgvCatalogo.CurrentRow.DataBoundItem;
                    fmrAltaCatalogo modificar = new fmrAltaCatalogo(seleccionado);
                    modificar.ShowDialog();
                    cargar();
                }
                else
                {
                    MessageBox.Show("Debe seleccionar un árticulo ");
                }
              
            }
            catch (Exception ex)
            {
                throw ex;
            }
            
           
        }
    }
        
}
