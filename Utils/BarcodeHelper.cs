using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Parking.Utils
{
    public static class BarcodeHelper
    {

        //public static String GenerateUniqueCodebar(String prefix = "TKT")
        //{
        //    var timeStamp = DateTime.UtcNow.ToString("yyyyMMddHHmmssfff");
        //    var random = Guid.NewGuid().ToString("N").Substring(0, 6);

        //    using SHA256 sha = SHA256.Create();

        //    var hashBytes = sha.ComputeHash(Encoding.UTF8.GetBytes(timeStamp + random));
        //    var hashShort = BitConverter.ToString(hashBytes).Replace("-", "").Substring(0, 6);

        //    return $"{prefix}-{timeStamp}-{hashShort}";
        //}


    }
}
