using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cssample
{
    public class Sales:Employees
    {
        //產生預設建構子() 裡面有一個鍊條 指向父親建構空參數
        private Decimal _qa;
        private Decimal _actQa;
        private Decimal _bon;

        public decimal Qa { get => _qa; set => _qa = value; }
        public decimal ActQa { get => _actQa; set => _actQa = value; }
        public decimal Bon { get => _bon;  }

        //核算獎金
        public void calBon()
        {
            if (_actQa >= _qa * 0.9M)
            {
                _bon=_actQa * 0.05M;
            }
        }
        //改寫繼承來的方法(Overiding)
        public override void calSalary(int days)
        {
            //呼叫父親既定的功能
            base.calSalary(days);
            //實際薪資加上獎金
            _actSalary += _bon;
        }
    }
}
