using System;
using System.Data;
using System.Data.SqlClient;

namespace SME.DB
{
    public class DumpsDBM : IDisposable
    {
    #region Members
        public string ConnString { get; protected internal set;}
        private SqlConnection SQLConnection { get; set; }

	#endregion

    #region Constructors
        public DumpsDBM(string connstr)
        {
            ConnString = string.Format("{0}", connstr);
            SQLConnection = new SqlConnection(ConnString);
            SQLConnection.Open();
        }
    #endregion

    #region private Methods
        
	#endregion

    #region public Methods
        public void Insert(string dumpname, int projectid)
        {
            string InsertQuery = string.Format("Insert INTO Dumps(Dump_Name, ProjectsProject_ID) VALUES('{0}','{1}')",
                dumpname, projectid);
            SqlCommand command = new SqlCommand(InsertQuery, SQLConnection);
            command.ExecuteNonQuery();
        }

        public DataSet SelectExp_ID(int projectid)
        {
            DataSet dataset = new DataSet();
            string query = string.Format("SELECT Dump_ID FROM Dumps WHERE ProjectsProject_ID = '{0}'", projectid);
            SqlCommand command = new SqlCommand(query, SQLConnection);
            SqlDataAdapter adapter = new SqlDataAdapter(query, SQLConnection);
            adapter.Fill(dataset);
            return dataset;
        }

        public DataSet SelectExp_ID(string dumpname)
        {
            DataSet dataset = new DataSet();
            string query = string.Format("SELECT Dump_ID FROM Dumps WHERE Dump_Name = '{0}'", dumpname);
            SqlCommand command = new SqlCommand(query, SQLConnection);
            SqlDataAdapter adapter = new SqlDataAdapter(query, SQLConnection);
            adapter.Fill(dataset);
            return dataset;
        }
    #endregion

    #region Dispose Interface
		void IDisposable.Dispose()
        {
 	        if(SQLConnection != null)
            {
                SQLConnection.Close();
            }
        } 
	#endregion
    }
}