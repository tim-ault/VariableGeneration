using System;
using System.IO;
using System.Text;
using System.Xml;

namespace Variable_Generation
{
    class Program
    {
        static void Main(string[] args)
        {

            string[] FileList = Directory.GetFiles(".\\Resources");
            XmlDocument Document = new XmlDocument();
            //XmlNodeList NodeList = new XmlNodeList();

            StringBuilder SbOutput = new StringBuilder(1000);
            // Loop through all CommandInfo nodes in /Root.
            SbOutput.AppendLine("using System;");
            SbOutput.AppendLine("// SSP protocol constants.");
            SbOutput.AppendLine("class CSspConstants");
            SbOutput.AppendLine("{");


            foreach (string FilePath in FileList)
            {
                if (Path.GetExtension(FilePath).ToLower() == ".xml")
                {
                    Console.WriteLine(FilePath);
                    SbOutput.AppendLine();
                    SbOutput.Append("    // From ");
                    SbOutput.AppendLine(Path.GetFileName(FilePath));
                    Document.Load(FilePath);
                    XmlNodeList NodeList = Document.DocumentElement.SelectNodes(@"/Root/SSPData");
                    foreach (XmlNode Node in NodeList)
                    {
                        if (Node.SelectSingleNode("Name") != null)
                        {
                            SbOutput.Append("    public const byte ");
                            SbOutput.Append(Node.SelectSingleNode("Name").InnerText);
                            SbOutput.Append(" = 0x");
                            SbOutput.Append(Int32.Parse(Node.SelectSingleNode("Code").InnerText).ToString("X2"));

                            SbOutput.AppendLine(";");
                            Console.WriteLine(Node.SelectSingleNode("Name").InnerText);
                        }
                    }
                }

            }
            SbOutput.AppendLine("}");

            File.WriteAllText(".\\Resources\\CSSPConstants.cs", SbOutput.ToString());

        }
    }
}

