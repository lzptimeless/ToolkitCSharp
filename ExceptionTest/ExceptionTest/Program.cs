using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ExceptionTest
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                MethodA();
            }
            catch (Exception ex)
            {
                CreateExceptionShortDescription(ex, 0);
            }
        }

        static string CreateExceptionShortDescription(Exception ex, int indentCount)
        {
            StringBuilder sb = new StringBuilder();
            StackTrace stackTrace = new StackTrace(ex, false);
            StackFrame[] stackFrames = stackTrace.GetFrames();
            sb.AppendLine($"{CreateIndent(indentCount)}{ex.GetType().Name}:{ex.Message}");
            if (ex.InnerException != null)
            {
                string innerException = CreateExceptionShortDescription(ex.InnerException, indentCount + 1);
                sb.Append(innerException);
            }
            foreach (var frame in stackFrames)
            {
                MethodBase methodBase = frame.GetMethod();
                sb.AppendLine($"{CreateIndent(indentCount)}{methodBase.DeclaringType.Name}.{methodBase.Name}");
            }
            return sb.ToString();
        }

        static string CreateIndent(int indentCount)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < indentCount; i++)
            {
                sb.Append('\t');
            }
            return sb.ToString();
        }

        static void MethodA()
        {
            Console.Write("A");
            MethodB("1", "2");
        }

        static void MethodB(string p1, string p2)
        {
            Console.Write("B");
            MethodC("1");
        }

        static void MethodC(string p1)
        {
            Console.Write("C");
            try
            {
                MethodD();
            }
            catch (Exception ex)
            {
                throw new Exception("Wrapper", ex);
            }
        }

        static void MethodD()
        {
            Console.Write("D");
            MethodE();
        }

        static void MethodE()
        {
            Console.Write("E");
            MethodF();
        }

        static void MethodF()
        {
            Console.Write("F");
            throw new Exception("Test exception");
        }
    }
}
