using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;

namespace Automapper
{
    class Program
    {
        private static void Main(string[] args)
        {
            var src1 = new Source1()
            {
                Id = 1,
                Name = "Ala ma kota",
                Inner = new InnerSource1 { Name = "Kot ma Alę" }
            };
            src1.Add(1);
            src1.Add(10);
            src1.Add(13);

            var sourceWithNestedMembers1 = new SourceWithNestedMembers1()
            {
                Id = 1,
                Name = "Ala ma kota",
                Inner = new InnerSource1 { Name = "Kot ma Alę" }
            };

            //Simple mapping 
            var map = Mapper.CreateMap<Source1, Target1>();
            var t1 = Mapper.Map<Target1>(src1);

            //Name2 doesn't have corresponding source property
            Mapper.Reset();
            var map2 = Mapper.CreateMap<Source1, Target2>();
            var t2 = Mapper.Map<Target2>(src1);
            Trace.Assert(t2.Name2 == null);

            try
            {
                Mapper.AssertConfigurationIsValid();
                Trace.Assert(false);
            }
            catch (Exception ex)
            {
            }

            //Usage of AfterMap to map Name2
            Mapper.Reset();
            map2 = Mapper.CreateMap<Source1, Target2>();
            map2.AfterMap((source, target2) => target2.Name2 = source.Name);
            t2 = Mapper.Map<Target2>(src1);
            Trace.Assert(t2.Name2 != null);

            //Usage of ForMember and MapFrom to map Name2
            Mapper.Reset();
            map2 = Mapper.CreateMap<Source1, Target2>();
            map2.ForMember(target2 => target2.Name2, expression => expression.MapFrom(source => source.Name));
            t2 = Mapper.Map<Target2>(src1);
            Trace.Assert(t2.Name2 != null);

            //Projection - Usage of ForMember and MapFrom to map Name2
            Mapper.Reset();
            map2 = Mapper.CreateMap<Source1, Target2>();
            map2.ForMember("Name2", expression => expression.MapFrom(source => source.Name));
            t2 = Mapper.Map<Target2>(src1);
            Trace.Assert(t2.Name2 != null);

            //Projection - Usage of ForMember and UseValue to map Name2
            Mapper.Reset();
            map2 = Mapper.CreateMap<Source1, Target2>();
            map2.ForMember(target2 => target2.Name2, expression => expression.UseValue("Dupa"));
            t2 = Mapper.Map<Target2>(src1);
            Trace.Assert(t2.Name2 == "Dupa");

            //Usage of ConstructUsing to map Name2
            Mapper.Reset();
            map2 = Mapper.CreateMap<Source1, Target2>();
            map2.ConstructUsing((Source1 source) => new Target2() { Name2 = "test" });
            t2 = Mapper.Map<Target2>(src1);
            Trace.Assert(t2.Name2 == "test");

            //Flattening
            Mapper.Reset();
            var map3 = Mapper.CreateMap<Source1, Target3>();
            var t3 = Mapper.Map<Target3>(src1);
            Trace.Assert(t3.Total == 24);

            //Flattening + Projection with ForMember + Ignore
            Mapper.Reset();
            map3 = Mapper.CreateMap<Source1, Target3>();
            map3.ForMember(target3 => target3.Total, expression => expression.Ignore());
            t3 = Mapper.Map<Target3>(src1);
            Trace.Assert(t3.Total == 0);

            //Collections + Conditional mapping
            Mapper.Reset();
            var map4 = Mapper.CreateMap<Source1, Target1>();
            map4.ForMember("Name", expression => expression.Condition(source1 => source1.Name != "bbb"));
            var sourceList = new List<Source1>
                {
                    new Source1() {Id = 1, Name = "aaa"},
                    new Source1() {Id = 2, Name = "bbb"},
                    new Source1() {Id = 3, Name = "ccc"}
                };
            var destArray = Mapper.Map<List<Source1>, Target1[]>(sourceList);
            Trace.Assert(destArray.Length == 3);
            Trace.Assert(destArray.Count(d => d.Id == 2 && d.Name == null) == 1);

            //Nested mapping + NullSubstitute
            Mapper.Reset();
            var map5 = Mapper.CreateMap<SourceWithNestedMembers1, TargetWithNestedMembers1>();
            map5.ForMember(members1 => members1.Name, expression => expression.NullSubstitute("NULL"));
            Mapper.CreateMap<InnerSource1, InnerTarget1>();

            var oldValue = sourceWithNestedMembers1.Name;
            sourceWithNestedMembers1.Name = null;
            var targetWithNestedMembers1 = Mapper.Map<TargetWithNestedMembers1>(sourceWithNestedMembers1);

            Trace.Assert(targetWithNestedMembers1.Inner != null);
            Trace.Assert(targetWithNestedMembers1.Name == "NULL");

            sourceWithNestedMembers1.Name = oldValue;

            //Nested mapping + ConvertUsing 
            Mapper.Reset();
            var map6 = Mapper.CreateMap<SourceWithNestedMembers1, TargetWithNestedMembers2>();
            map6.ConvertUsing(members1 => new TargetWithNestedMembers2
            {
                Id = members1.Id,
                Name = members1.Name,
                Inner = new InnerTarget2() { Name = members1.Inner.Name + "222" }
            });
            var targetWithNestedMembers2 = Mapper.Map<TargetWithNestedMembers2>(sourceWithNestedMembers1);
            Trace.Assert(targetWithNestedMembers2.Inner != null);
        }
    }
}
