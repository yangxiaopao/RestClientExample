using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Example
{
    class Program
    {
        static void Main(string[] args)
        {
            RestClient client = new RestClient("token");
            Result result = client.RunCMD(RestClient.MEMBER_URL, "cmd", "{}");
        }
    }
}
