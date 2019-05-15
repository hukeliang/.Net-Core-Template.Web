using Common.Helper.Core.CSharpExpand;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text.RegularExpressions;

namespace Template.Entity
{
    public class DbCommandInterceptor : IObserver<KeyValuePair<string, object>>
    {

        private readonly WeightedRoundRobin weightedRoundRobin = null;

        public DbCommandInterceptor(params string[] slaveConnectionString)
        {

            weightedRoundRobin = new WeightedRoundRobin(this.GetServerObject(slaveConnectionString));
        }

        public void OnCompleted()
        {
            throw new NotImplementedException();
        }

        public void OnError(Exception error)
        {
            throw new NotImplementedException();
        }


        /// <summary>
        /// 截获sql查询
        /// </summary>
        /// <param name="value"></param>
        public void OnNext(KeyValuePair<string, object> value)
        {
            if (value.Key == RelationalEventId.CommandExecuting.Name)
            {
                CommandEventData commandEventData = value.Value as CommandEventData;

                DbCommand command = commandEventData.Command;

                DbCommandMethod executeMethod = commandEventData.ExecuteMethod;

                Server server = new Server();
                for (int i = 0; i < 1000; i++)
                {
                    server = weightedRoundRobin.GetServer();

                    Console.Out.WriteLine(server.Weight);
                }
                
                switch (executeMethod)
                {
                    case DbCommandMethod.ExecuteNonQuery:
                        //ResetConnection(command, _masterConnectionString);
                        break;
                    case DbCommandMethod.ExecuteScalar: //如果是异步查询
                        ResetConnection(command, server.Address);
                        break;
                    case DbCommandMethod.ExecuteReader://如果是查询
                        ResetConnection(command, server.Address);
                        break;
                }
            }

        }

        /// <summary>
        /// 重新设置链接字符串
        /// </summary>
        /// <param name="command"></param>
        /// <param name="connectionString"></param>
        private void ResetConnection(DbCommand command, string connectionString)
        {
            if (command.Connection.State == ConnectionState.Open)
            {
                if (!command.CommandText.Contains("@@ROWCOUNT"))
                {
                    command.Connection.Close();
                    command.Connection.ConnectionString = connectionString;
                }
            }

            if (command.Connection.State == ConnectionState.Closed)
            {
                command.Connection.Open();
            }
        }


        private List<Server> GetServerObject(params string[] slaveConnectionString)
        {
            const string weightStr = " Weight=";

            Regex regex = new Regex(weightStr ,RegexOptions.IgnoreCase);

            List<Server> serversList = new List<Server>();

            foreach (string item in slaveConnectionString)
            {
                // 在字符串中匹配
                Match match = regex.Match(item);
                if (match.Success)
                {
                    //输入匹配字符的位置
                    int addressIndex = match.Index;
                    int weightIndex = match.Index + weightStr.Length;
                    Server server = new Server()
                    {
                        Address = item.Substring(0, addressIndex),
                        Weight = int.Parse(item.Substring(weightIndex, 1)),
                    };

                    serversList.Add(server);
                }
            }
            return serversList;
        }
    }
}
