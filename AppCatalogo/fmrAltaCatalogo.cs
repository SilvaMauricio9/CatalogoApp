using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using dominio;
using negocio;
using System.Configuration;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TextBox;
using System.Windows.Controls;

namespace AppCatalogo
{
    public partial class fmrAltaCatalogo : Form
    {
        private Articulo articulo = null;

        private OpenFileDialog archivo = null;
        public fmrAltaCatalogo()
        {
            InitializeComponent();
        }

        public fmrAltaCatalogo(Articulo articulo)
        {
            InitializeComponent();
            this.articulo = articulo;
            Text = "Modificar Articulo";
        }
        private void btnCancelar_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnConfirmar_Click(object sender, EventArgs e)
        {
            
            ArticuloNegocio negocio = new ArticuloNegocio();

            try
            {
                if (validarFiltro())
                    return;
                    if (articulo == null)
                        articulo = new Articulo();
                    articulo.Codigo = txtCodigo.Text;
                    articulo.Nombre = txtNombre.Text;
                    articulo.Descripcion = txtDescripcion.Text;
                    articulo.Precio = decimal.Parse(txtPrecio.Text);
                    articulo.ImagenUrl = txtUrlImagen.Text;
                    articulo.Articulos = (Categoria)cboProducto.SelectedItem;
                    articulo.Marcas = (Marca)cboMarca.SelectedItem;
                
               

                if (articulo.Id != 0)
                {
                    negocio.modificar(articulo);
                    MessageBox.Show("Modificado exitosamente");
                }
                else
                {
                    negocio.agregar(articulo);
                    MessageBox.Show("Agregado exitosamente");
                }

                if(archivo != null && !(txtUrlImagen.Text.ToUpper().Contains("HTTP")))
                {

                    File.Copy(archivo.FileName, ConfigurationManager.AppSettings["Art.App"] + archivo.SafeFileName);
                }
               
                Close();
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }
        }

        private void fmrAltaCatalogo_Load(object sender, EventArgs e)
        {
            CategorioNegocio categoriaNegocio = new CategorioNegocio();
            MarcaNegocio marcaNegocio = new MarcaNegocio();
            try
            {
                
                cboProducto.DataSource = categoriaNegocio.listar();
                cboProducto.ValueMember = "Id";
                cboProducto.DisplayMember = "Descripcion";
                cboMarca.DataSource = marcaNegocio.listar();
                cboMarca.ValueMember = "Id";
                cboMarca.DisplayMember = "Descripcion";
                
                if (articulo != null)
                {
                    
                    txtCodigo.Text = articulo.Codigo;
                    txtNombre.Text = articulo.Nombre;
                    txtDescripcion.Text = articulo.Descripcion;
                    decimal precio = articulo.Precio;
                    txtPrecio.Text = precio.ToString();
                    txtUrlImagen.Text = articulo.ImagenUrl;
                    cargarImagen(articulo.ImagenUrl);
                    cboProducto.SelectedValue = articulo.Articulos.Id;
                    cboMarca.SelectedValue = articulo.Marcas.Id;

                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }

        }

        private void txtUrlImagen_Leave(object sender, EventArgs e)
        {
            cargarImagen(txtUrlImagen.Text);
        }
        private void cargarImagen(string imagen)
        {
            try
            {
                pxbUrlImagen.Load(imagen);
            }
            catch (Exception ex)
            {

                pxbUrlImagen.Load("https://i0.wp.com/thefoodmanager.com/wp-content/uploads/2021/03/placeholder.png?ssl=1");
            }
        }

        private void btnArchivo_Click(object sender, EventArgs e)
        {
            archivo = new OpenFileDialog();
            archivo.Filter = "jpg |*.jpg; |png|*.png";
            if (archivo.ShowDialog() == DialogResult.OK)
            {
                txtUrlImagen.Text = archivo.FileName;
                cargarImagen(archivo.FileName);

                File.Copy(archivo.FileName, ConfigurationManager.AppSettings["Art.App"] + archivo.SafeFileName);
            }
        }

        private bool validarFiltro()
        {
            bool valido = false;
            if (txtCodigo.Text == "")
            {
                valido = true;
                errorAlta.SetError(txtCodigo, "Ingrese el código");
            }
            
            if(txtNombre.Text == "")
            {
                valido= true;
                errorAlta.SetError(txtNombre, "Ingrese el nombre");
            }
            if(txtDescripcion.Text == "")
            {
                valido= true;
                errorAlta.SetError(txtDescripcion, "Ingrese la descripción");
            }
            if (txtPrecio.Text == "")
            {
                valido= true;
                errorAlta.SetError(txtPrecio, "Ingrese el precio");                
            }
            if (!soloNúmeros(txtPrecio.Text))
            {
                MessageBox.Show("Solo números para el precio por favor");
                return true;
            }
            return valido;
        }
        private bool soloNúmeros(string cadena)
        {
            foreach (char caracter in cadena)
            {
                if (!(char.IsNumber(caracter)))
                    return false;
            }
            return true;
        }
        
    }
}
