using System.Diagnostics;

namespace FpMath;

public struct Fp64
{
    private const int  SHIFT     = 32;
    private const long ONE       = 1L << SHIFT;
    const         int  MAX_INPUT = 2147483647;

    public static readonly Fp64 MaxValue = new Fp64(MAX_INPUT);


    private long _raw;

    public Fp64(long raw)
    {
        _raw = raw;
    }

    [Conditional("DEBUG")]
    static void AssertOverflow(bool value)
    {
        if (value) Log.E("Input overflow");
    }

    static bool CheckAddOverflow(long a, long b)
    {
        if (a < 0 && b < 0 && a + b > 0) return true;
        if (a > 0 && b > 0 && a + b < 0) return true;
        return false;
    }

    static bool CheckSubtractOverflow(long a, long b)
    {
        if (a > 0 && b < 0 && a - b < 0) return true;
        if (a < 0 && b > 0 && a - b > 0) return true;
        return false;
    }

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

    public static Fp64 operator +(Fp64 a, Fp64 b)
    {
        AssertOverflow(CheckAddOverflow(a._raw, a._raw));
        return new Fp64(a._raw + b._raw);
    }

    public static Fp64 operator -(Fp64 a, Fp64 b)
    {
        AssertOverflow(CheckSubtractOverflow(a._raw, b._raw));
        return new Fp64(a._raw - b._raw);
    }

    public static Fp64 operator *(Fp64 a, Fp64 b)
    {
        var ahi = a._raw >> SHIFT;
        var bhi = b._raw >> SHIFT;
        var alo = (ulong)(a._raw & 0x00000000FFFFFFFF);
        var blo = (ulong)(b._raw & 0x00000000FFFFFFFF);

        AssertOverflow(CheckMulOverflow((long)alo, (long)blo));

        var lolo = (long)((alo * blo) >> SHIFT);

        AssertOverflow(CheckMulOverflow(ahi, (long)blo));

        var hilo = ahi * (long)blo;

        AssertOverflow(CheckMulOverflow((long)alo, bhi));
        var lohi = (long)alo * bhi;

        AssertOverflow(CheckMulOverflow(ahi, bhi));
        AssertOverflow(CheckLeftShitOverflow(ahi * bhi, SHIFT));
        var hihi = (ahi * bhi) << SHIFT;

        var sum = lolo;

        AssertOverflow(CheckAddOverflow(sum, hilo));
        sum += hilo;
        AssertOverflow(CheckAddOverflow(sum, lohi));
        sum += lohi;
        AssertOverflow(CheckAddOverflow(sum, hihi));
        sum += hihi;

        return new Fp64(sum);
    }

    public static explicit operator Fp64(int i)
    {
        return new Fp64(i * ONE);
    }

    public static explicit operator Fp64(long l)
    {
        AssertOverflow(l < MAX_INPUT);
        return new Fp64(l * ONE);
    }

    public static explicit operator Fp64(float f)
    {
        AssertOverflow(f < MAX_INPUT);
        return new Fp64((long)(f * ONE));
    }

    public static explicit operator Fp64(double d)
    {
        AssertOverflow(d < MAX_INPUT);
        return new Fp64((long)(d * ONE));
    }

    public static explicit operator int(Fp64 fp)
    {
        return (int)(fp._raw >> SHIFT);
    }

    public static explicit operator long(Fp64 fp)
    {
        return (long)(fp._raw >> SHIFT);
    }

    public static explicit operator float(Fp64 fp)
    {
        return (float)(fp._raw / (double)ONE);
    }

    public static explicit operator double(Fp64 fp)
    {
        return (double)fp._raw / (double)ONE;
    }
}