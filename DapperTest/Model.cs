using System;
using System.Collections.Generic;
using Dapper;

namespace DapperTest
{
    public class Subprogram
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Type Type { get; set; }
    }

    public class Type
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Subprogram Subprogram { get; set; }
        public Namespace Namespace { get; set; }
    }

    public class Namespace
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Type Type { get; set; }
        public List<Type> Types { get; set; }

        public Namespace()
        {
            Types = new List<Type>();
        }
    }

    [Table("Messages")]
    public class Message
    {
        public int Id { get; set; }

        public string Msg { get; set; }

        public DateTime DateTime { get; set; }

        public int ExecutionTraceId { get; set; }
    }
}
