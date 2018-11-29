using CTPCore;
using CTPMarketAdapter;
using CTPMarketAdapter.Adapter;
using CTPMarketAdapter.Model;
using CTPTradeAdapter.Adapter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CTPForm.Classes
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

        internal void CBInfo(DataResult result, string action)
        {
            if (result.IsSuccess)
            {
                Global.AddInfo("{0} success", action);
            }
            else
            {
                Global.GBAddInfo("{0} failed: {1}", action, result.Error);
            }
        }

        public void Start()
        {
            this.Connect(
                result => CBConect(result, "Trade connect " + _addr), 
                _broker, _addr);
        }

        private void CBConect(DataResult result, string action)
        {
            CBInfo(result, action);

            if (result.IsSuccess)
            {
                this.UserLogin(
                    res => CBInfo(res, "Trade login " + _investor), 
                    _investor, _password);
            }
        }
    }

    public class MyMarketAdapter : MarketAdapter
    {
        private string _addr, _broker, _investor, _password;

        public MyMarketAdapter(string addr, string broker, string investor, string password)
        {
            _addr = addr;
            _broker = broker;
            _investor = investor;
            _password = password;
        }

        public void Start()
        {
            this.Connect(
                result => CBConect(result, "Market connect " + _addr), 
                _broker, _addr);
        }

        private void CBConect(DataResult result, string action)
        {
            Global.CBInfo(result, action);

            if (result.IsSuccess)
            {
                this.UserLogin(res => Global.CBInfo(res, "Market login " + _investor),
                    _investor, _password);
            }
        }

        public void Subscribe(string [] tickers)
        {
            this.OnMarketDataChanged += OnData;

            SubscribeMarket(tickers);
        }

        private void OnData(CTPMarketData market)
        {
            Global.AddInfo("Ticker: {0}, Price: {1}, Volume: {2}", 
                market.InstrmentID, market.LastPrice, market.Volume);
        }
    }
}
