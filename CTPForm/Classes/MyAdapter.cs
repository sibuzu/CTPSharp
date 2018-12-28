using CTPCore;
using CTPMarketAdapter;
using CTPMarketAdapter.Adapter;
using CTPMarketAdapter.Model;
using CTPMarketApi;
using CTPTradeAdapter.Adapter;
using CTPTradeAdapter.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static CTPMarketApi.MarketApi;

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

            OnReturnOrder += MyReturnOrder;
            OnReturnTrade += MyReturnTrade;
        }

        private void MyReturnOrder(OrderInfo order)
        {
            Global.GBAddInfo("order: {0}, {1}, {2}, {3}, {4}, {5}",
                order.InstrumentID, order.Direction, order.OrderPrice, order.OrderQuantity,
                order.OrderStatus, order.StatusMessage);
        }

        private void MyReturnTrade(TradeInfo trade)
        {
            Global.GBAddInfo("order: {0}, {1}, {2}, {3}, {4}",
                trade.InstrumentID, trade.Direction, 
                trade.TradePrice, trade.TradeQuantity,
                trade.TradeType);
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

        static int _orderRef = 0;

        public void BuyOrder(string ticker, int quality)
        {
            OrderParameter order = new OrderParameter();
            order.InstrumentID = ticker;
            order.OrderRef = (++_orderRef).ToString();
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
            order.OrderRef = (++_orderRef).ToString();
            order.Direction = DirectionType.Sell;
            order.PriceType = OrderPriceType.LimitPrice;
            order.OpenCloseFlag = OpenCloseFlag.CloseToday;
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

        public void DoQueryPosition()
        {
            this.QueryPosition(CBQueryPosition);
        }

        public void CBQueryPosition(DataListResult<PositionInfo> result)
        {
            if (!result.IsSuccess)
            {
                Global.GBAddInfo("Query position failed: {0}", result.Error);
                return;
            }

            int n = result.Result.Count;
            // Global.AddInfo("Result count：{0}", n);
            for (int i = 0; i < n; ++i)
            {
                var res = result.Result[i];
                Global.AddInfo("{0}: {1}, {2}, {3}, {4}, {5}",
                    i + 1, res.InstrumentID, res.PositionDirection,
                    res.Position, res.PrePosition, res.PositionDate);
            }
        }

        public void DoQueryOrder()
        {
            this.QueryOrder(CBQueryOrder);
        }

        public void CBQueryOrder(DataListResult<OrderInfo> result)
        {
            if (!result.IsSuccess)
            {
                Global.GBAddInfo("Query order failed: {0}", result.Error);
                return;
            }

            int n = result.Result.Count;
            // Global.AddInfo("Result count：{0}", n);
            for (int i = 0; i < n; ++i)
            {
                var res = result.Result[i];
                Global.GBAddInfo("{0}: {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}, {9}",
                    i + 1, res.OrderRef, res.SequenceNo, res.InstrumentID,
                    res.OrderDate, res.OrderTime,
                    res.OrderPrice, res.OrderQuantity,
                    res.OrderStatus, res.StatusMessage);
            }
        }

        public void DoQueryTrade()
        {
            this.QueryTrade(CBQueryTrade);
        }

        public void CBQueryTrade(DataListResult<TradeInfo> result)
        {
            if (!result.IsSuccess)
            {
                Global.GBAddInfo("Query order failed: {0}", result.Error);
                return;
            }

            int n = result.Result.Count;
            // Global.AddInfo("Result count：{0}", n);
            for (int i = 0; i < n; ++i)
            {
                var res = result.Result[i];
                Global.AddInfo("{0}: {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}, {9}",
                    i + 1, res.TradeID, res.SequenceNo, res.InstrumentID,
                    res.TradeDate, res.TradeTime, res.OpenCloseFlag, res.TradeType,
                    res.TradePrice, res.TradeQuantity);
            }
        }


        public void DoUserInfo()
        {
            this.QueryInvestor(CBUserInfo);
        }

        public void CBUserInfo(DataResult<InvestorInfo> result)
        {
            if (!result.IsSuccess)
            {
                Global.GBAddInfo("User info failed: {0}", result.Error);
                return;
            }

            var res = result.Result;
            Global.AddInfo("{0}, {1}, {2}, {3}",
               res.InvestorID, res.InvestorName, res.OpenDate, res.IdentifiedCardNo);
        }

        public void DoAccountInfo()
        {
            this.QueryAccount(CBAccountInfo);
        }

        public void CBAccountInfo(DataResult<AccountInfo> result)
        {
            if (!result.IsSuccess)
            {
                Global.GBAddInfo("User info failed: {0}", result.Error);
                return;
            }

            var res = result.Result;
            Global.AddInfo("{0}, {1}, {2}, {3}, {4}",
               res.InvestorID, res.LoginTime, res.TradingDay, res.Available, res.Balance);
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

            /*
            _api.OnRspSubMarketData += new RspSubMarketData(
                (ref CThostFtdcSpecificInstrumentField pSpecificInstrument,
                 ref CThostFtdcRspInfoField pRspInfo, int nRequestID, byte bIsLast) =>
                {
                    Global.AddInfo("Subscribe {0} successful", pSpecificInstrument.InstrumentID);
                });
            */
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
