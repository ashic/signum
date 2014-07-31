using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace signum.tests
{
    public class Bootstrapper
    {
        public void Boot()
        {
            //var sec = new Sec()
            //    .RegisterLocalService("email", x => Console.WriteLine("Executing Email Step"))
            //    .RegisterLocalService("fraud", x => Console.WriteLine("Executing Fraud Step"))
            //    .RegisterLocalService("vigil", x => Console.WriteLine("Executing Vigil Step"));


            //var context = sec.RegisterSaga("start => email => fraud => vigil")
            //    .ServiceMap("email", "email")
            //    .ServiceData("fraud", "")
            //    .SharedData("orderId", "204HD")
            //    .Build();

            var sec = Sec.Initialise()
                .RegisterServices(x =>
                {
                    x["email"] = new LocalServiceExecutor(
                        y => Console.WriteLine("ExecutingEmailStep"),
                        y => Console.WriteLine("Compensating ExecutingEmailStep")
                        );
                    x["fraud"] = new LocalServiceExecutor(
                        y => Console.WriteLine("ExecutingFraudStep"),
                        y => Console.WriteLine("Compensating ExecutingFraudStep"));
                    x["vigil"] = new LocalServiceExecutor(
                        y => Console.WriteLine("ExecutingVigilStep"),
                        y => Console.WriteLine("Compensating ExecutingVigilStep"));
                })
                .RegisterContextFactory(x =>
                {
                    x.Name = "OrderPlaced";
                    x.Definition = "a => b => c";
                    x.ServiceMappings["a"] = "email";
                    x.ServiceMappings["b"] = "fraud";
                    x.ServiceMappings["c"] = "vigil";
                })
                .Build();


            var context = sec.CreateContext(x =>
            {
                x.Name = "OrderPlaced";
                x.Data["email"] = "blah";
                x.Data["fraud"] = "meh";
            });

            context.Execute();

        }

    }

}
