using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CalculatorRESTService.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class CalculatorController : ControllerBase
    {
        
        [HttpPost("add")]
        public Result Add(Numbers numbers)
        {
            return new Result() { Value = numbers.Number1 + numbers.Number2 };
        }
        
        [HttpPost]
        public Result Calculate(Operation operation)
        {
            if (operation.Operator.ToString().Equals(Operator.Add.ToString(), StringComparison.InvariantCultureIgnoreCase))
            {
                return new Result() { Value = operation.Num1 + operation.Num2 };
            }
            if (operation.Operator.ToString().Equals(Operator.Subtract.ToString(), StringComparison.InvariantCultureIgnoreCase))
            {
                return new Result() { Value = operation.Num1 - operation.Num2 };
            }

            throw new ArgumentException("No operation");
        }

        public class Operation
        {
            public Operator Operator { get; set; }
            public double Num1 { get; set; }
            public double Num2 { get; set; }
        }
        
        public enum Operator
        {
            Add, Subtract
        }

        public class Result
        {
            public double Value { get; set; }
        }
    }

    public class Numbers
    {
        public double Number1 { get; set; }
        public double Number2 { get; set; }
    }
}
