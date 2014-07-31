using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace signum
{
    internal class ProgramSample
    {
        public static void Main(string[] args)
        {
            var sec = Sec.Initialise()
                .RegisterServices(x =>
                {
                    x["email"] = new LocalServiceExecutor(
                        y =>
                        {
                            Console.WriteLine("ExecutingEmailStep");
                            y.Data["foo"] = "bar";
                        },
                        y => Console.WriteLine("Compensating ExecutingEmailStep")
                        );
                    x["fraud"] = new LocalServiceExecutor(
                        y =>  Console.WriteLine("ExecutingFraudStep " + y.Data["foo"]),
                        y => Console.WriteLine("Compensating ExecutingFraudStep"));
                    x["vigil"] = new LocalServiceExecutor(
                        y => Console.WriteLine("ExecutingVigilStep"),
                        y => Console.WriteLine("Compensating ExecutingVigilStep"));
                    x["fraud2"] = new LocalServiceExecutor(
                        y =>
                        {
                            Console.WriteLine("Executing fraud 2. Going to fail this.");
                            throw new ApplicationException("lala");
                        },
                        y =>
                        {
                            Console.WriteLine("Compensating fraud 2");
                            throw new MarshalDirectiveException();
                        });
                })
                .RegisterContextFactory(x =>
                {
                    x.Name = "OrderPlaced";
                    x.Definition = "a => b => c";
                    x.ServiceMappings["a"] = "email";
                    x.ServiceMappings["b"] = "fraud2";
                    x.ServiceMappings["c"] = "vigil";
                })
                .Build();


            var context = sec.CreateContext(x =>
            {
                x.Name = "OrderPlaced";
                x.Data["email"] = "blah";
                x.Data["fraud"] = "meh";
            });

            for (int i = 0; i < 1; i++)
            {
                context.Execute();
            }
        }
    }
}
