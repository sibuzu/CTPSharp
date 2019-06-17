using CTPForm.Classes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using CTPCore;

namespace CTPForm
{
    public partial class FormMain : Form
    {
        public FormMain()
        {
            InitializeComponent();
            Global.frmMain = this;
        }

        internal void AddInfo(string msg, object[] args)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new MethodInvoker(() => AddInfo(msg, args)));
            }
            else
            {
                msg = string.Format(msg, args);
                tbInfo.AppendText(msg + "\r\n");
            }
        }

        MyTradeAdapter _tradeAdapter;
        MyMarketAdapter _marketAdapter;
        // string[] _tickers = { "TF1812", "zn1812", "cs1901", "CF1812" };
        string[] _tickers = { "rb1909", "TF1909", "zn1809", "cs1909", "CF1909" };

        private void btnConnect_Click(object sender, EventArgs e)
        {
            string tradeFrontAddr = "tcp://180.168.146.187:10030";                         // 模擬經紀商代碼
            // string tradeFrontAddr = "tcp://180.168.146.187:10001";                         // 模擬經紀商代碼
            string brokerID = "9999";                         // 模擬經紀商代碼
            string investorID = "125291";                         // 投資者賬戶名
            string investorPassword = "jack6819";                     // 投資者密碼

            _tradeAdapter = new MyTradeAdapter(tradeFrontAddr, brokerID, investorID, investorPassword);

            // string mdFrontAddr = "tcp://180.168.146.187:10040";                         // 模擬經紀商代碼
            string mdFrontAddr = "tcp://180.168.146.187:10010";                         // 模擬經紀商代碼
            _marketAdapter = new MyMarketAdapter(mdFrontAddr, brokerID, investorID, investorPassword);

            _tradeAdapter.Start();
            _marketAdapter.Start();
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            if (_tradeAdapter != null)
            {
                _tradeAdapter.UserLogout(result => Global.CBInfo(result, "Logout"));
            }
        }

        private void FormMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (_tradeAdapter != null)
            {
                _tradeAdapter.Disconnect(null);
            }
            if (_marketAdapter != null)
            {
                _marketAdapter.Disconnect(null);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Global.GBAddInfo("Trading Day: {0}", _marketAdapter.GetTradingDay());
        }

        private void button2_Click(object sender, EventArgs e)
        {
            _marketAdapter.Subscribe(_tickers);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            _tradeAdapter.BuyOrder("ag1812", 1);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            _tradeAdapter.SellOrder("ag1812", 1);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            _tradeAdapter.DoQueryPosition();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            _tradeAdapter.DoQueryOrder();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            _tradeAdapter.DoQueryTrade();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            _tradeAdapter.DoUserInfo();
        }

        private void button9_Click(object sender, EventArgs e)
        {
            _tradeAdapter.DoAccountInfo();
        }

        private void button10_Click(object sender, EventArgs e)
        {
            _tradeAdapter.Disconnect(result => Global.CBInfo(result, "Trade disconnect"));
            _marketAdapter.Disconnect(result => Global.CBInfo(result, "Market disconnect"));
        }
    }
}
