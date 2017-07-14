using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace com.QH.QPGame.Services.Data
{
    /// <summary>
    /// 类BS_PayOrders。
    /// </summary>
    [Serializable]
	public partial class BS_MoneyRecordItem
    {
		public BS_MoneyRecordItem()
        {
        }

        #region Model

        private int _rowindex;
        private long _orderid;
        private string _orderno;
        private int _userid;
        private int _orderstatus = 0;
        private int _payamount;
        private int _gameamount;
        private string _threeorderno;
        private int? _ordertype;
        private DateTime _ordertime = DateTime.Now;
        private string _submitremark;
        private string _cancelremark;

        public int RowIndex
        {
            get { return _rowindex; }
            set { _rowindex = value; }
        }
    
        /// <summary>
        /// 订单ID
        /// </summary>
        public long OrderId
        {
            set { _orderid = value; }
            get { return _orderid; }
        }

        /// <summary>
        /// 订单号
        /// </summary>
        public string OrderNo
        {
            set { _orderno = value; }
            get { return _orderno; }
        }

        /// <summary>
        /// 用户ID
        /// </summary>
        public int UserId
        {
            set { _userid = value; }
            get { return _userid; }
        }

        /// <summary>
        /// 订单状态(-1,撤销;0,待处理;1,成功)
        /// </summary>
        public int OrderStatus
        {
            set { _orderstatus = value; }
            get { return _orderstatus; }
        }

        /// <summary>
        /// 充值金额
        /// </summary>
        public int PayAmount
        {
            set { _payamount = value; }
            get { return _payamount; }
        }

        /// <summary>
        /// 充值金币
        /// </summary>
        public int GameAmount
        {
            set { _gameamount = value; }
            get { return _gameamount; }
        }

        /// <summary>
        /// 第三方流水号（在线支付预留）
        /// </summary>
        public string ThreeOrderNo
        {
            set { _threeorderno = value; }
            get { return _threeorderno; }
        }

        /// <summary>
        /// 订单类型(玩家上分=200,玩家下分=201,代理上分=202,代理下分=203,发红利=204)
        /// </summary>
        public int? OrderType
        {
            set { _ordertype = value; }
            get { return _ordertype; }
        }

        /// <summary>
        /// 下单时间
        /// </summary>
        public DateTime OrderTime
        {
            set { _ordertime = value; }
            get { return _ordertime; }
        }

        /// <summary>
        /// 下单备注
        /// </summary>
        public string SubmitRemark
        {
            set { _submitremark = value; }
            get { return _submitremark; }
        }

        /// <summary>
        /// 取消订单备注
        /// </summary>
        public string CancelRemark
        {
            set { _cancelremark = value; }
            get { return _cancelremark; }
        }

        public string GenerateOrderNo()
        {
            string OrderNo = DateTime.Now.ToString("yyyyMMddhhmmss");
            Random rnd = new Random();
            string RandomValue = rnd.Next(1000, 9999).ToString();
            return OrderNo + RandomValue;
        }
        #endregion Model
    }
}
