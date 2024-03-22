namespace doan.Models.VnPay
{
    public class VnPayNode
    {
        private string vnp_Amount, vnp_BankCode; 
        private string vnp_BankTranNo, vnp_CardType;
        private string vnp_OrderInfo, vnp_PayDate;
        private string vnp_ResponseCode, vnp_TmnCode;
        private string vnp_TransactionNo, vnp_TransactionStatus;
        private string vnp_TxnRef, vnp_SecureHash;
        private string vnp_HashSecret;

        public VnPayNode() { }

        public string Vnp_Amount 
        { 
            get => this.vnp_Amount; 
            set => this.vnp_Amount = value; 
        }
        public string Vnp_BankCode 
        { 
            get => this.vnp_BankCode; 
            set => this.vnp_BankCode = value; 
        }
        public string Vnp_BankTranNo 
        { 
            get => this.vnp_BankTranNo; 
            set => this.vnp_BankTranNo = value; 
        }
        public string Vnp_CardType 
        { 
            get => this.vnp_CardType; 
            set => this.vnp_CardType = value; 
        }
        public string Vnp_OrderInfo 
        { 
            get => this.vnp_OrderInfo; 
            set => this.vnp_OrderInfo = value; 
        }
        public string Vnp_PayDate 
        { 
            get => this.vnp_PayDate; 
            set => this.vnp_PayDate = value; 
        }
        public string Vnp_ResponseCode
        {
            get => this.vnp_ResponseCode;
            set => this.vnp_ResponseCode = value;
        }
        public string Vnp_TmnCode 
        { 
            get => this.vnp_TmnCode; 
            set => this.vnp_TmnCode = value; 
        }
        public string Vnp_TransactionNo 
        { 
            get => this.vnp_TransactionNo; 
            set => this.vnp_TransactionNo = value; 
        }
        public string Vnp_TransactionStatus 
        { 
            get => this.vnp_TransactionStatus; 
            set => this.vnp_TransactionStatus = value; 
        }
        public string Vnp_TxnRef 
        { 
            get => this.vnp_TxnRef; 
            set => this.vnp_TxnRef = value; 
        }
        public string Vnp_SecureHash 
        { 
            get => this.vnp_SecureHash; 
            set => this.vnp_SecureHash = value; 
        }
        public string Vnp_HashSecret
        {
            get => this.vnp_HashSecret;
        }

        public VnPayLibrary GetResponseData()
        {
            DotNetEnv.Env.Load();
            DotNetEnv.Env.TraversePath().Load();
            this.vnp_HashSecret = DotNetEnv.Env.GetString("vnp_hashSecret");
            VnPayLibrary vnpay = new VnPayLibrary();
            vnpay.AddResponseData("vnp_Amount", this.vnp_Amount);
            vnpay.AddResponseData("vnp_BankCode", this.vnp_BankCode);
            vnpay.AddResponseData("vnp_BankTranNo", this.vnp_BankTranNo);
            vnpay.AddResponseData("vnp_CardType", this.vnp_CardType);
            vnpay.AddResponseData("vnp_OrderInfo", this.vnp_OrderInfo);
            vnpay.AddResponseData("vnp_PayDate", this.vnp_PayDate);
            vnpay.AddResponseData("vnp_ResponseCode", this.vnp_ResponseCode);
            vnpay.AddResponseData("vnp_TmnCode", this.vnp_TmnCode);
            vnpay.AddResponseData("vnp_TransactionNo", this.vnp_TransactionNo);
            vnpay.AddResponseData("vnp_TransactionStatus", this.vnp_TransactionStatus);
            vnpay.AddResponseData("vnp_TxnRef", this.vnp_TxnRef);
            vnpay.AddResponseData("vnp_SecureHash", this.vnp_SecureHash);
            vnpay.AddResponseData("vnp_HashSecret", this.vnp_HashSecret);

            return vnpay;
        }
        public bool IsValidSignature()
        {
            try
            {
                VnPayLibrary vnpay = this.GetResponseData();
                bool checkSignature = vnpay.ValidateSignature(this.vnp_SecureHash, this.vnp_HashSecret);

                return checkSignature; 
            }
            catch
            {
                
            }

            return false;
        }
    }
}
