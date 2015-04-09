using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Calculator.CheckBook;
using System.Linq;
using System.Collections.ObjectModel;

namespace CalculatorTests
{
    [TestClass]
    public class LinqAssignment
    {
        //--------------------What is the average transaction amount for each tag?--------------------------
        [TestMethod]
        public void AvgTransactionAmountPerTag()
        {
            var ob = new CheckBookVM();
            ob.Fill();

            var total = ob.Transactions.GroupBy(t => t.Tag).Select(g => new { g.Key, Avg = g.Average(t => t.Amount) });
            Assert.AreEqual(32.625, total.First().Avg);
            Assert.AreEqual(75, total.Last().Avg);
        }


        //------------------------How much did you pay each payee?---------------------------------------------
        [TestMethod]
        public void SumOfMoneyPaidPerPayee()
        {
            var ob = new CheckBookVM();
            ob.Fill();

            var MoneyPaidToMoshe = ob.Transactions.Where(p => p.Payee == "Moshe").Sum(a => a.Amount);
            Assert.AreEqual(130, MoneyPaidToMoshe);

            var MoneyPaidToTim = ob.Transactions.Where(p => p.Payee == "Tim").Sum(a => a.Amount);
            Assert.AreEqual(300, MoneyPaidToTim);

            var MoneyPaidToBracha = ob.Transactions.Where(p => p.Payee == "Bracha").Sum(a => a.Amount);
            Assert.AreEqual(131, MoneyPaidToBracha);
        }


        //-------------------------How much did you pay each payee for food?--------------------------------
        [TestMethod]
        public void SumOfMoneySpentOnFoodToPayee()
        {
            var ob = new CheckBookVM();
            ob.Fill();

            var MoneyPaidToMoshe = ob.Transactions.Where(p => p.Payee == "Moshe" && p.Tag == "Food").Sum(a => a.Amount);
            Assert.AreEqual(130, MoneyPaidToMoshe);

            var MoneyPaidToTim = ob.Transactions.Where(p => p.Payee == "Tim" && p.Tag == "Food").Sum(a => a.Amount);
            Assert.AreEqual(0, MoneyPaidToTim);

            var MoneyPaidToBracha = ob.Transactions.Where(p => p.Payee == "Bracha" && p.Tag == "Food").Sum(a => a.Amount);
            Assert.AreEqual(131, MoneyPaidToBracha);
        }


        //---------------------List the transaction between April 5th and 7th-------------------------------
        [TestMethod]
        public void TransactionsBwnDates()
        {
            var ob = new CheckBookVM();
            ob.Fill();

            var TransactionsCount = ob.Transactions.Where(d => d.Date >= DateTime.Parse("2015-4-5") && d.Date < DateTime.Parse("2015-4-8")).Count();
            Assert.AreEqual(6, TransactionsCount);

            var TransactionsList = ob.Transactions.Where(d => d.Date >= DateTime.Parse("2015-4-5") && d.Date < DateTime.Parse("2015-4-8")).Select(g => new { g.Date, g.Account, g.Payee, g.Amount, g.Tag });
        }


        //---------------------List the dates on which each account was used--------------------------------
        [TestMethod]
        public void DatesOnEachAccountUsed()
        {
            var ob = new CheckBookVM();
            ob.Fill();

            var DatesNdAccounts = ob.Transactions.Select(g => new { g.Date, g.Account }).Count();
            Assert.AreEqual(12, DatesNdAccounts);

            var DatesNAccountsList = ob.Transactions.Select(g => new { g.Date, g.Account }).Count();
        }


        //---------------------Which account was used most (amount of money) on auto expenses?------------------
        [TestMethod]
        public void MoreMoneySpentOnAuto()
        {
            var ob = new CheckBookVM();
            ob.Fill();

            var CheckingAmount = ob.Transactions.Where(p => p.Tag == "Auto" && p.Account == "Checking").Sum(a => a.Amount);
            var CreditAmount = ob.Transactions.Where(p => p.Tag == "Auto" && p.Account == "Credit").Sum(a => a.Amount);
            
            Assert.IsTrue(CheckingAmount == CreditAmount);
        }


        //-----------------List the number of transactions from each account between April 5th and 7th-----------
        [TestMethod]
        public void TransactionsBwnDatesForEachAccount()
        {
            var ob = new CheckBookVM();
            ob.Fill();

            var CreditTransactionsCount = ob.Transactions.Where(d => d.Date >= DateTime.Parse("2015-4-5") && d.Date < DateTime.Parse("2015-4-8") && d.Account == "Credit").Count();
            Assert.AreEqual(3, CreditTransactionsCount);

            var CheckingTransactionsCount = ob.Transactions.Where(d => d.Date >= DateTime.Parse("2015-4-5") && d.Date < DateTime.Parse("2015-4-8") && d.Account == "Checking").Count();
            Assert.AreEqual(3, CheckingTransactionsCount);
        }
    }
}
