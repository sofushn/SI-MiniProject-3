using System;
using System.Threading.Tasks;

namespace LoanUi.Services
{
    interface ILoanService
    {
        void RequestNewLoan(Guid customerId);
    }
}