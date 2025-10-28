using System;
namespace TestDISC.Models.ResultDISC
{
    public class MappingOnTable
    {
        public int D { get; set; }
        public int I { get; set; }
        public int S { get; set; }
        public int C { get; set; }

        public MappingOnTable(int D, int I, int S, int C)
        {
            this.D = D;
            this.I = I;
            this.S = S;
            this.C = C;
        }
    }
}
