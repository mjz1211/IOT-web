using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cssample
{
    //規劃一個可以重複使用 具有分類雛形base
    public class Employees //就是既成Object
    {
        //Data Field 具有可以儲存資訊 
        private String _empid;
        private String _name;
        private String _address;
        private Decimal _salary;
        protected Decimal _actSalary;
        //編譯產生預測建構子Default Constructor
        //自訂建構子(往往需要物件建構起 有初始化資訊要定義)Constructor Injection
        public Employees(String _empid,String _name,String _address)
        {
            this._empid = _empid;
            this._name = _name;
            this._address = _address;
        }
        //Overloading
        public Employees() { }
        //自訂Properpy (setter and getter) 程序　使用=操作
        public String Empid
        {
            //xxx.property=value
            set {
                _empid = value;
                }
            //=xxx.property
            get {
                return _empid;
            }
        }
        #region 屬性
        /// <summary>
        /// 設定或者取得姓名
        /// </summary>
        public string Name { get => _name; set => _name = value; }
        public string Address { get => _address; set => _address = value; }
        public decimal Salary {
            set {
                if(value>30000)
                {
                    _salary = value;
                }else
                {
                    _salary = 30000;
                }
            }
            get {
                return _salary;
            }
        }

        //Read only Property
        /// <summary>
        /// 取得實際薪資
        /// </summary>
        public decimal ActSalary { get => _actSalary;  }
      
        #endregion

        //Method 薪資核算月薪制
        public virtual void calSalary(Int32 days)
        {
            if(days>0)
            {
                _actSalary=(days / 22.0M) * _salary;
            }

        }
        //Method 薪資核算 PT-Parttime Overloading 
        public void calSalary(Decimal hoursSalary ,Int32 hours)
        {
            _actSalary = hours * hoursSalary;
        }


    }
}
