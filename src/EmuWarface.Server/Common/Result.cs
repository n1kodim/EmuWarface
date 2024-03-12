namespace EmuWarface.Server.Common
{
    public readonly struct Result<TValue, EValue>
    {
        public TValue? Value { get; init; }
        public EValue? Error { get; init; }

        public Result(TValue? value = default, EValue? error = default)
        {
            Value = value;
            Error = error;
        }

        public static Result<TValue, EValue> Res(TValue v)
        {
            return new(v, default);
        }

        public static Result<TValue, EValue> Err(EValue e)
        {
            return new(default, e);
        }

        public static implicit operator Result<TValue, EValue>(TValue v) => new(v, default);
        public static implicit operator Result<TValue, EValue>(EValue e) => new(default, e);
    }
}
