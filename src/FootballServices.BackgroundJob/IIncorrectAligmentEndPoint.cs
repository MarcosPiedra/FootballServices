using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FootballServices.BackgroundJob
{
    public interface IIncorrectAligmentEndPoint
    {
        Task Send(List<int> incorrectIds);
    }
}
