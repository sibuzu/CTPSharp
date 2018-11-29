using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CTPTradeApi;
using CTPTradeAdapter.Adapter;
using CTPCore;

namespace CTPMain
{
    public class MyTradeAdapter : TradeAdapter
    {
        private string _addr, _broker, _investor, _password;

        public MyTradeAdapter(string addr, string broker, string investor, string password)
        {
            _addr = addr;
            _broker = broker;
            _investor = investor;
            _password = password;
        }

        public void Start()
        {
            this.Connect(CBConect, _broker, _addr);
        }

        private void CBConect(DataResult result)
        {
            if (result.IsSuccess)
            {
                Console.WriteLine("Connect {0} Success", _addr);
                this.UserLogin(CBLogin, _investor, _password);
            }
            else
            {
                Console.WriteLine("Connect {0} Failed", _addr);
            }
        }

        private void CBLogin(DataResult result)
        {
            if (result.IsSuccess)
            {
                Console.WriteLine("Login {0} Success", _investor);
            }
            else
            {
                Console.WriteLine("Login {0} Failed: {1}", _investor, result.Error);
            }
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            string mdFrontAddr = "tcp://180.168.146.187:10031";               // 模擬行情前置地址
            string[] instrumentID = { "TX1812", "zn1812", "cs1901", "CF1812" }; // 行情合約代碼列表，中、上、大、鄭交易所各選一種
            int instrumentNum = 4;                                             // 行情合約訂閱數量

            string tradeFrontAddr = "tcp://180.168.146.187:10030";                         // 模擬經紀商代碼
            string brokerID = "9999";                         // 模擬經紀商代碼
            string investorID = "125288";                         // 投資者賬戶名
            string investorPassword = "jack6819";                     // 投資者密碼

            Console.WriteLine("InvesterID: {0}", investorID);
            Console.WriteLine("BrokerID: {0}", brokerID);

            var tradeAdapter = new MyTradeAdapter(tradeFrontAddr, brokerID, investorID, investorPassword);

            tradeAdapter.Start();

            Console.ReadKey();
        }
    }
}
