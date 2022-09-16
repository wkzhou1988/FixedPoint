// See https://aka.ms/new-console-template for more information

using System.Net;
using FpMath;

static bool CheckMulOverflow(long a, long b)
{
    if (a == 0 || b == 0) return false;
    if (a == (a * b) / b) return false;
    return true;
}

static bool CheckLeftShitOverflow(long a, int shift)
{
    Log.V("Long a: " + Convert.ToString((long)a, 2));
    var leading = 0;
    var b       = (ulong)(a < 0 ? ~a + 1 : a);

    Log.V("Long b: " + Convert.ToString((long)b, 2));

    while ((b & 0xF000000000000000) == 0)
    {
        leading +=  4;
        b       <<= 4;
    }

    while ((b & 0x8000000000000000) == 0)
    {
        leading +=  1;
        b       <<= 1;
    }

    return shift > leading - 1;
}

var a = (Fp64)int.MaxValue;
var b = a * a;
Log.V((long)b);