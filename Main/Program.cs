// See https://aka.ms/new-console-template for more information

using System.Net;
using FpMath;
static bool CheckMulOverflow(long a, long b)
{
    if (a == 0 || b == 0) return false;
    if (a == (a * b) / b) return false;
    return true;
}

static bool CheckLeftShitOverflow(ulong a, int shift, bool negative)
{
    Log.V(Convert.ToString((long)a, 2));
    var leading = 0;
    if (negative)
    {
        while ((a & 0x8000000000000000UL) == 0x8000000000000000UL)
        {
            a <<= 1;
            leading++;
        }
    }
    else
    {
        while ((a & 0x8000000000000000UL) == 0)
        {
            a <<= 1;
            leading++;
        }
    }

    leading -= 1;
    Log.V($"leading {leading}");

    return leading < shift;
}

Console.WriteLine("Hello, World!");

Log.V(long.MaxValue);
var a     = -15;
var count = int.Parse(Console.ReadLine());
var ret   = CheckLeftShitOverflow((ulong)a, count, a < 0);
var c     = a << count;
Log.V($"Overflow: {ret}");
Log.V(c);