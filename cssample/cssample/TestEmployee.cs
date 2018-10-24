using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cssample
{
    class TestEmployee
    {
        //主程式
        public static void Main()
        {
            //定義一個區變數(Local Variable)
            Employees emp;
            //參照一個員工物件 (凡是類別就是參考型別)
            emp=new Employees();
            //操作物件化的成員
            emp.Empid = "0001"; //字串是物件　給位址
            emp.Address = "板橋區民族路";
            emp.Name = "張三豐";
            emp.Salary = 29999;

            Employees emp2;
            emp2 = emp; //assign address to emp2
            emp2.Salary = 50000;


            Employees emp3 = new Employees();
            //操作物件化的成員
            emp3.Empid = "0002"; //字串是物件　給位址
            emp3.Address = "板橋區民族路";
            emp3.Name = "張無際";
            emp3.Salary = 400000;
            //薪資核算
            emp.calSalary(22); //月薪算法
            emp3.calSalary(140, 100);


            Console.WriteLine(emp.ActSalary);
            Console.WriteLine(emp3.ActSalary);
        }
    }
}
