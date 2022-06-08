using LinqTutorials.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LinqTutorials
{
    public static class LinqTasks
    {
        public static IEnumerable<Emp> Emps { get; set; }
        public static IEnumerable<Dept> Depts { get; set; }

        static LinqTasks()
        {
            var empsCol = new List<Emp>();
            var deptsCol = new List<Dept>();

            #region Load depts

            var d1 = new Dept
            {
                Deptno = 1,
                Dname = "Research",
                Loc = "Warsaw"
            };

            var d2 = new Dept
            {
                Deptno = 2,
                Dname = "Human Resources",
                Loc = "New York"
            };

            var d3 = new Dept
            {
                Deptno = 3,
                Dname = "IT",
                Loc = "Los Angeles"
            };

            deptsCol.Add(d1);
            deptsCol.Add(d2);
            deptsCol.Add(d3);
            Depts = deptsCol;

            #endregion

            #region Load emps

            var e1 = new Emp
            {
                Deptno = 1,
                Empno = 1,
                Ename = "Jan Kowalski",
                HireDate = DateTime.Now.AddMonths(-5),
                Job = "Backend programmer",
                Mgr = null,
                Salary = 2000
            };

            var e2 = new Emp
            {
                Deptno = 1,
                Empno = 20,
                Ename = "Anna Malewska",
                HireDate = DateTime.Now.AddMonths(-7),
                Job = "Frontend programmer",
                Mgr = e1,
                Salary = 4000
            };

            var e3 = new Emp
            {
                Deptno = 1,
                Empno = 2,
                Ename = "Marcin Korewski",
                HireDate = DateTime.Now.AddMonths(-3),
                Job = "Frontend programmer",
                Mgr = null,
                Salary = 5000
            };

            var e4 = new Emp
            {
                Deptno = 2,
                Empno = 3,
                Ename = "Paweł Latowski",
                HireDate = DateTime.Now.AddMonths(-2),
                Job = "Frontend programmer",
                Mgr = e2,
                Salary = 5500
            };

            var e5 = new Emp
            {
                Deptno = 2,
                Empno = 4,
                Ename = "Michał Kowalski",
                HireDate = DateTime.Now.AddMonths(-2),
                Job = "Backend programmer",
                Mgr = e2,
                Salary = 5500
            };

            var e6 = new Emp
            {
                Deptno = 2,
                Empno = 5,
                Ename = "Katarzyna Malewska",
                HireDate = DateTime.Now.AddMonths(-3),
                Job = "Manager",
                Mgr = null,
                Salary = 8000
            };

            var e7 = new Emp
            {
                Deptno = null,
                Empno = 6,
                Ename = "Andrzej Kwiatkowski",
                HireDate = DateTime.Now.AddMonths(-3),
                Job = "System administrator",
                Mgr = null,
                Salary = 7500
            };

            var e8 = new Emp
            {
                Deptno = 2,
                Empno = 7,
                Ename = "Marcin Polewski",
                HireDate = DateTime.Now.AddMonths(-3),
                Job = "Mobile developer",
                Mgr = null,
                Salary = 4000
            };

            var e9 = new Emp
            {
                Deptno = 2,
                Empno = 8,
                Ename = "Władysław Torzewski",
                HireDate = DateTime.Now.AddMonths(-9),
                Job = "CTO",
                Mgr = null,
                Salary = 12000
            };

            var e10 = new Emp
            {
                Deptno = 2,
                Empno = 9,
                Ename = "Andrzej Dalewski",
                HireDate = DateTime.Now.AddMonths(-4),
                Job = "Database administrator",
                Mgr = null,
                Salary = 9000
            };

            empsCol.Add(e1);
            empsCol.Add(e2);
            empsCol.Add(e3);
            empsCol.Add(e4);
            empsCol.Add(e5);
            empsCol.Add(e6);
            empsCol.Add(e7);
            empsCol.Add(e8);
            empsCol.Add(e9);
            empsCol.Add(e10);
            Emps = empsCol;

            #endregion
        }

        /// <summary>
        ///     SELECT * FROM Emps WHERE Job = "Backend programmer";
        /// </summary>
        public static IEnumerable<Emp> Task1()
        {
            //query syntax
            var query = from e in Emps
                        where e.Job.Contains("Backend programmer")
                        select e;
            //method syntax
            var methodSyntax = Emps.Where(e => e.Job.Contains("Backend programmer"));
            return methodSyntax;
        }

        /// <summary>
        ///     SELECT * FROM Emps Job = "Frontend programmer" AND Salary>1000 ORDER BY Ename DESC;
        /// </summary>
        public static IEnumerable<Emp> Task2()
        {
            //query syntax
            var query = from e in Emps
                        where e.Job.Contains("Frontend programmer") && e.Salary > 1000
                        select e;
            //method syntax
            var methodSyntax = Emps.Where(e => e.Job.Contains("Frontend programmer") && e.Salary > 1000);
            return methodSyntax;
        }


        /// <summary>
        ///     SELECT MAX(Salary) FROM Emps;
        /// </summary>
        public static int Task3()
        {
            //method syntax
            var methodSyntax = Emps.Max(e => e.Salary);
            return methodSyntax;
        }

        /// <summary>
        ///     SELECT * FROM Emps WHERE Salary=(SELECT MAX(Salary) FROM Emps);
        /// </summary>
        public static IEnumerable<Emp> Task4()
        {
            var max = Emps.Max(e => e.Salary);
            var query = from e in Emps
                        where e.Salary == max
                        select e;
            return query;
        }

        /// <summary>
        ///    SELECT ename AS Nazwisko, job AS Praca FROM Emps;
        /// </summary>
        public static IEnumerable<object> Task5()
        {
            var query = from e in Emps
                        select new { Nazwisko = e.Ename, Praca = e.Job };
            var methodSyntax = Emps
                        .Select(e => new { Nazwisko = e.Ename, Praca = e.Job });
            return query;
        }

        /// <summary>
        ///     SELECT Emps.Ename, Emps.Job, Depts.Dname FROM Emps
        ///     INNER JOIN Depts ON Emps.Deptno=Depts.Deptno
        ///     Rezultat: Złączenie kolekcji Emps i Depts.
        /// </summary>
        public static IEnumerable<object> Task6()
        {
            var query = from e in Emps
                        join d in Depts
                        on e.Deptno equals d.Deptno
                        select new { e.Ename, e.Job, d.Dname };

            var methodSyntax = Emps.Join(
                    Depts,
                    e => e.Deptno,
                    d => d.Deptno,
                    (e, d) => new { e.Ename, e.Job, d.Dname }
                );
            return query;
        }

        /// <summary>
        ///     SELECT Job AS Praca, COUNT(1) LiczbaPracownikow FROM Emps GROUP BY Job;
        /// </summary>
        public static IEnumerable<object> Task7()
        {
            var query = from e in Emps
                        group e.Job by e.Job
                        into g
                        select new { Praca = g.Key, LiczbaPracownikow = g.Count() };
            var methodSyntax = Emps.GroupBy(e => e.Job)
                                   .Select(g => new { Praca = g.Key, LiczbaPracownikow = g.Count() });
            return query;
        }

        /// <summary>
        ///     Zwróć wartość "true" jeśli choć jeden
        ///     z elementów kolekcji pracuje jako "Backend programmer".
        /// </summary>
        public static bool Task8()
        {
            var methodSyntax = Emps.Any(e => e.Job.Equals("Backend programmer"));
            return methodSyntax;
        }

        /// <summary>
        ///     SELECT TOP 1 * FROM Emp WHERE Job="Frontend programmer"
        ///     ORDER BY HireDate DESC;
        /// </summary>
        public static Emp Task9()
        {
            var methodSyntax = Emps.Where(e => e.Job.Equals("Frontend programmer")).First();
            return methodSyntax;
        }

        /// <summary>
        ///     SELECT Ename, Job, Hiredate FROM Emps
        ///     UNION
        ///     SELECT "Brak wartości", null, null;
        /// </summary>
        public static IEnumerable<object> Task10()
        {
            var methodSyntax = Emps
              .Select(e => new { e.Ename, Job = (string?)e.Job, HireDate = e.HireDate })
              .Union(Emps
                    .Select(e => new { Ename = "Brak wartości", Job = (string?)null, HireDate = (DateTime?)null }));
            return methodSyntax;
        }

        /// <summary>
        /// Wykorzystując LINQ pobierz pracowników podzielony na departamenty pamiętając, że:
        /// 1. Interesują nas tylko departamenty z liczbą pracowników powyżej 1
        /// 2. Chcemy zwrócić listę obiektów o następującej srukturze:
        ///    [
        ///      {name: "RESEARCH", numOfEmployees: 3},
        ///      {name: "SALES", numOfEmployees: 5},
        ///      ...
        ///    ]
        /// 3. Wykorzystaj typy anonimowe
        /// </summary>
        public static IEnumerable<object> Task11()
        {
            var methodSyntax = Emps
                         .Join(
                                Depts,
                                    e => e.Deptno,
                                    d => d.Deptno,
                                  (e, d) => new { Dname = d.Dname, e.Deptno })
                         .GroupBy(e => e.Dname)
                         .Where(g => g.Count() > 1)
                         .Select(g => new { name = g.Key, numOfEmployees = g.Count() });
            return methodSyntax;
        }

        /// <summary>
        /// Napisz własną metodę rozszerzeń, która pozwoli skompilować się poniższemu fragmentowi kodu.
        /// Metodę dodaj do klasy CustomExtensionMethods, która zdefiniowana jest poniżej.
        /// 
        /// Metoda powinna zwrócić tylko tych pracowników, którzy mają min. 1 bezpośredniego podwładnego.
        /// Pracownicy powinny w ramach kolekcji być posortowani po nazwisku (rosnąco) i pensji (malejąco).
        /// </summary>
        public static IEnumerable<Emp> Task12()
        {
            return Emps.GetEmpsWithSubordinates();
        }

        /// <summary>
        /// Poniższa metoda powinna zwracać pojedyczną liczbę int.
        /// Na wejściu przyjmujemy listę liczb całkowitych.
        /// Spróbuj z pomocą LINQ'a odnaleźć tę liczbę, które występuja w tablicy int'ów nieparzystą liczbę razy.
        /// Zakładamy, że zawsze będzie jedna taka liczba.
        /// Np: {1,1,1,1,1,1,10,1,1,1,1} => 10
        /// </summary>
        public static int Task13(int[] arr)
        {
            return arr.GroupBy(item => item).Where(item => item.Count() % 2 == 1).Select(i => i.Key).First();
        }

        /// <summary>
        /// Zwróć tylko te departamenty, które mają 5 pracowników lub nie mają pracowników w ogóle.
        /// Posortuj rezultat po nazwie departament rosnąco.
        /// </summary>
        public static IEnumerable<Dept> Task14()
        {
            var methodSyntax = Depts
                               .GroupJoin(Emps, dept => dept.Deptno, emp => emp.Deptno, (dept, emp) => new { Dept = dept, Emp = emp })
                               .Where(x => x.Emp.Count() == 0 || x.Emp.Count() == 5)
                               .OrderBy(x => x.Dept.Dname)
                               .Select(x => x.Dept)
                               .ToList();


            var querySyntax = from d in Depts
                              let empCount = (from e in Emps
                                              where e.Deptno == d.Deptno
                                              select e).Count()
                              where empCount == 0 || empCount == 5
                              orderby d.Dname
                              select d;

            return methodSyntax;
        }
    }

    public static class CustomExtensionMethods
    {
        //Put your extension methods here
        public static IEnumerable<Emp> GetEmpsWithSubordinates(this IEnumerable<Emp> emps)
        {
            return emps.Where(emp => emps
                .Where(e => e.Mgr != null)
                .Select(e => e.Mgr)
                .Contains(emp))
                .OrderBy(e => e.Ename)
                .ThenByDescending(e => e.Salary)
                .Select(e => e);
        }

    }
}
