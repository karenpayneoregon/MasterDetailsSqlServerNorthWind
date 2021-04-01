using System.Data;

namespace DataOperations
{
    public static class DataRelationsExtensions
    {
        /// <summary>
        /// USed to create a one to many relationship for a master-detail in a DataSet.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="masterTableName">master table</param>
        /// <param name="childTableName">child table of master table</param>
        /// <param name="masterKeyColumn">master table primary key</param>
        /// <param name="childKeyColumn">child table of master's primary key</param>
        public static void SetRelation(this DataSet sender, string masterTableName, string childTableName, string masterKeyColumn, string childKeyColumn)
        {
            sender.Relations.Add(new DataRelation(string.Concat(masterTableName, childTableName), sender.Tables[masterTableName].Columns[masterKeyColumn], sender.Tables[childTableName].Columns[childKeyColumn]));
        }
    }
}
