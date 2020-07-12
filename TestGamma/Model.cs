using System;

namespace TestGamma
{
    public enum Actions
    {
        Buy,
        Sell,
        Wait
    }

    public class Model
    {
        public int Value { get; }
        public int Index { get; }
        public Actions Action { get; set; }
        public Model(int _value, int _index) { Value = _value; Index = _index; Action = Actions.Wait; }
    }
}
