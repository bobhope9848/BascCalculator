using System;
using System.Collections.Generic;
using System.Text;

namespace BascCalculator
{
    public class Operate
    {
        public string LeftSide { get; set; }
        public string RightSide { get; set; }

        public OpType OpType { get; set; }

        public OpType InnerOp { get; set; }

        public Operate()
        {
            this.LeftSide = "";
            this.RightSide = "";
        }
    }
}
