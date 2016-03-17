using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.XPath;
using MK.Utilities;

namespace LinqTo
{
    public class LinqToXML
    {
        public void Test()
        {
            Console.WriteLine("******************** LINQ to XML ********************");

            var xml =
                new XElement("root",
                             new XElement("parent", new XAttribute("mother", "Kasia"),
                                          new XAttribute("father", "Wojtek"),
                                          new XElement("child", new XAttribute("name", "Ala")),
                                          new XElement("child", new XAttribute("name", "Maciek"))),
                             new XElement("parent", new XAttribute("mother", "Ola"), new XAttribute("father", "Tomek"),
                                          new XElement("child", new XAttribute("name", "Ala")),
                                          new XElement("child", new XAttribute("name", "Maciek")),
                                          new XElement("child", new XAttribute("name", "Tomek")),
                                          new XElement("child", new XAttribute("name", "Piotrek"))));

            Console.WriteLine(xml);

            Console.WriteLine("xml.DescendantNodes()");
            Console.WriteLine(xml.DescendantNodes());


            Console.WriteLine("xml.Element(\"parent\")");
            Console.WriteLine(xml.Element("parent"));

            Console.WriteLine("xml.Element(\"parent\").FirstAttribute");
            Console.WriteLine(xml.Element("parent").FirstAttribute);

            Console.WriteLine("xml.Elements(\"parent\").Last().RemoveAttributes()");
            xml.Elements("parent").Last().RemoveAttributes();
            Console.WriteLine(xml);

            Console.WriteLine("xml.Descendants(\"child\").Count()");
            Console.WriteLine(xml.Descendants("child").Count());

            Console.WriteLine(" xml.Elements(\"parent\").Last().AddFirst(new XElement(\"child\", new XAttribute(\"name\", \"Ala2\")))");
            xml.Elements("parent").Last().AddFirst(new XElement("child", new XAttribute("name", "Ala2")));
            Console.WriteLine(xml);

            Console.WriteLine("xml.XPathSelectElement(\"//child[@name=\"Tomek\"]\")");
            Console.WriteLine(xml.XPathSelectElement("//child[@name=\"Tomek\"]"));

            Console.WriteLine("xml.XPathEvaluate(\"//child[@name=\"Tomek\"]\")");
            var evaluator = xml.XPathEvaluate("//child[@name=\"Tomek\"]") as IEnumerable;

            foreach (var x in evaluator)
                Console.WriteLine(x);

            Console.WriteLine("LINQ");

            var res = from n in xml.Elements()
                      where n.Descendants().Count() > 2
                      select n;

            foreach (var x in res)
                Console.WriteLine(x);


        }
    }
}
