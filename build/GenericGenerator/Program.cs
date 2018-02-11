using System.IO;
using System.Linq;
using System.Text;

namespace GenericGenerator
{
    class Program
    {
        static void Main(string[] args)
        {
            foreach (var argument in args)
            {
                GenerateGenericTypes(argument, 8);
            }
        }

        private static void GenerateGenericTypes(string file, int count)
        {
            // 读取原始文件并创建泛型代码生成器。
            var template = File.ReadAllText(file, Encoding.UTF8);
            var generator = new GenericTypeGenerator(template);

            // 根据泛型个数生成目标文件路径和文件内容。
            var format = GetIndexedFileNameFormat(file);
            (string targetFileName, string targetFileContent)[] contents = Enumerable.Range(2, count - 1).Select(i =>
                (string.Format(format, i.ToString().PadLeft(2, '0')), generator.Generate(i))
            ).ToArray();

            // 写入目标文件。
            foreach (var writer in contents)
            {
                File.WriteAllText(writer.targetFileName, writer.targetFileContent);
            }
        }

        private static string GetIndexedFileNameFormat(string fileName)
        {
            var directory = Path.GetDirectoryName(fileName);
            var name = Path.GetFileNameWithoutExtension(fileName);
            if (name.EndsWith(".01"))
            {
                name = name.Substring(0, name.Length - 3);
            }
            if (name.EndsWith("1"))
            {
                name = name.Substring(0, name.Length - 1);
            }

            return Path.Combine(directory, name + ".{0}.cs");
        }
    }
}
