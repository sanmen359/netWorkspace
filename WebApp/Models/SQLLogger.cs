using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.Common;
using Microsoft.Extensions.Configuration;

namespace WebApp.Models
{
    public class SQLLogger : ILogger, IDisposable
    {
        private const string InsertSql = "INSERT INTO CM_Log ([Time],[Level],[Logger],[Message],[Exception]) VALUES (@log_date, @log_level, @logger, @message, @exception)";
        private System.Data.SqlClient.SqlConnection Connection;
        private string _Name;
        public string Name { get { return _Name; } }
        private Func<string, LogLevel, bool> _filter;
        private string _Connection;
        public SQLLogger(IConfiguration Configuration)
        {
            //var LogLevel = Configuration.GetValue<LogLevel>("Logging:LogLevel:Default");
            var LogLevel = Configuration.GetSection("Loggin").GetSection("LogLevel").GetValue<LogLevel>("Default");
            _Name ="sql";
            _filter = (name,level)=> { return LogLevel>=level; };
            _Connection = Configuration.GetConnectionString("Logger");
        }
        public IDisposable BeginScope<TState>(TState state)
        {
            return this;
        }

        public void Dispose()
        {
            if (Connection != null)
            {
                Connection.Close();
                Connection.Dispose();
            }
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return _filter.Invoke(_Name, logLevel);
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {

            using (Connection = new System.Data.SqlClient.SqlConnection(_Connection))
            {

                Connection.Open();
                System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand(InsertSql,Connection);
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@log_date",DateTime.Now));
                //cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@thread", 0));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@log_level",logLevel.ToString()));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@logger", Name));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@message", formatter(state,exception)));

                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@exception", exception.StackTrace));
                cmd.ExecuteNonQuery();
            }
        }
    }
}
