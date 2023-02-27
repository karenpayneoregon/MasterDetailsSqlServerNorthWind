using DataOperations;
using System;
using System.Windows.Forms;

namespace WindowsApplication_cs
{
    /// <summary>
    /// Simple example for master detail with ComboBox controls
    /// </summary>
    public partial class ComboBoxForm : Form
    {
        BindingSource _bsMaster = new BindingSource();
        BindingSource _bsDetails = new BindingSource();
        public ComboBoxForm()
        {
            InitializeComponent();
        }

        private void ComboBoxForm_Load(object sender, EventArgs e)
        {
            var ops = new Operations();
            ops.LoadData();

            _bsMaster = ops.Master;
            _bsDetails = ops.Details;

            CustomerComboBox.DataSource = _bsMaster;
            CustomerComboBox.DisplayMember = "LastName";

            OrdersComboBox.DataSource = _bsDetails;
            OrdersComboBox.DisplayMember = "Invoice";


        }
    }
}
