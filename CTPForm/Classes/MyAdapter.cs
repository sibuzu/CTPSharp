using CTPCore;
using CTPMarketAdapter;
using CTPMarketAdapter.Adapter;
using CTPMarketAdapter.Model;
using CTPTradeAdapter.Adapter;
using CTPTradeAdapter.Model;
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

        public void BuyOrder(string ticker, int quality)
        {
            OrderParameter order = new OrderParameter();
            order.InstrumentID = ticker;
            order.OrderRef = "1";
            order.Direction = DirectionType.Buy;
            order.PriceType = OrderPriceType.LimitPrice;
            order.OpenCloseFlag = OpenCloseFlag.Open;
            order.HedgeFlag = HedgeFlag.Speculation;
            order.Price = 3520;
            order.Quantity = quality;
            order.TimeCondition = TimeConditionType.GFD;
            order.VolumeCondition = VolumeConditionType.AV;
            order.MinVolume = 1;
            order.ContingentCondition = ContingentConditionType.Immediately;
            order.ForceCloseReason = ForceCloseReasonType.NotForceClose;
            order.IsAutoSuspend = 0;
            order.UserForceClose = 0;

            this.InsertOrder(CBInsertOrder, order);
        }

        public void SellOrder(string ticker, int quality)
        {
            OrderParameter order = new OrderParameter();
            order.InstrumentID = ticker;
            order.OrderRef = "1";
            order.Direction = DirectionType.Sell;
            order.PriceType = OrderPriceType.LimitPrice;
            order.OpenCloseFlag = OpenCloseFlag.Open;
            order.HedgeFlag = HedgeFlag.Speculation;
            order.Price = 3500;
            order.Quantity = quality;
            order.TimeCondition = TimeConditionType.GFD;
            order.VolumeCondition = VolumeConditionType.AV;
            order.MinVolume = 1;
            order.ContingentCondition = ContingentConditionType.Immediately;
            order.ForceCloseReason = ForceCloseReasonType.NotForceClose;
            order.IsAutoSuspend = 0;
            order.UserForceClose = 0;

            this.InsertOrder(CBInsertOrder, order);
        }

        public void CBInsertOrder(DataResult<OrderInfo> result)
        {
            OrderInfo orderInfo = new OrderInfo();
            orderInfo = result.Result;
            if (result.IsSuccess)
            {
                Global.AddInfo("下單成功, OrderRef：{0}, OrderSysID：{1}", 
                    orderInfo.OrderRef, orderInfo.OrderSysID);
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
