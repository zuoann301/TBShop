using System;
using System.Collections.Generic;
using System.Text;

namespace Ace.Entity
{
    public class SmsResult
    {
        //{"Data":"{\"Message\":\"OK\",\"RequestId\":\"181C30CA-C2C2-414B-A9BE-26A2258FED2D\",\"BizId\":\"102820951786046447^0\",\"Code\":\"OK\"}","Status":100,"Msg":null}


        public int Status { get; set; }

        public string Msg { get; set; }

        public ResultData Data { get; set; }
    }

    public class ResultData
    {
        public string Message { get; set; }

        public string RequestId { get; set; }

        public string BizId { get; set; }

        public string Code { get; set; }


    }
}
