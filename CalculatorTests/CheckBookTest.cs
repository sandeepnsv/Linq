﻿using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Calculator.CheckBook;
using System.Linq;
using System.Collections.ObjectModel;

namespace CalculatorTests
{
    [TestClass]
    public class CheckBookTest
    {
        [TestMethod]
        public void FillsUpProperly()
        {
            var ob = new CheckBookVM();

            Assert.IsNull(ob.Transactions);

            ob.Fill();

            Assert.AreEqual(12, ob.Transactions.Count);
        }

        [TestMethod]
        public void CountofEqualsMoshe()
        {
            var ob = new CheckBookVM();
            ob.Fill();

            var count = ob.Transactions.Where( t => t.Payee == "Moshe" ).Count();

            Assert.AreEqual(4, count);
        }

        [TestMethod]
        public void SumOfMoneySpentOnFood()
        {
            var ob = new CheckBookVM();
            ob.Fill();

            var category = "Food";

            var food = ob.Transactions.Where(t=> t.Tag == category );

            var total = food.Sum(t => t.Amount);

            Assert.AreEqual(261, total);

        }

        [TestMethod]
        public void Group()
        {
            var ob = new CheckBookVM();
            ob.Fill();

            var total = ob.Transactions.GroupBy(t => t.Tag).Select(g => new { g.Key, Sum=g.Sum( t=> t.Amount ) });

            Assert.AreEqual(261, total.First().Sum);
            Assert.AreEqual(300, total.Last().Sum);
        }

        //-------------------------------- One -------------------------------------------------
        [TestMethod]
        public void AvgAmountTag()
        {
            var ob = new CheckBookVM();
            ob.Fill();

            var total = ob.Transactions.GroupBy(t => t.Tag).Select(g => new { g.Key, Avg = g.Average(t => t.Amount) });
            Assert.AreEqual(32.625, total.First().Avg);
            Assert.AreEqual(75, total.Last().Avg);
        }


        //-------------------------------- Two -------------------------------------------------
        [TestMethod]
        public void SumOfMoneySpentPerPayee()
        {
            var ob = new CheckBookVM();
            ob.Fill();

            var PaymentToMoshe = ob.Transactions.Where(p => p.Payee == "Moshe").Sum(a => a.Amount);
            Assert.AreEqual(130, PaymentToMoshe);

            var PaymentToTim = ob.Transactions.Where(p => p.Payee == "Tim").Sum(a => a.Amount);
            Assert.AreEqual(300, PaymentToTim);

            var PaymentToBracha = ob.Transactions.Where(p => p.Payee == "Bracha").Sum(a => a.Amount);
            Assert.AreEqual(131, PaymentToBracha);
        }


        //-------------------------------- Three -------------------------------------------------
        [TestMethod]
        public void SumOfMoneySpentOnFoodPerPayee()
        {
            var ob = new CheckBookVM();
            ob.Fill();

            var PaymentToMoshe = ob.Transactions.Where(p => p.Payee == "Moshe" && p.Tag == "Food").Sum(a => a.Amount);
            Assert.AreEqual(130, PaymentToMoshe);

            var PaymentToTim = ob.Transactions.Where(p => p.Payee == "Tim" && p.Tag == "Food").Sum(a => a.Amount);
            Assert.AreEqual(0, PaymentToTim);

            var PaymentToBracha = ob.Transactions.Where(p => p.Payee == "Bracha" && p.Tag == "Food").Sum(a => a.Amount);
            Assert.AreEqual(131, PaymentToBracha);
        }


        //-------------------------------- Four -------------------------------------------------
        [TestMethod]
        public void TransacsBwnDates()
        {
            var ob = new CheckBookVM();
            ob.Fill();

            var TransacsCount = ob.Transactions.Where(d => d.Date >= DateTime.Parse("2015-4-5") && d.Date < DateTime.Parse("2015-4-8")).Count();
            Assert.AreEqual(6, TransacsCount);
        }


        //-------------------------------- Five -------------------------------------------------
        [TestMethod]
        public void DatesEachAccountUsed()
        {
            var ob = new CheckBookVM();
            ob.Fill();

            var Checkingtotal = ob.Transactions.Select(g => new { g.Date, g.Account}).Count();
            Assert.AreEqual(12, Checkingtotal);
        }


        //-------------------------------- Six -------------------------------------------------
        [TestMethod]
        public void MoreMoneyOnAuto()
        {
            var ob = new CheckBookVM();
            ob.Fill();

            var Checkingtotal = ob.Transactions.Where(p => p.Tag == "Auto" && p.Account == "Checking").Sum(a => a.Amount);
            var Credittotal = ob.Transactions.Where(p => p.Tag == "Auto" && p.Account == "Credit").Sum(a => a.Amount);
            Assert.IsTrue(Checkingtotal == Credittotal);
        }


        //-------------------------------- Seven -------------------------------------------------
        [TestMethod]
        public void TransacsBwnDatesForEachAccount()
        {
            var ob = new CheckBookVM();
            ob.Fill();

            var CheckingChckingTransacsCount = ob.Transactions.Where(d => d.Date >= DateTime.Parse("2015-4-5") && d.Date < DateTime.Parse("2015-4-8") && d.Account == "Checking").Count();
            Assert.AreEqual(3, CheckingChckingTransacsCount);

            var CreditTransacsCount = ob.Transactions.Where(d => d.Date >= DateTime.Parse("2015-4-5") && d.Date < DateTime.Parse("2015-4-8") && d.Account == "Credit").Count();
            Assert.AreEqual(3, CreditTransacsCount);
        }
    }
}