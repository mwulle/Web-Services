using Grpc.Core;

namespace CalculatorService.Services;

public class CalcService : Calculator.CalculatorBase
{
    private readonly ILogger<CalcService> _logger;

    public CalcService(ILogger<CalcService> logger)
    {
        _logger = logger;
    }
    
    public override Task<SumReply> Add(AddRequest request, ServerCallContext context)
    {
        return Task.FromResult(new SumReply
        {
            Sum = request.Number1 + request.Number1
        });
    }

}