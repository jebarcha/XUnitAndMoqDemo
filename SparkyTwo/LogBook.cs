﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SparkyTwo
{
    public interface ILogBook
    {
        public int LogSeverity { get; set; }
        public string LogType { get; set; }

        void Message(string message);
        bool LogToDB(string message);
        bool LogBalanceAfterWithdrawal(int balanceAfterWithrawal);
        string MessageWithReturnStr(string message);
        bool LogWithOutputResult(string str, out string outputStr);
        bool LogWithRefObj(ref Customer customer);
    }

    public class LogBook : ILogBook
    {
        public int LogSeverity { get; set; }
        public string LogType { get; set; }

        public bool LogBalanceAfterWithdrawal(int balanceAfterWithrawal)
        {
            if (balanceAfterWithrawal >= 0)
            {
                Console.WriteLine("Success");
                return true;
            }
            Console.WriteLine("Failure");
            return false;
        }

        public bool LogToDB(string message)
        {
            Console.WriteLine($"Log to DB. {message}");
            return true;
        }

        public bool LogWithOutputResult(string str, out string outputStr)
        {
            outputStr = $"Hello {str}";
            return true;
        }

        public bool LogWithRefObj(ref Customer customer)
        {
            return true;
        }

        public void Message(string message)
        {
            Console.WriteLine(message);
        }

        public string MessageWithReturnStr(string message)
        {
            Console.WriteLine($"MessageWithReturnStr: {message}");
            return message.ToLower();
        }
    }

    //public class LogFakker : ILogBook
    //{
    //    public void Message(string message)
    //    {
    //    }
    //}
}
