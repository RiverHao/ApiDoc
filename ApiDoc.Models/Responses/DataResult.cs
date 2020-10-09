﻿using System; 
namespace ApiDoc.Models.Responses
{ 
    public class DataResult
    {
        public DataResult()
        {
            
        }
        private int dataType = 0;
        public int DataType
        {
            get { return dataType; }
            set { dataType = value; }
        }

        private string exception = "";
        public string Exception
        {
            get { return exception; }
            set { exception = value; }
        }

        private object result;
        public object Result
        {
            get { return result; }
            set { result = value; }
        }

    }
}
