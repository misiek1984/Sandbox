
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Transactions;

using Dapper;
using Dapper.Fluent;

using MK.Utilities;



namespace DapperTest
{
    class Program
    {
        public Program()
        {
        }

        private static void Main(string[] args)
        {
            using (var transactionScope = new TransactionScope())
            {
                using (var connection =
                    new SqlConnection(
                        @"Data Source=.;Initial Catalog=ExecutionTraceDB;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False")
                    )
                {
                    connection.Open();

                    Console.WriteLine(QueryWithDynamic(connection).Dump(3, indent: "  ", tab: " "));
                    Console.WriteLine(QueryWithMap(connection).Dump(3, indent: "  ", tab: " "));
                    Console.WriteLine(OneToManyQuery(connection).Dump(3, indent: "  ", tab: " "));
                    Console.WriteLine(QueryWithMapFluent(connection).Dump(3, indent: "  ", tab: " "));
                    Console.WriteLine(QueryMultiple(connection).Dump(3, indent: "  ", tab: " "));

                    Console.WriteLine(QuerySP(connection).Dump(3, indent: "  ", tab: " "));


                    ManualCRUD(connection);
                    SimpleCRUD(connection);
                }
            }

            Console.ReadLine();
        }


        private static void SimpleCRUD(SqlConnection connection)
        {
            Console.WriteLine("SimpleCRUD");

            var msg = new Message {Msg = "Hello", DateTime = DateTime.Now, ExecutionTraceId = 119};

            Console.WriteLine("Insert");

            var msgId = connection.Insert(msg);
            msg = connection.Get<Message>(msgId);

            Trace.Assert(msg.Msg == "Hello");

            Console.WriteLine("Update");

            msg.Msg = "test";
            connection.Update(msg);
            var newMessage = connection.Get<Message>(msg.Id);

            Trace.Assert(newMessage.Msg == "test");

            Console.WriteLine("Delete");

            var count = connection.Delete<Message>(msg);
            Trace.Assert(count == 1);
            msg = connection.Get<Message>(msg.Id);
            Trace.Assert(msg == null);
        }

        private static void ManualCRUD(SqlConnection connection)
        {
            Console.WriteLine("ManualCRUD");

            Console.WriteLine("Insert");

            var msgId = connection.Query<int>(
                "insert into dbo.Messages (Msg, DateTime, ExecutionTraceId) values (@Msg, @DateTime, @ExecutionTraceId);" +
                "SELECT CAST(SCOPE_IDENTITY() as int);",
                new {Msg = "Hello", DateTime = DateTime.Now, ExecutionTraceId = 119}).First();

            Console.WriteLine("Update");

            var msg = connection.Query<string>("select Msg from dbo.Messages where Id = @Id", new { Id = msgId }).Single();
            Trace.Assert(msg == "Hello");

            var count = connection.Execute("update dbo.Messages set MSg = @Msg where Id = @Id", new { Msg = "test", Id = msgId });
            Trace.Assert(count == 1);

            var newMsg = connection.Query<string>("select Msg from dbo.Messages where Id = @Id", new { Id = msgId }).Single();
            Trace.Assert(newMsg == "test");

            Console.WriteLine("Delete");

            count = connection.Query<int>("select count(1) from dbo.Messages where Id = @Id", new { Id = msgId }).Single();
            Trace.Assert(count == 1);

            count = connection.Execute("delete from dbo.Messages where Id = @Id", new { Id = msgId });
            Trace.Assert(count == 1);

            count = connection.Query<int>("select count(1) from dbo.Messages where Id = @Id", new { Id = msgId }).Single();
            Trace.Assert(count == 0);
        }

        private static IEnumerable<dynamic> QuerySP(SqlConnection connection)
        {
            Console.WriteLine("QuerySP");

            connection.Execute(
                "create procedure dbo.pr_Test(@Id as bigint) as begin select * from dbo.Namespaces where ID = @Id end");
            try
            {

                return connection.Query("pr_test", new {Id = 2}, commandType: CommandType.StoredProcedure);
            }
            finally
            {
                connection.Execute("drop procedure dbo.pr_Test");
            }
        }

