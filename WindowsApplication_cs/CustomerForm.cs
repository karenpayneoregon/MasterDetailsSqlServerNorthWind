using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using DataOperations;

namespace WindowsApplication_cs
{
    public partial class CustomerForm : Form
    {
        private bool _inAddMode;
        private List<StateItems> _stateItems;
        private DataRow _customerRow;
        //currentRow
        private List<TextBox> _formTextBoxes = new List<TextBox>();
        public CustomerForm(bool AddMode, List<StateItems> stateInfo, ref DataRow currentRow)
        {
            InitializeComponent();
            _inAddMode = AddMode;
            _stateItems = stateInfo;
            _customerRow = currentRow;
        }
        public CustomerForm(bool AddMode, List<StateItems> stateInfo)
        {
            InitializeComponent();
            _inAddMode = AddMode;
            _stateItems = stateInfo;
        }
        public CustomerForm()
        {
            InitializeComponent();
        }
        private void CustomerForm_Load(object sender, EventArgs e)
        {
            _formTextBoxes.AddRange(Controls.OfType<TextBox>().ToArray());

            cboStates.DataSource = _stateItems;
            cboStates.DisplayMember = "Name";

            if (_inAddMode)
            {
                Text = "Adding new customer";
            }
            else
            {
                Text = "Editing current customer";

                txtFirstName.Text = _customerRow.Field<string>("FirstName");
                txtLastName.Text = _customerRow.Field<string>("LastName");
                txtAddress.Text = _customerRow.Field<string>("Address");
                txtCity.Text = _customerRow.Field<string>("City");
                txtZipCode.Text = _customerRow.Field<string>("ZipCode");

                string currentState = _customerRow.Field<string>("State");

                var customerState = _stateItems
                    .Select((data, index) => new { Index = index, Name = data.Name })
                    .Where((data) => data.Name == _customerRow.Field<string>("State"))
                    .FirstOrDefault();

                if (currentState != null)
                {
                    cboStates.SelectedIndex = customerState.Index;
                }
            }

        }

        private void cmdAccept_Click(object sender, EventArgs e)
        {
            if (_formTextBoxes.Where((textBox) => string.IsNullOrWhiteSpace(textBox.Text)).Any() || cboStates.SelectedIndex == 0)
            {
                MessageBox.Show("To save a record all fields must have information along with selecting a state.");
                return;
            }
            else
            {
                if (!_inAddMode)
                {
                    // remember we pass the data row by ref so these changes take affect to the UI but not the backend database table
                    // so we will handle this in the mainform.
                    _customerRow.SetField<string>("FirstName", txtFirstName.Text);
                    _customerRow.SetField<string>("LastName", txtLastName.Text);
                    _customerRow.SetField<string>("Address", txtAddress.Text);
                    _customerRow.SetField<string>("City", txtCity.Text);
                    _customerRow.SetField<string>("State", cboStates.Text);
                    _customerRow.SetField<string>("ZipCode", txtZipCode.Text);

                }

                DialogResult = DialogResult.OK;

            }

        }
        private void Button2_Click(object sender, EventArgs e)
        {
           
            if (_customerRow != null)
            {
                _customerRow.RejectChanges();
            }

            DialogResult = DialogResult.Cancel;
        }
    }
}
