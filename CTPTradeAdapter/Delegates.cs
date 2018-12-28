using CTPTradeAdapter.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CTPTradeAdapter
{
    /// <summary>
    /// 委托报单返回事件
    /// </summary>
    /// <param name="order">委托信息</param>
    public delegate void OrderReturnHandler(OrderInfo order);

    /// <summary>
    /// 成交回报事件
    /// </summary>
    /// <param name="trade">成交信息</param>
    public delegate void TradeReturnHandler(TradeInfo trade);

    /// <summary>
    /// 斷線回报事件
    /// </summary>
    /// <param name="nReason">斷線原因</param>
    public delegate void DisconnectedHandler(int nReason);
}
