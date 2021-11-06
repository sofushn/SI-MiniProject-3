using System;
using System.Threading;
using BankService.Models;

namespace BankService.Integrations
{
    public interface ILoanRequestConsumer: IDisposable
    {
        LoanRequest WaitForRequest(CancellationToken ct);
    }
}