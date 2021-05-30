using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using DataOperations;
using static MessageDialogs.Dialogs;

namespace WindowsApplication_cs
{
    public partial class MainForm : Form
    {
        /// <summary>
        /// Containers for our customer and orders data
        /// which are setup in Operations class.
        /// </summary>
        BindingSource _bsMaster = new BindingSource();
        BindingSource _bsDetails = new BindingSource();
        BindingSource _bsOrderDetails = new BindingSource();

        List<StateItems> _stateInformation;

        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            var ops = new Operations();

            var DetailsBindingNavigatorKaren = new ToolStripButton()
            {
                Name = "newButon",
                Image = Properties.Resources.exit
            };

            DetailsBindingNavigatorKaren.Click += DetailsBindingNavigatorKaren_Click;
            MasterBindingNavigator.Items.Add(DetailsBindingNavigatorKaren);

            ops.LoadData();

            if (!ops.HasErrors)
            {

                _stateInformation = ops.StateInformation;

                _bsMaster = ops.Master;
                _bsDetails = ops.Details;

                MasterDataGridView.DataSource = _bsMaster;
                DetailsDataGridView.AutoGenerateColumns = false;

                DetailsDataGridView.DataSource = _bsDetails;
                OrderDateColumn.ReadOnly = false;
                UpdateButtonColumn.UseColumnTextForButtonValue = true;

                MasterBindingNavigator.BindingSource = _bsMaster;

                _bsOrderDetails = ops.OrderDetails;
                OrderDetailsDataGridView.DataSource = _bsOrderDetails;
                OrderDetailsDataGridView.Columns["id"].Visible = false;
                OrderDetailsDataGridView.Columns["OrderId"].Visible = false;
                DetailBindingNavigator.BindingSource = _bsDetails;

                OrderDetailsDataGridView.Columns["ProductName"].HeaderText = "Product";
                OrderDetailsDataGridView.Columns["UnitPrice"].HeaderText = "Unit price";

            }
            else
            {
                MessageBox.Show(ops.ExceptionMessage);
            }
        }

