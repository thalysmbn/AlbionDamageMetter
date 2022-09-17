﻿using System.Globalization;

namespace AlbionDamageMetter.Albion
{
    public readonly struct FixPoint
    {
        public const long InternalFactor = 10000L;
        public static readonly FixPoint One = new(InternalFactor);
        public double DoubleValue => (double)InternalValue / InternalFactor;
        public long IntegerValue => InternalValue / InternalFactor;

        public long InternalValue { get; }

        private FixPoint(long internalValue)
        {
            InternalValue = internalValue;
        }

        public static FixPoint FromInternalValue(long internalValue)
        {
            return new FixPoint(internalValue);
        }

        public static FixPoint FromFloatingPointValue(double value)
        {
            value = Math.Min(Math.Max(value, double.MinValue), double.MaxValue);
            return new FixPoint((long)Math.Round(value * InternalFactor));
        }

        public override string ToString()
        {
            return DoubleValue.ToString(CultureInfo.InvariantCulture);
        }

        public static FixPoint operator +(FixPoint a, FixPoint b)
        {
            return new FixPoint(a.InternalValue + b.InternalValue);
        }

        public static FixPoint operator -(FixPoint a, FixPoint b)
        {
            return new FixPoint(a.InternalValue - b.InternalValue);
        }

        public static FixPoint operator -(FixPoint a)
        {
            return new FixPoint(-a.InternalValue);
        }

        public static FixPoint operator *(FixPoint a, FixPoint b)
        {
            return new FixPoint(a.InternalValue * b.InternalValue / InternalFactor);
        }

        public static FixPoint operator *(FixPoint a, int b)
        {
            return new FixPoint(a.InternalValue * b);
        }

        public static FixPoint operator *(int b, FixPoint a)
        {
            return new FixPoint(a.InternalValue * b);
        }

        public static FixPoint operator *(FixPoint a, long b)
        {
            return new FixPoint(a.InternalValue * b);
        }

        public static FixPoint operator *(long b, FixPoint a)
        {
            return new FixPoint(a.InternalValue * b);
        }
    }
}
