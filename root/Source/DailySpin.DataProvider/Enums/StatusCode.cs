using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DailySpin.DataProvider.Enums
{
    public enum StatusCode
    {
        UserNotFound = 0,
        UserAlreadyExists = 1,

        CarNotFound = 10,

        OrderNotFound = 20,

        OK = 200,
        InternalServerError = 500
    }
}