        private void DetailsBindingNavigatorKaren_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Clicked");
        }
        /// <summary>
        /// Point of entry for editing current customer from either
        /// a button on the BindingNavigator or pressing the enter key
        /// on the master DataGridView
        /// </summary>
        private void EditCurrentCustomer()
        {
            DataRow CustomerRow = ((DataRowView)_bsMaster.Current).Row;
            var customerForm = new CustomerForm(false, _stateInformation, ref CustomerRow);

            try
            {
                if (customerForm.ShowDialog() == DialogResult.OK)
                {
                    var ops = new Operations();
                    if (!(ops.UpdateCustomer(CustomerRow)))
                    {
                        MessageBox.Show($"Failed to update: {ops.ExceptionMessage}");
                    }
                }
            }
            finally
            {
                customerForm.Dispose();
            }
        }

        private void MasterDataGridView_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter)
            {
                e.Handled = true;
                EditCurrentCustomer();
            }
        }

        private void MasterBindingNavigatorEditCustomer_Click(object sender, EventArgs e)
        {
            EditCurrentCustomer();
        }

        private void MasterBindingNavigatorAddNewItem_Click(object sender, EventArgs e)
        {
            var customerForm = new CustomerForm(true, _stateInformation);

            try
            {

                if (customerForm.ShowDialog() == DialogResult.OK)
                {

                    Operations ops = new Operations();
                    int NewId = 0;

                    if (ops.AddCustomer(customerForm.txtFirstName.Text, customerForm.txtLastName.Text, customerForm.txtAddress.Text, customerForm.txtCity.Text, customerForm.cboStates.Text, customerForm.txtZipCode.Text, ref NewId))
                    {
                        var dt = ((DataSet)_bsMaster.DataSource).Tables["Customer"];
                        dt.Rows.Add(NewId, customerForm.txtFirstName.Text, customerForm.txtLastName.Text, customerForm.txtAddress.Text, customerForm.txtCity.Text, customerForm.cboStates.Text, customerForm.txtZipCode.Text);
                    }
                    else
                    {
                        MessageBox.Show($"Failed to add new row: {ops.ExceptionMessage}");
                    }
                }

            }
            finally
            {
                customerForm.Dispose();
            }
        }
        /// <summary>
        /// Remove current customer and all orders
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MasterBindingNavigatorDeleteCustomer_Click(object sender, EventArgs e)
        {
            if (Question("Do you really want to remove this customer and all their orders?"))
            {
                var ops = new Operations();
                int CustomerId = ((DataRowView)_bsMaster.Current).Row.Field<int>("id");
                if (ops.RemoveCustomerAndOrders(CustomerId))
                {
                    _bsMaster.RemoveCurrent();
                }
                else
                {
                    MessageBox.Show($"Failed to remove data{Environment.NewLine}{ops.ExceptionMessage}");
                }
            }
        }
        /// <summary>
        /// Update the order date for current row. 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <remarks>
        /// It's possible that there are no changes but for this code sample there is no check.
        /// If you wanted to implement this refer to DataTable events to peek at the current and
        /// proposed value for, in this case the data column.
        /// See my code sample on this
        /// https://code.msdn.microsoft.com/Working-with-DataTable-2ff5f158
        /// </remarks>
        private void DetailsDataGridView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            var senderGrid = (DataGridView)sender;

            if (senderGrid.Columns[e.ColumnIndex] is DataGridViewButtonColumn && e.RowIndex >= 0)
            {
                if (_bsDetails.Current != null)
                {

                    DateTime orderDate = ((DataRowView)_bsDetails.Current).Row.Field<DateTime>("OrderDate");
                    int orderId = ((DataRowView)_bsDetails.Current).Row.Field<int>("id");

                    var ops = new Operations();

                    if (!(ops.UpdateOrder(orderId, orderDate)))
                    {
                        MessageBox.Show($"Failed to update: {ops.ExceptionMessage}");
                    }

                }
            }
        }

        private void DetailsDataGridView_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter)
            {

                e.Handled = true;

                if (_bsDetails.Current != null)
                {
                    var orderForm = new OrderForm();
                    try
                    {
                        DateTime OrderDate = ((DataRowView)_bsDetails.Current).Row.Field<DateTime>("OrderDate");
                        orderForm.DateTimePicker1.Value = OrderDate;

                        if (orderForm.ShowDialog() == DialogResult.OK)
                        {

                            OrderDate = orderForm.DateTimePicker1.Value;

                            int orderId = ((DataRowView)_bsDetails.Current).Row.Field<int>("id");
                            var ops = new Operations();

                            if (!(ops.UpdateOrder(orderId, OrderDate)))
                            {
                                MessageBox.Show($"Failed to update: {ops.ExceptionMessage}");
                            }
                            else
                            {
                                ((DataRowView)_bsDetails.Current).Row.SetField<DateTime>("OrderDate", OrderDate);
                            }
                        }

                    }
                    finally
                    {
                        orderForm.Dispose();
                    }

                }
            }
        }

        private void DetailsBindingNavigatorAddNewItem_Click(object sender, EventArgs e)
        {
            var orderForm = new OrderForm();

            try
            {
                if (orderForm.ShowDialog() == DialogResult.OK)
                {
                    int id = 0;
                    int currentCustomerId = ((DataRowView)_bsMaster.Current).Row.Field<int>("id");
                    string invoiceValue = "";
                    var ops = new Operations();

                    ops.AddOrder(currentCustomerId, orderForm.DateTimePicker1.Value, ref invoiceValue, ref id);

                    if (!ops.HasErrors)
                    {
                        DataTable detailTable = ((DataSet)(((BindingSource)_bsDetails.DataSource).DataSource)).Tables["Orders"];
                        detailTable.Rows.Add(new object[] { id, currentCustomerId, DateTime.Now, invoiceValue });
                    }
                    else
                    {
                        MessageBox.Show(ops.ExceptionMessage);
                    }

                }
            }
            finally
            {
                orderForm.Dispose();
            }
        }

        private void DetailsBindingNavigatorDeleteItem_Click(object sender, EventArgs e)
        {
            if (Question("Remove this order?"))
            {

                int OrderId = ((DataRowView)_bsDetails.Current).Row.Field<int>("id");

                var ops = new Operations();

                if (!(ops.RemoveSingleOrder(OrderId)))
                {
                    MessageBox.Show($"Failed to update: {ops.ExceptionMessage}");
                }
                else
                {
                    _bsDetails.RemoveCurrent();
                }

            }
        }
        /// <summary>
        /// Get parent row id from child row
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            if (_bsDetails.Current != null)
            {
                var parentId = ((DataRowView)_bsDetails.Current).Row.GetParentRow("CustomerOrders").Field<int>("id");
                MessageBox.Show($"Parent id: {parentId}");

            }
            else
            {
                MessageBox.Show("There are no child rows");
            }
        }

        private void toolStripCloneButton_Click(object sender, EventArgs e)
        {

            var pRow = ((DataRowView)_bsMaster.Current).Row;

            Operations ops = new Operations();
            
            int NewCustomerId = 0;

            ops.AddCustomer(pRow.Field<string>("FirstName"), pRow.Field<string>("LastName"), pRow.Field<string>("Address"), pRow.Field<string>("City"), pRow.Field<string>("State"), pRow.Field<string>("State"), ref NewCustomerId);
            var customerDataTable = ((DataSet)_bsMaster.DataSource).Tables["Customer"];
            customerDataTable.Rows.Add(NewCustomerId, pRow.Field<string>("FirstName"), pRow.Field<string>("LastName"), pRow.Field<string>("Address"), pRow.Field<string>("City"), pRow.Field<string>("State"), pRow.Field<string>("ZipCode"));

            var childDataRows = pRow.GetChildRows("CustomerOrders");
            var newChildId = 0;
            var detailTable = ((DataSet)(((BindingSource)_bsDetails.DataSource).DataSource)).Tables["Orders"];
            
            foreach (var dataRow in childDataRows)
            {
                ops.AddOrder1(NewCustomerId, dataRow.Field<DateTime>("OrderDate"), dataRow.Field<string>("Invoice"), ref newChildId);
                detailTable.Rows.Add(newChildId, NewCustomerId, dataRow.Field<DateTime>("OrderDate"), dataRow.Field<string>("Invoice"));
            }
        }
    }
}
