namespace doan.Models.VnPay
{
    using System;

    public class VnPayReturn
    {
        private string terminalID;
        private string clientTransacID;
        private string serverTransacID;
        private string bankCode;
        private double paymentAmount;
        private int transacStatus;
        private string returnText;
        private VnPayNode node; 

        public static readonly string RESPONSE_CODE = "00";
        public static readonly string TRANSAC_CODE = "00";

        public VnPayReturn() 
        {
            
        }

        public string TerminalID 
        { 
            get => this.terminalID; 
            set => this.terminalID = value; 
        }
        public string ClientTransacID 
        { 
            get => this.clientTransacID;
            set => this.clientTransacID = value; 
        }
        public string ServerTransacID 
        { 
            get => this.serverTransacID; 
            set => this.serverTransacID = value; 
        }
        public double PaymentAmount 
        { 
            get => this.paymentAmount; 
            set => this.paymentAmount = value; 
        }
        public int TransacStatus 
        { 
            get => this.transacStatus; 
            set => this.transacStatus = value; 
        }
        public string ReturnText 
        { 
            get => this.returnText; 
            set => this.returnText = value; 
        }
        public string BankCode 
        { 
            get => this.bankCode; 
            set => this.bankCode = value; 
        }
        public VnPayNode Node
        {
            get => this.node;
            set => this.node = value;
        }

        public bool ProcessResponses()
        {
            VnPayLibrary vnpay = this.node.GetResponseData();

            long orderId = Convert.ToInt64(vnpay.GetResponseData("vnp_TxnRef"));
            long vnpayTranId = Convert.ToInt64(vnpay.GetResponseData("vnp_TransactionNo"));
            string vnp_ResponseCode = vnpay.GetResponseData("vnp_ResponseCode");
            string vnp_TransactionStatus = vnpay.GetResponseData("vnp_TransactionStatus");
            string vnp_SecureHash = vnpay.GetResponseData("vnp_SecureHash");
            string TerminalID = vnpay.GetResponseData("vnp_TmnCode");
            long vnp_Amount = Convert.ToInt64(vnpay.GetResponseData("vnp_Amount")) / 100;
            string bankCode = vnpay.GetResponseData("vnp_BankCode");

            bool checkSignature = vnpay.ValidateSignature(vnp_SecureHash, this.node.Vnp_HashSecret);
            if (checkSignature)
            {
                this.TerminalID = TerminalID;
                this.ClientTransacID = orderId.ToString();
                this.ServerTransacID = vnpayTranId.ToString();
                this.PaymentAmount = vnp_Amount;
                this.BankCode = bankCode;
                if (vnp_ResponseCode == RESPONSE_CODE && vnp_TransactionStatus == TRANSAC_CODE)
                {
                    //Thanh toan thanh cong
                    this.ReturnText = "Cảm ơn quý khách đã giao dịch";

                    return true;
                }
                else
                {
                    //Thanh toan khong thanh cong. Ma loi: vnp_ResponseCode
                    this.ReturnText = "Có lỗi xảy ra trong quá trình xử lý.\n(Mã lỗi: " + vnp_ResponseCode + ")";
                }
            }
            else
            {
                this.ReturnText = "Có lỗi xảy ra trong quá trình xử lý.\n(Mã lỗi: " + vnp_ResponseCode + ")";
            }

            return false;
        }
    }
}
