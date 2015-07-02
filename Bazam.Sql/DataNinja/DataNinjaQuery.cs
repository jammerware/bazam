using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using Bazam.DestupidifiedCollections;

namespace Bazam.DataNinja
{
    public class DataNinjaQuery
    {
        #region Fields
        private string _CommandText;
        private CommandType _CommandType;
        private int _CommandTimeout;
        private DestupidifiedDictionary<string, SqlParameter> _Parameters;
        #endregion

        #region Constructors
        public DataNinjaQuery(string commandText, CommandType commandType)
        {
            _CommandText = commandText;
            _CommandType = commandType;
            _Parameters = new DestupidifiedDictionary<string, SqlParameter>();
        }

        public DataNinjaQuery(string commandText, CommandType commandType, int commandTimeout) : this(commandText, commandType)
        {
            _CommandTimeout = commandTimeout;
        }

        public DataNinjaQuery(string commandText, CommandType commandType, params SqlParameter[] parameters) : this(commandText, commandType)
        {
            foreach (SqlParameter parameter in parameters)
                SetParameter(parameter);
        }

        public DataNinjaQuery(string commandText, CommandType commandType, params object[] parameterDefinitions) : this(commandText, commandType)
        {
            if (parameterDefinitions.Length % 2 != 0)
                throw new DataNinjaException("A DataNinjaQuery was initialized with an invalid parameter list.");
            else {
                SqlParameter currentParam = null;
                for (int i = 0; i < parameterDefinitions.Length; i++) {
                    if (i % 2 == 0) {
                        currentParam = new SqlParameter();
                        currentParam.ParameterName = parameterDefinitions[i].ToString();
                    }
                    else {
                        currentParam.Value = parameterDefinitions[i];
                        SetParameter(currentParam);
                    }
                }
            }
        }

        public DataNinjaQuery(string commandText, CommandType commandType, int commandTimeout, params SqlParameter[] parameters) : this(commandText, commandType, parameters)
        {
            _CommandTimeout = commandTimeout;
        }

        public DataNinjaQuery(string commandText, CommandType commandType, int commandTimeout, params object[] parameterDefinitions) : this(commandText, commandType, parameterDefinitions)
        {
            _CommandTimeout = commandTimeout;
        }
        #endregion

        #region Field Properties
        public string CommandText
        {
            get { return _CommandText; }
        }

        public int CommandTimeout
        {
            get { return _CommandTimeout; }
        }

        public CommandType CommandType
        {
            get { return _CommandType; }
        }

        private DestupidifiedDictionary<string, SqlParameter> Parameters
        {
            get { return _Parameters; }
        }
        #endregion

        #region Overridden Methods
        public override bool Equals(object obj)
        {
            if(obj.GetType() == typeof(DataNinjaQuery)) {
                DataNinjaQuery typedObj = (DataNinjaQuery)obj;
                return typedObj.CommandText == CommandText && typedObj.CommandTimeout == CommandTimeout && typedObj.CommandType == CommandType && typedObj.Parameters.Equals(Parameters);
            }
            return false;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
        #endregion

        #region Methods
        public void SetParameter(string name, object value)
        {
            SetParameter(new SqlParameter(name, value));
        }

        public void SetParameter(SqlParameter parameter)
        {
            if (!parameter.ParameterName.StartsWith("@"))
                parameter.ParameterName = "@" + parameter.ParameterName;
            _Parameters[parameter.ParameterName] = parameter;
        }

        public SqlParameter GetParameter(string name)
        {
            return _Parameters[name];
        }

        public SqlParameter[] GetParameters()
        {
            return _Parameters.Values.ToArray();
        }
        #endregion
    }
}
