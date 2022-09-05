using System.Diagnostics;

namespace FpMath;

public class Log
{
    [Conditional("DEBUG")]
    public static void V(object o)
    {
        Console.WriteLine(o);
    }
    
    [Conditional("DEBUG")]
    public static void E(object o)
    {
        Console.WriteLine(o);
    }
}