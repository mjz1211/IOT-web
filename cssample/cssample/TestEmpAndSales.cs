using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cssample
{
    class TestEmpAndSales
    {
        //Entry Point
        public  static void Main()
        {
            //建構員工物件
            Employees emp1 = new Employees("0001", "張三豐", "新北市板橋區");
            emp1.Salary = 60000;
            //物件初始化語法
            Employees emp2 = new Employees() {
                 Empid="0002",
                 Name="張無忌",
                 Address="高雄市",
                 Salary=50000
            };
            //建構業務員物件
            Sales sale1 = new Sales()
            {
                Empid = "003",
                Name = "張泰山",
                Address = "台北市",
                Salary = 30000,
                Qa = 100000
            };
            //核算薪資
            emp1.calSalary(22);
            //業務員核算獎金
            sale1.ActQa = 1000000;
            sale1.calBon();

            sale1.calSalary(22); //直接使用繼承來自於Employees Method
            Console.WriteLine($"員工薪水:{emp1.ActSalary}  業務員薪水:{sale1.ActSalary}");



            TestEmpAndSales.calSalaryTotal(emp1, 22);
            TestEmpAndSales.calSalaryTotal(sale1, 22); // 注意 sales 會呼叫 override 的方法

        }

        public static void calSalaryTotal(Employees emp, Int32 days)
        {
            emp.calSalary(days);
        }


    }
}