        private static IEnumerable<dynamic> QueryWithDynamic(SqlConnection connection)
        {
            Console.WriteLine("QueryWithDynamic");

            var res = connection.Query(
                "select top(10) s.Id, s.Name, t.Id, t.Name, n.Id, n.Name " +
                "from dbo.Subprograms s " +
                "   join dbo.Types t on s.ContainingTypeId = t.Id " +
                "   join dbo.Namespaces n on t.NamespaceId = n.Id " +
                "where n.UniqueId = @Id " +
                "order by s.Id",
                new { Id = 2 });

            return res;
        }

        private static IEnumerable<Subprogram> QueryWithMap(SqlConnection connection)
        {
            Console.WriteLine("QueryWithMap");

            var res = connection.Query<Subprogram, Type, Namespace, Subprogram>(
                "select top(10) s.Id, s.Name, t.Id, t.Name, n.Id, n.Name " +
                "from dbo.Subprograms s " +
                "   join dbo.Types t on s.ContainingTypeId = t.Id " +
                "   join dbo.Namespaces n on t.NamespaceId = n.Id " +
                "where n.UniqueId = @Id " +
                "order by s.Id",
                param: new {Id = 2},
                map: (subprogram, type, ns) =>
                    {
                        subprogram.Type = type;
                        type.Subprogram = subprogram;
                        type.Namespace = ns;
                        ns.Type = type;
                        return subprogram;
                    });

            return res;
        }

        private static IEnumerable<Namespace> OneToManyQuery(SqlConnection connection)
        {
            Console.WriteLine("OneToManyQuery");

            var dict = new Dictionary<int, Namespace>();

            var res = connection.Query<Type, Namespace, Namespace>(
                "select t.Id, t.Name, n.Id, n.Name " +
                "from dbo.Namespaces n " +
                "   join dbo.Types t on t.NamespaceId = n.Id " +
                "where n.UniqueId = @Id ",
                param: new { Id = 2 },
                map: (type, ns) =>
                {
                    Namespace oldNamespace;
                    if(!dict.TryGetValue(ns.Id, out oldNamespace))
                    {
                        oldNamespace = ns;
                        dict[ns.Id] = ns;
                    }

                    oldNamespace.Types.Add(type);

                    return oldNamespace;
                });

            return dict.Values;
        }

        private static IEnumerable<Subprogram> QueryWithMapFluent(SqlConnection connection)
        {
            Console.WriteLine("QueryWithMapFluent");

            IDbManager dbManager = new DbManager(connection);

            var res = dbManager.SetCommand(
                "select top(10) s.Id, s.Name, t.Id, t.Name, n.Id, n.Name " +
                "from dbo.Subprograms s " +
                "   join dbo.Types t on s.ContainingTypeId = t.Id " +
                "   join dbo.Namespaces n on t.NamespaceId = n.Id " +
                "where n.UniqueId = @Id " +
                "order by s.Id").
                                SetParameters(new {Id = 2}).
                                ExecuteMapping<Subprogram, Type, Namespace, Subprogram>((subprogram, type, ns) =>
                                    {
                                        subprogram.Type = type;
                                        type.Subprogram = subprogram;
                                        type.Namespace = ns;
                                        ns.Type = type;
                                        return subprogram;
                                    }, splitOn: "Id");

            return res;

        }

        private static Namespace QueryMultiple(SqlConnection connection)
        {
            Console.WriteLine("QueryMultiple");

            Namespace res;
            using (var reader = connection.QueryMultiple(
                "select n.Id, n.Name from dbo.Namespaces n where n.UniqueId = @Id; select t.Id, t.Name from dbo.Types t where t.NamespaceUniqueId = @Id;  ",
                new { Id = 2 }))
            {
                res = reader.Read<Namespace>().Single();
                res.Types = reader.Read<Type>().ToList();

                return res;
            }
        }
    }
}
